namespace RogueIslands.Boosters
{
    public class MultipliedScoringAction : ScoringAction
    {
        public bool MultiplyByDay { get; set; }
        public bool MultiplyByIslandCount { get; set; }

        public MultipliedScoringAction()
        {
            XMult = 0;
        }
    }
}