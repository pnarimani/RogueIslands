using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RogueIslands.View.Audio.FMOD
{
    public class ScoreAudio : IScoringAudio
    {
        private const string ParameterName = "score_intensity";
        private const string EventName = "event:/SFX/Scoring";

        public int ClipCount
        {
            get
            {
                var description = RuntimeManager.GetEventDescription(EventName);
                description.getParameterDescriptionByName(ParameterName, out var paramDesc);
                return Mathf.RoundToInt(paramDesc.maximum + 1);
            }
        }

        public void PlayScoreSound(int score)
        {
            var instance = CreateInstance();
            instance.setParameterByName(ParameterName, score);
            instance.start();
            instance.release();
        }

        public void PlayScoringFinished()
        {
            RuntimeManager.PlayOneShot("event:/SFX/ScoringFinished");
        }

        private static EventInstance CreateInstance() => RuntimeManager.CreateInstance(EventName);
    }
}