﻿using System.Linq;

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
            if (state.HasLost())
            {
                state.Result = GameResult.Lose;
                view.ShowLoseScreen();
                return;
            }

            var hasWeekEnded = false;

            if (state.IsWeekFinished())
            {
                hasWeekEnded = true;
                state.Money += state.MoneyPayoutPerWeek;
                state.PopulateShop();

                state.CurrentEvent = "WeekEnd";
                state.ExecuteAll(view);

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

            state.Energy = state.CalculateInitialEnergy();

            if (hasWeekEnded && state.Result == GameResult.InProgress)
            {
                view.ShowWeekWin();
            }
            else
            {
                view.GetUI().RefreshAll();
            }
        }

        public static void StartWeek(this GameState state, IGameView view)
        {
            state.CurrentScore = 0;
            state.Day = 0;
            state.Islands.Clear();
            state.TotalDays = state.DefaultTotalDays;

            view.DestroyBuildings();
            view.ShowBuildingsInHand();
            view.GetUI().RefreshAll();

            state.CurrentEvent = "WeekStart";
            state.ExecuteAll(view);
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