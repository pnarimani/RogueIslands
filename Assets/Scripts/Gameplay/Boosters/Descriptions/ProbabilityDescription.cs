using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using UnityEngine;

namespace RogueIslands.Gameplay.Boosters.Descriptions
{
    public class ProbabilityDescription : IDescriptionProvider
    {
        public ProbabilityDescription(string format)
        {
            Format = format;
        }

        public string Format { get; }

        public string Get(GameState state, IDescribableItem item)
        {
            if(item is not IBooster booster)
                return string.Empty;
            using var conditions = booster.EventAction.GetAllConditions();
            var probability = conditions.OfType<ProbabilityCondition>().First();
            var text = $"{probability.FavorableOutcome * (state.GetRiggedCount() + 1)} in {probability.TotalOutcomes} chance";
            return string.Format(Format, text);
        }
    }
}