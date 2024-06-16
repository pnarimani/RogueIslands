using System;

namespace RogueIslands.View
{
    public class AnimationScheduler : SingletonMonoBehaviour<AnimationScheduler>
    {
        private float _delay;
        private float _ensureTime;

        public static void ResetTime()
        {
            Instance._delay = 0;
            Instance._ensureTime = 1;
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

            Instance._ensureTime = Instance._delay + time;
            Instance._ensureTime = Instance._delay + time;
        }

        public static float GetAnimationTime()
            => Instance._delay;

        public static float GetExtraTime()
            => MathF.Max(Instance._ensureTime, Instance._delay);
    }
}