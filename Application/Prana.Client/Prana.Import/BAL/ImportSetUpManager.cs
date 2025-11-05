using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.PM.DAL;
using Prana.Utilities.MiscUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Import
{
    class ImportSetUpManager
    {
        #region singleton
        private static volatile ImportSetUpManager instance;
        private static object syncRoot = new Object();

        private ImportSetUpManager() { }

        public static ImportSetUpManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ImportSetUpManager();
                    }
                }

                return instance;
            }
        }
        #endregion
        #region RunUpLoadManager

        private CompanyUser _companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;

        private RunUploadList _runUploadList = null;

        public RunUploadList RunUploadList
        {
            get
            {
                if (_runUploadList == null)
                    FillRunUploadList();
                return _runUploadList;
            }
            set { _runUploadList = value; }
        }
        private RunUploadList _autoImportList;
        public RunUploadList AutoImportList
        {
            get
            {
                if (_autoImportList == null)
                    FillRunUploadList();
                return _autoImportList;
            }
            set { _autoImportList = value; }
        }
        private RunUploadList _ftpImportList;
        public RunUploadList FTPImportList
        {
            get
            {
                if (_ftpImportList == null)
                    FillRunUploadList();
                return _ftpImportList;
            }
            set { _ftpImportList = value; }
        }
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1387
        string _startupPath = Application.StartupPath;

        public string StartupPath
        {
            get { return _startupPath; }
            set { _startupPath = value; }
        }

        private void FillRunUploadList()
        {
            if (_companyUser == null)
            {
                _companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
            }
            int pMCompanyID = RunUploadManager.GetPMCompanyID(_companyUser.CompanyID);
            _runUploadList = RunUploadManager.GetRunUploadDataByCompanyID(pMCompanyID);

            FileAndDbSyncManager.SyncFileWithDataBase(_startupPath, ApplicationConstants.MappingFileType.PranaXSD);
            RunUploadManager.SavePMImportXSLTfromDB(_startupPath);
            AutoImportList = new RunUploadList();
            foreach (RunUpload rn in RunUploadList)
            {
                if (rn.AutoImport == true)
                {
                    AutoImportList.Add(rn);
                }
            }
            FTPImportList = new RunUploadList();
            foreach (RunUpload rn in RunUploadList)
            {
                if (!string.IsNullOrWhiteSpace(rn.FTPWatcherFilePath))
                {
                    FTPImportList.Add(rn);
                }
            }
        }

        #endregion
    }
}
