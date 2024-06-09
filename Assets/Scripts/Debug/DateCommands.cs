using IngameDebugConsole;
using RogueIslands.View;

namespace RogueIslands.Debug
{
    public static class DateCommands
    {
        [ConsoleMethod("set_date", "Set the date to the specified month and week. Usage: set_date <month> <week>")]
        public static void SetDate(int month, int week)
        {
            GameManager.Instance.State.Month = month;
            GameManager.Instance.State.Week = week;
            GameManager.Instance.GetUI().RefreshAll();
        }
        
        [ConsoleMethod("set_rem_days", "Set the remaining days to the specified value. Usage: set_rem_days <days>")]
        public static void SetRemainingDays(int remainingDays)
        {
            if(remainingDays > GameManager.Instance.State.TotalDays)
                remainingDays = GameManager.Instance.State.TotalDays;
            GameManager.Instance.State.Day = GameManager.Instance.State.TotalDays - remainingDays;
            GameManager.Instance.GetUI().RefreshAll();
        }
        
        [ConsoleMethod("set_energy", "Set the energy to the specified value. Usage: set_energy <energy>")]
        public static void SetEnergy(int energy)
        {
            GameManager.Instance.State.Energy = energy;
            GameManager.Instance.GetUI().RefreshAll();
        }
        
        [ConsoleMethod("set_money", "Set the money to the specified value. Usage: set_money <money>")]
        public static void SetMoney(int money)
        {
            GameManager.Instance.State.Money = money;
            GameManager.Instance.GetUI().RefreshAll();
        }
    }
}