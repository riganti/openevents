using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using OpenEvents.Backend.Mailing.Model;

namespace OpenEvents.Backend.Mailing.Services
{
    public class MailerService
    {
        private readonly SmtpClient smtpClient;
        private readonly TemplateProcessor templateProcessor;

        public MailerService(SmtpClient smtpClient, TemplateProcessor templateProcessor)
        {
            this.smtpClient = smtpClient;
            this.templateProcessor = templateProcessor;
        }

        public async Task SendMail<T>(string to, MailTemplateDTO template, MailTemplateDTO globalTemplate, T data, string[] cc = null, string[] bcc = null, Attachment[] attachments = null)
        {
            var body = await templateProcessor.TransformBody(template, data);

            var globalBody = await templateProcessor.TransformBody(globalTemplate, new GlobalTemplateDataDTO()
            {
                Html = body,
                Subject = template.Subject
            });

            await SendMail(template.FromAddress, to, template.Subject, globalBody, cc, bcc, attachments);
        }

        public async Task SendMail(string from, string to, string subject, string body, string[] cc = null, string[] bcc = null, Attachment[] attachments = null)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(from);
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
