using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.View.Boosters;
using RogueIslands.View.DeckPreview;
using RogueIslands.View.Lose;
using RogueIslands.View.RoundSelection;
using RogueIslands.View.Shop;
using RogueIslands.View.Win;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RogueIslands.View.DeckBuilding
{
    public class GameManager : SingletonMonoBehaviour<GameManager>, IGameView, IDisposable
    {
        [SerializeField] private ShopScreen _shopPrefab;
        [SerializeField] private RoundWinScreen _weekWinScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private RoundSelectionScreen _roundSelectionScreen;
        [SerializeField] private DeckPreviewScreen _deckPreviewScreen;
        [SerializeField] private OptionsPopup _optionsPopup;
        [SerializeField] private SettingsPopup _settingsPopup;
        [SerializeField] private GameWinScreen _gameWinScreen;
        
        private PlayController _playController;

        public GameState State { get; private set; }
        public bool IsPlaying { get; private set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;
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

            destroyCancellationToken.ThrowIfCancellationRequested();

            GameUI.Instance.RefreshScores();

            _playController.ProcessScore();

            IsPlaying = false;
        }

        public void ShowGameWinScreen()
        {
            Instantiate(_gameWinScreen);
        }

        public IWeekWinScreen ShowRoundWin()
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
                GameUI.Instance.RefreshMoney();
            }
            else if (instance is WorldBooster world)
            {
                var booster = Instantiate(Resources.Load<WorldBoosterView>(world.PrefabAddress), world.Position,
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
            
            GameUI.Instance.RefreshDeckText();
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
            => FindObjectsByType<BoosterView>(FindObjectsSortMode.None).FirstOrDefault(b => Equals(b.Data, booster));

        public async void HighlightIsland(List<Building> cluster)
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

        public async void LowlightIsland(List<Building> cluster)
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

        public void DestroyWorldBoosters()
        {
            foreach (var booster in FindObjectsOfType<BoosterView>())
            {
                if (booster.Data is WorldBooster)
                    Destroy(booster.gameObject);
            }
        }

        public void Initialize(GameState state, PlayController playController)
        {
            _playController = playController;
            State = state;
            ShowRoundsSelectionScreen();
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

        public void ShowRoundsSelectionScreen()
        {
            Instantiate(_roundSelectionScreen);
        }

        public void ShowDeckPreview()
        {
            Instantiate(_deckPreviewScreen);
        }

        public bool TryGetWorldBoosterSpawnPoint(WorldBooster blueprint, ref Random positionRandom, out Vector3 point) =>
            WorldBoosterSpawnPointProvider.TryGet(blueprint, ref positionRandom, out point);

        public IDeckBuildingView GetDeckBuildingView() => DeckBuildingView.Instance;

        public void ShowSettingsPopup()
        {
            Instantiate(_settingsPopup);
        }

        public void ShowOptions()
        {
            Instantiate(_optionsPopup);
        }
    }
}