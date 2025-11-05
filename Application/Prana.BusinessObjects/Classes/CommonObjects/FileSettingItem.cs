using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// The class represents the details of the object that has the details of the FTP item
    /// </summary>
    public class FileSettingItem
    {
        #region Properties

        public int FileSettingID { get; set; }
        public bool IsActive { get; set; }
        public string FormatName { get; set; }
        public int ImportTypeID { get; set; }
        public int ReleaseID { get; set; }
        public string XsltPath { get; set; }
        public string XsdPath { get; set; }
        public string ImportSpName { get; set; }
        public string FTPFolderPath { get; set; }
        public string LocalFolderPath { get; set; }
        //public string FileName { get; set; }
        public int FtpID { get; set; }
        public int EmailID { get; set; }
        public int EmailLogID { get; set; }
        public int DecryptionID { get; set; }
        public int ThirdPartyID { get; set; }
        public string PriceToleranceColumns { get; set; }
        public string FormatType { get; set; }

        //added by: Bharat raturi, 28 may 2014
        //purpose: Get the new detail BatchStartDate
        public DateTime? BatchStartDate { get; set; }

        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1648
        //To map recon batch setting with import batch
        public int ImportFormatID { get; set; }
        #endregion

        public List<object> CompanyAccountID;
        // added by : Bhavana, for multiple clients at JIRA : CHMW-445
        public List<object> ClientID;

        /// <summary>
        /// Initialize the object with the default details
        /// </summary>
        public FileSettingItem()
        {
            FileSettingID = 0;
            IsActive = true;
            FormatName = string.Empty;
            ImportTypeID = 0;
            ReleaseID = 0;
            XsltPath = string.Empty;
            XsdPath = string.Empty;
            ImportSpName = string.Empty;
            FTPFolderPath = string.Empty;
            LocalFolderPath = string.Empty;
            //FileName = string.Empty;
            FtpID = 0;
            EmailID = 0;
            EmailLogID = 0;
            DecryptionID = 0;
            ThirdPartyID = 0;
            CompanyAccountID = new List<object>();
            ClientID = new List<object>();
            PriceToleranceColumns = string.Empty;
            FormatType = string.Empty;
            BatchStartDate = null;
        }
    }
}
