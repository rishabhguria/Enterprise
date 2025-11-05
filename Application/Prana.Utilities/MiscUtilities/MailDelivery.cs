using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.IO;
using System.Net.Mail;


namespace Prana.Utilities.MiscUtilities
{
    public class MailDelivery : IDelivery
    {
        #region IDeliveryClass Members

        public bool SendFile(object objDetails)
        {
            bool isAllFilesTransfered = true;
            try
            {
                MailDeliveryParameters mailDeliveryParameters = (MailDeliveryParameters)objDetails;
                isAllFilesTransfered = MainMailSender(mailDeliveryParameters);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isAllFilesTransfered;
        }

        #endregion

        private bool MainMailSender(MailDeliveryParameters mailDeliveryParameters)//(string collatedFileFullPath, string mailReceiver, string clientMailSubject, string clientMailBody)
        {
            try
            {
                //MailMessage mail = new MailMessage();
                //mail.From =new MailAddress(mailDeliveryParameters.SenderMailID);
                //mail.To.Add(mailDeliveryParameters.ReceiverMailID);
                //mail.Subject = mailDeliveryParameters.MailSubject;
                //mail.Body = mailDeliveryParameters.MailBody;

                //SmtpClient smtpServer = new SmtpClient(mailDeliveryParameters.HostID);
                //smtpServer.Port = 587;
                //smtpServer.Credentials = new System.Net.NetworkCredential(mailDeliveryParameters.SenderMailID, mailDeliveryParameters.SenderPassword);
                // smtpServer.EnableSsl = true;

                if (mailDeliveryParameters.FileToBeAttachedWithFullPath != null)
                {
                    foreach (string attach in mailDeliveryParameters.FileToBeAttachedWithFullPath)
                    {
                        if (attach != null)
                        {
                            mailDeliveryParameters.Attachments.Add(attach);
                        }
                    }
                }
                //string[] filePaths = Directory.GetFiles(mailDeliveryParameters.FileToBeAttachedWithFullPath + "\\");
                //foreach (string currentFile in filePaths)
                //{
                //    bool isFileExist = System.IO.File.Exists(currentFile);
                //    if (isFileExist)
                //    {
                //        DateTime fileCreationDateTime = System.IO.File.GetCreationTime(currentFile);
                //        int comparison = fileCreationDateTime.Date.CompareTo(DateTime.Now.Date);
                //        if (comparison == 0)
                //        {
                //            mailDeliveryParameters.Attachments.Add(currentFile);
                //        }
                //    }
                //}

                SendMail(mailDeliveryParameters);

                return true;

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public bool SendMail(MailDeliveryParameters mailDeliveryParameters)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(mailDeliveryParameters.SenderMailID);
                mail.To.Add(mailDeliveryParameters.ReceiverMailID);
                mail.Subject = mailDeliveryParameters.MailSubject;
                mail.Body = mailDeliveryParameters.MailBody;

                SmtpClient smtpServer = new SmtpClient(mailDeliveryParameters.HostID);
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential(mailDeliveryParameters.SenderMailID, mailDeliveryParameters.SenderPassword);
                // smtpServer.EnableSsl = true;

                foreach (String attachment in mailDeliveryParameters.Attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }
                //bool isFileExist = System.IO.File.Exists(mailDeliveryParameters.FileToBeAttachedWithFullPath);
                //if (isFileExist)
                //{
                //    mail.Attachments.Add(new Attachment(mailDeliveryParameters.FileToBeAttachedWithFullPath));
                //}

                smtpServer.Send(mail);

                return true;

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }


        #region Extra Code

        //private bool MainMailSender(string collatedFilePath, string collatedFileName, string mailReceiver, string mailSender, string clientMailSubject, string clientMailBody)
        //private bool MainMailSender(string collatedFilePath, string collatedFileName, string mailReceiver, string clientMailSubject, string clientMailBody)
        //{
        //    try
        //    {
        //        //string collatedFileFullPath = collatedFilePath + "\\" + collatedFileName;
        //        bool isFileExist = System.IO.File.Exists(collatedFileFullPath);
        //        if (isFileExist)
        //        {
        //            DateTime fileCreationDateTime = System.IO.File.GetCreationTime(collatedFileFullPath);
        //            int comparison = fileCreationDateTime.Date.CompareTo(DateTime.Now.Date);
        //            if (comparison == 0)
        //            {
        //                MailMessage myMessage = new MailMessage();
        //                myMessage.To = mailReceiver; // "vinodnayal1981@gmail.com";
        //                myMessage.From = "support@nirvana-sol.com";  //"support@nirvana-sol.com";
        //                // myMessage.Priority = this.MessagePriority;
        //                myMessage.Subject = clientMailSubject;
        //                //StringBuilder sb = new StringBuilder();
        //                //sb.Append(clientMailSubject);
        //                myMessage.Body = clientMailBody;//sb.ToString();
        //                MailAttachment myAttach = new MailAttachment(collatedFileFullPath);

        //                myMessage.Attachments.Add(myAttach);
        //                SmtpMail.Send(myMessage);
        //                logger.Info("Mail has been sent successfully");
        //                return true;
        //            }
        //            else
        //            {
        //                logger.Info("No file found to send for Today  " + DateTime.Now.Date);
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            logger.Info("Collated file not found");
        //            return false;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //    }
        //    return false;
        //}

        #endregion
    }
}
