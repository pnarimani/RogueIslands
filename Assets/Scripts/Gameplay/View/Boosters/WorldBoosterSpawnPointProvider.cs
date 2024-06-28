using System.Linq;
using RogueIslands.Gameplay.Boosters;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class WorldBoosterSpawnPointProvider
    {
        private static readonly Collider[] _colliders = new Collider[1000];

        public static bool TryGet(WorldBooster blueprint, ref Random positionRandom, out Vector3 point)
        {
            var all = Object.FindObjectsOfType<WorldBoosterSpawnPoint>()
                .Select(booster => booster.transform.position)
                .ToList();

            point = Vector3.zero;
            if (all.Count == 0)
                return false;

            // can cache
            var prefab = Resources.Load<GameObject>(blueprint.PrefabAddress);
            var instance = Object.Instantiate(prefab);
            var bounds = instance.transform.GetCollisionBounds();
            Object.Destroy(instance);
            
            var mask = LayerMask.GetMask("Building", "WorldBooster");
            for (var i = all.Count - 1; i >= 0; i--)
            {
                if(Physics.OverlapBoxNonAlloc(all[i], bounds.extents, _colliders, Quaternion.identity, mask) > 0)
                    all.RemoveAt(i);
            }

            if (all.Count == 0)
                return false;
            
            point = all[positionRandom.NextInt(all.Count)];
            return true;
        }
    }
}