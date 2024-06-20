using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public class BoosterCard : IPurchasableItem
    {
        public BoosterInstanceId Id { get; set; }
        public string Name { get; set; }
        public IDescriptionProvider Description { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public GameAction BuyAction { get; set; }
        public GameAction SellAction { get; set; }
        public GameAction EventAction { get; set; }
        public IReadOnlyList<ConditionEvaluator> EvaluationOverrides { get; set; }
        
        public T GetEventAction<T>() where T : GameAction
        {
            if(EventAction is T action)
            {
                return action;
            }

            if (EventAction is CompositeAction composite)
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