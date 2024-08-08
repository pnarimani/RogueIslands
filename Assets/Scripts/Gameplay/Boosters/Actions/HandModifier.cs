using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class HandModifier : GameAction
    { 
        public int? Change { get; set;}
        public int? SetHandSize { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            
            if (Change is { } change)
                state.HandSize += change;
            
            if (SetHandSize is { } size)
                state.HandSize = size;
            
            return true;
        }
    }
}