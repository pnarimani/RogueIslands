using System;
using RogueIslands.Boosters.Descriptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ScalingBoosterDescriptionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(ScalingBoosterDescription);

        public object ReadYaml(IParser parser, Type type) =>
            new ScalingBoosterDescription(parser.Consume<Scalar>().Value);

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var description = (ScalingBoosterDescription)value!;
            var tagName = new TagName("!" + nameof(ScalingBoosterDescription));
            emitter.Emit(new MappingStart(AnchorName.Empty, tagName, false, MappingStyle.Flow));
            emitter.Emit(new Scalar("Prefix"));
            emitter.Emit(new Scalar(description.Prefix));
            emitter.Emit(new MappingEnd());
        }
    }
}