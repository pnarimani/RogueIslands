using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Lose
{
    public class LoseScreen : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _back;
        
        private void Start()
        {
            _back.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        }
    }
}
