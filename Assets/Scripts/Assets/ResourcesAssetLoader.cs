using System.IO;
using UnityEngine;

namespace RogueIslands.Assets
{
    internal class ResourcesAssetLoader : IAssetLoader
    {
        public T Load<T>(string key) where T : Object
        {
            var extension = Path.GetExtension(key);
            if (!string.IsNullOrEmpty(extension))
                key = key.Replace(extension, "");
            return Resources.Load<T>(key);
        }
    }
}