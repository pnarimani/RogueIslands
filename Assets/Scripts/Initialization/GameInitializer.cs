using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RogueIslands.Assets;
using RogueIslands.Autofac;
using UnityEngine;

namespace RogueIslands.Initialization
{
    public class GameInitializer : MonoBehaviour
    {
        private void Start()
        {
            Initialize().Forget();
        }

        private static async UniTask Initialize()
        {
            foreach (var step in StaticResolver.Resolve<IReadOnlyList<IInitializationStep>>()) await step.Initialize();

            StaticResolver.Resolve<IAssetLoader>().LoadScene("MainMenu");
        }
    }
}