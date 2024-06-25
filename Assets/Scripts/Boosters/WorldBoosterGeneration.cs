
using RogueIslands.Serialization;
using UnityEngine;

namespace RogueIslands.Boosters
{
    public class WorldBoosterGeneration
    {
        private readonly GameState _state;
        private readonly IGameView _view;
        private readonly ICloner _cloner;

        public WorldBoosterGeneration(GameState state, IGameView view, ICloner cloner)
        {
            _cloner = cloner;
            _view = view;
            _state = state;
        }
        
        public void GenerateWorldBoosters()
        {
            var boosters = _state.WorldBoosters;
            var count = boosters.Count.GetRandom(boosters.CountRandom);

            count -= boosters.SpawnedBoosters.Count;
            
            for (var i = 0; i < count; i++)
            {
                if (boosters.SpawnRandom.NextFloat() > boosters.SpawnChance)
                    continue;
                
                var index = boosters.SelectionRandom.NextInt(boosters.All.Count);
                var blueprint = boosters.All[index];
                
                if (!_view.TryGetWorldBoosterSpawnPoint(blueprint, ref boosters.PositionRandom, out var point))
                    return;

                var booster = _cloner.Clone(blueprint);
                booster.Id = new BoosterInstanceId(System.Guid.NewGuid().GetHashCode());
                booster.Position = point;
                booster.Rotation = Quaternion.identity;
                
                boosters.SpawnedBoosters.Add(booster);
                _view.AddBooster(booster);
            }
        }
    }
}