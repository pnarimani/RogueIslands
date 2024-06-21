using System.Globalization;
using RogueIslands.View.Feedbacks;
using TMPro;
using UnityEngine;

namespace RogueIslands.View
{
    public class NumberText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _format;
        [SerializeField] private bool _hasMax;
        [SerializeField] private double _max;
        [SerializeField] private NumberTextIncreaseFeedback _increaseFeedback;
        [SerializeField] private NumberTextDecreaseFeedback _decreaseFeedback;
        

        private double _currentNumber;

        public double CurrentNumber => _currentNumber;

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
            if (number > CurrentNumber)
            {
                _increaseFeedback.Play();
            }
            else if (number < CurrentNumber)
            {
                _decreaseFeedback.Play();
            }

            SetNumber(number);
        }

        public void SetMax(double max)
        {
            _hasMax = true;
            _max = max;
            SetNumber(CurrentNumber);
        }
    }
}