namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ModifyWorldBoosterSpawnAction : GameAction
    {
        public float FactorMultiplier { get; set; } = 1;
        public int CountMultiplier { get; set; } = 1;
    }
}