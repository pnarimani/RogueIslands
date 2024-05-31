using System.Linq;

namespace RogueIslands
{
    public static class PlayExtension
    {
        public static void Play(this GameState state)
        {
            state.CurrentEvent = "DayStart";
            state.ScoringState = new ScoringState();

            foreach (var building in state.Islands.SelectMany(island => island.Buildings))
                building.RemainingTriggers = 1;

            state.ExecuteAll();

            foreach (var island in state.Islands)
            {
                state.CurrentEvent = "BeforeIslandScore";
                state.ScoringState.CurrentScoringIsland = island;
                state.ScoringState.CurrentScoringBuilding = null;

                state.ExecuteAll();

                foreach (var building in island.Buildings)
                {
                    while (building.RemainingTriggers > 0)
                    {
                        state.CurrentEvent = "OnBuildingScore";
                        state.ScoringState.CurrentScoringBuilding = building;

                        building.RemainingTriggers--;
                        state.ScoringState.Products += building.Building.Output + building.Building.OutputUpgrade;

                        state.ExecuteAll();
                    }
                }

                state.CurrentEvent = "AfterIslandScore";
                state.ScoringState.CurrentScoringBuilding = null;

                state.ExecuteAll();
            }
            
            state.ScoringState.CurrentScoringIsland = null;
            state.ScoringState.CurrentScoringBuilding = null;
            state.CurrentEvent = "DayEnd";
            state.ExecuteAll();
        }
    }
}