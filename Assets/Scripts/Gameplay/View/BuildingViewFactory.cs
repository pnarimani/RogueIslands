using RogueIslands.Assets;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingViewFactory
    {
        public BuildingView Create(Building building)
        {
            var prefab = StaticResolver.Resolve<IAssetLoader>().Load<GameObject>(building.PrefabAddress);
            var instance = Object.Instantiate(prefab, building.Position, building.Rotation);
            var view = instance.GetComponent<BuildingView>();
            view.Initialize(building);
            return view;
        }
    }
}