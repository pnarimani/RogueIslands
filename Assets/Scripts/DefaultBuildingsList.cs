using System.Collections.Generic;

namespace RogueIslands
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
                        var card = new Building()
                        {
                            Name = $"{colorName}",
                            Color = (colorName, color),
                            Category = cat,
                            Size = (BuildingSize)sizeIndex,
                            Output = 3 + (sizeIndex * 2),
                            Range = 2 + sizeIndex, 
                            PrefabAddress = $"Buildings/{colorName} {(catIndex + sizeIndex * 4 + 1)}",
                        };
                        card.Description = $"Output: {card.Output}\nSize: {card.Size}\nCategory: {card.Category}";
                        allCards.Add(card);
                    }
                }
            }

            return allCards;
        }
    }
}