using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using UnityEngine;

namespace RogueIslands.Tests
{
    public class BuildingPlacementTests
    {
        private BuildingPlacement _placement;
        private GameState _gameState;

        [SetUp]
        public void SetUp()
        {
            _gameState = new GameState
            {
                Buildings = new BuildingsState()
                {
                    Deck = new List<Building>(),
                },
                WorldBoosters = new WorldBoostersState
                {
                    SpawnedBoosters = new List<WorldBooster>(),
                    All = new List<WorldBooster>(),
                },
            };

            _placement = new BuildingPlacement(_gameState, Substitute.For<IGameView>(), Substitute.For<IEventController>());
        }

        [Test]
        public void PlaceBuilding_WhenConnectingTwoClusters_MergesBothClustersIntoOne()
        {
            var firstBuilding = new Building()
            {
                Range = 1,
                Position = new Vector3(.9f, 0, 0),
                ClusterId = ClusterId.NewClusterId(),
            };
            var secondBuilding = new Building()
            {
                Range = 1,
                Position = new Vector3(-.9f, 0, 0),
                ClusterId = ClusterId.NewClusterId(),
            };
            var toPutDown = new Building()
            {
                Range = 1,
            };
            _gameState.Buildings.Deck.Add(firstBuilding);
            _gameState.Buildings.Deck.Add(secondBuilding);

            _placement.PlaceBuilding(toPutDown, Vector3.zero, Quaternion.identity);

            firstBuilding.ClusterId.Should().Be(toPutDown.ClusterId);
            secondBuilding.ClusterId.Should().Be(toPutDown.ClusterId);
        }
        
        [Test]
        public void PlaceBuilding_WhenBuildingIsNotNearby_CreatesNewCluster()
        {
            var otherClusterId = ClusterId.NewClusterId();
            var other = new Building()
            {
                Range = 1,
                Position = new Vector3(1.9f, 0, 0),
                ClusterId = otherClusterId,
            };
            _gameState.Buildings.Deck.Add(other);
            var toPutDown = new Building()
            {
                Range = 1,
            };
            
            _placement.PlaceBuilding(toPutDown, Vector3.zero, Quaternion.identity);
            
            toPutDown.ClusterId.IsDefault().Should().BeFalse();
            toPutDown.ClusterId.Should().NotBe(other.ClusterId);
            other.ClusterId.Should().Be(otherClusterId);
        }
        
        
        [Test]
        public void PlaceBuilding_WhenInRangeOfAnotherBuilding_JoinsThatCluster()
        {
            var otherClusterId = ClusterId.NewClusterId();
            var other = new Building()
            {
                Range = 1,
                Position = new Vector3(.9f, 0, 0),
                ClusterId = otherClusterId,
            };
            _gameState.Buildings.Deck.Add(other);
            var toPutDown = new Building()
            {
                Range = 1,
            };
            
            _placement.PlaceBuilding(toPutDown, Vector3.zero, Quaternion.identity);
            
            toPutDown.ClusterId.IsDefault().Should().BeFalse();
            toPutDown.ClusterId.Should().Be(other.ClusterId);
            other.ClusterId.Should().Be(otherClusterId);
        }
    }
}