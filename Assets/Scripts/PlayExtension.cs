using System.Linq;
using RogueIslands.Scoring;

namespace RogueIslands
{
    public static class PlayExtension
    {
        public static bool CanPlay(this GameState state)
        {
            if (state.Day >= state.TotalDays)
                return false;
            if (state.CurrentScore >= state.RequiredScore)
                return false;
            if (state.Result != GameResult.InProgress)
                return false;
            if (state.Islands.Count == 0)
                return false;
            return true;
        }

        public static void Play(this GameState state, IGameView view)
        {
            state.WorldBoosters.Clear();
            state.WorldBoosters.AddRange(view.GetWorldBoosters());

            foreach (var building in state.Islands.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;

            using (new ScoringScope(state, view))
            {
                foreach (var island in state.Islands)
                {
                    using (new IslandScoringScope(state, view, island))
                    {
                        foreach (var building in island.Buildings)
                        {
                            using (var buildingScoring = new BuildingScoringScope(state, view, building))
                            {
                                var buildingView = view.GetBuilding(building);

                                while (building.RemainingTriggers > 0)
                                {
                                    buildingScoring.TriggerBeforeScore();

                                    building.RemainingTriggers--;
                                    state.ScoringState.Products += building.Output + building.OutputUpgrade;
                                    buildingView.BuildingTriggered(false);

                                    buildingScoring.TriggerAfterScore();
                                }
                            }
                        }
                    }
                }
            }

            state.CurrentScore += state.ScoringState.Products * state.ScoringState.Multiplier;
            state.Day++;
            state.ScoringState = null;

            view.GetUI().RefreshDate();

            state.Validate();
        }

        public static void ProcessScore(this GameState state, IGameView view)
        {
            if (state.HasLost())
            {
                state.Result = GameResult.Lose;
                view.ShowLoseScreen();
                return;
            }

            if (state.IsWeekFinished())
            {
                state.ExecuteEvent(view, "WeekEnd");

                ShowWeekWinScreen(state, view);

                state.PopulateShop();

                state.Week++;
                if (state.Week >= GameState.TotalWeeks)
                {
                    state.Week = 0;
                    state.Month++;

                    state.ExecuteEvent(view, "MonthEnd");
                }

                if (state.Month >= GameState.TotalMonths)
                {
                    state.Result = GameResult.Win;
                    view.ShowGameWinScreen();
                    return;
                }
            }

            state.BuildingsInHand.Clear();
            state.BuildingsInHand.AddRange(
                state.AvailableBuildings
                    .Skip(state.Day * state.HandSize)
                    .Take(state.HandSize)
            );
            view.ShowBuildingsInHand();

            view.GetUI().RefreshAll();
        }

        private static void ShowWeekWinScreen(GameState state, IGameView view)
        {
            var winScreen = view.ShowWeekWin();
            winScreen.AddMoneyChange(new MoneyChange
            {
                Change = state.MoneyPayoutPerWeek,
                Reason = "Weekly Payout",
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

        public static void ClaimWeekEndMoney(this GameState state, IGameView view)
        {
            state.Money += state.MoneyPayoutPerWeek;
            state.Money += state.TotalDays - state.Day;
            foreach (var change in state.MoneyChanges)
                state.Money += change.Change;
            state.MoneyChanges.Clear();

            view.GetUI().RefreshAll();
            view.ShowShopScreen();
        }

        public static void StartWeek(this GameState state, IGameView view)
        {
            state.CurrentScore = 0;
            state.Day = 0;
            state.Islands.Clear();
            state.TotalDays = state.DefaultTotalDays;
            state.HandSize = state.DefaultHandSize;

            view.DestroyBuildings();
            view.ShowBuildingsInHand();
            view.GetUI().RefreshAll();

            state.ExecuteEvent(view, "WeekStart");
        }

        private static bool HasLost(this GameState state)
            => state.Day >= state.TotalDays && state.CurrentScore < state.RequiredScore;

        private static bool IsWeekFinished(this GameState state)
        {
            return state.CurrentScore >= state.RequiredScore;
        }
    }
}