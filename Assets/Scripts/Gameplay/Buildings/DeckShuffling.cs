namespace RogueIslands.Gameplay.Buildings
{
    public static class DeckShuffling
    {
        public static void ShuffleDeck(this GameState state)
            => state.Buildings.Deck.Shuffle(state.Buildings.ShufflingRandom.ForAct(state.Act));
    }
}