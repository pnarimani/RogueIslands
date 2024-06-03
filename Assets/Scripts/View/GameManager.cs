using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View
{
    public class GameManager : Singleton<GameManager>, IGameView
    {
        public GameState State { get; private set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;

            State = GameFactory.NewGame();

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);
        }

        private async void OnPlayClicked()
        {
            State.Play(this);

            await UniTask.WaitForSeconds(AnimationScheduler.GetTotalTime());

            if(State.HasLost())
                ShowLoseScreen();
            
            if (State.IsWeekFinished())
            {
                State.Win(this);
                
                if (State.Result == GameResult.Win)
                    ShowGameWinScreen();
                else
                    ShowWeekWin();
            }
        }

        private void ShowGameWinScreen()
        {
            
        }

        private void ShowWeekWin()
        {
        }

        private void ShowLoseScreen()
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