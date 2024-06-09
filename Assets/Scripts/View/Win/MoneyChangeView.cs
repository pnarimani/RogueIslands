using TMPro;
using UnityEngine;

namespace RogueIslands.View.Win
{
    public class MoneyChangeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _changeText;
        [SerializeField] private TextMeshProUGUI _reasonText;

        public void SetChange(int change)
        {
            _changeText.text = $"${change}";
        }

        public void SetReason(string reason)
        {
            _reasonText.text = reason;
        }
    }
}