using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using SimpleMonitoring.Utilites;
using Version = SimpleMonitoring.Utilites.Version;
using SimpleMonitoring.Monitoring.System;
using SimpleMonitoring.Monitoring;
using SimpleMonitoring.Monitoring.Monitors;
using SimpleMonitoring.Communication;
using System.Linq;
using SimpleMonitoring.Communication.TCP.Client;

namespace SimpleMonitoring.Agent
{
    public class Agent
    {
        public static Config Configuration = new Config("MonitoringAgent");
        public static void Main(string[] args) => Startup.Initialize();

        //public static Icon TrayIcon { get; private set; }
        //internal static Icon GetTrayIcon()
        //{
        //    var bmp = new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("SimpleMonitoring.Agent.index.png"));
        //    IntPtr Hicon = bmp.GetHicon();
        //    TrayIcon = Icon.FromHandle(Hicon);
        //    return TrayIcon;
        //}

        //Thread trayIcon = new Thread(() =>
        //{
        //    ProcessMonitoring.AddApplication("notepad.exe");
        //    new HarddiskMonitorOld();
        //    new RAMMonitor();
        //    new ProcessMonitoring();

        //    var notifyIcon = new NotifyIcon()
        //    {
        //        Icon = GetTrayIcon(),
        //        Text = "SimpleMonitoring Agent\r\nClick to Show."
        //    };
        //    notifyIcon.Click += (object sender, EventArgs e) =>
        //    {
        //        WIN32API.ShowWindow(WIN32API.GetConsoleWindow(), WIN32API.SW_SHOW);
        //    };
        //    notifyIcon.Visible = true;
        //    Application.Run();
        //});
        //trayIcon.SetApartmentState(ApartmentState.STA);
        //trayIcon.IsBackground = true;
        //trayIcon.Start();
    }
}
