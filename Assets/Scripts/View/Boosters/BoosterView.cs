using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace RogueIslands.View.Boosters
{
    public class BoosterView : MonoBehaviour, IBoosterView, IPointerEnterHandler, IPointerExitHandler, IHighlightable
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private DescriptionBox _descriptionBoxPrefab;
        [SerializeField] private Transform _descriptionBoxParent;
        [SerializeField] private Transform _rangeVisuals;

        private List<BoosterActionVisualizer> _visualizers;
        private DescriptionBox _descriptionBoxInstance;

        public IBooster Data { get; private set; }

        private void Awake()
        {
            _visualizers = new List<BoosterActionVisualizer>(GetComponents<BoosterActionVisualizer>());
            
            EffectRangeHighlighter.Register(this);
        }
        
        private void OnDestroy()
        {
            EffectRangeHighlighter.Remove(this);
        }

        public void Initialize(IBooster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            _name.text = booster.Name;

            if (booster is WorldBooster world)
                _rangeVisuals.transform.localScale = Vector3.one * (world.Range * 2);
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
            GameUI.Instance.RefreshMoneyAndEnergy();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_descriptionBoxInstance == null)
            {
                _descriptionBoxInstance = Instantiate(_descriptionBoxPrefab, _descriptionBoxParent);
                _descriptionBoxInstance.SetDescription(Data.Description.Get(Data));
            }
            
            if (Data is WorldBooster world)
            {
                _rangeVisuals.gameObject.SetActive(true);
                EffectRangeHighlighter.Highlight(transform.position, world.Range, gameObject);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_descriptionBoxInstance != null)
            {
                Destroy(_descriptionBoxInstance.gameObject);
                _descriptionBoxInstance = null;
            }
            
            if (Data is WorldBooster)
            {
                _rangeVisuals.gameObject.SetActive(false);
                EffectRangeHighlighter.LowlightAll();
            }
        }

        public void Highlight(bool highlight)
        {
            if (Data is WorldBooster)
            {
                
            }
        }

        public void ShowRange(bool showRange)
        {
            if (Data is WorldBooster)
                _rangeVisuals.gameObject.SetActive(showRange);
        }
    }
}