using Prana.BusinessObjects.Compliance;
using Prana.BusinessObjects.Compliance.CompliancePref;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.CommonDataCache.DAL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;

namespace Prana.CommonDataCache
{
    public class ComplianceCachedData
    {
        private static ComplianceCachedData _cachedData = null;

        //Object of compliance pref type. containing all compliance preferences.
        CompliancePref _compliancePref = new CompliancePref();
        static ComplianceCachedData()
        {
            try
            {
                _cachedData = new ComplianceCachedData();
                _companyID = CachedDataManager.GetInstance.GetCompanyID();
                //load permissions of pretrade check and override permission for a particular user in saparate dictionary
                _cachedData.LoadPrePostModuleEnabled(_companyID);
                if (_cachedData.GetPreOrPostModuleEnabledForCompany())
                {
                    _cachedData.LoadOverridePreTradeCheckPowerUserPermission();
                    _cachedData.LoadCompliancePrePostPermissions();
                    _cachedData.GetCompliancePref();
                    _cachedData.LoadCompanyUserEmailIds();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        /// <summary>
        /// Load Company User Email Ids
        /// </summary>
        private void LoadCompanyUserEmailIds()
        {
            try
            {
                DataSet companyUserEmailIds = CompliacePermissionDataManager.GetCompanyUserEmailIds(_companyID);
                if (companyUserEmailIds != null)
                {
                    if (companyUserEmailIds.Tables.Count > 0)
                    {
                        foreach (DataRow row in companyUserEmailIds.Tables[0].Rows)
                        {
                            _companyUserEmailIds.Add(Convert.ToInt32(row["UserId"].ToString()), row["EMail"].ToString());
                        }
                    }
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


        public static ComplianceCachedData GetInstance()
        {

            return _cachedData;
        }

        private static int _companyID;

        public static int CompanyID
        {
            get { return _companyID; }
        }

        /// <summary>
        /// Company User Email Ids Cache
        /// </summary>
         readonly Dictionary<int, string> _companyUserEmailIds = new Dictionary<int, string>();
        /// <summary>
        /// Company User Email Ids having compliance permission Cache 
        /// </summary>
        Dictionary<int, string> _companyUserEmailIdsCompliance = new Dictionary<int, string>();

        //Dictionary<int, bool> _preTradeCheck = new Dictionary<int, bool>();
        /// <summary>
        /// This Dictionary Contain the information  Whether the perticular user have permission for pre trade check or not 
        /// </summary>
        //public Dictionary<int, bool> PreTradeCheck
        //{
        //    get { return _preTradeCheck; }
        //    set { _preTradeCheck = value; }
        //}

        //private Dictionary<int, bool> _powerUser = new Dictionary<int, bool>();

        //public Dictionary<int, bool> PowerUser
        //{
        //    get { return _powerUser; }
        //    set { _powerUser = value; }
        //}

        //Dictionary<int, bool> _overridePermission = new Dictionary<int, bool>();
        /// <summary>
        /// This  Dictionary contain information whether the user have override permission to allow the tarde even it violated rule.
        /// </summary>
        //public Dictionary<int, bool> OverridePermission
        //{
        //    get { return _overridePermission; }
        //    set { _overridePermission = value; }
        //}

        /// <summary>
        /// This Dictionay contain information the user have apply to manual permisssion or not in case user have pre trade check permission
        /// </summary>
        //Dictionary<int, bool> _applyToManual = new Dictionary<int, bool>();

        readonly Dictionary<int, bool> _preTradeModule = new Dictionary<int, bool>();
        public Dictionary<int, bool> PreTradeModule
        {
            get { return _preTradeModule; }
        }
        ///// <summary>
        ///// Stores true or false company-wise if Pre-Post Compliance module is enabled or not
        ///// </summary>
        //Dictionary<int, bool> _prePostEnabled = new Dictionary<int, bool>();


        /// <summary>
        /// Stores true or false company-wise if Pre-Post Compliance module is enabled or not
        /// </summary>
        Dictionary<int, bool> _postEnabled = new Dictionary<int, bool>();
        public Dictionary<int, bool> PostEnabled
        {
            get { return _postEnabled; }
        }



        /// <summary>
        /// Stores true or false company-wise if Pre-Post Compliance module is enabled or not
        /// </summary>
        Dictionary<int, bool> _preEnabled = new Dictionary<int, bool>();
        public Dictionary<int, bool> PreEnabled
        {
            get { return _preEnabled; }
        }


        /// <summary>
        /// This Dictionary Contain Information Whether the user have permission of read or read write both for pre trade compliance  module
        /// </summary>
        //public Dictionary<int, bool> PreTradeModule
        //{
        //    get { return _preTradeModule; }
        //    set { _preTradeModule = value; }
        //}


        readonly Dictionary<int, bool> _postTradeModule = new Dictionary<int, bool>();
        public Dictionary<int, bool> PostTradeModule
        {
            get { return _postTradeModule; }
        }
        /// <summary>
        /// This Dictionary Contain Information Whether the user have permission of read or read write both for post trade compliance  module 
        /// </summary>
        //public Dictionary<int, bool> PostTradeModule
        //{
        //    get { return _postTradeModule; }
        //    set { _postTradeModule = value; }
        //}


        /// <summary>
        /// Used to avoid multithread access when _preEnabled and _postEnabled dictionary are accessed 
        /// </summary>
        readonly Object _lockerObject = new object();

        /// <summary>
        /// dictionary that stores user permissions
        /// key user id 
        /// </summary>
         readonly Dictionary<int, CompliancePermissions> _permissions = new Dictionary<int, CompliancePermissions>();
        public Dictionary<int, CompliancePermissions> Permissions
        {
            get { return _permissions; }
        }


        /// <summary>
        /// The rules allowed for override
        /// </summary>
        ImmutableSortedDictionary<int, HashSet<string>> _rulesAllowedForOverride;

        /// <summary>
        /// The rules for send to comp officer
        /// </summary>
        ImmutableSortedDictionary<int, HashSet<string>> _rulesForSendToCompOfficer;

        /// <summary>
        /// The rules for block
        /// </summary>
        ImmutableSortedDictionary<int, HashSet<string>> _rulesForBlock;

        /// <summary>
        /// The rules for Soft With Notes.
        /// </summary>
        ImmutableSortedDictionary<int, HashSet<string>> _rulesForSoftWithNotes;

        #region Compliance


        /// <summary>
        /// Return permission of pre trade module Enabled for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have enabled of pre trade module</returns>
        internal bool GetPreTradeModuleEnabledForUser(int companyUserId)
        {
            try
            {
                lock (_preTradeModule)
                {
                    if (_preTradeModule.ContainsKey(companyUserId))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }


        /// <summary>
        /// Return permission of pre trade module Enabled for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have enabled of pre trade module</returns>
        internal bool GetPostTradeModuleEnabledForUser(int companyUserId)
        {
            try
            {
                lock (_postTradeModule)
                {
                    if (_postTradeModule.ContainsKey(companyUserId))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }


        /// <summary>
        /// Return permission of pre trade module permission for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have permission of pre trade module</returns>
        internal bool GetPretradeModulePermission(int companyUserId)
        {
            try
            {
                lock (_preTradeModule)
                {
                    if (_preTradeModule.ContainsKey(companyUserId))
                        return Convert.ToBoolean(_preTradeModule[companyUserId]);
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }


        internal bool GetPreOrPostModuleEnabledForCompany()
        {
            try
            {
                lock (_lockerObject)
                {
                    if (
                                       (_preEnabled.ContainsKey(_companyID) && _preEnabled[_companyID]) ||
                                       (_postEnabled.ContainsKey(_companyID) && _postEnabled[_companyID])
                                   )
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }

            //return _prePostEnabled[_companyID];

        }

        internal bool GetPreModuleEnabledForCompany()
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_preEnabled.ContainsKey(_companyID))
                        return _preEnabled[_companyID];
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        internal bool GetPostModuleEnabledForCompany()
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_postEnabled.ContainsKey(_companyID))
                        return _postEnabled[_companyID];
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Return permission of post trade module permission for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have permission of post trade module</returns>

        internal bool GetPostTradeModulePermission(int companyUserId)
        {
            try
            {
                lock (_postTradeModule)
                {
                    if (_postTradeModule.ContainsKey(companyUserId))
                        return _postTradeModule[companyUserId];
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Return whether the user is simple or power user  for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user is power user </returns>


        internal bool GetPowerUserCheck(int userId)
        {
            try
            {
                //string key =CachedData.GetInstance
                lock (_permissions)
                {
                    if (_permissions.ContainsKey(userId))
                        return _permissions[userId].IsPowerUser;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Return Permission for manually allow trade for current user
        /// </summary>
        /// <param name="userId">User id  for current user</param>
        /// <returns>True if Manually Trading allowed  for current user</returns>
        internal bool GetApplyToManualCheck(int userId)
        {
            try
            {
                //string key =CachedData.GetInstance
                lock (_permissions)
                {
                    if (_permissions.ContainsKey(userId))
                        return _permissions[userId].RuleCheckPermission.IsApplyToManual;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }



        /// <summary>
        /// Return Pretrade Check permission for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have Pretrade Check permission </returns>

        internal bool GetPreTradeCheck(int userId)
        {
            try
            {

                if (!GetPreTradeModuleEnabledForUser(userId))
                    return false;


                lock (_permissions)
                {
                    if (_permissions.ContainsKey(userId))
                        return _permissions[userId].RuleCheckPermission.IsPreTradeEnabled && _permissions[userId].RuleCheckPermission.IsTrading;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        // All of the functions have to be moved here and we'll call them from cacheddatamanager

        #endregion

        private void LoadPrePostModuleEnabled(int _companyID)
        {
            try
            {
                //if (!_prePostEnabled.ContainsKey(_companyID))
                //    _prePostEnabled.Add(_companyID, CompliacePermissionDataManager.GetInstance().CheckPrePostComplianceEnabled(_companyID));
                if (!_preEnabled.ContainsKey(_companyID))
                    _preEnabled.Add(_companyID, CompliacePermissionDataManager.GetInstance().CheckPreComplianceEnabled(_companyID));
                if (!_postEnabled.ContainsKey(_companyID))
                    _postEnabled.Add(_companyID, CompliacePermissionDataManager.GetInstance().CheckPostComplianceEnabled(_companyID));



            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// load Read write permission for a User  of Pre and Post trade compliance Module.
        /// </summary>
        private void LoadCompliancePrePostPermissions()
        {
            try
            {
                DataSet ds = CompliacePermissionDataManager.GetInstance().GetPrePostModulePermission(_companyID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int companyUserId = Convert.ToInt32(dr["CompanyUserId"]);
                    String moduleName = (dr["ModuleName"].ToString());
                    bool read_WriteId = Convert.ToBoolean(dr["ReadWriteId"]);
                    if (moduleName == "Compliance Pre Trade")
                    {
                        if (_preTradeModule.ContainsKey(companyUserId))
                        {
                            _preTradeModule[companyUserId] = read_WriteId;
                        }
                        else
                        {
                            _preTradeModule.Add(companyUserId, read_WriteId);
                        }
                    }
                    else if (moduleName == "Compliance Post Trade")
                    {
                        if (_postTradeModule.ContainsKey(companyUserId))
                        {
                            _postTradeModule[companyUserId] = read_WriteId;
                        }
                        else
                        {
                            _postTradeModule.Add(companyUserId, read_WriteId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Load all permission of pretrade check, manual trade, power user and override_permission in dictionaries for user
        /// </summary>        
        private void LoadOverridePreTradeCheckPowerUserPermission()
        {
            try
            {
                var overrideRulesBuilder = ImmutableSortedDictionary.CreateBuilder<int, HashSet<string>>();
                var sendToCompRulesBuilder = ImmutableSortedDictionary.CreateBuilder<int, HashSet<string>>();
                var blockRulesBuilder = ImmutableSortedDictionary.CreateBuilder<int, HashSet<string>>();
                var softWithNotesBuilder = ImmutableSortedDictionary.CreateBuilder<int, HashSet<string>>();

                DataSet ds = CompliacePermissionDataManager.GetInstance().GetPermission(_companyID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int companyUserId = Convert.ToInt32(dr["UserId"]);
                    CompliancePermissions compliancePermissions = new CompliancePermissions(dr);

                    if (_permissions.ContainsKey(companyUserId))
                        _permissions[companyUserId] = compliancePermissions;
                    else
                        _permissions.Add(companyUserId, compliancePermissions);

                    //Getting data for individual rule permission
                    UpdateDataForUserRulePermissions(companyUserId, overrideRulesBuilder, sendToCompRulesBuilder, blockRulesBuilder, softWithNotesBuilder);
                }
                _rulesAllowedForOverride = overrideRulesBuilder.ToImmutable();
                _rulesForSendToCompOfficer = sendToCompRulesBuilder.ToImmutable();
                _rulesForBlock = blockRulesBuilder.ToImmutable();
                _rulesForSoftWithNotes = softWithNotesBuilder.ToImmutable();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the data for user rule permissions.
        /// </summary>
        /// <param name="companyUserId">The company user identifier.</param>
        /// <param name="overrideRulesBuilder">The override rules builder.</param>
        /// <param name="sendToCompRulesBuilder">The send to comp rules builder.</param>
        /// <param name="blockRulesBuilder">The block rules builder.</param>
        private static void UpdateDataForUserRulePermissions(int companyUserId, ImmutableSortedDictionary<int, HashSet<string>>.Builder overrideRulesBuilder, ImmutableSortedDictionary<int, HashSet<string>>.Builder sendToCompRulesBuilder, ImmutableSortedDictionary<int, HashSet<string>>.Builder blockRulesBuilder, ImmutableSortedDictionary<int, HashSet<string>>.Builder softWithNotesBuilder)
        {
            try
            {
                DataSet dataSet = CompliacePermissionDataManager.GetInstance().GetRuleOverRidden(companyUserId);
                if ((dataSet.Tables[0].Rows.Count) > 0)
                {
                    HashSet<string> overrideRules = new HashSet<string>();
                    HashSet<string> sendToCompOfficerRules = new HashSet<string>();
                    HashSet<string> blockRules = new HashSet<string>();
                    HashSet<string> softWithNotesRules = new HashSet<string>();

                    string preExclude = "PreTrade";
                    var preFilteredRows = from row in dataSet.Tables[0].AsEnumerable()
                                          where preExclude.Contains(row.Field<string>("PackageName"))
                                          select row;

                    foreach (DataRow row in preFilteredRows)
                    {
                        switch (Convert.ToInt32(row["AlertTypePermission"]))
                        {
                            case (int)RuleOverrideType.Soft:
                                overrideRules.Add(row["RuleName"].ToString());
                                break;
                            case (int)RuleOverrideType.RequiresApproval:
                                sendToCompOfficerRules.Add(row["RuleName"].ToString());
                                break;
                            case (int)RuleOverrideType.Hard:
                                blockRules.Add(row["RuleName"].ToString());
                                break;
                            case (int)RuleOverrideType.SoftWithNotes:
                                softWithNotesRules.Add(row["RuleName"].ToString());
                                break;
                        }
                    }

                    if (overrideRules.Count > 0)
                        overrideRulesBuilder.Add(companyUserId, overrideRules);
                    if (sendToCompOfficerRules.Count > 0)
                        sendToCompRulesBuilder.Add(companyUserId, sendToCompOfficerRules);
                    if (blockRules.Count > 0)
                        blockRulesBuilder.Add(companyUserId, blockRules);
                    if (softWithNotesRules.Count > 0)
                        softWithNotesBuilder.Add(companyUserId, softWithNotesRules);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void GetCompliancePref()
        {
            try
            {
                DataSet dsCompliancePref = CompliacePermissionDataManager.GetCompliancePreferences(_companyID);
                if (dsCompliancePref != null)
                {
                    //Putting a check if default path has not been set from admin
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3245
                    if (dsCompliancePref.Tables.Count > 0 && dsCompliancePref.Tables[0].Rows.Count > 0)
                    {
                        _compliancePref.ImportExportPath = dsCompliancePref.Tables[0].Rows[0]["ImportExportPath"].ToString();
                        _compliancePref.PrePostCrossImportAllowed = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["PrePostCrossImport"].ToString());
                        _compliancePref.InMarket = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["InMarket"].ToString());
                        _compliancePref.InStage = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["InStage"].ToString());
                        _compliancePref.PostInMarket = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["PostInMarket"].ToString());
                        _compliancePref.PostInStage = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["PostInStage"].ToString());
                        _compliancePref.BlockTradeOnComplianceFaliure = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["BlockTradeOnComplianceFaliure"].ToString());
                        _compliancePref.StageValueFromField = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["StageValueFromField"].ToString());
                        _compliancePref.StageValueFromFieldString = dsCompliancePref.Tables[0].Rows[0]["StageValueFromFieldString"].ToString();
                        _compliancePref.IsBasketComplianceEnabledCompany = Convert.ToBoolean(dsCompliancePref.Tables[0].Rows[0]["IsBasketComplianceEnabledCompany"].ToString());
                    }
                    else
                    {
                        throw new Exception("Import/Export preferences are not set from admin. Please set it then restart the client");
                    }
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

        internal string GetImportExportPath()
        {
            try
            {
                if (String.IsNullOrEmpty(_compliancePref.ImportExportPath))
                    return String.Empty;
                else
                    return _compliancePref.ImportExportPath;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        internal bool GetPrePostCrossImportAllowed()
        {
            try
            {
                return _compliancePref.PrePostCrossImportAllowed;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Getting InMarket field
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>true/false</returns>
        internal bool GetInMarket()
        {
            try
            {
                return _compliancePref.InMarket;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Getting InStage field
        /// </summary>
        /// <param name="companyId">company Id</param>
        /// <returns>true/false</returns>
        internal bool GetInStage()
        {
            try
            {
                return _compliancePref.InStage;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the post in market.
        /// </summary>
        /// <returns></returns>
        internal bool GetPostInMarket()
        {
            try
            {
                return _compliancePref.PostInMarket;
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
        /// Gets the post in stage.
        /// </summary>
        /// <returns></returns>
        internal bool GetPostInStage()
        {
            try
            {
                return _compliancePref.PostInStage;
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
        /// Gets the post with in market in stage.
        /// </summary>
        /// <returns></returns>
        internal int GetPostWithInMarketInStage()
        {
            try
            {
                return _compliancePref.PostWithInMarketInStage;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return 1; // default value 1
            }
        }

        /// <summary>
        /// Gets the IsBasketComplianceEnabled in Company.
        /// </summary>
        /// <returns></returns>
        internal bool GetIsBasketComplianceEnabledCompany()
        {
            try
            {
                return _compliancePref.IsBasketComplianceEnabledCompany;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false; //Default value false
            }
        }

        /// <summary>
        /// Gets the post with in market in stage.
        /// </summary>
        /// <returns></returns>
        internal bool GetBlockTradeOnComplianceFaliure()
        {
            try
            {
                return _compliancePref.BlockTradeOnComplianceFaliure;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return true; // default value 1
            }
        }

        /// <summary>
        /// Gets the post with in market in stage.
        /// </summary>
        /// <returns></returns>
        internal bool GetStageValueFromField()
        {
            try
            {
                return _compliancePref.StageValueFromField;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return true; // default value 1
            }
        }
        /// <summary>
        /// Get Stage Value From Field String
        /// </summary>
        /// <returns></returns>
        internal string GetStageValueFromFieldString()
        {
            try
            {
                return _compliancePref.StageValueFromFieldString;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty; // default value 1
            }
        }

        /// <summary>
        /// Gets Whether user can enable/disable rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetEnablePermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsEnable;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets Whether user can Rename rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetRenamePermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsRename;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets Whether user can create/update rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetCreatePermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsCreate;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets Whether user can Delete rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetDeletePermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsDelete;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets Whether user can import rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetImportPermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsImport;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets Whether user can Export rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetExportPermission(String packageName, int userId)
        {
            try
            {
                if (_permissions.ContainsKey(userId))
                {
                    if (_permissions[userId].complianceUIPermissions.ContainsKey((RuleType)Enum.Parse(typeof(RuleType), packageName)))
                        return _permissions[userId].complianceUIPermissions[(RuleType)Enum.Parse(typeof(RuleType), packageName)].IsExport;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Getting OverRidden Rule Permission If Have permission then Return true otherwise false
        /// </summary>
        /// <param name="userID">User Id</param>
        /// <param name="ruleID">Rule Id</param>
        /// <returns>true/false</returns>
        internal RuleOverrideType GetOverridePermissionForRule(int userID, HashSet<string> uniqueRules)
        {
            try
            {
                if (_rulesForBlock.ContainsKey(userID) && _rulesForBlock[userID].Overlaps(uniqueRules))
                    return RuleOverrideType.Hard;
                if (_rulesForSendToCompOfficer.ContainsKey(userID) && _rulesForSendToCompOfficer[userID].Overlaps(uniqueRules))
                    return RuleOverrideType.RequiresApproval;
                if ((_rulesAllowedForOverride.ContainsKey(userID) && _rulesAllowedForOverride[userID].Overlaps(uniqueRules)) || (_rulesForSoftWithNotes.ContainsKey(userID) && _rulesForSoftWithNotes[userID].Overlaps(uniqueRules)))
                    return RuleOverrideType.Soft;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _permissions[userID].RuleCheckPermission.DefaultOverRideType;
        }

        /// <summary>
        /// Gets alert type for specific rule.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="uniqueRules"></param>
        /// <returns></returns>
        internal AlertType GetAlertForRule(int userID, string uniqueRules)
        {
            try
            {
                if (_rulesForBlock.ContainsKey(userID) && _rulesForBlock[userID].Contains(uniqueRules))
                    return AlertType.HardAlert;
                if (_rulesForSendToCompOfficer.ContainsKey(userID) && _rulesForSendToCompOfficer[userID].Contains(uniqueRules))
                    return AlertType.RequiresApproval;
                if (_rulesAllowedForOverride.ContainsKey(userID) && _rulesAllowedForOverride[userID].Contains(uniqueRules))
                    return AlertType.SoftAlert;
                if (_rulesForSoftWithNotes.ContainsKey(userID) && _rulesForSoftWithNotes[userID].Contains(uniqueRules))
                    return AlertType.SoftAlertWithNotes;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return AlertType.SoftAlert;
        }

        /// <summary>
        /// Checks if pre trade to be checked while staging
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetPreTradeCheckStaging(int userId)
        {
            try
            {
                if (!GetPreTradeModuleEnabledForUser(userId))
                    return false;


                lock (_permissions)
                {
                    if (_permissions.ContainsKey(userId))
                        return _permissions[userId].RuleCheckPermission.IsPreTradeEnabled && _permissions[userId].RuleCheckPermission.IsStaging;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Getting Overridden Rule Permission
        /// </summary>
        /// <param name="uniqueRuleName"></param>
        /// <returns></returns>
        internal Dictionary<string, List<int>> GetOverriddenRulePermission(List<string> uniqueRuleName)
        {
            try
            {
                Dictionary<string, List<int>> overRiddenRulePermission = new Dictionary<string, List<int>>();
                foreach (int key in _rulesAllowedForOverride.Keys)
                {
                    foreach (string rule in uniqueRuleName)
                    {
                        if (_rulesAllowedForOverride[key].Contains(rule))
                        {
                            if (overRiddenRulePermission.ContainsKey(rule))
                            {
                                if (!overRiddenRulePermission[rule].Contains(key))
                                {
                                    overRiddenRulePermission[rule].Add(key);
                                }
                            }
                            else
                            {
                                overRiddenRulePermission.Add(rule, new List<int>() { key });
                            }
                        }
                    }
                }
                return overRiddenRulePermission;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Get Company User Email Ids having compliance permission
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetCompanyUserEmailIds()
        {
            try
            {
                lock (_companyUserEmailIds)
                {
                    foreach (KeyValuePair<int, string> userId in _companyUserEmailIds)
                    {
                        if (_preTradeModule.ContainsKey(userId.Key) && _postTradeModule.ContainsKey(userId.Key))
                        {
                            if (!_companyUserEmailIdsCompliance.ContainsKey(userId.Key))
                                _companyUserEmailIdsCompliance.Add(userId.Key, userId.Value);
                        }
                    }
                    return _companyUserEmailIdsCompliance;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Return Compliance check permission on Rebalancer for current user login
        /// </summary>
        /// <param name="companyUserId">User id of current user </param>
        /// <returns>true/false if current user have Compliance check permission on Rebalancer </returns>
        internal bool GetBasketComplianceCheckPermissionUser(int userId)
        {
            try
            {
                lock (_permissions)
                {
                    if (_permissions.ContainsKey(userId))
                        return _permissions[userId].EnableBasketComplianceCheck;
                    else
                        return false;
                }
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
        /// Updates rule name
        /// </summary>
        /// <param name="oldRuleName">old rule name </param>
        /// <param name="newRuleName">new rule name </param>
        internal void UpdateRenamedRule(string oldRuleName, string newRuleName)
        {
            try
            {
                foreach (int userID in _rulesForBlock.Keys)
                    if (_rulesForBlock[userID].Contains(oldRuleName))
                    {
                        _rulesForBlock[userID].Remove(oldRuleName);
                        _rulesForBlock[userID].Add(newRuleName);
                    }
                foreach (int userID in _rulesForSendToCompOfficer.Keys)
                    if (_rulesForSendToCompOfficer[userID].Contains(oldRuleName))
                    {
                        _rulesForSendToCompOfficer[userID].Remove(oldRuleName);
                        _rulesForSendToCompOfficer[userID].Add(newRuleName);
                    }
                foreach (int userID in _rulesAllowedForOverride.Keys)
                    if (_rulesAllowedForOverride[userID].Contains(oldRuleName))
                    {
                        _rulesAllowedForOverride[userID].Remove(oldRuleName);
                        _rulesAllowedForOverride[userID].Add(newRuleName);
                    }
                foreach (int userID in _rulesForSoftWithNotes.Keys)
                    if (_rulesForSoftWithNotes[userID].Contains(oldRuleName))
                    {
                        _rulesForSoftWithNotes[userID].Remove(oldRuleName);
                        _rulesForSoftWithNotes[userID].Add(newRuleName);
                    }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds new rule
        /// </summary>
        /// <param name="addedRuleName">added rule name </param>
        internal void AddRuleInCache(string addedRuleName)
        {
            try
            {
                foreach (int userID in _rulesForBlock.Keys)
                    if (_permissions[userID].RuleCheckPermission.DefaultOverRideType.Equals(RuleOverrideType.Hard) && !_rulesForBlock[userID].Contains(addedRuleName))
                    {
                        _rulesForBlock[userID].Add(addedRuleName);
                    }
                foreach (int userID in _rulesForSendToCompOfficer.Keys)
                    if (_permissions[userID].RuleCheckPermission.DefaultOverRideType.Equals(RuleOverrideType.RequiresApproval) && !_rulesForSendToCompOfficer[userID].Contains(addedRuleName))
                    {
                        _rulesForSendToCompOfficer[userID].Add(addedRuleName);
                    }
                foreach (int userID in _rulesAllowedForOverride.Keys)
                    if (_permissions[userID].RuleCheckPermission.DefaultOverRideType.Equals(RuleOverrideType.Soft) && !_rulesAllowedForOverride[userID].Contains(addedRuleName))
                    {
                        _rulesAllowedForOverride[userID].Add(addedRuleName);
                    }
                foreach (int userID in _rulesForSoftWithNotes.Keys)
                    if (_permissions[userID].RuleCheckPermission.DefaultOverRideType.Equals(RuleOverrideType.SoftWithNotes) && !_rulesForSoftWithNotes[userID].Contains(addedRuleName))
                    {
                        _rulesForSoftWithNotes[userID].Add(addedRuleName);
                    }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// TO update Compliance preference
        /// </summary>
        /// <param name="pref"></param>
        internal void UpdateCompliancePref(CompliancePref pref)
        {
            try
            {
                _compliancePref = pref;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}