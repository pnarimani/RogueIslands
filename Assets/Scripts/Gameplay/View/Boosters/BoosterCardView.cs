using DG.Tweening;
using Flexalon;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private BuySellPanelView _buySellPanel;
        
        private BoosterCard _booster;

        private void Awake()
        {
            GetComponentInChildren<FlexalonInteractable>().Clicked.AddListener(arg0 =>
            {
                if (_buySellPanel.gameObject.activeSelf)
                {
                    _buySellPanel.gameObject.SetActive(false);
                    RemoveCanvas();
                }
                else
                {
                    _buySellPanel.gameObject.SetActive(true);
                    // _buySellPanel.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
                    AddCanvas();
                }
            });
        }

        private void RemoveCanvas()
        {
            Destroy(GetComponent<GraphicRaycaster>());
            Destroy(GetComponent<Canvas>());
        }

        private void AddCanvas()
        {
            var source = gameObject.GetComponentInParent<Canvas>();
            var canvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = source.sortingOrder + 1;
            canvas.vertexColorAlwaysGammaSpace = source.vertexColorAlwaysGammaSpace;
            canvas.additionalShaderChannels = source.additionalShaderChannels;
        }

        private void OnDestroy()
        {
            
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

        private void OnBoosterReordered()
        {
            // var boosterOrder = new List<BoosterCard>();
            // foreach (var item in _cardListItem.Owner.Items)
            // {
            //     var booster = item.GetComponent<BoosterView>();
            //     boosterOrder.Add((BoosterCard)booster.Data);
            // }
            //
            // StaticResolver.Resolve<BoosterManagement>().ReorderBoosters(boosterOrder);
        }
    }
}