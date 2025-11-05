using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Responsible for setting Run Upload settings
    /// </summary>
    class RunUploadSetup
    {
        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the company name ID.
        /// </summary>
        /// <value>The company name ID.</value>
        public CompanyNameID CompanyNameID
        {
            get 
                {
                    if (_companyNameID == null)
                    {
                        _companyNameID = new CompanyNameID();
                    }
                    return _companyNameID; 
                }
            set { _companyNameID = value; }
        }

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get {
                if (_dataSourceNameID== null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }
                return _dataSourceNameID; 
                }
            set { _dataSourceNameID = value; }
        }

        private bool _enableAutoTime;

        /// <summary>
        /// Gets or sets a value indicating whether [enable auto time].
        /// </summary>
        /// <value><c>true</c> if [enable auto time]; otherwise, <c>false</c>.</value>
        public bool EnableAutoTime
        {
            get { return _enableAutoTime; }
            set { _enableAutoTime = value; }
        }


        private DateTime _autoTime;

        /// <summary>
        /// Gets or sets the auto time.
        /// </summary>
        /// <value>The auto time.</value>
        public DateTime AutoTime
        {
            get { return _autoTime; }
            set { _autoTime = value; }
        }

        private string _ftpServer;

        /// <summary>
        /// Gets or sets the FTP server.
        /// </summary>
        /// <value>The FTP server.</value>
        public string FTPServer
        {
            get { return _ftpServer; }
            set { _ftpServer = value; }
        }

        private int _serverPort;

        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        /// <value>The server port.</value>
        public int ServerPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }


        private string _userName;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _dirPath;

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        /// <value>The dir path.</value>
        public string DirPath
        {
            get { return _dirPath; }
            set { _dirPath = value; }
        }

        private string _fileName;

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
	
	
    }
}
