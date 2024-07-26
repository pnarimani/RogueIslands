namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterResetVisualizer
    {
        void PlayReset();
        void HideDryRun();
        void ShowDryRunReset();
        void ShowDryRunProbability();
    }
}