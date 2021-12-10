using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace SimpleMonitoring.Communication.Mailing
{
    public class SMTP
    {
        public Config Configuration { get; private set; } = new Config("COMMUNICATION.SMTP");
        public SmtpClient SmtpClient { get; private set; }

        public SMTP()
        {
            if(Configuration.IsNew)
            {
                Logging.Log("[SIMPLE-MONITORING-COMMUNICATION]", "It appears as if you're running this Application for the first time. Setting up config.");
                Configuration.Add("smtp-server", "smtp.strato.de");
                Configuration.Add("smtp-server-port", 587);
                Configuration.Add("smtp-username", "info@smtpserver.de");
                Configuration.Add("smtp-password", "USERPASS");
                Configuration.Add("smtp-isexchange", false);
                Configuration.Add("smtp-sender", "info@smtpserver.de");
                Configuration.Add("smtp-displayname", "INFO");
                Logging.Log("[SIMPLE-MONITORING-COMMUNICATION]", "finished setting up config.");
            }
            SmtpClient = new SmtpClient(Configuration.Get<string>("smtp-server"), Configuration.Get<int>("smtp-server-port"))
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(Configuration.Get<string>("smtp-username"), Configuration.Get<string>("smtp-password")),
            };
        }

        public void SendMail(string Subject, string Text, string[] Recipients, string[] CC = null, string [] BCC = null)
        {
            if (Configuration.Get<bool>("smtp-isexchange"))
                throw new InvalidOperationException("Sending E-Mails through an exchange is currently not supported.");

            var Message = new MailMessage()
            {
                IsBodyHtml = true,
                From = new MailAddress(Configuration.Get<string>("smtp-sender"), Configuration.Get<string>("smtp-displayname")),
            };
            if (Recipients == null || Recipients.Where(x => x.Contains("@") && x.Contains(".")).Count() <= 0)
                throw new InvalidOperationException("Recipients cannot be null or empty!");

            foreach(var rec in Recipients.Where(x => x.Contains("@") && x.Contains(".")))
            {
                Message.To.Add(new MailAddress(rec));
            }
            if (CC != null && CC.Length > 0)
            {
                foreach (var cc in CC.Where(x => x.Contains("@") && x.Contains(".")))
                {
                    Message.To.Add(new MailAddress(cc));
                }
            }
            if (BCC != null & BCC.Length > 0)
            {
                foreach (var bcc in BCC.Where(x => x.Contains("@") && x.Contains(".")))
                {
                    Message.To.Add(new MailAddress(bcc));
                }
            }

            Message.Subject = Subject;
            Message.Body = Text;
            SmtpClient.Send(Message);
        }
    }
}
