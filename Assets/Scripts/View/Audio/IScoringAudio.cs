namespace RogueIslands.View.Audio
{
    public interface IScoringAudio
    {
        void PlayScoreSound(float intensity);
        void PlayScoringFinished();
    }
}