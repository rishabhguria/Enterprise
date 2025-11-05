using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.IO;
using System.Windows.Forms;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.LogManager;
using System.Collections.Generic;
using Prana.ClientCommon;
using Prana.Global;
using System.Linq;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects;
using System.Threading.Tasks;
using Prana.Utilities.MiscUtilities;
using static Prana.Global.ApplicationConstants;
using Prana.Admin.BLL;
using Infragistics.Win;
using System.Drawing;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class BatchDetails : Form, IPublishing, IDisposable
    {
        #region Column Titles
        private const string HEADCOL_CHECKBOX = "HEADERCHECK";
        private const string HEADCOL_SYMBOL = "SYMBOL";
        private const string HEADCOL_ISINSEDOLCUSIP = "ISINSEDOLCUSIP";
        private const string HEADCOL_CURRENCY = "CURRENCY";
        private const string HEADCOL_GROSSAMOUNT = "GROSSAMOUNT";
        private const string HEADCOL_SIDE = "SIDE";
        private const string HEADCOL_QUANTITY = "QUANTITY";
        private const string HEADCOL_TRADEDATE = "TRADEDATE";
        private const string HEADCOL_SETTLEMENTDATE = "SETTLEMENTDATE";
        private const string HEADCOL_AVERAGEPX = "AVERAGEPX";
        private const string HEADCOL_COMMISSION = "COMMISSION";
        private const string HEADCOL_NETAMOUNT = "NETAMOUNT";
        private const string HEADCOL_LASTUPDATED = "LASTUPDATED";
        private const string HEADCOL_MATCHSTATUS = "MATCHSTATUS";
        private const string HEADCOL_SUBSTATUS = "SUBSTATUS";
        private const string HEADCOL_ALLOCATIONID = "ALLOCATIONID";
        private const string HEADCOL_ACCOUNT = "ACCOUNT";
        private const string HEADCOL_MISCFEES = "MISCFEES";
        private const string HEADCOL_NETMONEY = "NETMONEY";
        private const string HEADCOL_ACCOUNTALLOCATIONID = "ACCOUNTALLOCATIONID";
        private const string HEADCOL_ITEM = "ITEM";
        private const string HEADCOL_NIRVANA = "NIRVANA";
        private const string HEADCOL_BROKER = "BROKER";
        #endregion
        private DuplexProxyBase<ISubscription> _subscriptionProxy;
        private readonly object _locker = new object();
        private DateTime _runDate;
        private ThirdPartyBatch _incomingBatch;
        public virtual event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked = null;

        public const string COLUMN_ALLOCATIONID = "AllocationID";
        public const string COLUMN_TRADEDATE = "TradeDate";

        public BatchDetails()
        {
            try
            {
                InitializeComponent();
                MakeProxy();
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
        /// Initializes the required attributes
        /// </summary>
        /// <param name="thirdPartyServiceProxy"></param>
        public void SetUp(DateTime runDate, ThirdPartyBatch thirdPartyBatch)
        {
            try
            {
                _runDate = runDate;
                _incomingBatch = thirdPartyBatch;
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
        /// Handles Load event of BatchDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchDetails_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Allocation Matching", CustomThemeHelper.UsedFont);
                CustomThemeHelper.SetThemeProperties(GrdBlockLevelDetails as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                CustomThemeHelper.SetThemeProperties(GrdAccountLevelDetails as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                CustomThemeHelper.SetThemeProperties(GrdAccountComparison as UltraGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY_CUSTOM);
                LoadGridData();
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
        /// This method is to load the grid data
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)(delegate ()
                        {
                            LoadGridData();
                        }));
                    }
                    else
                    {
                        GrdBlockLevelDetails.DataSource = null;
                        GrdAccountLevelDetails.DataSource = null;
                        GrdAccountComparison.DataSource = null;
                        GrdBlockLevelDetails.DataSource = ThirdPartyClientManager.ServiceInnerChannel.GetBlockAllocationDetails(_incomingBatch.ThirdPartyBatchId, _runDate);
                        GrdAccountLevelDetails.DataSource = new ThirdPartyAllocationLevelDetails();
                        GrdAccountComparison.DataSource = new ThirdPartyAllocationDetailComparison();
                        tsStatus.Text = string.Empty;
                    }
                }
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
        /// Handles InitializeLayout event of GrdBlockLevelDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdBlockLevelDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = GrdBlockLevelDetails.DisplayLayout.Bands[0];

                band.Override.AllowColMoving = AllowColMoving.Default;
                GrdBlockLevelDetails.DisplayLayout.Override.AllowColMoving = AllowColMoving.Default;
                GrdBlockLevelDetails.DisplayLayout.GroupByBox.Hidden = true;
                GrdBlockLevelDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                GrdBlockLevelDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                band.Columns.Add(HEADCOL_CHECKBOX, "");
                band.Columns[HEADCOL_CHECKBOX].DataType = typeof(bool);
                band.Columns[HEADCOL_CHECKBOX].CellClickAction = CellClickAction.EditAndSelectText;
                band.Columns[HEADCOL_CHECKBOX].Hidden = false;
                band.Columns[HEADCOL_CHECKBOX].Header.VisiblePosition = 0;
                band.Columns[HEADCOL_CHECKBOX].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[HEADCOL_CHECKBOX].SortIndicator = SortIndicator.Disabled;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                band.Columns[HEADCOL_CHECKBOX].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
                band.Columns[HEADCOL_CHECKBOX].SetHeaderCheckedState(GrdAccountLevelDetails.Rows, true);

                band.Columns[HEADCOL_SYMBOL].Header.Caption = "Symbol";
                band.Columns[HEADCOL_SYMBOL].Header.VisiblePosition = 1;
                band.Columns[HEADCOL_SYMBOL].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_ISINSEDOLCUSIP].Header.Caption = "ISIN/Sedol/CUSIP";
                band.Columns[HEADCOL_ISINSEDOLCUSIP].Header.VisiblePosition = 2;
                band.Columns[HEADCOL_ISINSEDOLCUSIP].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_CURRENCY].Header.Caption = "Currency";
                band.Columns[HEADCOL_CURRENCY].Header.VisiblePosition = 3;
                band.Columns[HEADCOL_CURRENCY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_GROSSAMOUNT].Header.Caption = "Gross Amount";
                band.Columns[HEADCOL_GROSSAMOUNT].Header.VisiblePosition = 4;
                band.Columns[HEADCOL_GROSSAMOUNT].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SIDE].Header.Caption = "Side";
                band.Columns[HEADCOL_SIDE].Header.VisiblePosition = 5;
                band.Columns[HEADCOL_SIDE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_QUANTITY].Header.Caption = "Quantity";
                band.Columns[HEADCOL_QUANTITY].Header.VisiblePosition = 6;
                band.Columns[HEADCOL_QUANTITY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_TRADEDATE].Header.Caption = "Trade Date";
                band.Columns[HEADCOL_TRADEDATE].Header.VisiblePosition = 7;
                band.Columns[HEADCOL_TRADEDATE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SETTLEMENTDATE].Header.Caption = "Settlement Date";
                band.Columns[HEADCOL_SETTLEMENTDATE].Header.VisiblePosition = 8;
                band.Columns[HEADCOL_SETTLEMENTDATE].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_AVERAGEPX].Header.Caption = "Average PX";
                band.Columns[HEADCOL_AVERAGEPX].Header.VisiblePosition = 9;
                band.Columns[HEADCOL_AVERAGEPX].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_COMMISSION].Header.Caption = "Commission";
                band.Columns[HEADCOL_COMMISSION].Header.VisiblePosition = 10;
                band.Columns[HEADCOL_COMMISSION].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_NETAMOUNT].Header.Caption = "Net Amount";
                band.Columns[HEADCOL_NETAMOUNT].Header.VisiblePosition = 11;
                band.Columns[HEADCOL_NETAMOUNT].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_LASTUPDATED].Header.Caption = "Last Updated";
                band.Columns[HEADCOL_LASTUPDATED].Header.VisiblePosition = 12;
                band.Columns[HEADCOL_LASTUPDATED].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_MATCHSTATUS].Header.Caption = "Match Status";
                band.Columns[HEADCOL_MATCHSTATUS].Header.VisiblePosition = 13;
                band.Columns[HEADCOL_MATCHSTATUS].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_SUBSTATUS].Header.Caption = "Sub Status";
                band.Columns[HEADCOL_SUBSTATUS].Header.VisiblePosition = 14;
                band.Columns[HEADCOL_SUBSTATUS].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_ALLOCATIONID].Header.Caption = "Allocation ID";
                band.Columns[HEADCOL_ALLOCATIONID].Header.VisiblePosition = 15;
                band.Columns[HEADCOL_ALLOCATIONID].CellActivation = Activation.NoEdit;

                UltraGridLayout layout = e.Layout;
                layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// Handles InitializeLayout event of GrdAccountLevelDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdAccountLevelDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = GrdAccountLevelDetails.DisplayLayout.Bands[0];

                band.Override.AllowColMoving = AllowColMoving.Default;
                GrdAccountLevelDetails.DisplayLayout.Override.AllowColMoving = AllowColMoving.Default;
                GrdAccountLevelDetails.DisplayLayout.GroupByBox.Hidden = true;
                GrdAccountLevelDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                GrdAccountLevelDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                band.Columns[HEADCOL_ACCOUNT].Header.Caption = "Account";
                band.Columns[HEADCOL_ACCOUNT].Header.VisiblePosition = 0;
                band.Columns[HEADCOL_ACCOUNT].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_COMMISSION].Header.Caption = "Commission";
                band.Columns[HEADCOL_COMMISSION].Header.VisiblePosition = 1;
                band.Columns[HEADCOL_COMMISSION].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_QUANTITY].Header.Caption = "Quantity";
                band.Columns[HEADCOL_QUANTITY].Header.VisiblePosition = 2;
                band.Columns[HEADCOL_QUANTITY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_MISCFEES].Header.Caption = "Misc Fees";
                band.Columns[HEADCOL_MISCFEES].Header.VisiblePosition = 3;
                band.Columns[HEADCOL_MISCFEES].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_AVERAGEPX].Header.Caption = "Average PX";
                band.Columns[HEADCOL_AVERAGEPX].Header.VisiblePosition = 4;
                band.Columns[HEADCOL_AVERAGEPX].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_NETMONEY].Header.Caption = "NetMoney";
                band.Columns[HEADCOL_NETMONEY].Header.VisiblePosition = 5;
                band.Columns[HEADCOL_NETMONEY].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_MATCHSTATUS].Header.Caption = "Match Status";
                band.Columns[HEADCOL_MATCHSTATUS].Header.VisiblePosition = 6;
                band.Columns[HEADCOL_MATCHSTATUS].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_ACCOUNTALLOCATIONID].Header.Caption = "Account Allocation ID";
                band.Columns[HEADCOL_ACCOUNTALLOCATIONID].Header.VisiblePosition = 7;
                band.Columns[HEADCOL_ACCOUNTALLOCATIONID].CellActivation = Activation.NoEdit;

                UltraGridLayout layout = e.Layout;
                layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// Handles InitializeLayout event of GrdAccountComparison
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdAccountComparison_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = GrdAccountComparison.DisplayLayout.Bands[0];

                band.Override.AllowColMoving = AllowColMoving.Default;
                GrdAccountComparison.DisplayLayout.Override.AllowColMoving = AllowColMoving.Default;
                GrdAccountComparison.DisplayLayout.GroupByBox.Hidden = true;
                GrdAccountComparison.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                GrdAccountComparison.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;

                band.Columns[HEADCOL_ITEM].Header.Caption = "Item";
                band.Columns[HEADCOL_ITEM].Header.VisiblePosition = 0;
                band.Columns[HEADCOL_ITEM].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_NIRVANA].Header.Caption = "Nirvana";
                band.Columns[HEADCOL_NIRVANA].Header.VisiblePosition = 1;
                band.Columns[HEADCOL_NIRVANA].CellActivation = Activation.NoEdit;

                band.Columns[HEADCOL_BROKER].Header.Caption = "Broker";
                band.Columns[HEADCOL_BROKER].Header.VisiblePosition = 2;
                band.Columns[HEADCOL_BROKER].CellActivation = Activation.NoEdit;

                UltraGridLayout layout = e.Layout;
                layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// Handles Click event of btnForceConfirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForceConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow[] rows = GrdBlockLevelDetails.Rows.GetFilteredInNonGroupByRows();
                List<ThirdPartyForceConfirm> force = new List<ThirdPartyForceConfirm>();
                bool isRowSelected = false;
                int rejectedBlocks = 0;
                int notYetProcessedBlocks = 0;
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        isRowSelected = true;
                        ThirdPartyBlockLevelDetails data = row.ListObject as ThirdPartyBlockLevelDetails;
                        if (data.MatchStatus.Equals(EnumHelper.GetDescriptionWithDescriptionAttribute(BlockMatchStatus.BlockLevelReject)))
                        {
                            rejectedBlocks++;
                        }
                        else
                        {
                            var details = ThirdPartyClientManager.ServiceInnerChannel.GetForceConfirmDetails(data.AllocationID, _incomingBatch.ThirdPartyName);
                            if (details.Count > 0)
                            {
                                details.ForEach(x => x.AllocId = data.AllocationID);
                                force.AddRange(details);
                            }
                            else
                            {
                                notYetProcessedBlocks++;
                            }
                        }
                    }
                }
                // If any row is not selected
                if (!isRowSelected)
                {
                    MessageBox.Show(this, "No row was selected. Please select a row to continue.", "Allocation Matching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (rejectedBlocks > 0)
                {
                    MessageBox.Show(this, "One or more blocks in selection are rejected by the broker which cannot be force matched.", "Allocation Matching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (notYetProcessedBlocks > 0)
                {
                    MessageBox.Show(this, "One or more blocks in selection are not yet confirmed by the broker and cannot be force matched.", "Allocation Matching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (force.Count > 0)
                {
                    ForceConfirm forceConfirm = new ForceConfirm(_incomingBatch);
                    forceConfirm.InitializeDataSource(force);
                    forceConfirm.ShowDialog();
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
        /// Handles Click event of btnRejectConfirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRejectConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow[] rows = GrdBlockLevelDetails.Rows.GetFilteredInNonGroupByRows();
                bool isRowSelected = false;
                int rejectedBlocks = 0;
                List<string> allocIds = new List<string>();
                Dictionary<string, string> allocIdAllocReportIdPairs = new Dictionary<string, string>();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        isRowSelected = true;
                        ThirdPartyBlockLevelDetails data = row.ListObject as ThirdPartyBlockLevelDetails;
                        if (data.MatchStatus.Equals(EnumHelper.GetDescriptionWithDescriptionAttribute(BlockMatchStatus.BlockLevelReject)))
                        {
                            rejectedBlocks++;
                        }
                        else
                        {
                            allocIds.Add(data.AllocationID);
                            allocIdAllocReportIdPairs[data.AllocationID] = string.Empty;
                        }
                    }
                }
                if (allocIds.Count > 0)
                {
                    bool isAuSent = ThirdPartyClientManager.ServiceInnerChannel.SendAUMsg(allocIds, _incomingBatch.CounterPartyID, (int)AffirmStatus.Rejected);
                    bool isATSent = ThirdPartyClientManager.ServiceInnerChannel.SendATMsg(allocIdAllocReportIdPairs, _incomingBatch.CounterPartyID, (int)BlockMatchStatus.Incomplete);
                    if (!isAuSent && !isATSent)
                    {
                        MessageBox.Show(this, "One or more blocks do not have allocation details needed for rejection.", "Reject Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (!isRowSelected)
                {
                    MessageBox.Show(this, "No row was selected. Please select a row to continue.", "Reject Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (rejectedBlocks > 0)
                    {
                        MessageBox.Show(this, "One or more blocks in selection are rejected by the broker which cannot be processed.", "Reject Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Handles Click event of btnForceConfirmAudit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForceConfirmAudit_Click(object sender, EventArgs e)
        {
            try
            {
                ForceConfirmAudit forceConfirmAudit = new ForceConfirmAudit();
                forceConfirmAudit.InitializeDataSource(ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyForceConfirmAuditData(_incomingBatch.ThirdPartyBatchId, _runDate));
                forceConfirmAudit.ShowDialog();
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
        /// Handles Click event of btnRefresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
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
        /// Handles Click event of btnViewInAllocation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewInAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow[] rows = GrdBlockLevelDetails.Rows.GetFilteredInNonGroupByRows();
                List<string> groupIdsList = new List<string>();
                List<DateTime> tradeDates = new List<DateTime>();

                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[HEADCOL_CHECKBOX].Text.Equals("True"))
                    {
                        string groupID = row.Cells[COLUMN_ALLOCATIONID].Text.TrimEnd('N', 'R');
                        DateTime tradeDate;
                        if (DateTime.TryParse(row.Cells[COLUMN_TRADEDATE].Value?.ToString(), out tradeDate))
                        {
                            if (!tradeDates.Contains(tradeDate))
                                tradeDates.Add(tradeDate);

                            if (!groupIdsList.Contains(groupID))
                                groupIdsList.Add(groupID);
                        }
                    }
                }
                if (groupIdsList.Count > 0 && tradeDates.Count > 0)
                {
                    if (GoToAllocationClicked != null)
                        GoToAllocationClicked(nameof(BatchDetails), new EventArgs<string, DateTime, DateTime>(String.Join(",", groupIdsList), tradeDates.Min(), tradeDates.Max()));
                    else
                        tsStatus.Text = "Data Corrupted";
                }
                else
                    tsStatus.Text = "Please select at least one row";
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
        /// Handles Click event of btnExport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve all rows in the grid
                UltraGridRow[] rows = GrdBlockLevelDetails.Rows.GetFilteredInNonGroupByRows();

                if (rows.Length == 0)
                {
                    tsStatus.Text = "No data available for export.";
                    return;
                }

                // Get the user's Downloads folder path
                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
                // Generate a file name with the current date and time
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string csvFilePath = Path.Combine(downloadsPath, $"MismatchData_{timestamp}.csv");

                // Export to .csv
                using (var writer = new StreamWriter(csvFilePath))
                {
                    var columns = GrdBlockLevelDetails.DisplayLayout.Bands[0].Columns;

                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (i > 0) writer.Write(",");
                        writer.Write(columns[i].Header.Caption);
                    }
                    writer.WriteLine();

                    foreach (UltraGridRow row in rows)
                    {
                        for (int i = 0; i < columns.Count; i++)
                        {
                            if (i > 0) writer.Write(",");

                            // Handle special formatting for dates
                            string cellValue = row.Cells[columns[i].Key].Text;

                            if (DateTime.TryParse(cellValue, out DateTime dateValue))
                            {
                                cellValue = dateValue.ToString("yyyy-MM-dd");
                            }

                            // Escape values containing commas, quotes, or newlines
                            if (cellValue.Contains(",") || cellValue.Contains("\"") || cellValue.Contains("\n"))
                            {
                                cellValue = $"\"{cellValue.Replace("\"", "\"\"")}\""; // Escape double quotes
                            }

                            writer.Write(cellValue);
                        }
                        writer.WriteLine();
                    }
                }

                tsStatus.Text = ThirdPartyConstants.STATUS_EXPORT_SUCCESSFUL;
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
        /// This method is to load Allocation comparison grid after user clicks on account level detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdAccountLevelDetails_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                ThirdPartyAllocationLevelDetails batch = (ThirdPartyAllocationLevelDetails)e.Cell.Row.ListObject;

                GrdAccountComparison.DataSource = null;
                GrdAccountComparison.DataSource = batch.AllocationComparisons;
                GrdAccountComparison.Refresh();
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
        /// This method is to load Allocation Level Detail grid after user clicks on block level detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdBlockLevelDetails_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                ThirdPartyBlockLevelDetails blockLevelDetail = (ThirdPartyBlockLevelDetails)e.Cell.Row.ListObject;

                if (blockLevelDetail.AllocationLevelDetails.Count > 0)
                {
                    GrdAccountLevelDetails.DataSource = null;
                    GrdAccountLevelDetails.DataSource = blockLevelDetail.AllocationLevelDetails;
                    GrdAccountLevelDetails.Refresh();
                }
                else
                {
                    tsStatus.Text = "Account level details not available";
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

        private void MakeProxy()
        {
            try
            {
                _subscriptionProxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _subscriptionProxy.Subscribe(Topics.Topic_ThirdPartyAllocationMatchStatusUpdate, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = delegate { Publish(e, topicName); };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        lock (_locker)
                        {
                            object[] dataList = (object[])e.EventData;
                            switch (e.TopicName)
                            {
                                case Topics.Topic_ThirdPartyAllocationMatchStatusUpdate:
                                    var thirdPartyAllocationMatchDetails = (ThirdPartyAllocationMatchDetails)dataList[0];
                                    if (thirdPartyAllocationMatchDetails != null && thirdPartyAllocationMatchDetails.ThirdPartyBatchId == _incomingBatch.ThirdPartyBatchId
                                        && thirdPartyAllocationMatchDetails.BatchRunDate.ToShortDateString().Equals(_runDate.ToShortDateString()))
                                    {
                                        tsStatus.Text = "The statuses on the screen are obsolete. Please refresh to review updated statuses.";
                                    }
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
                {
                    throw;
                }
            }
        }

        public string getReceiverUniqueName()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the MouseDown event of the  GrdBlockLevelDetails control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdBlockLevelDetails_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);

                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        GrdBlockLevelDetails.Selected.Rows.Clear();
                        cell.Row.Activate();
                        cell.Row.Selected = true;
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
        /// Handles click event of eventLogs control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EventLogs_Click(object sender, EventArgs e)
        {
            try
            {
                ThirdPartyBlockLevelDetails row = (ThirdPartyBlockLevelDetails)GrdBlockLevelDetails.ActiveRow.ListObject;
                EventLogs eventLogs = new EventLogs();
                eventLogs.LoadData(row.BlockDetailId);
                eventLogs.ShowDialog();
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
        /// This method is for initializing the Block Level Detail Grid Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdBlockLevelDetails_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("GrossAmount"))
                {
                    if (decimal.TryParse(e.Row.Cells["GrossAmount"].Value?.ToString(), out var grossAmount))
                    {
                        e.Row.Cells["GrossAmount"].Value = grossAmount.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("Quantity"))
                {
                    if (decimal.TryParse(e.Row.Cells["Quantity"].Value?.ToString(), out var quantity))
                    {
                        e.Row.Cells["Quantity"].Value = quantity.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("AveragePX"))
                {
                    if (decimal.TryParse(e.Row.Cells["AveragePX"].Value?.ToString(), out var averagePX))
                    {
                        e.Row.Cells["AveragePX"].Value = averagePX.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("Commission"))
                {
                    if (decimal.TryParse(e.Row.Cells["Commission"].Value?.ToString(), out var commission))
                    {
                        e.Row.Cells["Commission"].Value = commission.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("NetAmount"))
                {
                    if (decimal.TryParse(e.Row.Cells["NetAmount"].Value?.ToString(), out var netAmount))
                    {
                        e.Row.Cells["NetAmount"].Value = netAmount.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.ListObject != null && ((ThirdPartyBlockLevelDetails)e.Row.ListObject).MatchStatus != null)
                    e.Row.Cells[HEADCOL_CHECKBOX].Hidden = ((ThirdPartyBlockLevelDetails)e.Row.ListObject).MatchStatus.Equals("Cancelled");
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
        /// This method is for initializing the Account Level Detail Grid Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdAccountLevelDetails_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("Commission"))
                {
                    if (decimal.TryParse(e.Row.Cells["Commission"].Value?.ToString(), out var commission))
                    {
                        e.Row.Cells["Commission"].Value = commission.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("Quantity"))
                {
                    if (decimal.TryParse(e.Row.Cells["Quantity"].Value?.ToString(), out var quantity))
                    {
                        e.Row.Cells["Quantity"].Value = quantity.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("MiscFees"))
                {
                    if (decimal.TryParse(e.Row.Cells["MiscFees"].Value?.ToString(), out var miscFees))
                    {
                        e.Row.Cells["MiscFees"].Value = miscFees.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("AveragePX"))
                {
                    if (decimal.TryParse(e.Row.Cells["AveragePX"].Value?.ToString(), out var averagePX))
                    {
                        e.Row.Cells["AveragePX"].Value = averagePX.ToString(FORMAT_QTY);
                    }
                }
                if (e.Row.Cells.Exists("NetMoney"))
                {
                    if (decimal.TryParse(e.Row.Cells["NetMoney"].Value?.ToString(), out var netMoney))
                    {
                        e.Row.Cells["NetMoney"].Value = netMoney.ToString(FORMAT_QTY);
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

        #region IDisposable members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_subscriptionProxy != null)
                    {
                        _subscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_ThirdPartyAllocationMatchStatusUpdate);
                        _subscriptionProxy.Dispose();
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    base.Dispose(disposing);
                }
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
        #endregion
    }
}
