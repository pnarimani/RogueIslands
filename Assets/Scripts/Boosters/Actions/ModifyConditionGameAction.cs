using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Actions
{
    public abstract class ModifyConditionGameAction : GameAction
    {
        protected ModifyConditionGameAction()
        {
            Conditions = new[]
            {
                GameEventCondition.Create<PropertiesRestored>(),
            };
        }
    }
}