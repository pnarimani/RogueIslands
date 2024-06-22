namespace RogueIslands.Boosters.Actions
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