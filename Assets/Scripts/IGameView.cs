using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster(Booster booster);
        
        void HighlightIsland(Island island);
        void LowlightIsland(Island island);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        void ShowWeekWin();
        void DestroyBuildings();
        void AddBooster(Booster instance);
        void RemoveBooster(Booster booster);
        void ShowBuildingsInHand();
        IGameUI GetUI();
        void SpawnBuilding(Building building);
    }
}