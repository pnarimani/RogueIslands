using System;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace RogueIslands.Assets.Editor
{
    public class ResourcesAssetSaver : IAssetSaver
    {
        private const string ResourcesFolder = "/Resources/";

        public bool CanAssetBeLoadedAtRuntime(Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            return assetPath.LastIndexOf(ResourcesFolder, StringComparison.Ordinal) >= 0;
        }

        public string GetRuntimeLoadKey(Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var index = assetPath.LastIndexOf(ResourcesFolder, StringComparison.Ordinal);
            return index == -1
                ? ""
                : assetPath[(index + ResourcesFolder.Length)..];
        }

        public void Save(string key, Object asset)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            var path = $"Assets/Resources/{key}";
            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(path)))
                AssetDatabase.CreateFolder("Assets/Resources", Path.GetDirectoryName(key));

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}