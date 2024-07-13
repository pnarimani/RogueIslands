using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class OptionsPopup : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _resume, _leave, _settings;
        [SerializeField] private TextMeshProUGUI _seed;
        
        private void Start()
        {
            _settings.onClick.AddListener(() => GameManager.Instance.ShowSettingsPopup());
            _resume.onClick.AddListener(() => Destroy(gameObject));
            _leave.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
            
            _seed.text = StaticResolver.Resolve<Seed>().Value;
        }
    }
}