using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Mailing.Services
{
    public class MailerService
    {
        private readonly SmtpClient smtpClient;

        public MailerService(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }

        public async Task SendMail(string to, string subject, string body, string[] cc = null, string[] bcc = null, Attachment[] attachments = null)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(to);
                message.Subject = subject;

                message.IsBodyHtml = true;
                message.Body = body;

                if (cc != null)
                {
                    foreach (var address in cc)
                    {
                        message.CC.Add(address);
                    }
                }

                if (bcc != null)
                {
                    foreach (var address in bcc)
                    {
                        message.Bcc.Add(address);
                    }
                }

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }

                await smtpClient.SendMailAsync(message);
            }
        }

    }
    
}
