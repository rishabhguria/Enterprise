using Prana.AuditManager.Definitions.Data;
using Prana.AuditManager.Definitions.Enum;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace Prana.AuditManager.DAL
{

    /// <summary>
    /// Class to handle audit detail for selected user
    /// </summary>
    internal class AuditDataManager
    {

        static Object singletonLockerObject = new object();
        static AuditDataManager _singletonInstance;

        internal static AuditDataManager GetInstance()
        {
            try
            {
                lock (singletonLockerObject)
                {
                    if (_singletonInstance == null)
                        _singletonInstance = new AuditDataManager();
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
            return _singletonInstance;
        }

        private AuditDataManager()
        { }

        /// <summary>
        /// Function to save audit data to database
        /// </summary>
        /// <param name="auditData"></param>
        internal void LogAuditData(List<AuditDataDefinition> auditData)
        {
            try
            {
                foreach (AuditDataDefinition data in auditData)
                {
                    object[] parameterSave = new object[11];
                    parameterSave[0] = data.CompanyId;
                    parameterSave[1] = data.CompanyAccountId;
                    parameterSave[2] = data.UserId;
                    parameterSave[3] = data.Action;
                    parameterSave[4] = data.AppliedAuditTime;
                    parameterSave[5] = data.StatusId;
                    parameterSave[6] = data.Comment;
                    parameterSave[7] = data.ModuleId;
                    parameterSave[8] = data.ActualAuditTime;
                    parameterSave[9] = data.AuditDimensionValue;
                    // Purpose : To save audit detail for deletion
                    parameterSave[10] = true;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveAuditData", parameterSave);

                    // Purpose : To deactivate deleted data
                    if (data.IsActive == false)
                    {
                        object[] parameterDel = new object[4];
                        parameterDel[0] = data.AuditDimensionValue;
                        parameterDel[1] = data.Action.Equals(AuditAction.AccountDeleted) ? AuditAction.AccountCreated : data.Action.Equals(AuditAction.MasterFundDeleted) ? AuditAction.MasterFundCreated : data.Action.Equals(AuditAction.BatchDeleted) ? AuditAction.BatchCreated : data.Action.Equals(AuditAction.MasterStrategyDeleted) ? AuditAction.MasterStrategyCreated : data.Action.Equals(AuditAction.PricingRuleDeleted) ? AuditAction.PricingRuleCreated : data.Action.Equals(AuditAction.StrategyDeleted) ? AuditAction.StrategyCreated : AuditAction.NotDefined;
                        parameterDel[2] = data.Action.Equals(AuditAction.AccountDeleted) ? AuditAction.AccountUpdated : data.Action.Equals(AuditAction.MasterFundDeleted) ? AuditAction.MasterFundUpdated : data.Action.Equals(AuditAction.BatchDeleted) ? AuditAction.BatchUpdated : data.Action.Equals(AuditAction.MasterStrategyDeleted) ? AuditAction.MasterStrategyUpdated : data.Action.Equals(AuditAction.PricingRuleDeleted) ? AuditAction.PricingRuleUpdated : data.Action.Equals(AuditAction.StrategyDeleted) ? AuditAction.StrategyUpdated : AuditAction.NotDefined;
                        parameterDel[3] = data.Action.Equals(AuditAction.AccountDeleted) ? AuditAction.AccountApproved : data.Action.Equals(AuditAction.MasterFundDeleted) ? AuditAction.MasterFundApproved : data.Action.Equals(AuditAction.BatchDeleted) ? AuditAction.BatchApproved : data.Action.Equals(AuditAction.MasterStrategyDeleted) ? AuditAction.MasterStrategyApproved : data.Action.Equals(AuditAction.PricingRuleDeleted) ? AuditAction.PricingRuleApproved : data.Action.Equals(AuditAction.StrategyDeleted) ? AuditAction.StrategyApproved : AuditAction.NotDefined;

                        DatabaseManager.DatabaseManager.ExecuteNonQuery("P_InActiveAuditData", parameterDel);
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

            //object[] parameter = new object[1];
            //parameter[0] = xmlElements.ToString();



        }

        /// <summary>
        /// Function to get datalist for audit details
        /// </summary>
        /// <param name="actionList"></param>
        /// <returns></returns>
        internal List<AuditDataDefinition> GetAuditDataFor(List<AuditAction> actionList)
        {
            List<AuditDataDefinition> dataList = new List<AuditDataDefinition>();


            XElement xmlElements = new XElement("ActionIds", actionList.Select(i => new XElement("ActionId", (int)i)));

            try
            {
                object[] parameter = new object[1];
                parameter[0] = xmlElements.ToString();
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAuditForActions", parameter);


                //Load data from databse for given action list
                // this might be helpful for multiple action from source

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AuditDataDefinition def = new AuditDataDefinition();
                    def.Action = (AuditAction)Enum.Parse(typeof(AuditAction), dr["ActionId"].ToString());
                    def.ActualAuditTime = DateTime.Parse(dr["ActualActionDate"].ToString());
                    def.UserId = Convert.ToInt32(dr["UserId"]);
                    def.CompanyId = Convert.ToInt32(dr["CompanyId"]);
                    def.CompanyAccountId = Convert.ToInt32(dr["CompanyFundId"]);
                    def.Comment = dr["Comments"].ToString();
                    def.StatusId = Convert.ToInt32(dr["StatusID"]);
                    def.ModuleId = Convert.ToInt32(dr["ModuleId"]);
                    def.AuditDimensionValue = Convert.ToInt32(dr["AuditDimensionValue"]);
                    def.AppliedAuditTime = DateTime.Parse(dr["ExecutionTime"].ToString());
                    def.IsActive = Convert.ToBoolean(dr["IsActive"]);

                    dataList.Add(def);
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


            //foreach (AuditAction cat in actionList)
            //{
            //    AuditDataDefinition def = new AuditDataDefinition();
            //    def.Action = cat;
            //    dataList.Add(def);
            //}


            return dataList;
        }



    }
}