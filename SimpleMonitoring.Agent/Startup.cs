﻿using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Version = SimpleMonitoring.Utilites.Version;

namespace SimpleMonitoring.Agent
{
    public static class Startup
    {
        public static Version Version;
        public static Config Configuration = new Config("SimpleMonitoring.Agent");

        public static void Initialize()
        {
            Logging.Log("[SimpleMonitoring.Agent]", "Setting up Agent for operation.");
            if(Configuration.IsNew)
            {
                Logging.Log("[SimpleMonitoring.Agent]", "It appreas as if youre running the agent for the first time on this system. Initializing and populating Agent config now.");

                Configuration.Add("agent.version", new Version("0.", "09", "3b"));
                Configuration.Add("agent.connectedaddresses", new (string ip, int port)[] { });

                Logging.Log("[SimpleMonitoring.Agent]", "Finished the initialization and population of the Agent config successfully.");
            }
            Monitoring.Initialize();
        }
    }
}