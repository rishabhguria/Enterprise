using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    /// <summary>
    /// Create and Allocate from Trading Ticket
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class AccountQty : Form, IExportGridData
    {
        /// <summary>
        /// The isComingFromPM
        /// </summary>
        public bool isComingFromPM = false;

        public bool isComingFromPMForIncrease = false;
        /// <summary>
        /// The total allocation qty
        /// </summary>
        private decimal _totalAllocationQty = 0;

        /// <summary>
        /// Gets or sets the total allocation qty.
        /// </summary>
        /// <value>
        /// The total allocation qty.
        /// </value>
        public decimal TotalAllocationQty
        {
            get { return _totalAllocationQty; }
            set { _totalAllocationQty = value; }
        }

        private Dictionary<int, double> _accountWithPostions = null;

        public Dictionary<int, double> AccountWithPostions
        {
            get { return _accountWithPostions; }
            set { _accountWithPostions = value; }
        }
        /// <summary>
        /// </summary>
        private DataTable dt;
        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol;
        /// <summary>
        /// The colum n_ account
        /// </summary>
        private const string COLUMN_ACCOUNT = "Account";
        /// <summary>
        /// The colum n_ accountid
        /// </summary>
        private const string COLUMN_ACCOUNTID = "AccountID";
        /// <summary>
        /// The colum n_ curren t_ position
        /// </summary>
        private const string COLUMN_CURRENT_POSITION = "Current Quantity";
        /// <summary>
        /// The colum n_ curren t_ percentage
        /// </summary>
        private const string COLUMN_CURRENT_PERCENTAGE = "Current %";
        /// <summary>
        /// The colum n_ allocatio n_ percent
        /// </summary>
        private const string COLUMN_ALLOCATION_PERCENT = "Allocation %";
        /// <summary>
        /// The column allocated position
        /// </summary>
        private const string COLUMN_ALLOCATED_POSITION = "Allocated Quantity";

        /// <summary>
        /// To know whether we are reloading an existing order or not
        /// </summary>
        public bool IsReloadOrderRequest { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountQty"/> class.
        /// </summary>
        /// <param name="Symbol">The symbol.</param>
        public AccountQty(string Symbol)
        {
            try
            {
                InitializeComponent();

                _symbol = Symbol;
                InitGrid();
                this.FormClosed += AccountQty_Closed;
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

        private void AccountQty_Closed(object sender, FormClosedEventArgs e)
        {
            try
            {
                InstanceManager.ReleaseInstance(typeof(AccountQty));
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
        /// Initializes a new instance of the <see cref="AccountQty"/> class.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fundId"></param>
        public AccountQty(string symbol, int fundId)
        {
            try
            {
                InitializeComponent();
                this._symbol = symbol;
                this._fundId = fundId;
                InitGrid();
                this.FormClosed += AccountQty_Closed;
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
        /// Sets the allocation percentage.
        /// </summary>
        /// <param name="selectedAccountText">The selected account text.</param>
        public void SetAllocationPercentage(string selectedAccountText)
        {
            try
            {
                if (!isComingFromPM)
                    foreach (UltraGridRow item in grdAccounts.Rows)
                    {
                        item.Cells[5].Value = 0;
                        item.Cells[4].Value = 0;
                    }

                if (!isComingFromPM && !String.IsNullOrEmpty(selectedAccountText) && selectedAccountText.ToLower().Contains("custom") && _allocationOperationPreference != null)
                {

                    foreach (KeyValuePair<int, AccountValue> kvp in _allocationOperationPreference.TargetPercentage)
                    {
                        UltraGridRow row = grdAccounts.Rows.FirstOrDefault(r => r.Cells[COLUMN_ACCOUNTID].Value.ToString().Equals(kvp.Key.ToString()));
                        if (row != null)
                        {
                            row.Cells[5].Value = kvp.Value.Value;
                        }
                    }


                }
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
        /// Handles the Load event of the AccountQty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AccountQty_Load(object sender, EventArgs e)
        {
            try
            {
                if (!ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected)
                {
                    if (!String.IsNullOrEmpty(ExpnlServiceConnector.GetInstance().TryGetChannel()))
                    {
                        _error = " Could not connect to Calculation Service";
                        this.Close();
                        return;
                    }

                }

                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Custom Allocation", CustomThemeHelper.UsedFont);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                    this.btnCancel.UseAppStyling = false;
                    this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                    this.btnCancel.Appearance.BackColor = System.Drawing.Color.FromArgb(48, 60, 78);
                    this.btnCancel.Appearance.ForeColor = System.Drawing.Color.White;
                    this.btnOK.UseAppStyling = false;
                    this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                    this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                    this.btnOK.Appearance.BackColor = System.Drawing.Color.FromArgb(48, 60, 78);
                    this.btnOK.Appearance.ForeColor = System.Drawing.Color.White;
                    this.BackColor = System.Drawing.Color.FromArgb(90, 89, 90);

                    this.btnClear.UseAppStyling = false;
                    this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                    this.btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                    this.btnClear.Appearance.BackColor = System.Drawing.Color.FromArgb(48, 60, 78);
                    this.btnClear.Appearance.ForeColor = System.Drawing.Color.White;
                }
                InstanceManager.RegisterInstance(this);
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
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnOK.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOK.ForeColor = System.Drawing.Color.White;
                btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOK.UseAppStyling = false;
                btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Initializes the grid.
        /// </summary>
        private void InitGrid()
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add(COLUMN_ACCOUNT, typeof(string));
                dt.Columns.Add(COLUMN_ACCOUNTID, typeof(int));
                dt.Columns.Add(COLUMN_CURRENT_POSITION, typeof(double));
                dt.Columns.Add(COLUMN_CURRENT_PERCENTAGE, typeof(double));
                dt.Columns.Add(COLUMN_ALLOCATED_POSITION, typeof(double));
                dt.Columns.Add(COLUMN_ALLOCATION_PERCENT, typeof(double));
                grdAccounts.DataSource = dt;


                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].Format = "N2";
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].Header.Appearance.TextHAlign = HAlign.Center;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].CellAppearance.TextHAlign = HAlign.Right;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION].Format = "N2";
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION].Header.Appearance.TextHAlign = HAlign.Center;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION].CellAppearance.TextHAlign = HAlign.Right;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION].CellAppearance.BackColor = Color.Gray;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT].CellAppearance.BackColor = Color.Gray;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT].CellAppearance.TextHAlign = HAlign.Right;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT].Header.Appearance.TextHAlign = HAlign.Center;
                grdAccounts.DisplayLayout.Bands[0].ColHeaderLines = 2;

                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].CellActivation = Activation.AllowEdit;
                grdAccounts.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                grdAccounts.DisplayLayout.Override.TemplateAddRowAppearance.BackColor = Color.FromArgb(245, 250, 255);
                grdAccounts.DisplayLayout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;
                grdAccounts.DisplayLayout.Override.AddRowAppearance.BackColor = Color.LightYellow;
                grdAccounts.DisplayLayout.Override.AddRowAppearance.ForeColor = Color.Blue;
                grdAccounts.DisplayLayout.Override.SpecialRowSeparator = SpecialRowSeparator.TemplateAddRow;
                grdAccounts.DisplayLayout.Override.SpecialRowSeparatorAppearance.BackColor = SystemColors.Control;
                // grdAccounts.DisplayLayout.Override.TemplateAddRowPrompt = "Click here to add a new row...";
                grdAccounts.DisplayLayout.Override.TemplateAddRowPromptAppearance.ForeColor = Color.Maroon;
                grdAccounts.DisplayLayout.Override.TemplateAddRowPromptAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].CellActivation = Activation.NoEdit;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].CellAppearance.TextHAlign = HAlign.Right;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE].CellActivation = Activation.NoEdit;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE].CellAppearance.TextHAlign = HAlign.Right;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE].Header.Appearance.TextHAlign = HAlign.Center;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE].Format = "N2";

                foreach (UltraGridColumn column in grdAccounts.DisplayLayout.Bands[0].Columns)
                {
                    column.Width = 100;
                }

                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].Header.Appearance.TextHAlign = HAlign.Center;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].Width = 120;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNTID].Hidden = true;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNTID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                this.grdAccounts.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;

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


        ValueList vlAccount = new ValueList();
        /// <summary>
        /// Loads the positions.
        /// </summary>
        bool flag = false;
        public void LoadPositions()
        {
            try
            {
                if (_symbol == TradingTicketConstants.MTT_BULK_UPDATE_SYMBOL)
                {
                    if (!flag)
                    {
                        List<string> permittedUserAccounts = CachedDataManager.GetInstance.GetAllAccountNamesForUser();
                        foreach (string accountName in permittedUserAccounts)
                        {
                            int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                            vlAccount.ValueListItems.Add(accountID, accountName);
                        }
                    }
                    grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].ValueList = vlAccount;
                    flag = true;
                    _error = String.Empty;
                }
                else if (!string.IsNullOrEmpty(_symbol))
                {
                    List<int> filteredAccountList = new List<int>();
                    if (_fundId > 0)
                    {
                        var mapping = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                        if (mapping.ContainsKey(_fundId))
                            filteredAccountList = mapping[_fundId];

                    }
                    else
                    {
                        List<string> permittedUserAccounts = CachedDataManager.GetInstance.GetAllAccountNamesForUser();
                        filteredAccountList.AddRange(permittedUserAccounts.Select(accountname => CachedDataManager.GetInstance.GetAccountID(accountname)));
                    }
                    StringBuilder sb = new StringBuilder();
                    Dictionary<int, decimal> items = null;
                    if (_accountWithPostions != null)
                        items = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(_symbol, _accountWithPostions.Keys.ToList(), ref sb);
                    else
                        items = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(_symbol, filteredAccountList, ref sb);

                    _error = sb.ToString();

                    dt.Rows.OfType<DataRow>().ToList().ForEach(r =>
                    {
                        r[COLUMN_CURRENT_POSITION] = 0;
                        r[COLUMN_CURRENT_PERCENTAGE] = 0;
                    });
                    decimal totalPosition = items.Sum(kvp => Math.Abs(kvp.Value));

                    grdAccounts.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);

                    if (isComingFromPM)
                    {
                        foreach (var Account in filteredAccountList)
                        {
                            if (!(items.Keys.Contains(Account)))
                            {
                                if (vlAccount.ValueListItems.All.ToList().FindIndex(G => G.ToString().Equals(CachedDataManager.GetInstance.GetAccountText(Account))) == -1)
                                    vlAccount.ValueListItems.Add(Account, CachedDataManager.GetInstance.GetAccountText(Account));
                            }
                        }
                    }
                    foreach (KeyValuePair<int, decimal> nameGroup in items)
                    {
                        string account = CachedDataManager.GetInstance.GetAccountText(nameGroup.Key);

                        if (nameGroup.Value == 0 && !flag)
                        {
                            Boolean isAllocatedAccount = false;
                            if (_allocationOperationPreference != null)
                            {
                                if (_allocationOperationPreference.TargetPercentage.ContainsKey(nameGroup.Key))
                                {
                                    isAllocatedAccount = true;
                                }
                            }
                            if (!isAllocatedAccount)
                            {
                                vlAccount.ValueListItems.Add(nameGroup.Key, account);
                                continue;
                            }

                        }
                        decimal currentPercentage = 0.0m;
                        decimal allocatedPercentage = 0.0m;
                        if (totalPosition != 0)
                        {
                            currentPercentage = (nameGroup.Value / totalPosition) * 100;
                            if (TotalAllocationQty != 0 && _accountWithPostions != null && _accountWithPostions.ContainsKey(nameGroup.Key) && !isComingFromPMForIncrease)
                                allocatedPercentage = Math.Abs(((decimal)_accountWithPostions[nameGroup.Key] / TotalAllocationQty) * 100);
                            else if (isComingFromPMForIncrease && _accountWithPostions != null && _accountWithPostions.ContainsKey(nameGroup.Key))
                                allocatedPercentage = Math.Abs(((decimal)_accountWithPostions[nameGroup.Key] / totalPosition) * 100);
                        }
                        if (!string.IsNullOrEmpty(account) && dt.AsEnumerable().All(row => CachedDataManager.GetInstance.GetAccountText(nameGroup.Key) != row.Field<String>(COLUMN_ACCOUNT)))
                        {
                            if (isComingFromPM)
                            {
                                if (isComingFromPMForIncrease)
                                {
                                    grdAccounts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
                                    dt.Rows.Add(account, nameGroup.Key, Math.Abs(nameGroup.Value), Math.Abs(currentPercentage), 0, allocatedPercentage);
                                    UltraGridRow newlyAddedrow = grdAccounts.Rows.Last();
                                    newlyAddedrow.Cells[5].Value = allocatedPercentage;
                                    grdAccounts.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
                                }
                                else
                                    dt.Rows.Add(account, nameGroup.Key, Math.Abs(nameGroup.Value), Math.Abs(currentPercentage), _accountWithPostions != null && _accountWithPostions.ContainsKey(nameGroup.Key) ? (decimal)_accountWithPostions[nameGroup.Key] : Math.Abs(nameGroup.Value), allocatedPercentage);
                            }
                            else if (!flag)
                                dt.Rows.Add(account, nameGroup.Key, Math.Abs(nameGroup.Value), Math.Abs(currentPercentage), 0, 0);
                        }
                        else
                        {
                            UltraGridRow row = grdAccounts.Rows.FirstOrDefault(r => r.Cells[COLUMN_ACCOUNTID].Value.ToString().Equals(nameGroup.Key.ToString()));
                            if (row != null)
                            {
                                row.Cells[2].Value = Math.Abs(nameGroup.Value);
                                row.Cells[3].Value = Math.Abs(currentPercentage);
                                if (isComingFromPM)
                                {
                                    if (isComingFromPMForIncrease)
                                    {
                                        grdAccounts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
                                        row.Cells[5].Value = allocatedPercentage;
                                        grdAccounts.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
                                    }
                                    else
                                    {
                                        row.Cells[4].Value = _accountWithPostions != null && _accountWithPostions.ContainsKey(nameGroup.Key) ? (decimal)_accountWithPostions[nameGroup.Key] : Math.Abs(nameGroup.Value);
                                        row.Cells[5].Value = allocatedPercentage;
                                    }
                                }
                            }
                        }
                        if (grdAccounts.Rows.Count != 0 && !isComingFromPM)
                        {
                            UltraGridRow lastAddedrow = grdAccounts.Rows.Last();
                            lastAddedrow.Cells[0].Activation = Activation.NoEdit;
                        }
                    }
                    flag = true;
                    grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].ValueList = vlAccount;
                    grdAccounts.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccounts_AfterCellUpdate);
                }
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
        /// Remove the Column "Allocated Quantity" and adjust the UI accordingly
        /// </summary>       
        public void RemoveColumnAllocatedQuantity()
        {
            try
            {
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION].Hidden = true;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_POSITION].Hidden = true;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE].Hidden = true;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT].Width = 175;
                grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].Width = 220;
                this.ClientSize = new System.Drawing.Size(440, 297);
                this.btnOK.Location = new System.Drawing.Point(228, 225);
                this.btnCancel.Location = new System.Drawing.Point(322, 225);
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
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                double percentageSum = Math.Round(dt.Rows.Cast<DataRow>().Sum(dr => Double.Parse(dr[COLUMN_ALLOCATION_PERCENT].ToString())));

                if (percentageSum != 100)
                {
                    if (percentageSum == 0)
                        return;
                    _error = "Sum of Allocation (%) must be 100, No preference created!";
                    return;
                }
                _error = createAllocationPreferecne();
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
        /// Handles the InitializeLayout event of the grdAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdAccounts_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;
                e.Layout.Bands[0].Summaries.Add("CURRENT_POSITION_KEY", SummaryType.Sum, e.Layout.Bands[0].Columns[COLUMN_CURRENT_POSITION]);
                e.Layout.Bands[0].Summaries["CURRENT_POSITION_KEY"].DisplayFormat = "{0:#,0}";
                e.Layout.Bands[0].Summaries["CURRENT_POSITION_KEY"].Appearance.TextHAlign = HAlign.Right;
                e.Layout.Bands[0].Summaries.Add("CURRENT_PERCENTAGE_KEY", SummaryType.Sum, e.Layout.Bands[0].Columns[COLUMN_CURRENT_PERCENTAGE]);
                e.Layout.Bands[0].Summaries["CURRENT_PERCENTAGE_KEY"].DisplayFormat = "{0:N0}";
                e.Layout.Bands[0].Summaries["CURRENT_PERCENTAGE_KEY"].Appearance.TextHAlign = HAlign.Right;
                e.Layout.Bands[0].Summaries.Add("ALLOCATION_POSITION_KEY", SummaryType.Sum, e.Layout.Bands[0].Columns[COLUMN_ALLOCATED_POSITION]);
                e.Layout.Bands[0].Summaries["ALLOCATION_POSITION_KEY"].DisplayFormat = "{0:#,0}";
                e.Layout.Bands[0].Summaries["ALLOCATION_POSITION_KEY"].Appearance.TextHAlign = HAlign.Right;
                e.Layout.Bands[0].Summaries.Add("ALLOCATION_PERCENT_KEY", SummaryType.Sum, e.Layout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT]);
                e.Layout.Bands[0].Summaries["ALLOCATION_PERCENT_KEY"].DisplayFormat = "{0:N0}";
                e.Layout.Bands[0].Summaries["ALLOCATION_PERCENT_KEY"].Appearance.TextHAlign = HAlign.Right;
                e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                //  grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENT].MaskInput = "-nnnnn.nn";
                foreach (UltraGridColumn column in e.Layout.Bands[0].Columns)
                {
                    column.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                    column.AllowRowSummaries = AllowRowSummaries.False;
                }

                UltraGridLayout layout = e.Layout;
                UltraGridBand band = layout.Bands[0];
                band.Columns[COLUMN_ALLOCATION_PERCENT].PromptChar = ' ';
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
        /// Handles the KeyUp event of the grdAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void grdAccounts_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdAccounts.ActiveCell.CanEnterEditMode)
                {
                    grdAccounts.PerformAction(UltraGridAction.ExitEditMode);
                }
                grdAccounts.PerformAction(UltraGridAction.BelowCell);
                if (grdAccounts.ActiveCell.CanEnterEditMode)
                {
                    grdAccounts.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (grdAccounts.ActiveCell != null && grdAccounts.ActiveCell.Column.Header.Caption == "" && grdAccounts.ActiveCell.IsInEditMode)
                {
                    grdAccounts.PerformAction(UltraGridAction.ExitEditMode);
                    grdAccounts.PerformAction(UltraGridAction.EnterEditMode);
                }
            }

        }

        /// <summary>
        /// Handles the MouseUp event of the grdAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void grdAccounts_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (grdAccounts.ActiveCell != null && grdAccounts.ActiveCell.Column.Header.Caption == "" && grdAccounts.ActiveCell.IsInEditMode)
                {
                    grdAccounts.PerformAction(UltraGridAction.ExitEditMode);
                    grdAccounts.PerformAction(UltraGridAction.EnterEditMode);
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
        /// Handles the AfterExitEditMode event of the grdAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void grdAccounts_AfterExitEditMode(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The _allocation manager
        /// </summary>
        private IAllocationManager _allocationManager;
        /// <summary>
        /// Sets the allocation manager.
        /// </summary>
        /// <value>
        /// The allocation manager.
        /// </value>
        public IAllocationManager AllocationManager
        {
            set { _allocationManager = value; }
        }

        /// <summary>
        /// The _allocation operation preference
        /// </summary>
        private AllocationOperationPreference _allocationOperationPreference = null;

        /// <summary>
        /// Gets the allocation operation preference.
        /// </summary>
        /// <value>
        /// The allocation operation preference.
        /// </value>
        internal AllocationOperationPreference AllocationOperationPreference
        {
            get { return _allocationOperationPreference; }
            set
            {
                _allocationOperationPreference = value;
            }
        }

        /// <summary>
        /// Creates the allocation preferecne.
        /// </summary>
        /// <returns></returns>
        public string createAllocationPreferecne(List<AccountValue> accountValues = null)
        {
            try
            {
                if (IsReloadOrderRequest)
                {
                    _allocationOperationPreference = null;
                }
                string tooltips = "Account Allocations:";               
                if(accountValues == null)
                {
                    accountValues = new List<AccountValue>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Decimal.Parse(dr[COLUMN_ALLOCATION_PERCENT].ToString()) > 0)
                        {
                            accountValues.Add(new AccountValue((int)dr[COLUMN_ACCOUNTID], Decimal.Parse(dr[COLUMN_ALLOCATION_PERCENT].ToString())));
                        }
                    }
                }
                string prefName = "*Custom#_" + _symbol + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "_" + DateTime.Now.Ticks;
                TTHelperManager.GetInstance().AllocationManager = _allocationManager;
                _error = TTHelperManager.GetInstance().CreateAllocationOperationPreference(accountValues, ref _allocationOperationPreference, prefName, MatchClosingTransactionType.None);
                TTHelperManager.GetInstance().AllocationManager = null;
                _toolTip = (_error == null) ? tooltips : string.Empty;
                return _error;
            }
            catch
            {
                return "Error when create allocation template";
            }

        }

        /// <summary>
        /// The _error
        /// </summary>
        private string _error = null;

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Gets the sum percentage.
        /// </summary>
        /// <value>
        /// The sum percentage.
        /// </value>
        public double SumPercentage
        {
            get { return Math.Round((dt.Rows.Cast<DataRow>().Sum(dr => Double.Parse(dr[COLUMN_ALLOCATION_PERCENT].ToString()))), 2); }
        }

        /// <summary>
        /// The _tool tip
        /// </summary>
        private string _toolTip;
        private int _fundId;

        /// <summary>
        /// Gets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        public string ToolTip
        {
            get { return _toolTip; }
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdAccounts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdAccounts_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.ListObject;
                if (_accountError)
                {
                    drv.Row.RowError = "Account is invalid !";
                    _accountError = false;
                }
                else
                {
                    drv.Row.RowError = "";
                }
                e.Row.Cells[COLUMN_ALLOCATION_PERCENT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                e.Row.Cells[COLUMN_ALLOCATED_POSITION].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
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
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraGridRow item in grdAccounts.Rows)
                {
                    item.Cells[4].Value = 0;
                    item.Cells[5].Value = 0;
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

        bool _accountError = false;
        private void grdAccounts_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                this.grdAccounts.UpdateData();
                if (e.Cell.Column.Key == COLUMN_ACCOUNT)
                {
                    int result;
                    bool ans = Int32.TryParse(e.Cell.Value.ToString(), out result);
                    if (!ans)
                    {
                        _accountError = true;
                        btnOK.Enabled = false;
                        return;
                    }
                    btnOK.Enabled = true;
                    dt.Rows[e.Cell.Row.Index][COLUMN_ACCOUNTID] = result;
                    dt.Rows[e.Cell.Row.Index][COLUMN_ACCOUNT] = CachedDataManager.GetInstance.GetAccountText(result);
                    foreach (var nameGroup in vlAccount.ValueListItems)
                    {
                        if (Convert.ToInt32(nameGroup.DataValue) == result)
                        {
                            vlAccount.ValueListItems.Remove(nameGroup);
                            break;
                        }
                    }
                    grdAccounts.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].ValueList = vlAccount;

                    UltraGridRow lastAddedrow = grdAccounts.Rows.Last();
                    lastAddedrow.Cells[0].Activation = Activation.NoEdit;
                }
                if (e.Cell.Column.Key == COLUMN_ALLOCATION_PERCENT)
                {
                    if (e.Cell.Value == DBNull.Value)
                    {
                        e.Cell.Value = 0.0;
                    }
                    decimal cellValue = Convert.ToDecimal(e.Cell.Value);
                    if (cellValue >= 0.0m && dt.Rows.Count > e.Cell.Row.Index)
                    {
                        dt.Rows[e.Cell.Row.Index][COLUMN_ALLOCATED_POSITION] = (TotalAllocationQty * cellValue) / 100;
                    }
                }
                if (e.Cell.Column.Key == COLUMN_ALLOCATED_POSITION)
                {
                    if (e.Cell.Value == DBNull.Value)
                    {
                        e.Cell.Value = 0.0;
                    }
                    decimal cellValue = Convert.ToDecimal(e.Cell.Value);
                    if (cellValue >= 0.0m && dt.Rows.Count > e.Cell.Row.Index)
                    {
                        dt.Rows[e.Cell.Row.Index][COLUMN_ALLOCATION_PERCENT] = TotalAllocationQty != 0 ? (cellValue * 100 / TotalAllocationQty) : 0;
                    }
                }
                this.grdAccounts.UpdateData();
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
        private void grdAccounts_InitializeTemplateAddRow(object sender, InitializeTemplateAddRowEventArgs e)
        {
            try
            {
                e.TemplateAddRow.Cells[COLUMN_ALLOCATION_PERCENT].Value = 0.0;
                e.TemplateAddRow.Cells[COLUMN_ALLOCATED_POSITION].Value = 0.0;
                e.TemplateAddRow.Cells[COLUMN_CURRENT_PERCENTAGE].Value = 0;
                e.TemplateAddRow.Cells[COLUMN_CURRENT_POSITION].Value = 0;
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

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "grdTrades")
                {
                    exporter.Export(grdAccounts, filePath);
                }
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
    }
}
