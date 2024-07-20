namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterScoreVisualizer
    {
        void MultiplierApplied(double multiplier, double products);
        void ProductApplied(double products);
    }
}