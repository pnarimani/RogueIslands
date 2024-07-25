using System.Collections.Generic;

namespace RogueIslands.Gameplay.Buildings
{
    public interface IBuildingView
    {
        void BuildingTriggered(int score);
        void BonusTriggered(int score);

        void ShowDryRunTrigger(Dictionary<int, int> triggerAndCount);
        void ShowDryRunBonus(Dictionary<int, int> bonusAndCount);
        void HideAllDryRunLabels();
    }
}