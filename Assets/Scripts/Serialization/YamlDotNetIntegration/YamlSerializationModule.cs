using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using RogueIslands.DependencyInjection;
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
            var types = GetProjectTypes();

            RegisterTypeConverters(builder, types);

            builder.Register(c =>
            {
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

        private static Type[] GetProjectTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    !x.IsAbstract &&
                    x.Namespace != null &&
                    x.Namespace.Contains("RogueIslands") &&
                    x.GetCustomAttribute<CompilerGeneratedAttribute>() == null
                )
                .ToArray();
        }

        private static void RegisterTypeConverters(ContainerBuilder builder, Type[] types)
        {
            foreach (var converter in types.Where(t => typeof(IYamlTypeConverter).IsAssignableFrom(t)))
            {
                builder.RegisterType(converter)
                    .AsImplementedInterfaces();
            }
        }

        private static void AddTypeConvertersToSerializer(IComponentContext c, SerializerBuilder serializerBuilder,
            DeserializerBuilder deserializerBuilder)
        {
            foreach (var converter in c.Resolve<IReadOnlyList<IYamlTypeConverter>>())
            {
                serializerBuilder.WithTypeConverter(converter);
                deserializerBuilder.WithTypeConverter(converter);
            }
        }

        private static void RegisterTypeTags(Type[] types, SerializerBuilder serializerBuilder,
            DeserializerBuilder deserializerBuilder)
        {
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
                    var fullTag = "!" + type.FullName;
                    serializerBuilder.WithTagMapping(tag, type);
                    deserializerBuilder.WithTagMapping(tag, type);
                    deserializerBuilder.WithTagMapping(fullTag, type);
                }
            }
        }
    }
}