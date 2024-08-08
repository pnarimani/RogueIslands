using RogueIslands.Autofac;

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

        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return TimeMode switch
            {
                Mode.Round => FromStart
                    ? state.Round == Time
                    : state.Round == GameState.RoundsPerAct - Time,
                Mode.Act => FromStart
                    ? state.Act == Time
                    : state.Act == GameState.TotalActs - Time,
                _ => false,
            };
        }
    }
}