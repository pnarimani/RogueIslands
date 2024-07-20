using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.View.RoundSelection;
using RogueIslands.UISystem;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class GameplayStartupWindowOpener : MonoBehaviour
    {
        public void Start()
        {
            var windowOpener = StaticResolver.Resolve<IWindowOpener>();
            windowOpener.Open<GameUI>();
            
            StaticResolver.Resolve<RoundController>().StartRound();
        }
    }
}