using System;

namespace RogueIslands.Gameplay.Buildings
{
    public readonly struct ClusterId : IEquatable<ClusterId>
    {
        public readonly uint Value;
        
        private static uint _nextId;

        public ClusterId(uint value) => Value = value;
        public bool Equals(ClusterId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ClusterId other && Equals(other);
        public override int GetHashCode() => (int)Value;
        public static bool operator ==(ClusterId left, ClusterId right) => left.Equals(right);
        public static bool operator !=(ClusterId left, ClusterId right) => !left.Equals(right);
        public bool IsDefault() => Value == default;
        public static ClusterId NewClusterId() => new(++_nextId);
    }
}