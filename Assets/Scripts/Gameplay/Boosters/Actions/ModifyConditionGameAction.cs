using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Actions
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