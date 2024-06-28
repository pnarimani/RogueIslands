using System;
using RogueIslands.Gameplay.Boosters.Descriptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ScalingBoosterDescriptionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(ScalingBoosterDescription);

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                string prefix = null;
                while (parser.TryConsume<Scalar>(out var scalar))
                {
                    switch (scalar.Value)
                    {
                        case "Prefix":
                            prefix = parser.Consume<Scalar>().Value;
                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return new ScalingBoosterDescription(prefix);
            }

            return null;
        }

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