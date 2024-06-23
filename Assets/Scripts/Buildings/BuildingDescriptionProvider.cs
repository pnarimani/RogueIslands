﻿namespace RogueIslands.Buildings
{
    public class BuildingDescriptionProvider : IDescriptionProvider
    {
        public string Get(IDescribableItem item)
        {
            if (item is not Building building)
                return string.Empty;
            return $"Output: {building.Output + building.OutputUpgrade}\nSize: {building.Size}\nCategory: {building.Category}";
        }
    }
}