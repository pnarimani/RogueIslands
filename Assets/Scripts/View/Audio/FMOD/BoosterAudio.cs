namespace RogueIslands.View.Audio.FMOD
{
    public class BoosterAudio : IBoosterAudio
    {
        public void BoosterTriggered()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/BoosterTriggered");
        }
    }
}