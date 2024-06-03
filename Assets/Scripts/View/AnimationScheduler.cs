using System;
using Cysharp.Threading.Tasks;

namespace RogueIslands.View
{
    public class AnimationScheduler : Singleton<AnimationScheduler>
    {
        private float _delay;

        public static void ResetDelay() => Instance._delay = 0;

        public static float AllocateTime(float time)
        {
            if (time <= 0)
                throw new ArgumentOutOfRangeException(nameof(time));

            var ret = Instance._delay;
            Instance._delay += time;
            return ret;
        }

        public static float GetTotalTime() 
            => Instance._delay;
    }
}