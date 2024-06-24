using FMODUnity;

namespace RogueIslands.View.Audio.FMOD
{
    public class BuildingAudio : IBuildingAudio
    {
        public void PlayBuildingPlaced()
        {
            RuntimeManager.PlayOneShot("event:/SFX/BuildingPlaced");
        }

        public void PlayBuildingTriggered()
        {
            RuntimeManager.PlayOneShot("event:/SFX/BuildingTriggered");
        }
    }
}