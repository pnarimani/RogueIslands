using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class DistinctBuildings : ISource<Building>
    {
        public ISource<Building> Source { get; set; }

        public IEnumerable<Building> Get(IBooster booster)
        {
            return Source.Get(booster).Distinct(new Comparer());
        }
        
        private struct Comparer : IEqualityComparer<Building>
        {
            public bool Equals(Building x, Building y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Category.Equals(y.Category) && x.Color.Equals(y.Color) && x.Size == y.Size;
            }

            public int GetHashCode(Building obj)
            {
                return HashCode.Combine(obj.Category, obj.Color, (int)obj.Size);
            }
        }
    }
}