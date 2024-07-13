using IngameDebugConsole;
using UnityEngine.SceneManagement;

namespace RogueIslands.Debug
{
    public static class Quit
    {
        [ConsoleMethod("quit", "Quits the run")]
        public static void QuitRun()
        {
            SceneManager.LoadScene(0);
        }
    }
}