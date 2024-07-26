using System.Threading;
using Cysharp.Threading.Tasks;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.View.Audio;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class PlayButtonHandler : Singleton<PlayButtonHandler>
    {
        private readonly BuildingPlacement _buildingPlacement;
        private readonly RoundController _roundController;
        public bool IsPlaying { get; private set; }

        public PlayButtonHandler(BuildingPlacement buildingPlacement, RoundController roundController)
        {
            _roundController = roundController;
            _buildingPlacement = buildingPlacement;
        }

        public async UniTask PlaceBuildingDown(Building building, Vector3 position, Quaternion rotation, CancellationToken destroyCancellationToken)
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            
            _buildingPlacement.PlaceBuilding(building, position, rotation);

            var increaseRate = 0.1f;
            var timer = 0f;
            while (timer < AnimationScheduler.GetTotalTime())
            {
                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
                timer += Time.deltaTime;

                Time.timeScale += increaseRate * Time.deltaTime;
            }
            
            destroyCancellationToken.ThrowIfCancellationRequested();

            GameUI.Instance.RefreshScores();

            BuildingView.TriggerCount = 0;
            StaticResolver.Resolve<IScoringAudio>().PlayScoringFinished();
            
            _roundController.TryEndingRound();
            
            Time.timeScale = 1;
            
            GameUI.Instance.ShowDeck();
            
            IsPlaying = false;
        }
    }
}