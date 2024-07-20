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
    public class BoosterCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name, _sellText;

        private BoosterCard _booster;

        private void Awake()
        {
        }

        private void OnDestroy()
        {
            
        }

        public void Initialize(BoosterCard card)
        {
            _booster = card;
            _name.text = card.Name;
            GetComponent<BoosterView>().Initialize(card);
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