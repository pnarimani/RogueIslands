using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using Newtonsoft.Json;
using RogueIslands.Serialization.JsonNet;
using RogueIslands.Serialization.XML;
using RogueIslands.Serialization.YamlDotNetIntegration;
using RogueIslands.Serialization.DeepClone;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Module = Autofac.Module;

namespace RogueIslands.Autofac.Modules
{
    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Cloner()).AsImplementedInterfaces().SingleInstance();
            RegisterYaml(builder);
        }

        private static void RegisterJson(ContainerBuilder builder)
        {
            var converterTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    !x.IsAbstract &&
                    x.GetCustomAttribute<CompilerGeneratedAttribute>() == null &&
                    typeof(JsonConverter).IsAssignableFrom(x)
                ).ToArray();

            builder.RegisterTypes(converterTypes).As<JsonConverter>();

            builder.Register(c =>
            {
                var jsonSerializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                };

                foreach (var converter in c.Resolve<IReadOnlyList<JsonConverter>>())
                    jsonSerializer.Converters.Add(converter);

                return new NewtonsoftJsonProxy(jsonSerializer);
            }).AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterXML(ContainerBuilder builder)
        {
            builder.RegisterType<XmlProxy>().AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterYaml(ContainerBuilder builder)
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    !x.IsAbstract &&
                    x.Namespace != null &&
                    x.Namespace.Contains("RogueIslands") &&
                    x.GetCustomAttribute<CompilerGeneratedAttribute>() == null
                )
                .ToArray();

            foreach (var converter in types.Where(t => typeof(IYamlTypeConverter).IsAssignableFrom(t)))
            {
                builder.RegisterType(converter).AsImplementedInterfaces();
            }

            builder.Register(c =>
            {
                var serializerBuilder = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .EnsureRoundtrip()
                    .WithIndentedSequences()
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull);

                var deserializerBuilder = new DeserializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance);

                RegisterTypeTags(types, serializerBuilder, deserializerBuilder);
                RegisterTypeConverters(c, serializerBuilder, deserializerBuilder);
                return new YamlDotNetProxy(serializerBuilder.Build(), deserializerBuilder.Build());
                
            }).AsImplementedInterfaces().SingleInstance();
        }

        private static void RegisterTypeConverters(IComponentContext c, SerializerBuilder serializerBuilder,
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
                    serializerBuilder.WithTagMapping(tag, type);
                    deserializerBuilder.WithTagMapping(tag, type);
                }
            }
        }
    }
}