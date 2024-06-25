using Newtonsoft.Json;
using Newtonsoft.Json.UnityConverters.Math;
using RogueIslands.Serialization.JsonNet.Converters;

namespace RogueIslands.Serialization.JsonNet
{
    public class NewtonsoftJsonProxy : ISerializer, IDeserializer
    {
        private JsonSerializer _serializer;

        public NewtonsoftJsonProxy(JsonSerializer jsonSerializer)
        {
            _serializer = jsonSerializer;
        }

        public string Serialize<T>(T data)
        {
            using var stringWriter = new System.IO.StringWriter();
            _serializer.Serialize(stringWriter, data);
            return stringWriter.ToString();
        }

        public string SerializePretty<T>(T data)
        {
            using var stringWriter = new System.IO.StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
            };
            _serializer.Serialize(jsonWriter, data);
            return stringWriter.ToString();
        }

        public T Deserialize<T>(string data)
        {
            using var stringReader = new System.IO.StringReader(data);
            return (T)_serializer.Deserialize(stringReader, typeof(T));
        }
    }
}