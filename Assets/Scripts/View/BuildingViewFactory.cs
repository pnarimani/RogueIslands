using RogueIslands.Buildings;
using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingViewFactory
    {
        public BuildingView Create(Building building)
        {
            var prefab = Resources.Load<BuildingView>(building.PrefabAddress);
            var instance = Object.Instantiate(prefab);
            instance.Initialize(building);
            return instance;
        }
    }
}