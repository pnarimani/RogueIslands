using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands
{
    public static class WorldBoosterExtensions
    {
        public static void SpawnWorldBoosters(this GameState state, IGameView view, IReadOnlyList<Vector3> spawnPoints)
        {
            foreach (var point in spawnPoints)
            {
                var index = state.WorldBoosterRandom.NextInt(state.AvailableWorldBoosters.Count);
                
                var booster = state.AvailableWorldBoosters[index].Clone();
                booster.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());
                booster.Position = point;
                booster.Rotation = Quaternion.identity;
                
                state.WorldBoosters.Add(booster);
                view.AddBooster(booster);
            }
        }
    }
}