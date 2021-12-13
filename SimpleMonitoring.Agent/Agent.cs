using SimpleMonitoring.Utilites;
namespace SimpleMonitoring.Agent
{
    public class Agent
    {
        public static Config Configuration = new Config("MonitoringAgent");
        public static void Main(string[] args) => Startup.Initialize();
    }
}
