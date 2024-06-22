using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Boosters;
using RogueIslands.Particles;
using RogueIslands.Rollback;
using RogueIslands.View.Boosters;
using RogueIslands.View.Shop;
using RogueIslands.View.Win;
using UnityEngine;

namespace RogueIslands.View
{
    public class GameManager : SingletonMonoBehaviour<GameManager>, IGameView, IDisposable
    {
        [SerializeField] private ShopScreen _shopPrefab;
        [SerializeField] private WeekWinScreen _weekWinScreen;
        [SerializeField] private LoseScreen _loseScreen;
        private PlayController _playController;

        public GameState State { get; private set; }
        public bool IsPlaying { get; private set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;

            ShowBuildingsInHand();

            GameUI.Instance.RefreshAll();
        }

        private async void OnPlayClicked()
        {
            if (!State.CanPlay() || IsPlaying)
                return;

            IsPlaying = true;

            AnimationScheduler.ResetTime();

            _playController.Play();

            var timer = 0f;
            while (timer < AnimationScheduler.GetExtraTime())
            {
                await UniTask.DelayFrame(1);
                timer += Time.deltaTime;
            }

            var particleTargets = FindObjectsOfType<ParticleSystemTarget>();
            while (particleTargets.Any(t => t.IsTrackingParticles()))
            {
                await UniTask.DelayFrame(1);
            }

            GameUI.Instance.RefreshScores();

            _playController.ProcessScore();

            IsPlaying = false;
        }

        public void ShowGameWinScreen()
        {
        }

        public IWeekWinScreen ShowWeekWin()
        {
            return Instantiate(_weekWinScreen);
        }

        public void DestroyBuildings()
        {
            foreach (var building in FindObjectsOfType<BuildingView>())
                Destroy(building.gameObject);
        }

        public void AddBooster(IBooster instance)
        {
            if (instance is BoosterCard card)
            {
                GameUI.Instance.ShowBoosterCard(card);
                GameUI.Instance.RefreshDate();
                GameUI.Instance.RefreshMoneyAndEnergy();
            }
            else if (instance is WorldBooster world)
            {
                var booster = Instantiate(Resources.Load<BoosterView>(world.PrefabAddress), world.Position,
                    world.Rotation);
                booster.Initialize(world);
            }
        }

        public void ShowBuildingsInHand()
        {
            foreach (var v in FindObjectsOfType<BuildingCardView>())
                Destroy(v.gameObject);

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);
        }

        public IGameUI GetUI() => GameUI.Instance;

        public void SpawnBuilding(Building data)
        {
            var building = Instantiate(Resources.Load<BuildingView>(data.PrefabAddress), data.Position, data.Rotation);
            building.transform.position += Vector3.up;
            building.transform.DOMove(data.Position, 0.3f)
                .SetEase(Ease.OutBounce);
            building.Initialize(data);
        }

        public void ShowLoseScreen()
        {
            Instantiate(_loseScreen);
        }

        public IBuildingView GetBuilding(Building building)
            => FindObjectsOfType<BuildingView>().First(b => b.Data == building);

        public IBoosterView GetBooster(IBooster booster)
            => FindObjectsByType<BoosterView>(FindObjectsSortMode.None).FirstOrDefault(b => b.Data == booster);

        public async void HighlightIsland(Cluster cluster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);

            foreach (var building in cluster)
            {
                foreach (var view in FindObjectsOfType<BuildingView>())
                {
                    if (view.Data != building)
                        continue;

                    view.transform.DOLocalMoveY(0.2f, 0.2f)
                        .SetEase(Ease.OutBack);
                    break;
                }
            }
        }

        public async void LowlightIsland(Cluster cluster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);

            foreach (var building in cluster)
            {
                foreach (var view in FindObjectsOfType<BuildingView>())
                {
                    if (view.Data != building)
                        continue;

                    view.transform.DOLocalMoveY(-0.2f, 0.2f)
                        .SetEase(Ease.OutBounce);
                    break;
                }
            }
        }

        public void ShowShopScreen()
        {
            Instantiate(_shopPrefab);
        }

        public IReadOnlyList<Vector3> GetWorldBoosterPositions()
            => FindObjectsOfType<WorldBoosterSpawnPoint>().Select(booster => booster.transform.position).ToList();

        public void Initialize(GameState state, PlayController playController)
        {
            _playController = playController;
            State = state;
            State.RestoreProperties();
            State.SpawnWorldBoosters(this, GetWorldBoosterPositions());
        }

        public void Dispose()
        {
            if (this != null && gameObject != null)
                Destroy(gameObject);
        }

        public Bounds GetBounds(Building buildingData)
        {
            return FindObjectsOfType<BuildingView>().First(b => b.Data == buildingData).transform.GetCollisionBounds();
        }

        public Bounds GetBounds(WorldBooster worldBooster)
        {
            return FindObjectsOfType<BoosterView>().First(b => b.Data == worldBooster).transform.GetCollisionBounds();
        }
    }
}