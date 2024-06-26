using System;
using RogueIslands.Buildings;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ClusterConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(Cluster);

        public object ReadYaml(IParser parser, Type type)
        {
            return new Cluster();
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var cluster = (Cluster)value;
            emitter.Emit(new MappingStart());

            if (cluster != null)
            {
                 emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, cluster.Id, ScalarStyle.DoubleQuoted, false, true));
                 
                 // emitter.Emit(new MappingStart());
                 emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, isImplicit: true, SequenceStyle.Block));
                 foreach (var building in cluster.Buildings)
                 {
                     // emitter.Emit(new Scalar(TagName.Empty, building.Id.Value.ToString()));
                     emitter.Emit(new AnchorAlias(new AnchorName("building " + building.Id.Value)));
                 }
                 emitter.Emit(new SequenceEnd());
                 // emitter.Emit(new MappingEnd());
            }

            // emitter.Emit(new Scalar("Buildings"));
            // emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, isImplicit: true, SequenceStyle.Block));
            // foreach (var building in cluster.Buildings)
            // {
            //     emitter.Emit(new AnchorAlias(new AnchorName("building " + building.Id.Value)));
            // }

            emitter.Emit(new MappingEnd());
        }
    }
}