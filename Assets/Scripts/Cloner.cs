using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.UnityConverters.Math;

namespace RogueIslands
{
    public static class Cloner
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            Converters =
            {
                new Vector3Converter(),
                new ColorConverter(),
                new QuaternionConverter(),
            },
        };

        private static readonly StringBuilder _buffer = new();

        public static T Clone<T>(this T original)
        {
            _buffer.Clear();
            using (var sw = new StringWriter(_buffer))
                _serializer.Serialize(sw, original);
            
            using var stringReader = new StringReader(_buffer.ToString());
            using var jsonTextReader = new JsonTextReader(stringReader);
            return _serializer.Deserialize<T>(jsonTextReader);
        }
    }
}