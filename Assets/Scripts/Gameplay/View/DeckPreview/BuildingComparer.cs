using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.View.DeckPreview
{
    public class BuildingComparer : IComparer<Building>
    {
        public int Compare(Building x, Building y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            
            var color = string.Compare(x.Color.Tag, y.Color.Tag, StringComparison.Ordinal);
            if (color != 0) return color;
            
            var category = string.Compare(x.Category.Value, y.Category.Value, StringComparison.Ordinal);
            if (category != 0) return category;
            
            return x.Size.CompareTo(y.Size);
        }
    }
}