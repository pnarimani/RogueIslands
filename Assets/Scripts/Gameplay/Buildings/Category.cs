using System;

namespace RogueIslands.Gameplay.Buildings
{
    public readonly struct Category : IEquatable<Category>
    {
        public readonly string Value;
        public Category(string value) => Value = value;
        public bool Equals(Category other) => Value == other.Value;
        public override bool Equals(object obj) => obj is Category other && Equals(other);
        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public override string ToString()
            => Value;

        public static bool operator ==(Category left, Category right) => left.Equals(right);
        public static bool operator !=(Category left, Category right) => !(left == right);

        public static readonly Category City = new("City");
        public static readonly Category Farming = new("Farming");
        public static readonly Category Lumber = new("Lumber");
        public static readonly Category Iron = new("Iron");
        public static readonly Category Statue = new("Statue");

        public static readonly Category[] All = { City, Farming, Lumber, Iron, Statue };
    }
}