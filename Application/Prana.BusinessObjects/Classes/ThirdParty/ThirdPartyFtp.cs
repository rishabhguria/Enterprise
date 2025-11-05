using Prana.LogManager;
using System;
using System.IO;
using System.Net;


namespace Prana.BusinessObjects
{

    /// <summary>
    /// Third Party Ftp
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ThirdPartyFtp
    {

        private int _FtpId;
        private string _FtpName;
        private string _Host;
        private int _Port;
        private bool _UsePassive;
        private string _encryption = string.Empty;
        private string _UserName;
        private string _Password;
        private string _FtpType = string.Empty;
        private string _PassPhrase;
        private string _KeyFingerprint;
        private string _SshPrivateKeyPath;
        private int _Timeout;
        private string _ftpFolderPath = string.Empty;

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
        public string Encryption
        {
            get { return _encryption; }
            set { _encryption = value; }
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


        public bool Upload(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + _Host + _ftpFolderPath + fileInf.Name;
            FtpWebRequest reqFTP;

            //Setup Request
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(_UserName, _Password);
            reqFTP.UsePassive = _UsePassive;
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;

            //Read and send file in 2kb size
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();

            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

    }
}