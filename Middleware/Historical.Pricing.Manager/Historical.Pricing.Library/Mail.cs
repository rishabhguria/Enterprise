using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Mail
    /// </summary>
    /// <remarks></remarks>
    public class Mail
    {
        /// <summary>
        /// Sends the log.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public static void Send(MailSettings settings)
        {
            
            SmtpClient client = new SmtpClient(settings.Smtp, settings.Port);
            client.Credentials = new System.Net.NetworkCredential(settings.User, settings.Password);


            MailMessage oMsg = new MailMessage { From = new MailAddress(settings.From) };
                                
            foreach (var MailTo in settings.To)
            {
                oMsg.To.Add(new MailAddress(MailTo));
            }

            oMsg.Subject = settings.Subject;
            oMsg.Body = settings.Body;



            foreach (var file in settings.Attachments)
            {
                if (System.IO.File.Exists(file))
                {
                    oMsg.Body += "\n\n" + System.IO.File.ReadAllText(file);
                    oMsg.Attachments.Add(new Attachment(file));
                }
            }
            client.Send(oMsg);          
        }
    }
}
