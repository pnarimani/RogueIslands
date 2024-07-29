using System.IO;
using UnityEditor;
using UnityEngine;

namespace RogueIslands.Assets.Editor
{
    internal class AddressableSaver : IAssetSaver
    {
        private const string ContentPath = "Assets/Content/";

        public string GetRuntimeLoadKey(Object asset)
        {
            var entry = asset.GetAddressableAssetEntry();
            if (entry != null) return entry.address;

            var path = AssetDatabase.GetAssetPath(asset);
            var key = path[ContentPath.Length..];
            Save(key, asset);
            return key;
        }

        public void Save(string key, Object asset)
        {
            var path = Path.Combine(ContentPath, key);
            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(path)))
                AssetDatabase.CreateFolder(ContentPath, Path.GetDirectoryName(key));

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            asset.SetAddressableID(key);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}