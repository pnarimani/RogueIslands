using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class AnimationScheduler : Singleton<AnimationScheduler>
    {
        private float _delay;
        private float _ensureTime;
        
        public float AnimationEndTime { get; private set; }
        public float AnimationStartTime { get; private set; }

        private static void AutoResetIfNeeded()
        {
            if (Instance.AnimationEndTime <= Time.time)
            {
                Instance._delay = 0;
                Instance._ensureTime = 0;

                Instance.AnimationStartTime = Time.time;
                Instance.AnimationEndTime = Time.time;
            }
        }

        private static void SetAutoResetTime()
        {
            Instance.AnimationEndTime = Instance.AnimationStartTime + Mathf.Max(Instance._ensureTime, Instance._delay);
        }

        public static void AllocateTime(float time)
        {
            if (time < 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            AutoResetIfNeeded();

            Instance._delay += time;
            
            SetAutoResetTime();
        }

        public static void EnsureExtraTime(float time)
        {
            if (time < 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            AutoResetIfNeeded();
            
            Instance._ensureTime = Mathf.Max(Instance._ensureTime, Instance._delay + time);
            
            SetAutoResetTime();
        }

        public static UniTask ScheduleAndWait(float time, float extraTime = 0)
        {
            var animationTime = GetAnimationTime();
            AllocateTime(time);
            if (extraTime > 0)
                EnsureExtraTime(extraTime);
            var passedTime = Time.time - Instance.AnimationStartTime;
            var duration = animationTime - passedTime;
            if(duration <= 0)
                return UniTask.CompletedTask;
            return UniTask.WaitForSeconds(duration);
        }

        public static float GetAnimationTime()
        {
            AutoResetIfNeeded();
            
            return Instance._delay;
        }

        public static float GetTotalTime()
        {
            AutoResetIfNeeded();
            
            return MathF.Max(Instance._ensureTime, Instance._delay);
        }

        public static void WaitForTotalTime()
        {
            AutoResetIfNeeded();
            
            Instance._delay = GetTotalTime();
            
            SetAutoResetTime();
        }
    }
}