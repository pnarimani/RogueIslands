using System;

namespace RogueIslands.Buildings
{
    public readonly struct BuildingInstanceId : IEquatable<BuildingInstanceId>
    {
        public readonly int Value;

        public BuildingInstanceId(int value) => Value = value;
        public bool Equals(BuildingInstanceId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BuildingInstanceId other && Equals(other);
        public override int GetHashCode() => Value;
        public static bool operator ==(BuildingInstanceId left, BuildingInstanceId right) => left.Equals(right);
        public static bool operator !=(BuildingInstanceId left, BuildingInstanceId right) => !left.Equals(right);
        
        public bool IsDefault() => Value == default;
    }
}