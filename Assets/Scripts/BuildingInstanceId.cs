using System;

namespace RogueIslands
{
    public readonly struct BuildingInstanceId : IEquatable<BuildingInstanceId>
    {
        public readonly int Value;

        public BuildingInstanceId(int value) => Value = value;
        public bool Equals(BuildingInstanceId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BuildingInstanceId other && Equals(other);
        public override int GetHashCode() => Value;
        
        public bool IsDefault() => Value == default;
    }
}