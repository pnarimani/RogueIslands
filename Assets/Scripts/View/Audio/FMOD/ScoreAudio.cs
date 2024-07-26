using FMOD.Studio;
using FMODUnity;

namespace RogueIslands.View.Audio.FMOD
{
    public class ScoreAudio : IScoringAudio
    {
        private const string ParameterName = "score_intensity";
        private const string EventName = "event:/SFX/Scoring";

        public void PlayScoreSound(float intensity)
        {
            if(intensity is < 0 or > 1)
                throw new System.ArgumentOutOfRangeException(nameof(intensity), intensity, "Intensity must be between 0 and 1");
            
            var instance = CreateInstance();
            instance.setParameterByName(ParameterName, intensity);
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