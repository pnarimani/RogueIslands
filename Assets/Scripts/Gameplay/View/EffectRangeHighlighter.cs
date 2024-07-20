using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class EffectRangeHighlighter
    {
        public static void HighlightBuilding(BuildingView center)
        {
            var range = center.Data.Range;

            center.Highlight(true);
            center.ShowRange(true);

            foreach (var building in ObjectRegistry.GetBuildings())
            {
                if (building == center)
                {
                    continue;
                }
                
                var distance = Vector3.Magnitude(center.transform.position - building.transform.position);
                building.Highlight(distance <= range);

                if (distance <= range)
                {
                    var bonus = StaticResolver.Resolve<ScoringController>().GetScoreBonus(center.Data, building.Data);
                    building.ShowBonus(bonus);
                }
                else
                {
                    building.HideBonus();
                }
            }
        }

        public static void LowlightAll()
        {
            foreach (var view in ObjectRegistry.GetBuildings())
            {
                view.Highlight(false);
                view.ShowRange(false);
                view.HideBonus();
            }
        }

        public static void HighlightWorldBooster(WorldBoosterView booster)
        {
            booster.ShowRange(true);

            foreach (var building in ObjectRegistry.GetBuildings())
            {
                var distance = Vector3.Magnitude(booster.transform.position - building.transform.position);
                if (distance < booster.Data.Range)
                    building.Highlight(true);
            }
        }
    }
}