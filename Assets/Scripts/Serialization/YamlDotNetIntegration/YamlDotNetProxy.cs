using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration
{
    using YamlDeserializer = YamlDotNet.Serialization.IDeserializer;
    using YamlSerializer = YamlDotNet.Serialization.ISerializer;

    public class YamlDotNetProxy : ISerializer, IDeserializer
    {
        private readonly YamlSerializer _serializer;
        private readonly YamlDeserializer _deserializer;
        private IValueSerializer _valueSerializer;
        private IValueDeserializer _valueDeserializer;

        public YamlDotNetProxy(
            YamlSerializer serializer,
            YamlDeserializer deserializer,
            IValueSerializer valueSerializer,
            IValueDeserializer valueDeserializer)
        {
            this.valueDeserializer = valueDeserializer;
            this.valueSerializer = valueSerializer;
            _deserializer = deserializer;
            _serializer = serializer;
        }

        public YamlSerializer Serializer => _serializer;

        public YamlDeserializer Deserializer => _deserializer;

        public IValueSerializer valueSerializer
        {
            get => _valueSerializer;
            set => _valueSerializer = value;
        }

        public IValueDeserializer valueDeserializer
        {
            get => _valueDeserializer;
            set => _valueDeserializer = value;
        }

        public string Serialize<T>(T data) => Serializer.Serialize(data);

        public string SerializePretty<T>(T data)
        {
            return Serialize(data);
        }

        public T Deserialize<T>(string data) => Deserializer.Deserialize<T>(data);
    }
}