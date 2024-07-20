using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using UnityEngine;

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
            _eventController.Execute(new BuildingPlaced
            {
                Building = building,
            });

            var buildingTriggered = new BuildingTriggered()
            {
                Building = building,
                TriggerCount = 0,
            };

            while (building.RemainingTriggers > 0)
            {
                building.RemainingTriggers--;
                buildingTriggered.TriggerCount++;

                _state.TransientScore = Math.Ceiling(building.Output + building.OutputUpgrade);
                _view.GetBuilding(building).BuildingTriggered((int)_state.TransientScore);

                if (buildingTriggered.TriggerCount == 1)
                {
                    foreach (var other in _state.GetInRangeBuildings(building))
                    {
                        var bonus = GetScoreBonus(building, other);

                        if (bonus > 0)
                        {
                            _view.GetBuilding(other).BuildingTriggered((int)bonus);
                            _state.TransientScore += bonus;
                        }
                    }
                }

                _eventController.Execute(buildingTriggered);
            }

            _state.TransientScore = Math.Ceiling(_state.TransientScore);

            _eventController.Execute(new AfterAllBuildingTriggers
            {
                Building = building,
            });

            _state.CurrentScore += _state.TransientScore;

            _state.TransientScore = 0;

            _view.CheckForRoundEnd();
        }

        public double GetScoreBonus(Building building, Building other)
        {
            var initialOutput = building.Output + building.OutputUpgrade;

            var output = initialOutput;

            if (building.Color == other.Color ||
                (_state.HasBadEyesight() && BadEyesightColorCheck(building.Color, other.Color)))
            {
                output = Math.Ceiling(output * 1.1);
            }

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