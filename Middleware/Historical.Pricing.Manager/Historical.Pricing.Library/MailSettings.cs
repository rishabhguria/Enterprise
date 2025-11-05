using System;
using System.Collections.Generic;

namespace Historical.Pricing.Library
{

    /// <summary>
    /// Mail Settings
    /// </summary>
    /// <remarks></remarks>
    public class MailSettings
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        /// <remarks></remarks>
        public string From { get; set; }
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        /// <remarks></remarks>
        public List<string> To { get; set; }
        /// <summary>
        /// Gets or sets the SMTP.
        /// </summary>
        /// <value>The SMTP.</value>
        /// <remarks></remarks>
        public string Smtp { get; set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        /// <remarks></remarks>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        /// <remarks></remarks>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        /// <remarks></remarks>
        public Int32 Port { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        /// <remarks></remarks>
        public string User { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        /// <remarks></remarks>
        public List<string> Attachments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public MailSettings()
        {
            To = new List<string>();
            Attachments = new List<string>();
        }        
    }

 
}
