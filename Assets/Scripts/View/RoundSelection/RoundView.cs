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
        }
    }
}