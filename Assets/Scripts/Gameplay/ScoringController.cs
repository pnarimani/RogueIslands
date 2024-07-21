using System;
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
            _eventController.Execute(new ResetRetriggers());
            
            _eventController.Execute(new BuildingPlaced { Building = building, });

            TriggerBuilding(building);

            _state.TransientScore = Math.Ceiling(_state.TransientScore);

            _eventController.Execute(new AfterAllBuildingTriggers { Building = building, });

            _state.CurrentScore += _state.TransientScore;

            _state.TransientScore = 0;
        }

        public void TriggerBuilding(Building building)
        {
            TriggerBuilding(building, true, Math.Ceiling(building.Output + building.OutputUpgrade));
        }

        private void TriggerBuilding(Building building, bool shouldScoreBonus, double buildingScore)
        {
            _state.TransientScore += buildingScore;
            _view.GetBuilding(building).BuildingTriggered((int)buildingScore);

            _eventController.Execute(new BuildingTriggered { Building = building, });

            if (shouldScoreBonus)
                ScoreBonusForBuilding(building);

            _eventController.Execute(new AfterBuildingScoreTrigger() { Building = building, });
        }

        private void ScoreBonusForBuilding(Building building)
        {
            foreach (var other in _state.GetInRangeBuildings(building))
            {
                var bonus = GetScoreBonus(building, other);

                if (bonus > 0)
                {
                    var otherBuildingView = _view.GetBuilding(other);
                    if (_state.HasSensitive())
                    {
                        TriggerBuilding(other, false, bonus);
                    }
                    else
                    {
                        otherBuildingView.BonusTriggered((int)bonus);
                        _state.TransientScore += bonus;
                    }
                }
            }
        }

        public double GetScoreBonus(Building building, Building other)
        {
            var initialOutput = building.Output + building.OutputUpgrade;

            var output = initialOutput;

            if (building.Color == other.Color ||
                (_state.HasBadEyesight() && BadEyesightColorCheck(building.Color, other.Color)))
                output = Math.Ceiling(output * 1.1);

            if (building.Category == other.Category) output = Math.Ceiling(output * 1.2);

            if (building.Size == other.Size) output = Math.Ceiling(output * 1.05);

            return Math.Ceiling(output - initialOutput);
        }

        private static bool BadEyesightColorCheck(ColorTag c1, ColorTag c2)
        {
            return c1 == c2 ||
                   (c1 == ColorTag.Red && c2 == ColorTag.Blue) ||
                   (c2 == ColorTag.Red && c1 == ColorTag.Blue) ||
                   (c1 == ColorTag.Purple && c2 == ColorTag.Green) ||
                   (c2 == ColorTag.Purple && c1 == ColorTag.Green);
        }
    }
}