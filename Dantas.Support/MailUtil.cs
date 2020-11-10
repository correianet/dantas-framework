using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// Help send email using system.net configuration
    /// </summary>
    public static class MailUtil
    {
        /// <summary>
        /// Send mail async with task library.
        /// </summary>
        /// <param name="to">To mail expression.</param>
        /// <param name="subject">Subject text.</param>
        /// <param name="body">Body text with or not html markup.</param>
        /// <param name="html">Send body with html support.</param>
        /// <returns></returns>
        public static Task SendEmail(string to, string subject, string body, bool html = true)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var mail = new MailMessage { Subject = subject, Body = body, IsBodyHtml = html })
                using (var smtp = new SmtpClient())
                {
                    mail.To.Add(to);
                    smtp.Send(mail);
                }   
            });
        }
    }
}
