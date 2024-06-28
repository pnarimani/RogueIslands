using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Descriptions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding.Actions;

namespace RogueIslands.Gameplay.DeckBuilding
{
    public static class ConsumableList
    {
        public static List<Consumable> Get()
        {
            return new List<Consumable>()
            {
                new()
                {
                    Name = "Demolition",
                    Description = new LiteralDescription("Destroy up to 2 buildings"),
                    BuyPrice = 2,
                    SellPrice = 1,
                    Action = new DemolitionDeckAction()
                    {
                        MinCardsRequired = 0,
                        MaxCardsRequired = 2,
                    },
                },
                new()
                {
                    Name = "Mirror",
                    Description = new LiteralDescription("Select 2 buildings. Turns the left building into the right building"),
                    BuyPrice = 2,
                    SellPrice = 1,
                    Action = new MirrorDeckAction()
                    {
                        MinCardsRequired = 2,
                        MaxCardsRequired = 2,
                    },
                },
                CreateCategoryChangeConsumable(Category.Cat1),
                CreateCategoryChangeConsumable(Category.Cat2),
                CreateCategoryChangeConsumable(Category.Cat3),
                CreateCategoryChangeConsumable(Category.Cat4),
                CreateCategoryChangeConsumable(Category.Cat5),
                CreateColorConsumable(ColorTag.Blue),
                CreateColorConsumable(ColorTag.Red),
                CreateColorConsumable(ColorTag.Purple),
                CreateColorConsumable(ColorTag.Green),
                new()
                {
                    Name = "Expansion",
                    Description = new LiteralDescription("Increases the size of up to 3 buildings"),
                    BuyPrice = 2,
                    SellPrice = 1,
                    Action = new SizeChangeDeckAction()
                    {
                        MinCardsRequired = 0,
                        MaxCardsRequired = 3,
                    },
                }
            };
        }

        private static Consumable CreateColorConsumable(ColorTag color)
        {
            return new Consumable()
            {
                Name = color.Tag,
                Description = new LiteralDescription($"Convert up to 3 buildings to {color}"),
                BuyPrice = 2,
                SellPrice = 1,
                Action = new ColorChangeDeckAction()
                {
                    MinCardsRequired = 0,
                    MaxCardsRequired = 3,
                    TargetColor = color,
                },
            };
        }

        private static Consumable CreateCategoryChangeConsumable(Category category)
        {
            return new()
            {
                Name = category.Value,
                Description = new LiteralDescription($"Convert up to 3 buildings to {category}"),
                BuyPrice = 2,
                SellPrice = 1,
                Action = new CategoryChangeDeckAction()
                {
                    MinCardsRequired = 0,
                    MaxCardsRequired = 3,
                    TargetCategory = category,
                },
            };
        }
    }
}