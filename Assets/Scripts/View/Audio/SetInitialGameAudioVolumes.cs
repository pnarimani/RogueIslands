using Cysharp.Threading.Tasks;
using RogueIslands.Initialization;

namespace RogueIslands.View.Audio
{
    internal class SetInitialGameAudioVolumes : IInitializationStep
    {
        private readonly IAudioSettings _audioSettings;

        public SetInitialGameAudioVolumes(IAudioSettings audioSettings)
        {
            _audioSettings = audioSettings;
        }

        public UniTask Initialize()
        {
            _audioSettings.SetMusicVolume(GameSettings.SettingsMusicVolume);
            _audioSettings.SetSfxVolume(GameSettings.SettingsSfxVolume);
            return UniTask.CompletedTask;
        }
    }
}