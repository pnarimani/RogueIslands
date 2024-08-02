using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using RogueIslands.Autofac;
using RogueIslands.Diagnostics;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RogueIslands.Serialization.YamlDotNetIntegration
{
    public class YamlSerializationModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            RegisterYaml(builder);
        }

        private static void RegisterYaml(ContainerBuilder builder)
        {
            using var profiler = new ProfilerScope("YamlSerializationModule.RegisterYaml");

            var types = TypeDatabase.GetProjectTypes();

            RegisterTypeConverters(builder, types);

            builder.Register(c =>
            {
                using var _ = new ProfilerScope("YamlSerializationModule.CreateYamlDotNetProxy");

                var serializerBuilder = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .EnsureRoundtrip()
                    .WithIndentedSequences()
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults);

                var deserializerBuilder = new DeserializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .WithNodeTypeResolver(new ReadOnlyCollectionNodeTypeResolver());

                RegisterTypeTags(types, serializerBuilder, deserializerBuilder);
                AddTypeConvertersToSerializer(c, serializerBuilder, deserializerBuilder);

                var valueSerializer = serializerBuilder.BuildValueSerializer();
                var valueDeserializer = deserializerBuilder.BuildValueDeserializer();

                return new YamlDotNetProxy(
                    Serializer.FromValueSerializer(valueSerializer, EmitterSettings.Default),
                    Deserializer.FromValueDeserializer(valueDeserializer),
                    valueSerializer,
                    valueDeserializer
                );
            }).AsImplementedInterfaces().AsSelf().SingleInstance();
        }

        private static void RegisterTypeConverters(ContainerBuilder builder, IEnumerable<Type> types)
        {
            using var profiler = new ProfilerScope("YamlSerializationModule.RegisterTypeConverters");

            foreach (var converter in types.Where(t => typeof(IYamlTypeConverter).IsAssignableFrom(t)))
            {
                builder.RegisterType(converter)
                    .AsImplementedInterfaces();
            }
        }

        private static void AddTypeConvertersToSerializer(IComponentContext c, SerializerBuilder serializerBuilder,
            DeserializerBuilder deserializerBuilder)
        {
            using var profiler = new ProfilerScope("YamlSerializationModule.AddTypeConvertersToSerializer");

            IReadOnlyList<IYamlTypeConverter> yamlTypeConverters;
            using (new ProfilerScope("YamlSerializationModule.AddTypeConvertersToSerializer.ResolveConverters"))
                yamlTypeConverters = c.Resolve<IReadOnlyList<IYamlTypeConverter>>();

            foreach (var converter in yamlTypeConverters)
            {
                serializerBuilder.WithTypeConverter(converter);
                deserializerBuilder.WithTypeConverter(converter);
            }
        }

        private static void RegisterTypeTags(IEnumerable<Type> types, SerializerBuilder serializerBuilder,
            DeserializerBuilder deserializerBuilder)
        {
            using var profiler = new ProfilerScope("YamlSerializationModule.RegisterTypeTags");

            var typeGrouping = types.GroupBy(t => t.FullName!.Replace(t.Namespace! + ".", ""));
            foreach (var group in typeGrouping)
            {
                if (group.Count() > 1)
                {
                    foreach (var type in group)
                    {
                        var tag = "!" + type.FullName;
                        serializerBuilder.WithTagMapping(tag, type);
                        deserializerBuilder.WithTagMapping(tag, type);
                    }
                }
                else
                {
                    var type = group.First();
                    var tag = "!" + group.Key;
                    serializerBuilder.WithTagMapping(tag, type);
                    deserializerBuilder.WithTagMapping(tag, type);

                    var fullTag = "!" + type.FullName;
                    if (fullTag != tag)
                        deserializerBuilder.WithTagMapping(fullTag, type);
                }
            }
        }
    }
}