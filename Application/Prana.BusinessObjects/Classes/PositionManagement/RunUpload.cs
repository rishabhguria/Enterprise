using Csla;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class RunUpload : BusinessBase<RunUpload>, IDisposable
    {

        private int _progress = 0;

        public int Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }

        private ImportType _importTypeAcronym;
        public ImportType ImportTypeAcronym
        {
            get { return _importTypeAcronym; }
            set { _importTypeAcronym = value; }
        }
        private string _formatName;
        public string FormatName
        {
            get { return _formatName; }
            set { _formatName = value; }
        }

        private int _uploadID;

        /// <summary>
        /// Gets or sets the upload ID (Import ID).
        /// </summary>
        /// <value>The upload ID.</value>
        [Browsable(false)]
        public int UploadID
        {
            get { return _uploadID; }
            set { _uploadID = value; }
        }

        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the company name ID value.
        /// </summary>
        /// <value>The company name ID value.</value>
        public CompanyNameID CompanyNameIDValue
        {
            get
            {
                if (_companyNameID == null)
                {
                    _companyNameID = new CompanyNameID();
                }
                return _companyNameID;
            }
            set
            {
                _companyNameID = value;
                PropertyHasChanged();
            }
        }




        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameIDValue
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new ThirdPartyNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
                PropertyHasChanged();
            }
        }

        private bool _autoImport;

        /// <summary>
        /// Gets or sets a value indicating whether [enable auto time].
        /// </summary>
        /// <value><c>true</c> if [enable auto time]; otherwise, <c>false</c>.</value>
        public bool AutoImport
        {
            get { return _autoImport; }
            set
            {
                _autoImport = value;
                PropertyHasChanged();
            }
        }


        private DateTime _autoTime = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);

        /// <summary>
        /// Gets or sets the auto time.
        /// </summary>
        /// <value>The auto time.</value>
        public DateTime AutoTime
        {
            get { return _autoTime; }
            set
            {
                _autoTime = value;
                PropertyHasChanged();
            }
        }


        private DateTime? _lastRunUploadDate;

        public DateTime? LastRunUploadDate
        {
            get { return _lastRunUploadDate; }
            set { _lastRunUploadDate = value; }
        }

        private string _ftpWatcherFilePath = string.Empty;

        /// <summary>
        /// Gets or sets the FTP file path.
        /// </summary>
        /// <value>The FTP file path.</value>
        public string FTPWatcherFilePath
        {
            get { return _ftpWatcherFilePath; }
            set
            {
                _ftpWatcherFilePath = value;
                PropertyHasChanged();
            }
        }

        private string _lastImportedFile = string.Empty;
        /// <summary>
        /// Gets or sets the Last Imported File.
        /// </summary>
        /// <value>The Last Imported File.</value>
        public string LastImportedFile
        {
            get { return _lastImportedFile; }
            set
            {
                _lastImportedFile = value;
                PropertyHasChanged();
            }
        }

        private string _ftpServer;

        /// <summary>
        /// Gets or sets the FTP server.
        /// </summary>
        /// <value>The FTP server.</value>
        public string FTPServer
        {
            get { return _ftpServer; }
            set
            {
                _ftpServer = value;
                PropertyHasChanged();
            }
        }

        private int _serverPort = 21;

        /// <summary>
        /// Gets or sets the server port.
        /// 21 is default FTP port
        /// </summary>
        /// <value>The server port.</value>
        public int Port
        {
            get { return _serverPort; }
            set
            {
                _serverPort = value;
                PropertyHasChanged();
            }
        }


        private string _userName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyHasChanged();
            }
        }

        private string _password = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyHasChanged();
            }
        }

        private string _dirPath = string.Empty;

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        /// <value>The dir path.</value>
        public string DirPath
        {
            get { return _dirPath; }
            set
            {
                _dirPath = value;
                PropertyHasChanged();
            }
        }

        private string _fileName = string.Empty;

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                PropertyHasChanged();
            }
        }

        private DateTime _fileLastModifiedUTCTime = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);
        public DateTime FileLastModifiedUTCTime
        {
            get { return _fileLastModifiedUTCTime; }
            set
            {
                _fileLastModifiedUTCTime = value;
                PropertyHasChanged();
            }
        }

        private RunUploadStatus _status = RunUploadStatus.Awaiting;

        /// <summary>
        /// Gets or sets the upload status.
        /// </summary>
        /// <value>The status.</value>
        public RunUploadStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                PropertyHasChanged();
            }
        }

        private DataSourceFileLayout _fileLayoutType;

        /// <summary>
        /// Gets or sets the type of the file layout.
        /// </summary>
        /// <value>The type of the file layout.</value>
        [Browsable(false)]
        public DataSourceFileLayout FileLayoutType
        {
            get { return _fileLayoutType; }
            set
            {
                _fileLayoutType = value;
                PropertyHasChanged();
            }
        }

        private DateTime _uploadStartTime = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);

        /// <summary>
        /// Gets or sets the upload start time.
        /// Added Rajat (25 Nov 2006)
        /// </summary>
        /// <value>The upload start time.</value>
        [Browsable(false)]
        public DateTime UploadStartTime
        {
            get { return _uploadStartTime; }
            set { _uploadStartTime = value; }
        }

        private DateTime _uploadEndTime = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);

        /// <summary>
        /// Gets or sets the upload end time.
        /// Added Rajat (25 Nov 2006)
        /// </summary>
        /// <value>The upload end time.</value>
        [Browsable(false)]
        public DateTime UploadEndTime
        {
            get { return _uploadEndTime; }
            set { _uploadEndTime = value; }
        }

        private int _totalRecords;

        /// <summary>
        /// Gets or sets the total no of records uploaded (imported) in file.
        /// Added Rajat (25 Nov 2006)
        /// </summary>
        /// <value>The total records.</value>
        //[Browsable(false)]
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }

        private int _firstRecordIndex;

        /// <summary>
        /// Gets or sets the index of the first record in the currenct upload file.
        /// </summary>
        /// <value>The index of the first record.</value>
        [Browsable(false)]
        public int FirstRecordIndex
        {
            get { return _firstRecordIndex; }
            set { _firstRecordIndex = value; }
        }

        private int _headerIndex;

        /// <summary>
        /// Gets or sets the index of the first record in the currenct upload file.
        /// </summary>
        /// <value>The index of the first record.</value>
        [Browsable(false)]
        public int HeaderIndex
        {
            get { return _headerIndex; }
            set { _headerIndex = value; }
        }

        private string _statusDescription;

        /// <summary>
        /// Gets or sets the status description, The error description also goes here.
        /// Added Rajat (25 Nov 2006)
        /// </summary>
        /// <value>The status description.</value>
        [Browsable(false)]
        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }


        private int _companyUploadSetupID;

        /// <summary>
        /// Gets or sets the company upload setup ID.
        /// </summary>
        /// <value>The company upload setup ID.</value>
	    public int CompanyUploadSetupID
        {
            get { return _companyUploadSetupID; }
            set { _companyUploadSetupID = value; }
        }

        private int _tableTypeID = 1;
        /// <summary>        
        /// 1 is for Table Type Transaction, by default setting it to one, user will have to see, if he wants transaction
        /// or Net Postion types... Gets or sets the table type ID.
        /// </summary>
        /// <value>The data table type ID.</value>
        public int TableTypeID
        {
            get
            {
                return _tableTypeID;
            }
            set
            {
                _tableTypeID = value;
                PropertyHasChanged();
            }
        }


        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _uploadID;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        private string _filePath = string.Empty;

        /// <summary>
        /// Gets or sets the File Path.
        /// Added Sandeep (14 Nov 2007)
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _filePath = value;
                PropertyHasChanged();
            }
        }

        private string _rawFilePath = string.Empty;
        /// <summary>
        /// Gets or sets the Row File Path, Before decryption or any other processing
        /// </summary>
        public string RawFilePath
        {
            get { return _rawFilePath; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _rawFilePath = value;
            }
        }

        private string _processedFilePath = string.Empty;
        /// <summary>
        /// Gets or sets the Row File Path, Before decryption or any other processing
        /// </summary>
        public string ProcessedFilePath
        {
            get { return _processedFilePath; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _processedFilePath = value;
            }
        }

        private string _ftpFilePath = string.Empty;
        /// <summary>
        /// Ftp File path is needed to re run file upload
        /// </summary>
        public string FtpFilePath
        {
            get { return _ftpFilePath; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _ftpFilePath = value;
                PropertyHasChanged();
            }
        }

        private string _dataSourceXSLT = string.Empty;
        /// <summary>
        /// Gets or sets the XSLT for DataSource.
        /// Added By Sandeep (04 Jan 2008)
        /// </summary>
        public string DataSourceXSLT
        {
            get { return _dataSourceXSLT; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _dataSourceXSLT = value;
                PropertyHasChanged();
            }
        }

        private string _XSDName = string.Empty;
        public string XSDName
        {
            get { return _XSDName; }
            set
            {
                _XSDName = value;
                PropertyHasChanged();
            }
        }
        private string _tableFormatName = string.Empty;
        /// <summary>
        /// Gets or sets the XSLT for DataSource.
        /// Added By Sandeep (04 Jan 2008)
        /// </summary>
        public string TableFormatName
        {
            get { return _tableFormatName; }
            set
            {
                //if (value is System.DBNull)
                //{
                //    value = string.Empty;
                //}
                _tableFormatName = value;
                PropertyHasChanged();
            }
        }
        private bool _isUserSelectedDate = false;

        public bool IsUserSelectedDate
        {
            get { return _isUserSelectedDate; }
            set { _isUserSelectedDate = value; }
        }

        private DateTime _selectedDate = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);
        public DateTime SelectedDate { get { return _selectedDate; } set { _selectedDate = value; } }

        private bool _isUserSelectedAccount = false;

        public bool IsUserSelectedAccount
        {
            get { return _isUserSelectedAccount; }
            set { _isUserSelectedAccount = value; }
        }

        private ImportDataSource _importDataSource;
        public ImportDataSource ImportDataSource
        {
            get { return _importDataSource; }
            set { _importDataSource = value; }
        }

        private int _selectedAccount;
        public int SelectedAccount { get { return _selectedAccount; } set { _selectedAccount = value; } }

        private ImportSource _importSource;
        public ImportSource ImportSource { get { return _importSource; } set { _importSource = value; } }

        #region file watcher 

        readonly object _lockerObj = new object();

        [field: NonSerializedAttribute()]
        FileSystemWatcher _watcher = null;
        public void StartService()
        {
            try
            {
                lock (_lockerObj)
                {
                    if (_watcher == null)
                    {
                        _watcher = new FileSystemWatcher();
                        _watcher.Path = DirPath;
                        //_watcher.Filter = ConfigurationManager.AppSettings["WatchFilter"];
                        _watcher.IncludeSubdirectories = false;
                    }
                    if (_watcher != null)
                    {
                        _watcher.EnableRaisingEvents = true;
                        _watcher.Created += new FileSystemEventHandler(Watcher_Created);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                try
                {
                    //TODO : Gaurav : Have to move in the AutoimportManager in case of any exception

                    //SendMail.MailSend("Exception Report", ex.ToString());
                }
                catch (Exception exc)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrows = Logger.HandleException(exc, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrows)
                    {
                        throw;
                    }
                }
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                lock (_lockerObj)
                {
                    this.ImportSource = ImportSource.Automatic;
                    this.FilePath = e.FullPath;
                    this.FileName = e.Name;
                    this.FileLastModifiedUTCTime = File.GetLastWriteTime(e.FullPath);
                    WaitTillFileCopies(e.FullPath);
                    if (File_Created != null)
                    {
                        File_Created(this, e);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }

        private void WaitTillFileCopies(string filePath)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        using (Stream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            if (stream != null)
                            {
                                break;
                            }
                        }
                    }
                    catch (FileNotFoundException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                bool rethrows = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrows)
                {
                    throw;
                }
            }
        }
        //public void StopService()
        //{
        //    try
        //    {
        //        lock (_lockerObj)
        //        {
        //            if (_watcher != null)
        //            {
        //                _watcher.EnableRaisingEvents = false;                        
        //            }
        //        }
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

        #endregion

        public string Key { get { return _importSource.ToString() + _importTypeAcronym.ToString() + _fileName; } }
        public event FileSystemEventHandler File_Created;

        [field: NonSerializedAttribute()]
        private ThirdPartyFtp _ftpDetails = null;
        public ThirdPartyFtp FtpDetails
        {
            get
            { return _ftpDetails; }
            set
            { _ftpDetails = value; }
        }

        [field: NonSerializedAttribute()]
        private ThirdPartyGnuPG _decryptionDetails = null;
        public ThirdPartyGnuPG DecryptionDetails
        {
            get
            { return _decryptionDetails; }
            set
            { _decryptionDetails = value; }
        }

        [field: NonSerializedAttribute()]
        private ThirdPartyEmail _emailDetails = null;
        public ThirdPartyEmail EmailDetails
        {
            get
            { return _emailDetails; }
            set
            { _emailDetails = value; }
        }

        private bool _isPriceToleranceChecked;

        /// <summary>
        /// Gets or sets a value indicating whether [check price tolerance while import].
        /// </summary>
        /// <value><c>true</c> if [Price tolerance check enabled]; otherwise, <c>false</c>.</value>
        public bool IsPriceToleranceChecked
        {
            get
            {
                return _isPriceToleranceChecked;
            }
            set
            {
                _isPriceToleranceChecked = value;
            }
        }

        /// <summary>
        /// Used to get and set price tolerance value
        /// </summary>
        private double _priceTolerance;

        public double PriceTolerance
        {
            get { return _priceTolerance; }
            set { _priceTolerance = value; }
        }

        /// <summary>
        /// Used to get and set price tolerance columns
        /// </summary>
        private string _priceToleranceColumns;

        public string PriceToleranceColumns
        {
            get { return _priceToleranceColumns; }
            set { _priceToleranceColumns = value; }
        }

        /// <summary>
        /// Used to get and set batch start date
        /// </summary>
        private DateTime _batchStartDate = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);

        public DateTime BatchStartDate
        {
            get { return _batchStartDate; }
            set { _batchStartDate = value; }
        }

        /// <summary>
        /// Used to get and set batch start date
        /// </summary>
        private DateTime _executionDate = Convert.ToDateTime(DateTimeConstants.DateTimeMinVal);

        public DateTime ExecutionDate
        {
            get { return _executionDate; }
            set { _executionDate = value; }
        }


        /// <summary>
        /// Sp name of Recon and for generic import
        /// IF Null then need to take default for the recon Type in case of recon
        /// </summary>
        private string _spName = String.Empty;

        public string SPName
        {
            get { return _spName; }
            set { _spName = value; }
        }

        private List<int> _lstAccountID = new List<int>();
        [Browsable(false)]
        public List<int> LstAccountID
        {
            get { return _lstAccountID; }
            set { _lstAccountID = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_watcher != null)
                        _watcher.Dispose();
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
    }
}
