using FMODUnity;

namespace RogueIslands.View.Audio.FMOD
{
    public class AudioSettings : IAudioSettings
    {
        public void SetMusicVolume(float volume)
        {
            RuntimeManager.GetBus("bus:/MusicGroup").setVolume(volume);
        }

        public void SetSfxVolume(float volume)
        {
            RuntimeManager.GetBus("bus:/SFXGroup").setVolume(volume);
        }
    }
}