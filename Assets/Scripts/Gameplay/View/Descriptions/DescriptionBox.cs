using RogueIslands.Gameplay.Boosters;
using TMPro;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class DescriptionBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private RectTransform _container;
        [SerializeField] private GameObject[] _rarities;
        
        public void SetDescription(string value) => _desc.text = value;
        
        public void ShowName(string name)
        {
            _name.text = name;
            _name.gameObject.SetActive(true);
        }

        public void HideName()
        {
            _name.gameObject.SetActive(false);
        }

        public void ShowRarity(Rarity rarity)
        {
            Instantiate(_rarities[(int)rarity], _container, false);
        }
        
        public void SetAlignment(DescriptionAlignment horizontal, DescriptionAlignment vertical)
        {
            var x = horizontal switch
            {
                DescriptionAlignment.Start => 0,
                DescriptionAlignment.Center => 0.5f,
                DescriptionAlignment.End => 1,
                _ => 0,
            };
            
            var y = vertical switch
            {
                DescriptionAlignment.Start => 0,
                DescriptionAlignment.Center => 0.5f,
                DescriptionAlignment.End => 1,
                _ => 0,
            };
            
            _container.pivot = new Vector2(x, y);
        }
    }
}