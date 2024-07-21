using System;
using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Boosters.Descriptions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Serialization;

namespace RogueIslands.Tests
{
    public class SerializationIntegrationTest
    {
        private ISerializer _serializer;
        private IDeserializer _deserializer;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
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
                Category = Category.Cat1,
                Color = ColorTag.Blue,
                Size = BuildingSize.Small,
                Output = 34,
                OutputUpgrade = 540,
                Description = new LiteralDescription("A"),
            };
            var buildingState = new BuildingsState()
            {
                Deck = new List<Building>() { building },
                All = new List<Building>() { building },
            };
            var text = _serializer.Serialize(buildingState);

            var deserialized = _deserializer.Deserialize<BuildingsState>(text);

            deserialized.Deck[0].Should().Be(deserialized.All[0]);
        }
    }
}