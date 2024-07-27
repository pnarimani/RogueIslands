namespace RogueIslands.Gameplay.View.Descriptions
{
    public static class TextColorWrapping
    {
        public static string WrapWithColor(this string text, string color) => $"<color=#{color}>{text}</color>";
    }
}