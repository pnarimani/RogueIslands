using System.Linq;

namespace RogueIslands
{
    public static class PlayExtension
    {
        public static void Play(this GameState state, IGameView view)
        {
            state.CurrentEvent = "DayStart";
            state.ScoringState = new ScoringState();

            foreach (var building in state.Islands.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;

            state.ExecuteAll(view);

            foreach (var island in state.Islands)
            {
                state.CurrentEvent = "BeforeIslandScore";
                state.ScoringState.CurrentScoringIsland = island;
                state.ScoringState.CurrentScoringBuilding = null;

                view.HighlightIsland(island);

                state.ExecuteAll(view);

                foreach (var building in island.Buildings)
                {
                    var buildingView = view.GetBuilding(building);
                    var triggeredOnce = false;

                    while (building.RemainingTriggers > 0)
                    {
                        state.CurrentEvent = "OnBuildingScore";
                        state.ScoringState.CurrentScoringBuilding = building;

                        building.RemainingTriggers--;
                        state.ScoringState.Products += building.Output + building.OutputUpgrade;
                        buildingView.BuildingTriggered(triggeredOnce);
                        triggeredOnce = true;


                        state.ExecuteAll(view);
                    }
                }

                state.CurrentEvent = "AfterIslandScore";
                state.ScoringState.CurrentScoringBuilding = null;

                state.ExecuteAll(view);
            }

            state.ScoringState.CurrentScoringIsland = null;
            state.ScoringState.CurrentScoringBuilding = null;
            state.CurrentEvent = "DayEnd";
            state.ExecuteAll(view);

            state.CurrentScore += state.ScoringState.Products * state.ScoringState.Multiplier;
            state.Day++;
            state.ScoringState = null;

            state.Validate();
        }

        public static void ProcessScore(this GameState state, IGameView view)
        {
            bool hasWeekEnded = false, hasMonthEnded = false;

            if (state.IsWeekFinished())
            {
                hasWeekEnded = true;
                state.Money += state.MoneyPayoutPerWeek;
                state.PopulateShop();

                state.Week++;
                if (state.Week >= GameState.TotalWeeks)
                {
                    state.Week = 0;
                    state.Month++;
                    hasMonthEnded = true;
                }

                if (state.Month >= GameState.TotalMonths)
                {
                    state.Result = GameResult.Win;
                }
            }

            state.Energy = state.CalculateInitialEnergy();

            if (hasWeekEnded)
            {
                state.CurrentEvent = "WeekEnd";
                state.ExecuteAll(view);
            }

            if (hasMonthEnded)
            {
                state.CurrentEvent = "MonthEnd";
                state.ExecuteAll(view);
            }

            if (state.HasLost())
            {
                state.Result = GameResult.Lose;
                view.ShowLoseScreen();
            }

            if (state.Result == GameResult.Win)
                view.ShowGameWinScreen();

            if (hasWeekEnded && state.Result != GameResult.Win)
                view.ShowWeekWin();
        }

        private static bool HasLost(this GameState state)
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