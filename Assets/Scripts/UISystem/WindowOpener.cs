using RogueIslands.Assets;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.UISystem
{
    internal class WindowOpener : IWindowOpener
    {
        private readonly IWindowRegistry _registry;
        private readonly IAssetLoader _assetLoader;
        private readonly IUIRootProvider _rootProvider;

        public WindowOpener(IWindowRegistry registry, IAssetLoader assetLoader, IUIRootProvider rootProvider)
        {
            _rootProvider = rootProvider;
            _assetLoader = assetLoader;
            _registry = registry;
        }

        public T Open<T>(UILayer layer = default)
        {
            var layerTransform = _rootProvider.GetRoot(layer);
            var path = _registry.GetAssetKey<T>();
            var prefab = _assetLoader.Load<GameObject>(path);
            var gameObject = Object.Instantiate(prefab, layerTransform, false);
            AttachNestedCanvas(gameObject, layerTransform);
            return gameObject.GetComponent<T>();
        }

        private static void AttachNestedCanvas(GameObject gameObject, Transform layerTransform)
        {
            gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }
}