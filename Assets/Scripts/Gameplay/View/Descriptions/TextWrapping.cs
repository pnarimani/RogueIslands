namespace RogueIslands.Gameplay.View.Descriptions
{
    public static class TextWrapping
    {
        public static string WrapWithColor(this string text, string color) => $"<color=#{color}>{text}</color>";
        
        public static string WrapWithHighlight(this string text, string color) 
            => $"<mark=#{color}>{text}</mark>";
    }
}