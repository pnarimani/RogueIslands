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
            GameUI.Instance.ShowScoringPanel(false);
            _backToMenu.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"));
        }
    }
}