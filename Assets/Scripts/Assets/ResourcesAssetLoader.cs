using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public async UniTask<T> LoadAsync<T>(string key) where T : Object
        {
            var resourceRequest = Resources.LoadAsync<T>(key);
            while (!resourceRequest.isDone)
            {
                await UniTask.Yield();
            }

            return (T)resourceRequest.asset;
        }

        public UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) 
            => SceneManager.LoadSceneAsync(sceneName).ToUniTask();

        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}