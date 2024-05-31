using System;
using System.Collections;
using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.View;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private NumberText _products,
            _multiplier,
            _requiredOutput,
            _currentAmount,
            _budget,
            _energy,
            _days,
            _week,
            _month;

        [SerializeField] private Button _playButton;
        [SerializeField] private BuildingCardView _buildingCardPrefab;
        [SerializeField] private BoosterView _boosterPrefab;
        [SerializeField] private CardListView _buildingCardList, _boosterList;

        public event Action PlayClicked;

        private void Start()
        {
            _playButton.onClick.AddListener(() => PlayClicked?.Invoke());
        }

        public void ShowBuildingCard(Building building)
        {
            var card = Instantiate(_buildingCardPrefab, _buildingCardList.transform);
            card.Show(building);
            _buildingCardList.Add(card.GetComponent<CardListItem>());
        }

        public void ShowBoosterCard(Booster booster)
        {
            var card = Instantiate(_boosterPrefab, _boosterList.transform);
            card.Show(booster);
            _boosterList.Add(card.GetComponent<CardListItem>());
        }
    }
}