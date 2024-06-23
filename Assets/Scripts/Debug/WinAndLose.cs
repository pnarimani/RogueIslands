using IngameDebugConsole;
using RogueIslands.View;

namespace RogueIslands.Debug
{
    public static class WinAndLose
    {
        [ConsoleMethod("win_week", "Win the week")]
        public static void WeekWin()
        {
            GameManager.Instance.State.CurrentScore = GameManager.Instance.State.GetCurrentRequiredScore();
            GameManager.Instance.State.ProcessScore(GameManager.Instance);
        }
        
        [ConsoleMethod("lose", "Lose")]
        public static void Lose()
        {
            GameManager.Instance.State.CurrentScore = 0;
            GameManager.Instance.State.Day = GameManager.Instance.State.TotalDays;
            GameManager.Instance.State.ProcessScore(GameManager.Instance);
        }
    }
}