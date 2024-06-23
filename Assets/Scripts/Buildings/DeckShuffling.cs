namespace RogueIslands.Buildings
{
    public static class DeckShuffling
    {
        public static void Shuffle(this BuildingDeck deck)
        {
            deck.Deck.Shuffle(deck.ShufflingRandom);
        }
    }
}