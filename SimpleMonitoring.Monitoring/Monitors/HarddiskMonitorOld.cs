using System;
using System.Collections.Generic;
using System.Text;

using SimpleMonitoring.Monitoring.System;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public class HarddiskMonitorOld
    {
        public HarddiskMonitorOld()
        {
            p_Threshholds.Add(new Notifier(AlertLevel.Warning, HarddiskRunningLow, 60 * 1000, new TimeSpan(3, 0, 0)));
            p_Threshholds.Add(new Notifier(AlertLevel.Critical, HarddiskCritical, 60 * 1000, new TimeSpan(1, 0, 0)));
        }
        protected (bool, string) HarddiskRunningLow()
        {
            StringBuilder Builder = new StringBuilder();

            var Harddisks = Disk.GetAll();
            List<(Disk, Partition)> runningLow = new List<(Disk, Partition)>();
            foreach(var d in Harddisks)
            {
                foreach(var p in d.Partitions)
                {
                    double Free = p.FreeSpace / Math.Pow(1024, 3);
                    if (Free < 150)
                        runningLow.Add((d, p));
                }
            }

            if(runningLow.Count > 0)
            {
                Builder.AppendLine("The following Paritions are running low on space.");
                foreach(var l in runningLow)
                {
                    Builder.AppendLine($"   Partition: {l.Item2.DeviceID} on {l.Item1.DeviceName}-{l.Item1.SerialNumber} is running low on disk space.\r\n   less than 150 GB are remaining, current free capacity: {Math.Round(l.Item2.FreeSpace / Math.Pow(1024, 3), 2)} GB");
                }
                return (true, Builder.ToString());
            }
            return (false, string.Empty);
        }
        protected (bool, string) HarddiskCritical()
        {
            StringBuilder Builder = new StringBuilder();

            var Harddisks = Disk.GetAll();
            List<(Disk, Partition)> runningLow = new List<(Disk, Partition)>();
            foreach (var d in Harddisks)
            {
                foreach (var p in d.Partitions)
                {
                    double Free = p.FreeSpace / Math.Pow(1024, 3);
                    if (Free < 50)
                        runningLow.Add((d, p));
                }
            }

            if (runningLow.Count > 0)
            {
                Builder.AppendLine("The following Paritions are running low on space.");
                foreach (var l in runningLow)
                {
                    Builder.AppendLine($"   Partition: {l.Item2.DeviceID} on {l.Item1.DeviceName}-{l.Item1.SerialNumber} is at critical capacity!.\r\n   less than 50 GB are remaining, current free capacity: {Math.Round(l.Item2.FreeSpace / Math.Pow(1024, 3), 2)} GB");
                }
                return (true, Builder.ToString());
            }
            return (false, string.Empty);
        }
        private List<Notifier> p_Threshholds = new List<Notifier>();
    }
}
