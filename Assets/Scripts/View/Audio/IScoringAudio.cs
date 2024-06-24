namespace RogueIslands.View.Audio
{
    public interface IScoringAudio
    {
        int ClipCount { get; }
        void PlayScoreSound(int score);
        void PlayScoringFinished();
    }
}