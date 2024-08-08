using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;
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
            var types = TypeDatabase.GetProjectTypesOf<IGameEvent>();
            _nameToType = types.ToDictionary(t => t.Name, t => t);
            _typeToName = types.ToDictionary(t => t, t => t.Name);
        }

        public bool Accepts(Type type) => typeof(GameEventCondition).IsAssignableFrom(type);

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                var events = new List<Type>();
                while (parser.TryConsume(out Scalar scalar))
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

                parser.Consume<MappingEnd>();
                return new GameEventCondition(events);
            }

            return null;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var tagName = new TagName("!" + nameof(GameEventCondition));
            emitter.Emit(new MappingStart(AnchorName.Empty, tagName, false, MappingStyle.Block));

            emitter.Emit(new Scalar("TriggeringEvents"));

            emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, true, SequenceStyle.Block));
            foreach (var eventType in ((GameEventCondition)value!).TriggeringEvents)
                emitter.Emit(new Scalar(_typeToName[eventType]));

            emitter.Emit(new SequenceEnd());
            emitter.Emit(new MappingEnd());
        }
    }
}