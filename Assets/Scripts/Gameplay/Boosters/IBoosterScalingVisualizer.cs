namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterScalingVisualizer
    {
        void PlayScaleUp();
        void PlayScaleDown();
        void HideDryRun();
        void ShowDryRunScaleUp(int count);
        void ShowDryRunScaleDown(int count);
    }
}