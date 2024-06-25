namespace RogueIslands.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T data);
        string SerializePretty<T>(T data);
    }
}