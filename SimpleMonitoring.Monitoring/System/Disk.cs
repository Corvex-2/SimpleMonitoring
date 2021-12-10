using System.Collections.Generic;
using System.Text;

namespace SimpleMonitoring.Monitoring.System
{
    public class Disk
    {
        public string DeviceID { get; private set; }
        public string DeviceName { get; private set; }
        public string SerialNumber { get; private set; }
        public List<Partition> Partitions { get; private set; }

        internal Disk(string DeviceID, string DeviceName, string SerialNumber)
        {
            this.DeviceID = DeviceID;
            this.DeviceName = DeviceName;
            this.SerialNumber = SerialNumber;
            Partitions = new List<Partition>();
        }

        internal void AddParition(Partition Partition)
        {
            Partitions.Add(Partition);
        }

        public static List<Disk> GetAll() => WMI.GetDiskInfo();

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.AppendLine($"Device: {DeviceName}\r\nDeviceID: {DeviceID}\r\nSerialNumber: {SerialNumber}");
            foreach(var p in Partitions)
            {
                Builder.AppendLine($"    Partition: {p.DeviceID}\r\n    VolumeSerialNumber: {p.SerialNumber}\r\n    Free Space: {p.FreeSpace}\r\n    Total Space: {p.TotalSpace}");
            }
            return Builder.ToString();
        }
    }
}
