using System.Collections.Generic;
using System.Linq;
using RogueIslands.GameEvents;
using RogueIslands.Rollback;
using RogueIslands.Serialization;

namespace RogueIslands.Boosters
{
    public class BoosterManagement
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly IEventController _eventController;
        private readonly GameActionController _gameActionController;
        private readonly ResetController _resetController;
        private readonly ICloner _cloner;

        public BoosterManagement(
            GameState state,
            IGameView view,
            IEventController eventController,
            GameActionController gameActionController,
            ResetController resetController,
            ICloner cloner)
        {
            _cloner = cloner;
            _resetController = resetController;
            _gameActionController = gameActionController;
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

            _resetController.RestoreProperties();

            if (instance.BuyAction != null)
                _gameActionController.Execute(instance, instance.BuyAction);

            _eventController.Execute(new BoosterBought { Booster = instance });
            return true;
        }

        public void SellBooster(BoosterInstanceId boosterId)
        {
            var booster = _state.Boosters.First(x => x.Id == boosterId);
            _state.Boosters.Remove(booster);
            if (booster.SellAction != null)
                _gameActionController.Execute(booster, booster.SellAction);
            _state.Money += booster.SellPrice;
            _view.GetBooster(booster).Remove();
            _view.GetUI().RefreshAll();

            _resetController.RestoreProperties();

            _eventController.Execute(new BoosterSold() { Booster = booster });
        }

        public void DestroyBooster(BoosterInstanceId boosterId)
        {
            var booster = _state.Boosters.First(x => x.Id == boosterId);
            _state.Boosters.Remove(booster);
            _view.GetBooster(booster).Remove();
            _view.GetUI().RefreshAll();

            _resetController.RestoreProperties();

            _eventController.Execute(new BoosterDestroyed() { Booster = booster });
        }

        public void ReorderBoosters(IReadOnlyList<BoosterCard> order)
        {
            _state.Boosters.Clear();
            _state.Boosters.AddRange(order);
        }
    }
}