using System.Collections.Generic;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster(BoosterCard booster);
        
        void HighlightIsland(Island island);
        void LowlightIsland(Island island);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        IWeekWinScreen ShowWeekWin();
        void DestroyBuildings();
        void AddBooster(BoosterCard instance);
        void RemoveBooster(BoosterCard booster);
        void ShowBuildingsInHand();
        IGameUI GetUI();
        void SpawnBuilding(Building building);
        void ShowShopScreen();

        IReadOnlyList<IWorldBooster> GetWorldBoosters();
    }
}