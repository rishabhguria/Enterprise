using Prana.LogManager;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Prana.Utilities.MiscUtilities
{
    public static class EmailsHelper
    {
        public static void MailSend(string Subject, string Body, string sender, string senderName, string senderPassword, string[] recipients, int port, string host, bool enableSSL, bool authenticationRequired, bool isBodyHtml = false, string[] ccRecipients = null, string[] bccRecipients = null)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.Subject = Subject;
                mailMsg.Body = Body;
                mailMsg.IsBodyHtml = isBodyHtml;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.From = new MailAddress(sender, senderName);
                mailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay;
                foreach (string recipient in recipients)
                {
                    mailMsg.To.Add(new MailAddress(recipient));
                }
                if (ccRecipients != null)
                {
                    foreach (string ccRecipient in ccRecipients)
                    {
                        mailMsg.CC.Add(new MailAddress(ccRecipient));
                    }
                }
                if (bccRecipients != null)
                {
                    foreach (string bccRecipient in bccRecipients)
                    {
                        mailMsg.Bcc.Add(new MailAddress(bccRecipient));         
                    }
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = host;
                smtp.Port = port;
                if (authenticationRequired)
                    smtp.Credentials = new NetworkCredential(sender, senderPassword);
                smtp.EnableSsl = enableSSL;

                smtp.Send(mailMsg);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //TODO: Need to refactor mail sending methods
        public static void SendMail(string filePath, string fileName, string bbgFileImportReceiverIDs, string bbgFileImportSenderID, string bbgFileImportMailSubject, string bbgFileImportMailBody, string bbgFileImportCCIDs, string bbgFileImportBCCIDs, string bbgFileImportSenderPWD, string bbgFileImportMailServer, int bbgFileImportMailServerSMTPPort, bool bbgFileImportSecureEmail, string sendername = "")
        {
            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(filePath, fileName)))
                {
                    MailMessage myMessage = new MailMessage();

                    string[] arrToAddresses = bbgFileImportReceiverIDs.Split(';');
                    for (int i = 0; i < arrToAddresses.Length; ++i)
                    {
                        myMessage.To.Add(new MailAddress(arrToAddresses[i].Trim()));
                    }

                    if (!string.IsNullOrEmpty(bbgFileImportCCIDs))
                    {
                        arrToAddresses = bbgFileImportCCIDs.Split(';');
                        for (int i = 0; i < arrToAddresses.Length; ++i)
                        {
                            myMessage.CC.Add(new MailAddress(arrToAddresses[i].Trim()));
                        }
                    }

                    if (!string.IsNullOrEmpty(bbgFileImportBCCIDs))
                    {
                        arrToAddresses = bbgFileImportBCCIDs.Split(';');
                        for (int i = 0; i < arrToAddresses.Length; ++i)
                        {
                            myMessage.Bcc.Add(new MailAddress(arrToAddresses[i].Trim()));
                        }
                    }

                    if (string.IsNullOrEmpty(sendername))
                        myMessage.From = new MailAddress(bbgFileImportSenderID);
                    else
                        myMessage.From = new MailAddress(bbgFileImportSenderID, sendername);
                    myMessage.Subject = bbgFileImportMailSubject;
                    myMessage.IsBodyHtml = true;
                    myMessage.Body = bbgFileImportMailBody;
                    Attachment myAttach = new Attachment(System.IO.Path.Combine(filePath, fileName));
                    myMessage.Attachments.Add(myAttach);
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = bbgFileImportMailServer;
                    smtpClient.Port = bbgFileImportMailServerSMTPPort;
                    smtpClient.EnableSsl = true;
                    if (!bbgFileImportSecureEmail)
                    {
                        smtpClient.EnableSsl = false;
                    }

                    smtpClient.Credentials = new NetworkCredential(bbgFileImportSenderID, bbgFileImportSenderPWD);

                    smtpClient.Send(myMessage);
                    smtpClient = null;
                    myMessage.Dispose();

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Mail has been sent to " + bbgFileImportReceiverIDs, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Unable to sent mail because file not found on " + System.IO.Path.Combine(filePath, fileName), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
