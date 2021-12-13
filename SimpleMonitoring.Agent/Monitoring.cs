using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SimpleMonitoring.Monitoring;
using SimpleMonitoring.Monitoring.Monitors;
using SimpleMonitoring.Utilites;

namespace SimpleMonitoring.Agent
{
    public static class Monitoring
    {
        internal static List<Monitor> RunningMonitors = new List<Monitor>();

        public static void Initialize()
        {
            Logging.Log("[SimpleMonitoring.Agent]", "Setting up and initializing Monitors.");
            foreach (var t in Assembly.GetAssembly(typeof(Monitor)).GetTypes())
            {
                if(t.Name != "Monitor" && t.BaseType.Name == "Monitor" && !t.IsAbstract)
                {
                    var mon = Activator.CreateInstance(t, new object[] { 60000d }) as Monitor;
                    mon.Start();
                    RunningMonitors.Add(mon);
                }
            }
            if(RunningMonitors.Count <= 0)
                Logging.Log("[SimpleMonitoring.Agent]", "Monitor setup failed, please contact an Administrator!");
            else
                Logging.Log("[SimpleMonitoring.Agent]", "Monitor setup and initialization was successful!");
        }

    }
}
