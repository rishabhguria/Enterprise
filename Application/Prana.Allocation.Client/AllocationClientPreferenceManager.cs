using Prana.Allocation.Client.CacheStore;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.Allocation.Client.Definitions;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using Prana.CommonDataCache;
using System.Collections;

namespace Prana.Allocation.Client
{
    public class AllocationClientPreferenceManager
    {
        #region Events

        /// <summary>
        /// Occurs when [allocation fixed preference updated].
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, string>>> AllocationFixedPreferenceUpdated;

        /// <summary>
        /// Occurs when [allocation operation preference updated].
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, string>>> AllocationOperationPreferenceUpdated;

        /// <summary>
        /// Occurs when [allocation preferences saved].
        /// </summary>
        public event EventHandler<EventArgs<AllocationPreferencesCollection>> AllocationPreferencesSaved;

        /// <summary>
        /// Occurs when [allocation mf preferences saved].
        /// </summary>
        public event EventHandler<EventArgs<AllocationMasterFundPreference>> AllocationMFPreferencesSaved;

        #endregion Events

        #region Members

        /// <summary>
        /// The custom xml serializer
        /// </summary>
        private static CustomXmlSerializer _Xml = new CustomXmlSerializer();

        /// <summary>
        /// The _singleton
        /// </summary>
        private static AllocationClientPreferenceManager _singleton = new AllocationClientPreferenceManager();

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();
        /// <summary>
        /// The _locker
        /// </summary>
        private readonly int _userId;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        internal static AllocationClientPreferenceManager GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_locker)
                    {
                        if (_singleton == null)
                            _singleton = new AllocationClientPreferenceManager();
                    }
                }
                return _singleton;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AllocationClientPreferenceManager"/> class from being created.
        /// </summary>
        private AllocationClientPreferenceManager()
        {
            _userId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            AllocationClientPreferenceDataManager.GetInstance().InitializeData(CachedDataManager.GetInstance.LoggedInUser.CompanyID);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the preference.
        /// </summary>
        /// <param name="prefName">Name of the preference.</param>
        /// <param name="key">The key.</param>
        /// <param name="allocationPrefType">Type of the allocation preference.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns></returns>
        internal PreferenceUpdateResult AddPreference(string prefName, int key, AllocationPreferencesType allocationPrefType, bool isPrefVisible)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceDataManager.GetInstance().AddPreference(prefName, key, allocationPrefType, isPrefVisible, _userId);
                return preferenceUpdateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Converts the XML to data set.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <returns></returns>
        private DataSet ConvertXMLToDataSet(string xmlData)
        {
            DataSet xmlDS = new DataSet();
            try
            {
                StringReader stream = null;
                XmlTextReader reader = null;

                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return xmlDS;
        }

        /// <summary>
        /// Copies the preference.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefName">Name of the preference.</param>
        internal PreferenceUpdateResult CopyPreference(int key, string prefName, AllocationPreferencesType prefType)
        {
            try
            {
                PreferenceUpdateResult result = AllocationClientPreferenceDataManager.GetInstance().CopyPreference(key, prefName, prefType, _userId);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        internal bool DeleteAllocationScheme(string schemeName, int schemeID)
        {
            bool isDeleted = false;
            try
            {
                isDeleted = AllocationClientPreferenceDataManager.GetInstance().DeleteAllocationScheme(schemeID, schemeName, _userId);
                if (isDeleted)
                    AllocationClientPreferenceCache.GetInstance.RemoveFixedPreferenceFromCache(schemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isDeleted;
        }

        /// <summary>
        /// Deletes the preference.
        /// </summary>
        /// <param name="key">The key.</param>
        internal PreferenceUpdateResult DeletePreference(int key, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceDataManager.GetInstance().DeletePreference(key, allocationPrefType, _userId);
                return preferenceUpdateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.Dispose();
                if (_singleton != null)
                    _singleton = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets all allocation operation preferences.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, AllocationOperationPreference> GetAllAllocationOperationPreferences()
        {
            try
            {
                return AllocationClientPreferenceCache.GetInstance.GetAllocationPreferenceCache();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the allocation company wise preferences.
        /// </summary>
        /// <param name="comapanyId">The comapany identifier.</param>
        /// <returns></returns>
        internal AllocationCompanyWisePref GetAllocationCompanyWisePreferences()
        {
            AllocationCompanyWisePref companyWisePref = null;
            try
            {
                companyWisePref = AllocationCompanyWisePreferenceCache.GetInstance.GetCompanyWisePreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return companyWisePref;
        }

        /// <summary>
        /// Gets the allocation fixed preferences.
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetAllocationFixedPreferences()
        {
            Dictionary<int, string> allocationFixedPreferencesList = null;
            try
            {
                allocationFixedPreferencesList = AllocationClientPreferenceDataManager.GetInstance().GetAllocationFixedPreferences(_userId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationFixedPreferencesList;
        }

        /// <summary>
        /// Return Allocation Operation Preference
        /// </summary>
        /// <param name="prefId"></param>
        /// <returns></returns>
        internal AllocationOperationPreference GetAllocationOperationPreference(int prefId)
        {
            AllocationOperationPreference alOperationPreference = null;
            try
            {
                Dictionary<int, AllocationOperationPreference> allocationPreference = AllocationClientPreferenceCache.GetInstance.GetAllocationPreferenceCache();
                if (allocationPreference.ContainsKey(prefId))
                    alOperationPreference = allocationPreference[prefId];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return alOperationPreference;
        }

        /// <summary>
        /// Gets the allocation operation preferences.
        /// </summary>
        /// <returns></returns>
        private List<AllocationOperationPreference> GetAllocationOperationPreferences()
        {
            List<AllocationOperationPreference> allocationOperationPrefList = new List<AllocationOperationPreference>();
            try
            {
                allocationOperationPrefList.AddRange(AllocationClientPreferenceDataManager.GetInstance().GetAllocationOperationPreferences(_userId));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationOperationPrefList;
        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal string GetAllocationSchemeByName(string allocationSchemeName)
        {
            string allocationSchemeXML = string.Empty;
            try
            {
                AllocationFixedPreference fp = AllocationClientPreferenceDataManager.GetInstance().GetAllocationSchemeByName(allocationSchemeName, _userId);
                if (fp != null)
                {
                    allocationSchemeXML = fp.Scheme;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemeXML;
        }

        /// <summary>
        /// Gets the fixed preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetFixedPreferencesList()
        {
            Dictionary<int, string> fixedPreferenceList = new Dictionary<int, string>();
            try
            {
                fixedPreferenceList = AllocationClientPreferenceCache.GetInstance.GetFixedPreferencesList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return fixedPreferenceList;
        }

        /// <summary>
        /// Gets the prorata fixed preference names.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetProrataFixedPreferenceNames()
        {
            List<string> prorataFixedPreferenceList = null;
            try
            {
                Dictionary<int, string> fixedPreferenceList = AllocationClientPreferenceDataManager.GetInstance().GetAllocationSchemesBySource(FixedPreferenceCreationSource.ProrataUI, _userId);
                prorataFixedPreferenceList = fixedPreferenceList.Select(x => x.Value).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prorataFixedPreferenceList;
        }

        /// <summary>
        /// Gets the master funds.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetMasterFundsRatio()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = AllocationClientPreferenceDataManager.GetInstance().GetMasterFundsRatio();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return ds;
        }

        /// <summary>
        /// Gets the preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetPreferencesList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                preferenceList.Add(int.MinValue, "-Select-");
                preferenceList.AddRangeThreadSafely(AllocationClientPreferenceCache.GetInstance.GetPreferencesList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        /// <summary>
        /// Gets filtered preferences list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetFilteredPreferencesList()
        {
            Dictionary<int, string> preferenceList = new Dictionary<int, string>();
            try
            {
                preferenceList.Add(int.MinValue, "-Select-");
                preferenceList.AddRangeThreadSafely(AllocationClientPreferenceCache.GetInstance.GetFilteredPreferencesList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceList;
        }

        /// <summary>
        /// Gets the allocation preferences.
        /// </summary>
        /// <returns></returns>
        internal AllocationPreferences GetUserWisePreferences()
        {
            AllocationPreferences allocationPreferences = new AllocationPreferences();
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string allocationPreferencesDirectoryPath = startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string allocationPreferencesFilePath = allocationPreferencesDirectoryPath + @"\AllocationClientPreference.xml";
                if (File.Exists(allocationPreferencesFilePath))
                    allocationPreferences = (AllocationPreferences)_Xml.ReadXml(allocationPreferencesFilePath, allocationPreferences);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationPreferences;
        }

        /// <summary>
        /// Imports the preference.
        /// </summary>
        /// <param name="importedPref">The imported preference.</param>
        internal PreferenceUpdateResult ImportPreference(AllocationOperationPreference importedPref)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceDataManager.GetInstance().ImportPreference(importedPref, _userId);
                return preferenceUpdateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Imports the master fund preference.
        /// </summary>
        /// <param name="importedPref">The imported preference.</param>
        internal PreferenceUpdateResult ImportMasterfundPreference(AllocationMasterFundPreference importedPref, List<AllocationOperationPreference> mfCalcPref)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceDataManager.GetInstance().ImportMasterfundPreference(importedPref, mfCalcPref, _userId);
                return preferenceUpdateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        ///  <summary>
        /// Initialize Cache
        ///  </summary>
        internal void InitializeCache()
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.Initialize(GetAllocationOperationPreferences(), GetAllocationFixedPreferences(), GetMasterFundPrefByCompanyId());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Renames the preference.
        /// </summary>
        /// <param name="prefID">The preference identifier.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        internal PreferenceUpdateResult RenamePreference(int prefID, string prefName, AllocationPreferencesType allocationPrefType)
        {
            try
            {
                PreferenceUpdateResult preferenceUpdateResult = AllocationClientPreferenceDataManager.GetInstance().RenamePreference(prefID, prefName, allocationPrefType, _userId);
                return preferenceUpdateResult;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Saves the default rule.
        /// </summary>
        /// <param name="defaultPref">The default preference.</param>
        internal bool SaveAllocationCompanyWisePreferences(AllocationCompanyWisePref defaultPref)
        {
            bool isSuccess = true;
            try
            {
                isSuccess = AllocationClientServiceConnector.Allocation.InnerChannel.SaveCompanyWisePreference(defaultPref);
                if (isSuccess)
                    AllocationCompanyWisePreferenceCache.GetInstance.SetCompanyWisePreferences(defaultPref);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSuccess;
        }

        /// <summary>
        /// Saves the allocation general preferences.
        /// </summary>
        /// <param name="allocationCompanyWisePref">The allocation company wise preference.</param>
        /// <param name="attributeNameDataSet">The attribute name data set.</param>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="masterFundTargetRatioDataSet">The master fund target ratio data set.</param>
        /// <param name="isMasterFundRatioEnable">if set to <c>true</c> [is master fund ratio enable].</param>
        internal bool SaveAllocationGeneralPreferences(AllocationCompanyWisePref allocationCompanyWisePref, DataSet attributeNameDataSet, AllocationPreferences allocationPreferences)
        {
            bool isSaveSuccess = false;
            try
            {
                isSaveSuccess = SaveAllocationCompanyWisePreferences(allocationCompanyWisePref);
                SaveAttributeNames(attributeNameDataSet);
                SaveAllocationUserWisePreferences(allocationPreferences);

                if (isSaveSuccess && AllocationPreferencesSaved != null)
                    AllocationPreferencesSaved(this, new EventArgs<AllocationPreferencesCollection>(new AllocationPreferencesCollection(allocationPreferences, allocationCompanyWisePref)));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSaveSuccess;
        }

        /// <summary>
        /// Saves the old master fund allocation preferences.
        /// </summary>
        /// <param name="masterFundTargetRatioDataSet">The master fund target ratio data set.</param>
        /// <returns></returns>
        internal bool SaveOldMasterFundAllocationPreferences(DataSet masterFundTargetRatioDataSet)
        {
            bool isSaveSuccess = false;
            try
            {
                isSaveSuccess = SaveMasterFundTargetRatio(masterFundTargetRatioDataSet);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSaveSuccess;
        }

        /// <summary>
        /// Saves the allocation scheme data.
        /// </summary>
        /// <param name="_schemeName">Name of the _scheme.</param>
        /// <param name="data">The data.</param>
        /// <param name="allocationSchemeKey">The allocation scheme key.</param>
        /// <returns></returns>
        internal int SaveAllocationSchemeData(AllocationFixedPreference fixedPref)
        {
            int resultantID = 0;
            try
            {
                resultantID = AllocationSchemeDataManager.GetInstance.SaveAllocationScheme(fixedPref);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return resultantID;
        }

        /// <summary>
        /// Saves the preferences.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        internal void SaveAllocationUserWisePreferences(AllocationPreferences allocationPreferences)
        {
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string allocationPreferencesDirectoryPath = startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string allocationPreferencesFilePath = allocationPreferencesDirectoryPath + @"\AllocationClientPreference.xml";

                _Xml.WriteFile(allocationPreferences, allocationPreferencesFilePath, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the attribute names.
        /// </summary>
        /// <param name="ds">The ds.</param>
        internal void SaveAttributeNames(DataSet ds)
        {
            try
            {
                AllocationClientPreferenceDataManager.GetInstance().SaveAttributeNames(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the calculated preferences.
        /// </summary>
        /// <param name="updatedAllocationOperationPreferences">The updated allocation operation preferences.</param>
        /// <returns></returns>
        internal bool SaveCalculatedPreferences(List<AllocationOperationPreference> updatedAllocationOperationPreferences)
        {
            try
            {
                foreach (AllocationOperationPreference preference in updatedAllocationOperationPreferences)
                {
                    PreferenceUpdateResult result = AllocationClientPreferenceDataManager.GetInstance().UpdatePreference(preference, _userId);

                    if (result.Error != null)
                    {
                        MessageBox.Show(result.Error, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                        AllocationClientPreferenceCache.GetInstance.UpDatePrefCache(result.Preference, preference.OperationPreferenceId);
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Saves the master fund target ratio.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        internal bool SaveMasterFundTargetRatio(DataSet ds)
        {
            bool isSaved = false;
            try
            {
                isSaved = AllocationClientPreferenceDataManager.GetInstance().SaveMasterFundTargetRatio(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Gets the and update allocation operation preference.
        /// </summary>
        /// <returns></returns>
        internal void UpdateAllocationOperationPref()
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.UpdateOperationPrefCache(GetAllocationOperationPreferences());
                if (AllocationOperationPreferenceUpdated != null)
                    AllocationOperationPreferenceUpdated(this, new EventArgs<Dictionary<int, string>>(AllocationClientPreferenceCache.GetInstance.GetPreferencesList()));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the fixed preferences in cache.
        /// </summary>
        internal void UpdateFixedPreferencesOnImport()
        {
            try
            {
                Dictionary<int, string> fixedPreferences = GetAllocationFixedPreferences();
                AllocationClientPreferenceCache.GetInstance.UpdateFixedPreferencesCache(fixedPreferences);
                if (AllocationFixedPreferenceUpdated != null)
                    AllocationFixedPreferenceUpdated(this, new EventArgs<Dictionary<int, string>>(GetFixedPreferencesList()));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Ups the date preference cache.
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        /// <param name="prefKey">The preference key.</param>
        internal void UpDatePrefCache(PreferenceUpdateResult preferenceUpdateResult, int prefKey)
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.UpDatePrefCache(preferenceUpdateResult.Preference, prefKey);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Updates calculate preference cache for mf update.
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        internal void UpDateCalcPrefCacheforMFUpdate(PreferenceUpdateResult preferenceUpdateResult)
        {
            try
            {
                preferenceUpdateResult.MasterFundCalculatedPreferences.ForEach(calPref =>
                {
                    AllocationClientPreferenceCache.GetInstance.UpDatePrefCache(calPref, calPref.OperationPreferenceId);
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the master fund preference list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetMasterFundPreferenceList()
        {
            Dictionary<int, string> masterFundPreferenceList = new Dictionary<int, string>();
            try
            {
                masterFundPreferenceList.AddRangeThreadSafely(AllocationClientPreferenceCache.GetInstance.GetMasterFundPreferenceList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return masterFundPreferenceList;
        }

        /// <summary>
        /// Return Allocation Operation Preference
        /// </summary>
        /// <param name="prefId"></param>
        /// <returns></returns>
        internal AllocationMasterFundPreference GetAllocationMFPreference(int prefId)
        {
            AllocationMasterFundPreference allocationMFPreference = null;
            try
            {
                allocationMFPreference = AllocationClientPreferenceCache.GetInstance.GetAllocationMFPreferenceByPrefId(prefId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationMFPreference;
        }

        /// <summary>
        /// Adds the or update master fund preference cache.
        /// </summary>
        /// <param name="preferenceUpdateResult">The preference update result.</param>
        /// <param name="prefKey">The preference key.</param>
        internal void AddOrUpdateMasterFundPrefCache(PreferenceUpdateResult preferenceUpdateResult, int prefKey)
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.AddOrUpdateMasterFundPrefCache(preferenceUpdateResult, prefKey);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the master fund preference by company identifier.
        /// </summary>
        /// <returns></returns>
        private List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId()
        {
            List<AllocationMasterFundPreference> allocationMFpreferenceList = new List<AllocationMasterFundPreference>();
            try
            {
                allocationMFpreferenceList.AddRange(AllocationClientPreferenceDataManager.GetInstance().GetMasterFundPrefByCompanyId(_userId));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationMFpreferenceList;
        }

        /// <summary>
        /// Saves the master fund preferences.
        /// </summary>
        /// <param name="updatedMasterFundPreferences">The updated master fund preferences.</param>
        /// <returns></returns>
        internal bool SaveMasterFundPreferences(List<AllocationMasterFundPreference> updatedMasterFundPreferences, Dictionary<int, List<AllocationOperationPreference>> mfCalculatedPrefs)
        {
            try
            {
                foreach (AllocationMasterFundPreference mfPreference in updatedMasterFundPreferences)
                {
                    PreferenceUpdateResult result = AllocationClientPreferenceDataManager.GetInstance().SaveMasterFundPreference(mfPreference, mfCalculatedPrefs[mfPreference.MasterFundPreferenceId], _userId);

                    if (result.Error != null)
                    {
                        MessageBox.Show(result.Error, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        //Update MF cache
                        AllocationClientPreferenceCache.GetInstance.AddOrUpdateMasterFundPrefCache(result, mfPreference.MasterFundPreferenceId);

                        //Update MF calculated Pref cache
                        result.MasterFundCalculatedPreferences.ForEach(calPref =>
                        {
                            AllocationClientPreferenceCache.GetInstance.UpDatePrefCache(calPref, calPref.OperationPreferenceId);
                        });

                        //Update local cache
                        if (AllocationMFPreferencesSaved != null)
                            AllocationMFPreferencesSaved(this, new EventArgs<AllocationMasterFundPreference>(result.MasterFundPreference));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Updates the master fund allocation preference.
        /// </summary>
        internal void UpdateMasterFundAllocationPref()
        {
            try
            {
                AllocationClientPreferenceCache.GetInstance.UpdateMasterFundPrefCache(GetMasterFundPrefByCompanyId());
                if (AllocationOperationPreferenceUpdated != null)
                    AllocationOperationPreferenceUpdated(this, new EventArgs<Dictionary<int, string>>(AllocationClientPreferenceCache.GetInstance.GetMasterFundPreferenceList()));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Uses Excel logic to sort the provided dictionary Alphabetically according to values.
        /// </summary>
        /// <param name="preferenceList">Provided dictionary.</param>
        /// <returns>Alphabetically sorted dictionary.</returns>
        internal Dictionary<int, string> UpdateSorting(Dictionary<int, string> preferenceList)
        {
            try
            {
                return preferenceList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return preferenceList;
            }
        }

        #endregion Methods
    }
}
