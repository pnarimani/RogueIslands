using System.Collections.Generic;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.View.Boosters
{
    public class BoosterView : MonoBehaviour, IBoosterView
    {
        [SerializeField] private TextMeshProUGUI _name, _desc;
        [SerializeField] private MMF_Player _triggerFeedback;

        private List<BoosterActionVisualizer> _visualizers;

        public Booster Data { get; private set; }

        private void Awake()
        {
            _visualizers = new List<BoosterActionVisualizer>(GetComponents<BoosterActionVisualizer>());
        }

        public void Show(Booster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            _name.text = booster.Name;
            _desc.text = booster.Description.Get(booster);
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

        public void UpdateDescription()
            => _desc.text = Data.Description.Get(Data);
    }
}