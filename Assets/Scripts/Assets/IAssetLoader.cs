using UnityEngine;

namespace RogueIslands.Assets
{
    public interface IAssetLoader
    {
        T Load<T>(string key) where T : Object;
        void LoadScene(string sceneName);
    }
}