using System.Linq;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;
using RogueIslands.Rollback;

namespace RogueIslands
{
    public static class PlayExtension
    {
        public static bool CanPlay(this GameState state)
        {
            if (state.Day >= state.TotalDays)
                return false;
            if (state.CurrentScore >= state.GetCurrentRequiredScore())
                return false;
            if (state.Result != GameResult.InProgress)
                return false;
            if (state.Clusters.Count == 0)
                return false;
            return true;
        }

        public static void ProcessScore(this GameState state, IGameView view)
        {
            if (state.HasLost())
            {
                state.Result = GameResult.Lose;
                view.ShowLoseScreen();
                return;
            }

            if (state.IsRoundFinished())
            {
                state.ExecuteEvent(view, new RoundEnd());

                ShowRoundWinScreen(state, view);

                state.PopulateShop();

                state.Round++;
                if (state.Round >= GameState.RoundsPerAct)
                {
                    state.Round = 0;
                    state.Act++;

                    state.ExecuteEvent(view, new ActEnd());
                }

                if (state.Act >= GameState.TotalActs)
                {
                    state.Result = GameResult.Win;
                    view.ShowGameWinScreen();
                }
            }
            else
            {
                state.BuildingsInHand.Clear();
                state.BuildingsInHand.AddRange(
                    state.BuildingDeck.Deck
                        .Skip(state.Day * state.HandSize)
                        .Take(state.HandSize)
                );
                view.ShowBuildingsInHand();
            }
        }

        private static void ShowRoundWinScreen(GameState state, IGameView view)
        {
            var winScreen = view.ShowRoundWin();
            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = state.MoneyPayoutPerRound,
                Reason = "Round Completion Prize",
            });

            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = state.TotalDays - state.Day,
                Reason = "Days Left (1$ per day)",
            });

            foreach (var change in state.MoneyChanges)
            {
                winScreen.AddMoneyChange(change);
            }
        }

        public static void ClaimRoundEndMoney(this GameState state, IGameView view)
        {
            state.Money += state.MoneyPayoutPerRound;
            state.Money += state.TotalDays - state.Day;
            foreach (var change in state.MoneyChanges)
                state.Money += change.Change;
            state.MoneyChanges.Clear();

            view.GetUI().RefreshAll();
            view.ShowShopScreen();
        }

        public static void StartRound(this GameState state, IGameView view)
        {
            StaticResolver.Resolve<ResetController>().RestoreProperties();

            state.BuildingDeck.Shuffle();
            state.BuildingsInHand = state.BuildingDeck.Deck.Take(state.HandSize).ToList();
            state.CurrentScore = 0;
            state.Day = 0;
            state.Clusters.Clear();

            view.DestroyBuildings();
            view.ShowBuildingsInHand();

            state.ExecuteEvent(view, new RoundStart());

            view.GetUI().RefreshAll();
        }

        private static bool HasLost(this GameState state)
            => state.Day >= state.TotalDays && state.CurrentScore < state.GetCurrentRequiredScore();

        private static bool IsRoundFinished(this GameState state)
        {
            return state.CurrentScore >= state.GetCurrentRequiredScore();
        }
    }
}