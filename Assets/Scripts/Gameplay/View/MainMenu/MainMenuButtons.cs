using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button _play, _settingsButton, _quit;
        [SerializeField] private SettingsPopup _settings;

        private void Start()
        {
            _play.onClick.AddListener(() => SceneManager.LoadScene(1));
            _settingsButton.onClick.AddListener(() => Instantiate(_settings));
            _quit.onClick.AddListener(Application.Quit);
        }
    }
}