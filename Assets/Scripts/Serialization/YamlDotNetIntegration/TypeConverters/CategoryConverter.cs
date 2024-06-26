using System;
using RogueIslands.Buildings;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class CategoryConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(Category);

        public object ReadYaml(IParser parser, Type type)
        {
            return new Category(parser.Consume<Scalar>().Value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((Category)value!).Value));
        }
    }
}