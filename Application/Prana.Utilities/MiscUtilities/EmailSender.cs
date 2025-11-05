using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;

namespace Prana.Utilities.MiscUtilities
{
    public class EmailSender
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="file">The file.</param>
        /// <param name="status">The status.</param>
        /// <param name="OnMessage">The on message.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool SendEmail(ThirdPartyEmail settings, List<string> mailAttachments, string status, string body)
        {
            try
            {
                if (settings != null && settings.Enabled)
                {
                    //check for mailTo data

                    if (string.IsNullOrEmpty(settings.MailTo))
                    {
                        //No Mail To was defined. Please review email settings
                        return false;
                    }
                    //
                    else if (settings.MailTo.Contains(",") || settings.MailTo.Contains(":"))
                    {
                        //Email not Sent, Please use only ';' between email Ids. Review email settings                     
                        return false;
                    }
                    //check for MailFrom data
                    if (string.IsNullOrEmpty(settings.MailFrom))
                    {
                        //No Mail From was defined. Please review email settings
                        return false;
                    }

                    //check for CcTo data
                    if (!string.IsNullOrEmpty(settings.CcTo) && (settings.CcTo.Contains(",") || settings.CcTo.Contains(":")))
                    {
                        //Email not Sent, Please use only ';' between email Ids in CC. Review email settings
                        return false;
                    }

                    SmtpClient client = new SmtpClient(settings.Smtp, settings.Port);
                    client.EnableSsl = settings.SSLEnabled;
                    //NOTE: send Completed event not added
                    //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                    client.Credentials = new System.Net.NetworkCredential(settings.UserName, settings.Password);
                    MailMessage oMsg = new MailMessage();
                    if (settings.CcTo != null)
                    {
                        if (settings.CcTo.Contains(";"))
                        {
                            String[] emailIDsList = settings.CcTo.Split(';');
                            foreach (string emailId in emailIDsList)
                            {
                                oMsg.CC.Add(new MailAddress(emailId));
                            }
                        }
                        else
                        {
                            oMsg.CC.Add(new MailAddress(settings.CcTo));
                        }
                    }
                    if (settings.MailTo.Contains(";"))
                    {
                        String[] emailIDsList = settings.MailTo.Split(';');
                        foreach (string emailId in emailIDsList)
                        {
                            oMsg.To.Add(new MailAddress(emailId));
                        }
                    }
                    else
                    {
                        oMsg.To.Add(new MailAddress(settings.MailTo));
                    }

                    oMsg.From = new MailAddress(settings.MailFrom);

                    if (string.IsNullOrEmpty(settings.Subject) == false)
                    {
                        oMsg.Subject = string.Format(settings.Subject, status);
                    }
                    else
                    {
                        oMsg.Subject = string.Format("Nirvana EOD Mail");//, status);
                    }
                    oMsg.Body = body;
                    oMsg.Priority = settings.Priority;
                    oMsg.IsBodyHtml = true;
                    if (mailAttachments != null)
                    {
                        foreach (String file in mailAttachments)
                        {
                            oMsg.Attachments.Add(new Attachment(file));
                        }
                    }
                    //modified to sending in async - omshiv
                    client.SendAsync(oMsg, oMsg);
                    //Email sent
                }
                return true;
            }
            catch (Exception ex)
            {
                //Error in Sending mail! Please review email settings.

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }

                return false;
            }

        }

        //void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    try
        //    {
        //        MailMessage mMsg = e.UserState as MailMessage;
        //        if (mMsg != null)
        //        {
        //            mMsg.Dispose();
        //        }
        //        //OnStatus(this, new StatusEventArgs("Archiving files"));
        //        if (File.Exists(this.Format.ArchivePath))
        //        {
        //            File.Delete(Format.ArchivePath);
        //        }
        //        if (File.Exists(this.Format.LogPath))
        //        {
        //            File.Delete(Format.LogPath);
        //        }
        //        File.Move(this.Format.FilePath, this.Format.ArchivePath);
        //        File.Move(LogFile, this.Format.LogPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

    }
}
