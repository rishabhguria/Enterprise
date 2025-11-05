using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class EmailHelper
    {
        public static void Mail_ExportedFile(string smtp, int port, string sender, string reciever, string Cc, string Bcc, string username, string password,string emailsubject, string directorypath)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient(smtp);
                string senderEmail = sender;
                string receiverEmail = reciever;
                string cc = Cc;
                string bcc = Bcc;
                string subject = emailsubject;
                MailMessage mail = new MailMessage(senderEmail, receiverEmail, subject, MessageConstants.Email_Message);

                foreach (string fileName in Directory.GetFiles(directorypath))
                {
                    mail.Attachments.Add(new Attachment(fileName.ToString()));
                }

                if (!String.IsNullOrEmpty(receiverEmail) || !string.IsNullOrWhiteSpace(receiverEmail))
                {
                    string[] ToMuliId = receiverEmail.Split(',', ';');
                    foreach (string ToEMailId in ToMuliId)
                    {
                        mail.To.Add(new MailAddress(ToEMailId));
                    }
                }

                if (!String.IsNullOrEmpty(cc) || !string.IsNullOrWhiteSpace(cc))
                {
                    string[] CCId = cc.Split(',', ';');
                    foreach (string CCEmail in CCId)
                    {
                        mail.CC.Add(new MailAddress(CCEmail));
                    }
                }

                if (!String.IsNullOrEmpty(bcc) || !string.IsNullOrWhiteSpace(bcc))
                {
                    string[] bccid = bcc.Split(',', ';');
                    foreach (string bccEmailId in bccid)
                    {
                        mail.Bcc.Add(new MailAddress(bccEmailId));
                    }
                }
                mail.IsBodyHtml = true;
                SmtpServer.Port = port;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                foreach (Attachment attachment in mail.Attachments)
                {
                    attachment.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
