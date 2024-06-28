using System;

namespace RogueIslands.Gameplay.Buildings
{
    public static class PrefabAddressProvider
    {
        public static string GetPrefabAddress(string colorName, int catIndex, int sizeIndex)
        {
            var legacyColorName = colorName.Replace("Green", "White").Replace("Purple", "Black");
            var prefabAddress = $"Buildings/{legacyColorName} {(catIndex + sizeIndex * 4 + 1)}";
            return prefabAddress;
        }

        public static string GetPrefabAddress(ColorTag color, Category category, BuildingSize size)
            => GetPrefabAddress(color.Tag, Array.IndexOf(Category.All, category), (int)size);

        public static string GetPrefabAddress(Building building)
            => GetPrefabAddress(building.Color, building.Category, building.Size);
    }
}