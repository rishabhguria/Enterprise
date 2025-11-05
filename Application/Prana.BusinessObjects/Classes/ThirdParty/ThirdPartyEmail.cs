using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyEmail
    {

        /// <summary>
        /// Gets or sets the email id.
        /// </summary>
        /// <value>The email id.</value>
        /// <remarks></remarks>
        private int emailId;
        public int EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the email.
        /// </summary>
        /// <value>The name of the email.</value>
        /// <remarks></remarks>
        private string emailName;
        public string EmailName
        {
            get { return emailName; }
            set { emailName = value; }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        /// <remarks></remarks>
        private MailPriority priority = MailPriority.Normal;
        public MailPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        /// <remarks></remarks>
        private string mailFrom;
        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }
        /// <summary>
        /// Gets or sets the SMTP.
        /// </summary>
        /// <value>The SMTP.</value>
        /// <remarks></remarks>
        private string smtp;
        public string Smtp
        {
            get { return smtp; }
            set { smtp = value; }
        }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        /// <remarks></remarks>
        private Int32 port = 25;
        public Int32 Port
        {
            get { return port; }
            set { port = value; }
        }
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        /// <remarks></remarks>
        private string mailTo;
        public string MailTo
        {
            get { return mailTo; }
            set { mailTo = value; }
        }

        /// <summary>
        /// Gets or sets the cc to.
        /// </summary>
        /// <value>The cc to.</value>
        /// <remarks></remarks>
        private string ccTo;
        public string CcTo
        {
            get { return ccTo; }
            set { ccTo = value; }
        }

        /// <summary>
        /// Gets or sets the bcc to.
        /// </summary>
        /// <value>The cc to.</value>
        /// <remarks></remarks>
        private string bccTo;
        public string BccTo
        {
            get { return bccTo; }
            set { bccTo = value; }
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        /// <remarks></remarks>
        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        /// <remarks></remarks>
        private string body;
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        /// <remarks></remarks>
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private MailType mailType = MailType.DataFile;
        public MailType MailType
        {
            get { return mailType; }
            set { mailType = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ThirdPartyEmail"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SSL"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        private bool sslEnabled = false;
        public bool SSLEnabled
        {
            get { return sslEnabled; }
            set { sslEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        /// <remarks></remarks>
        public List<string> Attachments = new List<string>();


    }
}
