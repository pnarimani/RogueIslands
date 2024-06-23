using System;

namespace RogueIslands
{
    public readonly struct BoosterInstanceId : IEquatable<BoosterInstanceId>
    {
        public readonly int Value;

        public BoosterInstanceId(int value) => Value = value;
        public bool Equals(BoosterInstanceId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is BoosterInstanceId other && Equals(other);
        public override int GetHashCode() => Value;
        public static bool operator ==(BoosterInstanceId left, BoosterInstanceId right) => left.Equals(right);
        public static bool operator !=(BoosterInstanceId left, BoosterInstanceId right) => !(left == right);

        public bool IsDefault() => Value == default;
    }
}