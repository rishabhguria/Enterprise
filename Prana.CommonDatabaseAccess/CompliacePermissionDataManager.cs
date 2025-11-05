using Prana.LogManager;
using System;
using System.Data;


//add new class in DAL in CommanData cache
namespace Prana.CommonDataCache.DAL
{

    public class CompliacePermissionDataManager
    {
        //private static readonly CompliacePermissionDataManager instance = new CompliacePermissionDataManager();
        static CompliacePermissionDataManager _compliacePermissionDataManager;
        public static CompliacePermissionDataManager GetInstance()
        {
            if (_compliacePermissionDataManager == null)
                _compliacePermissionDataManager = new CompliacePermissionDataManager();
            return _compliacePermissionDataManager;
        }
        /// <summary>
        /// get permission of Read write Of pre trade and post trade Compliance module from Data Base
        /// Company id id hard coded as 6.
        /// </summary>
        /// <returns>module id and read Write Id for a user</returns>
        public DataSet GetPrePostModulePermission(int companyId)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;

                return DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetPrePostModulesPermission", parameter);
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
        /// Get all permission of preTrade check and override permission for all users   from DataBase 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPermission(int companyId)
        {
            //TODO: this is fixed company id initialization need to review

            try
            {
                //int companyId = 6;
                object[] parameter = new object[1];
                parameter[0] = companyId;

                //Gets data from T_CA_UserReadWritePermission and T_CA_OtherCompliancePermission for companyId and userId
                return DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetCompliancePermissionCompany", parameter);
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

        public Boolean CheckPreComplianceEnabled(int companyId)
        {
            //int companyId = 6;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_CheckPreComplianceEnabled", parameter);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
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


        public Boolean CheckPostComplianceEnabled(int companyId)
        {
            //int companyId = 6;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_CheckPostComplianceEnabled", parameter);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
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

        public static DataSet GetCompliancePreferences(int companyId)
        {
            try
            {
                String procedureName = "P_CA_GetCompliancePreferences";

                return DatabaseManager.DatabaseManager.ExecuteDataSet(procedureName, new object[] { companyId });
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

        public DataSet GetRuleOverRidden(int companyUserId)
        {
            try
            {
                String procedureName = "P_CA_GetRuleOverRiddenPermission";

                return DatabaseManager.DatabaseManager.ExecuteDataSet(procedureName, new object[] { companyUserId });
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
        /// Get Company User Email Ids
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public static DataSet GetCompanyUserEmailIds(int companyId)
        {
            try
            {
                String procedureName = "P_CA_GetCompanyUserEmailIds";

                return DatabaseManager.DatabaseManager.ExecuteDataSet(procedureName, new object[] { companyId });
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

    }
}