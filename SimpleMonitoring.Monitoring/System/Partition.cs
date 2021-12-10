using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Monitoring.System
{
    public class Partition
    {
        public string DeviceID { get; private set; }
        public string SerialNumber { get; private set; }
        public char VolumeLetter { get; private set; }

        public double TotalSpace { get; private set; }
        public double UsedSpace { get; private set; }
        public double FreeSpace { get; private set; }

        public Partition(string DeviceID, string SerialNumber, string TotalSpace, string FreeSpace)
        {
            this.DeviceID = DeviceID;
            this.SerialNumber = SerialNumber;
            this.TotalSpace = Convert.ToDouble(TotalSpace);
            this.FreeSpace = Convert.ToDouble(FreeSpace);
            this.UsedSpace = Convert.ToDouble(TotalSpace) - Convert.ToDouble(FreeSpace);
        }
    }
}
