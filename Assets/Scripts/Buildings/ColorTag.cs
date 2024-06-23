using System;
using UnityEngine;

namespace RogueIslands.Buildings
{
    public readonly struct ColorTag : IEquatable<ColorTag>
    {
        public readonly string Tag;
        public readonly Color Color;

        public ColorTag(string tag, Color color)
        {
            Tag = tag;
            Color = color;
        }

        public void Deconstruct(out string tag, out Color color)
        {
            tag = Tag;
            color = Color;
        }

        public override string ToString() => Tag;
        public static implicit operator ColorTag((string tag, Color color) tuple) => new(tuple.tag, tuple.color);
        public static bool operator ==(ColorTag left, ColorTag right) => left.Equals(right);
        public static bool operator !=(ColorTag left, ColorTag right) => !(left == right);
        public bool Equals(ColorTag other) => Tag == other.Tag;
        public override bool Equals(object obj) => obj is ColorTag other && Equals(other);
        public override int GetHashCode() => (Tag != null ? Tag.GetHashCode() : 0);

        public static readonly ColorTag Green = new("Green", new Color(0.76f, 1f, 0.65f));
        public static readonly ColorTag Purple = new("Purple", new Color(0.73f, 0.46f, 1f));
        public static readonly ColorTag Red = new("Red", new Color(1f, 0.42f, 0.34f));
        public static readonly ColorTag Blue = new("Blue", new Color(0.46f, 0.74f, 1f));

        public static readonly ColorTag[] All = { Green, Purple, Red, Blue };
    }
}