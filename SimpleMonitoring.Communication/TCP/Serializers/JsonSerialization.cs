using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Serializers
{
    public static class JsonSerialization
    {
        static readonly JsonSerializer _serializer;
        static readonly JsonSerializerSettings _settings;

        static JsonSerialization()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = false
                    }
                }
            };
            _settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            _serializer = JsonSerializer.Create(_settings);
        }

        public static JObject Serialize(object @object) => JObject.FromObject(@object, _serializer);
        public static JObject Deserialize(string json) => JObject.Parse(json);
        public static T Deserialize<T>(JObject @object) => @object.ToObject<T>(); /*(T)_serializer.Deserialize(new JTokenReader(@object), typeof(T)); //overengineering.... */
        public static object ToObject(Type type, JObject source) => source.ToObject(type);
    }
}
