using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Descriptions
{
    public class ProbabilityDescription : IDescriptionProvider
    {
        public ProbabilityDescription(string format)
        {
            Format = format;
        }

        public string Format { get; }

        public string Get(IDescribableItem item)
        {
            if(item is not IBooster booster)
                return string.Empty;
            var probability = booster.EventAction.GetAllConditions().OfType<ProbabilityCondition>().First();
            var text = $"{probability.FavorableOutcome} in {probability.TotalOutcomes} chance";
            return string.Format(Format, text);
        }
    }
}