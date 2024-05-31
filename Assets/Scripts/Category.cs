using System;

namespace RogueIslands
{
    public readonly struct Category : IEquatable<Category>
    {
        public readonly string Value;
        public Category(string value) => Value = value;
        public bool Equals(Category other) => Value == other.Value;
        public override bool Equals(object obj) => obj is Category other && Equals(other);
        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);
        public override string ToString() => Value;

        public static bool operator ==(Category left, Category right) => left.Equals(right);
        public static bool operator !=(Category left, Category right) => !(left == right);

        public static readonly Category Cat1 = new(nameof(Cat1));
        public static readonly Category Cat2 = new(nameof(Cat2));
        public static readonly Category Cat3 = new(nameof(Cat3));
        public static readonly Category Cat4 = new(nameof(Cat4));
    }
}