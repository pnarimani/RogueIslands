using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ScoreBonusExecutor : GameActionExecutor<ModifyBonusAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ModifyBonusAction action)
        {
            if (action.Addition is { } add)
            {
                state.Score.TransientExtraBonus += add;
            }

            if (action.Multiplier is { } xMult)
            {
                state.Score.MultiplyBonus(xMult);
            }
            
            if (action.ColorMultiplier is { } colorMult)
            {
                state.Score.TransientColorBonus *= colorMult;
            }
            
            if (action.CategoryMultiplier is { } categoryMult)
            {
                state.Score.TransientCategoryBonus *= categoryMult;
            }
            
            if (action.SizeMultiplier is { } sizeMult)
            {
                state.Score.TransientSizeBonus *= sizeMult;
            }
        }
    }
}