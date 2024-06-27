using RogueIslands.Assets;
using UnityEngine;

namespace RogueIslands.UISystem
{
    internal class WindowOpener : IWindowOpener
    {
        private readonly IWindowRegistry _registry;
        private readonly IAssetLoader _assetLoader;
        
        public WindowOpener(IWindowRegistry registry, IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
            _registry = registry;
        }

        public T Open<T>(UILayer layer = default) where T : IWindow
        {
            if(string.IsNullOrEmpty(layer.Value))
                layer = UILayer.Default;
            
            var path = _registry.GetKey<T>();
            var prefab = _assetLoader.Load<GameObject>(path);
            var gameObject = Object.Instantiate(prefab);
            return gameObject.GetComponent<T>();
        }
    }
}