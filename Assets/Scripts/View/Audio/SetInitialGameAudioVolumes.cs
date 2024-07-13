namespace RogueIslands.View.Audio
{
    internal class SetInitialGameAudioVolumes
    {
        public SetInitialGameAudioVolumes(IAudioSettings audioSettings)
        {
            audioSettings.SetMusicVolume(GameSettings.SettingsMusicVolume);
            audioSettings.SetSfxVolume(GameSettings.SettingsSfxVolume);
        }
    }
}