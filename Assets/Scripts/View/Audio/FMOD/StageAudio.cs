using FMODUnity;

namespace RogueIslands.View.Audio.FMOD
{
    public class StageAudio : IStageAudio
    {
        public void PlayRoundWin()
        {
            RuntimeManager.PlayOneShot("event:/SFX/RoundWin");
        }

        public void PlayRoundStart()
        {
            RuntimeManager.PlayOneShot("event:/SFX/RoundStart");
        }

        public void PlayYouWin()
        {
            RuntimeManager.PlayOneShot("event:/SFX/YouWin");
        }

        public void PlayYouLose()
        {
            RuntimeManager.PlayOneShot("event:/SFX/YouLose");
        }

        public void PlayRoundUISpawn()
        {
            RuntimeManager.PlayOneShot("event:/SFX/RoundUISpawn");
        }
    }
}