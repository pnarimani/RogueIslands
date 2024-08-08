using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BonusAction : GameAction
    {
        public double? Addition { get; set; }
        public double? Multiplier { get; set; }
        public double? ColorMultiplier { get; set; }
        public double? CategoryMultiplier { get; set; }
        public double? SizeMultiplier { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();

            if (Addition is { } add)
                state.Score.TransientExtraBonus += add;

            if (Multiplier is { } xMult)
                state.Score.MultiplyBonus(xMult);

            if (ColorMultiplier is { } colorMult)
                state.Score.TransientColorBonus *= colorMult;

            if (CategoryMultiplier is { } categoryMult)
                state.Score.TransientCategoryBonus *= categoryMult;

            if (SizeMultiplier is { } sizeMult)
                state.Score.TransientSizeBonus *= sizeMult;

            return true;
        }
    }
}