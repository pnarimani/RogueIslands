namespace RogueIslands.Serialization
{
    public interface ICloner
    {
        T Clone<T>(T data);
    }
}