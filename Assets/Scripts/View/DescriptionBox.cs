using TMPro;
using UnityEngine;

namespace RogueIslands.View
{
    public class DescriptionBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private GameObject _nameBg;
        [SerializeField] private TextMeshProUGUI _name;

        public void SetDescription(string value) => _desc.text = value;
        
        public void ShowName(string name)
        {
            _nameBg.SetActive(true);
            _name.text = name;
        }
    }
}