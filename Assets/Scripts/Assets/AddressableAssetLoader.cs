using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RogueIslands.Assets
{
    internal class AddressableAssetLoader : IAssetLoader
    {
        public T Load<T>(string key) where T : Object
        {
            var loadAsset = Addressables.LoadAssetAsync<T>(key);
            loadAsset.WaitForCompletion();
            return loadAsset.Result;
        }

        public void LoadScene(string sceneName)
        {
            Addressables.LoadSceneAsync(sceneName);
        }
    }
}