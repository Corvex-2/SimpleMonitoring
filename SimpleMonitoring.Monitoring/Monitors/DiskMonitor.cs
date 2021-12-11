using SimpleMonitoring.Monitoring.System;
using SimpleMonitoring.Utilites;
using System;
using System.Text;

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

                Configuration.Add("monitoring.diskmonitor.spacewarningthreshhold", 200d);
                Configuration.Add("monitoring.diskmonitor.spacecriticalthreshhold", 100d);
                Configuration.Add("monitoring.diskmonitor.spacewarningalertspan", new TimeSpan(3, 0, 0));
                Configuration.Add("monitoring.diskmonitor.spacecriticalalertspan", new TimeSpan(1, 0, 0));
            }
            SetAlertSpan(MonitorResult.Warning, Configuration.Get<TimeSpan>("monitoring.diskmonitor.spacewarningalertspan"));
            SetAlertSpan(MonitorResult.Critical, Configuration.Get<TimeSpan>("monitoring.diskmonitor.spacecriticalalertspan"));
        }

        internal override void Check()
        {
            var warningStates = new StringBuilder();
            var criticalStates = new StringBuilder();
            foreach(var disk in Disk.GetAll())
            {
                if(IsCriticallyLow(disk))
                {
                    criticalStates.AppendLine($"\r\nThe following disk in {OS.GetAll().ComputerName} is currently at critical capacity!\r\n{disk.DeviceName}-{disk.SerialNumber} [{Math.Round(disk.GetTotalFreeSpace(), 2)} GB of {Math.Round(disk.GetTotalSpace(), 2)} GB]");
                }
                else if(IsRunningLow(disk))
                {
                    warningStates.AppendLine($"\r\nThe following disk in {OS.GetAll().ComputerName} is currently at low capacity!\r\n{disk.DeviceName}-{disk.SerialNumber} [{Math.Round(disk.GetTotalFreeSpace(), 2)} GB of {Math.Round(disk.GetTotalSpace(), 2)} GB]");
                }
            }
            if(warningStates.Length > 0)
                Alert(MonitorResult.Warning, warningStates.ToString());
            if (criticalStates.Length > 0)
                Alert(MonitorResult.Critical, criticalStates.ToString());
        }

        internal bool IsRunningLow(Disk Disk)
        {
            if (Disk.GetTotalFreeSpace() <= Configuration.Get<double>("monitoring.diskmonitor.spacewarningthreshhold"))
                return true;
            return false;
        }
        internal bool IsCriticallyLow(Disk Disk)
        {
            if (Disk.GetTotalFreeSpace() <= Configuration.Get<double>("monitoring.diskmonitor.spacecriticalthreshhold"))
                return true;
            return false;
        }
    }
}
