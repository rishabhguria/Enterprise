using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.Global;
using Prana.LogManager;
using Prana.NotificationManager.BLL.Extractor;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks.Dataflow;

namespace Prana.NotificationManager.BLL.Processor
{
    internal class EmailProcessor : INotificationProcessor, IDisposable
    {
        //Queue<MailMessage> _pendingMails = new Queue<MailMessage>();
        //bool _isClientBusy = false;

        String _senderAddress = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderAddress");
        String _senderName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderName");
        String _senderId = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderId");
        String _password = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailPassword");
        String _hostName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailHostName");
        int _port = Convert.ToInt16(ConfigurationHelper.Instance.GetAppSettingValueByKey("MailPort"));
        String _recipients = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailRecieverAddress");
        bool _enableSSL = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableSSL"));
        //List<String> _recipientsBCC = new List<string>();
        private List<String> _defaultEmailId = new List<string>();

        private SmtpClient _client;
        BufferBlock<MailMessage> dataBuffer;

        /// <summary>
        /// to count alerts received
        /// </summary>
        private int alertsCount = 0;

        /// <summary>
        /// Check the number of alerts
        /// </summary>
        private static int _totalAlerts = 1;
        public static int TotalAlerts
        {
            get { return _totalAlerts; }
            set { _totalAlerts = value; }
        }

        /// <summary>
        /// final message to send to the mail
        /// </summary>
        String finalMessage = String.Empty;

        /// <summary>
        /// Store mailID for toList,
        /// </summary>
        List<String> recipientsTo = new List<String>();

        /// <summary>
        ///  Store mail id for ccList 
        /// </summary>
        List<String> recipientsCC = new List<String>();

        /// <summary>
        /// Store mails id for BccList
        /// </summary>
        List<String> recipientsBCC = new List<String>();

        /// <summary>
        /// Initialize sender settings..
        /// </summary>
        internal EmailProcessor()
        {
            try
            {
                InitializeClient();
                this._defaultEmailId = EmailFormatter.GetRulewiseList(_recipients);
                dataBuffer = new BufferBlock<MailMessage>();
                System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(dataBuffer)).ConfigureAwait(false);
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
        }

