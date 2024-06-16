using System.Linq;

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
            state.CurrentEvent = "DayStart";
            state.ScoringState = new ScoringState();

            foreach (var building in state.Islands.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;

            state.ExecuteAll(view);

            foreach (var island in state.Islands)
            {
                view.HighlightIsland(island);

                state.CurrentEvent = "BeforeIslandScore";
                state.ScoringState.CurrentScoringIsland = island;
                state.ScoringState.CurrentScoringBuilding = null;
                state.ExecuteAll(view);

                foreach (var building in island.Buildings)
                {
                    state.ScoringState.CurrentScoringBuilding = building;
                    var buildingView = view.GetBuilding(building);
                    var triggeredOnce = false;

                    while (building.RemainingTriggers > 0)
                    {
                        state.CurrentEvent = "BeforeBuildingScored";
                        state.ExecuteAll(view);

                        building.RemainingTriggers--;
                        state.ScoringState.Products += building.Output + building.OutputUpgrade;
                        buildingView.BuildingTriggered(triggeredOnce);

                        if (!triggeredOnce)
                        {
                            state.CurrentEvent = "BuildingFirstTrigger";
                            state.ExecuteAll(view);
                        }

                        triggeredOnce = true;

                        state.CurrentEvent = "AfterBuildingScored";
                        state.ExecuteAll(view);
                    }
                }

                state.CurrentEvent = "AfterIslandScore";
                state.ScoringState.CurrentScoringBuilding = null;
                state.ExecuteAll(view);

                view.LowlightIsland(island);
            }

            state.ScoringState.CurrentScoringIsland = null;
            state.ScoringState.CurrentScoringBuilding = null;
            state.CurrentEvent = "DayEnd";
            state.ExecuteAll(view);

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
                state.CurrentEvent = "WeekEnd";
                state.ExecuteAll(view);

                ShowWeekWinScreen(state, view);

                state.PopulateShop();

                state.Week++;
                if (state.Week >= GameState.TotalWeeks)
                {
                    state.Week = 0;
                    state.Month++;

                    state.CurrentEvent = "MonthEnd";
                    state.ExecuteAll(view);
                }

                if (state.Month >= GameState.TotalMonths)
                {
                    state.Result = GameResult.Win;
                    view.ShowGameWinScreen();
                    return;
                }
            }

            state.BuildingsInHand.Clear();
            state.BuildingsInHand.AddRange(state.AvailableBuildings.Skip(state.Day * state.HandSize)
                .Take(state.HandSize));
            view.ShowBuildingsInHand();

            state.Energy = state.CalculateInitialEnergy();

            view.GetUI().RefreshAll();
        }

        private static void ShowWeekWinScreen(GameState state, IGameView view)
        {
            var winScreen = view.ShowWeekWin();
            winScreen.SetWeeklyPayout(state.MoneyPayoutPerWeek);

            foreach (var change in state.MoneyChanges)
            {
                winScreen.AddMoneyChange(change);
            }
        }

        public static void ClaimWeekEndMoney(this GameState state, IGameView view)
        {
            state.Money += state.MoneyPayoutPerWeek;
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

            state.CurrentEvent = "WeekStart";
            state.ExecuteAll(view);
        }

        private static bool HasLost(this GameState state)
            => state.Day >= state.TotalDays && state.CurrentScore < state.RequiredScore;

        private static bool IsWeekFinished(this GameState state)
        {
            return state.CurrentScore >= state.RequiredScore;
        }

        private static int CalculateInitialEnergy(this GameState state)
        {
            return state.StartingEnergy +
                   state.EnergyIncreasePerWeek * (state.Month * GameState.TotalWeeks + state.Week) +
                   state.EnergyIncreasePerMonth * state.Month;
        }
    }
}