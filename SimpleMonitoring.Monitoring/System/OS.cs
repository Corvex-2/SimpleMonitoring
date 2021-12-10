using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Monitoring.System
{
    public class OS
    {
        public string ComputerName { get; private set; }
        public string OperatingSystem { get; private set; }
        public string Version { get; private set; }
        public string Manufacturer { get; private set; }

        public OS(string ComputerName, string OperatingSystem, string Version, string Manufacturer)
        {
            this.ComputerName = ComputerName;
            this.OperatingSystem = OperatingSystem;
            this.Version = Version;
            this.Manufacturer = Manufacturer;
        }

        public static OS GetAll() => WMI.GetOSInfo();
    }
}
