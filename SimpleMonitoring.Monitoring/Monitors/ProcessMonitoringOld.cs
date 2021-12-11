using SimpleMonitoring.Monitoring.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class ProcessMonitoringOld
    {
        public static List<(string processName, bool critical)> ExecutablesToMonitor { get; private set; } = new List<(string processName, bool critical)>();

        public static void AddApplication(string ExecutableName, bool Critical = false)
        {
            if (!ExecutablesToMonitor.Contains((ExecutableName, Critical)) && ExecutableName.EndsWith(".exe"))
                ExecutablesToMonitor.Add((ExecutableName, Critical));
        }

        public ProcessMonitoringOld()
        {
            p_Threshholds.Add(new Notifier(AlertLevel.Warning, ProcessMonitoringWarning, 60 * 1000, new TimeSpan(3, 0, 0)));
            p_Threshholds.Add(new Notifier(AlertLevel.Critical, ProcessMonitoringCritical, 60 * 1000, new TimeSpan(1, 0, 0)));
        }

        protected (bool, string) ProcessMonitoringWarning()
        {
            StringBuilder Builder = new StringBuilder();
            var Processes = Process.GetAll();
            Builder.AppendLine("An important Process is currently not running!");
            bool hit = false;
            foreach (var m in ExecutablesToMonitor)
            {
                if (Processes.Where(x => x.Name == m.processName).Count() == 0 && !m.critical)
                {
                    hit = true;
                    Builder.AppendLine($"Process: {m.processName} could not be found!");
                }
            }
            if (hit)
                return (hit, Builder.ToString());
            return (false, "");
        }
        protected (bool, string) ProcessMonitoringCritical()
        {
            StringBuilder Builder = new StringBuilder();
            var Processes = Process.GetAll();
            Builder.AppendLine("A critical Process is currently not running!");
            bool hit = false;
            foreach (var m in ExecutablesToMonitor)
            {
                if (Processes.Where(x => x.Name == m.processName).Count() == 0 && m.critical)
                {
                    hit = true;
                    Builder.AppendLine($"Process: {m.processName} could not be found!");
                }
            }
            if (hit)
                return (hit, Builder.ToString());

            return (false, "");
        }
        private List<Notifier> p_Threshholds = new List<Notifier>();
    }
}
