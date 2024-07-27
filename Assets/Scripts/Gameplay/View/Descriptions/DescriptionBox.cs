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

        public void SetGrowToBottom(bool growToBottom)
        {
            _container.pivot = growToBottom ? new Vector2(0.5f, 1) : new Vector2(0.5f, 0);
        }
    }
}