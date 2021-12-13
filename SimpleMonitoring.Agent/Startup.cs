using System;
using SimpleMonitoring.Utilites;

using Version = SimpleMonitoring.Utilites.Version;

namespace SimpleMonitoring.Agent
{
    public static class Startup
    {
        public static Config Configuration = new Config("SimpleMonitoring.Agent");

        public static void Initialize()
        {
            Logging.Log("[SimpleMonitoring.Agent]", "Setting up Agent for operation.");
            if(Configuration.IsNew)
            {
                Logging.Log("[SimpleMonitoring.Agent]", "It appreas as if youre running the agent for the first time on this system. Initializing and populating Agent config now.");

                Configuration.Add("agent.version", new Version("0", "09", "3b"));
                Configuration.Add("agent.connectedaddresses", new (string ip, int port)[] { });

                Logging.Log("[SimpleMonitoring.Agent]", "Finished the initialization and population of the Agent config successfully.");
            }
            Console.Title = $"SimpleMonitoring.Agent - Version {Configuration.Get<Version>("agent.version").Complete}";
            Monitoring.Initialize();
            TrayIcon.Initialize();
            Linking.Initialize();
            Input.Start();
        }
    }
}
