using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Proc = System.Diagnostics.Process;

namespace SimpleMonitoring.Monitoring.System
{
    public class Process
    {
        public string CSName;
        public string Description;
        public string Name;
        public string ProcessId;
        public string ExecutablePath;
        public Proc FrameworkProcess;
        public string WindowTitle;

        public Process(string CSName, string Description, string Name, string ProcessId, string ExecutablePath)
        {
            this.CSName = CSName;
            this.Description = Description;
            this.Name = Name;
            this.ProcessId = ProcessId;
            this.ExecutablePath = ExecutablePath;
            try
            {
                this.FrameworkProcess = Proc.GetProcessById(Convert.ToInt32(ProcessId));
                this.WindowTitle = FrameworkProcess.MainWindowTitle;
            }
            catch(Exception ex)
            {
                this.WindowTitle = "";
                this.FrameworkProcess = null;
            }
        }


        public static List<Process> GetAll() => WMI.GetProcessInfo();

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine($"Process Info for: {Name}\r\n  Window Title: {WindowTitle}\r\n  Description: {Description}\r\n  Name: {Name}\r\n  ProcessId: {ProcessId}\r\n  ExecutablePath: {ExecutablePath}");
            return Builder.ToString();
        }
    }
}
