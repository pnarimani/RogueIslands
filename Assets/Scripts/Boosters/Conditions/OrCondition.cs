namespace RogueIslands.Boosters.Conditions
{
    public class OrCondition : IGameCondition
    {
        public IGameCondition[] Conditions { get; set; }

        public OrCondition(params IGameCondition[] conditions) => Conditions = conditions;
    }
}