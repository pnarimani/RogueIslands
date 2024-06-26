using System;

namespace RogueIslands.Buildings
{
    public readonly struct BuildingId : IEquatable<BuildingId>
    {
        public readonly uint Value;

        public BuildingId(uint value) => Value = value;
        public bool Equals(BuildingId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BuildingId other && Equals(other);
        public override int GetHashCode() => (int)Value;
        public static bool operator ==(BuildingId left, BuildingId right) => left.Equals(right);
        public static bool operator !=(BuildingId left, BuildingId right) => !left.Equals(right);

        public bool IsDefault() => Value == default;
    }
}