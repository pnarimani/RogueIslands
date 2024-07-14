using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class PlayButtonHandler : Singleton<PlayButtonHandler>
    {
        private readonly PlayController _playController;
        private readonly RoundController _roundController;
        public bool IsPlaying { get; private set; }

        public PlayButtonHandler(PlayController playController, RoundController roundController)
        {
            _roundController = roundController;
            _playController = playController;
        }

        public async UniTask OnPlayClicked(CancellationToken destroyCancellationToken)
        {
            if (!_playController.CanPlay() || IsPlaying)
                return;

            IsPlaying = true;

            AnimationScheduler.ResetTime();

            _playController.Play();

            var timer = 0f;
            while (timer < AnimationScheduler.GetTotalTime())
            {
                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                timer += Time.deltaTime;
            }

            destroyCancellationToken.ThrowIfCancellationRequested();

            GameUI.Instance.RefreshScores();
            
            AnimationScheduler.ResetTime();

            _roundController.TryEndingRound();

            IsPlaying = false;
        }
    }
}