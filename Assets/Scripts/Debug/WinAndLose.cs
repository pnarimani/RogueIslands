using IngameDebugConsole;
using RogueIslands.View;
using RogueIslands.View.DeckBuilding;

namespace RogueIslands.Debug
{
    public static class WinAndLose
    {
        [ConsoleMethod("win_round", "Win the round")]
        public static void RoundWin()
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