using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Timers;

namespace SimpleMonitoring.Monitoring.Monitors
{
    public abstract class Monitor
    {
        public double Interval
        {
            get { return p_Check.Interval; }
            set { p_Check.Interval = value; }
        }

        public Monitor(double Interval) 
        {
            this.Interval = Interval;
            Initialize();
        }

        internal abstract void Check();

        internal virtual void Initialize()
        {
            p_Check.Elapsed += (s, e) => 
            {
                Check();
            };
        }
        public void Start() => p_Check.Start();
        public void Stop() => p_Check.Stop();

        internal void Alert(MonitorResult Result, string Message)
        {
            if (p_AlertSpans.ContainsKey(Result) && p_LastAlerts.ContainsKey(Result) && DateTime.UtcNow - p_LastAlerts[Result] < p_AlertSpans[Result])
                return;
            if (!p_LastAlerts.ContainsKey(Result))
                p_LastAlerts.Add(Result, DateTime.UtcNow);
            else
                p_LastAlerts[Result] = DateTime.UtcNow;

            switch (Result)
            {
                case MonitorResult.Critical:
                    {
                        if (Message.Length > 0 && !string.IsNullOrWhiteSpace(Message))
                        {
                            Logging.Log("[SimpleMonitoring.Monitor: ERROR]", Message);
                        }
                        break;
                    }
                case MonitorResult.Warning:
                    {
                        if (Message.Length > 0 && !string.IsNullOrWhiteSpace(Message))
                        {
                            Logging.Log("[SimpleMonitoring.Monitor: WARNING]", Message);
                        }
                        break;
                    }
                case MonitorResult.Success:
                default:
                    {
                        if (Message.Length > 0 && !string.IsNullOrWhiteSpace(Message))
                        {
                            Logging.Log("[SimpleMonitoring.Monitor]", Message);
                        }
                        break;
                    }
            }
        }
        public void ResetAlertCooldown(MonitorResult Key)
        {
            if (p_LastAlerts.ContainsKey(Key))
                p_LastAlerts[Key] = DateTime.MinValue;
        }
        public void SetAlertSpan(MonitorResult Key, TimeSpan AlertSpan)
        {
            if(!p_AlertSpans.ContainsKey(Key))
            {
                p_AlertSpans.Add(Key, AlertSpan);
                return;
            }
            p_AlertSpans[Key] = AlertSpan;
        }

        private Dictionary<MonitorResult, DateTime> p_LastAlerts = new Dictionary<MonitorResult, DateTime>();
        private Dictionary<MonitorResult, TimeSpan> p_AlertSpans = new Dictionary<MonitorResult, TimeSpan>();
        private Timer p_Check = new Timer(1000);
    }
}
