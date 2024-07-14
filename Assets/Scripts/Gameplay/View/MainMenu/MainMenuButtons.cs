using RogueIslands.DependencyInjection;
using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button _play, _settingsButton, _quit;

        private void Start()
        {
            _play.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));
            _settingsButton.onClick.AddListener(() => StaticResolver.Resolve<IWindowOpener>().Open<SettingsPopup>());
            _quit.onClick.AddListener(Application.Quit);
        }
    }
}