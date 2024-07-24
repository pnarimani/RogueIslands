using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public static class PlayExtension
    {
        public static IEnumerable<Building> GetInRangeBuildings(this GameState state, Building building)
        {
            return state.PlacedDownBuildings.Where(o =>
                o != building && Vector3.Distance(building.Position, o.Position) <= building.Range);
        }
        
        public static bool HasBadEyesight(this GameState state)
        {
            return state.Boosters.Any(b => b.Name == "Bad Eyesight");
        }
        
        public static bool HasColorful(this GameState state)
        {
            return state.Boosters.Any(b => b.Name == "Colorful");
        }
        
        public static bool HasTourism(this GameState state)
        {
            return state.Boosters.Any(b => b.Name == "Tourism");
        }
        
        public static bool HasGoodYear(this GameState state)
        {
            return state.Boosters.Any(b => b.Name == "Good Year");
        }
        
        public static bool HasSensitive(this GameState state)
        {
            return state.Boosters.Any(b => b.Name == "Sensitive");
        }
        
        public static int GetRiggedCount(this GameState state)
        {
            return state.Boosters.Count(b => b.Name == "Rigged");
        }
    }
}