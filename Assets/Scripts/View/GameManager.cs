using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using RogueIslands.View.Shop;
using UnityEngine;

namespace RogueIslands.View
{
    public class GameManager : Singleton<GameManager>, IGameView
    {
        [SerializeField] private ShopScreen _shopPrefab;
        
        public GameState State { get; private set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;

            State = GameFactory.NewGame("B");
            
            ShowBuildingsInHand();
            
            GameUI.Instance.RefreshAll();
        }

        private async void OnPlayClicked()
        {
            if (!State.CanPlay())
                return;
            
            AnimationScheduler.ResetTime();
            
            State.Play(this);

            var timer = 0f;
            while (timer < AnimationScheduler.GetExtraTime())
            {
                await UniTask.DelayFrame(1);
                timer += Time.deltaTime;
            }
            
            GameUI.Instance.RefreshScores();

            State.ProcessScore(this);
        }

        public void ShowGameWinScreen()
        {
            
        }

        public async void ShowWeekWin()
        {
            await UniTask.WaitForSeconds(1);
            Instantiate(_shopPrefab);
        }

        public void DestroyBuildings()
        {
            foreach (var building in FindObjectsOfType<BuildingView>())
                Destroy(building.gameObject);
        }

        public void AddBooster(Booster instance)
        {
            GameUI.Instance.ShowBoosterCard(instance);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoneyAndEnergy();
        }

        public void RemoveBooster(Booster booster)
        {
            GameUI.Instance.RemoveBoosterCard(booster);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoneyAndEnergy();
        }

        public void ShowBuildingsInHand()
        {
            foreach (var v in FindObjectsOfType<BuildingCardView>()) 
                Destroy(v.gameObject);

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);
        }

        public IGameUI GetUI() => GameUI.Instance;

        public void ShowLoseScreen()
        {
        }

        public IBuildingView GetBuilding(Building building)
            => FindObjectsOfType<BuildingView>().First(b => b.Data == building);

        public IBoosterView GetBooster(Booster booster) 
            => GameUI.Instance.GetBoosterCard(booster);

        public void HighlightIsland(Island island)
        {
        }
    }
}