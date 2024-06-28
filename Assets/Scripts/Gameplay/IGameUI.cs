namespace RogueIslands.Gameplay
{
    public interface IGameUI
    {
        void RefreshScores();
        void RefreshDate();
        void RefreshAll();
        void RefreshMoney();
    }
}