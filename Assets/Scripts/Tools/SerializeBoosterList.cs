﻿using System.Collections.Generic;
using Autofac;
using RogueIslands.Serialization;
using RogueIslands.Autofac.Modules;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.DeckBuilding;
using RogueIslands.GameEvents;
using UnityEditor;
using UnityEngine;
using Random = Unity.Mathematics.Random;

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
                // var list = BoosterList.Get();

                var builder = new ContainerBuilder();
                builder.RegisterModule(new YamlSerializationModule());
                var container = builder.Build();
                var serializer = container.Resolve<ISerializer>();
                var gameState = GameFactory.NewGame(new System.Random());
                gameState.Clusters.Add(new Cluster
                {
                    Id = "A",
                    Buildings = new List<Building>(gameState.BuildingDeck.Deck),
                });
                _text = serializer.SerializePretty(gameState);
            }

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.TextArea(_text);
            GUILayout.EndScrollView();
        }
    }
}