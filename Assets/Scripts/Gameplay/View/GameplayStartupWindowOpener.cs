﻿using RogueIslands.Autofac;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.Gameplay.View.RoundSelection;
using RogueIslands.UISystem;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class GameplayStartupWindowOpener : MonoBehaviour
    {
        public void Start()
        {
            var windowOpener = StaticResolver.Resolve<IWindowOpener>();
            windowOpener.Open<GameUI>();

            // windowOpener.Open<CardPackSelection>();
            
            StaticResolver.Resolve<RoundController>().StartRound();
            GameUI.Instance.RefreshDeckText();
        }
    }
}