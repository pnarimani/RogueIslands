using System;
using System.Collections.Generic;
using RogueIslands.Buildings;
using RogueIslands.DeckBuilding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.DeckBuilding
{
    public class BuildingSelectorPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title, _description;
        [SerializeField] private Button _submit;
        
        private int _count;

        public event Action<IReadOnlyList<Building>> BuildingsSelected;
        
        private void Awake()
        {
            _submit.onClick.AddListener(() =>
            {
                var buildings = new List<Building>();
                for (var i = 0; i < _count; i++)
                {
                    buildings.Add(new Building());
                }
                BuildingsSelected?.Invoke(buildings);
            });
        }
        
        public void Select(int count)
        {
            _count = count;
        }

        public void Show(Consumable consumable)
        {
            _title.text = consumable.Name;
            _description.text = consumable.Description.Get(consumable);
        }
    }
}