using System.Linq;
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
            if (state.CurrentScore >= state.RequiredScore)
                return false;
            if (state.Result != GameResult.InProgress)
                return false;
            if (state.Clusters.Count == 0)
                return false;
            return true;
        }

        public static void Play(this GameState state, IGameView view)
        {
            foreach (var building in state.Clusters.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;
            
            state.ScoringState = new ScoringState();
            
            state.ExecuteEvent(view, new DayStart());
            
            foreach (var cluster in state.Clusters)
            {
                foreach (var building in cluster.Buildings)
                {
                    BuildingScored buildingScoredEvent = new() { Building = building };

                    var buildingView = view.GetBuilding(building);

                    while (building.RemainingTriggers > 0)
                    {
                        building.RemainingTriggers--;
                        state.ScoringState.Products += building.Output + building.OutputUpgrade;
                        buildingView.BuildingTriggered(false);
                        buildingScoredEvent.TriggerCount++;

                        state.ExecuteEvent(view, buildingScoredEvent);
                    }
                }

                state.ExecuteEvent(view, new ClusterScored { Cluster = cluster });
            }

            BuildingRemainedInHand buildingRemainedInHand = new();

            foreach (var building in state.BuildingsInHand)
            {
                buildingRemainedInHand.Building = building;
                buildingRemainedInHand.TriggerCount = 1;
                state.ExecuteEvent(view, buildingRemainedInHand);
            }

            state.ExecuteEvent(view, new DayEnd());

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

            if (state.IsRoundFinished())
            {
                state.ExecuteEvent(view, new RoundEnd());

                ShowRoundWinScreen(state, view);

                state.PopulateShop();

                state.Round++;
                if (state.Round >= GameState.TotalRounds)
                {
                    state.Round = 0;
                    state.Act++;

                    state.ExecuteEvent(view, new ActEnd());
                }

                if (state.Act >= GameState.TotalActs)
                {
                    state.Result = GameResult.Win;
                    view.ShowGameWinScreen();
                    return;
                }
            }

            state.BuildingsInHand.Clear();
            state.BuildingsInHand.AddRange(
                state.BuildingDeck.Deck
                    .Skip(state.Day * state.HandSize)
                    .Take(state.HandSize)
            );
            view.ShowBuildingsInHand();

            view.GetUI().RefreshAll();
        }

        private static void ShowRoundWinScreen(GameState state, IGameView view)
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

        public static void StartRound(this GameState state, IGameView view)
        {
            state.RestoreProperties();
            state.ExecuteEvent(view, new PropertiesRestored());
            
            state.BuildingDeck.Deck.Shuffle(state.BuildingDeck.ShufflingRandom);
            state.CurrentScore = 0;
            state.Day = 0;
            state.Clusters.Clear();

            view.DestroyBuildings();
            view.ShowBuildingsInHand();
            view.GetUI().RefreshAll();

            state.ExecuteEvent(view, new RoundStart());
        }

        private static bool HasLost(this GameState state)
            => state.Day >= state.TotalDays && state.CurrentScore < state.RequiredScore;

        private static bool IsRoundFinished(this GameState state)
        {
            return state.CurrentScore >= state.RequiredScore;
        }
    }
}