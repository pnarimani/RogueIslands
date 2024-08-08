using RogueIslands.Autofac;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class CopyBoosterAction : GameAction
    {
        public GameAction Cloned { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var cloner = StaticResolver.Resolve<ICloner>();
            
            var index = state.Boosters.IndexOf((BoosterCard)booster);
            if (index >= state.Boosters.Count - 1)
            {
                Cloned = null;
                return false;
            }

            var nextBooster = state.Boosters[index + 1];
            if (nextBooster.EventAction == null)
            {
                Cloned = null;
                return false;
            }
            
            if (state.CurrentEvent is ResetRetriggersEvent)
            {
                Cloned = cloner.Clone(nextBooster.EventAction);
                return false;
            }

            return Cloned != null && Cloned.Execute(booster);
        }
    }
}