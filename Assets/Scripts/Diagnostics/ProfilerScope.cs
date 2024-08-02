using System;
using System.Runtime.CompilerServices;
using UnityEngine.Profiling;

namespace RogueIslands.Diagnostics
{
    public struct ProfilerScope : IDisposable
    {
        public ProfilerScope(string name)
        {
            Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            Profiler.EndSample();
        }

        public static ProfilerScope Begin([CallerMemberName] string name = default) => new(name);
    }
}