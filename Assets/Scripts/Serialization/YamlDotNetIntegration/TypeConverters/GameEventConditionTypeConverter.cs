using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters.Conditions;
using RogueIslands.GameEvents;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class GameEventConditionTypeConverter : IYamlTypeConverter
    {
        private readonly Dictionary<string, Type> _nameToType;
        private readonly Dictionary<Type, string> _typeToName;

        public GameEventConditionTypeConverter()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => typeof(IGameEvent).IsAssignableFrom(t))
                .ToArray();
            _nameToType = types.ToDictionary(t => t.Name, t => t);
            _typeToName = types.ToDictionary(t => t, t => t.Name);
        }

        public bool Accepts(Type type)
        {
            return type == typeof(GameEventCondition);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                var events = new List<Type>();
                while (parser.TryConsume(out Scalar scalar))
                {
                    switch (scalar.Value)
                    {
                        case "TriggeringEvents":
                            parser.Consume<SequenceStart>();
                            while (!parser.TryConsume<SequenceEnd>(out _))
                            {
                                var eventType = _nameToType[parser.Consume<Scalar>().Value];
                                events.Add(eventType);
                            }

                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return new GameEventCondition() { TriggeringEvents = events };
            }

            return null;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var tagName = new TagName("!" + nameof(GameEventCondition));
            emitter.Emit(new MappingStart(AnchorName.Empty, tagName, false, MappingStyle.Block));

            emitter.Emit(new Scalar("TriggeringEvents"));

            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, isImplicit: true, SequenceStyle.Block));
            foreach (var eventType in ((GameEventCondition)value!).TriggeringEvents)
            {
                emitter.Emit(new Scalar(_typeToName[eventType]));
            }

            emitter.Emit(new SequenceEnd());
            emitter.Emit(new MappingEnd());
        }
    }
}