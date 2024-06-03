using System;

namespace RogueIslands
{
    public static class GameProgression
    {
        public static bool HasLost(this GameState state)
        {
            if (state.CurrentScore < state.RequiredScore)
            {
                if (state.Day >= state.TotalDays)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsWeekFinished(this GameState state)
        {
            return state.CurrentScore >= state.RequiredScore;
        }

        public static void Win(this GameState state, IGameView view)
        {
            if (!IsWeekFinished(state))
                throw new InvalidOperationException();

            state.Week++;
            if (state.Week >= GameState.TotalWeeks)
            {
                state.Week = 0;
                state.Month++;
            }

            if (state.Month >= GameState.TotalMonths)
            {
                state.Result = GameResult.Win;
            }

            state.CurrentEvent = "WeekEnd";
            state.ExecuteAll(view);
        }

        public static void Lose(this GameState state)
        {
            if (!HasLost(state))
                throw new InvalidOperationException();

            state.Result = GameResult.Lose;
        }
    }
}