        /// <summary>
        /// Initialize sender settings.
        /// </summary>
        private void InitializeClient()
        {
            try
            {
                _client = new SmtpClient();
                _client.UseDefaultCredentials = true;
                _client.Host = _hostName;//host of your mail account
                _client.EnableSsl = _enableSSL;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = _senderId;
                NetworkCred.Password = _password;

                _client.UseDefaultCredentials = true;
                _client.Credentials = NetworkCred;
                _client.Port = _port;
                _client.Timeout = 50000;
                //_client.SendCompleted += new SendCompletedEventHandler(_client_SendCompleted);
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
        }

        /// <summary>
        /// If sending has error thow error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void _client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //    try
        //    {
        //        SendNextMail();
        //        if (e.Error != null)
        //            throw e.Error;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        #region INotificationProcessor Members

        /// <summary>
        /// Process for mail.
        /// remove repeated email id.
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="notification"></param>
        public void Process(Alert alert, NotificationSetting notification, NotificationStrategy notificationStrategy)
        {
            try
            {
                List<String> recipientsTo = new List<string>();
                List<String> recipientsCC = new List<string>();
                List<String> recipientsBCC = new List<string>();
                String alertSubject = notification.EmailSubject;
                string emailSub = string.Empty;
                if (alert.UserNotes.Equals(BusinessObjects.Compliance.Constants.PreTradeConstants.CONST_USER_NOTE))
                {
                    string userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(alert.ActionUser);
                    emailSub = "A previous request made by the " + userName + " to approve a trade that would breach compliance has been REPLACED and NEEDS NO ACTION. Please review the details in the Alert History module.";
                }
                if (notification.EmailEnabled)
                {
                    recipientsTo.AddRange(EmailFormatter.GetRulewiseList(notification.EmailToList));
                    recipientsCC.AddRange(EmailFormatter.GetRulewiseList(notification.EmailCCList));
                    foreach (String defaultemail in _defaultEmailId)
                    {
                        if (!recipientsTo.Contains(defaultemail) && !recipientsCC.Contains(defaultemail))
                            recipientsBCC.Add(defaultemail);
                    }
                    if (recipientsTo.Count > 0 || recipientsCC.Count > 0 || recipientsBCC.Count > 0)
                    {
                        FormatAndSendMail(alert, recipientsTo, recipientsCC, recipientsBCC, notificationStrategy, alertSubject, emailSub);
                    }
                }
                else
                {
                    recipientsBCC.AddRange(_defaultEmailId);
                    if (recipientsBCC.Count > 0)
                        FormatAndSendMail(alert, recipientsTo, recipientsCC, recipientsBCC, notificationStrategy, alertSubject, emailSub);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Processes the in one batch.
        /// </summary>
        /// <param name="alerts">The alerts.</param>
        /// <param name="notificationStrategy">The notification strategy.</param>
        public void ProcessInOneBatch(List<Alert> alerts, NotificationStrategy notificationStrategy)
        {
            try
            {
                RuleBase rule = NotificationCache.GetInstance().GetRuleForAlert(alerts[0]);
                NotificationSetting notification = NotificationExtractionManager.GetInstance().Extract(alerts[0], rule);
                List<String> recipientsTo = new List<string>();
                List<String> recipientsCC = new List<string>();
                List<String> recipientsBCC = new List<string>();
                String alertSubject = notification.EmailSubject;
                if (notification.EmailEnabled)
                {
                    recipientsTo.AddRange(EmailFormatter.GetRulewiseList(notification.EmailToList));
                    recipientsCC.AddRange(EmailFormatter.GetRulewiseList(notification.EmailCCList));
                    foreach (String defaultemail in _defaultEmailId)
                    {
                        if (!recipientsTo.Contains(defaultemail) && !recipientsCC.Contains(defaultemail))
                            recipientsBCC.Add(defaultemail);
                    }
                }
                else
                    recipientsBCC.AddRange(_defaultEmailId);

                if (recipientsTo.Count > 0 || recipientsCC.Count > 0 || recipientsBCC.Count > 0)
                {
                    String message = String.Empty;
                    EmailFormatter.FormatAlertsInOneMessage(alerts, ref alertSubject, out message);
                    MailMessage msg = ConstructMail(alertSubject, message, recipientsTo, recipientsCC, recipientsBCC);
                    SendEmail(msg);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// Format and send mail.
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        private void FormatAndSendMail(Alert alert, List<String> toList, List<String> ccList, List<String> bccList, NotificationStrategy notificationStrategy, string alertSubject, string emailSub = null)
        {
            try
            {
                String message = String.Empty;
                alertsCount++;
                if (notificationStrategy.Equals(NotificationStrategy.Alerting))
                    EmailFormatter.FormatAlertMessage(alert, ref alertSubject, out message);

                if (notificationStrategy.Equals(NotificationStrategy.Approval))
                    EmailFormatter.FormatApprovalMessage(alert, out alertSubject, out message, emailSub);

                if (TotalAlerts > 1)
                    finalMessage = String.Concat(finalMessage, message);
                else
                    finalMessage = message;

                foreach (string recipient in toList)
                {
                    if (!recipientsTo.Contains(recipient))
                        recipientsTo.Add(recipient);
                }
                foreach (string recipient in ccList)
                {
                    if (!recipientsCC.Contains(recipient))
                        recipientsCC.Add(recipient);
                }
                foreach (string recipient in bccList)
                {
                    if (!recipientsBCC.Contains(recipient))
                        recipientsBCC.Add(recipient);
                }

                if (alertsCount == TotalAlerts)
                {
                    if (notificationStrategy.Equals(NotificationStrategy.Alerting) && TotalAlerts > 1)
                        alertSubject = "Nirvana: Compliance and Alerting - PreTrade - Basket Orders";
                    MailMessage msg = ConstructMail(alertSubject, finalMessage, recipientsTo, recipientsCC, recipientsBCC);
                    SendEmail(msg);
                    alertsCount = 0;
                    finalMessage = string.Empty;
                    recipientsTo.Clear();
                    recipientsCC.Clear();
                    recipientsBCC.Clear();
                }
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
        }

        /// <summary>
        /// Constructs Mail message.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        /// <param name="_recipientsBCC"></param>
        /// <returns></returns>
        private MailMessage ConstructMail(string subject, string message, List<string> toList, List<string> ccList, List<string> recipientsBCC)
        {
            MailMessage msg = new MailMessage();
            try
            {
                msg.Sender = new MailAddress(_senderAddress, _senderName);
                msg.Subject = subject;
                msg.Body = message;
                msg.Priority = MailPriority.High;
                msg.From = new MailAddress(_senderAddress, _senderName);
                msg.IsBodyHtml = true;
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay;
                foreach (String add in toList)
                {
                    msg.To.Add(new MailAddress(add));
                }
                foreach (String add in ccList)
                {
                    msg.CC.Add(new MailAddress(add));
                }
                foreach (String add in recipientsBCC)
                {
                    msg.Bcc.Add(new MailAddress(add));
                }
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
            return msg;
        }

        /// <summary>
        /// Send mail
        /// If client is busy sending a mail then queue it up
        /// else send the mail
        /// </summary>
        /// <param name="msg"></param>
        private void SendEmail(MailMessage msg)
        {
            try
            {
                //if (_isClientBusy)
                //{
                //    _pendingMails.Enqueue(msg);
                //}
                //else
                //{
                //    _isClientBusy = true;
                //    _client.SendMailAsync(msg);
                //}

                BufferMessage(dataBuffer, msg);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void BufferMessage(ITargetBlock<MailMessage> target, MailMessage message)
        {
            try
            {
                target.Post(message);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        async System.Threading.Tasks.Task<MailMessage> ConsumeBufferMessageAsync(IReceivableSourceBlock<MailMessage> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    MailMessage message;
                    while (source.TryReceive(out message))
                    {
                        if (message != null)
                        {

                            await _client.SendMailAsync(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        ///// <summary>
        ///// Send pending mails(if any)
        ///// </summary>
        //private async void SendNextMail()
        ////{
        //    try
        //    {
        //        if (_pendingMails.Count > 0)
        //        {
        //            MailMessage msg = _pendingMails.Dequeue();
        //            _isClientBusy = true;
        //            await _client.SendMailAsync(msg);
        //        }
        //        else
        //        {
        //            _isClientBusy = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Disposing object
        /// </summary>
        public void Dispose()
        {
            try
            {
                _client.Dispose();
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
        }
    }
}
