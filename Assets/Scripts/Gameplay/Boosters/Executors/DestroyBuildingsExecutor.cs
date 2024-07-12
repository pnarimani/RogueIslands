using RogueIslands.Gameplay.Boosters.Actions;
using UnityEngine;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class DestroyBuildingsExecutor : GameActionExecutor<DestroyBuildingsAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            DestroyBuildingsAction action)
        {
            if (action.DestroyBuildingsInRange && booster is WorldBooster worldBooster)
            {
                for (var i = state.Buildings.Deck.Count - 1; i >= 0; i--)
                {
                    var building = state.Buildings.Deck[i];
                    if (!building.IsPlacedDown()) 
                        continue;
                    if (Vector3.Distance(building.Position, worldBooster.Position) > worldBooster.Range)
                        continue;
                    state.Buildings.Deck.RemoveAt(i);
                    view.GetBuilding(building).Destroy();
                    if (i < state.Buildings.HandPointer) 
                        state.Buildings.HandPointer--;
                    else
                        Debug.LogError("WTF");
                }
            }
        }
    }
}