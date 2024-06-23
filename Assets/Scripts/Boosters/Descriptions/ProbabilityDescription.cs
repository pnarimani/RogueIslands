using System.Linq;

namespace RogueIslands.Boosters.Descriptions
{
    public class ProbabilityDescription : IDescriptionProvider
    {
        private readonly string _format;

        public ProbabilityDescription(string format)
        {
            _format = format;
        }
        
        public string Get(IDescribableItem item)
        {
            if(item is not IBooster booster)
                return string.Empty;
            var probability = booster.EventAction.GetAllConditions().OfType<ProbabilityCondition>().First();
            var text = $"{probability.FavorableOutcome} in {probability.TotalOutcomes} chance";
            return string.Format(_format, text);
        }
    }
}