using System;
using Cysharp.Threading.Tasks;
using IngameDebugConsole;
using RogueIslands.Autofac;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.View;

namespace RogueIslands.Debug
{
    public static class WinAndLose
    {
        [ConsoleMethod("win_round", "Win the round")]
        public static void RoundWin()
        {
            GameManager.Instance.State.CurrentScore = GameManager.Instance.State.GetCurrentRequiredScore();
            StaticResolver.Resolve<RoundController>().TryEndingRound();
        }
        
        [ConsoleMethod("lose", "Lose")]
        public static void Lose()
        {
            GameManager.Instance.State.CurrentScore = 0;
            GameManager.Instance.State.Buildings.Deck.Clear();
            StaticResolver.Resolve<RoundController>().TryEndingRound();
        }
    }
}