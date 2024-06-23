namespace RogueIslands
{
    public static class RequiredScoreExtensions
    {
        public static double GetRequiredScore(this GameState state, int act, int round) => state.AllRequiredScores[act * GameState.RoundsPerAct + round];

        public static double GetCurrentRequiredScore(this GameState state) => GetRequiredScore(state, state.Act, state.Round);
    }
}