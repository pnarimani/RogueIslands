using System;
using System.Globalization;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class QuaternionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Quaternion);
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
                return Quaternion.Euler(x, y, z);
            }

            return Quaternion.identity;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Flow));
            
            var quaternion = (Quaternion)value!;
            var angles = quaternion.eulerAngles;
            
            if (angles != Vector3.zero)
            {
                emitter.Emit(new Scalar("X"));
                emitter.Emit(new Scalar(angles.x.ToString(CultureInfo.InvariantCulture)));
                emitter.Emit(new Scalar("Y"));
                emitter.Emit(new Scalar(angles.y.ToString(CultureInfo.InvariantCulture)));
                emitter.Emit(new Scalar("Z"));
                emitter.Emit(new Scalar(angles.z.ToString(CultureInfo.InvariantCulture)));
            }

            emitter.Emit(new MappingEnd());
        }
    }
}