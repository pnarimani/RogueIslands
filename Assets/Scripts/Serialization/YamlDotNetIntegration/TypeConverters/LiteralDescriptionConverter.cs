using System;
using RogueIslands.Gameplay.Boosters.Descriptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class LiteralDescriptionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(LiteralDescription);

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                string text = null;
                while (parser.TryConsume<Scalar>(out var scalar))
                {
                    switch (scalar.Value)
                    {
                        case "Value":
                            text = parser.Consume<Scalar>().Value;
                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return new LiteralDescription(text);
            }

            return null;
        }

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