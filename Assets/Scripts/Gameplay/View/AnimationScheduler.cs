using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class AnimationScheduler : Singleton<AnimationScheduler>
    {
        private const float Multiplier = 1;

        private float _delay;
        private float _ensureTime;
        private float _lastAutoResetTime;
        private float _nextAutoResetTime;

        private static void AutoResetIfNeeded()
        {
            if (Instance._nextAutoResetTime < Time.time)
            {
                Instance._delay = 0;
                Instance._ensureTime = 0;

                Instance._lastAutoResetTime = Time.time;
                Instance._nextAutoResetTime = Time.time;
            }
        }

        private static void SetAutoResetTime()
        {
            Instance._nextAutoResetTime = Instance._lastAutoResetTime + GetTotalTime();
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
            var wait = GetAnimationTime();
            AllocateTime(time);
            if (extraTime > 0)
                EnsureExtraTime(extraTime);
            return UniTask.WaitForSeconds(wait);
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

        public static float Scale(float f)
        {
            return f * Multiplier;
        }

        public static void WaitForTotalTime()
        {
            AutoResetIfNeeded();
            
            Instance._delay = GetTotalTime();
            
            SetAutoResetTime();
        }
    }
}