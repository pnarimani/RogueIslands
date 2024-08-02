using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class SelectCardActionExecutor : GameActionExecutor<SelectCardAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            SelectCardAction action)
        {
            var rand = state.GetRandomForType<SelectCardAction>().ForAct(state.Act);
            action.SelectedCard = state.BuildingsInHand.ToArray().SelectRandom(rand);
        }
    }
}