using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;

using System.Text;

using System.Windows.Forms;
using System.Xml;
using System.Data;
using Prana.RuleEngine.BusinessObjects;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.RuleEngine.ImportExport
{
    public delegate void ImportCompleteHandler(String packageName, String ruleName, NotificationSettings notification, String category,String ruleId,int remainingImport);
    public delegate void ExportCompleteHandler(String message,bool isSuccessful);




    internal class ImportExportManager
    {

        //TODO: Make this class singleton as it has cache of handler objects categorywise
        
        public event ImportCompleteHandler ImportComplete;
        public event ExportCompleteHandler ExportComplete;

        private static Object _singletonLockerObject = new object();
        private static ImportExportManager _singletonInstance;

        public static ImportExportManager GetInstance()
        {
            lock (_singletonLockerObject)
            {
                if (_singletonInstance == null)
                    _singletonInstance = new ImportExportManager();
                return _singletonInstance; 
            }
        }


        Object _lockerObject = new object();
         Dictionary<RuleCategory, IImportHelper> _importExportHelperCategoryWise = new Dictionary<RuleCategory, IImportHelper>();
         Dictionary<String, NotificationSettings> _ruleNotification = new Dictionary<String, NotificationSettings>();
        Dictionary<string, ExportDefinition> _exportDefinitionCache = new Dictionary<string, ExportDefinition>();
        Dictionary<String, ImportDefinition> _importDefinitionCache = new Dictionary<string, ImportDefinition>();

        /// <summary>
        /// 
        /// </summary>
        private ImportExportManager()
        {
            try
            {
                lock (_lockerObject)
                {

                    IImportHelper customRuleHelper = new CustomRuleExportImport();
                    customRuleHelper.ImportExportActionReceived += ImportExportActionReceived;
                    _importExportHelperCategoryWise.Add(RuleCategory.CustomRule, customRuleHelper);
                    _importExportHelperCategoryWise[RuleCategory.CustomRule].IntialiseAmqpPlugins();

                    IImportHelper userDefinedRuleHelper = new UserDefinedExportImport();
                    userDefinedRuleHelper.ImportExportActionReceived += ImportExportActionReceived;
                    _importExportHelperCategoryWise.Add(RuleCategory.UserDefined, userDefinedRuleHelper);
                    _importExportHelperCategoryWise[RuleCategory.UserDefined].IntialiseAmqpPlugins();
                }
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
        /// <summary>
        /// Import Export Action recieved from esper for custom rule and rule engine for user defined rules.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ruleCategory"></param>
        private void ImportExportActionReceived(DataSet ds, RuleCategory ruleCategory)
        {
            try
            {
                String key = ds.Tables[0].Rows[0]["ResponseType"].ToString();
                switch (key)
                {
                    case "UserDefinedExportComplete":
                    case "CustomRuleExportComplete":
                        ExportRuleDef(ds);
                        break;
                    case "UserDefinedImportComplete":
                    case "CustomRuleImportComplete":
                        ImportRuleDef(ds);
                        break;
                }
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
            //ExportRuleDef(ds);
        }
        /// <summary>
        /// Importing rule notification for the imported rule. 
        /// </summary>
        /// <param name="ds"></param>
        private void ImportRuleDef(DataSet ds)
        {
            try
            {
                String packageName = ds.Tables[0].Rows[0]["PackageName"].ToString();
                String oldRuleName = ds.Tables[0].Rows[0]["OldRuleName"].ToString();
                String newRuleName = ds.Tables[0].Rows[0]["NewRuleName"].ToString();
                String category = ds.Tables[0].Rows[0]["RuleCategory"].ToString();
                String ruleId = String.Empty;
                if (ds.Tables[0].Columns.Contains("RuleId"))
                    ruleId = ds.Tables[0].Rows[0]["RuleId"].ToString();
                NotificationSettings notification = new NotificationSettings();
                if (_importDefinitionCache.ContainsKey(packageName + "_" + newRuleName))
                {
                    notification = _importDefinitionCache[packageName + "_" + newRuleName].Notification;
                    _importDefinitionCache.Remove(packageName + "_" + newRuleName);
                }

                // NotificationSettings notification = _ruleNotification[packageName + "_" + oldRuleName];
                //if (_importDefinitionCache.Count == 0)
               // {
                if (ImportComplete != null)
                        ImportComplete(packageName, newRuleName, notification, category, ruleId, _importDefinitionCache.Count);
               // }
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



        private void Import(ImportDefinition importDefinition)
        {
            UpdateNotificationCache(importDefinition.PackageName + "_" + importDefinition.NewRuleName, importDefinition.Notification);
        }



        /// <summary>
        /// Recieving import request from Rule Engine UI
        /// Allow or block cross import of user Defined rules on the basis of "prePostCrossImportAllowed"
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="ruleCategory"></param>
        /// <param name="filePath"></param>
        /// <param name="metaData"></param>
        /// <param name="prePostCrossImportAllowed"></param>
        private void Import(String packageName, String ruleCategory, String filePath, DataTable metaData, Boolean prePostCrossImportAllowed)
        {

            try
            {
                lock (_lockerObject)
                {
                    String category = metaData.Rows[0]["RuleCategory"].ToString();
                    String rulePackageName = metaData.Rows[0]["PackageName"].ToString();
                    String oldRuleName = metaData.Rows[0]["RuleName"].ToString();
                    String newRuleName = metaData.Rows[0]["NewRuleName"].ToString();

                    NotificationSettings notification = new NotificationSettings(metaData.Rows[0]);
                    UpdateNotificationCache(packageName + "_" + newRuleName, notification);
                    if (category == ruleCategory)
                    {
                        if (ruleCategory.Equals(Constants.USER_DEFINED_RULES_TAG) && (prePostCrossImportAllowed || (!prePostCrossImportAllowed && rulePackageName == packageName)))
                            _importExportHelperCategoryWise[RuleCategory.UserDefined].ImportRule(GetImportRuleDict(packageName, filePath, oldRuleName, newRuleName, category));
                       
                        else if (ruleCategory.Equals(Constants.CUSTOM_RULE_NODE_TAG) && rulePackageName == packageName)
                            _importExportHelperCategoryWise[RuleCategory.CustomRule].ImportRule(GetImportRuleDict(packageName, filePath, oldRuleName, newRuleName, category));
                        
                        else
                            MessageBox.Show("Select valid rule package", "Nirvana - Rule Import");
                    }
                    else
                        MessageBox.Show("Not valid rule category.", "Nirvana - Rule Import");


                }
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

        /// <summary>
        /// Generating Import request Dictionary.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="filePath"></param>
        /// <param name="oldRuleName"></param>
        /// <param name="newRuleName"></param>
        /// <param name="ruleCategory"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetImportRuleDict(string packageName, string filePath, string oldRuleName, string newRuleName, string ruleCategory)
        {
            try
            {
                Dictionary<String, String> requestDict = new Dictionary<string, string>();

                requestDict.Add("PackageName", packageName);
                requestDict.Add("DirectoryPath", filePath);
                requestDict.Add("OldRuleName", oldRuleName);
                requestDict.Add("NewRuleName", newRuleName);
                requestDict.Add("RuleCategory", ruleCategory);
                //requestDict.Add("RuleId", ruleId);
                return requestDict;
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
        /// <summary>
        /// recieving Export rule request from rule engine win.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="ruleCategory"></param>
        /// <param name="ruleName"></param>
        /// <param name="directoryPath"></param>
        /// <param name="ruleId"></param>
        /// <param name="notification"></param>
        //internal void Export(String packageName, String ruleCategory, String ruleName, String directoryPath, String ruleId, NotificationSettings notification)
        //{

        //    try
        //    {
        //        lock (_lockerObject)
        //        {
        //            if (ruleCategory.Equals(Constants.USER_DEFINED_RULES_TAG))
        //            {
        //                _importExportHelperCategoryWise[RuleCategory.UserDefined].ExportRule(GetExportRuleDict(packageName, directoryPath, ruleName, ruleCategory, ruleId));
        //            }
        //            else if (ruleCategory.Equals(Constants.CUSTOM_RULE_NODE_TAG))
        //            {
        //                _importExportHelperCategoryWise[RuleCategory.CustomRule].ExportRule(GetExportRuleDict(packageName, directoryPath, ruleName, ruleCategory, ruleId));
        //            }
        //            else
        //                MessageBox.Show("Select valid rule category");
        //            UpdateNotificationCache(packageName + "_" + ruleName, notification);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
            
        /// <summary>
        /// Generating rule export dictionary.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="directoryPath"></param>
        /// <param name="ruleName"></param>
        /// <param name="ruleCategory"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        private Dictionary<String, String> GetExportRuleDict(string packageName, string directoryPath, string ruleName, string ruleCategory, string ruleId)
        {
            try
            {
                Dictionary<String, String> requestDict = new Dictionary<string, string>();

                requestDict.Add("PackageName", packageName);
                requestDict.Add("DirectoryPath", directoryPath);
                requestDict.Add("RuleName", ruleName);
                requestDict.Add("RuleCategory", ruleCategory);
                requestDict.Add("RuleId", ruleId);
                return requestDict;
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

        /// <summary>
        /// Updating notification for rule to be imported and exported.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="notification"></param>
        private void UpdateNotificationCache(String key,NotificationSettings notification)
        {
            try
            {
                if (_importDefinitionCache.ContainsKey(key))
                {
                    _importDefinitionCache[key].Notification = notification;
                }

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


        /// <summary>
        /// Writing Metadata for the exported rule.
        /// </summary>
        /// <param name="dsReceived"></param>
        private void ExportRuleDef(DataSet dsReceived)
        {
            try
            {
                String rulePath = dsReceived.Tables[0].Rows[0]["FilePath"].ToString();
                String ruleName = dsReceived.Tables[0].Rows[0]["RuleName"].ToString();
                String packageName = dsReceived.Tables[0].Rows[0]["PackageName"].ToString();

                XmlDocument myxml = new XmlDocument();

                XmlElement ruleDef_tag = myxml.CreateElement("RuleDef");

                XmlElement ruleName_tag = myxml.CreateElement("RuleName");
                ruleName_tag.InnerText = dsReceived.Tables[0].Rows[0]["RuleName"].ToString();
                ruleDef_tag.AppendChild(ruleName_tag);

                XmlElement packageName_tag = myxml.CreateElement("PackageName");
                packageName_tag.InnerText = dsReceived.Tables[0].Rows[0]["PackageName"].ToString();
                ruleDef_tag.AppendChild(packageName_tag);


                XmlElement directoryPath_tag = myxml.CreateElement("DirectoryPath");
                directoryPath_tag.InnerText = rulePath;
                ruleDef_tag.AppendChild(directoryPath_tag);


                XmlElement ruleType_tag = myxml.CreateElement("RuleCategory");
                ruleType_tag.InnerText = dsReceived.Tables[0].Rows[0]["RuleCategory"].ToString();
                ruleDef_tag.AppendChild(ruleType_tag);

                XmlElement popUpEnabled_tag = myxml.CreateElement("PopUpEnabled");
                popUpEnabled_tag.InnerText = _exportDefinitionCache[packageName + "_" + ruleName].Notification.PopUpEnabled.ToString();
                ruleDef_tag.AppendChild(popUpEnabled_tag);

                XmlElement emailEnabled_tag = myxml.CreateElement("EmailEnabled");
                emailEnabled_tag.InnerText = _exportDefinitionCache[packageName + "_" + ruleName].Notification.EmailEnabled.ToString();
                ruleDef_tag.AppendChild(emailEnabled_tag);

                XmlElement emailList_tag = myxml.CreateElement("EmailList");
                emailList_tag.InnerText = _exportDefinitionCache[packageName + "_" + ruleName].Notification.EmailList;
                ruleDef_tag.AppendChild(emailList_tag);

                XmlElement limitFrequencyMinutes_tag = myxml.CreateElement("LimitFrequencyMinutes");
                limitFrequencyMinutes_tag.InnerText = _exportDefinitionCache[packageName + "_" + ruleName].Notification.LimitFrequencyMinutes.ToString();
                ruleDef_tag.AppendChild(limitFrequencyMinutes_tag);

                XmlElement manualTradeEnabled_tag = myxml.CreateElement("ManualTradeEnabled");
                manualTradeEnabled_tag.InnerText = _exportDefinitionCache[packageName + "_" + ruleName].Notification.ManualTradeEnabled.ToString();
                ruleDef_tag.AppendChild(manualTradeEnabled_tag);


                myxml.AppendChild(ruleDef_tag);
                myxml.Save(rulePath + "\\MetaData.xml");

                if (_exportDefinitionCache.ContainsKey(packageName + "_" + ruleName))
                    _exportDefinitionCache.Remove(packageName + "_" + ruleName);
                if (_exportDefinitionCache.Count == 0)
                {
                    String directoryPath = dsReceived.Tables[0].Rows[0]["DirectoryPath"].ToString();
                    StringBuilder message = new StringBuilder();
                    message.Append("All selected rules are exported to " + directoryPath);

                    if (ExportComplete != null)
                        ExportComplete(message.ToString(), true);
                }
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

        /// <summary>
        /// Reading rule MetaData.xml for Import.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal DataTable GetImportExportRuleMetadata(string filePath)
        {
            try
            {
                DataTable dtTemp = new DataTable("MetaData");
                if (File.Exists(filePath + "\\MetaData.xml"))
                {
                    XmlDocument myXml = new XmlDocument();
                    myXml.Load(filePath + "\\MetaData.xml");

                    dtTemp.Columns.Add("RuleCategory");
                    dtTemp.Columns.Add("RuleName");
                    dtTemp.Columns.Add("PackageName");
                    dtTemp.Columns.Add("DirectoryPath");
                    dtTemp.Columns.Add("PopUpEnabled");
                    dtTemp.Columns.Add("EmailEnabled");
                    dtTemp.Columns.Add("EmailToList");
                    dtTemp.Columns.Add("LimitFrequencyMinutes");
                    dtTemp.Columns.Add("WarningFrequencyMinutes");
                    dtTemp.Columns.Add("ManualTradeEnabled");
                    dtTemp.Columns.Add("NewRuleName");
                    dtTemp.Columns.Add("SoundEnabled");
                    dtTemp.Columns.Add("SoundFilePath");
                    dtTemp.Rows.Add(new object[] { });
                    dtTemp.Rows[0]["RuleCategory"] = myXml.SelectSingleNode("//RuleCategory").InnerText;
                    dtTemp.Rows[0]["RuleName"] = myXml.SelectSingleNode("//RuleName").InnerText;
                    dtTemp.Rows[0]["PackageName"] = myXml.SelectSingleNode("//PackageName").InnerText;
                    dtTemp.Rows[0]["DirectoryPath"] = myXml.SelectSingleNode("//DirectoryPath").InnerText;
                    dtTemp.Rows[0]["PopUpEnabled"] = myXml.SelectSingleNode("//PopUpEnabled").InnerText;
                    dtTemp.Rows[0]["EmailEnabled"] = myXml.SelectSingleNode("//EmailEnabled").InnerText;
                    dtTemp.Rows[0]["EmailToList"] = myXml.SelectSingleNode("//EmailList").InnerText;
                    dtTemp.Rows[0]["LimitFrequencyMinutes"] = myXml.SelectSingleNode("//LimitFrequencyMinutes").InnerText;
                    dtTemp.Rows[0]["ManualTradeEnabled"] = myXml.SelectSingleNode("//ManualTradeEnabled").InnerText;
                    dtTemp.Rows[0]["WarningFrequencyMinutes"] = 1;
                    dtTemp.Rows[0]["SoundEnabled"] = false;
                    dtTemp.Rows[0]["SoundFilePath"] = String.Empty;
                    return dtTemp;

                }
                else
                {
                    return null;
                }
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

        internal static void DestroySingletonInstance()
        {
            lock (_singletonLockerObject)
            {
                if (_singletonInstance != null)
                {
                    foreach (RuleCategory cat in _singletonInstance._importExportHelperCategoryWise.Keys)
                    {
                        _singletonInstance._importExportHelperCategoryWise[cat].Dispose();
                    }
                    _singletonInstance = null;
                }
            }
        }



        internal void Export(Dictionary<string, ExportDefinition> exportDefCache)
        {
            try
            {
            lock (_lockerObject)
            {
                _exportDefinitionCache = exportDefCache;
                foreach (String key in exportDefCache.Keys)
                {

                    if (exportDefCache[key].RuleCategory.Equals(Constants.USER_DEFINED_RULES_TAG))
                    {
                        _importExportHelperCategoryWise[RuleCategory.UserDefined].ExportRule(GetExportRuleDict(exportDefCache[key].PackageName, exportDefCache[key].DirectoryPath, exportDefCache[key].RuleName, exportDefCache[key].RuleCategory, exportDefCache[key].RuleId));
    }
                    else if (exportDefCache[key].RuleCategory.Equals(Constants.CUSTOM_RULE_NODE_TAG))
                    {
                        _importExportHelperCategoryWise[RuleCategory.CustomRule].ExportRule(GetExportRuleDict(exportDefCache[key].PackageName, exportDefCache[key].DirectoryPath, exportDefCache[key].RuleName, exportDefCache[key].RuleCategory, exportDefCache[key].RuleId));
                    }
                    else
                        MessageBox.Show("Select valid rule category");
                    // UpdateNotificationCache(exportDefCache[key].PackageName + "_" + exportDefCache[key].RuleName, exportDefCache[key].Notification);
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

        internal void Import(Dictionary<string, ImportDefinition> importDefCache)
        {
            try
            {
            _importDefinitionCache = importDefCache;
            foreach (String key in importDefCache.Keys)
            {
                Import(importDefCache[key].PackageName, importDefCache[key].RuleCategory, importDefCache[key].DirectoryPath, importDefCache[key].MetaData, importDefCache[key].PrePostCrossImportAllowed);
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


        internal bool CheckValidImport(ImportDefinition def, String package, String category)
        {
            try
            {
            if (def.RuleCategory == category)
            {
                if (def.PackageName == package)
                {
                    return true;
                }
                else
                {
                    if (def.RuleCategory == Constants.USER_DEFINED_RULES_TAG && def.PrePostCrossImportAllowed)
                        return true;
                    else
                        return false;

                }
            }
            else return false;
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
                return false;
            }
        }
    }

    #region ImportExport Definition classes

    public abstract class ImportExportDefinitionBase
    {
        private String _packageName;

        public String PackageName
        {
            get { return _packageName; }
            set { _packageName = value; }
        }
        private String _ruleCategory;

        public String RuleCategory
        {
            get { return _ruleCategory; }
            set { _ruleCategory = value; }
        }
        private String _ruleName;

        public String RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }
        private String _directoryPath;

        public String DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; }
        }
        
        private NotificationSettings _notification;

        public NotificationSettings Notification
        {
            get { return _notification; }
            set { _notification = value; }
        }
    }

    public class ExportDefinition : ImportExportDefinitionBase
    {
        private String _ruleId;

        public String RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }
    }

    public class ImportDefinition : ImportExportDefinitionBase
    {
        private String _newRuleName;

        public String NewRuleName
        {
            get { return _newRuleName; }
            set { _newRuleName = value; }
        }
        DataTable _metaData;

        public DataTable MetaData
        {
            get { return _metaData; }
            set { _metaData = value; }
    }
        Boolean _prePostCrossImportAllowed;

        public Boolean PrePostCrossImportAllowed
        {
            get { return _prePostCrossImportAllowed; }
            set { _prePostCrossImportAllowed = value; }
        }
    }

    #endregion


}
