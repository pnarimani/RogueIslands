namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class TimeCondition : IGameCondition
    {
        public enum Mode
        {
            Round,
            Act,
        }

        public Mode TimeMode { get; set; }
        public bool FromStart { get; set; }
        public int Time { get; set; }

        public static TimeCondition LastDay()
        {
            return new TimeCondition
            {
                FromStart = false,
                Time = 1,
            };
        }

        public static TimeCondition FirstDay()
        {
            return new TimeCondition
            {
                FromStart = true,
                Time = 0,
            };
        }
    }
}