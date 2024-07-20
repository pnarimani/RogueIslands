using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Rand;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster(IBooster booster);
        
        void ShowLoseScreen();
        void ShowGameWinScreen();
        IWeekWinScreen ShowRoundWin();
        void AddBooster(IBooster instance);
        IGameUI GetUI();
        void SpawnBuilding(Building building);
        void ShowShopScreen();
        IDeckBuildingView GetDeckBuildingView();
        void CheckForRoundEnd();
    }
}