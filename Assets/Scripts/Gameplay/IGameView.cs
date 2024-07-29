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
        IBoosterView GetBooster(BoosterInstanceId boosterId);

        void ShowLoseScreen();
        void ShowGameWinScreen();
        IRoundWinScreen ShowRoundWin();
        void AddBooster(IBooster instance);
        IGameUI GetUI();
        void SpawnBuilding(Building building);
        void ShowShopScreen();
        IDeckBuildingView GetDeckBuildingView();
        void DestroyAllBuildings();
    }
}