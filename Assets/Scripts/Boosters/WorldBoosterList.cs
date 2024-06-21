using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public static class WorldBoosterList
    {
        public static List<WorldBooster> Get()
        {
            return new List<WorldBooster>()
            {
                new()
                {
                  Name  = "Simple Product Boost",
                  Description = new LiteralDescription("+30 for all buildings nearby"),
                  Range = 4,
                  PrefabAddress = "WorldBoosters/Simple",
                  EventAction = new ScoringAction
                  {
                      Conditions = new IGameCondition[]
                      {
                          new GameEventCondition("AfterBuildingScored"),
                          new SelectedBuildingRangeCondition(),
                      },
                      Products = 30,
                  },
                },
            };
        }
    }
}