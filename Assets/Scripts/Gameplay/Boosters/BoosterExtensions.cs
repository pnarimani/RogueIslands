using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters
{
    public static class BoosterExtensions
    {
        public static T GetEventAction<T>(this IBooster booster) where T : GameAction
        {
            return GetEvent<T>(booster.EventAction);
        }

        private static T GetEvent<T>(GameAction boosterEventAction) where T : GameAction
        {
            if(boosterEventAction is T action)
            {
                return action;
            }

            if (boosterEventAction is CompositeAction composite)
            {
                foreach (var sub in composite.Actions)
                {
                    if (GetEvent<T>(sub) is { } result)
                        return result;
                }
            }

            return null;
        }
    }
}