using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SimpleMonitoring.Agent
{
    public static class TrayIcon
    {
        internal static bool IsConsoleVisible { get; private set; } = false;

        public static Icon Icon
        {
            get
            {
                return GetTrayIcon();
            }
        }

        public static void Initialize()
        {
            Thread trayIcon = new Thread(() =>
            {
                var notifyIcon = new NotifyIcon()
                {
                    Icon = GetTrayIcon(),
                    Text = "SimpleMonitoring Agent\r\nClick to Show."
                };
                notifyIcon.Click += (object sender, EventArgs e) =>
                {
                    if (!IsConsoleVisible)
                    {
                        WIN32API.ShowWindow(WIN32API.GetConsoleWindow(), WIN32API.SW_SHOW);
                        IsConsoleVisible = true;
                    }
                    else
                    {
                        WIN32API.ShowWindow(WIN32API.GetConsoleWindow(), WIN32API.SW_HIDE);
                        IsConsoleVisible = false;
                    }
                };
                notifyIcon.Visible = true;
                Application.Run();
            });
            trayIcon.SetApartmentState(ApartmentState.STA);
            trayIcon.IsBackground = true;
            trayIcon.Start();
        }

        internal static byte[] DownloadIcon()
        {
            using(WebClient c = new WebClient())
            {
                var bytesIcon = c.DownloadData("https://avatars.githubusercontent.com/t/4125136?s=280&v=4");
                return bytesIcon;
            }
        }

        internal static Icon GetTrayIcon()
        {
            var bmp = new Bitmap(new MemoryStream(DownloadIcon()));
            IntPtr Hicon = bmp.GetHicon();
            var ico = Icon.FromHandle(Hicon);
            return ico;
        }
    }
}
