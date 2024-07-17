using UnityEngine;

namespace RogueIslands.View
{
    public class GameSettings
    {
        public static float SettingsMusicVolume
        {
            get => PlayerPrefs.GetFloat("SettingsMusicVolume", 1);
            set
            {
                PlayerPrefs.SetFloat("SettingsMusicVolume", value);
                PlayerPrefs.Save();
            }
        }
        
        public static float AnimationSpeedMultiplier
        {
            get => PlayerPrefs.GetFloat("AnimationSpeedMultiplier", 1);
            set
            {
                PlayerPrefs.SetFloat("AnimationSpeedMultiplier", value);
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
        
        public static float SettingsSfxVolume
        {
            get => PlayerPrefs.GetFloat("SettingsSfxVolume", 1);
            set
            {
                PlayerPrefs.SetFloat("SettingsSfxVolume", value );
                PlayerPrefs.Save();
            }
        }
    }
}