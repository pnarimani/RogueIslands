using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RogueIslands.Buildings;

namespace RogueIslands.Serialization.JsonNet.Converters
{
    public class ClusterConverter : JsonConverter<Cluster>
    {
        public override void WriteJson(JsonWriter writer, Cluster value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Id");
            writer.WriteValue(value.Id);
            writer.WritePropertyName("Buildings");
            serializer.Serialize(writer, value.Buildings);
            writer.WriteEndObject();
        }

        public override Cluster ReadJson(JsonReader reader, Type objectType, Cluster existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var cluster = new Cluster();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                var propertyName = (string)reader.Value;
                reader.Read();
                switch (propertyName)
                {
                    case "Id":
                        cluster.Id = (string)reader.Value;
                        break;
                    case "Buildings":
                        cluster.Buildings = serializer.Deserialize<List<Building>>(reader);
                        break;
                }
            }

            return cluster;
        }
    }
}