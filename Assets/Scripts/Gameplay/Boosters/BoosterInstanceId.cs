using System;

namespace RogueIslands.Gameplay.Boosters
{
    public readonly struct BoosterInstanceId : IEquatable<BoosterInstanceId>
    {
        private static uint _nextId;
        public readonly uint Value;

        public BoosterInstanceId(uint value) => Value = value;
        public bool Equals(BoosterInstanceId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BoosterInstanceId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(BoosterInstanceId left, BoosterInstanceId right) => left.Equals(right);
        public static bool operator !=(BoosterInstanceId left, BoosterInstanceId right) => !(left == right);

        public bool IsDefault() => Value == default;
        public static BoosterInstanceId New() => new(++_nextId);
    }
}