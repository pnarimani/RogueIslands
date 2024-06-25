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

        public string Prefix => _prefix;

        public string Get(IDescribableItem item)
        {
            if (item is not IBooster booster)
                return string.Empty;
            var scoringAction = booster.GetEventAction<ScoringAction>();

            if (scoringAction.Products != null)
                return $"{Prefix}\n<color=blue>Current: {scoringAction.Products} products.";
            if (scoringAction.PlusMult != null)
                return $"{Prefix}\n<color=red>Current: +{scoringAction.PlusMult} mult.";
            if (scoringAction.XMult != null)
                return $"{Prefix}\n<color=red>Current: x{scoringAction.XMult:F2} mult.";
            return Prefix;
        }
    }
}