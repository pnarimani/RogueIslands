using System.Collections.Generic;

namespace RogueIslands.Gameplay.Descriptions
{
    public class DescriptionData
    {
        public string Text { get; set; }
        public IReadOnlyList<string> Keywords { get; set; }
        
        public static implicit operator DescriptionData(string text) => new() { Text = text };
    }
}