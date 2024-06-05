using JetBrains.Annotations;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IGameView
    {
        [CanBeNull]
        IBuildingView GetBuilding(Building building);
        
        [CanBeNull]
        IBoosterView GetBooster(Booster booster);
        
        void HighlightIsland(Island island);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        void ShowWeekWin();
        void DestroyBuildings();
        void AddBooster(Booster instance);
        void RemoveBooster(Booster booster);
        void ShowBuildingsInHand();
    }
}