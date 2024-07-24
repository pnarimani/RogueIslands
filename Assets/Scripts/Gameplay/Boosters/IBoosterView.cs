namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterView
    {
        void Remove();
        IBoosterScoreVisualizer GetScoringVisualizer();
        void ShowRetriggerEffect();
        IBoosterScalingVisualizer GetScalingVisualizer();
        IBoosterMoneyVisualizer GetMoneyVisualizer();
        void BoosterReset();
    }
}