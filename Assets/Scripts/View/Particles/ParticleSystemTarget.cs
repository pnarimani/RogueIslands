using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

namespace RogueIslands.Particles
{
    public class ParticleSystemTarget : MonoBehaviour
    {
        [SerializeField] private float _attractionSpeed = 1f;
        [SerializeField] private bool _isUI;
        [SerializeField] private LayerMask _groundMask;

        private readonly List<ManagingParticle> _particleSystems = new();
        private readonly List<JobHandle> _handles = new();
        private readonly List<Vector4> _buffer = new();
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public async UniTask Attract(ParticleSystem particles, Action particleReachedTargetCallback = null)
        {
            _particleSystems.Add(new ManagingParticle(particles, Time.time, particleReachedTargetCallback));

            await UniTask.WaitForSeconds(GetMaxDuration(particles) + 0.1f);

            _particleSystems.RemoveAll(p => p.ParticleSystem == particles);
        }

        private float GetMaxDuration(ParticleSystem particles)
        {
            var maxDuration = float.MinValue;
            _buffer.Clear();
            using (var arr = new NativeArray<ParticleSystem.Particle>(particles.particleCount, Allocator.Temp))
            {
                particles.GetParticles(arr);
                var targetPos = GetTargetPosition();
                for (var i = 0; i < particles.particleCount; i++)
                {
                    var position = arr[i].position;
                    var duration = (targetPos - position).magnitude / _attractionSpeed;
                    maxDuration = Mathf.Max(maxDuration, duration);
                    _buffer.Add(new Vector4(position.x, position.y, position.z, duration));
                }
            }
            particles.SetCustomParticleData(_buffer, ParticleSystemCustomData.Custom1);
            return maxDuration;
        }

        private void Update()
        {
            _handles.Clear();

            _particleSystems.RemoveAll(p => p.ParticleSystem == null);

            if (_particleSystems.Count == 0)
                return;
            
            MoveParticles();

            foreach (var handle in _handles)
                handle.Complete();

            DestroyParticlesAtDestination();
        }

        private void DestroyParticlesAtDestination()
        {
            foreach (var p in _particleSystems)
            {
                _buffer.Clear();
                p.ParticleSystem.GetCustomParticleData(_buffer, ParticleSystemCustomData.Custom1);

                var playbackTime = Time.time - p.AdditionTime;
                for (var i = _buffer.Count - 1; i >= 0; i--)
                {
                    if (_buffer[i].w <= playbackTime)
                    {
                        p.ParticleReachedTargetCallback?.Invoke();

                        var arr = new NativeArray<ParticleSystem.Particle>(p.ParticleSystem.particleCount, Allocator.Temp);
                        p.ParticleSystem.GetParticles(arr);
                        var last = arr[^1];
                        arr[i] = last;
                        p.ParticleSystem.SetParticles(arr, arr.Length - 1);
                        arr.Dispose();
                    }
                }
            }
        }

        private void MoveParticles()
        {
            var targetPos = GetTargetPosition();

            foreach (var p in _particleSystems)
            {
                _handles.Add(new MoveParticlesJob()
                {
                    TargetPosition = targetPos,
                    PlaybackTime = Time.time - p.AdditionTime,
                }.Schedule(p.ParticleSystem, 8));
            }
        }

        private Vector3 GetTargetPosition()
        {
            var targetPos = transform.position;
            if (_isUI)
            {
                var ray = _mainCamera.ScreenPointToRay(transform.position);
                if (Physics.Raycast(ray, out var hit, 1000, _groundMask))
                {
                    targetPos = Vector3.MoveTowards(hit.point, _mainCamera.ScreenToWorldPoint(targetPos), 10);
                }
            }

            return targetPos;
        }

        private struct MoveParticlesJob : IJobParticleSystemParallelFor
        {
            public float PlaybackTime;
            public Vector3 TargetPosition;

            public void Execute(ParticleSystemJobData jobData, int index)
            {
                var positions = jobData.positions;
                var velocities = jobData.velocities;
                var customData = jobData.customData1;

                var particleCustomData = customData[index];
                var initialPosition = new Vector3(particleCustomData.x, particleCustomData.y, particleCustomData.z);
                var duration = particleCustomData.w;

                velocities[index] = Vector3.zero;

                var lerp = EaseManager.Evaluate(Ease.InQuad, null, PlaybackTime, duration, 1.70158f, 0);

                positions[index] = Vector3.Lerp(initialPosition, TargetPosition, lerp);
            }
        }

        public bool IsTrackingParticles() => _particleSystems.Count > 0;

        private readonly struct ManagingParticle
        {
            public readonly ParticleSystem ParticleSystem;
            public readonly float AdditionTime;
            public readonly Action ParticleReachedTargetCallback;

            public ManagingParticle(ParticleSystem particleSystem, float additionTime,
                Action particleReachedTargetCallback)
            {
                ParticleSystem = particleSystem;
                AdditionTime = additionTime;
                ParticleReachedTargetCallback = particleReachedTargetCallback;
            }
        }
    }
}