using System;
using System.Collections.Generic;
using System.Management;

using SimpleMonitoring.Monitoring.System;

namespace SimpleMonitoring.Monitoring
{
    internal static class WMI
    {
        internal static ConnectionOptions ConnOptions = new ConnectionOptions();
        internal static ManagementScope Scope = new ManagementScope("\\\\localhost\\root\\cimv2", ConnOptions);
        internal static EnumerationOptions EnumOptions = new EnumerationOptions() { Timeout = EnumerationOptions.InfiniteTimeout, Rewindable = false, ReturnImmediately = true };

        static WMI()
        {
            Scope.Connect();
        }

        internal static List<Process> GetProcessInfo()
        {
            List<Process> query = new List<Process>();
            SelectQuery WIN32_PROCESS_SEARCHER = new SelectQuery("SELECT * FROM WIN32_PROCESS");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, WIN32_PROCESS_SEARCHER, EnumOptions);

            foreach(var WIN32_PROCESS in Searcher.Get())
            {
                var Process = new Process(WIN32_PROCESS.Properties["CSName"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["Description"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["Name"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["ProcessID"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["ExecutablePath"].Value?.ToString());
                query.Add(Process);
            }
            return query;
        }

        internal static System.OS GetOSInfo()
        {
            List<Process> query = new List<Process>();
            SelectQuery Win32_OperatingSystem_SEARCHER = new SelectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Win32_OperatingSystem_SEARCHER, EnumOptions);

            foreach (var WIN32_PROCESS in Searcher.Get())
            {

                var OS = new System.OS(WIN32_PROCESS.Properties["csname"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["Caption"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["Version"].Value?.ToString(),
                                              WIN32_PROCESS.Properties["Manufacturer"].Value?.ToString());
                return OS;
            }
            return null;
        }

        internal static List<RAM> GetRAMInfo()
        {
            List<RAM> query = new List<RAM>();
            SelectQuery WIN32_OPERATINGSYSTEM_SEARCHER = new SelectQuery("SELECT * FROM WIN32_OPERATINGSYSTEM");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, WIN32_OPERATINGSYSTEM_SEARCHER, EnumOptions);
            
            foreach(var WIN32_OPERATINGSYSTEM in Searcher.Get())
            {
                var w = Convert.ToDouble(WIN32_OPERATINGSYSTEM.Properties["TotalVisibleMemorySize"].Value.ToString());
                var x = Convert.ToDouble(WIN32_OPERATINGSYSTEM.Properties["TotalVirtualMemorySize"].Value.ToString());
                var y = Convert.ToDouble(WIN32_OPERATINGSYSTEM.Properties["FreePhysicalMemory"].Value.ToString());
                var z = Convert.ToDouble(WIN32_OPERATINGSYSTEM.Properties["FreeVirtualMemory"].Value.ToString());
                var RAM = new RAM(w, x, y, z);
                query.Add(RAM);
            }
            
            return query;
        }

        internal static List<Disk> GetDiskInfo()
        {
            List<Disk> query = new List<Disk>();
            SelectQuery WIN32_DISKDRIVE_QUERY = new SelectQuery("SELECT * FROM WIN32_DISKDRIVE");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, WIN32_DISKDRIVE_QUERY, EnumOptions);
            foreach(var WIN32_DISKDRIVE in Searcher.Get())
            {
                ManagementObjectSearcher ASSOCIATED_PARTITIONS_SEARCHER = new ManagementObjectSearcher("Associators of {Win32_DiskDrive.DeviceID='" +
                                                                                                         WIN32_DISKDRIVE.Properties["DeviceID"].Value.ToString() + "'}" +
                                                                                                         "where AssocClass=Win32_DiskDriveToDiskPartition");

                var Disk = new Disk(WIN32_DISKDRIVE.Properties["DeviceID"].Value.ToString(), WIN32_DISKDRIVE.Properties["Model"].Value.ToString(), WIN32_DISKDRIVE.Properties["SerialNumber"].Value.ToString());

                foreach(var WIN32_DISKPARTITION in ASSOCIATED_PARTITIONS_SEARCHER.Get())
                {
                    ManagementObjectSearcher ASSOCIATED_LOGICALDISK_SEARCHER = new ManagementObjectSearcher("Associators of {Win32_DiskPartition.DeviceID='" +
                                                                                                           WIN32_DISKPARTITION.Properties["DeviceID"].Value.ToString() + "'} " +
                                                                                                           "where AssocClass=Win32_LogicalDiskToPartition");
                    foreach(var WIN32_LOGICALDISK in ASSOCIATED_LOGICALDISK_SEARCHER.Get())
                    {
                        var Partition = new Partition(WIN32_LOGICALDISK.Properties["DeviceID"].Value.ToString(), WIN32_LOGICALDISK.Properties["VolumeSerialNumber"].Value.ToString(), WIN32_LOGICALDISK.Properties["Size"].Value.ToString(), WIN32_LOGICALDISK.Properties["FreeSpace"].Value.ToString());
                        Disk.AddParition(Partition);
                    }
                }
                query.Add(Disk);
            }
            return query;
        }
    }
}
