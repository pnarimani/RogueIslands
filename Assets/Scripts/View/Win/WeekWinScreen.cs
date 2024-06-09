using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.Win
{
    public class WeekWinScreen : MonoBehaviour, IWeekWinScreen
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _totalMoneyText, _weeklyPayoutText;
        [SerializeField] private Transform _moneyChangeParent;
        [SerializeField] private MoneyChangeView _moneyChangePrefab;
        
        private int _totalChange;

        private void Awake()
        {
            _nextButton.onClick.AddListener(() =>
            {
                GameManager.Instance.State.ClaimWeekEndMoney(GameManager.Instance);
                Destroy(gameObject);
            });
        }

        public void SetWeeklyPayout(int money)
        {
            _totalChange += money;
            SetTotalMoneyText();
            _weeklyPayoutText.text = money.ToString();
        }

        private void SetTotalMoneyText()
        {
            _totalMoneyText.text = $"${_totalChange}";
        }

        public void AddMoneyChange(MoneyChange change)
        {
            _totalChange += change.Change;
            SetTotalMoneyText();
            
            var moneyChangeView = Instantiate(_moneyChangePrefab, _moneyChangeParent);
            moneyChangeView.SetChange(change.Change);
            moneyChangeView.SetReason(change.Reason);
        }
    }
}