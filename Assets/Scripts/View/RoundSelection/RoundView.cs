using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.RoundSelection
{
    public class RoundView : MonoBehaviour
    {
        [SerializeField] private NumberText _requiredScore;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Color _activeRoundColor = Color.white;
        [SerializeField] private Color _inactiveRoundColor = Color.gray;
        [SerializeField] private Image _background;

        public void SetPlayButtonAction(System.Action action)
        {
            _playButton.onClick.AddListener(() => action?.Invoke());
        }

        public void SetRound(int round)
        {
            _title.text = $"Round {round}";
        }

        public void SetRequiredScore(double score)
        {
            _requiredScore.SetNumber(score);
        }

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void ActivatePlayButton(bool active)
        {
            _playButton.gameObject.SetActive(active);
            _background.color = active ? _activeRoundColor : _inactiveRoundColor;
        }
    }
}