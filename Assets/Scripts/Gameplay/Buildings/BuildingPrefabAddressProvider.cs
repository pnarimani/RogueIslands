using System;

namespace RogueIslands.Gameplay.Buildings
{
    public static class BuildingPrefabAddressProvider
    {
        public static string GetPrefabAddress(string colorName, int catIndex, int sizeIndex)
        {
            var legacyColorName = colorName.Replace("Green", "White").Replace("Purple", "Black");
            var prefabAddress = $"{legacyColorName} Cat{catIndex + 1} {sizeIndex + 1}";
            return prefabAddress;
        }

        public static string GetPrefabAddress(ColorTag color, Category category, BuildingSize size)
            => GetPrefabAddress(color.Tag, Array.IndexOf(Category.All, category), (int)size);

        public static string GetPrefabAddress(Building building)
            => GetPrefabAddress(building.Color, building.Category, building.Size);
    }
}