using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.UISystem;
using UnityEngine;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class CardPackSelection : MonoBehaviour, IWindow
    {
        [SerializeField] private CardPackView _packPrefab;
        [SerializeField] private RectTransform _packParent;

        private void Start()
        {
            var categories = StaticResolver.Resolve<CardPackSpawner>().GetCategories();
            foreach (var category in categories)
            {
                var pack = Instantiate(_packPrefab, _packParent);
                pack.Initialize(category);
                pack.Clicked += () =>
                {
                    _packParent.gameObject.SetActive(false);
                    var addedCards = StaticResolver.Resolve<CardPackSpawner>().SelectCategory(category);
                    AnimateCards(addedCards).Forget();
                };
            }
        }

        private async UniTask AnimateCards(List<Building> buildings)
        {
            var state = GameManager.Instance.State;
            var hand = state.BuildingsInHand.ToList();
            Debug.Log("hand.Count = " + hand.Count);
            foreach (var inHand in hand)
            {
                if (buildings.Contains(inHand))
                {
                    GameUI.Instance.ShowBuildingCard(inHand);
                    await UniTask.WaitForSeconds(0.25f);
                }
            }
            
            foreach (var peek in state.DeckPeek.ToList())
            {
                if (buildings.Contains(peek))
                {
                    GameUI.Instance.ShowBuildingCardPeek(peek);
                    await UniTask.WaitForSeconds(0.25f);
                }
            }

            Destroy(gameObject);
        }
    }   
}