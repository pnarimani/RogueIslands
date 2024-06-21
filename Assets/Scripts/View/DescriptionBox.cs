using TMPro;
using UnityEngine;

namespace RogueIslands.View
{
    public class DescriptionBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _desc;

        public void SetDescription(string value) => _desc.text = value;
    }
}