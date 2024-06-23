using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button _play, _discord, _quit;
        
        private void Start()
        {
            _play.onClick.AddListener(() => SceneManager.LoadScene(1));
            _discord.onClick.AddListener(() => Debug.Log("Discord button clicked"));
            _quit.onClick.AddListener(Application.Quit);
        }
    }
}
