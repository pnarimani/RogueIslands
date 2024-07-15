using System.Collections.Generic;
using RogueIslands.Assets;
using UnityEngine;

namespace RogueIslands.UISystem
{
    public class UIRootProvider : IUIRootProvider
    {
        private readonly Dictionary<UILayer, Transform> _loadedRoots = new();
        private readonly IAssetLoader _assetLoader;

        public UIRootProvider(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }
        
        public Transform GetRoot(UILayer layer)
        {
            if(string.IsNullOrEmpty(layer.Value))
                layer = UILayer.Default;
            
            if (!_loadedRoots.TryGetValue(layer, out var layerTransform) || layerTransform == null)
            {
                var root = _assetLoader.Load<GameObject>($"UISystem/{layer.Value}");
                layerTransform = Object.Instantiate(root).transform;
                layerTransform.name = layer.Value;
                _loadedRoots[layer] = layerTransform;
            }

            return layerTransform;
        }
    }
}