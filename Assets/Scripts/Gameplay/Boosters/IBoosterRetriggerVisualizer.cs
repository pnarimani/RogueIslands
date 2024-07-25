namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterRetriggerVisualizer
    {
        void PlayRetrigger();
        void ShowDryRunRetriggers(int count);
        void HideDryRun();
    }
}