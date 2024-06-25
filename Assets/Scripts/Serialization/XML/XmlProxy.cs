using System.Xml.Serialization;

namespace RogueIslands.Serialization.XML
{
    public class XmlProxy : ISerializer, IDeserializer
    {
        public string Serialize<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new System.IO.StringWriter();
            serializer.Serialize(stringWriter, data);
            return stringWriter.ToString();
        }

        public string SerializePretty<T>(T data)
        {
            return Serialize(data);
        }

        public T Deserialize<T>(string data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringReader = new System.IO.StringReader(data);
            return (T)serializer.Deserialize(stringReader);
        }
    }
}