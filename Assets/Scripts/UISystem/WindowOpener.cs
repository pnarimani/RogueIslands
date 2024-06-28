using System.Collections.Generic;
using RogueIslands.Assets;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.UISystem
{
    public class WindowOpener : IWindowOpener
    {
        private readonly IWindowRegistry _registry;
        private readonly IAssetLoader _assetLoader;
        private readonly Dictionary<UILayer, Transform> _loadedRoots = new();
        public WindowOpener(IWindowRegistry registry, IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
            _registry = registry;
        }

        public T Open<T>(UILayer layer = default)
        {
            if(string.IsNullOrEmpty(layer.Value))
                layer = UILayer.Default;

            var layerTransform = GetLayerTransform(layer);
            var path = _registry.GetKey<T>();
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

        private Transform GetLayerTransform(UILayer layer)
        {
            if (!_loadedRoots.TryGetValue(layer, out var layerTransform))
            {
                var root = _assetLoader.Load<GameObject>($"UISystem/{layer.Value}");
                layerTransform = Object.Instantiate(root).transform;
                layerTransform.name = layer.Value;
                _loadedRoots.Add(layer, layerTransform);
            }

            return layerTransform;
        }
    }
}