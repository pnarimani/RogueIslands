using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Boosters
{
    public class BoosterManagement
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly IEventController _eventController;
        private readonly ICloner _cloner;

        public BoosterManagement(
            GameState state,
            IGameView view,
            IEventController eventController,
            ICloner cloner)
        {
            _cloner = cloner;
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public bool TryAddBooster(BoosterCard booster)
        {
            if (_state.Boosters.Count >= _state.MaxBoosters)
                return false;

            var instance = _cloner.Clone(booster);
            instance.Id = BoosterInstanceId.New();
            instance.SellPrice = instance.BuyPrice - 1;

            _state.Boosters.Add(instance);
            _view.AddBooster(instance);

            instance.BuyAction?.Execute(booster);

            _eventController.Execute(new BoosterBoughtEvent { Booster = instance });
            _eventController.Execute(new ResetRetriggersEvent());
            return true;
        }

        public void SellBooster(BoosterInstanceId boosterId)
        {
            var booster = _state.Boosters.First(x => x.Id == boosterId);
            _state.Boosters.Remove(booster);
            booster.SellAction?.Execute(booster);
            _state.Money += booster.SellPrice;
            _view.GetBooster(boosterId).Remove();
            _view.GetUI().RefreshMoney();

            _eventController.Execute(new BoosterSoldEvent { Booster = booster });
            _eventController.Execute(new ResetRetriggersEvent());
        }

        public void DestroyBooster(BoosterInstanceId boosterId)
        {
            if (_state.Boosters.FirstOrDefault(x => x.Id == boosterId) is { } card)
            {
                _state.Boosters.Remove(card);
                _view.GetBooster(boosterId).Remove();

                _eventController.Execute(new BoosterDestroyedEvent { Booster = card });
                _eventController.Execute(new ResetRetriggersEvent());
            }
            else if (_state.WorldBoosters.SpawnedBoosters.FirstOrDefault(x => x.Id == boosterId) is { } worldBooster)
            {
                _state.WorldBoosters.SpawnedBoosters.Remove(worldBooster);
                _view.GetBooster(boosterId).Remove();

                _eventController.Execute(new BoosterDestroyedEvent { Booster = worldBooster });
                _eventController.Execute(new ResetRetriggersEvent());
            }
            else
            {
                throw new Exception("Failed to find booster with id " + boosterId);
            }
        }

        public void ReorderBoosters(IReadOnlyList<BoosterCard> order)
        {
            _state.Boosters.Clear();
            _state.Boosters.AddRange(order);

            _eventController.Execute(new ResetRetriggersEvent());
        }
    }
}