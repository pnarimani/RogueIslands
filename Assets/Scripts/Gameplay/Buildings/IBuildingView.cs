namespace RogueIslands.Gameplay.Buildings
{
    public interface IBuildingView
    {
        void BuildingTriggered(int count);
        void BonusTriggered(int count);
    }
}