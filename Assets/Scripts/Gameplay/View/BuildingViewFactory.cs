using RogueIslands.Gameplay.Buildings;
using UnityEngine;

namespace RogueIslands.Gameplay.View
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