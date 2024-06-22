namespace RogueIslands.Boosters
{
    public class TimeCondition : IGameCondition
    {
        public enum Mode
        {
            Day,
            Week,
            Month,
        }

        public Mode TimeMode { get; set; }
        public bool FromStart { get; set; }
        public int Time { get; set; }

        public static TimeCondition LastDay()
        {
            return new TimeCondition
            {
                TimeMode = Mode.Day,
                FromStart = false,
                Time = 1,
            };
        }

        public static TimeCondition FirstDay()
        {
            return new TimeCondition
            {
                TimeMode = Mode.Day,
                FromStart = true,
                Time = 0,
            };
        }
    }
}