using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.GameEvents;
using RogueIslands.Rollback;
using UnityEngine;

namespace RogueIslands
{
    public class BoosterManagement
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly EventController _eventController;
        private readonly GameActionController _gameActionController;

        public BoosterManagement(GameState state, IGameView view, EventController eventController,
            GameActionController gameActionController)
        {
            _gameActionController = gameActionController;
            _eventController = eventController;
            _view = view;
            _state = state;
        }

        public bool TryAddBooster(BoosterCard booster)
        {
            if (_state.Boosters.Count >= _state.MaxBoosters)
                return false;

            var instance = booster.Clone();
            instance.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());

            _state.Boosters.Add(instance);
            _view.AddBooster(instance);

            _state.RestoreProperties();
            _state.ExecuteEvent(_view, new PropertiesRestored());

            if (instance.BuyAction != null)
                _gameActionController.Execute(instance, instance.BuyAction);

            _eventController.Execute(new BoosterBought { Booster = instance });
            return true;
        }

        public void SpawnWorldBoosters(IReadOnlyList<Vector3> spawnPoints)
        {
            foreach (var point in spawnPoints)
            {
                var index = _state.WorldBoosterRandom.NextInt(_state.AvailableWorldBoosters.Count);

                var booster = _state.AvailableWorldBoosters[index].Clone();
                booster.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());
                booster.Position = point;
                booster.Rotation = Quaternion.identity;

                _state.WorldBoosters.Add(booster);
                _view.AddBooster(booster);
            }
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

            _state.RestoreProperties();
            _state.ExecuteEvent(_view, new PropertiesRestored());

            _state.ExecuteEvent(_view, new BoosterSold() { Booster = booster });
        }

        public void DestroyBooster(BoosterInstanceId boosterId)
        {
            var booster = _state.Boosters.First(x => x.Id == boosterId);
            _state.Boosters.Remove(booster);
            _view.GetBooster(booster).Remove();
            _view.GetUI().RefreshAll();
            _state.ExecuteEvent(_view, new BoosterDestroyed() { Booster = booster });
        }
    }
}