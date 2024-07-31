using System;
using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using RogueIslands.Autofac;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Descriptions;
using RogueIslands.Serialization;

namespace RogueIslands.Tests
{
    public class SerializationIntegrationTest
    {
        private IDeserializer _deserializer;
        private ISerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            foreach (var module in ModuleFinder.GetProjectModules())
            {
                module.Load(builder);
            }
            var container = builder.Build();
            _serializer = container.Resolve<ISerializer>();
            _deserializer = container.Resolve<IDeserializer>();
        }

        [Test]
        public void ReferencePreservation()
        {
            var building = new Building
            {
                Id = BuildingId.NewBuildingId(),
                Position = default,
                Rotation = default,
                PrefabAddress = "A",
                IconAddress = "B",
                Range = 3,
                Category = Category.City,
                Color = ColorTag.Blue,
                Size = BuildingSize.Small,
                Output = 34,
                OutputUpgrade = 540,
                Description = new DescriptionData
                {
                    Text = "Text goes here",
                    Keywords = new List<string> { "a", "b", "c" },
                },
            };
            var buildingState = new BuildingsState
            {
                Deck = new List<Building> { building },
                All = new List<Building> { building },
            };
            var text = _serializer.Serialize(buildingState);

            var deserialized = _deserializer.Deserialize<BuildingsState>(text);

            deserialized.Deck[0].Should().Be(deserialized.All[0]);
        }
    }
}