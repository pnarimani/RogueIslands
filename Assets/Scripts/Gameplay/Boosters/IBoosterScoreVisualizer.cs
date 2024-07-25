using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterScoreVisualizer
    {
        void MultiplierApplied(double multiplier, double products);
        void ProductApplied(double products);
        void ShowDryRunMultiplier(Dictionary<double, int> multipliersAndCount);
        void ShowDryRunProducts(Dictionary<double, int> productsAndCount);
        void HideDryRun();
    }
}