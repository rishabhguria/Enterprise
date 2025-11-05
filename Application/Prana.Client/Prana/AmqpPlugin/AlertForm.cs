using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.AmqpPlugin
{
    public partial class AlertForm : Form
    {
        DataTable _dtListViewSource = new DataTable("ListView");

        delegate void MainThreadDelegate(String ruleType, String ruleName, String description);
        public AlertForm()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
            //InitialiseControls();            
        }

        private void SetButtonsColor()
        {
            try
            {
                ubtnDismiss.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnDismiss.ForeColor = System.Drawing.Color.White;
                ubtnDismiss.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnDismiss.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnDismiss.UseAppStyling = false;
                ubtnDismiss.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ubtnDismissAll.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnDismissAll.ForeColor = System.Drawing.Color.White;
                ubtnDismissAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnDismissAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnDismissAll.UseAppStyling = false;
                ubtnDismissAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Initialises all controls for given user
        /// </summary>
        /// <param name="userId">UserId for which pop up will be initialised</param>
        public void InitialiseControls(int userId)
        {

            try
            {
                LoadListViewDataDefinition();
                LoadAlertCount(userId);
                if (this.lstViewAlert.Items.Count == 0)
                    this.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// For a given user load all alert count for given user
        /// For pre-trade - Alert by given user occured today
        /// For post-trade - All alert occured today
        /// </summary>
        /// <param name="userId">User Id for which data will be loaded</param>
        private void LoadAlertCount(int userId)
        {
            try
            {
                DataTable tempDt = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetAlertCount", new Object[] { userId }).Tables[0];
                if (tempDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in tempDt.Rows)
                    {
                        String ruleType = dr["RuleType"].ToString();
                        String ruleName = dr["RuleName"].ToString();
                        _dtListViewSource.Rows.Add(new Object[] { ruleType, ruleName, String.Empty, dr["TriggerCount"].ToString(), ruleType + ruleName });
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Definition of data table as it contains additional columns as loaded from DB so inialising definition
        /// </summary>
        private void LoadListViewDataDefinition()
        {
            try
            {
                _dtListViewSource.Columns.Add("RuleType", typeof(String));
                _dtListViewSource.Columns.Add("RuleName", typeof(String));
                _dtListViewSource.Columns.Add("Description", typeof(String));
                _dtListViewSource.Columns.Add("Count", typeof(Double));
                _dtListViewSource.Columns.Add("Key", typeof(String));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Ocurrs when new alert is receive and pop up need to be shown
        /// </summary>
        /// <param name="ruleType">Type of rule (Either pre-trade or post-trade)</param>
        /// <param name="ruleName">Name of the rule</param>
        /// <param name="description">New description for the rule</param>
        public void NewAlertReceived(String ruleType, String ruleName, String description)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = this.NewAlertReceived;
                        this.BeginInvoke(del, new object[] { ruleType, ruleName, description });
                    }
                    else
                    {
                        String filter = "Key='" + ruleType + ruleName + "'";
                        if (_dtListViewSource.Select(filter).Length > 0)
                        {
                            _dtListViewSource.Select(filter)[0]["Count"] = Convert.ToInt32(_dtListViewSource.Select(filter)[0]["Count"]) + 1;
                            _dtListViewSource.Select(filter)[0]["Description"] = description;
                        }
                        else
                        {
                            _dtListViewSource.Rows.Add(new Object[] { ruleType, ruleName, description, 1, ruleType + ruleName });
                        }

                        ReloadItemsInListView();
                        if (lstViewAlert.Items.ContainsKey(ruleType + ruleName))
                            lstViewAlert.Items[ruleType + ruleName].Selected = true;
                        //ulstAlert.SelectedItems.Clear();
                        //ulstAlert.SelectedItems.Add(ulstAlert.Items[ruleName]);
                        if (this.WindowState == FormWindowState.Minimized)
                        {
                            this.WindowState = FormWindowState.Normal;
                        }
                        this.Visible = true;
                        this.BringToFront();
                        System.Media.SystemSounds.Exclamation.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Reloading list view of pop up
        /// </summary>
        private void ReloadItemsInListView()
        {
            try
            {
                lstViewAlert.Items.Clear();
                foreach (DataRow dr in _dtListViewSource.Rows)
                {
                    if (!String.IsNullOrEmpty(dr["Description"].ToString()))
                    {
                        ListViewItem item = new ListViewItem();//dr["Key"].ToString());
                        item.Name = dr["Key"].ToString();
                        item.Text = MessageFormatter.FormatRuleNameForDisplay(dr["RuleName"].ToString()) + " (" + dr["Count"].ToString() + " alerts today) - (" + dr["RuleType"].ToString() + ")";
                        item.ToolTipText = MessageFormatter.FormatRuleNameForDisplay(dr["RuleName"].ToString()) + " (" + dr["Count"].ToString() + " alerts today) - (" + dr["RuleType"].ToString() + ")\n" + dr["Description"].ToString();
                        item.SubItems.Add(item.Text);
                        lstViewAlert.Items.Add(item);
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// When a particular alert to be dismissed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnDismiss_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstViewAlert.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in lstViewAlert.SelectedItems)
                    {
                        String key = item.Name;
                        String filter = "Key='" + key + "'";
                        _dtListViewSource.Rows.Remove(_dtListViewSource.Select(filter)[0]);
                        lstViewAlert.SelectedItems.Clear();
                        lstViewAlert.Items.RemoveByKey(key);
                    }

                    if (lstViewAlert.Items.Count == 0)
                        this.Close();

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// When all alert to dismissed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnDismissAll_Click(object sender, EventArgs e)
        {
            try
            {
                _dtListViewSource.Rows.Clear();
                lstViewAlert.Items.Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Closing is canceled and simply set visible = false, so that reloading is avoided
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //CloseReason is UserClosing: The user is closing the form through the user interface (UI), for example
                //by clicking the Close button on the form window, selecting Close from the
                //window's control menu, or pressing ALT+F4.
                //Return in case of other close reason. JIRA: PRANA-12552
                if (e.CloseReason != CloseReason.UserClosing) return;
                e.Cancel = true;
                this.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Occurs when new description is to be loaded into labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstViewAlert_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstViewAlert.SelectedItems.Count == 1)
                {
                    String filter = "Key='" + lstViewAlert.SelectedItems[0].Name + "'";
                    DataRow dr = _dtListViewSource.Select(filter)[0];//["RuleName"]
                    ulblDescription.Text = dr["Description"].ToString();
                    ulblRuleName.Text = lstViewAlert.SelectedItems[0].Text;
                    spAlert.Panel2Collapsed = false;
                    if (ulblDescription.Height + ulblRuleName.Height <= 130)
                        spAlert.SplitterDistance = spAlert.Height - (ulblRuleName.Height + ulblDescription.Height + spAlert.SplitterWidth + spAlert.Panel2MinSize);
                    else
                        spAlert.SplitterDistance = spAlert.Height - (130 + spAlert.SplitterWidth + spAlert.Panel2MinSize);
                    //ReloadItemsInListView();
                }
                else
                {
                    ulblDescription.Text = String.Empty;// dr["Description"].ToString();
                    ulblRuleName.Text = String.Empty;// dr["RuleName"].ToString();
                    spAlert.Panel2Collapsed = true;
                }

                if (lstViewAlert.SelectedItems.Count > 0)
                    ubtnDismiss.Enabled = true;
                else
                    ubtnDismiss.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


    }
}