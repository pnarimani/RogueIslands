using TMPro;
using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name, _description;
        [SerializeField] private RectTransform _animationParent;
        
        public Building Data { get; private set; }

        public void Show(Building data)
        {
            Data = data;
            
            _name.text = data.Name;
            _description.text = data.Description;
        }
    }
}