using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimpleMonitoring.Utilites
{
    public class Config
    {
        #region PUBLIC FIELDS AND PROPERTIES
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public bool IsNew { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public Config() { }
        public Config(string Name)
        {
            var n = GetConfigFile(Name);
            if (File.Exists(GetConfigFile(Name)))
            {
                IsNew = false;
                var Json = File.ReadAllText(GetConfigFile(Name));
                var Cfg = JsonConvert.DeserializeObject<Config>(Json);
                if(Cfg.Name == Name)
                {
                    this.p_Content = Cfg.p_Content;
                }
                return;
            }
            this.Name = Name;
            IsNew = true;
        }
        #endregion

        #region PUBLIC METHODS
        public void Add(string Key, object Entry)
        {
            if (p_Content.Where(x => x.Key == Key).Count() >= 1)
                return;

            var jentry = JToken.FromObject(Entry);
            p_Content.Add(Key, jentry);
            Logging.Log("[SimpleMonitoring.Utilities]", $"Added item {Entry.ToString()} with key {Key} to config: {Name}");
            SaveToFile(this);
        }
        public T Get<T>(string Key)
        {
            var Value = p_Content.Where(x => x.Key == Key).FirstOrDefault();
            if (Value.Value == null || Value.Value == default)
            {
                Logging.Log("[SimpleMonitoring.Utilities]", $"CONFIG ERROR: The specified item does not exist in this config. {Key}");
                throw new InvalidOperationException($"The specified item does not exist in this config. {Key}");
            }
            return Value.Value.ToObject<T>();
        }
        #endregion

        #region INTERNAL STATIC METHODS

        internal static string GetConfigPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SimpleMonitoring");
        }
        internal static string GetConfigFile(string Name)
        {
            return Path.Combine(GetConfigPath(), Name + ".json");
        }
        internal static bool SaveToFile(Config Cfg)
        {
            var Json = JsonConvert.SerializeObject(Cfg);
            var CfgFile = GetConfigFile(Cfg.Name);
            File.WriteAllText(CfgFile, Json);

            return File.Exists(CfgFile);
        }

        #endregion

        #region INTERNAL FIELDS AND PROPERTIES
        [JsonProperty]
        internal Dictionary<string, JToken> p_Content = new Dictionary<string, JToken>();
        #endregion
    }
}
