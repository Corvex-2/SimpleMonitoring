using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class DiskMonitor : Monitor
    {
        public static Config Configuration = new Config("SimpleMonitoring.Monitoring.DiskMonitor");

        public DiskMonitor(double Interval) : base(Interval) { }

        internal override void Initialize()
        {
            base.Initialize();
            if(Configuration.IsNew)
            {
                Logging.Log("[SimpleMonitoring.Monitoring.DiskMonitor]", "It appears as if this is the first time the disk monitor is being executed. Initializing and populating the configuration now!");

                Configuration.Add("monitoring.diskmonitor.spacewarningthreshhold", 200);
                Configuration.Add("monitoring.diskmonitor.spacecriticalthreshhold", 100);
                Configuration.Add("monitoring.diskmonitor.spacewarningalertspan", new TimeSpan(3, 0, 0));
                Configuration.Add("monitoring.diskmonitor.spacecriticalalertspan", new TimeSpan(1, 0, 0));
            }
        }

        internal override void Check()
        {

        }
    }
}
