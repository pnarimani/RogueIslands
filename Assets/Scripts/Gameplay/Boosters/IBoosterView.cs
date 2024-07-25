namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterView
    {
        void Remove();
        IBoosterScoreVisualizer GetScoringVisualizer();
        IBoosterScalingVisualizer GetScalingVisualizer();
        IBoosterMoneyVisualizer GetMoneyVisualizer();
        IBoosterResetVisualizer GetResetVisualizer();
        IBoosterRetriggerVisualizer GetRetriggerVisualizer();
    }
}