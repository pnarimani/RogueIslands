using System;
using RogueIslands.Boosters.Descriptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class LiteralDescriptionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(LiteralDescription);

        public object ReadYaml(IParser parser, Type type) => new LiteralDescription(parser.Consume<Scalar>().Value);

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var description = (LiteralDescription)value!;
            var tagName = new TagName("!" + nameof(LiteralDescription));
            emitter.Emit(new MappingStart(AnchorName.Empty, tagName, false, MappingStyle.Flow));
            emitter.Emit(new Scalar("Value"));
            emitter.Emit(new Scalar(description.Text));
            emitter.Emit(new MappingEnd());
        }
    }
}