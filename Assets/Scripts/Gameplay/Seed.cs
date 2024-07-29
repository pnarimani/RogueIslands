using System;

namespace RogueIslands.Gameplay
{
    public sealed class Seed
    {
        public readonly string Value;

        public Seed(string value) => Value = value;

        public static Seed GenerateRandom()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var seed = new char[8];
            for (var i = 0; i < seed.Length; i++) seed[i] = chars[random.Next(chars.Length)];
            return new Seed(new string(seed));
        }

        private bool Equals(Seed other) => Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Seed)obj);
        }

        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;
    }
}