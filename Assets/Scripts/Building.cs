namespace RogueIslands
{
    public class Building
    {
        public string Name { get; set;}
        public string Description { get; set; }
        public string PrefabAddress { get; set; }
        public float Range { get; set; }
        public BuildingInstanceId Id { get; set; }
        public Category Category { get; set; }
        public ColorTag Color { get; set; }
        public int EnergyCost { get; set; }
        public double Output { get; set; }
        
        public double OutputUpgrade { get; set; }
    }
}