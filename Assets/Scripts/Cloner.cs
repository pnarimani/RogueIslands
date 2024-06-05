using Force.DeepCloner;
using UnityEngine.Profiling;

namespace RogueIslands
{
    public static class Cloner
    {
        public static T Clone<T>(this T original)
        {
            Profiler.BeginSample("Clone");
            var deepClone = original.DeepClone();
            Profiler.EndSample();
            return deepClone;
        }
    }
}