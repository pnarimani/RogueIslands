﻿using System.Collections.Generic;

namespace RogueIslands.Gameplay.Buildings
{
    public class DefaultBuildingsList
    {
        public static List<Building> Get()
        {
            var allCards = new List<Building>();

            foreach (var (colorName, color) in ColorTag.All)
            {
                for (var catIndex = 0; catIndex < Category.All.Length; catIndex++)
                {
                    var cat = Category.All[catIndex];
                    for (var sizeIndex = 0; sizeIndex < 3; sizeIndex++)
                    {
                        var prefabAddress = BuildingPrefabAddressProvider.GetPrefabAddress(colorName, catIndex, sizeIndex);

                        var card = new Building()
                        {
                            Color = (colorName, color),
                            Category = cat,
                            Size = (BuildingSize)sizeIndex,
                            Output = 3 + (sizeIndex * 2),
                            Range = 15,
                            PrefabAddress = prefabAddress,
                            IconAddress = "Buildings/Icons/Sample",
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