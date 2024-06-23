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
        [SerializeField] private Transform _rangeVisuals;

        private List<BoosterActionVisualizer> _visualizers;
        private CardListItem _cardListItem;

        public IBooster Data { get; private set; }

        private void Awake()
        {
            _visualizers = new List<BoosterActionVisualizer>(GetComponents<BoosterActionVisualizer>());

            EffectRangeHighlighter.Register(this);
            _cardListItem = GetComponent<CardListItem>();
            if (_cardListItem != null) 
                _cardListItem.CardReordered += OnBoosterReordered;
        }

        private void OnBoosterReordered()
        {
            var boosterOrder = new List<BoosterCard>();
            foreach (var item in _cardListItem.Owner.Items)
            {
                var booster = item.GetComponent<BoosterView>();
                boosterOrder.Add((BoosterCard)booster.Data);
            }

            StaticResolver.Resolve<BoosterManagement>().ReorderBoosters(boosterOrder);
        }

        private void OnDestroy()
        {
            EffectRangeHighlighter.Remove(this);
        }

        public void Initialize(IBooster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            if (_name != null)
                _name.text = booster.Name;

            if (booster is WorldBooster world)
                _rangeVisuals.transform.localScale = Vector3.one * (world.Range * 2);

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Data is WorldBooster world)
            {
                _rangeVisuals.gameObject.SetActive(true);
                EffectRangeHighlighter.Highlight(transform.position, world.Range, gameObject);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
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