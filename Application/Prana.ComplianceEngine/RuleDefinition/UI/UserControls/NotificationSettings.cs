using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public partial class NotificationSettings : UserControl
    {
        /// <summary>
        /// Initializes controls for notification settings.
        /// disables all controls.
        /// </summary>
        public NotificationSettings()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    ultraChckPopUp.Checked = false;
                    ultraChckEmail.Checked = false;
                    ultraCmboFrequency.SelectedIndex = 0;
                    ultraTxtEmailCc.Text = "";
                    ultraTxtEmailTo.Text = "";
                    ultraTextEditorEmailSubject.Text = "";
                    ultraCheckAlertHoliday.Checked = true;
                    ultraCheckAlertMarket.Checked = false;
                }
                chkSendInOneEmail.Visible = false;
                enableScheduleSlotLayout.Visible = false;

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

        Dictionary<String, NotificationSetting> notificationSettingDef = new Dictionary<string, NotificationSetting>();
        String openRuleId = "";
        String openGroupId = "";

        /// <summary>
        /// sets notification settings on UI for rule.
        /// </summary>
        /// <param name="rule"></param>
        internal void SetNotificationSettings(RuleBase rule, bool isAnotherUser)
        {
            try
            {
                lock (notificationSettingDef)
                {
                    if (!String.IsNullOrEmpty(openRuleId) && !isAnotherUser)
                    {
                        notificationSettingDef[openRuleId] = GetCurrentNotificationSettings();
                    }

                    if (rule.Notification != null)
                    {
                        if (!notificationSettingDef.ContainsKey(rule.RuleId))
                        {
                            notificationSettingDef.Add(rule.RuleId, rule.Notification);
                        }
                        openRuleId = rule.RuleId;

                        ultraChckPopUp.Checked = notificationSettingDef[rule.RuleId].PopUpEnabled;
                        ultraChckEmail.Checked = notificationSettingDef[rule.RuleId].EmailEnabled;
                        ultraTxtEmailCc.Text = notificationSettingDef[rule.RuleId].EmailCCList;
                        ultraTxtEmailTo.Text = notificationSettingDef[rule.RuleId].EmailToList;
                        ultraTextEditorEmailSubject.Text = notificationSettingDef[rule.RuleId].EmailSubject;
                        ultraCheckAlertMarket.Checked = notificationSettingDef[rule.RuleId].AlertInTimeRange;
                        ultraCheckAlertHoliday.Checked = notificationSettingDef[rule.RuleId].StopAlertOnHolidays;
                        chkSendInOneEmail.Checked = notificationSettingDef[rule.RuleId].SendInOneEmail;
                        if (notificationSettingDef[rule.RuleId].LimitFrequencyMinutes == 8)
                        {
                            enableScheduleSlotLayout.Visible = true;
                            chkSendInOneEmail.Visible = true;
                            ultraLblStart.Visible = false;
                            ultraLblEnd.Visible = false;
                            ultraTimeStart.Visible = false;
                            ultraTimeEnd.Visible = false;
                        }
                        else if (ultraCheckAlertMarket.Checked)
                        {
                            ultraLblStart.Visible = true;
                            ultraLblEnd.Visible = true;
                            ultraTimeStart.Visible = true;
                            ultraTimeStart.Value = notificationSettingDef[rule.RuleId].StartTime;
                            ultraTimeEnd.Visible = true;
                            ultraTimeEnd.Value = notificationSettingDef[rule.RuleId].EndTime;
                            enableScheduleSlotLayout.Visible = false;
                            chkSendInOneEmail.Visible = false;
                        }
                        else
                        {
                            ultraLblStart.Visible = false;
                            ultraLblEnd.Visible = false;
                            ultraTimeStart.Visible = false;
                            //ultraTimeStart.Value = notificationSettingDef[rule.RuleId].StartTime;
                            ultraTimeEnd.Visible = false;
                            enableScheduleSlotLayout.Visible = false;
                            chkSendInOneEmail.Visible = false;
                            // ultraTimeEnd.Value = notificationSettingDef[rule.RuleId].EndTime;
                        }
                        if (rule.Package == RulePackage.PostTrade)
                        {
                            ultraCmboFrequency.SelectedIndex = notificationSettingDef[rule.RuleId].LimitFrequencyMinutes - 1;
                            DateTime minDateTime = Prana.BusinessObjects.DateTimeConstants.MinValue;
                            if (notificationSettingDef[rule.RuleId].TimeSlots[0].Date != minDateTime.Date)
                                slotOne.Value = notificationSettingDef[rule.RuleId].TimeSlots[0];
                            else
                                slotOne.Value = null;
                            if (notificationSettingDef[rule.RuleId].TimeSlots[1].Date != minDateTime.Date)
                                slotTwo.Value = notificationSettingDef[rule.RuleId].TimeSlots[1];
                            else
                                slotTwo.Value = null;
                            if (notificationSettingDef[rule.RuleId].TimeSlots[2].Date != minDateTime.Date)
                                slotThree.Value = notificationSettingDef[rule.RuleId].TimeSlots[2];
                            else
                                slotThree.Value = null;
                            if (notificationSettingDef[rule.RuleId].TimeSlots[3].Date != minDateTime.Date)
                                slotFour.Value = notificationSettingDef[rule.RuleId].TimeSlots[3];
                            else
                                slotFour.Value = null;
                            if (notificationSettingDef[rule.RuleId].TimeSlots[4].Date != minDateTime.Date)
                                slotFive.Value = notificationSettingDef[rule.RuleId].TimeSlots[4];
                            else
                                slotFive.Value = null;
                        }



                    }
                    else
                    {
                        openRuleId = "";
                    }
                    if (rule.Package == RulePackage.PreTrade)
                    {
                        ultraGrpBoxPref.Enabled = false;
                        ultraGrpBoxPref.Location = new Point(662, 3);
                        ultraGrpBoxEmailSubject.Location = new Point(338, 3);
                        ultraGrpBoxPref.Visible = false;
                        ultraCmboFrequency.SelectedIndex = 0;
                        ultraCmboFrequency.Enabled = false;
                        ultraGrpBoxNotify.Visible = true;
                        ultraGrpBoxEmailSubject.Visible = true;
                    }
                    else if (rule.Package == RulePackage.PostTrade)
                    {
                        ultraGrpBoxPref.Location = new Point(338, 3);
                        ultraGrpBoxEmailSubject.Location = new Point(662, 3);
                        ultraGrpBoxPref.Enabled = true;
                        ultraCmboFrequency.Enabled = true;
                        ultraGrpBoxPref.Visible = true;
                        ultraGrpBoxNotify.Visible = true;
                        ultraGrpBoxEmailSubject.Visible = true;
                    }
                    if (rule.GroupId != "-1")
                    {
                        ultraGrpBoxNotify.Visible = false;
                        ultraGrpBoxPref.Visible = false;
                        ultraGrpBoxEmailSubject.Visible = false;
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


        /// <summary>
        /// returns notification settings from UI.
        /// </summary>
        /// <returns></returns>
        internal NotificationSetting GetCurrentNotificationSettings()
        {
            try
            {
                NotificationSetting newNoti = new NotificationSetting();
                newNoti.EmailCCList = ultraTxtEmailCc.Text;
                newNoti.EmailEnabled = ultraChckEmail.Checked;
                newNoti.EmailToList = ultraTxtEmailTo.Text;
                newNoti.EmailSubject = ultraTextEditorEmailSubject.Text;
                newNoti.LimitFrequencyMinutes = ultraCmboFrequency.SelectedIndex + 1;
                newNoti.PopUpEnabled = ultraChckPopUp.Checked;
                newNoti.AlertInTimeRange = ultraCheckAlertMarket.Checked;
                newNoti.StopAlertOnHolidays = ultraCheckAlertHoliday.Checked;
                newNoti.StartTime = Convert.ToDateTime(ultraTimeStart.Value);
                newNoti.EndTime = Convert.ToDateTime(ultraTimeEnd.Value);
                newNoti.SendInOneEmail = chkSendInOneEmail.Checked;
                newNoti.TimeSlots[0] = (slotOne.Value != null ? Convert.ToDateTime(slotOne.Value) : Prana.BusinessObjects.DateTimeConstants.MinValue);
                newNoti.TimeSlots[1] = (slotTwo.Value != null ? Convert.ToDateTime(slotTwo.Value) : Prana.BusinessObjects.DateTimeConstants.MinValue);
                newNoti.TimeSlots[2] = (slotThree.Value != null ? Convert.ToDateTime(slotThree.Value) : Prana.BusinessObjects.DateTimeConstants.MinValue);
                newNoti.TimeSlots[3] = (slotFour.Value != null ? Convert.ToDateTime(slotFour.Value) : Prana.BusinessObjects.DateTimeConstants.MinValue);
                newNoti.TimeSlots[4] = (slotFive.Value != null ? Convert.ToDateTime(slotFive.Value) : Prana.BusinessObjects.DateTimeConstants.MinValue);
                return newNoti;
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
                return null;
            }
        }

        /// <summary>
        /// Initializes Aler frequency combo box using value from DB.
        /// </summary>
        /// <param name="allNotificationFrequency"></param>
        internal void InitializeNotificationFrequency(Dictionary<int, string> allNotificationFrequency)
        {
            try
            {
                try
                {
                    ultraCmboFrequency.DataSource = new BindingSource(allNotificationFrequency, null);
                    ultraCmboFrequency.ValueMember = "Key";
                    ultraCmboFrequency.DisplayMember = "Value";
                    ultraCmboFrequency.SelectedIndex = 0;
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

        /// <summary>
        /// Overload for getting notification settings from UI for list of rules.
        /// </summary>
        /// <param name="ruleList"></param>
        internal void GetNotificationSetting(ref List<RuleBase> ruleList)
        {
            try
            {
                lock (notificationSettingDef)
                {
                    foreach (RuleBase rule in ruleList)
                    {
                        if (notificationSettingDef.ContainsKey(rule.RuleId))
                        {
                            notificationSettingDef[rule.RuleId].EmailCCList = ultraTxtEmailCc.Text;
                            notificationSettingDef[rule.RuleId].EmailEnabled = ultraChckEmail.Checked;
                            notificationSettingDef[rule.RuleId].EmailToList = ultraTxtEmailTo.Text;
                            notificationSettingDef[rule.RuleId].EmailSubject = ultraTextEditorEmailSubject.Text;
                            notificationSettingDef[rule.RuleId].LimitFrequencyMinutes = ultraCmboFrequency.SelectedIndex + 1;
                            notificationSettingDef[rule.RuleId].PopUpEnabled = ultraChckPopUp.Checked;
                            notificationSettingDef[rule.RuleId].AlertInTimeRange = ultraCheckAlertMarket.Checked;
                            notificationSettingDef[rule.RuleId].StopAlertOnHolidays = ultraCheckAlertHoliday.Checked;
                            notificationSettingDef[rule.RuleId].StartTime = Convert.ToDateTime(ultraTimeStart.Value);
                            notificationSettingDef[rule.RuleId].EndTime = Convert.ToDateTime(ultraTimeEnd.Value);
                            notificationSettingDef[rule.RuleId].SendInOneEmail = chkSendInOneEmail.Checked;
                            notificationSettingDef[rule.RuleId].TimeSlots[0] = (slotOne.Value == null ? Prana.BusinessObjects.DateTimeConstants.MinValue : Convert.ToDateTime(slotOne.Value));
                            notificationSettingDef[rule.RuleId].TimeSlots[1] = (slotTwo.Value == null ? Prana.BusinessObjects.DateTimeConstants.MinValue : Convert.ToDateTime(slotTwo.Value));
                            notificationSettingDef[rule.RuleId].TimeSlots[2] = (slotThree.Value == null ? Prana.BusinessObjects.DateTimeConstants.MinValue : Convert.ToDateTime(slotThree.Value));
                            notificationSettingDef[rule.RuleId].TimeSlots[3] = (slotFour.Value == null ? Prana.BusinessObjects.DateTimeConstants.MinValue : Convert.ToDateTime(slotFour.Value));
                            notificationSettingDef[rule.RuleId].TimeSlots[4] = (slotFive.Value == null ? Prana.BusinessObjects.DateTimeConstants.MinValue : Convert.ToDateTime(slotFive.Value));
                            rule.Notification = notificationSettingDef[rule.RuleId];
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

        /// <summary>
        /// Enable Disable start and end time picker according to the check box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraCheckAlertMarket_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraCheckAlertMarket.Checked)
                {
                    ultraLblStart.Visible = true;
                    ultraLblEnd.Visible = true;
                    ultraTimeEnd.Visible = true;
                    ultraTimeStart.Visible = true;
                    ultraLblStart.Enabled = true;
                    ultraLblEnd.Enabled = true;
                    ultraTimeEnd.Enabled = true;
                    ultraTimeStart.Enabled = true;
                    ultraTimeEnd.Value = DateTime.Now;
                    ultraTimeStart.Value = DateTime.Now;
                }
                else
                {
                    ultraLblStart.Visible = false;
                    ultraLblEnd.Visible = false;
                    ultraTimeEnd.Visible = false;
                    ultraTimeStart.Visible = false;
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

        /// <summary>
        /// Checks if Email is enabled then user has entered email id or not
        /// </summary>
        /// <returns></returns>
        internal bool VerifySettings()
        {
            try
            {
                if (ultraChckEmail.Checked && (string.IsNullOrEmpty(ultraTxtEmailCc.Text) && string.IsNullOrEmpty(ultraTxtEmailTo.Text)))
                {
                    return false;
                }
                else
                    return true;
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
                return false;
            }
        }

        /// <summary>
        /// Updates notification settings when updated from other user.
        /// </summary>
        /// <param name="rule"></param>
        internal void UpdateNotification(List<RuleBase> ruleList)
        {
            try
            {
                lock (notificationSettingDef)
                {
                    foreach (RuleBase rule in ruleList)
                    {
                        if (notificationSettingDef.ContainsKey(rule.RuleId))
                            notificationSettingDef[rule.RuleId] = rule.Notification;
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

        /// <summary>
        /// Sets Id wise notification settings.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notificationSetting"></param>
        internal void SetNotificationSettings(string id, NotificationSetting notificationSetting)
        {

            try
            {
                if (!String.IsNullOrEmpty(openGroupId))
                {
                    notificationSettingDef[openGroupId] = GetCurrentNotificationSettings();
                }
                if (!notificationSettingDef.ContainsKey(id))
                {
                    notificationSettingDef.Add(id, notificationSetting);
                }
                openGroupId = id;

                SetNotificationControls(notificationSettingDef[id]);

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

        /// <summary>
        /// Shows settings on UI
        /// </summary>
        /// <param name="notificationSetting"></param>
        private void SetNotificationControls(NotificationSetting notificationSetting)
        {
            try
            {
                ultraChckPopUp.Checked = notificationSetting.PopUpEnabled;
                ultraChckEmail.Checked = notificationSetting.EmailEnabled;
                ultraTxtEmailCc.Text = notificationSetting.EmailCCList;
                ultraTxtEmailTo.Text = notificationSetting.EmailToList;
                ultraTextEditorEmailSubject.Text = notificationSetting.EmailSubject;
                ultraCheckAlertMarket.Checked = notificationSetting.AlertInTimeRange;
                ultraCheckAlertHoliday.Checked = notificationSetting.StopAlertOnHolidays;
                chkSendInOneEmail.Checked = notificationSetting.SendInOneEmail;
                if (notificationSetting.LimitFrequencyMinutes == 8)
                {
                    enableScheduleSlotLayout.Visible = true;
                    chkSendInOneEmail.Visible = true;
                    ultraLblStart.Visible = false;
                    ultraLblEnd.Visible = false;
                    ultraTimeStart.Visible = false;
                    ultraTimeEnd.Visible = false;
                    DateTime minDateTime = Prana.BusinessObjects.DateTimeConstants.MinValue;
                    if (notificationSetting.TimeSlots[0] != minDateTime)
                        slotOne.Value = notificationSetting.TimeSlots[0];
                    else
                        slotOne.Value = null;
                    if (notificationSetting.TimeSlots[1] != minDateTime)
                        slotTwo.Value = notificationSetting.TimeSlots[1];
                    else
                        slotTwo.Value = null;
                    if (notificationSetting.TimeSlots[2] != minDateTime)
                        slotThree.Value = notificationSetting.TimeSlots[2];
                    else
                        slotThree.Value = null;
                    if (notificationSetting.TimeSlots[3] != minDateTime)
                        slotFour.Value = notificationSetting.TimeSlots[3];
                    else
                        slotFour.Value = null;
                    if (notificationSetting.TimeSlots[4] != minDateTime)
                        slotFive.Value = notificationSetting.TimeSlots[4];
                    else
                        slotFive.Value = null;
                }
                else if (ultraCheckAlertMarket.Checked)
                {
                    ultraLblStart.Visible = true;
                    ultraLblEnd.Visible = true;
                    ultraTimeStart.Visible = true;
                    ultraTimeStart.Value = notificationSetting.StartTime;
                    ultraTimeEnd.Visible = true;
                    ultraTimeEnd.Value = notificationSetting.EndTime;
                }
                else
                {
                    ultraLblStart.Visible = false;
                    ultraLblEnd.Visible = false;
                    ultraTimeStart.Visible = false;
                    //ultraTimeStart.Value = notificationSettingDef[rule.RuleId].StartTime;
                    ultraTimeEnd.Visible = false;
                    // ultraTimeEnd.Value = notificationSettingDef[rule.RuleId].EndTime;
                }
                ultraCmboFrequency.SelectedIndex = notificationSetting.LimitFrequencyMinutes - 1;
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

        /// <summary>
        /// Returns Notification setting for Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal NotificationSetting GetNotificationSetting(String id)
        {
            try
            {
                if (notificationSettingDef.ContainsKey(id))
                    return notificationSettingDef[id].DeepClone();
                else
                    return null;
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
                return null;
            }
        }

        /// <summary>
        /// When rule is open if rule belongs to a group hide notification settings
        /// </summary>
        /// <param name="group"></param>
        internal void SetGroupDetails(GroupBase group)
        {
            try
            {
                if (group == null)
                {
                    ultraLblGroupDetails.Visible = false;
                }
                else
                {
                    ultraLblGroupDetails.Visible = true;
                    ultraLblGroupDetails.Text = "This rule belongs to group: " + group.GroupName + " so, please set notification from Group UI.";
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

        /// <summary>
        /// Updates notification setting for group in cache.
        /// When updated from other user.
        /// </summary>
        /// <param name="groupList"></param>
        /// <param name="selectedGroupId"></param>
        internal void UpdateNotification(List<GroupBase> groupList, string selectedGroupId)
        {
            try
            {
                foreach (GroupBase group in groupList)
                {
                    if (group.GroupId == selectedGroupId)
                    {
                        if (notificationSettingDef.ContainsKey(selectedGroupId))
                            notificationSettingDef[selectedGroupId] = group.Notification;
                        else
                            notificationSettingDef.Add(selectedGroupId, group.Notification);
                        SetNotificationControls(notificationSettingDef[selectedGroupId]);
                    }
                    else
                    {
                        if (notificationSettingDef.ContainsKey(group.GroupId))
                            notificationSettingDef[group.GroupId] = group.Notification;
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

        private void NotificationSettings_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();
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

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();

                appearance1.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlMain.Appearance = appearance1;

                appearance2.FontData.BoldAsString = "True";
                appearance2.FontData.ItalicAsString = "True";
                appearance2.FontData.SizeInPoints = 9.5F;
                appearance2.ForeColor = System.Drawing.Color.White;
                this.ultraLblGroupDetails.Appearance = appearance2;

                appearance3.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraGrpBoxNotify.Appearance = appearance3;

                appearance4.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraLblEmailCc.Appearance = appearance4;

                appearance5.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraChckEmail.Appearance = appearance5;

                appearance6.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraLblEmailTo.Appearance = appearance6;

                appearance7.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraChckPopUp.Appearance = appearance7;
                appearance8.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraChckPopUp.HotTrackingAppearance = appearance8;

                appearance9.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraGrpBoxPref.Appearance = appearance9;

                appearance10.ForeColorDisabled = System.Drawing.Color.Black;
                appearance10.TextHAlignAsString = "Left";
                appearance10.TextVAlignAsString = "Middle";
                this.ultraLblEnd.Appearance = appearance10;

                appearance11.ForeColorDisabled = System.Drawing.Color.Black;
                appearance11.TextHAlignAsString = "Left";
                appearance11.TextVAlignAsString = "Middle";
                this.ultraLblStart.Appearance = appearance11;

                appearance12.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraCheckAlertMarket.Appearance = appearance12;

                appearance13.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraCheckAlertHoliday.Appearance = appearance13;

                appearance14.ForeColorDisabled = System.Drawing.Color.Black;
                this.ultraLblFrequency.Appearance = appearance14;
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
        /// Handles the ValueChanged event of the ultraCmboFrequency control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ultraCmboFrequency_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraCmboFrequency.SelectedIndex == 7)
                {
                    chkSendInOneEmail.Visible = true;
                    ultraLblStart.Visible = false;
                    ultraTimeStart.Visible = false;
                    ultraLblEnd.Visible = false;
                    ultraTimeEnd.Visible = false;
                    ultraCheckAlertMarket.Visible = false;
                    enableScheduleSlotLayout.Visible = true;
                    ultraGrpBoxPref.Size = new Size(ultraGrpBoxPref.Size.Width, 175);
                    ultraPanel1.Size = new Size(ultraPanel1.Size.Width, 180);
                    ultraPnlMain.Size = new Size(ultraPnlMain.Size.Width, 180);
                }
                else
                {
                    chkSendInOneEmail.Visible = false;
                    enableScheduleSlotLayout.Visible = false;
                    ultraGrpBoxPref.Size = new Size(ultraGrpBoxPref.Size.Width, 116);
                    ultraPanel1.Size = new Size(ultraPanel1.Size.Width, 132);
                    ultraPnlMain.Size = new Size(ultraPnlMain.Size.Width, 163);
                    ultraCheckAlertMarket.Visible = true;
                    if (ultraCheckAlertMarket.Checked)
                    {
                        ultraLblStart.Visible = true;
                        ultraTimeStart.Visible = true;
                        ultraLblEnd.Visible = true;
                        ultraTimeEnd.Visible = true;
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
    }
}
