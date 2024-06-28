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
            => $"<color=#005500><b>{Value}</b></color>";

        public static bool operator ==(Category left, Category right) => left.Equals(right);
        public static bool operator !=(Category left, Category right) => !(left == right);

        public static readonly Category Cat1 = new("City");
        public static readonly Category Cat2 = new("Farming");
        public static readonly Category Cat3 = new("Lumber");
        public static readonly Category Cat4 = new("Iron");
        public static readonly Category Cat5 = new("Statue");

        public static readonly Category[] All = { Cat1, Cat2, Cat3, Cat4, Cat5 };
    }
}