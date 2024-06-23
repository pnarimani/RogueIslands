using System;
using UnityEngine;

namespace RogueIslands.View.RoundSelection
{
    public class RoundSelectionScreen : MonoBehaviour
    {
        [SerializeField] private RoundView _roundPrefab;
        [SerializeField] private Transform _roundParent;

        private void Start()
        {
            var state = GameManager.Instance.State;

            for (var round = 0; round < GameState.RoundsPerAct; round++)
            {
                var roundView = Instantiate(_roundPrefab, _roundParent);
                roundView.SetRound(round + 1);
                roundView.SetRequiredScore(state.GetRequiredScore(state.Act, round));

                roundView.ActivatePlayButton(round == state.Round);
                if (round == state.Round)
                    roundView.SetPlayButtonAction(PlayRound);
            }
        }

        private void PlayRound()
        {
            GameManager.Instance.State.StartRound(GameManager.Instance);

            Destroy(gameObject);
        }
    }
}