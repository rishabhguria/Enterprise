using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.Classes;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    public partial class BrokersConnectionStatus : Form
    {
        /// <summary>
        /// Account wise mapped brokers for selected fund
        /// </summary>
        private Dictionary<int, int> _accountBrokerMapping;
        /// <summary>
        /// Value list for Broker drop down
        /// </summary>
        private ValueList _brokerValueList = new ValueList();
        /// <summary>
        /// The list having available brokers for current user
        /// </summary>
        private List<int> _userBrokers;
        /// <summary>
        /// The column account
        /// </summary>
        private const string COLUMN_ACCOUNT = "Account";
        /// <summary>
        /// The column broker
        /// </summary>
        private const string COLUMN_BROKER = "Broker";
        /// <summary>
        /// The column connection status
        /// </summary>
        private const string COLUMN_CONNECTION_STATUS = "Connection Status";

        public BrokersConnectionStatus(Dictionary<int, int> accountBrokerMapping, ValueList brokerDropDownValueList)
        {
            try
            {
                InitializeComponent();
                _accountBrokerMapping = accountBrokerMapping;

                foreach (ValueListItem vl in brokerDropDownValueList.ValueListItems)
                {
                    _brokerValueList.ValueListItems.Add(vl.DataValue, vl.DisplayText);
                }
                SetDataSource();
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += BrokersConnectionStatus_CounterPartyStatusUpdate;
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
        /// Set the theme and appearance of form
        /// </summary>
        private void BrokerMapping_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Broker Connection Status", CustomThemeHelper.UsedFont);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.btnOK.UseAppStyling = false;
                    this.btnOK.UseOsThemes = DefaultableBoolean.False;
                    this.btnOK.ButtonStyle = UIElementButtonStyle.Button3D;
                    this.btnOK.Appearance.BackColor = Color.FromArgb(48, 60, 78);
                    this.btnOK.Appearance.ForeColor = Color.White;
                    this.BackColor = Color.FromArgb(90, 89, 90);
                }
                _userBrokers = new List<int>();
                foreach (ValueListItem vl in _brokerValueList.ValueListItems)
                {
                    _userBrokers.Add(Convert.ToInt32(vl.DataValue));
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
        /// Sets the color of the button.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnOK.BackColor = Color.FromArgb(55, 67, 85);
                btnOK.ForeColor = Color.White;
                btnOK.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOK.ButtonStyle = UIElementButtonStyle.Button3D;
                btnOK.UseAppStyling = false;
                btnOK.UseOsThemes = DefaultableBoolean.False;
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
        /// Sets the datasource of brokers grid.
        /// </summary>
        private void SetDataSource()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(COLUMN_ACCOUNT);
                dt.Columns.Add(COLUMN_BROKER);
                dt.Columns.Add(COLUMN_CONNECTION_STATUS);
                foreach (KeyValuePair<int, int> kvp in _accountBrokerMapping)
                {
                    string account = CachedDataManager.GetInstance.GetAccount(kvp.Key);
                    string broker = CachedDataManager.GetInstance.GetCounterPartyText(kvp.Value);
                    dt.Rows.Add(account, broker, null);
                }
                grdBrokers.DataSource = dt;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initilaize the grdBrokers with required properties.
        /// </summary>
        private void grdBrokers_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdBrokers.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].CellActivation = Activation.NoEdit;
                grdBrokers.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].Width = 50;
                grdBrokers.DisplayLayout.Bands[0].Columns[COLUMN_BROKER].Width = 50;
                grdBrokers.DisplayLayout.Bands[0].Columns[COLUMN_CONNECTION_STATUS].CellActivation = Activation.NoEdit;
                grdBrokers.DisplayLayout.Bands[0].Columns[COLUMN_CONNECTION_STATUS].Width = 50;
                grdBrokers.DisplayLayout.Bands[0].ColHeaderLines = 2;
                grdBrokers.DrawFilter = new MultiBrokerGridDrawFilter();

                grdBrokers.DisplayLayout.Override.CellPadding = 5;
                grdBrokers.DisplayLayout.Override.HeaderAppearance.TextHAlign = HAlign.Center;
                grdBrokers.DisplayLayout.Override.RowAppearance.TextHAlign = HAlign.Center;
                grdBrokers.DisplayLayout.Override.RowAppearance.ImageHAlign = HAlign.Center;
                grdBrokers.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsOnly;

                _brokerValueList.SortStyle = ValueListSortStyle.Ascending;

                e.Layout.Bands[0].Columns[COLUMN_BROKER].ValueList = _brokerValueList;
                e.Layout.Bands[0].Columns[COLUMN_BROKER].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                e.Layout.Bands[0].Columns[COLUMN_BROKER].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                e.Layout.Bands[0].Columns[COLUMN_BROKER].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                e.Layout.Bands[0].Columns[COLUMN_BROKER].AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith;

                //Event to call grdBrokers_AfterCellUpdate as per need
                grdBrokers.AfterCellListCloseUp += delegate { grdBrokers.UpdateData(); };
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
        /// Disable the rows of accounts mapped with broker and allow selection of broker for unmapped accounts
        /// </summary>
        private void grdBrokers_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                int accountId = CachedDataManager.GetInstance.GetAccountID(e.Row.Cells[COLUMN_ACCOUNT].Value.ToString());
                if (CachedDataManager.GetInstance.GetCounterPartyByAccountId(accountId) != int.MinValue && _userBrokers.Contains(CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping()[accountId]))
                {
                    e.Row.Cells[COLUMN_BROKER].Activation = Activation.Disabled;
                    e.Row.Cells[COLUMN_ACCOUNT].Activation = Activation.Disabled;
                }
                else
                {
                    e.Row.Cells[COLUMN_BROKER].Activation = Activation.AllowEdit;
                    if (String.IsNullOrEmpty(e.Row.Cells[COLUMN_BROKER].Text))
                    {
                        e.Row.DataErrorInfo.RowError = "Broker can't be empty!";
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
        /// Sets/resets the row error as per the selected broker
        /// </summary>
        private void grdBrokers_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == COLUMN_BROKER)
                {
                    if (e.Cell.Value == null || !ValueListUtilities.CheckIfValueExistsInValuelist(_brokerValueList, e.Cell.Value.ToString()))
                    {
                        grdBrokers.Rows[e.Cell.Row.Index].DataErrorInfo.RowError = "Invalid Broker!";
                    }
                    else
                    {
                        grdBrokers.Rows[e.Cell.Row.Index].DataErrorInfo.ResetRowError();
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
        /// Handles the Ok button click of form and saves the current mapping
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in grdBrokers.Rows)
                {
                    int accountId = CachedDataManager.GetInstance.GetAccountID(row.Cells[COLUMN_ACCOUNT].Text);
                    _accountBrokerMapping[accountId] = CachedDataManager.GetInstance.GetCounterPartyID(row.Cells[COLUMN_BROKER].Text);
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
        /// Updates the broker connection status on UI whenever there is any change in it
        /// <summary>
        private void BrokersConnectionStatus_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            BrokersConnectionStatus_CounterPartyStatusUpdate(sender, e);
                        }));
                    }
                    else
                    {
                        grdBrokers.Refresh();
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
