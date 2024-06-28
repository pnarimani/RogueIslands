using System;
using RogueIslands.Gameplay.Buildings;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class BuildingInstanceIdConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(BuildingId);

        public object ReadYaml(IParser parser, Type type)
        {
            return new BuildingId(uint.Parse(parser.Consume<Scalar>().Value));
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((BuildingId)value!).Value.ToString()));
        }
    }
}