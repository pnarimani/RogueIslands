
using UnityEngine;

namespace RogueIslands.Assets.Editor
{
    public interface IAssetSaver
    {
        bool CanAssetBeLoadedAtRuntime(Object asset);
        string GetRuntimeLoadKey(Object asset);
        void Save(string key, Object asset);
    }
}