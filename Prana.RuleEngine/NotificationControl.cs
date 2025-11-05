using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.RuleEngine.DataAccessControl;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.AmqpAdapter.Amqp;
using Prana.RuleEngine.BusinessObjects;
using Prana.RuleEngine.BussinessObjects;

namespace Prana.RuleEngine
{
    public partial class NotificationControl : System.Windows.Forms.UserControl
    {
        DataSet _notificationSettingsDS = new DataSet();
        DataTable _dtTemp = new DataTable();
        int _lastRuleId;
        public NotificationControl()
        {
            InitializeComponent();
            InitializeNotifyFreqCombo();
            GetRulesNotificationSettings();
            InitializeTemDataTable();
        }

        private void InitializeTemDataTable()
        {
            try
            {
            _dtTemp.Columns.Add("Uuid");
            _dtTemp.Columns.Add("RuleName");
            _dtTemp.Columns.Add("RuleId");
            _dtTemp.Columns.Add("PackageName");
                _dtTemp.Columns.Add("PopUpEnabled");
            _dtTemp.Columns.Add("EmailEnabled");
            _dtTemp.Columns.Add("EmailToList");
            _dtTemp.Columns.Add("LimitFrequencyMinutes");
            _dtTemp.Columns.Add("WarningFrequencyMinutes");
            _dtTemp.Columns.Add("ManualTradeEnabled");
            _dtTemp.Columns.Add("SoundEnabled");
            _dtTemp.Columns.Add("SoundFilePath");
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

        private void GetRulesNotificationSettings()
        {
            try
            {
            _notificationSettingsDS = RulesDAO.GetNotificationSettings();
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

        private void InitializeNotifyFreqCombo()
        {
            try
            {
                DataSet frequenciesDS = RulesDAO.GetNotificationFrequencyList();
                comboNotifyFreq.DataSource = frequenciesDS.Tables[0];
                comboNotifyFreq.ValueMember = "ID";
                comboNotifyFreq.DisplayMember = "MeasurementDescription";
                comboNotifyFreq.SelectedIndex = 0;

                //if (comboCompressionLevel.Items.Count > 0)
                //    comboCompressionLevel.SelectedIndex = 0;
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

        internal void SetNotificationSettings(string ruleName,string packageName)
        {
             string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName+"'";
           // string filter = "RuleId ='" + ruleId + "'";
            //string filter = "Uuid='" + ruleId + "'";
            DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
            try
            {
                _dtTemp.Rows.Clear();
                //if (rule.Length < 1)
                //    return;
                //if (rule.Length < 1)
                //{
                //    String ruleId = "R" + DateTime.Now.ToString("yyyyddMMHHmmssfff");
                //    CreateNotificationSettings(packageName, ruleName, "", ruleId);
                //}
                if (rule.Length > 0)
                {
                    _dtTemp.Rows.Add(new Object[] { rule[0]["Uuid"],
                                                rule[0]["RuleName"],
                                                rule[0]["RuleId"],
                                                rule[0]["PackageName"],
                                                 rule[0]["PopUpEnabled"],
                                                 rule[0]["EmailEnabled"],
                                                rule[0]["EmailToList"],
                                                rule[0]["LimitFrequencyMinutes"],
                                                rule[0]["WarningFrequencyMinutes"],
                                                rule[0]["ManualTradeEnabled"],
                                               rule[0]["SoundEnabled"],
                                                rule[0]["SoundFilePath"]
                                                });


                    chckbxEmail.DataBindings.Clear();
                    chckbxEmail.DataBindings.Add("Checked", _dtTemp, "EmailEnabled");
                    chckbxPopUp.DataBindings.Clear();
                    chckbxPopUp.DataBindings.Add("Checked", _dtTemp, "PopUpEnabled");
                    chckBxSound.DataBindings.Clear();
                    chckBxSound.DataBindings.Add("Checked", _dtTemp, "SoundEnabled");
                    chckBxManualTrade.DataBindings.Clear();
                    chckBxManualTrade.DataBindings.Add("Checked", _dtTemp, "ManualTradeEnabled");
                    txtBxEmails.DataBindings.Clear();
                    txtBxEmails.DataBindings.Add("text", _dtTemp, "EmailToList");
                    comboNotifyFreq.DataBindings.Clear();
                    comboNotifyFreq.DataBindings.Add("SelectedValue", _dtTemp, "LimitFrequencyMinutes");
                    //// chckbxEmail.DataBindings.Add("Checked", _dtTemp, "WarningFrequencyMinutes");

                    // comboNotifyFreq.DataBindings.Clear();
                    //comboNotifyFreq.DataBindings.Add("Checked", _dtTemp, "SoundFilePath");

                    
                }
                // chckbxEmail.DataBindings = frequenciesDS.Tables[0].Columns[""];
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


        internal void UpdateNotificationSettings()
        {
            try
            {
                if (_dtTemp.Rows.Count > 0)
                {
                    RulesDAO.UpdateNotificationSettings(_dtTemp);
                    String ruleId = _dtTemp.Rows[0]["RuleId"].ToString();
                    //String ruleName = _dtTemp.Rows[0]["RuleName"].ToString();
                    //String packageName = _dtTemp.Rows[0]["PackageName"].ToString();
                    //string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName + "'";
                    string filter = "RuleId ='" + ruleId + "'" ;
                    DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
                    if (rule.Length > 0)
                    {
                        rule[0].Delete();
                    }
                    _notificationSettingsDS.Tables[0].Rows.Add(_dtTemp.Rows[0].ItemArray);
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

               

        internal void CreateNotificationSettings(string packageName, string ruleName,string uuid,string ruleId)
        {
            try
            {
                
                //_dtTemp.Rows.Clear();
                //if(packageName=Constants.PRE_TRADE_COMPLIANCE)
                bool emailEnabled = chckbxEmail.Checked;
                bool popUpEnabled = chckbxPopUp.Checked;
                string emailList = txtBxEmails.Text;
                int frequencyId = 1;
                if (packageName.Equals(Constants.POST_TRADE_COMPLIANCE) && comboNotifyFreq.SelectedIndex >= 0)
                    frequencyId = comboNotifyFreq.SelectedIndex + 1;
                
                _dtTemp.Rows.Add(new Object[] { uuid, ruleName, ruleId, packageName, popUpEnabled, emailEnabled, emailList, frequencyId, 1, false, false, "" });
                RulesDAO.SaveNotificationSettings(_dtTemp);
                _notificationSettingsDS.Tables[0].Rows.Add(new Object[] { uuid, ruleName, ruleId, packageName, popUpEnabled, emailEnabled, emailList, frequencyId, 1, false, false, "" });
                SetNotificationSettings(ruleName, packageName);
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

        internal void DeleteNotificationSettings(string packageName, string ruleName)
        {
            try
            {
               //String uuid = _dtTemp.Rows[0]["Uuid"].ToString();
                if (_dtTemp.Rows.Count > 0)
                {
                    String ruleId = _dtTemp.Rows[0]["RuleId"].ToString();
                    RulesDAO.DeleteUserDefinedRuleFromDB(ruleId);

                    string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName + "'";
                    //string filter = "Uuid ='" + uuid + "'";
                    if (_notificationSettingsDS.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
                        if (rule.Length > 0)
                        {
                            rule[0].Delete();
                        }
                    }
                    Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                    savedMessage.Add("ApplicationStatus", "Saved");
                    AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
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

        internal bool isNotificationExists(string packageName, string ruleName)
        {
            bool isExists = false;
            try
            {
                //String uuid = _dtTemp.Rows[0]["Uuid"].ToString();
                string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName + "'";
               // string filter = "Uuid ='" + uuid + "'";
                if (_notificationSettingsDS.Tables[0].Rows.Count > 0)
                {
                    DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
                    if (rule.Length > 0)
                    {
                        isExists = true;
                    }
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
            return isExists;
        }

        internal void ToggleFrequencyVisibility(bool isFrequencyEnabled)
        {
            try
            {
            if (isFrequencyEnabled)
            {
                grpBoxFrequency.Enabled = true;
            }
            else
            {
                grpBoxFrequency.Enabled = false;
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


        internal void UpdateNotificationSettings(string newRuleNameText, string uuid)
        {
            try
            {
                if (_dtTemp.Rows.Count > 0)
                {
                    _dtTemp.Rows[0]["RuleName"] = newRuleNameText;
                    _dtTemp.Rows[0]["Uuid"] = uuid;
                    RulesDAO.UpdateNotificationSettings(_dtTemp);
                    String ruleId = _dtTemp.Rows[0]["RuleId"].ToString();
                    //String ruleName = _dtTemp.Rows[0]["RuleName"].ToString();
                    //String packageName = _dtTemp.Rows[0]["PackageName"].ToString();
                    //string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName + "'";
                    string filter = "RuleId ='" + ruleId + "'";
                    DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
                    if (rule.Length > 0)
                    {
                        rule[0].Delete();
                    }
                    _notificationSettingsDS.Tables[0].Rows.Add(_dtTemp.Rows[0].ItemArray);
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

        internal String GetRuleId()
        {
            String ruleID = "";
            try
            {
                String ruleId = _dtTemp.Rows[0]["RuleId"].ToString();
                string filter = "RuleId ='" + ruleId + "'";
                DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);

                if (rule.Length > 0)
                {
                    ruleID = rule[0]["RuleId"].ToString();
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
            return ruleID;
        }

        internal void UpdateNotificationDS(string ruleId)
        {
            try
            {

            string filter = "RuleId ='" + ruleId + "'";
            DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
            if (rule.Length > 0)
            {
                rule[0].Delete();
            }
            DataSet dsTemp = RulesDAO.GetNotificationSettings(ruleId);
            if (dsTemp != null)
                _notificationSettingsDS.Tables[0].Rows.Add(dsTemp.Tables[0].Rows[0].ItemArray);
            
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
        

        internal NotificationSettings GetNotificationSetting(string packageName, string ruleName)
        {
            NotificationSettings notification = new NotificationSettings();
            try
            {
                string filter = "RuleName ='" + ruleName + "' AND PackageName= '" + packageName + "'";
               
                if (isNotificationExists(packageName, ruleName))
                {
                    DataRow[] rule = _notificationSettingsDS.Tables[0].Select(filter);
                    if (rule.Length > 0)
                    {
                        notification = new NotificationSettings(rule[0]);
                    }
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

            return notification;

        }

        internal void SaveNotificationSettings(string packageName, string ruleName, NotificationSettings notification, string uuid, string ruleId)
        {
            try
            {
                _dtTemp.Rows.Clear();
                _dtTemp.Rows.Add(new Object[] { uuid, ruleName, ruleId, packageName, notification.PopUpEnabled, notification.EmailEnabled, notification.EmailList, notification.LimitFrequencyMinutes, 1, false, false, "" });
                RulesDAO.SaveNotificationSettings(_dtTemp);
                _notificationSettingsDS.Tables[0].Rows.Add(new Object[] { uuid, ruleName, ruleId, packageName, notification.PopUpEnabled, notification.EmailEnabled, notification.EmailList, notification.LimitFrequencyMinutes, 1, false, false, "" });

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

        internal void SynchronizeCustomRules(string package, Dictionary<string, CustomRule> customRulesDict)
        {
            try
            {
                Boolean isExists = false;
                NotificationSettings notification = new NotificationSettings();
                foreach (String rule in customRulesDict.Keys)
                {
                    isExists = isNotificationExists(package, customRulesDict[rule].Name);
                    if (!isExists)
                        SaveNotificationSettings(package, customRulesDict[rule].Name, notification, customRulesDict[rule].RuleId, customRulesDict[rule].RuleId);
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

        internal void SynchronizeUserDefinedRules(string package, Dictionary<string, RulesAsset> userDefinedRulesDict)
        {
            try
            {
                Boolean isExists = false;
                NotificationSettings notification = new NotificationSettings();
                foreach (String rule in userDefinedRulesDict.Keys)
                {
                    isExists = isNotificationExists(package, rule);
                    if (!isExists)
                        SaveNotificationSettings(package, rule, notification, userDefinedRulesDict[rule].metadata.uuid, userDefinedRulesDict[rule].metadata.uuid);
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
    }
}
