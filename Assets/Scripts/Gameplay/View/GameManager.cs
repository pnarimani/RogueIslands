using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RogueIslands.Assets;
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

        public IRoundWinScreen ShowRoundWin()
        {
            return _windowOpener.Open<IRoundWinScreen>();
        }

        public void RemoveAllCardsFromHand()
        {
            foreach (var card in ObjectRegistry.GetBuildingCards())
                Object.Destroy(card.gameObject);
        }

        public void DestroyAllBuildings()
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
            => ObjectRegistry.GetBuildings().FirstOrDefault(b => b.Data.Id == building.Id);

        public IBoosterView GetBooster(BoosterInstanceId boosterId)
            => ObjectRegistry.GetBoosters().FirstOrDefault(b => b.Data.Id == boosterId);

        public void ShowShopScreen() => _windowOpener.Open<ShopScreen>();

        public void ShowDeckPreview() => _windowOpener.Open<DeckPreviewScreen>();

        public IDeckBuildingView GetDeckBuildingView() => DeckBuildingView.Instance;

        public void ShowSettingsPopup() => _windowOpener.Open<SettingsPopup>();

        public void ShowOptions() => _windowOpener.Open<OptionsPopup>();

        public void ShowCardPackSelectionScreen()
        {
            _windowOpener.Open<CardPackSelection>();
        }
    }
}