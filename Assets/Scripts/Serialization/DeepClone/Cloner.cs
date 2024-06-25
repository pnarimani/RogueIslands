using Force.DeepCloner;
using UnityEngine.Profiling;

namespace RogueIslands.Serialization.DeepClone
{
    public class Cloner : ICloner
    {
        public T Clone<T>(T original)
        {
            Profiler.BeginSample("Clone");
            var deepClone = original.DeepClone();
            Profiler.EndSample();
            return deepClone;
        }
    }
}