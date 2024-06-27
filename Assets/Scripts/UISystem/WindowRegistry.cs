using System;
using System.Collections.Generic;
using RogueIslands.Assets;
using RogueIslands.Serialization;
using UnityEngine;

namespace RogueIslands.UISystem
{
    public class WindowRegistry : IWindowRegistry
    {
        internal const string RegistryPath = "UISystem/WindowRegistry";
        
        private readonly IAssetLoader _assetLoader;
        private readonly IDeserializer _deserializer;
        private Dictionary<Type, string> _paths;

        public WindowRegistry(IAssetLoader assetLoader, IDeserializer deserializer)
        {
            _deserializer = deserializer;
            _assetLoader = assetLoader;
        }
        
        public string GetKey<T>() where T : IWindow
        {
            _paths ??= BuildPaths();
            return _paths[typeof(T)];
        }

        private Dictionary<Type,string> BuildPaths()
        {
            var result = new Dictionary<Type, string>();
            var textAsset = _assetLoader.Load<TextAsset>(RegistryPath);
            var data = _deserializer.Deserialize<Dictionary<string, string>>(textAsset.text);
            foreach (var (key, value) in data)
            {
                var type = Type.GetType(key);
                if (type == null)
                {
                    Debug.LogError($"Could not find type {key}");
                    continue;
                }
                result[type] = value;
            }

            return result;
        }
    }
}