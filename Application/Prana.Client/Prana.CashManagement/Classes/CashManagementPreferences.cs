using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.IO;

namespace Prana.CashManagement
{
    class CashManagementPreferences : IPreferenceData
    {
        #region Members

        /// <summary>
        /// The cash save preferences file path
        /// </summary>
        string _cashSavePreferencesFilePath = string.Empty;

        /// <summary>
        /// The start path
        /// </summary>
        string _startPath = string.Empty;

        /// <summary>
        /// The cash save preferences directory path
        /// </summary>
        string _cashSavePreferencesDirectoryPath = string.Empty;

        /// <summary>
        /// The user identifier
        /// </summary>
        int _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

        /// <summary>
        /// The old cash save preference
        /// </summary>
        static CashSavePreference _oldCashSavePreference = null;

        /// <summary>
        /// The XML
        /// </summary>
        static CustomXmlSerializer _Xml = new CustomXmlSerializer();

        #endregion Members

        #region Methods

        /// <summary>
        /// Gets the cash defualt preferences.
        /// </summary>
        /// <returns></returns>
        public static CashSavePreference GetCashDefualtPreferences()
        {
            CashSavePreference _cashSavePreference = new CashSavePreference();

            //string yesterdayEndColumn = String.Empty;
            //string transcationDetailColumn = String.Empty;

            //List<string> yesterdayEndNames = AllocationConstants.UnAllocatedDisplayColumns;
            //foreach (string str in UnAllocatedColumnNames)
            //{
            //    unAllocatedColumns += str + ",";
            //}

            //List<string> AllocatedColumnNames = AllocationConstants.AllocatedDisplayColumns;
            //foreach (string str in AllocatedColumnNames)
            //{
            //    allocatedColumns += str + ",";
            //}

            //_allocationPreferences.UnAllocatedColumns = unAllocatedColumns;
            //_allocationPreferences.AllocatedColumns = allocatedColumns;
            //_allocationPreferences.AllocationDefaultList = CachedDataManager.GetInstance.GetAllocationDefaults();
            ////_allocationPreferences.AccountingMethods.AccountingMethodsTable = CachedDataManager.GetInstance.GetAccountingMethodsTable();

            return _cashSavePreference;


        }

        /// <summary>
        /// Gets the preferences.
        /// </summary>
        /// <returns></returns>
        public CashSavePreference GetPreferences()
        {
            TextReader r = null;
            CashSavePreference _cashSavePreference = new CashSavePreference();
            _startPath = System.Windows.Forms.Application.StartupPath;
            _cashSavePreferencesDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + _userID.ToString();
            _cashSavePreferencesFilePath = _cashSavePreferencesDirectoryPath + @"\CashManagementPreferences.xml";


            try
            {


                if (!Directory.Exists(_cashSavePreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_cashSavePreferencesDirectoryPath);

                }

                if (File.Exists(_cashSavePreferencesFilePath))
                {

                    _cashSavePreference = (CashSavePreference)_Xml.ReadXml(_cashSavePreferencesFilePath, _cashSavePreference);

                }
                //if No Preferences File Exist Take Default Preferences
                else
                {
                    _cashSavePreference = GetCashDefualtPreferences();
                }

                _oldCashSavePreference = _cashSavePreference;
                return _oldCashSavePreference;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (r != null)
                    r.Close();
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion

        }

        #endregion Methods
    }
}
