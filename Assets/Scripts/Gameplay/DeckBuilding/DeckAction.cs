namespace RogueIslands.Gameplay.DeckBuilding
{
    public abstract class DeckAction
    {
        public int MinCardsRequired { get; set; }
        public int MaxCardsRequired { get; set; }
    }
}