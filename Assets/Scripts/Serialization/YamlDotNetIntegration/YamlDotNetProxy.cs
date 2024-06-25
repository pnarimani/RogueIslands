namespace RogueIslands.Serialization.YamlDotNetIntegration
{
    public class YamlDotNetProxy : ISerializer, IDeserializer
    {
        private readonly YamlDotNet.Serialization.ISerializer _serializer;
        private readonly YamlDotNet.Serialization.IDeserializer _deserializer;

        public YamlDotNetProxy(YamlDotNet.Serialization.ISerializer serializer,
            YamlDotNet.Serialization.IDeserializer deserializer)
        {
            _deserializer = deserializer;
            _serializer = serializer;
        }

        public string Serialize<T>(T data) => _serializer.Serialize(data);
        public string SerializePretty<T>(T data)
        {
            return Serialize(data);
        }

        public T Deserialize<T>(string data) => _deserializer.Deserialize<T>(data);
    }
}