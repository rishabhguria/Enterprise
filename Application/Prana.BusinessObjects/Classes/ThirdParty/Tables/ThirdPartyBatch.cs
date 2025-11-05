using Prana.Global;
using System;
using System.ComponentModel;
using static Prana.Global.ApplicationConstants;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ThirdPartyBatch : ThirdPartyBatchCommon
    {
        private string _logFile;
        private int _userId;
        private int _companyID;
        private string _transmissionType = string.Empty;
        private string _fixConnectionStatus;
        private string _brokerConnectionType;
        private string _allocationMatchStatus;
        private bool _haveFoundPBMismatchOverride;
        private bool _haveOverridePBDuplicateFileWarning;

        #region Public Properties

        /// <summary>
        /// Gets the log file.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(false)]
        public string LogFile
        {
            get
            {
                string chars = "\\/:*?\"<>|";
                foreach (char ch in chars)
                {
                    Description = Description.Replace(ch, '_');
                }
                _logFile = string.Format(".\\EOD\\{0}\\{1}.log", ThirdPartyShortName, Description);
                return _logFile;
            }
            set
            {
                _logFile = value;
            }
        }

        [Browsable(false)]
        public string ThirdPartyShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the company id.
        /// </summary>
        /// <value>The company id.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public int CompanyId
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string TransmissionType
        {
            get { return _transmissionType; }
            set { _transmissionType = value; }
        }

        public string FIXConnectionStatus
        {
            get { return _fixConnectionStatus; }
            set { _fixConnectionStatus = value; }
        }
        public string BrokerConnectionType
        {
            get { return _brokerConnectionType; }
            set { _brokerConnectionType = value; }
        }

        public string AllocationMatchStatus
        {
            get { return _allocationMatchStatus; }
            set { _allocationMatchStatus = value; }
        }

        /// <summary>
        /// Gets whether user has overriden the mismatch warning
        /// </summary>
        /// <value>The mismatch warning overriden status.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public bool HaveFoundPBMismatchOverride
        {
            get { return _haveFoundPBMismatchOverride; }
            set { _haveFoundPBMismatchOverride = value; }
        }

        /// <summary>
        /// Gets whether user has overriden the duplicate file warning
        /// </summary>
        /// <value>The duplicate file warning overriden status.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        public bool HaveOverridePBDuplicateFileWarning
        {
            get { return _haveOverridePBDuplicateFileWarning; }
            set { _haveOverridePBDuplicateFileWarning = value; }
        }

        /// <summary>
        /// Get and Set automated batch status
        /// </summary>
        public string AutomatedBatchStatus
        {
            get;set;
        }

        /// <summary>
        /// Get and Set automated batch status for FIX transmission
        /// </summary>
       [Browsable(false)]
        public string FixAutomatedBatchStatus
        {
            get; set;
        }
        #endregion
    }
}
