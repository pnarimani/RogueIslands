namespace RogueIslands.Gameplay
{
    public class ScoreState
    {
        public double TransientColorBonus { get; set; }
        public double TransientCategoryBonus { get; set; }
        public double TransientSizeBonus { get; set; }
        public double TransientExtraBonus { get; set; }
        public double TransientScore { get; set; }
        public double CurrentScore { get; set; }
        public double[] AllRequiredScores { get; set; }

        public double GetTotalBonus() => TransientColorBonus + TransientCategoryBonus + TransientSizeBonus + TransientExtraBonus;

        public void ResetBonuses()
        {
            TransientColorBonus = 0;
            TransientCategoryBonus = 0;
            TransientSizeBonus = 0;
            TransientExtraBonus = 0;
        }
        
        public void MultiplyBonus(double multiplier)
        {
            TransientColorBonus *= multiplier;
            TransientCategoryBonus *= multiplier;
            TransientSizeBonus *= multiplier;
            TransientExtraBonus *= multiplier;
        }
    }
}