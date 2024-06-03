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
                foreach (var cat in new[] { Category.Cat1, Category.Cat2, Category.Cat3, Category.Cat4 })
                {
                    for (var i = 0; i < 4; i++)
                    {
                        var card = new Building()
                        {
                            Name = $"{colorName}",
                            Color = (colorName, color),
                            Category = cat,
                            EnergyCost = i + 1,
                            Output = i + 1,
                            PrefabAddress = $"Buildings/{colorName} {i + 1}",
                        };
                        card.Description = $"Energy: {card.EnergyCost}\nOutput: {card.Output}\nCategory: {card.Category}";
                        allCards.Add(card);
                    }
                }
            }

            return allCards;
        }
    }
}