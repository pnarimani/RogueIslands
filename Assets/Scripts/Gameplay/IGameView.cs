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
        
        void HighlightIsland(List<Building> cluster);
        void LowlightIsland(List<Building> cluster);
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
        void DestroyWorldBoosters();
        bool TryGetWorldBoosterSpawnPoint(WorldBooster blueprint, RogueRandom positionRandom, out Vector3 point);
        IDeckBuildingView GetDeckBuildingView();
        void DestroyBuildingsInHand();
    }
}