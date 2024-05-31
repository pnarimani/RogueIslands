using System;
using UnityEngine;

namespace RogueIslands.View
{
    public class GameManager : MonoBehaviour
    {
        public GameState State { get; set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;
            
            State = GameFactory.NewGame();
            
            foreach (var b in State.BuildingsInHand) 
                GameUI.Instance.ShowBuildingCard(b);
        }
        
        private void OnPlayClicked()
        {
            State.Play();
        }
    }
}