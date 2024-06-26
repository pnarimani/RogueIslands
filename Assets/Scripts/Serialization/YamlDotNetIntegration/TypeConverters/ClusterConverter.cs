using System;
using System.Collections.Generic;
using RogueIslands.Buildings;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;
using YamlSerializer = YamlDotNet.Serialization.IValueSerializer;
using YamlDeserializer = YamlDotNet.Serialization.IValueDeserializer;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ClusterConverter : IYamlTypeConverter, IRequireYamlSerializer
    {
        private YamlSerializer _serializer;
        private YamlDeserializer _deserializer;

        public bool Accepts(Type type) => type == typeof(Cluster);

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume(out MappingStart _))
            {
                var cluster = new Cluster() { Buildings = new List<Building>() };
                while (parser.TryConsume(out Scalar scalar))
                {
                    switch (scalar.Value)
                    {
                        case "Id":
                            cluster.Id = parser.Consume<Scalar>().Value;
                            break;
                        case "Buildings":
                            parser.Consume<SequenceStart>();
                            while (!parser.TryConsume<SequenceEnd>(out _))
                            {
                                using var state = new SerializerState();
                                var building = (Building)_deserializer.DeserializeValue(parser, typeof(Building), state,
                                    _deserializer);
                                state.OnDeserialization();
                                cluster.Buildings.Add(building);
                            }

                            break;
                    }
                }

                parser.Consume<MappingEnd>();
                return cluster;
            }

            return null;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var cluster = (Cluster)value;
            emitter.Emit(new MappingStart());

            if (cluster != null)
            {
                if (!string.IsNullOrEmpty(cluster.Id))
                {
                    emitter.Emit(new Scalar("Id"));
                    emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, cluster.Id, ScalarStyle.DoubleQuoted,
                        false,
                        true));
                }

                emitter.Emit(new Scalar("Buildings"));
                emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, isImplicit: true, SequenceStyle.Block));

                foreach (var building in cluster.Buildings)
                {
                    _serializer.SerializeValue(emitter, building, typeof(Building));
                }

                emitter.Emit(new SequenceEnd());
            }

            emitter.Emit(new MappingEnd());
        }

        public void SetSerializer(YamlSerializer serializer, YamlDeserializer deserializer)
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }
    }
}