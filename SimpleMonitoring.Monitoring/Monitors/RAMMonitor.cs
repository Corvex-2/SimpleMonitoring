using SimpleMonitoring.Monitoring.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class RAMMonitor
    {
        public RAMMonitor()
        {
            p_Treshholds.Add(new Notifier(AlertLevel.Warning, RAMWarning, 60 * 1000, new TimeSpan(3, 0, 0)));
            p_Treshholds.Add(new Notifier(AlertLevel.Critical, RAMCritical, 60 * 1000, new TimeSpan(1, 0, 0)));
        }

        protected (bool, string) RAMWarning()
        {
            StringBuilder Builder = new StringBuilder();

            var mRAM = RAM.GetAll().FirstOrDefault();
            if (mRAM != null && mRAM != default(RAM) && mRAM.FreePhysicalMemory / Math.Pow(1024, 2) < 2.5)
            {
                Builder.AppendLine("Your System is currently running low on memory");
                Builder.AppendLine($"less than 2,5 GB of RAM are remaining, currently free memory {Math.Round(mRAM.FreePhysicalMemory / Math.Pow(1024, 2), 2)} GB");
                return (true, Builder.ToString());
            }
            return (false, string.Empty);
        }

        protected (bool, string) RAMCritical()
        {
            StringBuilder Builder = new StringBuilder();

            var nRAM = RAM.GetAll().FirstOrDefault();
            if (nRAM != null && nRAM != default(RAM) && nRAM.FreePhysicalMemory / Math.Pow(1024, 2) < 1.5)
            {
                Builder.AppendLine("Your System is currently at critical memory!");
                Builder.AppendLine($"less than 1,5 GB of RAM are remaining, currently free memory {Math.Round(nRAM.FreePhysicalMemory / Math.Pow(1024, 2), 2)} GB");
                return (true, Builder.ToString());
            }
            return (false, string.Empty);
        }
        private List<Notifier> p_Treshholds = new List<Notifier>();
    }
}
