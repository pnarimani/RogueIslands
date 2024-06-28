namespace RogueIslands.Gameplay.DeckBuilding.Actions
{
    public abstract class DeckAction
    {
        public int MinCardsRequired { get; set; }
        public int MaxCardsRequired { get; set; }
    }
}