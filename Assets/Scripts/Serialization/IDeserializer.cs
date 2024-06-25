namespace RogueIslands.Serialization
{
    public interface IDeserializer
    {
        T Deserialize<T>(string data);
    }
}