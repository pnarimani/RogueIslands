using System.Collections.Generic;
using RogueIslands.Assets;
using RogueIslands.Serialization;
using UnityEngine;

namespace RogueIslands.UISystem
{
    internal class WindowRegistry : IWindowRegistry
    {
        internal const string RegistryPath = "UISystem/WindowRegistry.asset";

        private readonly IAssetLoader _assetLoader;
        private readonly IDeserializer _deserializer;
        private Dictionary<string, string> _paths;

        public WindowRegistry(IAssetLoader assetLoader, IDeserializer deserializer)
        {
            _deserializer = deserializer;
            _assetLoader = assetLoader;
        }

        public string GetKey<T>()
        {
            _paths ??= BuildPaths();
            if (_paths.TryGetValue(typeof(T).Name, out var path))
                return path;
            var fullName = typeof(T).FullName;
            if (fullName != null && _paths.TryGetValue(fullName, out path))
                return path;
            return null;
        }

        private Dictionary<string, string> BuildPaths()
        {
            var textAsset = _assetLoader.Load<TextAsset>(RegistryPath);
            return _deserializer.Deserialize<Dictionary<string, string>>(textAsset.text);
        }
    }
}