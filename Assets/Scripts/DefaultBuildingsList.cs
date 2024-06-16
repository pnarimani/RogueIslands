using System.Collections.Generic;

namespace RogueIslands
{
    public class DefaultBuildingsList
    {
        public static List<Building> Get()
        {
            var allCards = new List<Building>();
            
            foreach (var (colorName, color) in ColorTag.All)
            {
                foreach (var cat in Category.All)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var card = new Building()
                        {
                            Name = $"{colorName}",
                            Color = (colorName, color),
                            Category = cat,
                            Size = (BuildingSize) i,
                            Output = 3 + (i * 2),
                            PrefabAddress = $"Buildings/{colorName} {i + 1}",
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