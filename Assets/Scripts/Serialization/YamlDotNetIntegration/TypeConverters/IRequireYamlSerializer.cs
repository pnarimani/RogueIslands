using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public interface IRequireYamlSerializer
    {
        void SetSerializer(IValueSerializer serializer, IValueDeserializer deserializer);
    }
}