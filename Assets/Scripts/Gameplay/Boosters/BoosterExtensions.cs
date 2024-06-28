using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters
{
    public static class BoosterExtensions
    {
        public static T GetEventAction<T>(this IBooster booster) where T : GameAction
        {
            if(booster.EventAction is T action)
            {
                return action;
            }

            if (booster.EventAction is CompositeAction composite)
            {
                foreach (var compositeAction in composite.Actions)
                {
                    if(compositeAction is T subAction)
                    {
                        return subAction;
                    }
                }
            }

            return null;
        }
    }
}