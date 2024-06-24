using FMODUnity;
using UnityEngine;

namespace RogueIslands.View.Audio.FMOD
{
    public class BuildingCardAudio : IBuildingCardAudio
    {
        public void PlayCardSelected() => RuntimeManager.PlayOneShot("event:/SFX/BuildingCardSelected");

        public void PlayCardDeselected() => RuntimeManager.PlayOneShot("event:/SFX/BuildingCardDeselected");

        public void PlayCardDrawn() => RuntimeManager.PlayOneShot("event:/SFX/BuildingCardDraw");
    }
}