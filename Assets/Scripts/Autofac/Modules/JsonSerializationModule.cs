using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using Newtonsoft.Json;
using RogueIslands.Serialization.JsonNet;
using CustomAttributeExtensions = System.Reflection.CustomAttributeExtensions;

namespace RogueIslands.Autofac.Modules
{
    public class JsonSerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterJson(builder);
        }

        private static void RegisterJson(ContainerBuilder builder)
        {
            var converterTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    !x.IsAbstract &&
                    CustomAttributeExtensions.GetCustomAttribute<CompilerGeneratedAttribute>(x) == null &&
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
    }
}