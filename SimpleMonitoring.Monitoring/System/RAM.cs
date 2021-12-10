using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Monitoring.System
{
    public class RAM
    {
        public double TotalVisibleMemorySize;
        public double TotalVirtualMemory;
        public double FreePhysicalMemory;
        public double FreeVirtualMemory;

        public RAM(double TotalVisibleMemorySize, double TotalVirtualMemory, double FreePhysicalMemory, double FreeVirtualMemory)
        {
            this.TotalVisibleMemorySize = TotalVisibleMemorySize;
            this.TotalVirtualMemory = TotalVirtualMemory;
            this.FreePhysicalMemory = FreePhysicalMemory;
            this.FreeVirtualMemory = FreeVirtualMemory;
        }

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine($"Total Physical Memory: {TotalVisibleMemorySize} KB\r\nTotal Virtual Memory: {TotalVirtualMemory} KB\r\nFree Physcial Memory: {FreePhysicalMemory} KB\r\nFree Virtual Memory: {FreeVirtualMemory} KB");
            return Builder.ToString();
        }

        public static List<RAM> GetAll() => WMI.GetRAMInfo();
    }
}
