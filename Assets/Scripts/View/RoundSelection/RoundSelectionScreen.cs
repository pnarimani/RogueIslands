using System;
using DG.Tweening;
using RogueIslands.View.DeckBuilding;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.View.RoundSelection
{
    public class RoundSelectionScreen : MonoBehaviour
    {
        [SerializeField] private RoundView _roundPrefab;
        [SerializeField] private Transform _roundParent;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Button _back;
        
        private void Start()
        {
            var state = GameManager.Instance.State;
            
            _title.text = $"Act {state.Act + 1}";
            _back.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

            for (var round = 0; round < GameState.RoundsPerAct; round++)
            {
                var roundView = Instantiate(_roundPrefab, _roundParent);
                roundView.SetRound(round + 1);
                roundView.SetRequiredScore(state.GetRequiredScore(state.Act, round));

                roundView.ActivatePlayButton(round == state.Round);
                if (round == state.Round)
                    roundView.SetPlayButtonAction(PlayRound);

                roundView.transform.DOScale(0, 0.3f)
                    .From()
                    .SetEase(Ease.OutBack)
                    .SetDelay(round * 0.05f);
            }
        }

        private void PlayRound()
        {
            GameManager.Instance.State.StartRound(GameManager.Instance);

            Destroy(gameObject);
        }
    }
}