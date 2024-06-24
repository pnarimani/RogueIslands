using System;
using DG.Tweening;
using RogueIslands.View.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.Win
{
    public class RoundWinScreen : MonoBehaviour, IWeekWinScreen
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
                GameManager.Instance.State.ClaimRoundEndMoney(GameManager.Instance);
                Destroy(gameObject);
            });
        }

        private void Start()
        {
            StaticResolver.Resolve<IStageAudio>().PlayRoundWin();
        }

        private void SetTotalMoneyText()
        {
            _totalMoneyText.text = $"Total: ${_totalChange}";
        }

        public void AddMoneyChange(MoneyChange change)
        {
            _totalChange += change.Change;
            SetTotalMoneyText();
            
            var moneyChangeView = Instantiate(_moneyChangePrefab, _moneyChangeParent);
            moneyChangeView.SetChange(change.Change);
            moneyChangeView.SetReason(change.Reason);

            var t = moneyChangeView.transform;
            t.localScale = Vector3.zero;
            t.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.1f * _moneyChangeParent.childCount);
        }
    }
}