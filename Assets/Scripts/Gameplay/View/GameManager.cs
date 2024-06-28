using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Boosters;
using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.Gameplay.View.DeckPreview;
using RogueIslands.Gameplay.View.Lose;
using RogueIslands.Gameplay.View.RoundSelection;
using RogueIslands.Gameplay.View.Shop;
using RogueIslands.UISystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;

namespace RogueIslands.Gameplay.View
{
    public class GameManager : Singleton<GameManager>, IGameView
    {
        private readonly IWindowOpener _windowOpener;

        public GameState State { get; private set; }

        public GameManager(GameState state, IWindowOpener windowOpener)
        {
            _windowOpener = windowOpener;
            _windowOpener.Open<GameUI>();
            State = state;
            ShowRoundsSelectionScreen();
        }


        public void ShowGameWinScreen()
        {
            _windowOpener.Open<GameWinScreen>();
        }

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
                GameUI.Instance.RefreshDate();
                GameUI.Instance.RefreshMoney();
            }
            else if (instance is WorldBooster world)
            {
                var booster = Object.Instantiate(Resources.Load<WorldBoosterView>(world.PrefabAddress), world.Position,
                    world.Rotation);
                booster.Initialize(world);
            }
        }

        public void ShowBuildingsInHand()
        {
            foreach (var v in ObjectRegistry.GetBuildingCards())
                Object.Destroy(v.gameObject);

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);

            GameUI.Instance.RefreshDeckText();
        }

        public IGameUI GetUI() => GameUI.Instance;

        public void SpawnBuilding(Building data)
        {
            var building = Object.Instantiate(Resources.Load<BuildingView>(data.PrefabAddress), data.Position,
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

        public Bounds GetBounds(Building buildingData)
        {
            return ObjectRegistry.GetBuildings().First(b => b.Data == buildingData).transform.GetCollisionBounds();
        }

        public Bounds GetBounds(WorldBooster worldBooster)
        {
            return ObjectRegistry.GetBoosters().First(b => Equals(b.Data, worldBooster)).transform.GetCollisionBounds();
        }

        public void ShowRoundsSelectionScreen() => _windowOpener.Open<RoundSelectionScreen>();
        public void ShowDeckPreview() => _windowOpener.Open<DeckPreviewScreen>();

        public bool TryGetWorldBoosterSpawnPoint(WorldBooster blueprint, ref Random positionRandom,
            out Vector3 point) =>
            WorldBoosterSpawnPointProvider.TryGet(blueprint, ref positionRandom, out point);

        public IDeckBuildingView GetDeckBuildingView() => DeckBuildingView.Instance;

        public void DestroyBuildingsInHand()
        {
            foreach (var v in ObjectRegistry.GetBuildingCards())
                Object.Destroy(v.gameObject);
        }

        public void ShowSettingsPopup() => _windowOpener.Open<SettingsPopup>();

        public void ShowOptions() => _windowOpener.Open<OptionsPopup>();
    }
}