using System;
using RogueIslands.Buildings;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ClusterIdConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(ClusterId);

        public object ReadYaml(IParser parser, Type type)
        {
            return new ClusterId(uint.Parse(parser.Consume<Scalar>().Value));
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((ClusterId)value!).Value.ToString()));
        }
    }
}