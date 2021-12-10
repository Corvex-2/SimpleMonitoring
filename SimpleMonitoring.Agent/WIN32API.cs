using System;
using System.Runtime.InteropServices;

namespace SimpleMonitoring.Agent
{
    public static class WIN32API
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);

        public const int SW_SHOW = 0x05;
        public const int SW_HIDE = 0x00;
    }
}
