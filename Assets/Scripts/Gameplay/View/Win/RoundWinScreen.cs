using DG.Tweening;
using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View.Win
{
    public class RoundWinScreen : MonoBehaviour, IWeekWinScreen, IWindow
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
                StaticResolver.Resolve<RoundController>().ClaimRoundEndMoney();
                Destroy(gameObject);
            });
        }

        private void Start()
        {
            GameUI.Instance.ShowScoringPanel(false);
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
                .SetDelay(0.2f * (_moneyChangeParent.childCount + 1));
        }
    }
}