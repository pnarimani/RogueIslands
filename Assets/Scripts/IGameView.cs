using System.Collections.Generic;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster(IBooster booster);
        
        void HighlightIsland(Island island);
        void LowlightIsland(Island island);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        IWeekWinScreen ShowWeekWin();
        void DestroyBuildings();
        void AddBooster(IBooster instance);
        void ShowBuildingsInHand();
        IGameUI GetUI();
        void SpawnBuilding(Building building);
        void ShowShopScreen();
        Bounds GetBounds(Building buildingData);
        Bounds GetBounds(WorldBooster worldBooster);
    }
}