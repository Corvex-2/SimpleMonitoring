using System;
using System.IO;

namespace SimpleMonitoring.Utilites
{
    public static class Logging
    {
        public static string PATH 
        { 
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SimpleMonitoring");
            }
        }
        public static string FILE
        {
            get
            {
                return Path.Combine(PATH, "Monitor.log");
            }
        }
        public static void Log(string Prefix, string Message)
        {
            if (!Directory.Exists(PATH))
                Directory.CreateDirectory(PATH);

            var currentLogText = File.Exists(FILE) ? File.ReadAllText(FILE) : "";
            var additionalLogText = Prefix + Environment.NewLine + Message;
            var totalLogText = currentLogText + (currentLogText == "" ? "" : Environment.NewLine) + additionalLogText;
            try
            {
                File.WriteAllText(FILE, totalLogText);
            }
            catch(Exception ex) { }

            Console.WriteLine(additionalLogText);
        }
    }
}
