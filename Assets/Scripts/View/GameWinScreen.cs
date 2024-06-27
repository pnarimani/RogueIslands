using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View
{
    public class GameWinScreen : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _backToMenu;
        
        private void Start()
        {
            _backToMenu.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"));
        }
    }
}