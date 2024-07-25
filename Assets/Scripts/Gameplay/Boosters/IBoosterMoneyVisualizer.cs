using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterMoneyVisualizer
    {
        void Play(int moneyChange);
        void ShowDryRunMoney(Dictionary<int,int> moneyAndCount);
        void HideDryRun();
    }
}