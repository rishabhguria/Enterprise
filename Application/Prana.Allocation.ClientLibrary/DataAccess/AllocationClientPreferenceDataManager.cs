using Prana.Allocation.Common.Constants;
using Prana.Allocation.Common.Helper;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Allocation.ClientLibrary.DataAccess
{
    /// <summary>
    /// </summary>
    public class AllocationClientPreferenceDataManager
    {
        #region Members

        /// <summary>
        /// The _singleton
        /// </summary>
        private static AllocationClientPreferenceDataManager _singleton = new AllocationClientPreferenceDataManager();

        /// <summary>
        /// The _locker
        /// </summary>
        private static object _locker = new object();
        private int _companyID;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AllocationClientPreferenceDataManager"/> class.
        /// </summary>
        private AllocationClientPreferenceDataManager()
        {
            
        }

        public void InitializeData(int companyID)
        {
            try
            {
                _companyID = companyID;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the preference.
        /// </summary>
        /// <param name="prefName">Name of the preference.</param>
        /// <param name="key">The key.</param>
        /// <param name="_allocationPrefType">Type of the allocation preference.</param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns></returns>
        public PreferenceUpdateResult AddPreference(string prefName, int key, AllocationPreferencesType _allocationPrefType, bool isPrefVisible, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_NAME + prefName + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.ADD_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.AddPreference(prefName, key, _allocationPrefType, isPrefVisible);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_NAME + prefName + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.ADD_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Copies the preference.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="prefName">Name of the preference.</param>
        /// <returns></returns>
        public PreferenceUpdateResult CopyPreference(int key, string prefName, AllocationPreferencesType prefType, int userID)
        {
            PreferenceUpdateResult result = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.PREFERENCE_NAME + prefName + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.COPY_ALLOCATION_PREFERENCE);
                result = AllocationClientServiceConnector.Allocation.InnerChannel.CopyPreference(key, prefName, prefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.PREFERENCE_NAME + prefName + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.COPY_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        public bool DeleteAllocationScheme(int schemeID, string schemeName, int userID)
        {
            bool isDeleted = false;
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + schemeID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.DELETE_ALLOCATION_SCHEME);
                isDeleted = AllocationClientServiceConnector.Allocation.InnerChannel.DeleteAllocationScheme(schemeID, schemeName);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + schemeID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.DELETE_ALLOCATION_SCHEME);
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
        /// <returns></returns>
        public PreferenceUpdateResult DeletePreference(int key, AllocationPreferencesType allocationPrefType, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.DELETE_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.DeletePreference(key, allocationPrefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.DELETE_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Gets the allocation fixed preferences.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllocationFixedPreferences(int userID)
        {
            Dictionary<int, string> allocationFixedPreferencesList = null;
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ALLOCATION_FIXED_PREFERENCE);
                allocationFixedPreferencesList = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllAllocationSchemeNames();
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_FIXED_PREFERENCE);
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
        /// Gets the allocation schemes by source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllocationSchemesBySource(FixedPreferenceCreationSource source, int userID)
        {
            Dictionary<int, string> allocationFixedPreferencesList = null;
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ALLOCATION_SCHEME_BY_SOURCE);
                allocationFixedPreferencesList = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllocationSchemesBySource(source);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_SCHEME_BY_SOURCE);
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
        /// Gets the allocation operation preferences.
        /// </summary>
        /// <returns></returns>
        public List<AllocationOperationPreference> GetAllocationOperationPreferences(int userID)
        {
            List<AllocationOperationPreference> allocationOperationpreferenceList = new List<AllocationOperationPreference>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_ALLOCATION_OPERATION_PREFERENCE);
                allocationOperationpreferenceList = AllocationClientServiceConnector.Allocation.InnerChannel.GetCalculatedPreferencesByCompanyId(_companyID, userID);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_ALLOCATION_OPERATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationOperationpreferenceList;

        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        public AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName, int userID)
        {
            AllocationFixedPreference allocationScheme = null;
            try
            {
                allocationScheme = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllocationSchemeByName(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation company wise preference.
        /// </summary>
        /// <param name="comapanyId">The comapany identifier.</param>
        /// <returns></returns>
        public AllocationCompanyWisePref GetCompanyWisePreference(int comapanyId, int userID)
        {
            AllocationCompanyWisePref allocationCompanyWisePref = new AllocationCompanyWisePref();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.GET_COMPANYWISE_PREFERENCE_DEFAULT_RULE);
                allocationCompanyWisePref = AllocationClientServiceConnector.Allocation.InnerChannel.GetCompanyWisePreference(comapanyId);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.GET_COMPANYWISE_PREFERENCE_DEFAULT_RULE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationCompanyWisePref;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static AllocationClientPreferenceDataManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new AllocationClientPreferenceDataManager();
                    }
                }
            }
            return _singleton;
        }

        /// <summary>
        /// Gets the master funds.
        /// </summary>
        /// <returns></returns>
        public DataSet GetMasterFundsRatio()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = AllocationClientServiceConnector.Allocation.InnerChannel.GetAllMasterFundsRatio();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        /// Imports the preference.
        /// </summary>
        /// <param name="importedPref">The imported preference.</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportPreference(AllocationOperationPreference importedPref, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.ImportPreference(importedPref);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Imports the master fund preference.
        /// </summary>
        /// <param name="importedPref">The imported preference.</param>
        /// <returns></returns>
        public PreferenceUpdateResult ImportMasterfundPreference(AllocationMasterFundPreference importedPref, List<AllocationOperationPreference> mfCalcPref, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.ImportMasterfundPreference(importedPref, mfCalcPref);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.IMPORT_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Renames the preference.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public PreferenceUpdateResult RenamePreference(int key, string value, AllocationPreferencesType allocationPrefType, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.PREFERENCE_NAME + value + AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.RENAME_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.RenamePreference(key, value, allocationPrefType);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.PREFERENCE_ID + key + AllocationLoggingConstants.PREFERENCE_NAME + value + AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.RENAME_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Saves the attribute names.
        /// </summary>
        /// <param name="ds">The ds.</param>
        public void SaveAttributeNames(DataSet ds)
        {
            try
            {
                AllocationClientServiceConnector.Allocation.InnerChannel.SaveAttributeNames(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the master fund target ratio.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public bool SaveMasterFundTargetRatio(DataSet ds)
        {
            bool isSaved = false;
            try
            {
                isSaved = AllocationClientServiceConnector.Allocation.InnerChannel.SaveMasterFundTargetRatio(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSaved;
        }

        /// <summary>
        /// Updates the preference.
        /// </summary>
        /// <param name="preference">The preference.</param>
        /// <returns></returns>
        public PreferenceUpdateResult UpdatePreference(AllocationOperationPreference preference, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.UPDATE_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.UpdatePreference(preference);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.UPDATE_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        /// <summary>
        /// Gets the allocation operation preferences.
        /// </summary>
        /// <returns></returns>
        public List<AllocationMasterFundPreference> GetMasterFundPrefByCompanyId(int userID)
        {
            List<AllocationMasterFundPreference> allocationMFpreferenceList = new List<AllocationMasterFundPreference>();
            try
            {
                allocationMFpreferenceList = AllocationClientServiceConnector.Allocation.InnerChannel.GetMasterFundPrefByCompanyId(_companyID, userID);
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
        /// <param name="mfPreference">The mf preference.</param>
        /// <returns></returns>
        public PreferenceUpdateResult SaveMasterFundPreference(AllocationMasterFundPreference mfPreference, List<AllocationOperationPreference> mfCalculatedPrefs, int userID)
        {
            PreferenceUpdateResult preferenceUpdateResult = new PreferenceUpdateResult();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.REQUEST_SEND, AllocationLoggingConstants.SAVE_MF_ALLOCATION_PREFERENCE);
                preferenceUpdateResult = AllocationClientServiceConnector.Allocation.InnerChannel.SaveMasterFundPreference(mfPreference, mfCalculatedPrefs);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.USER_ID + userID + AllocationLoggingConstants.COMPONENT_ALLOCATION_PREFERENCE + AllocationLoggingConstants.RESPONSE_RECEIVED, AllocationLoggingConstants.SAVE_MF_ALLOCATION_PREFERENCE);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return preferenceUpdateResult;
        }

        #endregion Methods
    }
}
