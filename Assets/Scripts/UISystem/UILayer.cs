namespace RogueIslands.UISystem
{
    public readonly struct UILayer
    {
        public readonly string Value;

        public UILayer(string value)
        {
            Value = value;
        }
        
        public static UILayer Default => new("Default UI Root");
    }
}