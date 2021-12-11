using SimpleMonitoring.Monitoring.System;
using SimpleMonitoring.Utilites;
using System;
using System.Linq;
using System.Text;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class MemoryMonitor : Monitor
    {
        public static Config Configuration = new Config("SimpleMonitoring.Monitoring.MemoryMonitor");

        public MemoryMonitor(double Interval) : base(Interval) { }

        internal override void Initialize()
        {
            base.Initialize();
            if (Configuration.IsNew)
            {
                Logging.Log("[SimpleMonitoring.Monitoring.MemoryMonitor]", "It appears as if this is the first time the memory monitor is being executed. Initializing and populating the configuration now!");

                Configuration.Add("monitoring.memorymonitor.spacewarningthreshhold", 3d);
                Configuration.Add("monitoring.memorymonitor.spacecriticalthreshhold", 2d);
                Configuration.Add("monitoring.memorymonitor.spacewarningalertspan", new TimeSpan(3, 0, 0));
                Configuration.Add("monitoring.memorymonitor.spacecriticalalertspan", new TimeSpan(1, 0, 0));
            }
            SetAlertSpan(MonitorResult.Warning, Configuration.Get<TimeSpan>("monitoring.memorymonitor.spacewarningalertspan"));
            SetAlertSpan(MonitorResult.Critical, Configuration.Get<TimeSpan>("monitoring.memorymonitor.spacecriticalalertspan"));
        }

        internal override void Check()
        {
            var warningStates = new StringBuilder();
            var criticalStates = new StringBuilder();
            var memory = RAM.GetAll().FirstOrDefault();
            if (memory != default(RAM) && IsCriticallyLow(memory))
            {
                criticalStates.AppendLine($"\r\nThe following system {OS.GetAll().ComputerName} is currently at critical ram capacity!\r\n[{Math.Round(memory.GetTotalFreeSpace(), 2)} GB of {Math.Round(memory.GetTotalSpace(), 2)} GB]");
            }
            else if (memory != default(RAM) && IsRunningLow(memory))
            {
                warningStates.AppendLine($"\r\nThe following system {OS.GetAll().ComputerName} is currently at low ram capacity!\r\n[{Math.Round(memory.GetTotalFreeSpace(), 2)} GB of {Math.Round(memory.GetTotalSpace(), 2)} GB]");
            }
            if (warningStates.Length > 0)
                Alert(MonitorResult.Warning, warningStates.ToString());
            if (criticalStates.Length > 0)
                Alert(MonitorResult.Critical, criticalStates.ToString());
        }

        internal bool IsRunningLow(RAM Memory)
        {
            if (Memory.GetTotalFreeSpace() <= Configuration.Get<double>("monitoring.memorymonitor.spacewarningthreshhold"))
                return true;
            return false;
        }
        internal bool IsCriticallyLow(RAM Memory)
        {
            if (Memory.GetTotalFreeSpace() <= Configuration.Get<double>("monitoring.memorymonitor.spacecriticalthreshhold"))
                return true;
            return false;
        }
    }
}
