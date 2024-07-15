using DG.Tweening;
using RogueIslands.DependencyInjection;
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
        [SerializeField] private Transform _bg;

        private int _totalChange;

        private void Awake()
        {
            _nextButton.onClick.AddListener(() =>
            {
                StaticResolver.Resolve<RoundController>().ClaimRoundEndMoney();
                
                _bg.DOLocalMoveY(-Screen.height, 0.5f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        GameManager.Instance.ShowShopScreen();
                        Destroy(gameObject);
                    });
            });
        }

        private void Start()
        {
            GameUI.Instance.ShowScoringPanel(false);
            StaticResolver.Resolve<IStageAudio>().PlayRoundWin();

            _bg.DOLocalMoveY(-Screen.height, 0.5f)
                .From()
                .SetEase(Ease.OutBack);
        }

        private void SetTotalMoneyText()
        {
            _totalMoneyText.text = $"Total: ${_totalChange}";
        }

        public void AddMoneyChange(MoneyChange change)
        {
            if (change.Change == 0)
                return;
            
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