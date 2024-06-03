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
            
            state.Validate();
        }
    }
}