using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.AmqpAdapter;
using System.Data.SqlClient;
using System.Configuration;
using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;

namespace Prana.RuleEngine.DataAccessControl
{
    internal class SixteenBDataHelper
    {
        static String connetionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString;
        static SqlConnection sqlCnn = new SqlConnection(connetionString);
        static Dictionary<String, List<DateTime>> sixteenBRuleList = new Dictionary<string, List<DateTime>>();
        //static String _hostName;
        static String _ruleSaveQueueName;
        //static AmqpHelper _helper;

        static SixteenBDataHelper()
        {
            //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings,ConfigurationHelper.CONFIGKEY_AmqpServer);
            _ruleSaveQueueName = ConfigurationHelper.Instance.GetAppSettingValueByKey("RuleSixteenBExchangeName");
            AmqpHelper.InitializeSender("SixteenB",  _ruleSaveQueueName, MediaType.Exchange_Fanout);
            //_helper = AmqpHelper.ForExchange(_hostName, _ruleSaveQueueName);//new AmqpHelper(ConfigurationHelper.Instance.GetAppSettingValueByKey("AmqpServer"));
        }

        /// <summary>
        /// Save SixteenB rule to database and send ExPnL for apply
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        internal static void SaveAndSendSymbolToExchange(string symbol, DateTime date,String userId)
        {
            try
            {
                if (!ValidateRule(symbol, date, userId))
                {
                    MessageBox.Show("Rule not validated.");
                }

                else
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RuleId");
                    dt.Columns.Add("Symbol");
                    dt.Columns.Add("StartDate");
                    dt.Columns.Add("CreatedBy");
                    dt.Rows.Add(new object[] { Guid.NewGuid(), symbol, date.Date, userId });
                    ds.Tables.Add(dt);
                    int res = SaveSixteenBRules(ds);
                    //AmqpHelper helper = AmqpHelper.ForExchange(_hostName, _ruleSaveQueueName);//new AmqpHelper(ConfigurationHelper.Instance.GetAppSettingValueByKey("AmqpServer"));
                    //helper.SendDataSet(ds);
                    if (res > 0)
                    {
                        MessageBox.Show("Rule saved and applied");
                        AmqpHelper.SendObject(ds, "SixteenB", null);
                        //_helper.SendDataSet(ds);
                    }
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
        }

        /// <summary>
        /// Validate rule, currently for duplicate only
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        private static bool ValidateRule(string symbol, DateTime date, string userId)
        {
            if (sixteenBRuleList.ContainsKey(symbol))
                if (sixteenBRuleList[symbol].Contains(date.Date))
                    return false;

            return true;
        }
        
        /// <summary>
        /// Add 16 B rules to given treeview
        /// </summary>
        internal static void AddSixteenBNodeToTree(ref TreeView treeView1)
        {
            try
            {
                String packageName = "PreTradeCompliance";
                if (treeView1.Nodes[0].Nodes[packageName].Nodes.ContainsKey("16B"))
                    treeView1.Nodes[0].Nodes[packageName].Nodes.RemoveByKey("16B");

                DataSet dsTemp = GetAllSixteenBRules();
                TreeNode nodeSixteenB = new TreeNode("16B");
                nodeSixteenB.Name = "16B";
                nodeSixteenB.Tag = "16B";
                nodeSixteenB.Text = "16B";

                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    String symbol = dr["Symbol"].ToString();
                    if (sixteenBRuleList.ContainsKey(symbol))
                    {
                        //List<DateTime> ruleList = new List<string>();
                        //ruleList.Add(Convert.ToDateTime(dr["StartDate"].ToString()));
                        DateTime dtTemp = Convert.ToDateTime(dr["StartDate"].ToString());
                        if (!sixteenBRuleList[symbol].Contains(dtTemp.Date))
                            sixteenBRuleList[symbol].Add(dtTemp.Date);
                    }
                    else
                    {
                        List<DateTime> ruleList = new List<DateTime>();
                        ruleList.Add(Convert.ToDateTime(dr["StartDate"].ToString()).Date);
                        sixteenBRuleList.Add(symbol, ruleList);
                    }

                }
                foreach (String symbol in sixteenBRuleList.Keys)
                {

                    TreeNode nodeRootTemp = new TreeNode(symbol);
                    nodeRootTemp.Tag = Constants.SIXTEEN_B_SYMBOL;
                    //nodeSixteenB.Nodes[symbol].Nodes = new TreeNodeCollection();
                    foreach (DateTime dt in sixteenBRuleList[symbol])
                    {
                        TreeNode nodeTemp = new TreeNode(dt.Date.ToString());
                        nodeTemp.Tag = Constants.SIXTEEN_B_DATE;
                        nodeRootTemp.Nodes.Add(nodeTemp);
                    }
                    nodeSixteenB.Nodes.Add(nodeRootTemp);
                    //nodeSixteenB.Nodes[symbol].Nodes = nodeCollectionTemp;
                }

                treeView1.Nodes[0].Nodes[packageName].Nodes.Add(nodeSixteenB);

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
                //MessageBox.Show("Problem in getting categories data!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static DataSet GetAllSixteenBRules()
        {
            try
            {
                String procedureName = "P_CA_GetAllSixteenBRules";
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

        internal static int SaveSixteenBRules(DataSet dsRule)
        {
            int rowAffected = 0;
            try
            {
                DataSet resultDataSet = new DataSet();

                String procedureName = "P_CA_SaveSixteenBRules";
                //DataTable dtList = dsRule.Tables[0];

                Database db = DatabaseFactory.CreateDatabase();
                //StringBuilder sbTemp = new StringBuilder();
                foreach (DataRow dr in dsRule.Tables[0].Rows)
                {
                    object[] parameters = new object[4];
                    parameters[0] = dr["RuleId"];
                    parameters[1] = dr["Symbol"];
                    parameters[2] = dr["StartDate"];
                    parameters[3] = dr["CreatedBy"];
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

        internal static bool DeleteSixteenBRule(string Symbol, string dateRule, int userId)
        {

            bool isDeleted = false;
            string sqlQuery="";
            if (dateRule != "")
                sqlQuery = @"delete from T_CA_RuleSixteenB where CalenderStartDate= '" + dateRule + "' AND Symbol= '" + Symbol + "' AND CreatedBy=" + userId;
            else
                sqlQuery = @"delete from T_CA_RuleSixteenB where Symbol= '" + Symbol + "' AND CreatedBy=" + userId;

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCnn); ;
            try
            {
                sqlCnn.Open();
                if (sqlCmd.ExecuteNonQuery() > 0)
                {
                    if (sixteenBRuleList.ContainsKey(Symbol))
                    {
                        if (dateRule == "")
                        {
                            sixteenBRuleList.Remove(Symbol);
                        }
                        else
                        {
                            if (sixteenBRuleList[Symbol].Count == 1)
                            {
                                sixteenBRuleList.Remove(Symbol);
                            }
                            else
                            {
                                List<DateTime> dateRuleList = sixteenBRuleList[Symbol];
                                dateRuleList.Remove(DateTime.Parse(dateRule));
                            }
                        }
                    }
                   // AmqpAdapter.AmqpHelper helper = new AmqpHelper(ConfigurationHelper.Instance.GetAppSettingValueByKey("AmqpServer"));
                   // helper.SendDataSet("", ConfigurationHelper.Instance.GetAppSettingValueByKey("RuleSaveQueueName"));
                    isDeleted = true;
                } 
                
                sqlCnn.Close();
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
            return isDeleted;
        }
    }
}
