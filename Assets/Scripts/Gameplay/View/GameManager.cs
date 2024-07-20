using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Assets;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Rand;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.Gameplay.View.DeckPreview;
using RogueIslands.Gameplay.View.Lose;
using RogueIslands.Gameplay.View.RoundSelection;
using RogueIslands.Gameplay.View.Shop;
using RogueIslands.UISystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RogueIslands.Gameplay.View
{
    public class GameManager : Singleton<GameManager>, IGameView
    {
        private readonly IWindowOpener _windowOpener;
        private readonly IAssetLoader _assetLoader;

        public GameState State { get; private set; }

        public GameManager(GameState state, IWindowOpener windowOpener, IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
            _windowOpener = windowOpener;
            State = state;
        }

        public void ShowGameWinScreen() => _windowOpener.Open<GameWinScreen>();

        public IWeekWinScreen ShowRoundWin()
        {
            return _windowOpener.Open<IWeekWinScreen>();
        }

        public void DestroyBuildings()
        {
            foreach (var building in ObjectRegistry.GetBuildings())
                Object.Destroy(building.gameObject);
        }

        public void AddBooster(IBooster instance)
        {
            if (instance is BoosterCard card)
            {
                GameUI.Instance.ShowBoosterCard(card);
            }
            else if (instance is WorldBooster world)
            {
                var booster = Object.Instantiate(_assetLoader.Load<WorldBoosterView>(world.PrefabAddress),
                    world.Position,
                    world.Rotation);
                booster.Initialize(world);
            }
        }

        public IGameUI GetUI() => GameUI.Instance;

        public void SpawnBuilding(Building data)
        {
            var building = Object.Instantiate(_assetLoader.Load<BuildingView>(data.PrefabAddress), data.Position,
                data.Rotation);
            building.transform.position += Vector3.up;
            building.transform.DOMove(data.Position, 0.3f)
                .SetEase(Ease.OutBounce);
            building.Initialize(data);
        }

        public void ShowLoseScreen() => _windowOpener.Open<LoseScreen>();

        public IBuildingView GetBuilding(Building building)
            => ObjectRegistry.GetBuildings().First(b => b.Data == building);

        public IBoosterView GetBooster(IBooster booster)
            => ObjectRegistry.GetBoosters().FirstOrDefault(b => Equals(b.Data, booster));

        public async void HighlightIsland(List<Building> cluster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);

            foreach (var building in cluster)
            {
                foreach (var view in ObjectRegistry.GetBuildings())
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
                foreach (var view in ObjectRegistry.GetBuildings())
                {
                    if (view.Data != building)
                        continue;

                    view.transform.DOLocalMoveY(-0.2f, 0.2f)
                        .SetEase(Ease.OutBounce);
                    break;
                }
            }
        }

        public void ShowShopScreen() => _windowOpener.Open<ShopScreen>();

        public void DestroyWorldBoosters()
        {
            foreach (var booster in ObjectRegistry.GetBoosters())
            {
                if (booster.Data is WorldBooster)
                    Object.Destroy(booster.gameObject);
            }
        }

        public void ShowDeckPreview() => _windowOpener.Open<DeckPreviewScreen>();

        public bool TryGetWorldBoosterSpawnPoint(WorldBooster blueprint, RogueRandom positionRandom,
            out Vector3 point) =>
            WorldBoosterSpawnPointProvider.TryGet(blueprint, positionRandom.ForAct(State.Act), out point);

        public IDeckBuildingView GetDeckBuildingView() => DeckBuildingView.Instance;

        public async void CheckForRoundEnd()
        {
            var timer = 0f;
            while (timer < AnimationScheduler.GetTotalTime())
            {
                await UniTask.DelayFrame(1);
                timer += Time.deltaTime;
            }
            
            GameUI.Instance.RefreshScores();
            StaticResolver.Resolve<RoundController>().TryEndingRound();
            GameUI.Instance.RefreshScores();
        }

        public void DestroyBuildingsInHand()
        {
            foreach (var v in ObjectRegistry.GetBuildingCards())
                Object.Destroy(v.gameObject);
        }

        public bool IsOverlapping(Building building, WorldBooster worldBooster)
        {
            return false;
        }

        public void ShowSettingsPopup() => _windowOpener.Open<SettingsPopup>();

        public void ShowOptions() => _windowOpener.Open<OptionsPopup>();
    }
}