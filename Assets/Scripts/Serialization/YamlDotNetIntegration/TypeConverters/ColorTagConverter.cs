using System;
using RogueIslands.Buildings;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ColorTagConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(ColorTag);

        public object ReadYaml(IParser parser, Type type)
        {
            var tag = parser.Consume<Scalar>().Value;
            foreach (var c in ColorTag.All)
            {
                if (c.Tag == tag)
                    return c;
            }

            return new ColorTag(tag, Color.clear);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((ColorTag)value!).Tag));
        }
    }
}