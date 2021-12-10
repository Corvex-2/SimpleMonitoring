using System;
using System.Timers;
using SimpleMonitoring.Communication.Mailing;
using SimpleMonitoring.Utilites;

namespace SimpleMonitoring.Monitoring
{
    public class Notifier
    {
        public SMTP Messaging = new SMTP();
        public AlertLevel Level { get; private set; }
        public Func<(bool Alert, string Message)> Action { get; private set; }
        public TimeSpan AlertingSpan { get; private set; }

        public Notifier(AlertLevel Level, Func<(bool, string)> Action)
        {
            this.Level = Level;
            this.Action = Action;
            this.p_ActionInvoker.Elapsed += P_ActionInvoker_Elapsed;
            p_ActionInvoker.Start();
        }
        public Notifier(AlertLevel Level, Func<(bool, string)> Action, int Interval)
        {
            this.Level = Level;
            this.Action = Action;
            this.p_ActionInvoker = new Timer(Interval);
            this.p_ActionInvoker.Elapsed += P_ActionInvoker_Elapsed;
            p_ActionInvoker.Start();
        }
        public Notifier(AlertLevel Level, Func<(bool, string)> Action, int Interval, TimeSpan AlertingSpan)
        {
            this.Level = Level;
            this.Action = Action;
            this.p_ActionInvoker = new Timer(Interval);
            this.p_ActionInvoker.Elapsed += P_ActionInvoker_Elapsed;
            p_ActionInvoker.Start();
            this.AlertingSpan = AlertingSpan;
        }

        public void Reset()
        {
            p_LastAlert = DateTime.MinValue;
        }

        public void Start() => p_ActionInvoker.Start();
        public void Stop() => p_ActionInvoker.Stop();

        private void P_ActionInvoker_Elapsed(object sender, ElapsedEventArgs e)
        {
            var Result = Action?.Invoke();

            if(Result.HasValue && Result.Value.Alert && (AlertingSpan != null && DateTime.Now - p_LastAlert > AlertingSpan))
            {
                switch(Level)
                {
                    case AlertLevel.Info:
                        {
                            Logging.Log("[SIMPLE-MONITOR-MONITORING: INFO]", Result.Value.Message);
                            break;
                        }
                    case AlertLevel.Warning:
                        {
                            ////Messaging.SendMessage();
                            Logging.Log("[SIMPLE-MONITOR-MONITORING: WARNING]", Result.Value.Message);
                            break;
                        }
                    case AlertLevel.Critical:
                        {
                            //Send to Monitor
                            //Messaging.SendMail();
                            Logging.Log("[SIMPLE-MONITOR-MONITORING: CRITICAL]", Result.Value.Message);
                            break;
                        }
                }
                p_LastAlert = DateTime.Now;
            }
        }

        private DateTime p_LastAlert = DateTime.MinValue;
        private Timer p_ActionInvoker = new Timer(1000);
    }
}
