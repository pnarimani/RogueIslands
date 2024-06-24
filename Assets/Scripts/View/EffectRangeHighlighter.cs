using System.Linq;
using RogueIslands.View.Boosters;
using UnityEngine;

namespace RogueIslands.View
{
    public class EffectRangeHighlighter
    {
        public static void HighlightBuilding(BuildingView center)
        {
            var range = center.Data.Range;
            var clusters = GameManager.Instance.State.Clusters;
            var buildingViews = Object.FindObjectsOfType<BuildingView>()
                .ToDictionary(x => x.Data, x => x);

            foreach (var cluster in clusters)
            {
                var closestBuilding = cluster.Buildings
                    .Select(x => buildingViews[x])
                    .OrderBy(x => Vector3.SqrMagnitude(center.transform.position - x.transform.position))
                    .FirstOrDefault();

                if (closestBuilding == null)
                    continue;

                if (closestBuilding == center)
                {
                    foreach (var otherBuilding in cluster)
                    {
                        var neighbour = buildingViews[otherBuilding];
                        neighbour.ShowRange(false);
                        neighbour.Highlight(true);
                    }

                    continue;
                }

                foreach (var otherBuilding in cluster)
                {
                    buildingViews[otherBuilding].ShowRange(otherBuilding == closestBuilding.Data);
                    var distance = Vector3.Magnitude(closestBuilding.transform.position - center.transform.position);
                    buildingViews[otherBuilding].Highlight(distance < range);
                }
            }

            foreach (var booster in Object.FindObjectsOfType<WorldBoosterView>())
            {
                booster.ShowRange(true);

                var distance = Vector3.Magnitude(center.transform.position - booster.transform.position);
                booster.Highlight(distance < booster.Data.Range);
            }
        }

        public static void LowlightAll()
        {
            foreach (var view in Object.FindObjectsOfType<BuildingView>())
            {
                view.Highlight(false);
                view.ShowRange(false);
            }

            foreach (var booster in Object.FindObjectsOfType<WorldBoosterView>())
            {
                booster.ShowRange(false);
                booster.Highlight(false);
            }
        }

        public static void HighlightWorldBooster(WorldBoosterView booster)
        {
            booster.ShowRange(true);

            foreach (var building in Object.FindObjectsOfType<BuildingView>())
            {
                var distance = Vector3.Magnitude(booster.transform.position - building.transform.position);
                if (distance < booster.Data.Range)
                    building.Highlight(true);
            }
        }
    }
}