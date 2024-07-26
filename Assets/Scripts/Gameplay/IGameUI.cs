using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay
{
    public interface IGameUI
    {
        void RefreshDate();
        void RefreshMoney();
        void RefreshScores();
        void RefreshDeckText();
        void ShowBuildingCard(Building building);
        void ShowBuildingCardPeek(Building building);
        void MoveCardToHand(Building building);
        void RemoveCard(Building building);
        void ShowStageInformation();
    }
}