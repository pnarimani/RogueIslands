using System;
using System.Runtime.CompilerServices;
using UnityEngine.Profiling;

namespace RogueIslands.Diagnostics
{
    public struct ProfilerBlock : IDisposable
    {
        public ProfilerBlock(string name)
        {
            Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            Profiler.EndSample();
        }

        public static ProfilerBlock Begin([CallerMemberName] string name = default) => new(name);
    }
}