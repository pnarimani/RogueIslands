using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterView : MonoBehaviour, IBoosterView
    {
        private List<BoosterActionVisualizer> _visualizers;
        private CardListItem _cardListItem;

        public IBooster Data { get; private set; }

        private void Awake()
        {
            _visualizers = new List<BoosterActionVisualizer>(GetComponents<BoosterActionVisualizer>());
        }

        public void Initialize(IBooster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            GetComponent<DescriptionBoxSpawner>().Initialize(booster);
        }

        public async void OnBeforeActionExecuted(GameState state, GameAction action)
        {
            foreach (var visualizer in _visualizers)
            {
                if (visualizer.CanVisualize(action))
                {
                    await visualizer.OnBeforeBoosterExecuted(state, action, this);
                    break;
                }
            }
        }

        public async void OnAfterActionExecuted(GameState state, GameAction action)
        {
            foreach (var visualizer in _visualizers)
            {
                if (visualizer.CanVisualize(action))
                {
                    await visualizer.OnAfterBoosterExecuted(state, action, this);
                    break;
                }
            }
        }

        public void Remove()
        {
            Destroy(gameObject);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoney();
        }
    }
}