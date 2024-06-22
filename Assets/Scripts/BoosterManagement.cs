using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.GameEvents;
using UnityEngine;

namespace RogueIslands
{
    public static class BoosterManagement
    {
        public static bool TryAddBooster(this GameState state, IGameView view, BoosterCard booster)
        {
            if (state.Boosters.Count >= state.MaxBoosters)
                return false;

            var instance = booster.Clone();
            instance.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());

            state.Boosters.Add(instance);
            view.AddBooster(instance);
            
            state.RestoreProperties();
            state.ExecuteEvent(view, new PropertiesRestored());

            if (instance.BuyAction != null)
                state.Execute(view, instance, instance.BuyAction);

            AddEvaluationOverrides(instance);

            state.ExecuteEvent(view, new BoosterBought { Booster = instance });
            return true;
        }
        
        public static void SpawnWorldBoosters(this GameState state, IGameView view, IReadOnlyList<Vector3> spawnPoints)
        {
            foreach (var point in spawnPoints)
            {
                var index = state.WorldBoosterRandom.NextInt(state.AvailableWorldBoosters.Count);
                
                var booster = state.AvailableWorldBoosters[index].Clone();
                booster.Id = new BoosterInstanceId(Guid.NewGuid().GetHashCode());
                booster.Position = point;
                booster.Rotation = Quaternion.identity;
                
                state.WorldBoosters.Add(booster);
                view.AddBooster(booster);
            }
        }

        public static void SellBooster(this GameState state, IGameView view, BoosterInstanceId boosterId)
        {
            var booster = state.Boosters.First(x => x.Id == boosterId);
            state.Boosters.Remove(booster);
            if (booster.SellAction != null)
                state.Execute(view, booster, booster.SellAction);
            state.Money += booster.SellPrice;
            RemoveEvaluationOverrides(booster);
            view.GetBooster(booster).Remove();
            view.GetUI().RefreshAll();
            
            state.RestoreProperties();
            state.ExecuteEvent(view, new PropertiesRestored());
            
            state.ExecuteEvent(view, new BoosterSold() { Booster = booster });
        }

        public static void DestroyBooster(this GameState state, IGameView view, BoosterInstanceId boosterId)
        {
            var booster = state.Boosters.First(x => x.Id == boosterId);
            state.Boosters.Remove(booster);
            RemoveEvaluationOverrides(booster);
            view.GetBooster(booster).Remove();
            view.GetUI().RefreshAll();
            state.ExecuteEvent(view, new BoosterDestroyed() { Booster = booster });
        }

        private static void AddEvaluationOverrides(BoosterCard instance)
        {
            if (instance.EvaluationOverrides != null)
            {
                foreach (var e in instance.EvaluationOverrides) 
                    GameConditionsManager.RegisterEvaluatorOverride(e);
            }
        }

        private static void RemoveEvaluationOverrides(BoosterCard booster)
        {
            if (booster.EvaluationOverrides != null)
            {
                foreach (var e in booster.EvaluationOverrides) 
                    GameConditionsManager.UnregisterEvaluatorOverride(e);
            }
        }
    }
}