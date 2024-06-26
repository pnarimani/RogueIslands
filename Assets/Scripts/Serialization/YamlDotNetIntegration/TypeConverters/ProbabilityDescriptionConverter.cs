﻿using System;
using RogueIslands.Boosters.Descriptions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RogueIslands.Serialization.YamlDotNetIntegration.TypeConverters
{
    public class ProbabilityDescriptionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(ProbabilityDescription);

        public object ReadYaml(IParser parser, Type type) => new ProbabilityDescription(parser.Consume<Scalar>().Value);

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var description = (ProbabilityDescription)value!;
            var tagName = new TagName("!" + nameof(ProbabilityDescription));
            emitter.Emit(new MappingStart(AnchorName.Empty, tagName, false, MappingStyle.Flow));
            emitter.Emit(new Scalar("Format"));
            emitter.Emit(new Scalar(description.Format));
            emitter.Emit(new MappingEnd());
        }
    }
}