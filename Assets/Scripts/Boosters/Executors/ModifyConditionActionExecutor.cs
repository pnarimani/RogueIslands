using System.Linq;
using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Conditions;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Executors
{
    public abstract class ModifyConditionActionExecutor<TCondition, TAction> : GameActionExecutor<TAction>
        where TAction : ModifyConditionGameAction
        where TCondition : IGameCondition
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, TAction action)
        {
            switch (state.CurrentEvent)
            {
                case PropertiesRestored:
                    ModifyAllBoosters(state);
                    break;
                case BoosterAdded boosterAdded when !Equals(boosterAdded.Booster, booster):
                    ModifySingleBooster(boosterAdded.Booster);
                    break;
            }
        }

        private void ModifySingleBooster(IBooster booster)
        {
            foreach (var c in booster.EventAction.GetAllConditions().OfType<TCondition>()) 
                ModifyCondition(c);
        }

        private void ModifyAllBoosters(GameState state)
        {
            foreach (var b in state.Boosters)
                ModifySingleBooster(b);

            foreach (var b in state.WorldBoosters.SpawnedBoosters)
                ModifySingleBooster(b);
        }

        protected abstract void ModifyCondition(TCondition condition);
    }
}