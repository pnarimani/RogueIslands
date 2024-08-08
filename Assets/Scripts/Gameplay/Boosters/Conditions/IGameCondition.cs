namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public interface IGameCondition
    {
        bool Evaluate(IBooster booster);
    }
}