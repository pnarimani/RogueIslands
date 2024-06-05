using System;
using System.Linq;
using Cysharp.Threading.Tasks;
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

            State = GameFactory.NewGame("A");

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);
            
            GameUI.Instance.Refresh();
        }

        private async void OnPlayClicked()
        {
            AnimationScheduler.ResetTime();
            
            State.Play(this);

            var timer = 0f;
            while (timer < AnimationScheduler.GetExtraTime())
            {
                await UniTask.DelayFrame(1);
                timer += Time.deltaTime;
            }

            State.ProcessScore(this);
            
            GameUI.Instance.Refresh();
        }

        public void ShowGameWinScreen()
        {
            
        }

        public void ShowWeekWin()
        {
            Instantiate(_shopPrefab);
        }

        public void ShowLoseScreen()
        {
        }

        public IBuildingView GetBuilding(Building building)
            => FindObjectsOfType<BuildingView>().First(b => b.Data == building);

        public IBoosterView GetBooster()
        {
            throw new NotImplementedException();
        }

        public void HighlightIsland(Island island)
        {
        }
    }
}