namespace RogueIslands.Gameplay
{
    public sealed class Seed
    {
        public readonly string Value;

        public Seed(string value) => Value = value;
        
        public static Seed GenerateRandom()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            var random = new System.Random();
            var seed = new char[8];
            for (var i = 0; i < seed.Length; i++)
            {
                seed[i] = chars[random.Next(chars.Length)];
            }
            return new Seed(new string(seed));
        }

        private bool Equals(Seed other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Seed)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}