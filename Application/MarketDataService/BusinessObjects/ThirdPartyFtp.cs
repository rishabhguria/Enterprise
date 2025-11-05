using System.ComponentModel;
using System;
using System.IO;
using System.Net;
using System.Configuration;


namespace BusinessObjects
{

    /// <summary>
    /// Third Party Ftp
    /// </summary>
    /// <remarks></remarks>
    public class ThirdPartyFtp
    {

        private int _FtpId;
        private string _FtpName;
        private string _Host;
        private int _Port;
        private bool _UsePassive;
        private bool _UseSsl;
        private string _UserName;
        private string _Password;
        private string _FtpType;
        private string _PassPhrase;
        private string _KeyFingerprint;
        private string _SshPrivateKeyPath;
        private int _Timeout;
        private string _ftpFolderPath = string.Empty;

        public static ThirdPartyFtp GetFtpFromConfig()
        {
            ThirdPartyFtp thirdPartyFtp = new ThirdPartyFtp();
            try
            {
                thirdPartyFtp.FtpName = ConfigurationManager.AppSettings["FtpName"];
                thirdPartyFtp.FtpType = ConfigurationManager.AppSettings["FtpType"];
                thirdPartyFtp.Host = ConfigurationManager.AppSettings["Host"];
                thirdPartyFtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                thirdPartyFtp.UsePassive = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePassive"]);
                thirdPartyFtp.UseSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSsl"]);
                thirdPartyFtp.UserName = ConfigurationManager.AppSettings["UserName"];
                thirdPartyFtp.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
                thirdPartyFtp.Password = ConfigurationManager.AppSettings["Password"];
                thirdPartyFtp.KeyFingerPrint = ConfigurationManager.AppSettings["KeyFile"];
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
            }
            return thirdPartyFtp;
        }

        /// <summary>
        /// Gets or sets the FTP id.
        /// </summary>
        /// <value>The FTP id.</value>
        /// <remarks></remarks>
        public int FtpId
        {
            get { return _FtpId; }
            set { _FtpId = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks></remarks>
        public string FtpName
        {
            get { return _FtpName; }
            set { _FtpName = value; }
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        /// <remarks></remarks>
        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        /// <remarks></remarks>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use passive].
        /// </summary>
        /// <value><c>true</c> if [use passive]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UsePassive
        {
            get { return _UsePassive; }
            set { _UsePassive = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use SSL].
        /// </summary>
        /// <value><c>true</c> if [use SSL]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UseSsl
        {
            get { return _UseSsl; }
            set { _UseSsl = value; }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        /// <remarks></remarks>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        /// <summary>
        /// Gets or sets the type of the FTP.
        /// </summary>
        /// <value>The type of the FTP.</value>
        /// <remarks></remarks>
        public string FtpType
        {
            get { return _FtpType; }
            set { _FtpType = value; }
        }

        /// <summary>
        /// Gets or sets the pass phrase.
        /// </summary>
        /// <value>The pass phrase.</value>
        /// <remarks></remarks>
        public string PassPhrase
        {
            get { return _PassPhrase; }
            set { _PassPhrase = value; }
        }

        /// <summary>
        /// Gets or sets the key fingerprint.
        /// </summary>
        /// <value>
        /// The key fingerprint.
        /// </value>
        public string KeyFingerPrint
        {
            get { return _KeyFingerprint; }
            set { _KeyFingerprint = value; }
        }

        /// <summary>
        /// Gets or sets the SSH private key path.
        /// </summary>
        /// <value>
        /// The SSH private key path.
        /// </value>
        public string SshPrivateKeyPath
        {
            get { return _SshPrivateKeyPath; }
            set { _SshPrivateKeyPath = value; }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        /// <remarks></remarks>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the FolderPath.
        /// </summary>
        /// <value>The FolderPath.</value>
        /// <remarks></remarks>
        public string FtpFolderPath
        {
            get { return _ftpFolderPath; }
            set { _ftpFolderPath = value; }
        }

    }
}