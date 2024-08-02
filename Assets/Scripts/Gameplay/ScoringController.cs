using System;
using RogueIslands.Diagnostics;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay
{
    public class ScoringController
    {
        private readonly IEventController _eventController;
        private readonly GameState _state;
        private readonly IGameView _view;

        public ScoringController(GameState state, IGameView view, IEventController eventController)
        {
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public void ScoreBuilding(Building building)
        {
            using var profiler = ProfilerBlock.Begin();
            
            _eventController.Execute(new BuildingPlacedEvent { Building = building, });

            TriggerBuilding(building);

            _state.TransientScore = Math.Ceiling(_state.TransientScore);

            _eventController.Execute(new FinalScoreReadyEvent { Building = building, });

            _state.CurrentScore += _state.TransientScore;

            _state.TransientScore = 0;

            _state.Metadata.RunCategoryPlayCount[building.Category]++;
            _state.Metadata.RoundSizePlayCount[building.Size]++;
        }

        public void TriggerBuilding(Building building)
        {
            TriggerBuilding(building, true);
        }

        private void TriggerBuilding(Building building, bool shouldScoreBonus)
        {
            using var profiler = ProfilerBlock.Begin();
            
            var buildingScore = Math.Ceiling(building.Output + building.OutputUpgrade);
            _state.TransientScore += buildingScore;
            _view.GetBuilding(building).BuildingTriggered((int)buildingScore);

            _eventController.Execute(new BuildingTriggeredEvent { Building = building, });

            if (shouldScoreBonus)
                ScoreBonusForBuilding(building);

            _eventController.Execute(new RetriggerStepEvent() { Building = building, });
        }

        private void ScoreBonusForBuilding(Building building)
        {
            using var profiler = ProfilerBlock.Begin();

            var potentialBuildings = _state.HasInsideOut()
                ? _state.GetOutOfRangeBuildings(building)
                : _state.GetInRangeBuildings(building);
            
            foreach (var other in potentialBuildings)
            {
                if (_state.HasSensitive())
                {
                    TriggerBuilding(other, false);
                    continue;
                }
                
                _state.Score.ResetBonuses();
                
                var initialOutput = building.Output + building.OutputUpgrade;

                if (building.Color == other.Color || (_state.HasBadEyesight() && BadEyesightColorCheck(building.Color, other.Color)))
                    _state.Score.TransientColorBonus = initialOutput * 0.3;

                if (building.Category == other.Category)
                    _state.Score.TransientCategoryBonus = initialOutput * 0.4;

                if (building.Size == other.Size) 
                    _state.Score.TransientSizeBonus = initialOutput * 0.2;
                
                _state.Score.TransientCategoryBonus = Math.Ceiling(_state.Score.TransientCategoryBonus);
                _state.Score.TransientColorBonus = Math.Ceiling(_state.Score.TransientColorBonus);
                _state.Score.TransientSizeBonus = Math.Ceiling(_state.Score.TransientSizeBonus);

                if (_state.Score.GetTotalBonus() <= 0)
                    continue;
                
                _eventController.Execute(new BuildingBonusEvent
                {
                    Building = other,
                    PlacedBuilding = building,
                });

                _state.Score.TransientCategoryBonus = Math.Ceiling(_state.Score.TransientCategoryBonus);
                _state.Score.TransientColorBonus = Math.Ceiling(_state.Score.TransientColorBonus);
                _state.Score.TransientSizeBonus = Math.Ceiling(_state.Score.TransientSizeBonus);
                _state.Score.TransientExtraBonus = Math.Ceiling(_state.Score.TransientExtraBonus);
                
                var otherBuildingView = _view.GetBuilding(other);
                otherBuildingView.BonusTriggered((int)_state.Score.GetTotalBonus());
                _state.TransientScore += _state.Score.GetTotalBonus();
                _state.Score.ResetBonuses();
                
                _eventController.Execute(new AfterBuildingBonusEvent
                {
                    Building = other,
                    PlacedBuilding = building,
                });
            }
        }

        private static bool BadEyesightColorCheck(ColorTag c1, ColorTag c2) =>
            c1 == c2 ||
            (c1 == ColorTag.Red && c2 == ColorTag.Blue) ||
            (c2 == ColorTag.Red && c1 == ColorTag.Blue) ||
            (c1 == ColorTag.Purple && c2 == ColorTag.Green) ||
            (c2 == ColorTag.Purple && c1 == ColorTag.Green);
    }
}