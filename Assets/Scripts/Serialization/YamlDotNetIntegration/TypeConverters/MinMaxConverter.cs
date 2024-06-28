using System;
using RogueIslands.Gameplay;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class MinMaxConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(MinMax);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                int min = 0, max = 0;
                while (parser.TryConsume<Scalar>(out var scalar))
                {
                    switch (scalar.Value)
                    {
                        case "Min":
                            min = int.Parse(parser.Consume<Scalar>().Value);
                            break;
                        case "Max":
                            max = int.Parse(parser.Consume<Scalar>().Value);
                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return new MinMax(min, max);
            }

            return new MinMax(0, 0);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var minMax = (MinMax)value!;
            
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
            emitter.Emit(new Scalar("Min"));
            emitter.Emit(new Scalar(minMax.Min.ToString()));
            emitter.Emit(new Scalar("Max"));
            emitter.Emit(new Scalar(minMax.Max.ToString()));
            emitter.Emit(new MappingEnd());
        }
    }
}