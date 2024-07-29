
using UnityEngine;

namespace RogueIslands.Assets.Editor
{
    public interface IAssetSaver
    {
        string GetRuntimeLoadKey(Object asset);
        void Save(string key, Object asset);
    }
}