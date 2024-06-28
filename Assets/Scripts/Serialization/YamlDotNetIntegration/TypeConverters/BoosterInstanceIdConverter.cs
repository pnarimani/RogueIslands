using System;
using RogueIslands.Gameplay.Boosters;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class BoosterInstanceIdConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(BoosterInstanceId);

        public object ReadYaml(IParser parser, Type type)
        {
            return new BoosterInstanceId(uint.Parse(parser.Consume<Scalar>().Value));
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((BoosterInstanceId)value!).Value.ToString()));
        }
    }
}