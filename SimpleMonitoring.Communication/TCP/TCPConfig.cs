using SuK_IT.Resources.Configuration;
using SuK_IT.Resources.Cryptography;

namespace SuK_IT.Networking.TCP
{
    /// <summary>
    /// SuK-IT, 29.11.2020
    ///
    /// TODO:
    /// 
    /// </summary>
    /// 

    public class TCPConfig
    {
        private string _identifier = "default";
        public TCPConfig(string Name)
        {
            _identifier = Name;
        }
        public int Port
        {
            get
            {
                return ConfigManager.GetConfig(_identifier).GetValue<int>("tcp_port", 9001);
            }
            set
            {
                ConfigManager.GetConfig(_identifier).SetValue("tcp_port", value);
            }
        }
        public string Address
        {
            get
            {
                return ConfigManager.GetConfig(_identifier).GetValue<string>("server_address", "127.0.0.1");
            }
            set
            {
                ConfigManager.GetConfig(_identifier).SetValue("server_address", value);
            }
        }        
        public bool Authentication
        {
            get
            {
                return ConfigManager.GetConfig(_identifier).GetValue<bool>("server_authrequired", false);
            }
            set
            {
                ConfigManager.GetConfig(_identifier).SetValue("server_authrequired", value);
            }
        }
        public string AuthenticationKey
        {
            get
            {
                return Base64Encoding.FromBase64String<string>(ConfigManager.GetConfig(_identifier).GetValue("server_authenticationkey", ""));
            }
            set
            {
                ConfigManager.GetConfig(_identifier).SetValue("server_authenticationkey", Base64Encoding.ToBase64String(value));
            }
        }
    }
}
