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

        public static void ResetTime()
        {
            Instance._delay = 0;
            Instance._ensureTime = 0;
        }

        public static void AllocateTime(float time)
        {
            if (time < 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            Instance._delay += time;
        }

        public static void EnsureExtraTime(float time)
        {
            if (time < 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            Instance._ensureTime = Mathf.Max(Instance._ensureTime, Instance._delay + time);
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
            => Instance._delay;

        public static float GetTotalTime()
            => MathF.Max(Instance._ensureTime, Instance._delay);

        public static float Scale(float f)
        {
            return f * Multiplier;
        }

        public static void WaitForTotalTime() 
            => Instance._delay = GetTotalTime();
    }
}