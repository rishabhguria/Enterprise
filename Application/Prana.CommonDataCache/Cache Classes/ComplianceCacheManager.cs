using Prana.BusinessObjects.Compliance.CompliancePref;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CommonDataCache
{

    public class ComplianceCacheManager
    {
        public static bool GetPreOrPostModuleEnabledForUser(int userId)
        {
            if (ComplianceCachedData.GetInstance().GetPreOrPostModuleEnabledForCompany())
            {
                if (ComplianceCachedData.GetInstance().GetPreTradeModuleEnabledForUser(userId) || ComplianceCachedData.GetInstance().GetPostTradeModuleEnabledForUser(userId))
                    return true;
                else
                    return false;
            }
            else
            {
                //returning false if compliance module is disabled for company
                return false;
            }
        }


        /// <summary>
        /// Returns true if any of the module is enabled
        /// </summary>
        /// <returns></returns>
        public static bool GetPreOrPostModuleEnabled()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPreOrPostModuleEnabledForCompany();
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
        /// Returns true if pre trade compliance is enabled for company
        /// </summary>
        /// <returns></returns>
        public static bool GetPreComplianceModuleEnabled()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPreModuleEnabledForCompany();
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

        public static bool GetPostComplianceModuleEnabled()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPostModuleEnabledForCompany();
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



        public static bool GetPreTradeModuleEnabledForUser(int companyUserId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPreTradeModuleEnabledForUser(companyUserId);
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
        public static bool GetPostTradeModuleEnabledForUser(int companyUserId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPostTradeModuleEnabledForUser(companyUserId);
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
        /// Returns pre-trade Module permission for the given user 
        /// </summary>
        /// <param name="companyUserId">Company userid for the user permission required</param>
        /// <returns>0 - read, 1- read/write, -1 - not found</returns>
        public static bool GetPretradeModulePermission(int companyUserId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPretradeModulePermission(companyUserId);
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
        /// Returns Post-trade Module permission for the given user 
        /// </summary>
        /// <param name="companyUserId">Company userid for the user permission required</param>
        /// <returns>0 - read, 1- read/write, -1 - not found</returns>
        public static bool GetPostTradeModulePermission(int companyUserId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPostTradeModulePermission(companyUserId);
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
        /// Get Apply TO manual Permission from cachedata for a user
        /// </summary>
        /// <param name="userId"> user id for current user</param>
        /// <returns>ApplyToManual permission int value</returns>
        public static bool GetApplyToManualPermission(int userId)
        {
            //string key =CachedData.GetInstance
            try
            {
                return ComplianceCachedData.GetInstance().GetApplyToManualCheck(Convert.ToInt32(userId));
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
        /// Returns true/false whether the given user is a power user
        /// </summary>
        /// <param name="userId">Company userId for which power user setting needs to be checked</param>
        /// <returns>true/false if user is a power user. Returns false if not found for user</returns>
        public static bool GetPowerUserCheck(int userId)
        {
            //string key =CachedData.GetInstance
            try
            {
                return ComplianceCachedData.GetInstance().GetPowerUserCheck(userId);
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
        /// Get PreTrade Check permission from Cache Data for a user
        /// </summary>
        /// <param name="userId">Userid from T_CA_  PreTradeCheck</param>
        /// <returns>PretradeCheck Permission int value</returns>
        public static bool GetPreTradeCheck(int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPreTradeCheck(userId);
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

        public static string GetImportExportPath()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetImportExportPath();
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

        public static bool GetPrePostCrossImportAllowed()
        {
            return ComplianceCachedData.GetInstance().GetPrePostCrossImportAllowed();
        }

        /// <summary>
        /// Gets Whether user can enable/disable rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetEnablePermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetEnablePermission(packageName, userId);
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
        public static bool GetRenamePermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetRenamePermission(packageName, userId);
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
        /// Gets Whether user can Create rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetCreatePermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetCreatePermission(packageName, userId);
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
        /// Gets Whether user can delete rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetDeletePermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetDeletePermission(packageName, userId);
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
        public static bool GetImportPermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetImportPermission(packageName, userId);
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
        /// Gets Whether user can export rule of that package
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetExportPermission(String packageName, int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetExportPermission(packageName, userId);
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
        /// Getting OverRidden Permission for Individual Rule
        /// </summary>
        /// <param name="userID">User Id</param>
        /// <param name="ruleID">Rule Id</param>
        /// <returns>True/False</returns>
        public static RuleOverrideType GetOverridePermissionForRule(int userID, HashSet<string> uniqueRules)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetOverridePermissionForRule(userID, uniqueRules);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return RuleOverrideType.Soft;
            }
        }

        /// <summary>
        /// Gets rule type for specific rule
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="uniqueRules"></param>
        /// <returns></returns>
        public static AlertType GetAlertForRule(int userID, string uniqueRules)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetAlertForRule(userID, uniqueRules);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return AlertType.HardAlert;
            }
        }

        /// <summary>
        /// returns if Pre trade to be checked for user on staging
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetPreTradeCheckStaging(int userId)
        {
            return ComplianceCachedData.GetInstance().GetPreTradeCheckStaging(userId);
        }

        /// <summary>
        /// Include In market trade in what if
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static bool GetInMarket()
        {
            return ComplianceCachedData.GetInstance().GetInMarket();
        }

        /// <summary>
        /// Include in stage trade in what if
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static bool GetInStaging()
        {
            return ComplianceCachedData.GetInstance().GetInStage();
        }

        /// <summary>
        /// Getting Overridden Rule Permission
        /// </summary>
        /// <param name="uniqueRuleName"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetOverriddenRulePermission(List<string> uniqueRuleName)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetOverriddenRulePermission(uniqueRuleName);
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
        /// The _compliance UI tab selected
        /// </summary>
        static string _complianceUITabSelected = ApplicationConstants.CONST_COMPLIANCE_RULE_DEFINITION;

        /// <summary>
        /// Set the value true if Pending approval UI is to be select
        /// </summary>
        public static void SetComplianceUITabSelected(string action)
        {
            try
            {
                _complianceUITabSelected = action;
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
        /// Get the value pending appoval UI select
        /// </summary>
        public static string GetComplianceUITabSelect()
        {
            return _complianceUITabSelected;
        }

        /// <summary>
        /// Get Company User Email Ids
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetCompanyUserEmailIds()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetCompanyUserEmailIds();
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
        /// Gets the post with in market in stage.
        /// </summary>
        /// <returns></returns>
        public static int GetPostWithInMarketInStage()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetPostWithInMarketInStage();
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
        /// Gets the post with in market in stage.
        /// </summary>
        /// <returns></returns>
        public static bool GetBlockTradeOnComplianceFaliure()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetBlockTradeOnComplianceFaliure();
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
        /// Get Compliance Check permission for Rebalancer from Cache Data for a user
        /// </summary>
        public static bool GetBasketComplianceCheckPermissionUser(int userId)
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetBasketComplianceCheckPermissionUser(userId);
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
        /// Get Stage Value From Field String
        /// </summary>
        /// <returns></returns>
        public static bool GetStageValueFromField()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetStageValueFromField();
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
        public static string GetStageValueFromFieldString()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetStageValueFromFieldString();
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
        /// Updates rule name
        /// </summary>
        /// <param name="oldRuleName">old rule name </param>
        /// <param name="newRuleName">new rule name </param>
        public static void UpdateRenamedRule(String oldRuleName, String newRuleName)
        {
            try
            {
                ComplianceCachedData.GetInstance().UpdateRenamedRule(oldRuleName, newRuleName);
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
        public static void AddRuleInCache(String addedRuleName)
        {
            try
            {
                ComplianceCachedData.GetInstance().AddRuleInCache(addedRuleName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Returns true if Basket compliance is enabled for company
        /// </summary>
        /// <returns></returns>
        public static bool GetIsBasketComplianceEnabledCompany()
        {
            try
            {
                return ComplianceCachedData.GetInstance().GetIsBasketComplianceEnabledCompany();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;//Default value is false
            }
        }

        /// <summary>
        /// TO update Compliance preference
        /// </summary>
        /// <param name="pref"></param>
        public static void UpdateCompliancePref(CompliancePref pref)
        {
            try
            {
                ComplianceCachedData.GetInstance().UpdateCompliancePref(pref);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        /// <summary>
        /// TO Check If Basket Compliance is Enabled or not
        /// </summary>
        /// <param name="pref"></param>

        public static bool CheckIsBasketComplianceEnabled(int userId)
        {
            try
            {
                if (ComplianceCachedData.GetInstance().GetPreTradeCheck(userId) && ComplianceCachedData.GetInstance().GetBasketComplianceCheckPermissionUser(userId))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;//Default value is false
            }
        }
    }
}