using System.Collections.Generic;

namespace RogueIslands
{
    public class Island
    {
        public string Id { get; set; }
        public List<PlacedBuilding> Buildings { get; set; }
    }
}