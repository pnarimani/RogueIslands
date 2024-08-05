using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueIslands.Assets
{
    public interface IAssetLoader
    {
        T Load<T>(string key) where T : Object;
        UniTask<T> LoadAsync<T>(string key) where T : Object;
        UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
        void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    }
}