using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IGameView
    {
        IBuildingView GetBuilding(Building building);
        IBoosterView GetBooster();
        void HighlightIsland(Island island);
        void ShowLoseScreen();
        void ShowGameWinScreen();
        void ShowWeekWin();
    }
}