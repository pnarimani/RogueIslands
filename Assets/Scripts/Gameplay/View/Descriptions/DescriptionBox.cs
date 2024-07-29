using TMPro;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class DescriptionBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private GameObject _nameBg;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private RectTransform _container;
        
        public void SetDescription(string value) => _desc.text = value;
        
        public void ShowName(string name)
        {
            _nameBg.SetActive(true);
            _name.text = name;
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