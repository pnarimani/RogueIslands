﻿using IngameDebugConsole;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay;

namespace RogueIslands.Debug
{
    public static class SeedCommands
    {
        [ConsoleMethod("seed", "Get the current seed.")]
        public static void GetSeed() => UnityEngine.Debug.Log(StaticResolver.Resolve<Seed>().Value);
    }
}