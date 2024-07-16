using System;
using System.Collections.Generic;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterCardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Button _sellButton;
        [SerializeField] private TextMeshProUGUI _name, _sellText;

        private CardListItem _cardListItem;
        private BoosterCard _booster;

        private void Awake()
        {
            _cardListItem = GetComponent<CardListItem>();
            if (_cardListItem != null) 
                _cardListItem.DragEnded += OnBoosterReordered;
            
            _sellButton.onClick.AddListener(() =>
            {
                StaticResolver.Resolve<BoosterManagement>().SellBooster(_booster.Id);
            });
        }

        private void OnDestroy()
        {
            if (_cardListItem != null) 
                _cardListItem.DragEnded -= OnBoosterReordered;
        }

        public void Initialize(BoosterCard card)
        {
            _booster = card;
            _name.text = card.Name;
            GetComponent<BoosterView>().Initialize(card);
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

        public void OnPointerClick(PointerEventData eventData)
        {
            _sellText.text = StaticResolver.Resolve<ILocalization>().Get("Sell-Price-Button", _booster.SellPrice);
            _sellButton.gameObject.SetActive(!_sellButton.gameObject.activeSelf);
        }
    }
}