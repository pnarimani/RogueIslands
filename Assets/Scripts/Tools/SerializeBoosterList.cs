using System;
using System.Collections.Generic;
using Autofac;
using RogueIslands.Serialization;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Boosters.Descriptions;
using RogueIslands.Gameplay.Buildings;
using UnityEditor;
using UnityEngine;

namespace RogueIslands.Tools
{
    public class SerializeBoosterList : EditorWindow
    {
        private string _text;
        private Vector2 _scrollPos;

        [MenuItem("Rogue Islands/Serialize Boosters")]
        public static void Open()
        {
            GetWindow<SerializeBoosterList>().Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Serialize"))
            {
                var building = new Building
                {
                    Id = BuildingId.NewBuildingId(),
                    Position = default,
                    Rotation = default,
                    RemainingTriggers = 1,
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
                    Deck = new List<Building> { building },
                    All = new List<Building> { building },
                };

                var builder = new ContainerBuilder();
                builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
                var container = builder.Build();
                var serializer = container.Resolve<ISerializer>();
                _text = serializer.SerializePretty(buildingState);
            }

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.TextArea(_text);
            GUILayout.EndScrollView();
        }
    }
}