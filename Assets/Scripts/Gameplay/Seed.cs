namespace RogueIslands.Gameplay
{
    public class Seed
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
    }
}