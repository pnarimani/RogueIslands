using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class SettingsPopup : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _close;
        [SerializeField] private Slider _music, _sfx;
        [SerializeField] private Toggle _enableParticles;
        
        private static float SettingsMusicVolume
        {
            get => PlayerPrefs.GetFloat("SettingsMusicVolume", 1);
            set
            {
                PlayerPrefs.SetFloat("SettingsMusicVolume", value);
                PlayerPrefs.Save();
            }
        }
        
        public static bool SettingsParticles
        {
            get => PlayerPrefs.GetInt("SettingsParticles", 1) == 1;
            set
            {
                PlayerPrefs.SetInt("SettingsParticles", value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        private static float SettingsSfxVolume
        {
            get => PlayerPrefs.GetFloat("SettingsSfxVolume", 1);
            set
            {
                PlayerPrefs.SetFloat("SettingsSfxVolume", value );
                PlayerPrefs.Save();
            }
        }
        
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