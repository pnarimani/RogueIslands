using System.Collections;
using System.Collections.Generic;

namespace RogueIslands
{
    public class Island : IEnumerable<Building>
    {
        public string Id { get; set; }
        public List<Building> Buildings { get; set; }
        
        public List<Building>.Enumerator GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator<Building> IEnumerable<Building>.GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}