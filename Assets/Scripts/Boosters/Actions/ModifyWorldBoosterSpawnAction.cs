namespace RogueIslands.Boosters.Actions
{
    public class ModifyWorldBoosterSpawnAction : GameAction
    {
        public float SpawnMultiplier { get; set; } = 1;
        public int CountMultiplier { get; set; } = 1;
    }
}