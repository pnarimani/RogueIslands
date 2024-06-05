using RogueIslands.Boosters;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.View
{
    public class BoosterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name, _desc;
        public Booster Data { get; private set; }

        public void Show(Booster booster)
        {
            Assert.IsNotNull(booster);
            
            Data = booster;
            _name.text = booster.Name;
            _desc.text = booster.Description;
        }
    }
}