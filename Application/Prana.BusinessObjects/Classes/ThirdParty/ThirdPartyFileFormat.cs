using Prana.BusinessObjects.Classes.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdParty File Format.
    /// </summary>
    [Serializable]
    public class ThirdPartyFileFormat
    {
        private int _fileFormatId = int.MinValue;
        private string _fileFormatName = string.Empty;
        private int _thirdPartyID = int.MinValue;

        private string _PranaToThirdParty = string.Empty;
        //private string _thirdPartyToPrana = string.Empty;

        public ThirdPartyFileFormat()
        {
        }
        public ThirdPartyFileFormat(int fileFormatId, string fileFormatName)
        {
            _fileFormatId = fileFormatId;
            _fileFormatName = fileFormatName;
        }

        public int FileFormatId
        {
            get
            {
                return _fileFormatId;
            }
            set
            {
                _fileFormatId = value;
            }
        }

        public string FileFormatName
        {
            get
            {
                return _fileFormatName;
            }
            set
            {
                _fileFormatName = value;
            }
        }

        public int ThirdPartyID
        {
            get
            {
                return _thirdPartyID;
            }
            set
            {
                _thirdPartyID = value;
            }
        }

        public string PranaToThirdParty
        {
            get { return _PranaToThirdParty; }
            set { _PranaToThirdParty = value; }
        }

        private string _headerFile = string.Empty;
        public string HeaderFile
        {
            get { return _headerFile; }
            set { _headerFile = value; }
        }

        private string _footerFile = string.Empty;
        public string FooterFile
        {
            get { return _footerFile; }
            set { _footerFile = value; }
        }

        private bool _exportOnly = false;
        public bool ExportOnly
        {
            get { return _exportOnly; }
            set { _exportOnly = value; }
        }

        private string _fileDisplayName = string.Empty;
        public string FileDisplayName
        {
            get { return _fileDisplayName; }
            set { _fileDisplayName = value; }
        }

        string _delimiter = string.Empty;
        public string Delimiter
        {
            get { return _delimiter; }
            set
            {
                if (value.ToString().ToUpper().Equals("\\T"))
                {
                    _delimiter = "\t";
                }
                else
                {
                    _delimiter = value;
                }
            }
        }

        string _delimitorName = string.Empty;
        public string DelimiterName
        {
            get { return _delimitorName; }
            set { _delimitorName = value; }
        }

        private string _fileExtension = string.Empty;
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }

        private string _storedProcName = string.Empty;
        public string StoredProcName
        {
            get { return _storedProcName; }
            set { _storedProcName = value; }
        }

        private bool _doNotShowFileOpenDialogue = false;
        public bool DoNotShowFileOpenDialogue
        {
            get { return _doNotShowFileOpenDialogue; }
            set { _doNotShowFileOpenDialogue = value; }
        }

        private bool _clearExternalTransID = false;
        public bool ClearExternalTransID
        {
            get { return _clearExternalTransID; }
            set { _clearExternalTransID = value; }
        }

        // To Include Exercised/Assigned/Physically Settled/Expired Transactions
        private bool _includeExercisedAssignedTransaction = false;
        public bool IncludeExercisedAssignedTransaction
        {
            get { return _includeExercisedAssignedTransaction; }
            set { _includeExercisedAssignedTransaction = value; }
        }

        private bool _includeExercisedAssignedUnderlyingTransaction = false;
        public bool IncludeExercisedAssignedUnderlyingTransaction
        {
            get { return _includeExercisedAssignedUnderlyingTransaction; }
            set { _includeExercisedAssignedUnderlyingTransaction = value; }
        }

        private bool _includeCATransaction = false;
        public bool IncludeCATransaction
        {
            get { return _includeCATransaction; }
            set { _includeCATransaction = value; }
        }

        private bool _generateCancelNewForAmend = false;
        public bool GenerateCancelNewForAmend
        {
            get { return _generateCancelNewForAmend; }
            set { _generateCancelNewForAmend = value; }
        }

        private bool _fIXEnabled;

        public bool FIXEnabled
        {
            get { return _fIXEnabled; }
            set { _fIXEnabled = value; }
        }
        private bool _fileEnabled = true;

        public bool FileEnabled
        {
            get { return _fileEnabled; }
            set { _fileEnabled = value; }
        }
        private string _fixStorProc = "";

        public string FIXStorProc
        {
            get { return _fixStorProc; }
            set { _fixStorProc = value; }
        }

        public bool TimeBatchesEnabled
        {
            get; set;
        }

        [Browsable(false)]
        public List<ThirdPartyTimeBatch> TimeBatches { get; set; } = new List<ThirdPartyTimeBatch>();

    }
}
