using Cysharp.Threading.Tasks;
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
            var path = _registry.GetAssetKey<T>();
            var prefab = _assetLoader.Load<GameObject>(path);
            return OpenInternal<T>(layer, prefab);
        }

        public async UniTask<T> OpenAsync<T>(UILayer layer = default)
        {
            var path = _registry.GetAssetKey<T>();
            var prefab = await _assetLoader.LoadAsync<GameObject>(path);
            return OpenInternal<T>(layer, prefab);
        }

        private T OpenInternal<T>(UILayer layer, GameObject prefab)
        {
            var layerTransform = _rootProvider.GetRoot(layer);
            var gameObject = Object.Instantiate(prefab, layerTransform, false);
            AttachNestedCanvas(gameObject);
            return gameObject.GetComponent<T>();
        }

        private static void AttachNestedCanvas(GameObject gameObject)
        {
            gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }
}