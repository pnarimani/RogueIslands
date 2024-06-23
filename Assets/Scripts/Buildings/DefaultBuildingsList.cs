using System.Collections.Generic;

namespace RogueIslands.Buildings
{
    public class DefaultBuildingsList
    {
        public static List<Building> GetDefaultDeckBuildings()
        {
            var allCards = new List<Building>();

            foreach (var (colorName, color) in ColorTag.All)
            {
                for (var catIndex = 0; catIndex < Category.All.Length; catIndex++)
                {
                    var cat = Category.All[catIndex];
                    for (var sizeIndex = 0; sizeIndex < 3; sizeIndex++)
                    {
                        var legacyColorName = colorName.Replace("Green", "White").Replace("Purple", "Black");
                        var card = new Building()
                        {
                            Color = (colorName, color),
                            Category = cat,
                            Size = (BuildingSize)sizeIndex,
                            Output = 3 + (sizeIndex * 2),
                            Range = 5,
                            PrefabAddress = $"Buildings/{legacyColorName} {(catIndex + sizeIndex * 4 + 1)}",
                            IconAddress = "Buildings/Icons/Sample",
                            RemainingTriggers = 1,
                        };
                        card.Description = new BuildingDescriptionProvider();
                        allCards.Add(card);
                    }
                }
            }

            return allCards;
        }
    }
}