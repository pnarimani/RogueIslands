﻿using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;
using UnityEngine;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingsByRange : ISource<Building>
    {
        public ISource<Building> Center { get; set; }
        public bool ReturnInRange { get; set; }
        
        public IEnumerable<Building> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            foreach (var building in Center.Get(booster))
            {
                foreach (var other in state.PlacedDownBuildings)
                {
                    if (other == building)
                        continue;

                    var isInRange = Vector3.Distance(building.Position, other.Position) <= building.Range;
                    
                    if ((ReturnInRange && isInRange) || (!ReturnInRange && !isInRange))
                    {
                        yield return other;
                    }
                }
            }
        }
    }
}