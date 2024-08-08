using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public abstract class GameAction
    {
        public IGameCondition Condition { get; set; }

        public bool Execute(IBooster booster)
        {
            if (Condition != null && !Condition.Evaluate(booster))
                return false;
            return ExecuteAction(booster);
        }

        protected abstract bool ExecuteAction(IBooster booster);
    }
}