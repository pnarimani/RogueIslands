using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Descriptions
{
    public class ScalingBoosterDescription : IDescriptionProvider
    {
        private readonly string _prefix;
        
        public ScalingBoosterDescription(string prefix)
        {
            _prefix = prefix;
        }

        public string Get(IDescribableItem item)
        {
            if (item is not IBooster booster)
                return string.Empty;
            var scoringAction = booster.GetEventAction<ScoringAction>();

            if (scoringAction.Products != null)
                return $"{_prefix}\n<color=blue>Current: {scoringAction.Products} products.";
            if (scoringAction.PlusMult != null)
                return $"{_prefix}\n<color=red>Current: +{scoringAction.PlusMult} mult.";
            if (scoringAction.XMult != null)
                return $"{_prefix}\n<color=red>Current: x{scoringAction.XMult:F2} mult.";
            return _prefix;
        }
    }
}