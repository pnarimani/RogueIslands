using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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

        public UniTask<T> LoadAsync<T>(string key) where T : Object => Addressables.LoadAssetAsync<T>(key).ToUniTask();

        public UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
            => Addressables.LoadSceneAsync(sceneName, mode).ToUniTask();

        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            Addressables.LoadSceneAsync(sceneName, mode);
        }
    }
}