using System.Globalization;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace RogueIslands
{
    public class NumberText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _format;
        [SerializeField] private bool _hasMax;
        [SerializeField] private double _max;
        [SerializeField] private MMF_Player _increaseFx, _decreaseFx;

        private double _currentNumber;

        public void SetNumber(double number)
        {
            _currentNumber = number;

            if (_hasMax)
            {
                _text.text = string.Format(_format, number, _max);
            }
            else
            {
                _text.text = string.IsNullOrEmpty(_format)
                    ? number.ToString(CultureInfo.InvariantCulture)
                    : string.Format(_format, number);
            }
        }

        public void UpdateNumber(double number)
        {
            if (number > _currentNumber)
            {
                _increaseFx?.PlayFeedbacks();
            }
            else if (number < _currentNumber)
            {
                _decreaseFx?.PlayFeedbacks();
            }

            SetNumber(number);
        }

        public void SetMax(double max)
        {
            _hasMax = true;
            _max = max;
            SetNumber(_currentNumber);
        }
    }
}