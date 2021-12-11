using SimpleMonitoring.Monitoring.System;
using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class ProcessMonitor : Monitor
    {
        public static Config Configuration = new Config("SimpleMonitoring.Monitoring.ProcessMonitor");

        public ProcessMonitor(double Interval) : base(Interval) { }

        internal override void Initialize()
        {
            base.Initialize();
            if (Configuration.IsNew)
            {
                Logging.Log("[SimpleMonitoring.Monitoring.ProcessMonitor]", "It appears as if this is the first time the memory monitor is being executed. Initializing and populating the configuration now!");

                Configuration.Add("monitoring.processmonitor.applicationcriticalnotrunning", true);
                Configuration.Add("monitoring.processmonitor.applicationuncriticalnotrunning", true);
                Configuration.Add("monitoring.processmonitor.applicationuncriticalalertspan", new TimeSpan(3, 0, 0));
                Configuration.Add("monitoring.processmonitor.applicationcriticalalertspan", new TimeSpan(1, 0, 0));
            }
            SetAlertSpan(MonitorResult.Warning, Configuration.Get<TimeSpan>("monitoring.processmonitor.applicationuncriticalalertspan"));
            SetAlertSpan(MonitorResult.Critical, Configuration.Get<TimeSpan>("monitoring.processmonitor.applicationcriticalalertspan"));
        }

        internal override void Check()
        {
            var warningStates = new StringBuilder();
            var criticalStates = new StringBuilder();
            foreach (var proc in Process.GetAll())
            {
                if (IsCriticalProcess(proc))
                {
                    criticalStates.AppendLine($"\r\nThe following critical process is currently not running on {OS.GetAll().ComputerName}\r\n{proc.Name}");
                }
                else if (IsUnciritcalProcess(proc))
                {
                    warningStates.AppendLine($"\r\nThe following important process is currently not running on {OS.GetAll().ComputerName}\r\n{proc.Name}");
                }
            }
            if (warningStates.Length > 0)
                Alert(MonitorResult.Warning, warningStates.ToString());
            if (criticalStates.Length > 0)
                Alert(MonitorResult.Critical, criticalStates.ToString());
        }

        internal bool IsCriticalProcess(Process Proc)
        {
            return p_criticalProcesses.Where(x => x == Proc.Name).Count() > 0;
        }
        internal bool IsUnciritcalProcess(Process Proc)
        {
            return p_uncriticalProcesses.Where(x => x == Proc.Name).Count() > 0;
        }

        private List<string> p_criticalProcesses = new List<string>();
        private List<string> p_uncriticalProcesses = new List<string>();
    }
}
