using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using UnityEngine;

namespace RogueIslands
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster(IBooster booster);
        
        void HighlightIsland(Cluster cluster);
        void LowlightIsland(Cluster cluster);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        IWeekWinScreen ShowRoundWin();
        void DestroyBuildings();
        void AddBooster(IBooster instance);
        void ShowBuildingsInHand();
        IGameUI GetUI();
        void SpawnBuilding(Building building);
        void ShowShopScreen();
        Bounds GetBounds(Building buildingData);
        Bounds GetBounds(WorldBooster worldBooster);
        IReadOnlyList<Vector3> GetWorldBoosterPositions();
        void DestroyWorldBoosters();
    }
}