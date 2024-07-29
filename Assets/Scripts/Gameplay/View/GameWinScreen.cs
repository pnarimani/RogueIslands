using RogueIslands.Assets;
using RogueIslands.Autofac;
using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class GameWinScreen : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _backToMenu;
        
        private void Start()
        {
            _backToMenu.onClick.AddListener(() => StaticResolver.Resolve<IAssetLoader>().LoadScene("MainMenu"));
        }
    }
}