using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RogueIslands.Assets;
using RogueIslands.Autofac;
using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueIslands.Gameplay.View
{
    public class GameplayStartupWindowOpener : MonoBehaviour
    {
        public void Start()
        {
            Init().Forget();
        }

        private static async UniTask Init()
        {
            await StaticResolver.Resolve<IAssetLoader>().LoadSceneAsync("Island_1", LoadSceneMode.Additive);

            CameraMovement.Instance.CalculateBounds();
            
            await UniTask.DelayFrame(1);

            var windowOpener = StaticResolver.Resolve<IWindowOpener>();
            await windowOpener.OpenAsync<GameUI>();

            StaticResolver.Resolve<RoundController>().StartRound();
            GameUI.Instance.RefreshDeckText();
        }
    }
}