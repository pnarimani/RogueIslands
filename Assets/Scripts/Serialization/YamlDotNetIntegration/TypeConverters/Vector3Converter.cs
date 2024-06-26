using System;
using System.Globalization;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class Vector3Converter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Vector3);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                float x = 0, y = 0, z = 0;
                while (parser.TryConsume<Scalar>(out var scalar))
                {
                    switch (scalar.Value)
                    {
                        case "X":
                            x = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                            break;
                        case "Y":
                            y = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                            break;
                        case "Z":
                            z = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return new Vector3(x, y, z);
            }

            return Vector3.zero;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var v = (Vector3)value!;
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
            if (v != Vector3.zero)
            {
                emitter.Emit(new Scalar("X"));
                emitter.Emit(new Scalar(v.x.ToString(CultureInfo.InvariantCulture)));
                emitter.Emit(new Scalar("Y"));
                emitter.Emit(new Scalar(v.y.ToString(CultureInfo.InvariantCulture)));
                emitter.Emit(new Scalar("Z"));
                emitter.Emit(new Scalar(v.z.ToString(CultureInfo.InvariantCulture)));
            }

            emitter.Emit(new MappingEnd());
        }
    }
}