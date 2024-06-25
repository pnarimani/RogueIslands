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
            return false;
            return type == typeof(GameEventCondition);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var types = new List<Type>();
            parser.Consume<SequenceStart>();
            while (parser.Current is not SequenceEnd)
            {
                var scalar = parser.Consume<Scalar>();
                types.Add(_nameToType[scalar.Value]);
            }

            return new GameEventCondition()
            {
                TriggeringEvents = types,
            };
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new MappingStart(AnchorName.Empty, new TagName("!" + nameof(GameEventCondition)), true,
                MappingStyle.Block));
            // emitter.Emit(new SequenceStart(AnchorName.Empty, new TagName("!" + nameof(GameEventCondition)), true, SequenceStyle.Block));
            foreach (var eventType in ((GameEventCondition)value).TriggeringEvents)
            {
                emitter.Emit(new Scalar(_typeToName[eventType]));
            }

            // emitter.Emit(new SequenceEnd());
            emitter.Emit(new MappingEnd());
        }
    }
}