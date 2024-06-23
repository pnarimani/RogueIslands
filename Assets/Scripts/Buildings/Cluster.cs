using System.Collections;
using System.Collections.Generic;

namespace RogueIslands.Buildings
{
    public class Cluster : IEnumerable<Building>
    {
        public string Id;
        public List<Building> Buildings;
        
        public List<Building>.Enumerator GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator<Building> IEnumerable<Building>.GetEnumerator() 
            => Buildings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}