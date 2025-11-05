using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Utilities;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.ExtensionUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Blotter.Controls
{
    public partial class ViewAllocationDetails : UserControl
    {
        private ValueList vlAccount = new ValueList();

        private DataTable _gridData = null;
        private bool _isGroupedOrder = false;
        private decimal _executedQuantity = 0;
        private decimal totalQuantity = 0;
        private Dictionary<int, string> _userPermitedAccountList = new Dictionary<int, string>();
        /// <summary>
        /// The logged in user
        /// </summary>
        private CompanyUser _loggedInUser = null;

        /// <summary>
        /// The order group identifier
        /// </summary>
        private string _orderGroupId = string.Empty;

        private DateTime _tradeDate = DateTime.Now;

        /// <summary>
        /// The clOrderId of order
        /// </summary>
        private string _clOrderId = string.Empty;

        /// <summary>
        /// Occurs when [update status bar].
        /// </summary>
        public event EventHandler<EventArgs<AllocationResponse>> UpdateAuditAndStatusBar = null;

        bool _accountError = false;

        public ViewAllocationDetails()
        {
            InitializeComponent();
            SetButtonsColor();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="totalQuantity">The total quantity.</param>
        /// <param name="allocationDetails">The allocation details.</param>
        public void BindData(OrderSingle order, DataTable allocationDetails, Dictionary<int, string> userAccountList, CompanyUser loggedInUser)
        {
            try
            {
                _clOrderId = order.ParentClOrderID;
                _loggedInUser = loggedInUser;
                decimal percantage = 0;
                _userPermitedAccountList = userAccountList;
                var distinctCLOrderIds = allocationDetails.AsEnumerable().DistinctBy(c => c[OrderFields.PROPERTY_PARENT_CL_ORDERID]).ToList();
                _orderGroupId = allocationDetails.Rows[0][OrderFields.CAPTION_GROUPID].ToString();
                _isGroupedOrder = distinctCLOrderIds.Count > 1;
                if (_isGroupedOrder)
                {
                    DataTable groupData = new DataTable();
                    groupData.Columns.Add(BlotterConstants.CAPTION_ORDER_ID, typeof(string));
                    groupData.Columns.Add(OrderFields.PROPERTY_QUANTITY, typeof(decimal));
                    for (int i = 0; i < distinctCLOrderIds.Count; i++)
                    {
                        DataRow row = groupData.NewRow();
                        row[BlotterConstants.CAPTION_ORDER_ID] = distinctCLOrderIds[i][OrderFields.PROPERTY_PARENT_CL_ORDERID];
                        row[OrderFields.PROPERTY_QUANTITY] = distinctCLOrderIds[i][BlotterConstants.CAPTION_ORDER_QTY];
                        groupData.Rows.Add(row);
                    }
                    grdGroupDetails.DataSource = groupData;
                    this.flowLayoutPanel1.Controls.Add(this.panel3);
                    this.flowLayoutPanel1.Controls.Add(this.panel2);
                    this.flowLayoutPanel1.Controls.Add(this.grdGroupDetails);
                    btnAllocate.Visible = false;
                    btnClear.Visible = false;
                }
                else
                {
                    this.flowLayoutPanel1.Controls.Add(this.panel2);
                    this.gridViewAllocation.DisplayLayout.Override.AllowAddNew = AllowAddNew.TemplateOnBottom;
                    this.gridViewAllocation.Size = new System.Drawing.Size(727, 300);


                    DataColumn colAllocPercent = new DataColumn(BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE, typeof(decimal));
                    colAllocPercent.DefaultValue = 0;
                    allocationDetails.Columns.Add(colAllocPercent);

                    DataColumn colAllocQty = new DataColumn(BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY, typeof(decimal));
                    colAllocQty.DefaultValue = 0;
                    allocationDetails.Columns.Add(colAllocQty);

                    allocationDetails.Columns[OrderFields.PROPERTY_PERCENTAGE].DefaultValue = 0;
                    allocationDetails.Columns[OrderFields.CAPTION_ALLOCATEDQTY].DefaultValue = 0;
                }

                //Get Distinct Rows
                var distinctRows = allocationDetails.AsEnumerable().DistinctBy(c => c[OrderFields.CAPTION_GROUPID]).ToList();

                var startOfDayQuantity = (decimal)order.DayOrderQty;

                //Update total quantity value
                distinctRows.ForEach(x => { totalQuantity += Convert.ToDecimal(x[OrderFields.PROPERTY_QUANTITY]); });

                //If the Order is unallocated then the fund id is coming null 
                if (allocationDetails.Rows[0][BlotterConstants.CAPTION_FUND_ID] == DBNull.Value)
                {
                    _executedQuantity = Convert.ToDecimal(allocationDetails.Rows[0][OrderFields.PROPERTY_EXECUTED_QTY].ToString());
                    percantage = totalQuantity != 0.0m ? (_executedQuantity * 100) / totalQuantity : 0;
                }

                allocationDetails.Columns.Add(OrderFields.PROPERTY_ACCOUNT, typeof(string));
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_PARENT_CL_ORDERID);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_EXECUTED_QTY);
                allocationDetails.Columns.Remove(BlotterConstants.CAPTION_ORDER_QTY);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_AVGPRICE);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_FXRATE);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_ASSET_ID);
                allocationDetails.Columns.Remove(OrderFields.PROPERTY_CURRENCYID);

                DataRow[] result = allocationDetails.Select(BlotterConstants.CAPTION_FUND_ID + " is null");
                foreach (DataRow row in result)
                {
                    row.Delete();
                }
                _gridData = allocationDetails.DefaultView.ToTable(true);

                //Expression that calculate % on Total quantity and Round upto 4 Decimal places
                string expression = "Convert(((" + OrderFields.CAPTION_ALLOCATEDQTY + " / " + totalQuantity + ") * 100) , 'System.Decimal') ";

                DataColumn colPercTotalQty = new DataColumn(BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY, typeof(decimal), expression);
                colPercTotalQty.DefaultValue = 0;
                //Add Percentage Column on Total Qty
                _gridData.Columns.Add(colPercTotalQty);


                //Update Remaining Qty and percentage
                _gridData.AsEnumerable().ToList().ForEach(row =>
                {
                    _executedQuantity += Convert.ToDecimal(row[OrderFields.CAPTION_ALLOCATEDQTY]);
                    percantage += Convert.ToDecimal(row[BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY]);
                });

                List<int> selectedAccounts = new List<int>();
                for (int i = 0; i < _gridData.Rows.Count; i++)
                {
                    int accountID = Convert.ToInt32(_gridData.Rows[i][BlotterConstants.CAPTION_FUND_ID]);
                    selectedAccounts.Add(accountID);
                    _gridData.Rows[i][OrderFields.PROPERTY_ACCOUNT] = userAccountList[accountID];
                }

                foreach (int accountID in userAccountList.Keys)
                {
                    if (!selectedAccounts.Contains(accountID))
                        vlAccount.ValueListItems.Add(accountID, userAccountList[accountID]);
                }
                //Bind Grid Data Source
                gridViewAllocation.DataSource = _gridData;

                this.flowLayoutPanel1.Controls.Add(this.gridViewAllocation);

                decimal totalQty = 0;
                decimal remainingQty = 0;
                var dayExecutedQty = (decimal)order.DayCumQty;
                if (order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD)
                {
                    totalQuantity = startOfDayQuantity;
                    totalQty = Convert.ToDecimal(order.Quantity);
                    startOfDayQuantity = totalQuantity == 0 ? Convert.ToDecimal(order.DayOrderQty) : totalQuantity;
                    remainingQty = Math.Round((startOfDayQuantity - dayExecutedQty), 4);
                }
                else
                {
                    //Check Total Quantity is 0 then update with order quantity
                    totalQty = totalQuantity == 0 ? Convert.ToDecimal(order.Quantity) : totalQuantity;
                    remainingQty = Math.Round((totalQty - _executedQuantity), 4);
                }
 
                //Update Label Data on UI
                ultraLabelSymbolName.Text = order.Symbol;
                ultraLabelQuantity.Text = totalQty.ToString(ApplicationConstants.FORMAT_QTY);
                ultraLabelStartOfDayQuantity.Text = startOfDayQuantity.ToString(ApplicationConstants.FORMAT_QTY);
                ultraLabelRemainingPerc.Text = Math.Round((100 - percantage), 2) + "%";
                ultraLabelRemainingQty.Text = remainingQty.ToString(ApplicationConstants.FORMAT_QTY);
                if (remainingQty == 0)
                {
                    ultraLabelRemainingQty.Text = "0";
                    ultraLabelRemainingPerc.Text = "0.00%";
                }

                if(order.TIF != FIXConstants.TIF_GTC && order.TIF != FIXConstants.TIF_GTD)
                {
                    ultraLabelStartOfDayQuantity.Visible = false;
                    ultraLabelStartOfDayOrderQuantity.Visible = false;
                }
                gridViewAllocation.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;

                Dictionary<int, string> allocationPreferences = Prana.Allocation.ClientLibrary.DataAccess.AllocationClientDataManager.GetInstance.GetAllocationPreferences(_loggedInUser.CompanyID, _loggedInUser.CompanyUserID, AllocationSubModulePermission.IsLevelingPermitted, AllocationSubModulePermission.IsProrataByNavPermitted);
                allocationPrefCmb.ValueList.ValueListItems.Add(-1, "Select");
                foreach (var allocationPrefKeyValuePair in allocationPreferences)
                {
                    allocationPrefCmb.ValueList.ValueListItems.Add(allocationPrefKeyValuePair.Key, allocationPrefKeyValuePair.Value);
                }
                allocationPrefCmb.Value = -1;

                _tradeDate = order.AUECLocalDate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnClear.ButtonStyle = UIElementButtonStyle.Button3D;
                btnClear.BackColor = Color.FromArgb(140, 5, 5);
                btnClear.ForeColor = Color.White;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = DefaultableBoolean.False;

                btnAllocate.ButtonStyle = UIElementButtonStyle.Button3D;
                btnAllocate.BackColor = Color.FromArgb(104, 156, 46);
                btnAllocate.ForeColor = Color.White;
                btnAllocate.UseAppStyling = false;
                btnAllocate.UseOsThemes = DefaultableBoolean.False;

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
        /// Handles the InitializeLayout event of the gridViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void gridViewAllocation_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ValueList = vlAccount;
                e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_FUND_ID].Hidden = true;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].Header.VisiblePosition = 0;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].MinWidth = 120;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTAGE].Header.Caption = BlotterConstants.CAPTION_ALLOCATION_PERCENTAGE;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTAGE].Format = ApplicationConstants.FORMAT_QTY_TWO_DIGIT_PRECISION;
                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTAGE].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                e.Layout.Bands[0].Columns[OrderFields.CAPTION_ALLOCATEDQTY].Header.Caption = BlotterConstants.CAPTION_ALLOCATED_QUANTITY;
                e.Layout.Bands[0].Columns[OrderFields.CAPTION_ALLOCATEDQTY].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns[OrderFields.CAPTION_ALLOCATEDQTY].Format = ApplicationConstants.FORMAT_QTY_TWO_DIGIT_PRECISION;
                e.Layout.Bands[0].Columns[OrderFields.CAPTION_ALLOCATEDQTY].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY].Header.VisiblePosition = 1;
                e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY].Format = ApplicationConstants.FORMAT_QTY_TWO_DIGIT_PRECISION;
                this.gridViewAllocation.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.RowsAndCells;

                e.Layout.Bands[0].Columns[OrderFields.PROPERTY_QUANTITY].Hidden = true;
                e.Layout.Bands[0].Columns[OrderFields.CAPTION_GROUPID].Hidden = true;

                e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[OrderFields.PROPERTY_PERCENTAGE]).DisplayFormat = ApplicationConstants.SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION;
                e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[OrderFields.CAPTION_ALLOCATEDQTY]).DisplayFormat = ApplicationConstants.SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION;
                e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_PERCENTAGE_ON_TOTAL_QTY]).DisplayFormat = ApplicationConstants.SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION;
                if (!_isGroupedOrder)
                {
                    e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.FromArgb(245, 250, 255);
                    e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;
                    e.Layout.Override.AddRowAppearance.BackColor = Color.LightYellow;
                    e.Layout.Override.AddRowAppearance.ForeColor = Color.Blue;
                    e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.TemplateAddRow;
                    e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = SystemColors.Control;
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE].CellAppearance.BackColor = Color.Gray;
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE].PromptChar = ' ';
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].CellAppearance.BackColor = Color.Gray;
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].PromptChar = ' ';
                    e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].Format = ApplicationConstants.FORMAT_QTY_TWO_DIGIT_PRECISION;
                    e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE]).DisplayFormat = ApplicationConstants.SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION;
                    e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY]).DisplayFormat = ApplicationConstants.SUMMARY_DISPLAY_FORMAT_QTY_TWO_DIGIT_PRECISION;
                }

                e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AfterExitEditMode event of the gridViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void gridViewAllocation_AfterExitEditMode(object sender, System.EventArgs e)
        {
            try
            {
                if (gridViewAllocation.ActiveCell != null)
                {
                    SetStatusBarText();
                    switch (gridViewAllocation.ActiveCell.Column.Key)
                    {
                        case OrderFields.PROPERTY_ACCOUNT:
                            if (gridViewAllocation.ActiveCell.IgnoreRowColActivation && !string.IsNullOrWhiteSpace(gridViewAllocation.ActiveCell.Value.ToString()))
                            {
                                gridViewAllocation.ActiveCell.IgnoreRowColActivation = false;
                            }
                            break;
                        case BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE:
                            if (!string.IsNullOrEmpty(gridViewAllocation.ActiveCell.Value.ToString()))
                            {
                                decimal allocPercent = Convert.ToDecimal(gridViewAllocation.ActiveCell.Value);
                                decimal allocQty = (allocPercent * _executedQuantity) / 100m;
                                gridViewAllocation.ActiveRow.Cells[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].SetValue(allocQty, true);
                            }
                            break;
                        case BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY:
                            if (!string.IsNullOrEmpty(gridViewAllocation.ActiveCell.Value.ToString()))
                            {
                                if (_executedQuantity > 0.0m)
                                {
                                    decimal allocQty = Convert.ToDecimal(gridViewAllocation.ActiveCell.Value);
                                    decimal allocPercent = (allocQty * 100m) / _executedQuantity;
                                    gridViewAllocation.ActiveRow.Cells[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE].SetValue(allocPercent, true);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the InitializeTemplateAddRow event of the gridViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeTemplateAddRowEventArgs"/> instance containing the event data.</param>
        private void gridViewAllocation_InitializeTemplateAddRow(object sender, InitializeTemplateAddRowEventArgs e)
        {
            try
            {
                e.TemplateAddRow.Cells[OrderFields.PROPERTY_ACCOUNT].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                e.TemplateAddRow.Cells[OrderFields.PROPERTY_ACCOUNT].IgnoreRowColActivation = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (gridViewAllocation.ActiveRow != null)
                {
                    string accountName = gridViewAllocation.ActiveRow.Cells[OrderFields.PROPERTY_ACCOUNT].Value.ToString();
                    if (_userPermitedAccountList.ContainsValue(accountName))
                    {
                        int accountID = _userPermitedAccountList.First(x => x.Value.Equals(accountName)).Key;
                        vlAccount.ValueListItems.Add(accountID, accountName);
                    }
                    gridViewAllocation.ActiveRow.Delete(false);
                    _gridData.AcceptChanges();
                    if (!_gridData.HasErrors)
                        SetStatusBarText();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Opening event of the mnuAllocationGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void mnuAllocationGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                mnuAllocationGrid.Items[0].Enabled = gridViewAllocation.ActiveRow == null ? false : true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                for (int i = _gridData.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = _gridData.Rows[i];
                    dr[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE] = 0;
                    dr[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY] = 0;

                    string accountName = dr[OrderFields.PROPERTY_ACCOUNT].ToString();
                    if (!_userPermitedAccountList.ContainsValue(accountName))
                    {
                        dr.Delete();
                    }
                }
                _gridData.AcceptChanges();
                SetStatusBarText();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the BeforeRowUpdate event of the gridViewAllocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelableRowEventArgs"/> instance containing the event data.</param>
        private void gridViewAllocation_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            try
            {
                if (e.Row.IsAddRow)
                {
                    UltraGridCell cell = e.Row.Cells[OrderFields.PROPERTY_ACCOUNT];
                    if (string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        string msg = "Please select a valid " + cell.Column.Header.Caption.ToString();
                        SetStatusBarText(msg);
                        // MessageBox.Show(this, msg, "Nirvana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                    else
                    {
                        SetStatusBarText();
                        for (int index = 0; index < vlAccount.ValueListItems.Count; index++)
                        {
                            ValueListItem item = vlAccount.ValueListItems[index];
                            if (item.DisplayText.Equals(cell.Text))
                            {
                                vlAccount.ValueListItems.Remove(item);
                                cell.Value = item.DisplayText;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAllocate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAllocate_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(_tradeDate))
                {
                    MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                        + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (ValidateControlsBeforeAllocate())
                {
                    AllocationResponse response = null;
                    if ((int)allocationPrefCmb.Value == -1)
                    {
                        if (_gridData.Rows.Count > 0)
                        {
                            SerializableDictionary<int, AccountValue> targetPct = new SerializableDictionary<int, AccountValue>();
                            decimal totalPct = 0.0m;
                            foreach (DataRow row in _gridData.Rows)
                            {
                                decimal quantityPct = (decimal)row[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE];
                                if (quantityPct > 0)
                                {
                                    string accountName = row[OrderFields.PROPERTY_ACCOUNT].ToString();
                                    int accountID = _userPermitedAccountList.First(x => x.Value.Equals(accountName)).Key;
                                    targetPct.Add(accountID, new AccountValue(accountID, quantityPct));
                                    totalPct += quantityPct;
                                }
                            }

                            if (!EqualsPrecise(totalPct, 100M))
                            {
                                string msg = "The total allocation(%) across accounts should be 100%";
                                SetStatusBarText(msg);
                                //MessageBox.Show(this, msg, "Nirvana Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                                SetStatusBarText();

                            AllocationParameter allocationParameter = new AllocationParameter(new AllocationRule()
                            {
                                BaseType = AllocationBaseType.CumQuantity,
                                RuleType = MatchingRuleType.None,
                                MatchClosingTransaction = MatchClosingTransactionType.None,
                                PreferenceAccountId = -1,
                                ProrataAccountList = new List<int>(),
                                ProrataDaysBack = 0,
                            }, targetPct, -1, _loggedInUser.CompanyUserID, true);

                            response = Prana.Allocation.ClientLibrary.DataAccess.AllocationClientDataManager.GetInstance.ReallocateGroup_Blotter(_orderGroupId, allocationParameter, int.MinValue, _loggedInUser.CompanyUserID, _clOrderId);
                        }
                    }
                    else
                    {
                        response = Prana.Allocation.ClientLibrary.DataAccess.AllocationClientDataManager.GetInstance.ReallocateGroup_Blotter(_orderGroupId, null, (int)allocationPrefCmb.Value, _loggedInUser.CompanyUserID, _clOrderId);
                    }
                    if (response != null && UpdateAuditAndStatusBar != null)
                    {
                        UpdateAuditAndStatusBar(null, new EventArgs<AllocationResponse>(response));
                        this.FindForm().Close();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Equals Precise
        /// </summary>
        /// <param name="val"></param>
        /// <param name="decimalValue"></param>
        /// <returns></returns>
        public bool EqualsPrecise(decimal val, decimal decimalValue)
        {
            try
            {
                return Math.Round(val, 10) == Math.Round(decimalValue, 10);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;


        }
        /// <summary>
        /// Handles the ValueChanged event of the allocationPrefCmb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void allocationPrefCmb_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (CheckIfErrorExists((UltraComboEditor)sender) && this.allocationPrefCmb.Value != null)
                {
                    SetStatusBarText();
                    int comboValue;
                    bool result = Int32.TryParse(this.allocationPrefCmb.Value.ToString(), out comboValue);
                    if (result)
                        btnClear.Enabled = gridViewAllocation.Enabled = comboValue != -1 ? false : true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Leave event of the allocationPrefCmb control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allocationPrefCmb_Leave(object sender, System.EventArgs e)
        {
            try
            {
                CheckIfErrorExists((UltraComboEditor)sender);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Check If any error exist
        /// </summary>
        /// <param name="ultraComboEditor"></param>
        /// <returns></returns>
        private bool CheckIfErrorExists(UltraComboEditor ultraComboEditor)
        {
            bool isValidated = true;
            try
            {
                if (ultraComboEditor.Value == null || !ValueListUtilities.CheckIfValueExistsInValuelist(ultraComboEditor.ValueList, ultraComboEditor.Value.ToString()))
                {
                    errorProvider.SetIconPadding(ultraComboEditor, -35);
                    errorProvider.SetError(ultraComboEditor, BlotterConstants.MSG_VALUE_IS_INVALID_FOR_FIELD);
                    isValidated = false;
                }
                else
                {
                    errorProvider.SetError(ultraComboEditor, String.Empty);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return isValidated;
        }

        /// <summary>
        /// Validate Controls Before Allocate
        /// </summary>
        /// <returns></returns>
        private bool ValidateControlsBeforeAllocate()
        {
            bool isValidated = true;
            try
            {
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                errorProvider.Clear();
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>())
                {
                    if (ucmbEditor.Value == null || !ValueListUtilities.CheckIfValueExistsInValuelist(ucmbEditor.ValueList, ucmbEditor.Value.ToString()))
                    {
                        errorProvider.SetIconPadding(ucmbEditor, -35);
                        errorProvider.SetError(ucmbEditor, BlotterConstants.MSG_VALUE_IS_INVALID_FOR_FIELD);
                        isValidated = false;
                    }
                }
                if (_gridData.HasErrors || !isValidated)
                {
                    isValidated = false;
                    SetStatusBarText("Validation failed. Please remove errors before allocating !!");
                }
                else
                    SetStatusBarText();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isValidated;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            IEnumerable<Control> enumerable = controls as IList<Control> ?? controls.ToList();
            return enumerable.SelectMany(ctrl => GetAll(ctrl, type)).Concat(enumerable).Where(c => c.GetType() == type);
        }

        /// <summary>
        /// Handles error when wrong account is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAllocation_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Row.ListObject;
                if (drv != null && drv.Row != null)
                {
                    if (_accountError)
                    {
                        drv.Row.RowError = "Account is invalid !";
                        _accountError = false;
                    }
                    else
                    {
                        drv.Row.RowError = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the value of_accountError after cell update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gridViewAllocation_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                this.gridViewAllocation.UpdateData();
                if (e.Cell.Column.Key == OrderFields.PROPERTY_ACCOUNT)
                {
                    int result;
                    bool ans = Int32.TryParse(e.Cell.OriginalValue.ToString(), out result);
                    if (!ans)
                    {
                        _accountError = true;
                        gridViewAllocation.ActiveRow.Cells[BlotterConstants.CAPTION_TARGET_ALLOCATION_PERCENTAGE].Activation = Activation.Disabled;
                        gridViewAllocation.ActiveRow.Cells[BlotterConstants.CAPTION_TARGET_ALLOCATION_QTY].Activation = Activation.Disabled;
                        return;
                    }
                    btnAllocate.Enabled = true;
                    _gridData.Rows[e.Cell.Row.Index][BlotterConstants.CAPTION_FUND_ID] = result;
                    _gridData.Rows[e.Cell.Row.Index][OrderFields.PROPERTY_ACCOUNT] = CachedDataManager.GetInstance.GetAccountText(result);
                    foreach (var nameGroup in vlAccount.ValueListItems)
                    {
                        if (Convert.ToInt32(nameGroup.DataValue) == result)
                        {
                            vlAccount.ValueListItems.Remove(nameGroup);
                            break;
                        }
                    }
                    gridViewAllocation.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ACCOUNT].ValueList = vlAccount;

                    UltraGridRow lastAddedrow = gridViewAllocation.Rows.Last();
                    lastAddedrow.Cells[0].Activation = Activation.NoEdit;
                }

                this.gridViewAllocation.UpdateData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SetStatusBarText
        /// </summary>
        /// <param name="message"></param>
        private void SetStatusBarText(string message = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                    this.toolStripStatusLabel1.Text = "[" + DateTime.Now + "] " + message;
                else
                    this.toolStripStatusLabel1.Text = message;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
