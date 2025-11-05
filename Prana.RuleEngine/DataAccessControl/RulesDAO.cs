using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.RuleEngine.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Prana.RuleEngine.DataAccessControl
{
    internal class RulesDAO
    {
        static  String connetionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString;
        static SqlConnection sqlCnn = new SqlConnection(connetionString);
        internal static DataSet GetNotificationFrequencyList()
        {
            string sql = @"select ID,MeasurementDescription  from T_CA_NotifyFrequency";
            SqlCommand sqlCmd = new SqlCommand(sql, sqlCnn); ;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = sqlCmd;
            try
            {
                sqlCnn.Open();
                adapter.Fill(ds);
                adapter.Dispose();
                sqlCmd.Dispose();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //MessageBox.Show("Can not open connection ! ");
            }
            finally
            {
                sqlCnn.Close();
            }
            
            return ds;

        }

        
       
       // internal static void InsertUserDefinedRuleInDB(string AssetName, string PackageName, int CompressionLevel, string uuid)
        internal static void InsertUserDefinedRuleInDB(string AssetName, string PackageName, string uuid)
        {
//            string sql = @"insert into T_CA_RulesUserDefined (RuleID,RuleName,CompressLevelID,PackageName)
//                            values (@RuleID,@RuleName,@CompressLevelID,@PackageName)";
            string sql = @"insert into T_CA_RulesUserDefined (RuleID,RuleName,PackageName)
                            values (@RuleID,@RuleName,@PackageName)";
           
            SqlCommand sqlCmd = new SqlCommand(sql, sqlCnn);
            sqlCmd.Parameters.AddWithValue("@RuleID", uuid);
            sqlCmd.Parameters.AddWithValue("@RuleName", AssetName);
            //sqlCmd.Parameters.AddWithValue("@CompressLevelID", CompressionLevel);
            sqlCmd.Parameters.AddWithValue("@PackageName", PackageName);
            try
            {
                sqlCnn.Open();
                int result = sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //MessageBox.Show("Can not open connection ! ");
            }
            finally {
                sqlCnn.Close();
            }
          
            
        }
        internal static int SaveNotificationSettings(DataTable dtNotification)
        {
            int rowAffected = 0;
            try
            {
               

                String procedureName = "P_CA_SaveNotifySettings";
                //DataTable dtList = dsRule.Tables[0];

                Database db = DatabaseFactory.CreateDatabase();
                //StringBuilder sbTemp = new StringBuilder();
                foreach (DataRow dr in dtNotification.Rows)
                {
                    object[] parameters = new object[12];
                    parameters[0] = dr["Uuid"];
                    parameters[1] = dr["RuleName"];
                    parameters[2] = dr["RuleId"];
                    parameters[3] = dr["PackageName"];
                    parameters[4] = dr["PopUpEnabled"];
                    parameters[5] = dr["EmailEnabled"];
                    parameters[6] = dr["EmailToList"];
                    parameters[7] = Convert.ToInt32(dr["LimitFrequencyMinutes"]);
                    parameters[8] = Convert.ToInt32(dr["WarningFrequencyMinutes"]);
                    parameters[9] = dr["ManualTradeEnabled"];
                    parameters[10] = dr["SoundEnabled"];
                    parameters[11] = dr["SoundFilePath"];
                    rowAffected += db.ExecuteNonQuery(procedureName, parameters);

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowAffected;
        }

        internal static DataSet GetNotificationSettings()
        {
            try
            {
                String procedureName = "P_CA_GetNotifySettings";
                Database db = DatabaseFactory.CreateDatabase();
                return db.ExecuteDataSet(procedureName);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal static void DeleteUserDefinedRuleFromDB(String ruleId)
        {
            string sql = @"delete from T_CA_RulesUserDefined where RuleId= '" + ruleId + "'";

            SqlCommand sqlCmd = new SqlCommand(sql, sqlCnn);
            try
            {
                sqlCnn.Open();
                int result = sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //MessageBox.Show("Can not open connection ! ");
            }
            finally
            {
                sqlCnn.Close();
            }
        }

        internal static DataSet GetComplianceAlertHist(DateTime from,DateTime to)
        {
            try
            {
                String procedureName = "P_CA_GetAlertHistory";
                Database db = DatabaseFactory.CreateDatabase();
                return db.ExecuteDataSet(procedureName,new object[]{from,to});
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }


        internal static int UpdateNotificationSettings(DataTable _dtTemp)
        {
            int rowAffected = 0;
            try
            {


                String procedureName = "P_CA_UpdateNotifySettings";
                //DataTable dtList = dsRule.Tables[0];

                Database db = DatabaseFactory.CreateDatabase();
                //StringBuilder sbTemp = new StringBuilder();
                foreach (DataRow dr in _dtTemp.Rows)
                {
                    object[] parameters = new object[12];
                    parameters[0] = dr["Uuid"];
                    parameters[1] = dr["RuleName"];
                    parameters[2] = dr["RuleId"];
                    parameters[3] = dr["PackageName"];
                    parameters[4] = dr["PopUpEnabled"];
                    parameters[5] = dr["EmailEnabled"];
                    parameters[6] = dr["EmailToList"];
                    parameters[7] = Convert.ToInt32(dr["LimitFrequencyMinutes"]);
                    parameters[8] = Convert.ToInt32(dr["WarningFrequencyMinutes"]);
                    parameters[9] = dr["ManualTradeEnabled"];
                    parameters[10] = dr["SoundEnabled"];
                    parameters[11] = dr["SoundFilePath"];
                    rowAffected += db.ExecuteNonQuery(procedureName, parameters);

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowAffected;
        }

        internal static DataSet GetNotificationSettings(string ruleId)
        {
            try
            {
                String procedureName = "P_CA_GetNotification";
                Database db = DatabaseFactory.CreateDatabase();
                return db.ExecuteDataSet(procedureName, new object[] { ruleId });

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal static DataSet GetCompliancePreferences(int companyId)
        {
            try
            {
                String procedureName = "P_CA_GetCompliancePreferences";
                Database db = DatabaseFactory.CreateDatabase();
                return db.ExecuteDataSet(procedureName, new object[] { companyId });
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
