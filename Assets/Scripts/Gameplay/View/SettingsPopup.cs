using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using UnityEngine;
using UnityEngine.UI;
using static RogueIslands.View.GameSettings;

namespace RogueIslands.Gameplay.View
{
    public class SettingsPopup : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _close;
        [SerializeField] private Slider _music, _sfx;
        [SerializeField] private Toggle _enableParticles;
        

        
        private void Awake()
        {
            _close.onClick.AddListener(() => Destroy(gameObject));
            
            _music.value = SettingsMusicVolume;
            _sfx.value = SettingsSfxVolume;
            _enableParticles.isOn = SettingsParticles;
            
            _music.onValueChanged.AddListener(value =>
            {
                SettingsMusicVolume = value;
                StaticResolver.Resolve<IAudioSettings>().SetMusicVolume(value);
            });
            
            _sfx.onValueChanged.AddListener(value =>
            {
                SettingsSfxVolume = value;
                StaticResolver.Resolve<IAudioSettings>().SetSfxVolume(value);
            });
            
            _enableParticles.onValueChanged.AddListener(value =>
            {
                SettingsParticles = value;
            });
        }
    }
}