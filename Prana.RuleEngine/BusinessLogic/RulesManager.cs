using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Prana.RuleEngine.BussinessObjects;
using Newtonsoft.Json;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.RuleEngine.Utility;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using Prana.CommonDataCache;
using Prana.AmqpAdapter.Amqp;

namespace Prana.RuleEngine.BusinessLogic
{
    internal class RulesManager
    {

        static String _clientPackageExtension = String.Empty;

        static RulesManager()
        {
            _clientPackageExtension = "_" + ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_Vhost);
        }


        /// <summary>
        /// get unique id of rule from guvnor
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="PackageName"></param>
        /// <returns></returns>
        internal static string GetUUIDOfAssets(string ruleName, string PackageName)
        {
            try
            {
                RulesAsset assets = new RulesAsset();
                assets = GetAssetMetaData(ruleName, PackageName);
                return assets.metadata.uuid;
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
                return String.Empty;
            }
        }

        internal static RulesAsset GetAssetMetaData(string ruleName, string PackageName)
        {
            RulesAsset asset = new RulesAsset();
            try
            {
                String URLString = Constants.SERVER + "/" + Constants.GUVNOR_WARNAME + "/rest/packages/" + PackageName + _clientPackageExtension + "/assets/" + ruleName;
                String webClientData = WebClientAdaptor.GetDataUsingWebClient(URLString, "application/json");
                if (webClientData != "")
                {
                    asset = JsonConvert.DeserializeObject<RulesAsset>(webClientData.ToString());

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
            return asset;
        }
        /// <summary>
        /// Creating rule using wevdav API of guvnor using tamplate source code
        /// </summary>
        /// <param name="PackageName"></param>
        /// <param name="RuleName"></param>
        /// <returns></returns>
        internal static bool CreateRulewithTemplate(String PackageName, string RuleName)
        {
            Boolean isRuleCreated = false;
            try
            {
                String ruleTemplate = @"<rule>
                                      <name>Rule2</name>
                                      <modelVersion>1.0</modelVersion>
                                      <attributes>
                                        <attribute>
                                          <attributeName>enabled</attributeName>
                                          <value>true</value>
                                        </attribute>
                                      </attributes>
                                      <metadataList/>
                                      <lhs />
                                      <rhs>
                                        <assertLogical>
                                          <fieldValues>
                                            <fieldValue>
                                              <field>Summary</field>
                                                <value>Enter Summary</value>
                                              <nature>1</nature>
                                              <type>String</type>
                                            </fieldValue>
                                            <fieldValue>
                                              <field>Violated</field>
                                              <value>true</value>
                                              <nature>1</nature>
                                              <type>Boolean</type>
                                            </fieldValue>
                                          </fieldValues>
                                          <factType>Alert</factType>
                                          <isBound>false</isBound>
                                        </assertLogical>
                                      </rhs>
                                      <isNegated>false</isNegated>
                                    </rule>";


                String url = "http://localhost:8080/drools-guvnor/org.drools.guvnor.Guvnor/webdav/packages/" + PackageName + _clientPackageExtension + "/" + RuleName + ".brl";

                String response = WebClientAdaptor.UpdateRuleUsingWebClient(url, ruleTemplate);
                if (response.Equals("Created"))
                    isRuleCreated = true;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return isRuleCreated;
        }

        /// <summary>
        /// Geeting rules from web repository using REST Api and add to rules dictionary
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<String, Dictionary<String, RulesAsset>> GetRulesListFromWebRepo()
        {
            Dictionary<String, Dictionary<String, RulesAsset>> rulesDict = new Dictionary<string, Dictionary<string, RulesAsset>>();
            try
            {

                Dictionary<String, RulesAsset> preTradeRules = GetRulesForPackage(Constants.PRE_TRADE_COMPLIANCE);
                if (preTradeRules != null)
                    rulesDict.Add(Constants.PRE_TRADE_COMPLIANCE, preTradeRules);

                Dictionary<String, RulesAsset> postTradeRules = GetRulesForPackage(Constants.POST_TRADE_COMPLIANCE);
                if (postTradeRules != null)
                    rulesDict.Add(Constants.POST_TRADE_COMPLIANCE, postTradeRules);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Please check for Rule Server started!!", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            return rulesDict;

        }
        /// <summary>
        /// Get rules of package
        /// </summary>
        /// <param name="PackageName"> String PackageName</param>
        /// <returns>List of Rules</returns>
        internal static Dictionary<String, RulesAsset> GetRulesForPackage(String PackageName)
        {
            Dictionary<String, RulesAsset> rulesDict = new Dictionary<String, RulesAsset>();
            List<RulesAsset> rules = new List<RulesAsset>();
            try
            {
                String URLString = Constants.SERVER + "/" + Constants.GUVNOR_WARNAME + "/rest/packages/" + PackageName + _clientPackageExtension + "/assets";
                String webClientData = WebClientAdaptor.GetDataUsingWebClient(URLString, "application/json");
                rules = JsonConvert.DeserializeObject<List<RulesAsset>>(webClientData.ToString());

                foreach (RulesAsset rule in rules)
                {

                    if (!rulesDict.ContainsKey(rule.title) && rule.metadata.format.Equals(Constants.RULE_FILE_FORMATE))
                        rulesDict.Add(rule.title, rule);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
            return rulesDict;

        }



        internal static String EnableDisableUserDefnedRule(String PackageName, String RuleName, bool isDisabled)
        {
            String response = string.Empty;
            try
            {
                String urlSource = Constants.GUVNOR_REST_BASE_URL + "/packages/" + PackageName + _clientPackageExtension + "/assets/" + RuleName + "/source";

                String webClientData = WebClientAdaptor.GetDataUsingWebClient(urlSource, "text/plain");
                if (webClientData != "")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Prana.RuleEngine.BusinessObjects.Rule));

                    DataSet ds = new DataSet();
                    StringReader reader = new StringReader(webClientData);
                    ds.ReadXml(reader);
                    DataTable dt;
                    if (ds.Tables.Contains("attribute") && ds.Tables["attribute"].Select("attributeName = 'enabled'").Length > 0)
                        dt = ds.Tables["attribute"];
                    else
                        dt = GetDefaultAttributeTable();

                    if (dt.Columns.Contains("attributeName") && dt.Columns.Contains("value"))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["attributeName"].Equals("enabled"))
                            {
                                if (isDisabled)
                                    row["value"] = "false";
                                else
                                    row["value"] = "true";

                                break;

                            }
                        }




                        StringWriter sw = new StringWriter();
                        ds.WriteXml(sw);
                        string result = sw.ToString();

                        response = WebClientAdaptor.UpdateRuleUsingWebClient(urlSource, result);
                    }
                }

                String url = Constants.GUVNOR_REST_BASE_URL + "/packages/" + PackageName + _clientPackageExtension + "/assets/" + RuleName + "/isdisabled/" + isDisabled;
                response = WebClientAdaptor.GetDataUsingWebClient(url, "text/plain");


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
            return response;
        }

        private static DataTable GetDefaultAttributeTable()
        {
            DataTable dt = new DataTable("attribute");
            dt.Columns.Add("attributeName");
            dt.Columns.Add("value");
            dt.Rows.Add(new Object[] { "enabled", "true" });
            return dt;
        }

        internal static Boolean CheckPermissionsOnPackage(String packageName, int UserID)
        {
            bool canAccess = false;

            try
            {
                if (packageName.Equals("PreTradeCompliance"))
                {
                    canAccess = Convert.ToBoolean(ComplianceCacheManager.GetPretradeModulePermission(UserID));
                }
                else
                {
                    canAccess = Convert.ToBoolean(ComplianceCacheManager.GetPostTradeModulePermission(UserID));
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
            return canAccess;
        }

        internal static Boolean DeleteUserDefinedRule(String ruleName, String packageName)
        {
            Boolean isDeleted = false;
            try
            {

                // string uuid = _selectedTreeNode.Name;
                //_notificationCntrl.DeleteNotificationSettings(packageName, ruleName, uuid);
                String deleteRuleUrl = Constants.SERVER + "/" + Constants.GUVNOR_WARNAME + "/rest/packages/" + packageName + _clientPackageExtension + "/assets/" + ruleName;
                isDeleted = WebClientAdaptor.DeleteUsingWebClient(deleteRuleUrl);


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
            }
            return isDeleted;
        }

        internal static void RenameRule(string newRule, string oldRule, string _selectedPackageName)
        {
            try
            {
                String outputObj = "Rename," + _selectedPackageName + _clientPackageExtension + ":" + oldRule + ".brl" + ":" + newRule + ".brl";
                AmqpHelper.SendObject(outputObj, "Build", null);
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
            }
        }
    }
}
