using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Rand;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class WorldBoosterSpawnPointProvider
    {
        private static readonly Collider[] _colliders = new Collider[1000];

        public static bool TryGet(WorldBooster blueprint, RandomForAct positionRandom, out Vector3 point)
        {
            point = Vector3.zero;
            return false;
        }

    }
}