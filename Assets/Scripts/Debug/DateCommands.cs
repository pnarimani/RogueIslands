using IngameDebugConsole;
using RogueIslands.View;
using RogueIslands.View.DeckBuilding;

namespace RogueIslands.Debug
{
    public static class DateCommands
    {
        [ConsoleMethod("set_stage", "Set the stage to the specified act and round. Usage: set_stage <act> <round>")]
        public static void SetStage(int act, int round)
        {
            GameManager.Instance.State.Act = act;
            GameManager.Instance.State.Round = round;
            GameManager.Instance.GetUI().RefreshAll();
        }
        
        [ConsoleMethod("set_rem_days", "Set the remaining days to the specified value. Usage: set_rem_days <days>")]
        public static void SetRemainingDays(int remainingDays)
        {
            if(remainingDays > GameManager.Instance.State.TotalDays)
                GameManager.Instance.State.TotalDays = remainingDays;
            GameManager.Instance.State.Day = GameManager.Instance.State.TotalDays - remainingDays;
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