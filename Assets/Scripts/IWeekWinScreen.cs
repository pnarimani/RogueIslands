namespace RogueIslands
{
    public interface IWeekWinScreen
    {
        void SetWeeklyPayout(int money);
        void AddMoneyChange(MoneyChange change);
    }
}