using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Descriptions
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
            var scalingAction = booster.GetEventAction<BoosterScalingAction>();
            
            var prefix = Prefix;
            
            if(scalingAction.OneTime && scalingAction.Delay != null && scalingAction.Progress < scalingAction.Delay)
                prefix = $"{prefix}\n({scalingAction.Progress}/{scalingAction.Delay})";
            
            if (scoringAction.Products != null)
                return $"{prefix}\n<color=blue>Current: {scoringAction.Products:0.##} products.";
            if (scoringAction.PlusMult != null)
                return $"{prefix}\n<color=red>Current: +{scoringAction.PlusMult:0.##} mult.";
            if (scoringAction.XMult != null)
                return $"{prefix}\n<color=red>Current: x{scoringAction.XMult:0.##} mult.";
            
            return prefix;
        }
    }
}