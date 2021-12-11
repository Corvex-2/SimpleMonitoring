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
        public static Icon TrayIcon { get; private set; } 

        public static void Main(string[] args)
        {
            var DiskMonitor = new DiskMonitor(15000);
            DiskMonitor.Start();
            Console.ReadLine();
            //var m = new Random().Next(100000, 999999);

            //Console.Title = "SimpleMonitoring Agent";
            //Logging.Log("[SIMPLE-MONITOR-AGENT]", "Booting SimpleMonitoring Agent...");
            //if (Configuration.IsNew)
            //{
            //    Logging.Log("[SIMPLE-MONITOR-AGENT]", "This appears to be the first time the Agent is started, setting up config...");
            //    Configuration.Add("Version", new Version("00", "01", "00"));
            //    Logging.Log("[SIMPLE-MONITOR-AGENT]", "Configuration successfully setup.");
            //}
            //Console.Title = $"SimpleMonitoring Agent - Version: {Configuration.Get<Version>("Version").Complete} - running on: {Network.LocalIp} / {Network.PublicIp} [PUBLIC] {m}";
            //Logging.Log("[SIMPLE-MONITOR-AGENT]", "Youre running Version: " + Configuration.Get<Version>("Version").Complete);
            //Logging.Log("[SIMPLE-MONITOR-AGENT]", "Starting to monitor the system in 2 seconds.");
            //Thread.Sleep(2000);

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

            //while (true)
            //{
            //    Console.WriteLine("Press Enter to hide the window or enter Exit to Close.");
            //    var Input = Console.ReadLine();
            //    WIN32API.ShowWindow(WIN32API.GetConsoleWindow(), WIN32API.SW_HIDE);
            //    if(Input.ToLower() == "exit")
            //    {
            //        WIN32API.DestroyIcon(TrayIcon.Handle);
            //        Environment.Exit(0);
            //    }
            //}
        }

        //internal static Icon GetTrayIcon()
        //{
        //    var bmp = new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("SimpleMonitoring.Agent.index.png"));
        //    IntPtr Hicon = bmp.GetHicon();
        //    TrayIcon = Icon.FromHandle(Hicon);
        //    return TrayIcon;
        //}
    }
}
