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
            var all = ObjectRegistry.GetWorldBoosterSpawnPoints();

            point = Vector3.zero;
            if (all.Count == 0)
                return false;

            // can cache
            var prefab = Resources.Load<GameObject>(blueprint.PrefabAddress);
            var instance = Object.Instantiate(prefab);
            var bounds = instance.transform.GetBounds();
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