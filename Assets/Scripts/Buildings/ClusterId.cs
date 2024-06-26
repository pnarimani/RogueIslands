using System;

namespace RogueIslands.Buildings
{
    public readonly struct ClusterId : IEquatable<ClusterId>
    {
        public readonly uint Value;

        public ClusterId(int value) => Value = (uint)value;
        public ClusterId(uint value) => Value = value;
        public bool Equals(ClusterId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ClusterId other && Equals(other);
        public override int GetHashCode() => (int)Value;
        public static bool operator ==(ClusterId left, ClusterId right) => left.Equals(right);
        public static bool operator !=(ClusterId left, ClusterId right) => !left.Equals(right);

        public bool IsDefault() => Value == default;
    }
}