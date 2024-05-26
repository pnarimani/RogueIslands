using System;
using UnityEngine;

namespace RogueIslands
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
        
        public static readonly ColorTag White = new("White", Color.white);
        public static readonly ColorTag Black = new("Black", Color.black);
        public static readonly ColorTag Red = new("Red", Color.red);
        public static readonly ColorTag Blue = new("Blue", Color.blue);
    
        public static readonly ColorTag[] All = { White, Black, Red, Blue };
    }
}