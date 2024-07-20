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