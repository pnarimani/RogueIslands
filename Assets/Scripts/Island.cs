using System.Collections;
using System.Collections.Generic;

namespace RogueIslands
{
    public class Island : IEnumerable<PlacedBuilding>
    {
        public string Id { get; set; }
        public List<PlacedBuilding> Buildings { get; set; }
        
        public List<PlacedBuilding>.Enumerator GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator<PlacedBuilding> IEnumerable<PlacedBuilding>.GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}