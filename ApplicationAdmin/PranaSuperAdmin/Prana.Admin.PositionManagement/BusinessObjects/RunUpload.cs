using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Responsible for Running upload using Run Upload setting!
    /// </summary>
    class RunUpload
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
            get 
            {
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

        private RunUploadStatus _status;

        /// <summary>
        /// Gets or sets the upload status.
        /// </summary>
        /// <value>The status.</value>
        public RunUploadStatus Status
        {
            get { return _status; }
            set { _status = value; }
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

        //private string _errors;

        ///// <summary>
        ///// Gets or sets the errors.
        ///// </summary>
        ///// <value>The errors.</value>
        //public string Errors
        //{
        //    get { return _errors; }
        //    set { _errors = value; }
        //}

        //private string _exceptions;

        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>The exceptions.</value>
        //public string Exceptions
        //{
        //    get { return _exceptions; }
        //    set { _exceptions = value; }
        //}
	
    }
}
