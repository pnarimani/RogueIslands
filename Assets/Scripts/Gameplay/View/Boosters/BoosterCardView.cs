using System;
using Flexalon;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Shop;
using TMPro;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private BuySellPanelView _buySellPanel;

        private BoosterCard _booster;

        private void Awake()
        {
            var interactable = GetComponentInChildren<FlexalonInteractable>();
            interactable.DragEnd.AddListener(OnBoosterReordered);
            interactable.Clicked.AddListener(OnBoosterClicked);
        }

        private void OnBoosterClicked(FlexalonInteractable arg0)
        {
            foreach (var view in ObjectRegistry.GetBoosters())
            {
                view.GetComponent<BoosterCardView>().HideSellPanel();
            }

            if (_buySellPanel.gameObject.activeSelf)
            {
                HideSellPanel();
            }
            else
            {
                ShowSellPanel();
            }
        }

        private void ShowSellPanel()
        {
            _buySellPanel.gameObject.SetActive(true);
            gameObject.BringToFront();
        }

        private void HideSellPanel()
        {
            _buySellPanel.gameObject.SetActive(false);
            gameObject.ResetOrder();
        }

        public void Initialize(BoosterCard card)
        {
            _booster = card;
            _name.text = card.Name;
            GetComponent<BoosterView>().Initialize(card);
            _buySellPanel.ShowSellButton($"Sell ${card.SellPrice}", OnSellClicked);
            _buySellPanel.HideBuyButton();
        }

        private void OnSellClicked()
        {
            StaticResolver.Resolve<BoosterManagement>().SellBooster(_booster.Id);
        }

        private void OnBoosterReordered(FlexalonInteractable arg0)
        {
            var boosterOrder = new BoosterCard[GameManager.Instance.State.Boosters.Count];
            var cards = ObjectRegistry.GetBoosters();
            foreach (var card in cards)
            {
                boosterOrder[card.transform.GetSiblingIndex()] = (BoosterCard)card.Data;
            }

            StaticResolver.Resolve<BoosterManagement>().ReorderBoosters(boosterOrder);
        }
    }
}