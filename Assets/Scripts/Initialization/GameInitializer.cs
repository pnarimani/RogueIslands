using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RogueIslands.Autofac;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            foreach (var step in StaticResolver.Resolve<IReadOnlyList<IInitializationStep>>())
            {
                await step.Initialize();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}