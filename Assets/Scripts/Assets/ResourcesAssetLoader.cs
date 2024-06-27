using UnityEngine;

namespace RogueIslands.Assets
{
    public class ResourcesAssetLoader : IAssetLoader
    {
        public T Load<T>(string key) where T : Object
        {
            return UnityEngine.Resources.Load<T>(key);
        }
    }
}