using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Prana.ClientCommon;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UIUtilities;
using Prana.CommonDataCache;
using System.Xml.Serialization;
using System.IO;
using Prana.Utilities.XMLUtilities;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using System.Configuration;
using System.Xml;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessLogic;
using System.Text.RegularExpressions;
using Prana.AllocationNew.Allocation.UI.CostAdjustment;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.Utilities;

namespace Prana.AllocationNew
{
    public partial class CtrlAmendmend : UserControl, IDisposable
    {

        //Dictionary<string, KeyValuePair<string, DateTime>> _dictAmendments = new Dictionary<string, KeyValuePair<string, DateTime>>();
        //bool _isHeaderCheckBoxChecked = false;
        List<int> _accountIDUnlocked = new List<int>();
        StringBuilder _accountUnlocked = new StringBuilder();
        public CtrlAmendmend()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode())
            {
                LoadPreferences();
            }
        }
        public event EventHandler GetAuditClick;

        #region Grouped_Order_Columns

        const string CAPTION_OrderSideTagValue = "OrderSideTagValue";
        const string COL_OrderSide = "OrderSide";
        const string CAPTION_Side = "Side";
        const string COL_OrderType = "OrderType";
        const string CAPTION_OrderTypeTagValue = "OrderTypeTagValue";
        const string CAPTION_Symbol = "Symbol";
        const string COL_Venue = "Venue";
        const string CAPTION_Quantity = "Quantity";
        const string CAPTION_Asset = "Asset";
        const string COL_AvgPrice = "AvgPrice";
        const string CAPTION_AssetID = "AssetID";
        const string CAPTION_AssetName = "AssetName";
        const string CAPTION_UnderlyingID = "UnderlyingID";
        const string COL_UnderlyingName = "UnderlyingName";
        const string CAPTION_UnderLying = "Underlying";

        const string CAPTION_ExchangeID = "ExchangeID";
        const string CAPTION_ExchangeName = "ExchangeName";
        const string CAPTION_CurrencyID = "CurrencyID";
        const string CAPTION_CurrencyName = "CurrencyName";
        const string CAPTION_AUECID = "AUECID";
        const string CAPTION_TradingAccountID = "TradingAccountID";
        const string CAPTION_TradingAccountName = "TradingAccountName";
        const string CAPTION_CompanyUserName = "CompanyUserName";
        const string CAPTION_CounterPartyID = "CounterPartyID";
        const string COL_CounterPartyName = "CounterPartyName";
        const string CAPTION_COUNTERPARTY =ApplicationConstants.CONST_BROKER;
        const string CAPTION_VenueID = "VenueID";
        const string COL_CumQty = "CumQty";
        const string COL_AllocatedQty = "AllocatedQty";
        const string COL_COMPANYNAME = "CompanyName";

        const string CAPTION_TradeAttribute1 = "Trade Attribute 1";
        const string CAPTION_TradeAttribute2 = "Trade Attribute 2";
        const string CAPTION_TradeAttribute3 = "Trade Attribute 3";
        const string CAPTION_TradeAttribute4 = "Trade Attribute 4";
        const string CAPTION_TradeAttribute5 = "Trade Attribute 5";
        const string CAPTION_TradeAttribute6 = "Trade Attribute 6";

        const string COL_TradeAttribute1 = "TradeAttribute1";
        const string COL_TradeAttribute2 = "TradeAttribute2";
        const string COL_TradeAttribute3 = "TradeAttribute3";
        const string COL_TradeAttribute4 = "TradeAttribute4";
        const string COL_TradeAttribute5 = "TradeAttribute5";
        const string COL_TradeAttribute6 = "TradeAttribute6";

        const string CAPTION_Updated = "Updated";
        const string CAPTION_NotAllExecuted = "NotAllExecuted";
        const string CAPTION_GroupID = "GroupID";

        const string CAPTION_OtherFees = "OtherFees";
        const string COL_Level1Name = "Level1Name";
        const string CAPTION_COMMISSIONSOURCE = "Commission Source";
        const string COL_COMMISSIONSOURCE = "CommissionSource";
        const string CAPTION_SOFTCOMMISSIONSOURCE = "Soft Commission Source";
        const string COL_SOFTCOMMISSIONSOURCE = "SoftCommissionSource";
      
        const string CAPTION_AutoGrouped = "AutoGrouped";
        const string CAPTION_AllocatedEqualTotalQty = "AllocatedEqualTotalQty";
        const string CAPTION_StateID = "StateID";
        const string CAPTION_AllocationLevelList = "AllocationLevelList";
        const string CAPTION_IsCommissionCalculated = "IsCommissionCalculated";
        const string CAPTION_ProcessDate = "Process Date";
        const string CAPTION_OriginalPurchaseDate = "Original Purchase Date";
        const string CAPTION_AUECLocalDate = "Trade Date";

        const string COl_AUECLocalDate = "AUECLocalDate";
        const string COl_ProcessDate = "ProcessDate";
        const string COl_OriginalPurchaseDate = "OriginalPurchaseDate";
        const string CAPTION_AF_TaxLotID = "TaxLotID";
        const string CAPTION_Allocations = "Allocations";
        const string CAPTION_TaxlotQty = "TaxLotQty";
        const string CAPTION_Percentage = "Percentage";
        const string CAPTION_CreationDate = "CreationDate";
        const string COL_SettlementDate = "SettlementDate";
        const string CAPTION_ExpirationDate = "ExpirationDate";
        const string CAPTION_CommissionText = "CommissionText";
        const string CAPTION_OrderID = "ClOrderID";
        const string COL_FXRate = "FXRate";
        const string COL_FXConversionMethodOperator = "FXConversionMethodOperator";
        const string CAPTION_EXECUTED_QTY = "Executed Qty";
        const string CAPTION_AVERAGEPRICE = "Average Price";
        const string COL_ExternalTransId = "ExternalTransId";
        const string COL_LotID = "LotId";
        const string CAPTION_ExternalTransId = "External Transaction Id";
        const string CAPTION_LEVEL2NAME = "Strategy Name";
        const string CAPTION_COMPANYDESCRIPTION = "Company Description";
        //Added by Rahul on 20120207
        const string COL_DESCRIPTION = "Description";
        const string COL_DELTA = "Delta";
        const string COL_M2MPROFITLOSS = "M2MProfitLoss";
        const string COL_ACCRUEDINTEREST = "AccruedInterest";
        const string COL_SWAPPARAMETERS = "SwapParameters";
        const string COL_PRANAMSGTYPE = "PranaMsgType";
        const string CAP_PARENT = "Parent";
        const string COL_STRATEGY = "Level2Name";
        const string COL_COMPANYDESCRIPTION = "CompanyName";

        //Added by Rahul on 6,Sep'2012
        private const string COL_SEDOLSYMBOL = "SedolSymbol";
        private const string COL_BLOOMBERGSYMBOL = "BloombergSymbol";
        private const string COL_CUSIPSYMBOL = "CusipSymbol";
        private const string COL_ISINSYMBOL = "IsinSymbol";
        private const string COL_IDCOSYMBOL = "IDCOSymbol";
        private const string COL_OSISYMBOL = "OSISymbol";

        private const string CAP_SEDOLSYMBOL = "Sedol Symbol";
        private const string CAP_BLOOMBERGSYMBOL = "Bloomberg Symbol";
        private const string CAP_CUSIPSYMBOL = "Cusip Symbol";
        private const string CAP_ISINSYMBOL = "ISIN Symbol";
        private const string CAP_IDCOSYMBOL = "IDCO Symbol";
        private const string CAP_OSISYMBOL = "OSI Symbol";

        //change comment added for audit trail other columns already present new constants created
        private const string COL_CHANGECOMMENT = "ChangeComment";
        private const string COL_UNDERLYINGDELTA = "UnderlyingDelta";
        private const string COL_COMMISSIONAMOUNT = "CommissionAmt";
        private const string COL_COMMISSIONRATE = "CommissionRate";

        private const string COL_SOFTCOMMISSIONAMOUNT = "SoftCommissionAmt";
        private const string COL_SOFTCOMMISSIONRATE = "SoftCommissionRate";
        private const string COL_TransactionType = "TransactionType";

        const string CAPTION_ClosingStatus = "Closing Status";
        const string CAPTION_TransactionType = "Transaction Type";

        const string COL_ClosingStatus = "ClosingStatus";

        private const string COL_COMMISSIONPERSHARE = "CommissionPerShare";
        private const string CAP_COMMISSIONPERSHARE = "Commission/Share";

        private const string COL_TOTALCOMMISSION = "TotalCommission";
        private const string CAP_TOTALCOMMISSION = "Total Commission";

        private const string CAP_TOTALCOMMISSIONANDFEES = "Total Commission & Fees";

        private const string COL_FUNDNAME = "AccountName";
        private const string CAP_FUNDNAME = "Account Name";

        private const string COL_NAVLOCKSTATUS = "NavLockStatus";
        private const string CAP_NAVLOCKSTATUS = "Nav Lock Status";
        private const string COL_NirvanaProcessDate = "NirvanaProcessDate";

        private const string COL_TRANSACTIONSOURCE = "TransactionSource";
        private const string COL_INTERNALCOMMENTS = "InternalComments";
        private const string CAP_INTERNALCOMMENTS = "Internal Comments";

        private List<string> TradeStringFields = new List<string>();

        #endregion

        static string _startPath = string.Empty;
        int _userID = 0;

        public CompanyUser CurrentUser
        {
            set
            {
                _userID = value.CompanyUserID;
                ctrlRecalculate1.InitControl(_userID);
                ctrlAmendSingleGroup1.InitControl(_userID);
                ctrlAmendSingleGroup1.CloseEditSingleGroup += new EventHandler(ctrlAmendSingleGroup1_CloseEditSingleGroup);
                ctlTradeAttributes1.initControl(_userID);
            }
        }

        private List<string> _displayColumns = null;
        public List<string> GetDisplayColumns
        {
            get
            {
                if (_displayColumns == null)
                {
                    _displayColumns = DisplayableColumns();
                }
                return _displayColumns;
            }
        }

        void ctrlAmendSingleGroup1_CloseEditSingleGroup(object sender, EventArgs e)
        {
            try
            {
                if (this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed == false)
                {
                    this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Close();
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

        ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            set
            {
                _allocationServices = value;
                ctrlRecalculate1.AllocationServices = value;
                ctrlAmendSingleGroup1.AllocationServices = value;
                ctlTradeAttributes1.AllocationServices = value;
            }
        }

        ProxyBase<ICashManagementService> _cashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            set
            {
                _cashManagementServices = value;
                ctrlAmendSingleGroup1.CashManagementServices = value;
            }
        }

        ProxyBase<IClosingServices> _closingServices = null;
        public ProxyBase<IClosingServices> ClosingServices
        {
            set { _closingServices = value; }
        }

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                ctrlAmendSingleGroup1.SecurityMaster = value;
            }
        }

        void ctrlRecalculate1_DisplayMessage(object sender, EventArgs e)
        {
            try
            {
                DisplayErrorMessage();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void ctrlRecalculate1_BulkChangeOnGroupLevel(object sender, EventArgs e)
        {
            try
            {
                BulkChangeOnGroupLevel(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void ctrlRecalculate1_RecalculateCommission(object sender, EventArgs e)
        {
            try
            {
                RecalculateCommissionAndFees(sender, e);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Changes Enable satus of UIControls
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="elementStatus">Element Status Enabled or Disabled</param>
        public void ToggleUIElementsWithMessage(String message, Boolean elementStatus)
        {
            try
            {
                grdAmendmend.Enabled = elementStatus;
                grdCommission.Enabled = elementStatus;
                btnCancel.Enabled = elementStatus;
                btnSave.Enabled = elementStatus;
                btnExit.Enabled = elementStatus;

                if (elementStatus)
                {
                    grdAmendmend.ResumeRowSynchronization();
                    grdCommission.ResumeRowSynchronization();
                    grdCommission.EndUpdate();
                    grdAmendmend.EndUpdate();
                }
                else
                {
                    grdAmendmend.BeginUpdate();
                    grdCommission.BeginUpdate();
                    grdAmendmend.SuspendRowSynchronization();
                    grdCommission.SuspendRowSynchronization();
                }
                if (!String.IsNullOrEmpty(message))
                {
                    toolStripStatusLabel1.Text = message;
                    timerClear.Start();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Pins the Edit Trade Group UI and fill in the data
        /// </summary>
        /// <param name="group"></param>
        internal void EditGroupDetails(AllocationGroup group)
        {
            try
            {
                bool isAllocated = false;
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                {
                    isAllocated = true;
                }
                else
                {
                    isAllocated = false;
                }
                AllocationManager.GetInstance().DictUnsavedAdd(group.GroupID, (AllocationGroup)group.Clone());
                ctrlAmendSingleGroup1.ChangeEnableStatus(isAllocated);
                //Modified By amit
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3495
                ctrlAmendSingleGroup1.ChangeEnableStatusForExercise(group);
                ctrlAmendSingleGroup1.ChangeEnableStatusForClosedAndPartialClosed(group);
                ultraDockManager1.DockAreas[2].DockAreaPane.Pin();
                ctrlAmendSingleGroup1.lblSymbol.Focus();

                //The allocation group from _dictUnsaved is passed to the ctrlamend single group for original clone purpose only. 
                //Do not perform any chnages on the group, this will result in reference getting changed and locking problems also.
                UltraGridBand band = grdAmendmend.DisplayLayout.Bands[0];
                ValueList[] attribLists = new ValueList[] { (ValueList)band.Columns[COL_TradeAttribute1].ValueList, (ValueList)band.Columns[COL_TradeAttribute2].ValueList, (ValueList)band.Columns[COL_TradeAttribute3].ValueList, (ValueList)band.Columns[COL_TradeAttribute4].ValueList, (ValueList)band.Columns[COL_TradeAttribute5].ValueList, (ValueList)band.Columns[COL_TradeAttribute6].ValueList };
                ctrlAmendSingleGroup1.EditGroup(group, AllocationManager.GetInstance().DictunsavedCancelAmend[group.GroupID], attribLists);
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
        /// Unpins the Edit Trade Group UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlAmendSingleGroup1_UnpinEditSingleTradeControl(object sender, EventArgs e)
        {
            try
            {
                ultraDockManager1.DockAreas[2].DockAreaPane.Unpin();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Cancels the changes done on Edit UI by reverting them with the original IDs Do not call directly should be called only in after save completed worker or where the cancel has been pressed and user confirms with no
        /// </summary>
        internal void CancelEditChanges()
        {
            try
            {
                this.grdAmendmend.PerformAction(UltraGridAction.ExitEditMode);
                this.grdCommission.PerformAction(UltraGridAction.ExitEditMode);
                if (AllocationManager.GetInstance().DictUnsavedCount() > 0)
                {
                    AllocationManager.GetInstance().CancelEditChanges();
                    grdAmendmend.Refresh();
                    grdCommission.Refresh();
                }
                this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = true;
                AllocationManager.GetInstance().ClearDictionaryUnsaved();
                AllocationManager.GetInstance().ClearExercisedGroupsDictionary();
                //commented by omshiv, ACA Cleanup
                // AllocationManager.GetInstance().ClearACASymbolsDicitonary();
                toolStripStatusLabel1.Text = "";
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

        UltraGridBand _gridBandGroupedOrders = null;
        UltraGridBand _gridBandAllocationAccounts = null;
        UltraGridBand _gridBandUnallocatedOrders = null;

        private void grdCommission_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Added to fix column chooser, PRANA-4942
                e.Layout.UseFixedHeaders = true;
                BindSides();
                _gridBandGroupedOrders = grdCommission.DisplayLayout.Bands[0];
                //Remove column chooser from child band(Taxlots)
                grdCommission.DisplayLayout.Bands[1].Override.RowSelectors = DefaultableBoolean.False;
                UltraWinGridUtils.SetColumnsForColumnChooser(grdCommission, GetDisplayColumns);
                //created separate method to set visibility of columns,PRANA-11652
                UltraWinGridUtils.SetColumnsVisibility(grdCommission, GetDisplayColumns);
                // Asset Category column is not coming, so we are adding it as custom column.
                AddAssetCategoryColumn(grdCommission.DisplayLayout.Bands[0]);
                if (!grdCommission.DisplayLayout.Bands[0].Columns.Exists(COL_FUNDNAME))
                {
                    grdCommission.DisplayLayout.Bands[0].Columns.Add(COL_FUNDNAME);
                }
                //Added strategy column, PRANA-4526
                if (!grdCommission.DisplayLayout.Bands[0].Columns.Exists(COL_STRATEGY))
                {
                    grdCommission.DisplayLayout.Bands[0].Columns.Add(COL_STRATEGY);
                }
                if (!grdCommission.DisplayLayout.Bands[0].Columns.Exists(COL_NAVLOCKSTATUS))
                {
                    grdCommission.DisplayLayout.Bands[0].Columns.Add(COL_NAVLOCKSTATUS);
                }

                List<ColumnData> commissionColumns = _cancelAmendPref.GrdCommissionColumns;
                if (commissionColumns.Count != 0)
                {
                    SetGridColumnLayout(grdCommission, commissionColumns, _displayColumns);
                }
                else
                {
                    SetGridColumns(_gridBandGroupedOrders, true, false);
                }

                SetColumnCustomization(_gridBandGroupedOrders);
                // Chnaging the caption to the Broker, PRANA-13231
                if (grdCommission.DisplayLayout.Bands[0].Columns.Exists("CounterPartyName"))
                    grdCommission.DisplayLayout.Bands[0].Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;
                #region
                // To Restore the splitter Position
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-879
                List<string> panelSize = GeneralUtilities.GetListFromString(_cancelAmendPref.SplitPanelSize, ',');
                if (panelSize != null)
                {
                    if (panelSize.Count == 3)
                    {
                        int x = Convert.ToInt32(panelSize[0]);
                        int y = Convert.ToInt32(panelSize[1]);
                        int position = Convert.ToInt32(panelSize[2]);
                        splitter1.Location = new System.Drawing.Point(x, y);
                        splitter1.SplitPosition = position;
                    }
                }
                #endregion

                _gridBandAllocationAccounts = grdCommission.DisplayLayout.Bands["TaxLots"];
                _gridBandGroupedOrders = grdCommission.DisplayLayout.Bands["Orders"];
                _gridBandGroupedOrders.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                _gridBandAllocationAccounts.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                _gridBandAllocationAccounts.Columns[COL_CHANGECOMMENT].MaxLength = 200;
                e.Layout.Bands[0].Columns[COL_OrderSide].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                e.Layout.Bands[0].Columns[COL_TransactionType].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdCommission.DisplayLayout.Bands[0].Columns[COL_CHANGECOMMENT].MaxLength = 200;
                if (!grdCommission.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdCommission, headerCheckBoxCommission);
                }
                SetColors(grdCommission);
                SetAllocationAccountsColumns(_gridBandAllocationAccounts);
                AddSettlementCurrencyFields(grdCommission.DisplayLayout.Bands[0]);
                AddSettlementCurrencyFields(_gridBandGroupedOrders);
                AddSettlementCurrencyFields(_gridBandAllocationAccounts);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        // Added by Ankit Gupta on 10 Sep, 2014
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1416
        private void SetColumnCustomization(UltraGridBand gridBand)
        {
            //For release type CH quantity, trade date along with all other dates will be editable.            
            try
            {
                if (CachedDataManager.GetPranaReleaseType().Equals(PranaReleaseViewType.CHMiddleWare))
                {
                    gridBand.Columns[OrderFields.PROPERTY_AUECLOCALDATE].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[COL_CumQty].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[OrderFields.PROPERTY_PROCESSDATE].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[COL_SettlementDate].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[OrderFields.PROPERTY_ORIGINAL_PURCHASEDATE].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[COL_OrderSide].CellActivation = Activation.AllowEdit;
                    gridBand.Columns[COL_OrderSide].ValueList = _sides.Clone();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void grdAmendmend_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Added to fix column chooser, PRANA-4942
                e.Layout.UseFixedHeaders = true;
                _gridBandUnallocatedOrders = grdAmendmend.DisplayLayout.Bands[0];
                UltraWinGridUtils.SetColumnsForColumnChooser(grdAmendmend, GetDisplayColumns);
                //created separate method to set visibility of columns,PRANA-11652
                UltraWinGridUtils.SetColumnsVisibility(grdAmendmend, GetDisplayColumns);
                List<ColumnData> amendColumns = _cancelAmendPref.GrdAmendColums;
                AddAssetCategoryColumn(grdAmendmend.DisplayLayout.Bands[0]);
                if (amendColumns.Count != 0)
                {
                    SetGridColumnLayout(grdAmendmend, amendColumns, _displayColumns);
                }
                else
                {
                    SetGridColumns(grdAmendmend.DisplayLayout.Bands[0], false, false);
                }
                AddSettlementCurrencyFields(grdAmendmend.DisplayLayout.Bands[0]);
                AddSettlementCurrencyFields(grdAmendmend.DisplayLayout.Bands["Taxlots"]);
                AddSettlementCurrencyFields(grdAmendmend.DisplayLayout.Bands["Orders"]);
                grdAmendmend.DisplayLayout.Bands["Taxlots"].Hidden = true;
                grdAmendmend.DisplayLayout.Bands["Orders"].Hidden = true;
                grdAmendmend.DisplayLayout.Bands["Orders"].Override.RowSelectors = DefaultableBoolean.False;
                grdAmendmend.DisplayLayout.Bands["Orders"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdAmendmend.DisplayLayout.Bands["TaxLots"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdAmendmend.DisplayLayout.Bands[0].Columns[COL_ClosingStatus].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdAmendmend.DisplayLayout.Bands[0].Columns[COL_CHANGECOMMENT].MaxLength = 200;
                // Chnaging the caption to the Broker, PRANA-13231
                if (grdAmendmend.DisplayLayout.Bands[0].Columns.Exists("CounterPartyName"))
                    grdAmendmend.DisplayLayout.Bands[0].Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;

                e.Layout.Bands[0].Columns[COL_TransactionType].CellActivation = Activation.AllowEdit;
                if (!grdAmendmend.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdAmendmend, headerCheckBoxAmendmend);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddAssetCategoryColumn(UltraGridBand gridband)
        {
            try
            {
                gridband.Columns.Add("AssetCategory", "Asset Category");
                UltraGridColumn colAssetCategory = gridband.Columns["AssetCategory"];
                colAssetCategory.Width = 80;
                colAssetCategory.CellActivation = Activation.NoEdit;
                gridband.Columns[CAPTION_AssetName].Hidden = true;
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

        CheckBoxOnHeader_CreationFilter headerCheckBoxCommission = new CheckBoxOnHeader_CreationFilter();
        CheckBoxOnHeader_CreationFilter headerCheckBoxAmendmend = new CheckBoxOnHeader_CreationFilter();
        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
                grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.Caption = String.Empty;
                SetCheckBoxAtFirstPosition(grid);
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

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
                grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        private void ultraExpandableGroupBox2_ExpandedStateChanging(object sender, CancelEventArgs e)
        {

        }
        internal void SetGridDataSources()
        {
            try
            {
                grdCommission.DataSource = AllocationManager.GetInstance().AllocatedGroups;
                grdAmendmend.DataSource = AllocationManager.GetInstance().UnAllocatedGroups;
                BindCounterParty();
                BindVenue();
                BindFxRateConvertor();
                BindCommissionSources();
                bindAttrbLists();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private ValueList _sides = new ValueList();
        private void BindSides()
        {
            try
            {
                _sides = new ValueList();
                _sides.ValueListItems.Add(FIXConstants.SIDE_Buy, "Buy");
                _sides.ValueListItems.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
                _sides.ValueListItems.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
                _sides.ValueListItems.Add(FIXConstants.SIDE_Sell, "Sell");
                _sides.ValueListItems.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
                _sides.ValueListItems.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
                _sides.ValueListItems.Add(FIXConstants.SIDE_SellShort, "Sell short");
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

        private void BindCommissionSources()
        {
            try
            {
                ValueList _commissionSources = new ValueList();
                ValueList _commissionSourcesAllocated = new ValueList();
                ValueList _softCommissionSources = new ValueList();
                ValueList _softCommissionSourcesAllocated = new ValueList();
                UltraGridColumn colCommSource = grdAmendmend.DisplayLayout.Bands[0].Columns[COL_COMMISSIONSOURCE];
                UltraGridColumn colSoftCommSource = grdAmendmend.DisplayLayout.Bands[0].Columns[COL_SOFTCOMMISSIONSOURCE];

                UltraGridColumn colCommSourceAllocated = grdCommission.DisplayLayout.Bands[0].Columns[COL_COMMISSIONSOURCE];
                UltraGridColumn colSoftCommSourceAllocated = grdCommission.DisplayLayout.Bands[0].Columns[COL_SOFTCOMMISSIONSOURCE];
                List<EnumerationValue> commSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.CommisionSource));
                List<EnumerationValue> softCommSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.CommisionSource));
                foreach (EnumerationValue var in commSource)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        _commissionSources.ValueListItems.Add(var.Value, var.DisplayText);
                        _commissionSourcesAllocated.ValueListItems.Add(var.Value, var.DisplayText);
                    }
                }
                colCommSource.ValueList = _commissionSources;
                colCommSourceAllocated.ValueList = _commissionSourcesAllocated;
                colCommSource.Hidden = true;
                colCommSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colCommSourceAllocated.Hidden = true;
                colCommSourceAllocated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                foreach (EnumerationValue var in softCommSource)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        _softCommissionSources.ValueListItems.Add(var.Value, var.DisplayText);
                        _softCommissionSourcesAllocated.ValueListItems.Add(var.Value, var.DisplayText);
                    }
                }
                colSoftCommSource.ValueList = _softCommissionSources;
                colSoftCommSourceAllocated.ValueList = _softCommissionSourcesAllocated;
                colSoftCommSource.Hidden = true;
                colSoftCommSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSoftCommSourceAllocated.Hidden = true;
                colSoftCommSourceAllocated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The counter party unallocated list
        /// </summary>
        ValueList _counterParty = new ValueList();

        /// <summary>
        /// The counter party allocted list
        /// </summary>
        ValueList _counterPartyAllocated = new ValueList();

        private void BindCounterParty()
        {
            try
            {
                _gridBandUnallocatedOrders = grdAmendmend.DisplayLayout.Bands[0];
                _gridBandGroupedOrders = grdCommission.DisplayLayout.Bands[0];

                UltraGridColumn colCounterParty = _gridBandUnallocatedOrders.Columns[COL_CounterPartyName];
                UltraGridColumn colCounterPartyAllocated = _gridBandGroupedOrders.Columns[COL_CounterPartyName];

                Dictionary<int, string> counterParties = CachedDataManager.GetInstance.GetUserCounterParties();
                foreach (int var in counterParties.Keys)
                {
                    _counterParty.ValueListItems.Add(counterParties[var], counterParties[var]);
                    _counterPartyAllocated.ValueListItems.Add(counterParties[var], counterParties[var]);
                }

                _counterParty.ValueListItems.Insert(0, string.Empty, ApplicationConstants.C_COMBO_SELECT);
                _counterPartyAllocated.ValueListItems.Insert(0, string.Empty, ApplicationConstants.C_COMBO_SELECT);
                colCounterParty.ValueList = _counterParty;
                colCounterPartyAllocated.ValueList = _counterPartyAllocated;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private ValueList _venue = new ValueList();
        private void BindVenue()
        {
            try
            {
                _gridBandUnallocatedOrders = grdAmendmend.DisplayLayout.Bands[0];
                UltraGridColumn colVenue = _gridBandUnallocatedOrders.Columns[COL_Venue];
                Dictionary<int, string> venues = Prana.CommonDataCache.CachedData.GetInstance().Venues;
                foreach (int var in venues.Keys)
                {
                    _venue.ValueListItems.Add(var, venues[var]);
                }
                colVenue.ValueList = _venue;
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


        private void BindFxRateConvertor()
        {
            try
            {
                ValueList fxConversionMethodOperatorList = new ValueList();
                ValueList fxConversionMethodOperatorList_Unallocated = new ValueList();

                _gridBandUnallocatedOrders = grdAmendmend.DisplayLayout.Bands[0];

                UltraGridColumn colFXConversionMethodOperator = _gridBandUnallocatedOrders.Columns[COL_FXConversionMethodOperator];
                UltraGridColumn colFXConversionMethodOperator_Allocated = _gridBandGroupedOrders.Columns[COL_FXConversionMethodOperator];
                UltraGridColumn colFXConversionMethodOperator_Taxlots = _gridBandAllocationAccounts.Columns[COL_FXConversionMethodOperator];

                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        fxConversionMethodOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                        fxConversionMethodOperatorList_Unallocated.ValueListItems.Add(var.Value, var.DisplayText);
                    }
                }

                colFXConversionMethodOperator.ValueList = fxConversionMethodOperatorList_Unallocated;
                colFXConversionMethodOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFXConversionMethodOperator_Allocated.ValueList = fxConversionMethodOperatorList;
                colFXConversionMethodOperator_Allocated.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFXConversionMethodOperator_Taxlots.ValueList = fxConversionMethodOperatorList;
                colFXConversionMethodOperator_Taxlots.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
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
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetGridColumns(UltraGridBand gridBand, bool IsAllocatedGrid, bool IsPrefSaved)
        {
            try
            {
                UltraGridColumn ColAssetName = gridBand.Columns[CAPTION_AssetName];
                ColAssetName.Width = 40;
                ColAssetName.Header.Caption = CAPTION_Asset;
                ColAssetName.CellActivation = Activation.NoEdit;
                ColAssetName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colCreationDate = gridBand.Columns[OrderFields.PROPERTY_AUECLOCALDATE];
                colCreationDate.Width = 80;
                colCreationDate.Header.Caption = CAPTION_AUECLocalDate;
                colCreationDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                colCreationDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colCreationDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colCreationDate.MaskInput = "mm/dd/yyyy";
                colCreationDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colProcessDate = gridBand.Columns[OrderFields.PROPERTY_PROCESSDATE];
                colProcessDate.Width = 80;
                colProcessDate.Header.Caption = CAPTION_ProcessDate;
                colProcessDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                colProcessDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colProcessDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colProcessDate.MaskInput = "mm/dd/yyyy";
                colProcessDate.CellActivation = Activation.NoEdit;
                colProcessDate.Hidden = true;

                UltraGridColumn colOriginalPurchaseDate = gridBand.Columns[OrderFields.PROPERTY_ORIGINAL_PURCHASEDATE];
                colOriginalPurchaseDate.Width = 80;
                colOriginalPurchaseDate.Header.Caption = CAPTION_OriginalPurchaseDate;
                colOriginalPurchaseDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                colOriginalPurchaseDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colOriginalPurchaseDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                colOriginalPurchaseDate.MaskInput = "mm/dd/yyyy";
                colOriginalPurchaseDate.CellActivation = Activation.NoEdit;
                colOriginalPurchaseDate.Hidden = true;

                UltraGridColumn colSymbol = gridBand.Columns[CAPTION_Symbol];
                colSymbol.Width = 50;
                colSymbol.CellActivation = Activation.NoEdit;

                UltraGridColumn colOrderSide = gridBand.Columns[COL_OrderSide];
                colOrderSide.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colOrderSide.Width = 50;
                colOrderSide.CellActivation = Activation.NoEdit;
                colOrderSide.Header.Caption = CAPTION_Side;
                colOrderSide.ValueList = _sides.Clone();

                UltraGridColumn colCounterPartyName = gridBand.Columns[COL_CounterPartyName];
                colCounterPartyName.Width = 80;
                colCounterPartyName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCounterPartyName.Header.Caption = CAPTION_COUNTERPARTY;
                colCounterPartyName.CellActivation = Activation.AllowEdit;
                colCounterPartyName.Hidden = true;

                UltraGridColumn colQuantity = gridBand.Columns[COL_CumQty];
                colQuantity.Header.Caption = CAPTION_EXECUTED_QTY;
                colQuantity.Width = 60;
                colQuantity.CellActivation = Activation.NoEdit;
                colQuantity.Format = ApplicationConstants.FORMAT_QTY;

                UltraGridColumn colAvgPrice = gridBand.Columns[COL_AvgPrice];
                colAvgPrice.Width = 80;
                colAvgPrice.Header.Caption = CAPTION_AVERAGEPRICE;
                colAvgPrice.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colAvgPrice.CellActivation = Activation.AllowEdit;
                colAvgPrice.Format = ApplicationConstants.FORMAT_COSTBASIS;

                UltraGridColumn colFxRate = gridBand.Columns[COL_FXRate];
                colFxRate.Width = 80;
                colFxRate.CellActivation = Activation.AllowEdit;
                colFxRate.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colFxRate.Hidden = true;
                colFxRate.Header.Caption = "FX Rate";

                UltraGridColumn colFXConversionMethodOperator = gridBand.Columns[COL_FXConversionMethodOperator];
                colFXConversionMethodOperator.Width = 20;
                colFXConversionMethodOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFXConversionMethodOperator.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colFXConversionMethodOperator.Header.Caption = "FX Conversion Operator";
                colFXConversionMethodOperator.CellActivation = Activation.AllowEdit;
                colFXConversionMethodOperator.Hidden = true;

                UltraGridColumn colSETTLEMENTCURRENCY = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSETTLEMENTCURRENCY.Width = 80;
                colSETTLEMENTCURRENCY.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSETTLEMENTCURRENCY.Hidden = true;
                colSETTLEMENTCURRENCY.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCY;

                UltraGridColumn colSettCurrFXRate = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRate];
                colSettCurrFXRate.Width = 80;
                colSettCurrFXRate.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSettCurrFXRate.Hidden = true;
                colSettCurrFXRate.Header.Caption = OrderFields.CAPTION_SettCurrFXRate;

                UltraGridColumn colSettCurrFXRateCalc = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRateCalc];
                colSettCurrFXRateCalc.Width = 80;
                colSettCurrFXRateCalc.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSettCurrFXRateCalc.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSettCurrFXRateCalc.Hidden = true;
                colSettCurrFXRateCalc.Header.Caption = OrderFields.CAPTION_SettCurrFXRateCalc;

                UltraGridColumn colSETTLEMENTCURRENCYAMOUNT = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT];
                colSETTLEMENTCURRENCYAMOUNT.Width = 80;
                colSETTLEMENTCURRENCYAMOUNT.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSETTLEMENTCURRENCYAMOUNT.Hidden = true;
                colSETTLEMENTCURRENCYAMOUNT.Header.Caption = OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT;

                UltraGridColumn colVenue = gridBand.Columns[COL_Venue];
                colVenue.Width = 55;
                colVenue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colVenue.CellActivation = Activation.NoEdit;
                colVenue.Hidden = true;

                UltraGridColumn colSettlementDate = gridBand.Columns[COL_SettlementDate];
                colSettlementDate.Width = 55;
                colSettlementDate.CellActivation = Activation.NoEdit;
                colSettlementDate.Hidden = true;
                colSettlementDate.Header.Caption = "Settlement Date";

                UltraGridColumn colExpirationDate = gridBand.Columns[CAPTION_ExpirationDate];
                colExpirationDate.Width = 55;
                colExpirationDate.CellActivation = Activation.NoEdit;
                colExpirationDate.Hidden = true;
                colExpirationDate.Header.Caption = "Expiration Date";

                UltraGridColumn colUnderlyingName = gridBand.Columns[COL_UnderlyingName];
                colUnderlyingName.Header.Caption = CAPTION_UnderLying;
                colUnderlyingName.CellActivation = Activation.NoEdit;
                colUnderlyingName.Hidden = true;

                UltraGridColumn colTargetQuantity = gridBand.Columns[CAPTION_Quantity];
                colTargetQuantity.CellActivation = Activation.NoEdit;
                colTargetQuantity.Hidden = true;
                colTargetQuantity.Format = ApplicationConstants.FORMAT_QTY;

                UltraGridColumn colAllocatedQty = gridBand.Columns[COL_AllocatedQty];
                colAllocatedQty.Width = 60;
                colAllocatedQty.Header.Caption = "Allocated Qty";
                colAllocatedQty.CellActivation = Activation.NoEdit;
                colAllocatedQty.Format = ApplicationConstants.FORMAT_QTY;

                UltraGridColumn colUnallocatedQty = gridBand.Columns["UnAllocatedQty"];
                colUnallocatedQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colUnallocatedQty.Hidden = true;
                colUnallocatedQty.Format = ApplicationConstants.FORMAT_QTY;

                UltraGridColumn colContractMultiplier = gridBand.Columns["ContractMultiplier"];
                colContractMultiplier.Width = 55;
                colContractMultiplier.CellActivation = Activation.NoEdit;
                colContractMultiplier.Hidden = true;
                colContractMultiplier.Header.Caption = "Multiplier";

                UltraGridColumn colUnderlyingSymbol = gridBand.Columns["UnderlyingSymbol"];
                colUnderlyingSymbol.CellActivation = Activation.NoEdit;
                colUnderlyingSymbol.Hidden = true;
                colUnderlyingSymbol.Header.Caption = "Underlying Symbol";

                UltraGridColumn colCompanyName = gridBand.Columns[COL_COMPANYNAME];
                colCompanyName.CellActivation = Activation.NoEdit;
                colCompanyName.Hidden = true;
                colCompanyName.Header.Caption = "Company Name";

                UltraGridColumn colPutOrCall = gridBand.Columns["PutOrCalls"];
                colPutOrCall.CellActivation = Activation.NoEdit;
                colPutOrCall.Hidden = true;
                colPutOrCall.Header.Caption = "Put/Call";
                colPutOrCall.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn colExchangeName = gridBand.Columns[CAPTION_ExchangeName];
                colExchangeName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colExchangeName.Header.Caption = "Exchange";
                colExchangeName.CellActivation = Activation.NoEdit;
                colExchangeName.Hidden = true;

                UltraGridColumn colCurrencyName = gridBand.Columns[CAPTION_CurrencyName];
                colCurrencyName.CellActivation = Activation.NoEdit;
                colCurrencyName.Hidden = true;
                colCurrencyName.Header.Caption = "Currency";

                UltraGridColumn colTradeAtt1 = gridBand.Columns[COL_TradeAttribute1];
                colTradeAtt1.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute1); ;
                colTradeAtt1.CellActivation = Activation.AllowEdit;
                colTradeAtt1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;

                UltraGridColumn colTradeAtt2 = gridBand.Columns[COL_TradeAttribute2];
                colTradeAtt2.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute2); ;
                colTradeAtt2.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt3 = gridBand.Columns[COL_TradeAttribute3];
                colTradeAtt3.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute3); ;
                colTradeAtt3.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt4 = gridBand.Columns[COL_TradeAttribute4];
                colTradeAtt4.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute4); ;
                colTradeAtt4.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt5 = gridBand.Columns[COL_TradeAttribute5];
                colTradeAtt5.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute5); ;
                colTradeAtt5.CellActivation = Activation.AllowEdit;

                UltraGridColumn colTradeAtt6 = gridBand.Columns[COL_TradeAttribute6];
                colTradeAtt6.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute6); ;
                colTradeAtt6.CellActivation = Activation.AllowEdit;

                UltraGridColumn colClosingStatus = gridBand.Columns[COL_ClosingStatus];
                colClosingStatus.Header.Caption = CAPTION_ClosingStatus;
                colClosingStatus.CellActivation = Activation.NoEdit;

                if (gridBand.Columns.Exists(COL_FUNDNAME))
                {
                    UltraGridColumn colAccountName = gridBand.Columns[COL_FUNDNAME];
                    colAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = CAP_FUNDNAME;
                }
                //Added strategy column, PRANA-4526
                if (gridBand.Columns.Exists(COL_STRATEGY))
                {
                    UltraGridColumn colAccountName = gridBand.Columns[COL_STRATEGY];
                    colAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = CAPTION_LEVEL2NAME;
                }
                if (gridBand.Columns.Exists(COL_NAVLOCKSTATUS))
                {
                    UltraGridColumn colAccountName = gridBand.Columns[COL_NAVLOCKSTATUS];
                    colAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = CAP_NAVLOCKSTATUS;
                }

                UltraGridColumn colCommissionPerShare = gridBand.Columns[COL_COMMISSIONPERSHARE];
                colCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                colCommissionPerShare.Header.Caption = CAP_COMMISSIONPERSHARE;

                UltraGridColumn colSoftCommissionPerShare = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSIONPERSHARE];
                colSoftCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                colSoftCommissionPerShare.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSIONPERSHARE;

                UltraGridColumn colTotalCommissionPerShare = gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONPERSHARE];
                colTotalCommissionPerShare.Width = 80;
                colTotalCommissionPerShare.Header.Caption = OrderFields.CAPTION_TOTALCOMMISSIONPERSHARE;
                colTotalCommissionPerShare.CellActivation = Activation.NoEdit;
                colTotalCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colTotalCommission = gridBand.Columns[COL_TOTALCOMMISSION];
                colTotalCommission.Width = 80;
                colTotalCommission.Header.Caption = CAP_TOTALCOMMISSION;
                colTotalCommission.CellActivation = Activation.NoEdit;
                colTotalCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colOptionPremiumAdjustment = gridBand.Columns[OrderFields.PROPERTY_OptionPremiumAdjustment];
                colOptionPremiumAdjustment.Header.Caption = OrderFields.CAPTION_OptionPremiumAdjustment;
                //Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3455
                colOptionPremiumAdjustment.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingAlgo = gridBand.Columns[OrderFields.PROPERTY_ClosingAlgoText];
                colClosingAlgo.Header.Caption = OrderFields.CAPTION_ClosingAlgo;

                if (gridBand.Columns.Exists(OrderFields.PROPERTY_CHANGETYPE))
                {
                    UltraGridColumn colChangeType = gridBand.Columns[OrderFields.PROPERTY_CHANGETYPE];
                    colChangeType.Width = 80;
                    colChangeType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colChangeType.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colChangeType.Hidden = true;
                    colChangeType.Header.Caption = OrderFields.CAPTION_CHANGETYPE;
                }

                UltraGridColumn colTotalCommissionAndFees;
                UltraGridColumn colCommission;
                UltraGridColumn colSoftCommission;
                UltraGridColumn colFees;
                UltraGridColumn colStampDuty;
                UltraGridColumn colTransactionLevy;
                UltraGridColumn colClearingFee;
                UltraGridColumn colTaxonCommission;
                UltraGridColumn colMiscFees;
                UltraGridColumn colSecFee;
                UltraGridColumn colOccFee;
                UltraGridColumn colOrfFee;
                UltraGridColumn colClearingBrokerFee;

                if (IsAllocatedGrid)
                {
                    //commission calculation time is false
                    colTotalCommissionAndFees = gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES];
                    colTotalCommissionAndFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTotalCommissionAndFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                    colTotalCommissionAndFees.Width = 60;

                    colCommission = gridBand.Columns[OrderFields.PROPERTY_COMMISSION];
                    colCommission.Width = 60;
                    colCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                    colCommission.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                    colSoftCommission.Width = 60;
                    colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                    colSoftCommission.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colSoftCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colFees = gridBand.Columns[OrderFields.PROPERTY_OTHERBROKERFEES];
                    colFees.Width = 60;
                    colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colFees.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                    colClearingBrokerFee.Width = 60;
                    colClearingBrokerFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colClearingBrokerFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colClearingBrokerFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                    colStampDuty.Width = 60;
                    colStampDuty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colStampDuty.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colStampDuty.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                    colTransactionLevy.Width = 60;
                    colTransactionLevy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTransactionLevy.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colTransactionLevy.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colClearingFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                    colClearingFee.Width = 60;
                    colClearingFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colClearingFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colClearingFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colTaxonCommission = gridBand.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                    colTaxonCommission.Width = 60;
                    colTaxonCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTaxonCommission.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colTaxonCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                    colMiscFees.Width = 60;
                    colMiscFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colMiscFees.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colMiscFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                    colSecFee.Width = 60;
                    colSecFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colSecFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colSecFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                    colOccFee.Width = 60;
                    colOccFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colOccFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colOccFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                    colOrfFee.Width = 60;
                    colOrfFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colOrfFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colOrfFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                    if (!IsPrefSaved)
                    {
                        //ColAssetName.Header.VisiblePosition = 1;
                        colCreationDate.Header.VisiblePosition = 2;
                        colSymbol.Header.VisiblePosition = 3;
                        colOrderSide.Header.VisiblePosition = 4;
                        colQuantity.Header.VisiblePosition = 5;
                        colAvgPrice.Header.VisiblePosition = 6;
                        colTotalCommissionAndFees.Header.VisiblePosition = 7;
                        colCommission.Header.VisiblePosition = 8;
                        colSoftCommission.Header.VisiblePosition = 9;
                        colFees.Header.VisiblePosition = 10;
                        colStampDuty.Header.VisiblePosition = 11;
                        colTransactionLevy.Header.VisiblePosition = 12;
                        colClearingFee.Header.VisiblePosition = 13;
                        colTaxonCommission.Header.VisiblePosition = 14;
                        colMiscFees.Header.VisiblePosition = 15;
                        colSecFee.Header.VisiblePosition = 16;
                        colOccFee.Header.VisiblePosition = 17;
                        colOrfFee.Header.VisiblePosition = 18;
                        colClearingBrokerFee.Header.VisiblePosition = 19;
                    }
                    colTotalCommissionAndFees.CellActivation = Activation.NoEdit;
                }
                else
                {
                    //unallocated grid columns
                    colTotalCommissionAndFees = gridBand.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES];
                    colTotalCommissionAndFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTotalCommissionAndFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                    colTotalCommissionAndFees.Width = 60;

                    colCommission = gridBand.Columns[OrderFields.PROPERTY_COMMISSION];
                    colCommission.Width = 60;
                    colCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                    colSoftCommission.Width = 60;
                    colSoftCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colSoftCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colFees = gridBand.Columns[OrderFields.PROPERTY_OTHERBROKERFEES];
                    colFees.Width = 60;
                    colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                    colClearingBrokerFee.Width = 60;
                    colClearingBrokerFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colClearingBrokerFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                    colStampDuty.Width = 60;
                    colStampDuty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colStampDuty.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                    colTransactionLevy.Width = 60;
                    colTransactionLevy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTransactionLevy.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colClearingFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                    colClearingFee.Width = 60;
                    colClearingFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colClearingFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colTaxonCommission = gridBand.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                    colTaxonCommission.Width = 60;
                    colTaxonCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colTaxonCommission.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                    colMiscFees.Width = 60;
                    colMiscFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colMiscFees.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                    colSecFee.Width = 60;
                    colSecFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colSecFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                    colOccFee.Width = 60;
                    colOccFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colOccFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    colOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                    colOrfFee.Width = 60;
                    colOrfFee.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colOrfFee.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    //editable column for amendment Grid
                    colQuantity.CellActivation = Activation.AllowEdit;
                    colOrderSide.CellActivation = Activation.AllowEdit;
                    colCounterPartyName.CellActivation = Activation.AllowEdit;
                    colVenue.CellActivation = Activation.AllowEdit;
                    colCreationDate.CellActivation = Activation.AllowEdit;
                    colProcessDate.CellActivation = Activation.AllowEdit;
                    colSettlementDate.CellActivation = Activation.AllowEdit;
                    colExpirationDate.CellActivation = Activation.NoEdit;
                    colOriginalPurchaseDate.CellActivation = Activation.AllowEdit;
                    colAvgPrice.CellActivation = Activation.AllowEdit;
                    colFXConversionMethodOperator.CellActivation = Activation.AllowEdit;
                    colTotalCommissionAndFees.CellActivation = Activation.NoEdit;
                    colSETTLEMENTCURRENCY.CellActivation = Activation.AllowEdit;
                    colSettCurrFXRate.CellActivation = Activation.AllowEdit;
                    colSettCurrFXRateCalc.CellActivation = Activation.AllowEdit;
                    colSETTLEMENTCURRENCYAMOUNT.CellActivation = Activation.AllowEdit;

                    colAllocatedQty.Hidden = true;
                    colAllocatedQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    colTargetQuantity.Hidden = true;
                    colTargetQuantity.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    if (!IsPrefSaved)
                    {
                        colCreationDate.Header.VisiblePosition = 2;
                        colSymbol.Header.VisiblePosition = 3;
                        colOrderSide.Header.VisiblePosition = 4;
                        colCounterPartyName.Header.VisiblePosition = 5;
                        colQuantity.Header.VisiblePosition = 6;
                        colAvgPrice.Header.VisiblePosition = 7;
                        colSettlementDate.Header.VisiblePosition = 8;
                        colExpirationDate.Header.VisiblePosition = 9;
                    }
                }

                //common for both grids
                colTaxonCommission.Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                colMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                colTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                colStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                colFees.Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                colClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                colTotalCommissionAndFees.Header.Caption = CAP_TOTALCOMMISSIONANDFEES;
                colSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                colOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                colOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                colCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;

                gridBand.Columns[COL_DESCRIPTION].Hidden = true;
                gridBand.Columns[COL_INTERNALCOMMENTS].Hidden = true;
                gridBand.Columns[COL_INTERNALCOMMENTS].Header.Caption = CAP_INTERNALCOMMENTS;
                gridBand.Columns[COL_INTERNALCOMMENTS].CellActivation = Activation.AllowEdit;

                UltraGridColumn colAccruedInterest = gridBand.Columns[COL_ACCRUEDINTEREST];
                colAccruedInterest.Hidden = true;
                colAccruedInterest.Header.Caption = "Accrued Interest";

                UltraGridColumn colCommissionSource = gridBand.Columns[COL_COMMISSIONSOURCE];
                colCommissionSource.Hidden = true;
                colCommissionSource.Header.Caption = CAPTION_COMMISSIONSOURCE;
                colCommissionSource.Width = 20;
                colCommissionSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCommissionSource.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colCommissionSource.Hidden = true;

                UltraGridColumn colSoftCommissionSource = gridBand.Columns[COL_SOFTCOMMISSIONSOURCE];
                colSoftCommissionSource.Hidden = true;
                colSoftCommissionSource.Header.Caption = CAPTION_SOFTCOMMISSIONSOURCE;
                colSoftCommissionSource.Width = 21;
                colSoftCommissionSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSoftCommissionSource.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSoftCommissionSource.Hidden = true;

                UltraGridColumn colCommissionAmt = gridBand.Columns[COL_COMMISSIONAMOUNT];
                colCommissionAmt.Hidden = true;
                colCommissionAmt.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colSoftCommissionAmt = gridBand.Columns[COL_SOFTCOMMISSIONAMOUNT];
                colSoftCommissionAmt.Hidden = true;
                colSoftCommissionAmt.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colCommissionRate = gridBand.Columns[COL_COMMISSIONRATE];
                colCommissionRate.Hidden = true;
                colCommissionRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colSoftCommissionRate = gridBand.Columns[COL_SOFTCOMMISSIONRATE];
                colSoftCommissionRate.Hidden = true;
                colSoftCommissionRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colSedolSymbol = gridBand.Columns[COL_SEDOLSYMBOL];
                colSedolSymbol.CellActivation = Activation.NoEdit;
                colSedolSymbol.Header.Caption = CAP_SEDOLSYMBOL;
                colSedolSymbol.Hidden = true;

                UltraGridColumn colBloombergSymbol = gridBand.Columns[COL_BLOOMBERGSYMBOL];
                colBloombergSymbol.CellActivation = Activation.NoEdit;
                colBloombergSymbol.Header.Caption = CAP_BLOOMBERGSYMBOL;
                colBloombergSymbol.Hidden = true;

                UltraGridColumn colCusipSymbol = gridBand.Columns[COL_CUSIPSYMBOL];
                colCusipSymbol.CellActivation = Activation.NoEdit;
                colCusipSymbol.Header.Caption = CAP_CUSIPSYMBOL;
                colCusipSymbol.Hidden = true;

                UltraGridColumn colISINSymbol = gridBand.Columns[COL_ISINSYMBOL];
                colISINSymbol.CellActivation = Activation.NoEdit;
                colISINSymbol.Header.Caption = CAP_ISINSYMBOL;
                colISINSymbol.Hidden = true;

                UltraGridColumn colOSISymbol = gridBand.Columns[COL_OSISYMBOL];
                colOSISymbol.CellActivation = Activation.NoEdit;
                colOSISymbol.Hidden = true;
                colOSISymbol.Header.Caption = CAP_OSISYMBOL;

                UltraGridColumn colIDCOSymbol = gridBand.Columns[COL_IDCOSYMBOL];
                colIDCOSymbol.CellActivation = Activation.NoEdit;
                colIDCOSymbol.Hidden = true;
                colIDCOSymbol.Header.Caption = CAP_IDCOSYMBOL;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> DisplayableColumns()
        {
            List<string> lst = new List<string>();
            try
            {
                lst.Add("AccruedInterest");
                lst.Add("AllocatedQty");
                lst.Add("AllocationSchemeName");
                lst.Add("AssetCategory");
                lst.Add(OrderFields.PROPERTY_CLEARINGFEE);
                lst.Add(OrderFields.PROPERTY_MISCFEES);
                lst.Add("AccountName");
                lst.Add("AvgPrice");
                lst.Add("BloombergSymbol");
                lst.Add("ChangeComment");
                lst.Add(OrderFields.PROPERTY_CLEARINGBROKERFEE);
                lst.Add("ClosingStatus");
                lst.Add("ClosingDate");
                lst.Add(OrderFields.PROPERTY_COMMISSION);
                lst.Add(COL_COMMISSIONPERSHARE);
                lst.Add(OrderFields.PROPERTY_SOFTCOMMISSIONPERSHARE);
                lst.Add(OrderFields.PROPERTY_TOTALCOMMISSIONPERSHARE);
                lst.Add(COL_COMPANYDESCRIPTION);
                lst.Add("ContractMultiplier");
                lst.Add("CounterPartyName");
                lst.Add("CurrencyName");
                lst.Add("CusipSymbol");
                lst.Add("Delta");
                lst.Add("Description");
                lst.Add("ExchangeName");
                lst.Add("CumQty");
                lst.Add("ExpirationDate");
                lst.Add(COL_FXConversionMethodOperator);
                lst.Add(COL_FXRate);
                lst.Add("GroupStatus");
                lst.Add("IDCOSymbol");
                lst.Add(OrderFields.PROPERTY_OCCFEE);
                lst.Add(OrderFields.PROPERTY_ORFFEE);
                lst.Add("OriginalPurchaseDate");
                lst.Add("OSISymbol");
                lst.Add(OrderFields.PROPERTY_OTHERBROKERFEES);
                lst.Add("ProcessDate");
                lst.Add("ProxySymbol");
                lst.Add("PutOrCalls");
                lst.Add("Quantity");
                lst.Add(OrderFields.PROPERTY_SECFEE);
                lst.Add("SedolSymbol");
                lst.Add("SettlementDate");
                lst.Add("OrderSide");
                lst.Add(OrderFields.PROPERTY_SOFTCOMMISSION);
                lst.Add(OrderFields.PROPERTY_STAMPDUTY);
                lst.Add("Symbol");
                lst.Add(OrderFields.PROPERTY_TAXONCOMMISSIONS);
                lst.Add(OrderFields.PROPERTY_TOTALCOMMISSIONANDFEES);
                lst.Add(COL_TOTALCOMMISSION);
                lst.Add("TradeAttribute2");
                lst.Add("TradeAttribute3");
                lst.Add("TradeAttribute4");
                lst.Add("TradeAttribute5");
                lst.Add("TradeAttribute6");
                lst.Add("TradeAttribute1");
                lst.Add("AUECLocalDate");
                lst.Add("TradingAccountName");
                lst.Add(OrderFields.PROPERTY_TRANSACTIONLEVY);
                lst.Add("UnAllocatedQty");
                lst.Add("UnderlyingName");
                lst.Add("UnderlyingSymbol");
                lst.Add("CompanyUserName");
                lst.Add("Venue");
                lst.Add(CAP_NAVLOCKSTATUS);
                lst.Add(COL_NirvanaProcessDate);
                lst.Add(COL_TransactionType);
                lst.Add("InternalComments");
                lst.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                lst.Add(OrderFields.PROPERTY_SettCurrFXRate);
                lst.Add(OrderFields.PROPERTY_SettCurrFXRateCalc);
                lst.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT);
                lst.Add(OrderFields.PROPERTY_OptionPremiumAdjustment);
                lst.Add(OrderFields.PROPERTY_ClosingAlgoText);
                lst.Add(OrderFields.PROPERTY_CHANGETYPE);
                lst.Add("Level2Name");
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
            return lst;
        }

        private void SetColors(UltraGrid gridCommission)
        {
            try
            {
                UltraGridBand allocatedBand = grdCommission.DisplayLayout.Bands[0];
                UltraGridBand allocatedTaxlotBand = grdCommission.DisplayLayout.Bands["TaxLots"];

                List<string> commissionColumns = CommissionColumnList();
                foreach (string column in commissionColumns)
                {
                    allocatedBand.Columns[column].CellAppearance.BackColor = Color.DeepSkyBlue;
                    allocatedBand.Columns[column].CellAppearance.ForeColor = Color.Black;
                    allocatedBand.Columns[column].Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                    allocatedTaxlotBand.Columns[column].CellAppearance.BackColor = Color.DeepSkyBlue;
                    allocatedTaxlotBand.Columns[column].CellAppearance.ForeColor = Color.Black;
                    allocatedTaxlotBand.Columns[column].Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;
                }
                grdCommission.DisplayLayout.Bands["Orders"].Hidden = true;
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

        private List<string> CommissionColumnList()
        {
            List<string> lst = new List<string>();
            try
            {
                lst.Add(OrderFields.PROPERTY_COMMISSION);
                lst.Add(OrderFields.PROPERTY_SOFTCOMMISSION);
                lst.Add(OrderFields.PROPERTY_OTHERBROKERFEES);
                lst.Add(OrderFields.PROPERTY_MISCFEES);
                lst.Add(OrderFields.PROPERTY_TAXONCOMMISSIONS);
                lst.Add(OrderFields.PROPERTY_TRANSACTIONLEVY);
                lst.Add(OrderFields.PROPERTY_CLEARINGFEE);
                lst.Add(OrderFields.PROPERTY_STAMPDUTY);
                lst.Add(OrderFields.PROPERTY_SECFEE);
                lst.Add(OrderFields.PROPERTY_OCCFEE);
                lst.Add(OrderFields.PROPERTY_ORFFEE);
                lst.Add(OrderFields.PROPERTY_CLEARINGBROKERFEE);
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
            return lst;
        }

        /// <summary>
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetAllocationAccountsColumns(UltraGridBand gridBand)
        {
            try
            {
                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.Hidden = true;
                    if (VisibleColumnsList().Contains(column.Key))
                    {
                        column.Hidden = false;
                        column.Width = 80;
                    }
                }

                UltraGridColumn colAccount = gridBand.Columns[COL_Level1Name];
                colAccount.Header.Caption = "Account";
                colAccount.CellActivation = Activation.NoEdit;
                colAccount.Header.VisiblePosition = 0;

                UltraGridColumn colStrategy = gridBand.Columns[COL_STRATEGY];
                colStrategy.CellActivation = Activation.NoEdit;
                colStrategy.Header.VisiblePosition = 1;
                colStrategy.Header.Caption = CAPTION_LEVEL2NAME;

                UltraGridColumn colPercentage = gridBand.Columns[CAPTION_Percentage];
                colPercentage.CellActivation = Activation.NoEdit;
                colPercentage.Header.VisiblePosition = 2;

                UltraGridColumn colTaxlotQty = gridBand.Columns[CAPTION_TaxlotQty];
                colTaxlotQty.Header.Caption = "Taxlot Qty";
                colTaxlotQty.CellActivation = Activation.NoEdit;
                colTaxlotQty.Header.VisiblePosition = 3;

                UltraGridColumn colFXRate = gridBand.Columns[COL_FXRate];
                colFXRate.Header.Caption = "FX Rate";
                colFXRate.Header.VisiblePosition = 4;

                UltraGridColumn colFXConversionOperator = gridBand.Columns[COL_FXConversionMethodOperator];
                colFXConversionOperator.Header.Caption = "FX Conversion Operator";
                colFXConversionOperator.Header.VisiblePosition = 5;

                UltraGridColumn colCommission = gridBand.Columns[OrderFields.PROPERTY_COMMISSION];
                colCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                colCommission.Header.VisiblePosition = 6;

                UltraGridColumn colSoftCommission = gridBand.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                colSoftCommission.Header.VisiblePosition = 7;

                UltraGridColumn colOtherBrokerFees = gridBand.Columns[OrderFields.PROPERTY_OTHERBROKERFEES];
                colOtherBrokerFees.Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                colOtherBrokerFees.Header.VisiblePosition = 8;

                UltraGridColumn colClearingBrokerFee = gridBand.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                colClearingBrokerFee.Header.VisiblePosition = 9;

                UltraGridColumn colStampDuty = gridBand.Columns[OrderFields.PROPERTY_STAMPDUTY];
                colStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                colStampDuty.Header.VisiblePosition = 10;

                UltraGridColumn colTransactionLevy = gridBand.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                colTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                colTransactionLevy.Header.VisiblePosition = 11;

                UltraGridColumn colClearingFees = gridBand.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                colClearingFees.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                colClearingFees.Header.VisiblePosition = 12;

                UltraGridColumn colTaxOnComm = gridBand.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                colTaxOnComm.Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                colTaxOnComm.Header.VisiblePosition = 13;

                UltraGridColumn colMiscFees = gridBand.Columns[OrderFields.PROPERTY_MISCFEES];
                colMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                colMiscFees.Header.VisiblePosition = 14;

                UltraGridColumn colSecFee = gridBand.Columns[OrderFields.PROPERTY_SECFEE];
                colSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                colSecFee.Header.VisiblePosition = 15;

                UltraGridColumn colOccFee = gridBand.Columns[OrderFields.PROPERTY_OCCFEE];
                colOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                colOccFee.Header.VisiblePosition = 16;

                UltraGridColumn colOrfFee = gridBand.Columns[OrderFields.PROPERTY_ORFFEE];
                colOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                colOrfFee.Header.VisiblePosition = 17;

                UltraGridColumn colLotID = gridBand.Columns[COL_LotID];
                colLotID.Header.Caption = "LotID";
                colLotID.MaxLength = 100;
                colLotID.Header.VisiblePosition = 18;

                UltraGridColumn colExternalTransID = gridBand.Columns[COL_ExternalTransId];
                colExternalTransID.Header.Caption = CAPTION_ExternalTransId;
                colExternalTransID.CellActivation = Activation.ActivateOnly;
                colExternalTransID.Header.VisiblePosition = 19;

                UltraGridColumn colTradeAttribute1 = gridBand.Columns[COL_TradeAttribute1];
                colTradeAttribute1.Header.VisiblePosition = 20;
                colTradeAttribute1.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute1);

                UltraGridColumn colTradeAttribute2 = gridBand.Columns[COL_TradeAttribute2];
                colTradeAttribute2.Header.VisiblePosition = 21;
                colTradeAttribute2.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute2);

                UltraGridColumn colTradeAttribute3 = gridBand.Columns[COL_TradeAttribute3];
                colTradeAttribute3.Header.VisiblePosition = 22;
                colTradeAttribute3.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute3);

                UltraGridColumn colTradeAttribute4 = gridBand.Columns[COL_TradeAttribute4];
                colTradeAttribute4.Header.VisiblePosition = 23;
                colTradeAttribute4.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute4);

                UltraGridColumn colTradeAttribute5 = gridBand.Columns[COL_TradeAttribute5];
                colTradeAttribute5.Header.VisiblePosition = 24;
                colTradeAttribute5.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute5);

                UltraGridColumn colTradeAttribute6 = gridBand.Columns[COL_TradeAttribute6];
                colTradeAttribute6.Header.VisiblePosition = 25;
                colTradeAttribute6.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute6);

                UltraGridColumn colInternalComments = gridBand.Columns[COL_INTERNALCOMMENTS];
                colInternalComments.Header.VisiblePosition = 26;
                colInternalComments.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAP_INTERNALCOMMENTS);

                UltraGridColumn colSETTLEMENTCURRENCY = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSETTLEMENTCURRENCY.Header.VisiblePosition = 27;
                colSETTLEMENTCURRENCY.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SETTLEMENTCURRENCY);

                UltraGridColumn colSettCurrFXRate = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRate];
                colSettCurrFXRate.Header.VisiblePosition = 28;
                colSettCurrFXRate.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SettCurrFXRate);

                UltraGridColumn colSettCurrFXRateCalc = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRateCalc];
                colSettCurrFXRateCalc.Header.VisiblePosition = 29;
                colSettCurrFXRateCalc.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SettCurrFXRateCalc);

                UltraGridColumn colSETTLEMENTCURRENCYAMOUNT = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT];
                colSETTLEMENTCURRENCYAMOUNT.Header.VisiblePosition = 30;
                colSETTLEMENTCURRENCYAMOUNT.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT);



                UltraGridColumn colOptionPremiumAdjustment = gridBand.Columns[OrderFields.PROPERTY_OptionPremiumAdjustment];
                colOptionPremiumAdjustment.Header.Caption = OrderFields.CAPTION_OptionPremiumAdjustment;
                colOptionPremiumAdjustment.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingAlgo = gridBand.Columns[OrderFields.PROPERTY_ClosingAlgo];
                colClosingAlgo.Header.Caption = OrderFields.CAPTION_ClosingAlgo;
                colClosingAlgo.CellActivation = Activation.NoEdit;
                colClosingAlgo.ValueList = GetValueListForClosingAlgo();

                AddChangeType(gridBand);
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
        /// To add change type column
        /// </summary>
        /// <param name="gridBand"></param>
        private static void AddChangeType(UltraGridBand gridBand)
        {
            try
            {
                UltraGridColumn colChangeType = gridBand.Columns[OrderFields.PROPERTY_CHANGETYPE];
                colChangeType.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_CHANGETYPE);

                ValueList ChangeTypeList = new ValueList();
                List<EnumerationValue> ChangeTypeEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.ChangeType));
                foreach (EnumerationValue var in ChangeTypeEnumList)
                {
                    ChangeTypeList.ValueListItems.Add(var.Value, var.DisplayText);
                }
                colChangeType.ValueList = ChangeTypeList;
                colChangeType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colChangeType.CellActivation = Activation.NoEdit;
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
        /// Get Value List of Closing Algo
        /// </summary>
        /// <returns></returns>
        private ValueList GetValueListForClosingAlgo()
        {
            ValueList ClosingAlgoList = new ValueList();
            try
            {
                List<EnumerationValue> ClosingAlgoEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PostTradeEnums.CloseTradeAlogrithm));
                foreach (EnumerationValue var in ClosingAlgoEnumList)
                {
                    ClosingAlgoList.ValueListItems.Add(var.Value, var.DisplayText);
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
            return ClosingAlgoList;
        }

        private List<string> VisibleColumnsList()
        {
            List<string> visibleColList = new List<string>();

            try
            {
                visibleColList.Add(COL_Level1Name);
                visibleColList.Add(COL_STRATEGY);
                visibleColList.Add(CAPTION_Percentage);
                visibleColList.Add(CAPTION_TaxlotQty);
                visibleColList.Add(COL_FXRate);
                visibleColList.Add(COL_FXConversionMethodOperator);

                visibleColList.Add(OrderFields.PROPERTY_COMMISSION);
                visibleColList.Add(OrderFields.PROPERTY_SOFTCOMMISSION);
                visibleColList.Add(OrderFields.PROPERTY_OTHERBROKERFEES);
                visibleColList.Add(OrderFields.PROPERTY_CLEARINGBROKERFEE);
                visibleColList.Add(OrderFields.PROPERTY_STAMPDUTY);
                visibleColList.Add(OrderFields.PROPERTY_TRANSACTIONLEVY);
                visibleColList.Add(OrderFields.PROPERTY_CLEARINGFEE);
                visibleColList.Add(OrderFields.PROPERTY_TAXONCOMMISSIONS);
                visibleColList.Add(OrderFields.PROPERTY_MISCFEES);
                visibleColList.Add(OrderFields.PROPERTY_SECFEE);
                visibleColList.Add(OrderFields.PROPERTY_OCCFEE);
                visibleColList.Add(OrderFields.PROPERTY_ORFFEE);

                visibleColList.Add(COL_LotID);
                visibleColList.Add(COL_ExternalTransId);
                visibleColList.Add(COL_TradeAttribute1);
                visibleColList.Add(COL_TradeAttribute2);
                visibleColList.Add(COL_TradeAttribute3);
                visibleColList.Add(COL_TradeAttribute4);
                visibleColList.Add(COL_TradeAttribute5);
                visibleColList.Add(COL_TradeAttribute6);
                visibleColList.Add(COL_NAVLOCKSTATUS);
                visibleColList.Add(COL_ClosingStatus);
                visibleColList.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                visibleColList.Add(OrderFields.PROPERTY_SettCurrFXRate);
                visibleColList.Add(OrderFields.PROPERTY_SettCurrFXRateCalc);
                visibleColList.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT);
                visibleColList.Add(OrderFields.PROPERTY_ClosingAlgo);
                visibleColList.Add(OrderFields.PROPERTY_CHANGETYPE);
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
            return visibleColList;
        }
        /// <summary>
        /// This method is called once we change value of any Cell in a Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdCommission_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                CancelUpdateInSettlementColumn(e);
                //Added By faisal Shah
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1147
                if (e.Cell.Row.Cells[COL_NAVLOCKSTATUS].Value != null)
                    if (e.Cell.Row.Cells[COL_NAVLOCKSTATUS].Value.ToString().Equals("Locked"))
                    {
                        MessageBox.Show("You need to Unlock NavLock before making any amendments.\nPlease contact System Admin for the same", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.CancelUpdate();
                        return;
                    }
                    else if (e.Cell.Row.Cells[COL_NAVLOCKSTATUS].Value.ToString().Equals(ApplicationConstants.C_Multiple))
                    {
                        MessageBox.Show("You can make amendments after expanding the row only.\nPlease Expand the Row to see possible ammendable Rows", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.CancelUpdate();
                        return;
                    }
                bool isClosed = false;
                bool isExerised = false;
                bool isTotalCommissionChanged = false;
                bool isCostAdjusted = false;
                //Allow to save empty value in taxlotattributes at group level
                if (TradeStringFields.Contains(e.Cell.Column.Key) && string.IsNullOrEmpty(e.Cell.Text))
                {
                    e.Cell.Value = string.Empty;
                }

                //Allow to save empty value in taxlotattributes at TaxLot level
                if (e.Cell.Text != string.Empty || TradeStringFields.Contains(e.Cell.Column.Key))
                {
                    double updatedValue = 0.0;
                    bool temp = double.TryParse(e.Cell.Text, out updatedValue);
                    switch (e.Cell.Column.Key)
                    {
                        //columns which are not numbers
                        case COl_ProcessDate:
                        case COl_OriginalPurchaseDate:
                        case COl_AUECLocalDate:
                        case COL_SettlementDate:
                        case COL_FXConversionMethodOperator:
                        case COL_DESCRIPTION:
                        case COL_COMMISSIONSOURCE:
                        case COL_SOFTCOMMISSIONSOURCE:
                        case COL_LotID:
                        case COL_ExternalTransId:
                        case COL_TradeAttribute1:
                        case COL_TradeAttribute2:
                        case COL_TradeAttribute3:
                        case COL_TradeAttribute4:
                        case COL_TradeAttribute5:
                        case COL_TradeAttribute6:
                        case COL_CHANGECOMMENT:
                        case COL_CumQty:
                        case COL_OrderSide:
                        case COL_NirvanaProcessDate:
                        case COL_TransactionType:
                        case OrderFields.PROPERTY_SETTLEMENTCURRENCYID:
                        case OrderFields.PROPERTY_SettCurrFXRateCalc:
                        case OrderFields.PROPERTY_SettCurrFXRate:
                        case OrderFields.PROPERTY_CHANGETYPE:
                            temp = true;
                            break;
                        case COL_CounterPartyName:
                            temp = true;
                            if (e.Cell.Text.Equals(ApplicationConstants.C_COMBO_SELECT))
                                e.Cell.Value = string.Empty;
                            break;
                        default: break;
                    }

                    AllocationGroup gParent = null;

                    if (temp)
                    {
                        Type type = e.Cell.Row.ListObject.GetType();

                        if (type.Name == "TaxLot")
                        {
                            gParent = (AllocationGroup)e.Cell.Row.ParentRow.ListObject;
                            TaxLot taxlot = (TaxLot)e.Cell.Row.ListObject;
                            PostTradeEnums.Status GroupStatus = _closingServices.InnerChannel.CheckGroupStatus(gParent);

                            if (GroupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                            {
                                MessageBox.Show("Corporate Action has been Applied to this taxlot. First undo the corporate action to make any changes. ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (GroupStatus.Equals(PostTradeEnums.Status.Exercise))
                            {
                                MessageBox.Show("This Group is generated by Exercise.", "Warning", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            AllocationManager.GetInstance().DictUnsavedAdd(gParent.GroupID, (AllocationGroup)gParent.Clone());
                            gParent.CompanyUserID = _userID;

                            AllocationManager.GetInstance().UpdateFieldsForTaxLotCellChange(e.Cell.Column.Key, e.Cell.Text, gParent, taxlot);
                        }
                        else if (type.Name.Equals("AllocationGroup"))
                        {
                            gParent = (AllocationGroup)e.Cell.Row.ListObject;
                            bool isGroupAllocatedToOneAccount = false;
                            //Allow to change the fields like side,trade date and quantity only if group is allocated only in one account
                            switch (e.Cell.Column.Key)
                            {
                                case COl_ProcessDate:
                                case COl_OriginalPurchaseDate:
                                case COl_AUECLocalDate:
                                case COL_SettlementDate:
                                case COL_CumQty:
                                case COL_OrderSide:
                                    isGroupAllocatedToOneAccount = gParent.IsGroupAllocatedToOneTaxLot;
                                    break;
                                default:
                                    isGroupAllocatedToOneAccount = true;
                                    break;
                            }

                            if (!isGroupAllocatedToOneAccount)
                            {
                                MessageBox.Show("Value can be changed for the trade which is allocated to one account only.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }

                            PostTradeEnums.Status GroupStatus = _closingServices.InnerChannel.CheckGroupStatus(gParent);

                            switch (GroupStatus)
                            {
                                case PostTradeEnums.Status.Closed:
                                    isClosed = true;
                                    break;
                                //[RG: 20130121] Commissions and other fields can be updated on closed trades too.
                                //Hence we need not to return on closed status and reverting it back.
                                //return;

                                case PostTradeEnums.Status.IsExercised:
                                    isExerised = true;
                                    isClosed = true;
                                    break;
                                //[RG: 20130121]
                                //return;

                                case PostTradeEnums.Status.CorporateAction:
                                    MessageBox.Show("Corporate Action has been Applied to this Group. First undo the corporate action to make any changes. ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    e.Cell.CancelUpdate();
                                    return;

                                case PostTradeEnums.Status.Exercise:
                                    MessageBox.Show("This Group is generated by Exercise.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    e.Cell.CancelUpdate();
                                    return;

                                //Modified by Disha, changes done for http://jira.nirvanasolutions.com:8080/browse/PRANA-6754
                                case PostTradeEnums.Status.CostBasisAdjustment:
                                    isCostAdjusted = true;
                                    break;

                                default:
                                    break;
                            }
                            //Modified by omshiv, allowed COL_NirvanaProcessDate if it is not closed  
                            if (e.Cell.Column.Key.Equals(COL_NirvanaProcessDate) && isClosed)
                            {
                                MessageBox.Show("Prana Process Date cannot be changed on partially or fully closed trades. Please unwind before changing the trade date.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (e.Cell.Column.Key.Equals(COl_AUECLocalDate) && isClosed)
                            {
                                MessageBox.Show("Trade Date cannot be changed on partially or fully closed trades. Please unwind before changing the trade date.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (e.Cell.Column.Key.Equals(COl_ProcessDate) && isClosed)
                            {
                                MessageBox.Show("Process Date cannot be changed on partially or fully closed trades. Please unwind before changing the process date.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (e.Cell.Column.Key.Equals(COl_OriginalPurchaseDate) && isClosed)
                            {
                                MessageBox.Show("OriginalPurchase Date cannot be changed on partially or fully closed trades. Please unwind before changing the original purchase date.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (e.Cell.Column.Key.Equals(COL_CumQty) && isClosed)
                            {
                                MessageBox.Show("Quantity cannot be changed on partially or fully closed trades. Please unwind before changing the quantity.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }

                            #region Auto Unwind Code when quantity changes http://jira.nirvanasolutions.com:8080/browse/CHMW-1793
                            //if (isClosed && e.Cell.Column.Key.Equals(COL_CumQty)
                            //    && CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                            //{
                            //    bool isUnwindingToBeDone = false;
                            //    AllocationGroup allGroup = (AllocationGroup)e.Cell.Row.ListObject;
                            //    TaxLot taxlot = allGroup.TaxLots[0];  
                            //    string symbolAccountID = taxlot.Symbol + "~" + taxlot.Level1ID;                                
                            //    if (_dictAmendments.ContainsKey(symbolAccountID) && _dictAmendments[symbolAccountID].Value > taxlot.AUECLocalDate)
                            //    {
                            //        isUnwindingToBeDone = true;
                            //        _dictAmendments[symbolAccountID] = new KeyValuePair<string, DateTime>(_dictAmendments[symbolAccountID].Key, taxlot.AUECLocalDate);
                            //    }
                            //    else if (!_dictAmendments.ContainsKey(symbolAccountID))
                            //    {
                            //        isUnwindingToBeDone = true;
                            //        KeyValuePair<string, DateTime> assetDate = new KeyValuePair<string, DateTime>(CachedDataManager.GetInstance.GetAssetText(taxlot.AssetID), taxlot.AUECLocalDate);
                            //        _dictAmendments.Add(symbolAccountID, assetDate);
                            //    }
                            //    if (isUnwindingToBeDone)
                            //    {
                            //        isUnwindingToBeDone = false;
                            //        List<ClosingTemplate> lstClosingTemplates = new List<ClosingTemplate>();
                            //        lstClosingTemplates.Add(CreateTemplate("Symbol", taxlot.Symbol, taxlot.AUECLocalDate, taxlot.Level1ID));
                            //        //MessageBox.Show("Quantity cannot be changed on partially or fully closed trades. Please unwind before changing the quantity.", "Warning!", MessageBoxButtons.OK);
                            //        //check if background worker is already busy with unwinding of other accounts
                            //        if (bgworkerUnwinding.IsBusy)
                            //        {
                            //            MessageBox.Show("Unwinding in progress. Please wait.", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            //            e.Cell.CancelUpdate();
                            //        }
                            //        else
                            //        {
                            //            bgworkerUnwinding.RunWorkerAsync(lstClosingTemplates);
                            //        }
                            //    }
                            //}
                            #endregion

                            if (e.Cell.Column.Key.Equals(COL_OrderSide) && isClosed)
                            {
                                MessageBox.Show("Side cannot be changed on partially or fully closed trades. Please unwind before changing the side.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            if (e.Cell.Column.Key.Equals(COL_TransactionType) && isClosed)
                            {
                                MessageBox.Show("Transaction Type cannot be changed on partially or fully closed trades. Please unwind before changing the Transaction Type.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }

                            //Modified by Disha, do not allow to change transaction type for cost adjusted groups: http://jira.nirvanasolutions.com:8080/browse/PRANA-6754
                            if (e.Cell.Column.Key.Equals(COL_TransactionType) && isCostAdjusted)
                            {
                                MessageBox.Show(this, "Transaction Type cannot be changed for trades generated through cost adjustment.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }

                            // Trades closed through cost adjustmnet should not be editable
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7040
                            if (gParent.ClosingStatus == ClosingStatus.Closed && isCostAdjusted)
                            {
                                MessageBox.Show(this, "Trades closed through cost adjustment cannot be edited.", "Warning!", MessageBoxButtons.OK);
                                e.Cell.CancelUpdate();
                                return;
                            }
                            AllocationManager.GetInstance().DictUnsavedAdd(gParent.GroupID, (AllocationGroup)gParent.Clone());
                            gParent.CompanyUserID = _userID;
                            isTotalCommissionChanged = AllocationManager.GetInstance().UpdateFieldsForAllocationGroupCellChange(e.Cell.Column.Key, e.Cell.Text, isTotalCommissionChanged, gParent);
                        }


                        grdCommission.Refresh();
                        if (isExerised && isTotalCommissionChanged)
                        {
                            BackgroundWorker bgWoker = new BackgroundWorker();
                            bgWoker.DoWork += new DoWorkEventHandler(bgworkerCalculateMiscFeeUnderlying_DoWork);
                            bgWoker.RunWorkerAsync(gParent.Clone());
                        }
                    }
                    else
                    {
                        if (e.Cell.Column.Key != "checkBox" && e.Cell.Column.Key != COL_SettlementDate && e.Cell.Column.Key != OrderFields.PROPERTY_OTHERBROKERFEES && e.Cell.Column.Key != OrderFields.PROPERTY_CLEARINGBROKERFEE && e.Cell.Column.Key != COL_OrderSide)
                        {
                            grdCommission.CellChange -= new CellEventHandler(grdCommission_CellChange);
                            e.Cell.Value = e.Cell.OriginalValue;
                            grdCommission.CellChange += new CellEventHandler(grdCommission_CellChange);
                        }
                    }
                }
                else
                {
                    if (CheckGroupAndTaxlotStatus(e))
                    {
                        e.Cell.CancelUpdate();
                        return;
                    }
                    else
                    {
                        // If column is counter party, don't set its value to 0
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                        if (!e.Cell.Column.Key.Equals(COL_CounterPartyName))
                        {
                            e.Cell.Value = 0.0;
                        }
                    }
                }
                AllocationGroup allGroup = null;
                if (e.Cell.Row.ListObject.GetType().Name.Equals("AllocationGroup"))
                {
                    allGroup = (AllocationGroup)e.Cell.Row.ListObject;
                    //Code moved to PranaBasicMessage
                    UpdateSettlementFields(e, allGroup);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sandeep_10DEC2014: Check Group and Taxlot status whether Corporate Action is applied or it is exercise.
        /// This is a special case handling, whenever user uses back space key and in the parent function we set value to zero(0) but CA and Exercise checks bypassed
        /// so before doing value to 0, there should be check for CA and Exercise.
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5620
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CheckGroupAndTaxlotStatus(Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            bool isCAAppliedOrExercise = false;
            try
            {
                if (e != null && e.Cell != null)
                {
                    Type type = e.Cell.Row.ListObject.GetType();
                    AllocationGroup gParent = null;
                    if (type.Name == "TaxLot")
                    {
                        gParent = (AllocationGroup)e.Cell.Row.ParentRow.ListObject;
                        TaxLot taxlot = (TaxLot)e.Cell.Row.ListObject;
                        PostTradeEnums.Status GroupStatus = _closingServices.InnerChannel.CheckGroupStatus(gParent);
                        if (GroupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                        {
                            MessageBox.Show("Corporate Action has been Applied to this taxlot. First undo the corporate action to make any changes. ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isCAAppliedOrExercise = true;
                        }
                        if (GroupStatus.Equals(PostTradeEnums.Status.Exercise))
                        {
                            MessageBox.Show("This Group is generated by Exercise.", "Warning", MessageBoxButtons.OK);
                            isCAAppliedOrExercise = true;
                        }
                    }
                    else if (type.Name.Equals("AllocationGroup"))
                    {
                        gParent = (AllocationGroup)e.Cell.Row.ListObject;
                        PostTradeEnums.Status GroupStatus = _closingServices.InnerChannel.CheckGroupStatus(gParent);
                        if (GroupStatus.Equals(PostTradeEnums.Status.CorporateAction))
                        {
                            MessageBox.Show("Corporate Action has been Applied to this Group. First undo the corporate action to make any changes. ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isCAAppliedOrExercise = true;
                        }
                        if (GroupStatus.Equals(PostTradeEnums.Status.Exercise))
                        {
                            MessageBox.Show("This Group is generated by Exercise.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isCAAppliedOrExercise = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isCAAppliedOrExercise;
        }

        void bgworkerCalculateMiscFeeUnderlying_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AllocationGroup gParent = e.Argument as AllocationGroup;
                Dictionary<string, Dictionary<AllocationGroup, AllocationGroup>> Dictgroups = _allocationServices.InnerChannel.GetUnderlyingGroupsIfExercised(gParent);
                if (Dictgroups.Count > 0)
                {
                    foreach (TaxLot taxlot in gParent.TaxLots)
                    {
                        if (Dictgroups.ContainsKey(taxlot.TaxLotID))
                        {
                            taxlot.AvgPrice = gParent.AvgPrice;
                            taxlot.ContractMultiplier = gParent.ContractMultiplier;
                            Dictionary<AllocationGroup, AllocationGroup> Groups = Dictgroups[taxlot.TaxLotID];
                            foreach (KeyValuePair<AllocationGroup, AllocationGroup> kp in Groups)
                            {
                                AllocationGroup underlyingGroup = kp.Key;
                                AllocationGroup closingGroup = kp.Value;
                                if (underlyingGroup.OptionPremiumAdjustment != 0)
                                {
                                    //TODO: Option premium adjustment handling needs to be done here
                                    CalculateOptionPremiumAdjustmentForExercisedUnderlying(taxlot, underlyingGroup, closingGroup);
                                    underlyingGroup.ResetTaxlotDictionary(underlyingGroup.TaxLots);
                                    underlyingGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                                    closingGroup.ResetTaxlotDictionary(closingGroup.TaxLots);
                                    closingGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                                    AllocationManager.GetInstance().AddExercisedGroups(underlyingGroup, closingGroup);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CalculateOptionPremiumAdjustmentForExercisedUnderlying(TaxLot ParentTaxlot, AllocationGroup underlyingGroup, AllocationGroup closingGroup)
        {
            try
            {
                CommissionFields commFields = new CommissionFields();
                foreach (TaxLot taxlot in underlyingGroup.TaxLots)
                {
                    taxlot.OptionPremiumAdjustment = (ParentTaxlot.SideMultiplier) * (ParentTaxlot.AvgPrice * taxlot.TaxLotQty) + ((ParentTaxlot.TotalCommissionandFees / ParentTaxlot.TaxLotQty) * (taxlot.TaxLotQty / ParentTaxlot.ContractMultiplier));
                    commFields.OptionPremiumAdjustment = +taxlot.OptionPremiumAdjustment;
                }

                underlyingGroup.UpdateGroupCommissionAndFees(commFields);
                commFields = new CommissionFields();
                foreach (TaxLot taxlot in closingGroup.TaxLots)
                {
                    taxlot.OptionPremiumAdjustment = (-1) * ((ParentTaxlot.SideMultiplier) * (ParentTaxlot.AvgPrice * taxlot.TaxLotQty * ParentTaxlot.ContractMultiplier) + ((ParentTaxlot.TotalCommissionandFees / ParentTaxlot.TaxLotQty) * (taxlot.TaxLotQty)));
                    commFields.OptionPremiumAdjustment = +taxlot.OptionPremiumAdjustment;
                }
                closingGroup.UpdateGroupCommissionAndFees(commFields);
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

        //Rahul 20120127
        //Added this event as it was not wire up before to the grid to enable cancel button functionality 
        // in grdamendmend. see details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1751
        private void grdAmendmend_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                CancelUpdateInSettlementColumn(e);
                if (e.Cell.Text != string.Empty)
                {
                    if (e.Cell.Column.Key.Equals(COL_AvgPrice) || e.Cell.Column.Key.Equals(COL_CumQty))
                    {
                        double updatedValue = 0.0;
                        if (double.TryParse(e.Cell.Text, out updatedValue))
                        {
                            if (double.Parse(e.Cell.Text) < 0)
                            {
                                e.Cell.CancelUpdate();
                                return;
                            }
                        }
                    }
                    AllocationGroup gParent = (AllocationGroup)e.Cell.Row.ListObject;
                    AllocationManager.GetInstance().DictUnsavedAdd(gParent.GroupID, (AllocationGroup)gParent.Clone());
                    if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_COMMISSION) || e.Cell.Column.Key.Equals(CAPTION_OtherFees)
                        || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_OTHERBROKERFEES) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_CLEARINGFEE)
                        || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_MISCFEES) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_TRANSACTIONLEVY)
                        || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_CLEARINGBROKERFEE) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SOFTCOMMISSION))
                    {
                        gParent.CommSource = CommisionSource.Manual;
                        gParent.SoftCommSource = CommisionSource.Manual;
                    }
                    gParent.UpdateGroupPersistenceStatus();
                    gParent.CompanyUserID = _userID;
                    gParent.IsModified = true;
                }
                //Narendra Kumar Jangir, July 10 2013
                //trade attributes and description field should be blank not zero
                else if (TradeStringFields.Contains(e.Cell.Column.Key) && string.IsNullOrEmpty(e.Cell.Text))
                {
                    e.Cell.Value = string.Empty;
                }
                else
                {
                    // If column is counter party, venue, order side or transaction type don't set its value to 0
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                    if (!(e.Cell.Column.Key.Equals(COL_CounterPartyName) || e.Cell.Column.Key.Equals(COL_Venue) || e.Cell.Column.Key.Equals(COL_OrderSide) || e.Cell.Column.Key.Equals(COL_TransactionType)))
                    {
                        e.Cell.Value = 0.0;
                    }
                }
                AllocationGroup allGroup = null;
                if (e.Cell.Row.ListObject.GetType().Name.Equals("AllocationGroup"))
                {
                    allGroup = (AllocationGroup)e.Cell.Row.ListObject;
                    //Code moved to PranaBasicMessage
                    UpdateSettlementFields(e, allGroup);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CancelUpdateInSettlementColumn(CellEventArgs e)
        {
            try
            {
                int CurrencyID = 0;
                int SettlementCurrencyID = 0;

                if (e.Cell.Row.ListObject.GetType().Equals(typeof(AllocationGroup)))
                {
                    AllocationGroup grp = e.Cell.Row.ListObject as AllocationGroup;
                    CurrencyID = grp.CurrencyID;
                    SettlementCurrencyID = grp.SettlementCurrencyID;
                }
                else if (e.Cell.Row.ListObject.GetType().Equals(typeof(TaxLot)))
                {
                    TaxLot taxLot = e.Cell.Row.ListObject as TaxLot;
                    CurrencyID = taxLot.CurrencyID;
                    SettlementCurrencyID = taxLot.SettlementCurrencyID;
                }

                if (CurrencyID != SettlementCurrencyID)
                {
                    //Average Price is to be auto calculated
                    //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                    if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_AVGPRICE))
                    {
                        if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                        {
                            MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Cell.CancelUpdate();
                        }
                    }
                    else if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRate))
                    {
                        if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementFXRate)
                        {
                            MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Cell.CancelUpdate();
                        }
                    }
                    else if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRateCalc))
                    {
                        if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementFXRate)
                        {
                            MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Cell.CancelUpdate();
                        }
                    }
                    else if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT))
                    {
                        if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementPrice)
                        {
                            MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Cell.CancelUpdate();
                        }
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
        internal void RecalculateCommissionAndFees(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                {
                    CommissionRule commissionRule = sender as CommissionRule;
                    CommissionRuleEvents commissionRuleEvent = (CommissionRuleEvents)e;
                    toolStripStatusLabel1.Text = "";
                    bool isAnyGroupSelected = false;
                    StringBuilder strBuilder = new StringBuilder();

                    foreach (UltraGridRow existingRow in this.grdCommission.Rows.GetFilteredInNonGroupByRows())
                    {
                        AllocationGroup allocatedGroup = (AllocationGroup)existingRow.ListObject;
                        if (existingRow.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                        {
                            isAnyGroupSelected = true;
                            if ((allocatedGroup.CounterPartyID.Equals(0) || allocatedGroup.CounterPartyID == int.MinValue) || (allocatedGroup.VenueID.Equals(0) || allocatedGroup.VenueID == int.MinValue))
                            {
                                strBuilder.Append("Symbol : ");
                                strBuilder.Append(allocatedGroup.Symbol);
                                strBuilder.Append(", ");
                                strBuilder.Append("Qty : ");
                                strBuilder.Append(allocatedGroup.AllocatedQty);
                                strBuilder.Append(", ");
                                strBuilder.Append("Side : ");
                                strBuilder.Append(allocatedGroup.OrderSide);
                                strBuilder.Append(", ");
                                strBuilder.Append("AvgPrice : ");
                                strBuilder.Append(allocatedGroup.AvgPrice);
                                strBuilder.Append(Environment.NewLine);
                            }
                        }
                    }
                    if (isAnyGroupSelected)
                    {
                        //toolStripStatusLabel1.Text = "";
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Please select atleast one record!";
                        timerClear.Start();
                        return;
                    }

                    if (commissionRule == null)
                    {
                        int len = strBuilder.Length;
                        if (strBuilder.Length > 0)
                        {
                            strBuilder.Remove((len - 2), 2);

                            DialogResult diares;
                            diares = MessageBox.Show("CounterParty/Venue information is missing for \n" + strBuilder.ToString() + "\nDo you want to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (diares.Equals(DialogResult.No))
                            {
                                return;
                            }
                            else if (diares.Equals(DialogResult.Yes))
                            {
                                RecalculateCommission(commissionRule, commissionRuleEvent);
                            }
                        }
                        else
                        {
                            RecalculateCommission(commissionRule, commissionRuleEvent);
                        }
                    }
                    else
                    {
                        RecalculateCommission(commissionRule, commissionRuleEvent);
                    }
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
            finally
            {
                btnSave.Enabled = true;
            }
        }

        internal void BulkChangeOnGroupLevel(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "";
                BulkChangesAtGroupLevel bulkChanges = (BulkChangesAtGroupLevel)sender;
                bool updatedGroup = false;
                int selectedRow = 0;

                UltraGridRow[] filteredRows = this.grdCommission.Rows.GetFilteredInNonGroupByRows();
                if (filteredRows.Length > 0)
                {
                    foreach (UltraGridRow existingRow in filteredRows)
                    {
                        if (existingRow.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                        {
                            AllocationGroup allocatedGroup = (AllocationGroup)existingRow.ListObject;
                            AllocationManager.GetInstance().DictUnsavedAdd(allocatedGroup.GroupID, (AllocationGroup)allocatedGroup.Clone());
                            allocatedGroup.UpdateGroupPersistenceStatus();
                            allocatedGroup.CompanyUserID = _userID;
                            allocatedGroup.IsModified = true;

                            allocatedGroup.IsAnotherTaxlotAttributesUpdated = true;

                            if (bulkChanges.GroupWise)
                            {
                                if (bulkChanges.AvgPrice != 0)
                                {
                                    allocatedGroup.AvgPrice = double.Parse(bulkChanges.AvgPrice.ToString());
                                    allocatedGroup.UpdateTaxlotAvgPrice();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
                                }

                                // Rounding the Average Price upto given decimal places given by user
                                if (bulkChanges.AvgPxUpto != 0)
                                {
                                    allocatedGroup.AvgPrice = Math.Round(allocatedGroup.AvgPrice, bulkChanges.AvgPxUpto);
                                    allocatedGroup.UpdateTaxlotAvgPrice();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AvgPrice_Changed);
                                }
                                if (bulkChanges.FXRate != 0)
                                {
                                    allocatedGroup.FXRate = double.Parse(bulkChanges.FXRate.ToString());
                                    allocatedGroup.UpdateTaxlotFXRate();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.FxRate_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.FxRate_Changed);
                                }
                                if (bulkChanges.SettlCurrFxRate != 0)
                                {
                                    allocatedGroup.SettlCurrFxRate = double.Parse(bulkChanges.SettlCurrFxRate.ToString());
                                    allocatedGroup.UpdateTaxlotSettlCurrFxRate();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                                }
                                if (bulkChanges.SettlCurrency != 0)
                                {
                                    allocatedGroup.SettlementCurrencyID = int.Parse(bulkChanges.SettlCurrency.ToString());
                                    allocatedGroup.UpdateTaxlotSettlCurrency();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                                }
                                if (string.IsNullOrEmpty(bulkChanges.SettlCurrFxRateCalc))
                                {
                                    allocatedGroup.SettlCurrFxRateCalc = bulkChanges.SettlCurrFxRateCalc.ToString();
                                    allocatedGroup.UpdateTaxlotSettlCurrFxRateCalc(bulkChanges.SettlCurrFxRateCalc);
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                                }
                                if (!bulkChanges.FXConversionOperator.Equals(string.Empty))
                                {
                                    allocatedGroup.FXConversionMethodOperator = bulkChanges.FXConversionOperator;
                                    allocatedGroup.UpdateTaxlotFXConversionMethodOperator(bulkChanges.FXConversionOperator);
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                                }
                                if (bulkChanges.AccruedInterest != 0 && (allocatedGroup.AssetName.Equals(AssetCategory.FixedIncome.ToString()) || allocatedGroup.AssetName.Equals(AssetCategory.ConvertibleBond.ToString())))
                                {
                                    allocatedGroup.AccruedInterest = double.Parse(bulkChanges.AccruedInterest.ToString());
                                    allocatedGroup.UpdateTaxlotAccruedInterest();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                                }
                                if (!string.IsNullOrEmpty(bulkChanges.Description))
                                {
                                    allocatedGroup.Description = bulkChanges.Description;
                                    allocatedGroup.UpdateTaxlotDescription();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.Description_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Description_Changed);
                                }
                                if (!string.IsNullOrEmpty(bulkChanges.InternalComments))
                                {
                                    allocatedGroup.InternalComments = bulkChanges.InternalComments;
                                    allocatedGroup.UpdateTaxlotIneternalComments();
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.InternalComments_Changed);
                                    allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.InternalComments_Changed);
                                }
                                if (bulkChanges.CounterPartyID != 0 && bulkChanges.CounterPartyID != int.MinValue)
                                {
                                    allocatedGroup.CounterPartyID = bulkChanges.CounterPartyID;
                                    allocatedGroup.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(allocatedGroup.CounterPartyID);
                                    allocatedGroup.UpdateGroupTaxlots(string.Empty, string.Empty);
                                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
                                    //allocatedGroup.AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType.Counterparty_Changed);
                                }
                                updatedGroup = true;
                            }
                            else
                            {
                                if (bulkChanges.FXRate != 0 && bulkChanges.SettlCurrFxRate != 0 && bulkChanges.AccountIDs != null && bulkChanges.AccountIDs.Count > 0)
                                {
                                    allocatedGroup.AddTradeAuditActionToAllTaxlots(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                                    allocatedGroup.AddTradeAuditActionToAllTaxlots(TradeAuditActionType.ActionType.FxRate_Changed);
                                    allocatedGroup.AddTradeAuditActionToAllTaxlots(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                                    allocatedGroup.AddTradeAuditActionToAllTaxlots(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);

                                    // FX Counter Party is just for filtering the selected trades.
                                    if (!bulkChanges.FXCounterPartyID.Equals(int.MinValue))
                                    {
                                        if (bulkChanges.FXCounterPartyID.Equals(allocatedGroup.CounterPartyID))
                                        {
                                            CopyTaxlotFXRate(allocatedGroup, bulkChanges, ref updatedGroup);
                                            CopyTaxlotSettlCurrFxRate(allocatedGroup, bulkChanges, ref updatedGroup);
                                        }
                                    }
                                    else
                                    {
                                        CopyTaxlotFXRate(allocatedGroup, bulkChanges, ref updatedGroup);
                                        CopyTaxlotSettlCurrFxRate(allocatedGroup, bulkChanges, ref updatedGroup);
                                    }
                                }
                            }
                            if (updatedGroup)
                            {
                                existingRow.ChildBands[1].Rows.Refresh(RefreshRow.ReloadData);
                                existingRow.Refresh();
                                toolStripStatusLabel1.Text = "Bulk changes updated for the selected record(s).";
                            }
                            else
                            {
                                //when groups are not applicable to the filtered settings in company accounts.
                                toolStripStatusLabel1.Text = "No valid record(s) for the required change(s).";
                            }
                            selectedRow++;
                        }
                    }
                    if (selectedRow == 0)
                    {
                        toolStripStatusLabel1.Text = "Please select atleast one record!";
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select atleast one record!";
                }
                timerClear.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void DisplayErrorMessage()
        {
            try
            {
                toolStripStatusLabel1.Text = "Please select atleast one value!";
                timerClear.Start();
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

        private void CopyTaxlotFXRate(AllocationGroup allocatedGroup, BulkChangesAtGroupLevel bulkChanges, ref bool updatedGroup)
        {
            try
            {
                foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                {
                    if (bulkChanges.AccountIDs.Contains(taxlot.Level1ID))
                    {
                        taxlot.FXRate = double.Parse(bulkChanges.FXRate.ToString());
                        taxlot.FXConversionMethodOperator = bulkChanges.FXConversionOperator;
                        allocatedGroup.UpdateTaxlotFXRateAndOperator(taxlot.FXRate, taxlot.FXConversionMethodOperator, taxlot.TaxLotID);
                        updatedGroup = true;
                    }
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
        }

        private void CopyTaxlotSettlCurrFxRate(AllocationGroup allocatedGroup, BulkChangesAtGroupLevel bulkChanges, ref bool updatedGroup)
        {
            try
            {
                foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                {
                    if (bulkChanges.AccountIDs.Contains(taxlot.Level1ID))
                    {
                        taxlot.SettlCurrFxRate = double.Parse(bulkChanges.SettlCurrFxRate.ToString());
                        taxlot.SettlCurrFxRateCalc = bulkChanges.SettlCurrFxRateCalc;
                        allocatedGroup.UpdateTaxlotSettlCurrFxRateAndOperator(taxlot.SettlCurrFxRate, taxlot.SettlCurrFxRateCalc, taxlot.TaxLotID);
                        updatedGroup = true;
                    }
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
        }
        private void RecalculateCommission(CommissionRule commissionRule, CommissionRuleEvents commissionRuleEvent)
        {
            try
            {
                foreach (UltraGridRow existingRow in this.grdCommission.Rows.GetFilteredInNonGroupByRows())
                {
                    AllocationGroup allocatedGroup = (AllocationGroup)existingRow.ListObject;
                    if (existingRow.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                    {
                        AllocationManager.GetInstance().DictUnsavedAdd(allocatedGroup.GroupID, (AllocationGroup)allocatedGroup.Clone());
                        allocatedGroup.UpdateGroupPersistenceStatus();
                        allocatedGroup.CompanyUserID = _userID;
                        allocatedGroup.IsModified = true;
                        allocatedGroup.IsAnotherTaxlotAttributesUpdated = true;
                        TaxLot updatedTaxlot = new TaxLot();
                        CommissionFields commFields = null;

                        if (commissionRuleEvent.GroupWise.Equals(true))
                        {
                            if (commissionRule != null)
                            {
                                if (commissionRule.Commission.CommissionRate != double.MinValue)
                                {
                                    allocatedGroup = _allocationServices.InnerChannel.CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                                    commFields = new CommissionFields();
                                    commFields.Commission = allocatedGroup.Commission;
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.Commission_Changed);
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                    allocatedGroup.CommSource = CommisionSource.Manual;
                                    allocatedGroup.SoftCommSource = CommisionSource.Manual;
                                }

                                if (commissionRule.SoftCommission.CommissionRate != double.MinValue)
                                {
                                    allocatedGroup = _allocationServices.InnerChannel.CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                                    commFields = new CommissionFields();
                                    commFields.SoftCommission = allocatedGroup.SoftCommission;
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SoftCommission_Changed);
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                    allocatedGroup.CommSource = CommisionSource.Manual;
                                    allocatedGroup.SoftCommSource = CommisionSource.Manual;
                                }

                                if (commissionRule.ClearingFeeRate != double.MinValue)
                                {
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                                    allocatedGroup = _allocationServices.InnerChannel.CalculateFeesGroupwise(commissionRule, allocatedGroup);
                                    commFields = new CommissionFields();
                                    commFields.OtherBrokerFees = allocatedGroup.OtherBrokerFees;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.ClearingBrokerFeeRate != double.MinValue)
                                {
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                                    allocatedGroup = _allocationServices.InnerChannel.CalculateFeesGroupwise(commissionRule, allocatedGroup);
                                    commFields = new CommissionFields();
                                    commFields.ClearingBrokerFee = allocatedGroup.ClearingBrokerFee;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.StampDuty != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.StampDuty_Changed);
                                    commFields.StampDuty = allocatedGroup.StampDuty;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.ClearingFee_A != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingFee_Changed);
                                    commFields.ClearingFee = allocatedGroup.ClearingFee;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.TaxonCommissions != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                                    commFields.TaxOnCommissions = allocatedGroup.TaxOnCommissions;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.TransactionLevy != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                                    commFields.TransactionLevy = allocatedGroup.TransactionLevy;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.MiscFees != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.MiscFees_Changed);
                                    commFields.MiscFees = allocatedGroup.MiscFees;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.SecFee != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SecFee_Changed);
                                    commFields.SecFee = allocatedGroup.SecFee;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.OccFee != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OccFee_Changed);
                                    commFields.OccFee = allocatedGroup.OccFee;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }

                                if (commissionRule.OrfFee != double.MinValue)
                                {
                                    _allocationServices.InnerChannel.CalculateOtherFeeGroupwise(commissionRule, ref allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee);
                                    commFields = new CommissionFields();
                                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OrfFee_Changed);
                                    commFields.OrfFee = allocatedGroup.OrfFee;
                                    allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);
                                }
                                allocatedGroup.CommSource = CommisionSource.Manual;
                                allocatedGroup.SoftCommSource = CommisionSource.Manual;
                            }
                            else if (commissionRule == null)
                            {
                                //Changing commission source to auto before calculating commission based on default commission rule.
                                allocatedGroup.CommissionSource = (int)CommisionSource.Auto;
                                allocatedGroup.SoftCommissionSource = (int)CommisionSource.Auto;
                                allocatedGroup.CommSource = CommisionSource.Auto;
                                allocatedGroup.SoftCommSource = CommisionSource.Auto;
                                allocatedGroup = AllocationManager.CalculateCommission(allocatedGroup);
                             
                                commFields = new CommissionFields();
                                commFields.Commission = allocatedGroup.Commission;
                                commFields.SoftCommission = allocatedGroup.SoftCommission;
                                commFields.OtherBrokerFees = allocatedGroup.OtherBrokerFees;
                                commFields.StampDuty = allocatedGroup.StampDuty;
                                commFields.ClearingFee = allocatedGroup.ClearingFee;
                                commFields.TaxOnCommissions = allocatedGroup.TaxOnCommissions;
                                commFields.TransactionLevy = allocatedGroup.TransactionLevy;
                                commFields.MiscFees = allocatedGroup.MiscFees;
                                commFields.SecFee = allocatedGroup.SecFee;
                                commFields.OccFee = allocatedGroup.OccFee;
                                commFields.OrfFee = allocatedGroup.OrfFee;

                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.Commission_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SoftCommission_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.StampDuty_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.ClearingFee_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.MiscFees_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.SecFee_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OccFee_Changed);
                                AuditManager.Instance.AddActionToAllGroupAndTaxlots(allocatedGroup, TradeAuditActionType.ActionType.OrfFee_Changed);
                               allocatedGroup.UpdateTaxlotCommissionAndFees(commFields);// Commenting as commission is calculating at taxlot level at server side.
                                //allocatedGroup.CommSource = CommisionSource.Manual;
                                //allocatedGroup.SoftCommSource = CommisionSource.Manual;
                            }
                            AllocationManager.GetInstance().AddGroup(allocatedGroup);
                        }
                        else //if (commissionRuleEvent.GroupWise == false) As this is already false
                        {
                            allocatedGroup.Commission = 0;
                            allocatedGroup.SoftCommission = 0;
                            allocatedGroup.OtherBrokerFees = 0;
                            allocatedGroup.ClearingBrokerFee = 0;

                            allocatedGroup.StampDuty = 0;
                            allocatedGroup.ClearingFee = 0;
                            allocatedGroup.TaxOnCommissions = 0;
                            allocatedGroup.TransactionLevy = 0;
                            allocatedGroup.MiscFees = 0;
                            allocatedGroup.SecFee = 0;
                            allocatedGroup.OccFee = 0;
                            allocatedGroup.OrfFee = 0;
                            int i = 0;
                            // if account filter is applied i.e. only selected accounts commission and fee to be calculated
                            if (commissionRule.AccountIDs != null && commissionRule.AccountIDs.Count > 0)
                            {
                                foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                                {
                                    if (commissionRule.AccountIDs.Contains(taxlot.Level1ID))
                                    {
                                        CalculateTaxlotwiseCommissionAndFee(commissionRule, ref allocatedGroup, ref updatedTaxlot, ref commFields, i, taxlot);
                                        allocatedGroup.CommSource = CommisionSource.Manual;
                                        allocatedGroup.SoftCommSource = CommisionSource.Manual;
                                    }
                                    else if (!commissionRule.AccountIDs.Contains(taxlot.Level1ID))
                                    {
                                        allocatedGroup.Commission += taxlot.Commission;
                                        allocatedGroup.SoftCommission += taxlot.SoftCommission;
                                        allocatedGroup.OtherBrokerFees += taxlot.OtherBrokerFees;
                                        allocatedGroup.ClearingBrokerFee += taxlot.ClearingBrokerFee;
                                        allocatedGroup.StampDuty += taxlot.StampDuty;
                                        allocatedGroup.ClearingFee += taxlot.ClearingFee;
                                        allocatedGroup.TaxOnCommissions += taxlot.TaxOnCommissions;
                                        allocatedGroup.TransactionLevy += taxlot.TransactionLevy;
                                        allocatedGroup.MiscFees += taxlot.MiscFees;
                                        allocatedGroup.SecFee += taxlot.SecFee;
                                        allocatedGroup.OccFee += taxlot.OccFee;
                                        allocatedGroup.OrfFee += taxlot.OrfFee;
                                        allocatedGroup.CommSource = CommisionSource.Manual;
                                        allocatedGroup.SoftCommSource = CommisionSource.Manual;
                                    }
                                    i++;
                                }
                            }
                            else
                            {
                                foreach (TaxLot taxlot in allocatedGroup.TaxLots)
                                {
                                    CalculateTaxlotwiseCommissionAndFee(commissionRule, ref allocatedGroup, ref updatedTaxlot, ref commFields, i, taxlot);
                                    allocatedGroup.CommSource = CommisionSource.Manual;
                                    allocatedGroup.SoftCommSource = CommisionSource.Manual;
                                    i++;
                                }
                            }
                            AllocationManager.GetInstance().AddGroup(allocatedGroup);
                        }
                    }
                }
                toolStripStatusLabel1.Text = "Commission and Fee calculated for the selected record(s).";
                timerClear.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calculate Commission and Fee taxlot wise
        /// </summary>
        /// <param name="commissionRule"></param>
        /// <param name="allocatedGroup"></param>
        /// <param name="updatedTaxlot"></param>
        /// <param name="commFields"></param>
        /// <param name="i"></param>
        /// <param name="taxlot"></param>
        private void CalculateTaxlotwiseCommissionAndFee(CommissionRule commissionRule, ref AllocationGroup allocatedGroup, ref TaxLot updatedTaxlot, ref CommissionFields commFields, int i, TaxLot taxlot)
        {
            try
            {
                if (commissionRule.Commission.CommissionRate != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateCommissionAccountWise(commissionRule, taxlot, allocatedGroup);
                    taxlot.Commission = updatedTaxlot.Commission;
                    allocatedGroup.TaxLots[i].Commission = updatedTaxlot.Commission;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                }

                if (commissionRule.SoftCommission.CommissionRate != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateCommissionAccountWise(commissionRule, taxlot, allocatedGroup);
                    taxlot.SoftCommission = updatedTaxlot.SoftCommission;
                    allocatedGroup.TaxLots[i].SoftCommission = updatedTaxlot.SoftCommission;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                }

                if (commissionRule.ClearingFeeRate != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateFeesAccountWise(commissionRule, taxlot, allocatedGroup);
                    taxlot.OtherBrokerFees = updatedTaxlot.OtherBrokerFees;
                    allocatedGroup.TaxLots[i].OtherBrokerFees = updatedTaxlot.OtherBrokerFees;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                }

                if (commissionRule.ClearingBrokerFeeRate != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateFeesAccountWise(commissionRule, taxlot, allocatedGroup);
                    taxlot.ClearingBrokerFee = updatedTaxlot.ClearingBrokerFee;
                    allocatedGroup.TaxLots[i].ClearingBrokerFee = updatedTaxlot.ClearingBrokerFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                }

                if (commissionRule.StampDuty != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.StampDuty);
                    taxlot.StampDuty = updatedTaxlot.StampDuty;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.StampDuty_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.StampDuty_Changed);
                }

                if (commissionRule.ClearingFee_A != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.ClearingFee);
                    taxlot.ClearingFee = updatedTaxlot.ClearingFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.ClearingFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.ClearingFee_Changed);
                }

                if (commissionRule.TaxonCommissions != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.TaxOnCommissions);
                    taxlot.TaxOnCommissions = updatedTaxlot.TaxOnCommissions;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                }

                if (commissionRule.TransactionLevy != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.TransactionLevy);
                    taxlot.TransactionLevy = updatedTaxlot.TransactionLevy;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.TransactionLevy_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.TransactionLevy_Changed);
                }

                if (commissionRule.MiscFees != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.MiscFees);
                    taxlot.MiscFees = updatedTaxlot.MiscFees;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.MiscFees_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.MiscFees_Changed);
                }

                if (commissionRule.SecFee != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.SecFee);
                    taxlot.SecFee = updatedTaxlot.SecFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.SecFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.SecFee_Changed);
                }

                if (commissionRule.OccFee != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.OccFee);
                    taxlot.OccFee = updatedTaxlot.OccFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.OccFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.OccFee_Changed);
                }

                if (commissionRule.OrfFee != double.MinValue)
                {
                    updatedTaxlot = _allocationServices.InnerChannel.CalculateOtherFeesAccountWise(commissionRule, taxlot, allocatedGroup, Prana.BusinessObjects.AppConstants.OtherFeeType.OrfFee);
                    taxlot.OrfFee = updatedTaxlot.OrfFee;
                    taxlot.AddTradeAction(TradeAuditActionType.ActionType.OrfFee_Changed);
                    allocatedGroup.AddTradeAction(TradeAuditActionType.ActionType.OrfFee_Changed);
                }

                allocatedGroup.Commission += updatedTaxlot.Commission;
                allocatedGroup.SoftCommission += updatedTaxlot.SoftCommission;
                allocatedGroup.OtherBrokerFees += updatedTaxlot.OtherBrokerFees;
                allocatedGroup.ClearingBrokerFee += updatedTaxlot.ClearingBrokerFee;
                allocatedGroup.StampDuty += updatedTaxlot.StampDuty;
                allocatedGroup.ClearingFee += updatedTaxlot.ClearingFee;
                allocatedGroup.TaxOnCommissions += updatedTaxlot.TaxOnCommissions;
                allocatedGroup.TransactionLevy += updatedTaxlot.TransactionLevy;
                allocatedGroup.MiscFees += updatedTaxlot.MiscFees;
                allocatedGroup.SecFee += updatedTaxlot.SecFee;
                allocatedGroup.OccFee += updatedTaxlot.OccFee;
                allocatedGroup.OrfFee += updatedTaxlot.OrfFee;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAmendmend_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                DialogResult result = DialogResult.No;
                if (!e.Cell.Text.Equals(string.Empty) || (TradeStringFields.Contains(e.Cell.Column.Key) && string.IsNullOrEmpty(e.Cell.Text)))
                {
                    AllocationGroup gParent = (AllocationGroup)e.Cell.Row.ListObject;
                    double outResult = 0.0;
                    bool temp = false;
                    switch (e.Cell.Column.Key)
                    {
                        case COl_ProcessDate:
                            DateTime ProcessDate = Convert.ToDateTime(e.Cell.Value);
                            if (ProcessDate.Date < gParent.AUECLocalDate.Date)
                            {
                                MessageBox.Show("Process Date cannot be less than Trade Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                                e.Cell.Value = e.Cell.OriginalValue;

                            }
                            if (ProcessDate.Date > gParent.SettlementDate.Date)
                            {
                                MessageBox.Show("Process Date cannot be greater than Settlement Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                                e.Cell.Value = e.Cell.OriginalValue;

                            }
                            else
                            {
                                // if process date is changed, original purchase date is set equal to the process date.
                                grdAmendmend.ActiveRow.Cells[COl_OriginalPurchaseDate].Value = e.Cell.Value;
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                            }
                            break;

                        case COl_OriginalPurchaseDate:
                            DateTime OriginalPurchaseDate = Convert.ToDateTime(e.Cell.Value);
                            // original purchase is dependent on process date, it cannot be greater than the process date.
                            if (OriginalPurchaseDate.Date > gParent.ProcessDate.Date)
                            {
                                MessageBox.Show("OriginalPurchase Date cannot be greater than Process Date,it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                                e.Cell.Value = e.Cell.OriginalValue;
                            }
                            else
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                            }

                            break;

                        case COl_AUECLocalDate:
                            DateTime TradeDate = Convert.ToDateTime(e.Cell.Value);
                            // if trade date is changed, procss date and original purchase date are set to trade date.
                            gParent.ProcessDate = TradeDate;
                            gParent.OriginalPurchaseDate = TradeDate;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                            break;

                        case COL_SettlementDate:
                            DateTime settlementDate = Convert.ToDateTime(e.Cell.Value);
                            //Setllement date can not be less than the trade date.
                            if (settlementDate < gParent.ProcessDate.Date)
                            {
                                MessageBox.Show("Settlement Date cannot be less than Process Date,it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                                e.Cell.Value = e.Cell.OriginalValue;
                            }
                            else
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                            }
                            break;


                        case COL_DESCRIPTION:
                            gParent.Description = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.Description_Changed);
                            break;

                        case COL_INTERNALCOMMENTS:
                            gParent.InternalComments = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.InternalComments_Changed);
                            break;

                        case COL_TradeAttribute1:
                            gParent.TradeAttribute1 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                            break;

                        case COL_TradeAttribute2:
                            gParent.TradeAttribute2 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                            break;

                        case COL_TradeAttribute3:
                            gParent.TradeAttribute3 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                            break;

                        case COL_TradeAttribute4:
                            gParent.TradeAttribute4 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                            break;

                        case COL_TradeAttribute5:
                            gParent.TradeAttribute5 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                            break;

                        case COL_TradeAttribute6:
                            gParent.TradeAttribute6 = e.Cell.Text;
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                            break;

                        case COL_OrderSide:
                            // Check if order side is valid or not
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                            if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_sides, e.Cell.Text))
                            {
                                MessageBox.Show(this, "Please select a valid order side.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.Value = e.Cell.OriginalValue;
                                e.Cell.CancelUpdate();
                                return;
                            }
                            AllocationGroup grp = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                            if (grp != null)
                            {
                                result = SetTransactionTypeBasedOnSide(e, grp);
                            }
                            if (result == DialogResult.Yes)
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                            }
                            else if (result == DialogResult.No)
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            }
                            break;

                        case COL_CounterPartyName:
                            if (e.Cell.Text == ApplicationConstants.C_COMBO_SELECT)
                            {
                                grdAmendmend.AfterCellUpdate -= new CellEventHandler(grdAmendmend_AfterCellUpdate);
                                e.Cell.Value = string.Empty;
                                grdAmendmend.AfterCellUpdate += new CellEventHandler(grdAmendmend_AfterCellUpdate);
                            }
                            // Check if counter party is valid or not
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                            if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_counterParty, e.Cell.Text))
                            {
                                MessageBox.Show(this, "Please select a valid counter party.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.Value = e.Cell.OriginalValue;
                                e.Cell.CancelUpdate();
                                return;
                            }
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
                            break;

                        case COL_CumQty:
                            outResult = 0.0;
                            temp = double.TryParse(e.Cell.Text, out outResult);
                            if (!temp)
                            {
                                grdAmendmend.CellChange -= new CellEventHandler(grdAmendmend_CellChange);
                                e.Cell.Value = e.Cell.OriginalValue;
                                grdAmendmend.CellChange += new CellEventHandler(grdAmendmend_CellChange);
                            }
                            else
                            {
                                if (double.Parse(e.Cell.Value.ToString()) > double.Parse(e.Cell.Row.Cells[CAPTION_Quantity].Value.ToString()))
                                {
                                    //modified by amit on 15.04.2015
                                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3384
                                    MessageBox.Show("Executed Quantity should be less than or equal to the Quantity!", "Warning");
                                    //Puneet: cell value was not reverted after showing the warning corrected    
                                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                                    {
                                        e.Cell.Value = e.Cell.OriginalValue;
                                        e.Cell.CancelUpdate();
                                        return;
                                    }
                                }
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                            }
                            break;

                        case COL_AvgPrice:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.AvgPrice_Changed);
                            break;

                        case COL_FXRate:
                            outResult = 0.0;
                            temp = double.TryParse(e.Cell.Text, out outResult);
                            if (!temp)
                            {
                                grdAmendmend.CellChange -= new CellEventHandler(grdAmendmend_CellChange);
                                e.Cell.Value = e.Cell.OriginalValue;
                                grdAmendmend.CellChange += new CellEventHandler(grdAmendmend_CellChange);
                            }
                            else
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.FxRate_Changed);
                            }
                            break;

                        case OrderFields.PROPERTY_SettCurrFXRate:
                            outResult = 0.0;
                            temp = double.TryParse(e.Cell.Text, out outResult);
                            if (!temp)
                            {
                                grdAmendmend.CellChange -= new CellEventHandler(grdAmendmend_CellChange);
                                e.Cell.Value = e.Cell.OriginalValue;
                                grdAmendmend.CellChange += new CellEventHandler(grdAmendmend_CellChange);
                            }
                            else
                            {
                                gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
                            }
                            break;

                        case OrderFields.PROPERTY_SETTLEMENTCURRENCYID:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrency_Changed);
                            break;

                        case OrderFields.PROPERTY_SettCurrFXRateCalc:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
                            break;

                        case OrderFields.PROPERTY_COMMISSION:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                            break;

                        case OrderFields.PROPERTY_SOFTCOMMISSION:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                            break;

                        case OrderFields.PROPERTY_OTHERBROKERFEES:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                            break;

                        case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                            break;

                        case OrderFields.PROPERTY_STAMPDUTY:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.StampDuty_Changed);
                            break;

                        case OrderFields.PROPERTY_TRANSACTIONLEVY:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TransactionLevy_Changed);
                            break;

                        case OrderFields.PROPERTY_CLEARINGFEE:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.ClearingFee_Changed);
                            break;

                        case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                            break;

                        case OrderFields.PROPERTY_MISCFEES:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.MiscFees_Changed);
                            break;

                        case OrderFields.PROPERTY_SECFEE:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.SecFee_Changed);
                            break;

                        case OrderFields.PROPERTY_OCCFEE:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.OccFee_Changed);
                            break;

                        case OrderFields.PROPERTY_ORFFEE:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.OrfFee_Changed);
                            break;

                        case COL_Venue:
                            // Check if venue is valid or not
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                            if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_venue, e.Cell.Text))
                            {
                                MessageBox.Show(this, "Please select a valid venue.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.Value = e.Cell.OriginalValue;
                                e.Cell.CancelUpdate();
                                return;
                            }
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.Venue_Changed);
                            break;

                        case COL_FXConversionMethodOperator:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
                            break;

                        case COL_ACCRUEDINTEREST:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                            break;
                        case COL_UNDERLYINGDELTA:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.UnderlyingDelta_Changed);
                            break;

                        case COL_COMMISSIONAMOUNT: gParent.AddTradeAction(TradeAuditActionType.ActionType.CommissionAmount_Changed);
                            break;

                        case COL_COMMISSIONRATE: gParent.AddTradeAction(TradeAuditActionType.ActionType.CommissionRate_Changed);
                            break;

                        case COL_SOFTCOMMISSIONAMOUNT: gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommissionAmount_Changed);
                            break;

                        case COL_SOFTCOMMISSIONRATE: gParent.AddTradeAction(TradeAuditActionType.ActionType.SoftCommissionRate_Changed);
                            break;

                        case COL_TransactionType:
                            // Check if transaction type is valid or not
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                            if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_transactionTypeList, e.Cell.Text))
                            {
                                MessageBox.Show(this, "Please select a valid transaction type.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.Value = e.Cell.OriginalValue;
                                e.Cell.CancelUpdate();
                                return;
                            }
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                            break;

                        case COL_CHANGECOMMENT: break;

                        default:
                            gParent.AddTradeAction(TradeAuditActionType.ActionType.TradeEdited);
                            break;
                    }
                }
                else
                {
                    if (e.Cell.Column.Key.Equals(COL_CumQty))
                    {
                        if (double.Parse(e.Cell.Value.ToString()) <= 0.0)
                        {
                            MessageBox.Show("Executed Quantity should be greater than zero!", "Warning");
                            grdAmendmend.AfterCellUpdate -= new CellEventHandler(grdAmendmend_AfterCellUpdate);
                            e.Cell.Value = e.Cell.OriginalValue;
                            grdAmendmend.AfterCellUpdate += new CellEventHandler(grdAmendmend_AfterCellUpdate);
                            return;
                        }
                    }
                    else
                    {
                        e.Cell.Value = e.Cell.OriginalValue;
                    }
                }


                AllocationGroup allGroup = (AllocationGroup)e.Cell.Row.ListObject;
                allGroup.IsRecalculateCommission = false; //Initial value set before taking the user input
                grdAmendmend.AfterCellUpdate -= new CellEventHandler(grdAmendmend_AfterCellUpdate);

                switch (e.Cell.Column.Key)
                {
                    case OrderFields.PROPERTY_COMMISSION:
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.Commission_Changed);
                        break;
                    case OrderFields.PROPERTY_SOFTCOMMISSION:
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.SoftCommission_Changed);
                        break;
                    case COl_AUECLocalDate:
                        GetSettlementDate(allGroup);
                        break;

                    case COL_OrderSide:
                        if (result == DialogResult.Yes)
                        {
                            allGroup.OrderSideTagValue = e.Cell.Value.ToString();
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            allGroup.OrderSide = e.Cell.Text;
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                            allGroup.TransactionType = Regex.Replace(e.Cell.Text, @"\s+", "");

                        }
                        else if (result == DialogResult.No)
                        {
                            allGroup.OrderSideTagValue = e.Cell.Value.ToString();
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            allGroup.OrderSide = e.Cell.Text;
                        }
                        break;

                    case COL_CounterPartyName:
                        if (e.Cell.Text != null)
                        {
                            // Check if counter party is valid or not
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                            if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_counterParty, e.Cell.Text))
                            {
                                MessageBox.Show(this, "Please select a valid counter party.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.Value = e.Cell.OriginalValue;
                                e.Cell.CancelUpdate();
                                return;
                            }
                            allGroup.CounterPartyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyID(e.Cell.Text.ToString());

                            // calculate commission again if counterparty is changed, PRANA-13007
                            DialogResult choice = MessageBox.Show(this, "Would you like to calculate commission and fee again?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (choice == DialogResult.Yes)
                            {
                                allGroup.IsRecalculateCommission = (choice == DialogResult.Yes) ? true : false;

                                BackgroundWorker bgWorkerUpdateCommission = new BackgroundWorker();
                                bgWorkerUpdateCommission.DoWork += new DoWorkEventHandler(CalculateCommission_DoWork);
                                bgWorkerUpdateCommission.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CalculateCommission_RunWorkerCompleted);
                                bgWorkerUpdateCommission.RunWorkerAsync(allGroup);
                                }
                            }
                        break;

                    case COL_Venue:
                        // Check if venue is valid or not
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                        if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_venue, e.Cell.Text))
                        {
                            MessageBox.Show(this, "Please select a valid venue.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cell.Value = e.Cell.OriginalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }
                        allGroup.VenueID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetVenueID(e.Cell.Text.ToString());
                        allGroup.Venue = e.Cell.Text.ToString();
                        break;

                    case COL_FXConversionMethodOperator:
                        allGroup.FXConversionMethodOperator = e.Cell.Text.ToString();
                        break;

                    case OrderFields.PROPERTY_SETTLEMENTCURRENCYID:
                        int SettlCurrency;
                        if (int.TryParse(e.Cell.Value.ToString(), out SettlCurrency))
                        {
                            allGroup.SettlementCurrencyID = SettlCurrency;
                        }
                        break;

                    case OrderFields.PROPERTY_SettCurrFXRateCalc:
                        allGroup.SettlCurrFxRateCalc = e.Cell.Text.ToString();
                        break;
                }

                if (e.Cell.Column.DataType.Equals(typeof(DateTime)))
                {
                    DateTime cellDate = Convert.ToDateTime(e.Cell.Value);
                    if (cellDate < DateTimeConstants.MinValue)
                    {
                        e.Cell.CancelUpdate();
                        MessageBox.Show("Entered Date cannot be less than 1/1/1800, it will be reverted to current Date", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        grdAmendmend.DisplayLayout.Rows[0].Cells[e.Cell.Column].Value = DateTime.Now;
                    }
                }
                if (e.Cell.Row.Cells[CAPTION_AssetName].Value.ToString().Equals(AssetCategory.FixedIncome.ToString()) || e.Cell.Row.Cells[CAPTION_AssetName].Value.ToString().Equals(AssetCategory.ConvertibleBond.ToString()))
                {
                    switch (e.Cell.Column.Key)
                    {
                        case COl_AUECLocalDate:
                        case COl_OriginalPurchaseDate:
                        case COl_ProcessDate:
                        case COL_SettlementDate:
                        case COL_CumQty:
                            AllocationGroup gParent = (AllocationGroup)e.Cell.Row.ListObject;

                            _groupID = allGroup.GroupID;
                            SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                            string symbol = e.Cell.Row.Cells[CAPTION_Symbol].Value.ToString();
                            secMasterReqobj.AddData(symbol, ApplicationConstants.PranaSymbology);
                            secMasterReqobj.HashCode = this.GetHashCode();
                            _securityMaster.SendRequest(secMasterReqobj);
                            break;

                        default: break;
                    }
                }

                if (allGroup.PersistenceStatus != ApplicationConstants.PersistenceStatus.New)
                {
                    if (allGroup.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                    {
                        allGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    }
                    else
                    {
                        allGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                    }
                    TaxLot updatedTaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot((PranaBasicMessage)allGroup, allGroup.GroupID);
                    allGroup.UpdateTaxlotState(updatedTaxlot);
                    allGroup.IsModified = true;
                    allGroup.CompanyUserID = _userID;
                    if (allGroup.Orders.Count == 1)
                    {
                        allGroup.Orders[0].IsModified = true;
                        allGroup.Orders[0].AvgPrice = allGroup.AvgPrice;
                        allGroup.Orders[0].CumQty = allGroup.CumQty;
                        allGroup.Orders[0].Description = allGroup.Description;
                        allGroup.Orders[0].InternalComments = allGroup.InternalComments;
                    }
                }
                //Issues : http://jira.nirvanasolutions.com:8080/browse/PRANA-2106 and http://jira.nirvanasolutions.com:8080/browse/PRANA-2115
                //Updating order object in allocation group to keep same data in AllocationGroup Object, so that operation can be performed on 
                //this without refreshing data from database. This is to be done if only one Order object is in AllocationGroup object
                if (allGroup.Orders.Count == 1)
                {
                    allGroup.Orders[0].IsModified = true;
                    allGroup.Orders[0].AUECLocalDate = allGroup.AUECLocalDate;
                    allGroup.Orders[0].OriginalPurchaseDate = allGroup.OriginalPurchaseDate;
                    allGroup.Orders[0].ProcessDate = allGroup.ProcessDate;
                    allGroup.Orders[0].SettlementDate = allGroup.SettlementDate;
                    allGroup.Orders[0].CumQty = allGroup.CumQty;
                    allGroup.Orders[0].AvgPrice = allGroup.AvgPrice;
                    allGroup.Orders[0].Venue = allGroup.Venue;
                    allGroup.Orders[0].VenueID = allGroup.VenueID;
                    allGroup.Orders[0].CounterPartyID = allGroup.CounterPartyID;
                    allGroup.Orders[0].CounterPartyName = allGroup.CounterPartyName;
                    allGroup.Orders[0].OrderSideTagValue = allGroup.OrderSideTagValue;
                    allGroup.Orders[0].OrderSide = allGroup.OrderSide;
                    allGroup.Orders[0].FXRate = allGroup.FXRate;
                    allGroup.Orders[0].FXConversionMethodOperator = allGroup.FXConversionMethodOperator;
                    //Updatingattributes in order
                    allGroup.Orders[0].TradeAttribute1 = allGroup.TradeAttribute1;
                    allGroup.Orders[0].TradeAttribute2 = allGroup.TradeAttribute2;
                    allGroup.Orders[0].TradeAttribute3 = allGroup.TradeAttribute3;
                    allGroup.Orders[0].TradeAttribute4 = allGroup.TradeAttribute4;
                    allGroup.Orders[0].TradeAttribute5 = allGroup.TradeAttribute5;
                    allGroup.Orders[0].TradeAttribute6 = allGroup.TradeAttribute6;
                    allGroup.Orders[0].SettlCurrFxRate = allGroup.SettlCurrFxRate;
                    allGroup.Orders[0].SettlCurrFxRateCalc = allGroup.SettlCurrFxRateCalc;
                    allGroup.Orders[0].SettlementCurrencyID = allGroup.SettlementCurrencyID;
                    allGroup.Orders[0].SettlCurrAmt = allGroup.SettlCurrAmt;
                }
                grdAmendmend.AfterCellUpdate += new CellEventHandler(grdAmendmend_AfterCellUpdate);

                if (e.Cell.Column.Key.StartsWith("TradeAttribute"))
                {
                    updateAttribList(e);
                }
                UpdateSettlementFields(e, allGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks if a string exists in Valuelist items display text
        /// </summary>
        /// <param name="myValueList">the valuelist</param>
        /// <param name="text">text string</param>
        /// <returns>true if list contains text, false otherwise</returns>
        private bool IsValidText(ValueList myValueList, string text)
        {
            bool isValid = false;
            try
            {
                foreach (ValueListItem item in myValueList.ValueListItems)
                {
                    if (item.DisplayText.Equals(text))
                    {
                        isValid = true;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// this function checks whether Transaction Type will be changed on the basis of side
        /// Case 1: If side and Transaction Type have same values i.e. Transaction Type is super set of side, then Transaction Type will automatically change to side
        /// Case 2: If side and Transaction Type have not same value then ask to make same transaction type as Side
        /// </summary>
        /// <param name="e">Cell value</param>
        /// <returns>Yes/No/Cancel</returns>
        private DialogResult SetTransactionTypeBasedOnSide(CellEventArgs e, AllocationGroup grp)
        {
            DialogResult result = DialogResult.No;
            try
            {
                string orderSideTagValue = grp.OrderSideTagValue.ToString();
                string orderSideValue_Original = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideTagValue);
                string transactionType = grp.TransactionType.ToString();
                if (orderSideValue_Original.Equals(transactionType))
                {
                    result = DialogResult.Yes;
                    return result;
                }
                else
                {
                    if (e.Cell.Column.Key.Equals(COL_OrderSide) && orderSideValue_Original != transactionType)
                    {
                        result = MessageBox.Show("Transaction Type is '" + transactionType + "'. Changing the Side, Transaction Type will also change, do you want to change Transaction Type also?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow row = grdAmendmend.ActiveRow;
                if (row != null)
                {
                    AllocationGroup allGroup = (AllocationGroup)row.ListObject;
                    if (grdAmendmend.DataSource == null || grdAmendmend.Rows.Count == 0)
                    {
                        menuItem1.Enabled = false;
                        return;
                    }
                    if (grdAmendmend.ActiveRow == null || grdAmendmend.ActiveRow.Band.Index != 0)
                    {
                        menuItem1.Enabled = false;
                        return;
                    }

                    menuItem1.Enabled = true;
                }
                else
                {
                    menuItem1.Enabled = false;
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
        /// Enable/disable menu in menustrip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu2_Popup(object sender, EventArgs e)
        {
            try
            {
                //modified by amit.changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3597
                if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                {
                    mnuCostAdjustment.Enabled = false;
                }
                else
                {
                    List<AllocationGroup> groups = GetSelectedGroups(grdCommission);
                    //Commented to disable CostAdjustment, PRANA-13168
                    //if (groups.Count > 0)
                    //    mnuCostAdjustment.Enabled = true;
                    //else
                        mnuCostAdjustment.Enabled = false;
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

        void grdCommission_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            try
            {
                TaxLot taxlot = null;
                if (grdCommission.ActiveRow != null)
                {
                    if (grdCommission.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    if (e.Cell.Column.Key.Equals(COL_ExternalTransId))
                    {
                        taxlot = grdCommission.ActiveRow.ListObject as TaxLot;
                        if (taxlot != null)
                        {
                            string externalTransactionID = GeteExternalTrasactionIDs(taxlot.TaxLotID);

                            if (externalTransactionID != null && !externalTransactionID.Equals("Cancelled"))
                            {
                                e.Cell.SelText = externalTransactionID;
                                e.Cell.Value = externalTransactionID;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string GeteExternalTrasactionIDs(string taxlotID)
        {
            string externalTransactionID = string.Empty;
            try
            {
                AddAndUpdateExternalTransactionID frmExternalTransactionID = AddAndUpdateExternalTransactionID.GetInstance();
                frmExternalTransactionID.StartPosition = FormStartPosition.CenterParent;
                frmExternalTransactionID.AllocationServices = _allocationServices;
                frmExternalTransactionID.SetUp(taxlotID);
                frmExternalTransactionID.ShowDialog();
                externalTransactionID = frmExternalTransactionID.ExternalTransactionIDs;
                if (frmExternalTransactionID != null)
                {
                    frmExternalTransactionID = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return externalTransactionID;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AllocationGroup deleteGroup = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                DialogResult userChoice = DialogResult.Yes;
                if (!deleteGroup.IsManualGroup && deleteGroup.NotAllExecuted)
                {
                    userChoice = MessageBox.Show("This Fix Trade is partially  executed,Do you want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }
                if (userChoice == DialogResult.Yes)
                {
                    AllocationManager.GetInstance().DeleteGroupCancelAmendUI(deleteGroup);
                    grdAmendmend.ActiveRow = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAmendmend_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.ListObject != null)
                {
                    if (e.Row.ListObject.GetType().Name == "AllocationGroup")
                    {
                        AllocationGroup allGroup = (AllocationGroup)e.Row.ListObject;
                        e.Row.Cells[CAPTION_ExpirationDate].Activation = Activation.NoEdit;
                        grdAmendmend.AfterCellUpdate -= new CellEventHandler(grdAmendmend_AfterCellUpdate);
                        e.Row.Cells[COL_OrderSide].ValueList = _sides;
                        SetAssetCategoryValue(e.Row);
                        grdAmendmend.AfterCellUpdate += new CellEventHandler(grdAmendmend_AfterCellUpdate);
                        updateAttriblistsForGroup(grdAmendmend, allGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetAssetCategoryValue(UltraGridRow row)
        {
            try
            {
                if (row != null && row.ListObject != null)
                {
                    if (row.ListObject.GetType().Name.Equals("AllocationGroup"))
                    {
                        if (row.Cells[CAPTION_AssetName].Value.ToString().Equals("Equity") && row.Cells["IsSwapped"].Value.Equals(true))
                        {
                            string swap = "Equity swap";
                            row.Cells["AssetCategory"].Value = (object)swap;
                        }
                        else
                        {
                            if (row.Cells[CAPTION_AssetName].Value.ToString() != "")
                            {
                                row.Cells["AssetCategory"].Value = row.Cells[CAPTION_AssetName].Value;
                            }
                        }
                    }
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
        }

        private void grdCommission_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                grdCommission.AfterCellUpdate -= new CellEventHandler(grdCommission_AfterCellUpdate);
                AllocationGroup group = e.Row.ListObject as AllocationGroup;
                //Modified the function to set account and strategy column value, PRANA-4526
                HashSet<String> accountName = new HashSet<string>();
                HashSet<String> strategyName = new HashSet<string>();
                if (group != null)
                {
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        accountName.Add(taxlot.Level1Name);
                        strategyName.Add(taxlot.Level2Name);
                    }
                    if (accountName.Count > 0)
                    {
                        if (accountName.Count > 1)
                            e.Row.Cells[COL_FUNDNAME].Value = ApplicationConstants.C_Multiple;
                        else
                            e.Row.Cells[COL_FUNDNAME].Value = accountName.First();
                    }
                    if (strategyName.Count > 0)
                    {
                        if (strategyName.Count > 1)
                            e.Row.Cells[COL_STRATEGY].Value = ApplicationConstants.C_Multiple;
                        else
                            e.Row.Cells[COL_STRATEGY].Value = strategyName.First();
                    }
                    updateAttriblistsForGroup(grdCommission, group);
                }
                SetAssetCategoryValue(e.Row);
                grdCommission.AfterCellUpdate += new CellEventHandler(grdCommission_AfterCellUpdate);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetSettlementDate(AllocationGroup group)
        {
            try
            {
                int auecID = Convert.ToInt32(group.AUECID);
                string sideText = group.OrderSide.ToString();
                if (sideText != "0")
                {
                    string sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);
                    int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                    DateTime tradeDate = Convert.ToDateTime(group.AUECLocalDate.ToString());
                    if (auecSettlementPeriod == 0)
                    {
                        group.SettlementDate = tradeDate;
                    }
                    else
                    {
                        group.SettlementDate = Prana.Utilities.DateTimeUtilities.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void grdCommission_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                DialogResult result = DialogResult.Cancel;
                AllocationGroup allGroup = null;
                if (e.Cell.Row.ListObject.GetType().Name.Equals("AllocationGroup"))
                {
                    allGroup = (AllocationGroup)e.Cell.Row.ListObject;
                    allGroup.IsRecalculateCommission = false; //Initial value set before taking the user input
                    if (e.Cell.Column.Key.Equals(COl_AUECLocalDate))
                    {
                        GetSettlementDate(allGroup);
                        DateTime TradeDate = Convert.ToDateTime(e.Cell.Value);
                        // if trade date is changed, process date and original purchase date are set to trade date.
                        allGroup.AUECLocalDate = TradeDate.Date;
                        allGroup.ProcessDate = TradeDate.Date;
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-967
                        // Date were not getting change on edit trades UI
                        allGroup.AllocationDate = TradeDate.Date;
                        allGroup.OriginalPurchaseDate = TradeDate.Date;
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        allGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                        // if trade date is changed, process date and original purchase date are set to trade date.
                        allGroup.TaxLots[0].ProcessDate = TradeDate.Date;
                        allGroup.TaxLots[0].OriginalPurchaseDate = TradeDate.Date;
                        allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                        allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                        // Modified by Ankit Gupta on 31 Oct, 2014.
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1669
                        if (DateTime.Compare(allGroup.NirvanaProcessDate, TradeDate.Date) < 0)
                        {
                            allGroup.TaxLots[0].NirvanaProcessDate = TradeDate.Date;
                            allGroup.NirvanaProcessDate = TradeDate.Date;
                        }
                        allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                        allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                    }

                    if (e.Cell.Column.Key.Equals(COl_ProcessDate))
                    {
                        DateTime ProcessDate = Convert.ToDateTime(e.Cell.Value);
                        if (ProcessDate.Date < allGroup.AUECLocalDate.Date)
                        {
                            MessageBox.Show("Process Date cannot be less than Trade Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                        if (ProcessDate.Date > allGroup.SettlementDate.Date)
                        {
                            MessageBox.Show("Process Date cannot be greater than Settlement Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                        else
                        {
                            // if process date is changed, original purchase date is set equal to the process date.
                            allGroup.OriginalPurchaseDate = ProcessDate.Date;
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);

                            allGroup.TaxLots[0].ProcessDate = ProcessDate.Date;
                            allGroup.TaxLots[0].OriginalPurchaseDate = ProcessDate.Date;
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        }
                    }

                    if (e.Cell.Column.Key.Equals(COl_OriginalPurchaseDate))
                    {
                        DateTime OriginalPurchaseDate = Convert.ToDateTime(e.Cell.Value);
                        // original purchase is dependent on process date, it cannot be greater than the process date.
                        if (OriginalPurchaseDate.Date > allGroup.ProcessDate.Date)
                        {
                            MessageBox.Show("OriginalPurchase Date cannot be greater than Process Date, it will be reverted to Last Valid Value.", "Warning!", MessageBoxButtons.OK);
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                        else
                        {
                            allGroup.OriginalPurchaseDate = OriginalPurchaseDate.Date;
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                            allGroup.TaxLots[0].OriginalPurchaseDate = OriginalPurchaseDate.Date;
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        }
                    }

                    if (e.Cell.Column.Key.Equals(COL_SettlementDate))
                    {
                        DateTime settlementDate = Convert.ToDateTime(e.Cell.Value);
                        //Settlement date can not be less than the trade date.
                        if (settlementDate < allGroup.ProcessDate.Date)
                        {
                            MessageBox.Show("Settlement Date cannot be less than Process Date,it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                        else
                        {
                            //check that group is allocated to only one account                           
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                            allGroup.TaxLots[0].SettlementDate = settlementDate;
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                        }
                    }
                    // Modified by Ankit Gupta on 31 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1669
                    if (e.Cell.Column.Key.Equals(COL_NirvanaProcessDate))
                    {
                        if (DateTime.Compare(allGroup.NirvanaProcessDate, allGroup.TaxLots[0].NirvanaProcessDate) < 0)
                        {
                            MessageBox.Show("Prana Process Date cannot be less than Trade Date, it will be reverted to Last Valid Value.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                    }

                    if (e.Cell.Column.Key.Equals(COL_CumQty))
                    {
                        double outResult = 0.0;
                        bool temp = double.TryParse(e.Cell.Text, out outResult);
                        if (!temp)
                        {
                            grdCommission.CellChange -= new CellEventHandler(grdCommission_CellChange);
                            e.Cell.Value = e.Cell.OriginalValue;
                            grdCommission.CellChange += new CellEventHandler(grdCommission_CellChange);
                        }
                        else
                        {
                            if (double.Parse(e.Cell.Value.ToString()) > double.Parse(e.Cell.Row.Cells[CAPTION_Quantity].Value.ToString()))
                            {
                                //modified by amit on 15.04.2015
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3384
                                MessageBox.Show("Executed Quantity should be less than or equal to the Quantity!", "Warning");
                                //Puneet: cell value was not reverted after showing the warning corrected    
                                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                                {
                                    e.Cell.Value = e.Cell.OriginalValue;
                                    e.Cell.CancelUpdate();
                                    return;
                                }
                            }
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                            allGroup.TaxLots[0].TaxLotQty = outResult;
                            allGroup.AllocatedQty = outResult;
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
                        }
                    }

                    if (e.Cell.Column.Key.Equals(COL_OrderSide))
                    {
                        AllocationGroup grp = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                        if (grp != null)
                        {
                            result = SetTransactionTypeBasedOnSide(e, grp);
                        }
                        if (result == DialogResult.Yes)
                        {
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            allGroup.TaxLots[0].AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                        }
                        else if (result == DialogResult.No)
                        {
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                        }
                    }

                    if (e.Cell.Column.Key.Equals(COL_CounterPartyName))
                    {
                        // Check if counter party is valid or not
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7676
                        if (!string.IsNullOrEmpty(e.Cell.Text) && !IsValidText(_counterPartyAllocated, e.Cell.Text))
                        {
                            MessageBox.Show(this, "Please select a valid counter party.", "Nirvana Allocation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            e.Cell.Value = e.Cell.OriginalValue;
                            e.Cell.CancelUpdate();
                            return;
                        }
                        allGroup.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(e.Cell.Text);

                        // calculate commission again if counterparty is changed, PRANA-13007
                        DialogResult choice = MessageBox.Show(this, "Would you like to calculate commission and fee again?", "Nirvana Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (choice == DialogResult.Yes)
                        {
                            allGroup.IsRecalculateCommission = (choice == DialogResult.Yes) ? true : false;

                                BackgroundWorker bgWorkerUpdateCommission = new BackgroundWorker();
                                bgWorkerUpdateCommission.DoWork += new DoWorkEventHandler(CalculateCommission_DoWork);
                                bgWorkerUpdateCommission.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CalculateCommission_RunWorkerCompleted);
                                bgWorkerUpdateCommission.RunWorkerAsync(allGroup);
                            }
                        }

                    if (e.Cell.Column.Key.Equals(COL_FXConversionMethodOperator))
                    {
                        allGroup.FXConversionMethodOperator = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRateCalc))
                    {
                        allGroup.SettlCurrFxRateCalc = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                    {
                        int settlCurrency;
                        if (int.TryParse(e.Cell.Value.ToString(), out  settlCurrency))
                        {
                            allGroup.SettlementCurrencyID = settlCurrency;
                        }
                    }
                    if (e.Cell.Column.Key.Equals("Description"))
                    {
                        allGroup.Description = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_INTERNALCOMMENTS))
                    {
                        allGroup.InternalComments = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute1))
                    {
                        allGroup.TradeAttribute1 = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute2))
                    {
                        allGroup.TradeAttribute2 = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute3))
                    {
                        allGroup.TradeAttribute3 = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute4))
                    {
                        allGroup.TradeAttribute4 = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute5))
                    {
                        allGroup.TradeAttribute5 = e.Cell.Text;
                    }
                    if (e.Cell.Column.Key.Equals(COL_TradeAttribute6))
                    {
                        allGroup.TradeAttribute6 = e.Cell.Text;
                    }
                    //Code moved to PranaBasicMessage
                    UpdateSettlementFields(e, allGroup);

                    if (e.Cell.Column.Key.Equals(COL_OrderSide))
                    {
                        allGroup = (AllocationGroup)grdCommission.ActiveRow.ListObject;
                        if (result == DialogResult.Yes)
                        {
                            allGroup.OrderSideTagValue = e.Cell.Value.ToString();
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            allGroup.OrderSide = e.Cell.Text;
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                            allGroup.TransactionType = e.Cell.Text;
                        }
                        else if (result == DialogResult.No)
                        {
                            allGroup.OrderSideTagValue = e.Cell.Value.ToString();
                            allGroup.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
                            allGroup.OrderSide = e.Cell.Text;
                        }
                    }

                    if (AllocationManager.GetInstance().DictUnsavedContainsKey(allGroup.GroupID) && !e.Cell.Column.Key.Equals("checkBox"))
                    {
                        allGroup.UpdateGroupPersistenceStatus();
                        allGroup.UpdateGroupTaxlots(e.Cell.Column.Key, e.Cell.Text);
                    }
                }
                if (e.Cell.Row.ListObject.GetType().Name.Equals("TaxLot"))
                {
                    allGroup = (AllocationGroup)e.Cell.Row.ParentRow.ListObject;
                    AllocationManager.GetInstance().UpdateFieldsForTaxLotAfterCellUpdate(e.Cell.Column.Key, e.Cell.Value.ToString(), allGroup);
                }


                if (allGroup.Orders.Count == 1)
                {
                    allGroup.Orders[0].IsModified = true;
                    allGroup.Orders[0].AUECLocalDate = allGroup.AUECLocalDate;
                    allGroup.Orders[0].OriginalPurchaseDate = allGroup.OriginalPurchaseDate;
                    allGroup.Orders[0].ProcessDate = allGroup.ProcessDate;
                    allGroup.Orders[0].SettlementDate = allGroup.SettlementDate;
                    allGroup.Orders[0].CumQty = allGroup.CumQty;
                    allGroup.Orders[0].AvgPrice = allGroup.AvgPrice;
                    allGroup.Orders[0].Venue = allGroup.Venue;
                    allGroup.Orders[0].VenueID = allGroup.VenueID;
                    allGroup.Orders[0].CounterPartyID = allGroup.CounterPartyID;
                    allGroup.Orders[0].CounterPartyName = allGroup.CounterPartyName;
                    allGroup.Orders[0].OrderSideTagValue = allGroup.OrderSideTagValue;
                    allGroup.Orders[0].OrderSide = allGroup.OrderSide;
                    allGroup.Orders[0].FXRate = allGroup.FXRate;
                    allGroup.Orders[0].FXConversionMethodOperator = allGroup.FXConversionMethodOperator;
                    //Updatingattributes in order
                    allGroup.Orders[0].TradeAttribute1 = allGroup.TradeAttribute1;
                    allGroup.Orders[0].TradeAttribute2 = allGroup.TradeAttribute2;
                    allGroup.Orders[0].TradeAttribute3 = allGroup.TradeAttribute3;
                    allGroup.Orders[0].TradeAttribute4 = allGroup.TradeAttribute4;
                    allGroup.Orders[0].TradeAttribute5 = allGroup.TradeAttribute5;
                    allGroup.Orders[0].TradeAttribute6 = allGroup.TradeAttribute6;
                    allGroup.Orders[0].SettlCurrFxRate = allGroup.SettlCurrFxRate;
                    allGroup.Orders[0].SettlCurrFxRateCalc = allGroup.SettlCurrFxRateCalc;
                    allGroup.Orders[0].SettlementCurrencyID = allGroup.SettlementCurrencyID;
                    allGroup.Orders[0].SettlCurrAmt = allGroup.SettlCurrAmt;
                }

                //for chmw one account will be allocated 100% to taxlot
                if (allGroup.TaxLots.Count == 1)
                    AllocationManager.GetInstance().AddAccountIdToLockData(allGroup.TaxLots[0].Level1ID);
                if (e.Cell.Column.Key.StartsWith("TradeAttribute"))
                {
                    updateAttribList(e);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// calculate commission for group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateCommission_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AllocationGroup group = (AllocationGroup)e.Argument;
                AllocationGroup allocationGroup = AllocationManager.CalculateCommission(group);
                e.Result = allocationGroup;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set commission of existing group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateCommission_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.IsDisposed || this.Disposing)
                    return;
                if ((e.Cancelled == true))
                {
                    toolStripStatusLabel1.Text = "Cancelled";
                }
                else if (!(e.Error == null))
                {
                    toolStripStatusLabel1.Text = "Error: " + e.Error.Message;
                }
                else
                {
                    if (e.Result != null)
                    {
                        AllocationGroup group = (AllocationGroup)e.Result;
                        if (group != null)
                            AllocationManager.GetInstance().AddGroup(group);
                        else
                            toolStripStatusLabel1.Text = "Error during calculating commission! Please contact administrator.";
                    }
                    else
                        toolStripStatusLabel1.Text = "Error during calculating commission! Please contact administrator.";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// create by sachin mishra jira-CHMW-3481 30/04/15
        /// Code moved to PranaBasicMessage
        /// </summary>
        /// <param name="e"></param>
        /// <param name="allGroup"></param>
        private static void UpdateSettlementFields(CellEventArgs e, AllocationGroup allGroup)
        {
            try
            {
                ////update settlement amount if there is a change in settlement fx rate
                //if (e.Cell.Column.Key.Equals(COL_AvgPrice))
                //{
                //    allGroup.AvgPrice = double.Parse(e.Cell.Text);
                //    if (allGroup.SettlCurrFxRateCalc.Equals(Operator.D.ToString()))
                //    {
                //        if (allGroup.SettlCurrFxRate > 0)
                //            allGroup.SettlCurrAmt = allGroup.AvgPrice / allGroup.SettlCurrFxRate;
                //    }
                //    else
                //    {
                //        allGroup.SettlCurrAmt = allGroup.AvgPrice * allGroup.SettlCurrFxRate;
                //    }
                //}
                ////update settlement amount if there is a change in settlement fx rate
                //if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRate))
                //{
                //    allGroup.SettlCurrFxRate = double.Parse(e.Cell.Text);
                //    if (allGroup.SettlCurrFxRateCalc.Equals(Operator.D.ToString()))
                //    {
                //        if (allGroup.SettlCurrFxRate > 0)
                //            allGroup.SettlCurrAmt = allGroup.AvgPrice / allGroup.SettlCurrFxRate;
                //    }
                //    else
                //    {
                //        allGroup.SettlCurrAmt = allGroup.AvgPrice * allGroup.SettlCurrFxRate;
                //    }
                //}
                ////update settlement fx rate and settlement amount if there is a change in SettlCurrFxRateCalc
                //if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRateCalc))
                //{
                //    allGroup.SettlCurrFxRateCalc = e.Cell.Text;
                //    if (e.Cell.Text.Equals(Operator.D.ToString()))
                //    {
                //        if (allGroup.SettlCurrFxRate > 0)
                //            allGroup.SettlCurrAmt = allGroup.AvgPrice / allGroup.SettlCurrFxRate;
                //    }
                //    else
                //    {
                //        allGroup.SettlCurrAmt = allGroup.AvgPrice * allGroup.SettlCurrFxRate;
                //    }
                //}
                ////update settlement fx rate if there is a change in settlement amount
                //if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT))
                //{
                //if (allGroup.AvgPrice == 0)
                //    allGroup.SettlCurrAmt = 0;
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string _groupID = string.Empty;

        private double CalculateAccruedInterest(AllocationGroup group)
        {
            double accruedInterest = 0.0;
            List<PranaBasicMessage> lstAllocationGroup = new List<PranaBasicMessage>();
            try
            {
                lstAllocationGroup.Add(group);
                lstAllocationGroup = _cashManagementServices.InnerChannel.CalculateAccruedInterest(lstAllocationGroup);
                foreach (PranaBasicMessage obj in lstAllocationGroup)
                {
                    accruedInterest = obj.AccruedInterest;
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
            return accruedInterest;
        }

        /// <summary>
        /// Save Layout will save all the layout including filters, width, sorting, captions and visible 
        /// columns rather than saving just the column names.
        /// See Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1710
        /// </summary>
        CancelAmendPreferences _cancelAmendPref = null;
        private void menuSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                _cancelAmendPref.GrdAmendColums = GetGridColumnLayout(grdAmendmend); //UltraWinGridUtils.GetColumnsString(grdAmendmend);
                _cancelAmendPref.GrdCommissionColumns = GetGridColumnLayout(grdCommission); //UltraWinGridUtils.GetColumnsString(grdCommission);
                //To Save the splitter Position on Save Layout Click
                _cancelAmendPref.SplitPanelSize = GetSplitPanelsSize();
                SavePreferences();
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
        /// To get the splitter Position on Save Layout Click
        /// </summary>
        /// <returns></returns>
        private string GetSplitPanelsSize()
        {
            try
            {
                string panelsizes = string.Empty;
                panelsizes += splitter1.Location.X.ToString() + "," + splitter1.Location.Y.ToString() + "," + splitter1.SplitPosition.ToString();
                return panelsizes;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }
        private void menuUnallocatedAuditTrail_Click(object sender, EventArgs e)
        {

            try
            {
                List<string> groupIds = new List<string>();

                if (grdAmendmend.ActiveRow != null)
                {
                    if (grdAmendmend.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    AllocationGroup group = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                    if (group != null)
                    {
                        groupIds.Add(group.GroupID);
                        GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        //AuditManager.Instance.LaunchAuditUI(groupIds);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private List<String> GetSelectedGroupIds(UltraGrid grid)
        {
            List<String> allocationGroups = new List<String>();
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Value.ToString().ToLower().Equals("true"))
                    {
                        allocationGroups.Add(((AllocationGroup)row.ListObject).GroupID);
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
            return allocationGroups;
        }
        private void menuAllocatedAuditTrail_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> groupIds = GetSelectedGroupIds(grdCommission);
                if (groupIds.Count > 0)
                {
                    GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                    //AuditManager.Instance.LaunchAuditUI(groupIds);
                }
                else
                {
                    if (grdCommission.ActiveRow != null)
                    {
                        if (grdCommission.ActiveRow is UltraGridGroupByRow)
                        {
                            return;
                        }
                        AllocationGroup group = grdCommission.ActiveRow.ListObject as AllocationGroup;
                        if (group != null)
                        {
                            groupIds.Add(group.GroupID);
                            GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids", new object[] { groupIds })));
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void menuUNAllocatedEditPanel_Click(object sender, EventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                if (grdAmendmend.ActiveRow != null)
                {
                    if (grdAmendmend.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    ctrlAmendSingleGroup1.ChangeEnableStatus(false);
                    group = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                    if (group != null)
                    {
                        EditGroupDetails(group);
                    }

                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void menuAllocatedEditPanel_Click(object sender, EventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                if (grdCommission.ActiveRow != null)
                {
                    if (grdCommission.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    group = grdCommission.ActiveRow.ListObject as AllocationGroup;
                    if (group != null)
                    {
                        this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                        ctrlAmendSingleGroup1.ChangeEnableStatus(true);
                        EditGroupDetails(group);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        string _cancelAmendPrefFilePath = string.Empty;
        string _cancelAmendPrefDirectoryPath = string.Empty;

        private void SavePreferences()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(_cancelAmendPrefFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(CancelAmendPreferences));
                    serializer.Serialize(writer, _cancelAmendPref);

                    writer.Flush();
                    //writer.Close(); // commenting as object should not be disposed multiple times - MS managed rules
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LoadPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            _startPath = System.Windows.Forms.Application.StartupPath;
            _cancelAmendPrefDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + _userID.ToString();
            _cancelAmendPrefFilePath = _cancelAmendPrefDirectoryPath + @"\CancelAmendPreferences.xml";
            _cancelAmendPref = new CancelAmendPreferences();
            //add taxlotAttributes
            addStringColumns();
            try
            {
                if (!Directory.Exists(_cancelAmendPrefDirectoryPath))
                {
                    Directory.CreateDirectory(_cancelAmendPrefDirectoryPath);
                }
                if (File.Exists(_cancelAmendPrefFilePath))
                {
                    //_cancelAmendPref = (CancelAmendPreferences)_Xml.ReadXml(_cancelAmendPrefFilePath, _cancelAmendPref);
                    using (FileStream fs = File.OpenRead(_cancelAmendPrefFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CancelAmendPreferences));
                        _cancelAmendPref = (CancelAmendPreferences)serializer.Deserialize(fs);
                    }
                }
            }

            #region Catch
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
            #endregion
        }

        private void grdAmendmend_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAmendmend);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCommission_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdCommission);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAmendmend_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        private void grdAmendmend_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Caption = gridCol.Header.Caption;
                    colData.Hidden = gridCol.Hidden;
                    colData.Format = gridCol.Format;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.CellActivation = gridCol.CellActivation;
                    colData.SortIndicator = gridCol.SortIndicator;

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;
                    listGridCols.Add(colData);

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }

        /// <summary>
        /// The transaction type valuelist
        /// </summary>
        public static ValueList _transactionTypeList = new ValueList();

        public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData, List<string> DisplayList)
        {
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;
            listColData.Sort();
            try
            {
                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    if (DisplayList.Contains(colData.Key))
                    {
                        if (gridColumns.Exists(colData.Key))
                        {
                            UltraGridColumn gridCol = gridColumns[colData.Key];
                            gridCol.Width = colData.Width;
                            gridCol.Format = colData.Format;
                            gridCol.Header.Caption = colData.Caption;
                            gridCol.Header.VisiblePosition = colData.VisiblePosition;
                            gridCol.Hidden = colData.Hidden;
                            gridCol.Header.Fixed = colData.Fixed;
                            gridCol.SortIndicator = colData.SortIndicator;
                            gridCol.CellActivation = colData.CellActivation;

                            // Filter Settings
                            if (colData.FilterConditionList.Count > 0)
                            {
                                band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                                band.ColumnFilters[colData.Key].FilterConditions.Clear();
                                foreach (FilterCondition fCond in colData.FilterConditionList)
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }
                }
                UltraGridColumn colTradeAtt1 = band.Columns[COL_TradeAttribute1];
                colTradeAtt1.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute1);

                UltraGridColumn colTradeAtt2 = band.Columns[COL_TradeAttribute2];
                colTradeAtt2.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute2);

                UltraGridColumn colTradeAtt3 = band.Columns[COL_TradeAttribute3];
                colTradeAtt3.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute3);

                UltraGridColumn colTradeAtt4 = band.Columns[COL_TradeAttribute4];
                colTradeAtt4.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute4);

                UltraGridColumn colTradeAtt5 = band.Columns[COL_TradeAttribute5];
                colTradeAtt5.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute5);

                UltraGridColumn colTradeAtt6 = band.Columns[COL_TradeAttribute6];
                colTradeAtt6.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(CAPTION_TradeAttribute6);

                band.Columns[COL_INTERNALCOMMENTS].Header.Caption = CAP_INTERNALCOMMENTS;


                UltraGridColumn colClosingStatus = band.Columns[COL_ClosingStatus];
                colClosingStatus.Header.Caption = CAPTION_ClosingStatus;
                colClosingStatus.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingAlgo = band.Columns[OrderFields.PROPERTY_ClosingAlgoText];
                colClosingAlgo.Header.Caption = OrderFields.CAPTION_ClosingAlgo;
                colClosingAlgo.CellActivation = Activation.NoEdit;

                UltraGridColumn colTransactionType = band.Columns[COL_TransactionType];
                colTransactionType.Header.Caption = CAPTION_TransactionType; ;
                colTransactionType.CellActivation = Activation.AllowEdit;
                _transactionTypeList = CommonDataCache.CachedDataManager.GetInstance.GetTransactionTypeValueList().Clone();
                colTransactionType.ValueList = _transactionTypeList;

                if (band.Columns.Exists(COL_FUNDNAME))
                {
                    UltraGridColumn colAccountName = band.Columns[COL_FUNDNAME];
                    colAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = CAP_FUNDNAME;
                }
                //Added strategy column, PRANA-4526
                if (band.Columns.Exists(COL_STRATEGY))
                {
                    UltraGridColumn colStrategyName = band.Columns[COL_STRATEGY];
                    colStrategyName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colStrategyName.CellActivation = Activation.NoEdit;
                    colStrategyName.Header.Caption = CAPTION_LEVEL2NAME;
                }

                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_STAMPDUTY))
                {
                    UltraGridColumn colStampDuty = band.Columns[OrderFields.PROPERTY_STAMPDUTY];
                    colStampDuty.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_STAMPDUTY];
                }
                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_TRANSACTIONLEVY))
                {
                    UltraGridColumn colTransactionLevy = band.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                    colTransactionLevy.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_TRANSACTIONLEVY];
                }
                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_CLEARINGFEE))
                {
                    UltraGridColumn colClearingFee = band.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                    colClearingFee.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_CLEARINGFEE];
                }
                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_TAXONCOMMISSIONS))
                {
                    UltraGridColumn colTaxOnCommissions = band.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                    colTaxOnCommissions.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                }
                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_MISCFEES))
                {
                    UltraGridColumn colMiscFee = band.Columns[OrderFields.PROPERTY_MISCFEES];
                    colMiscFee.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_MISCFEES];
                }

                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_SECFEE))
                {
                    UltraGridColumn colSecFee = band.Columns[OrderFields.PROPERTY_SECFEE];
                    colSecFee.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_SECFEE];
                }

                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_OCCFEE))
                {
                    UltraGridColumn colOccFee = band.Columns[OrderFields.PROPERTY_OCCFEE];
                    colOccFee.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_OCCFEE];
                }

                if (OrderFields.FeeNamesCollection.ContainsKey(OrderFields.PROPERTY_ORFFEE))
                {
                    UltraGridColumn colOrfFee = band.Columns[OrderFields.PROPERTY_ORFFEE];
                    colOrfFee.Header.Caption = OrderFields.FeeNamesCollection[OrderFields.PROPERTY_ORFFEE];
                }

                UltraGridColumn colOtherBrokerFee = band.Columns[OrderFields.PROPERTY_OTHERBROKERFEES];
                colOtherBrokerFee.Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;

                UltraGridColumn colClearingBrokerFee = band.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;

                UltraGridColumn colSoftCommission = band.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;

                UltraGridColumn colCommissionPerShare = band.Columns[COL_COMMISSIONPERSHARE];
                colCommissionPerShare.Header.Caption = CAP_COMMISSIONPERSHARE;
                colCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colSoftCommissionPerShare = band.Columns[OrderFields.PROPERTY_SOFTCOMMISSIONPERSHARE];
                colSoftCommissionPerShare.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSIONPERSHARE;
                colSoftCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                UltraGridColumn colTotalCommissionPerShare = band.Columns[OrderFields.PROPERTY_TOTALCOMMISSIONPERSHARE];
                colTotalCommissionPerShare.Header.Caption = OrderFields.CAPTION_TOTALCOMMISSIONPERSHARE;
                colTotalCommissionPerShare.CellActivation = Activation.NoEdit;
                colTotalCommissionPerShare.Format = ApplicationConstants.FORMAT_COMMISSIONANDFEES;

                if (band.Columns.Exists(COL_NAVLOCKSTATUS))
                {
                    UltraGridColumn colAccountName = band.Columns[COL_NAVLOCKSTATUS];
                    colAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = CAP_NAVLOCKSTATUS;
                }
                AddChangeType(band);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        /// <summary>
        /// CHMW-2767 [Foreign Positions Settling in Base Currency] Update settlement currency fields on edit trades UI
        /// </summary>
        /// <param name="gridBand"></param>
        private static void AddSettlementCurrencyFields(UltraGridBand gridBand)
        {
            try
            {
                #region settlement currency fields

                UltraGridColumn colSettlementCurrency = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                colSettlementCurrency.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SETTLEMENTCURRENCY);
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    currencies.ValueListItems.Add(item.Key, item.Value);
                }
                //PRANA-7935 Settlement Currency defaulting inappropriately on the Trading Ticket.
                //currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_SELECT);
                colSettlementCurrency.ValueList = currencies;
                colSettlementCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;


                UltraGridColumn colSettlemmentCurrencyFXRate = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRate];
                colSettlemmentCurrencyFXRate.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SettCurrFXRate);

                UltraGridColumn colSettlemmentCurrencyFXRateOperator = gridBand.Columns[OrderFields.PROPERTY_SettCurrFXRateCalc];
                colSettlemmentCurrencyFXRateOperator.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SettCurrFXRateCalc);

                ValueList settlemmentCurrencyFXRateOperatorList = new ValueList();
                List<EnumerationValue> settlemmentCurrencyFXRateOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in settlemmentCurrencyFXRateOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        settlemmentCurrencyFXRateOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                    }
                }
                colSettlemmentCurrencyFXRateOperator.ValueList = settlemmentCurrencyFXRateOperatorList;
                colSettlemmentCurrencyFXRateOperator.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                UltraGridColumn colSettlemmentCurrencyAmount = gridBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT];
                colSettlemmentCurrencyAmount.Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_SETTLEMENTCURRENCYAMOUNT);
                #endregion
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        public void InvokeSecurityMaster()
        {
            try
            {
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                    //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
                    ctrlAmendSingleGroup1._securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(ctrlAmendSingleGroup1._securityMaster_SecMstrDataResponse);
                    //new SecMasterDataHandler(ctrlAmendSingleGroup1._securityMaster_SecMstrDataResponse);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public delegate void SecMasterObjHandler(object sender, EventArgs<SecMasterBaseObj> e);
        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SecMasterObjHandler secObjhandler = new SecMasterObjHandler(UpdateValue);
                        this.Invoke(secObjhandler, new object[] { sender, e });
                    }
                    else
                    {
                        UpdateValue(sender, e);
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
        }

        private void UpdateValue(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj == null)
                {
                    //Logger.Write(@"SecMasterObj is null in UpdateValue function in \Prana_CA\Prana.PM\Prana.PM.Client.UI\Controls\CtrlCreateAndImportPosition.cs class.");
                    return;
                }
                else
                {
                    SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)secMasterObj;
                    AllocationGroup group = null;
                    foreach (AllocationGroup allgroup in AllocationManager.GetInstance().UnAllocatedGroups)
                    {
                        if (allgroup.GroupID.Equals(_groupID))
                        {
                            group = allgroup;
                            break;
                        }
                    }

                    if (group != null)
                    {
                        group.CouponRate = fixedIncomeObj.Coupon;
                        group.MaturityDate = fixedIncomeObj.MaturityDate;
                        group.IssueDate = fixedIncomeObj.IssueDate;
                        group.FirstCouponDate = fixedIncomeObj.FirstCouponDate;
                        group.AccrualBasis = fixedIncomeObj.AccrualBasis;
                        group.BondType = fixedIncomeObj.BondType;
                        group.Freq = fixedIncomeObj.Frequency;
                        group.IsZero = fixedIncomeObj.IsZero;
                        group.ExpirationDate = fixedIncomeObj.MaturityDate;
                        double accrualTemp = group.AccruedInterest;
                        group.AccruedInterest = CalculateAccruedInterest(group);
                        if (group.AccruedInterest != accrualTemp)
                            group.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                        group.UpdateTaxlotAccruedInterest();
                    }
                    _groupID = string.Empty;
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
        private void toolStripStatusLabel1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (toolStripStatusLabel1.Text != String.Empty)
                {
                    StringBuilder build = new StringBuilder();
                    build.Append("[");
                    build.Append(DateTime.Now.ToString());
                    build.Append("]");
                    toolStripStatusLabel1.Text = build.ToString();
                    timerClear.Start();
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
        private void addStringColumns()
        {
            try
            {
                TradeStringFields.Add(COL_LotID);
                TradeStringFields.Add(COL_ExternalTransId);
                TradeStringFields.Add(COL_TradeAttribute1);
                TradeStringFields.Add(COL_TradeAttribute2);
                TradeStringFields.Add(COL_TradeAttribute3);
                TradeStringFields.Add(COL_TradeAttribute4);
                TradeStringFields.Add(COL_TradeAttribute5);
                TradeStringFields.Add(COL_TradeAttribute6);
                TradeStringFields.Add(COL_DESCRIPTION);
                TradeStringFields.Add(COL_INTERNALCOMMENTS);
                TradeStringFields.Add(COL_CHANGECOMMENT);
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


        private void grdAmendmend_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                if (grdAmendmend.ActiveRow != null)
                {
                    if (grdAmendmend.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    ctrlAmendSingleGroup1.ChangeEnableStatus(false);
                    group = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                    if (group != null)
                    {
                        EditGroupDetails(group);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCommission_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                if (grdCommission.ActiveRow != null)
                {
                    if (grdCommission.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    group = grdCommission.ActiveRow.ListObject as AllocationGroup;
                    if (group != null)
                    {
                        // Check for NAV Lock only for CH Release type.
                        if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                        {
                            if (e.Row.Cells[COL_NAVLOCKSTATUS].Value != null && e.Row.Cells[COL_NAVLOCKSTATUS].Value.ToString().Equals("Locked"))
                            {
                                MessageBox.Show("NAV is locked for the following account." + System.Environment.NewLine + "Amendments will be disabled.",
                                    "Edit Trades", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                                ctrlAmendSingleGroup1.ChangeEnableStatus(true);
                                EditGroupDetails(group);
                                ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, true);
                            }
                            else
                            {
                                this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;

                                if (IsReadOnlyPermission)
                                {
                                    ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, true);
                                }
                                else
                                {
                                    //Modified By amit
                                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3495
                                    if (group.GroupStatus == PostTradeEnums.Status.Exercise)
                                    {
                                        MessageBox.Show("This Group is generated by Exercise.", "Warning", MessageBoxButtons.OK);
                                    }
                                    ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, false);
                                    ctrlAmendSingleGroup1.ChangeEnableStatus(true);
                                    EditGroupDetails(group);
                                }
                            }
                        }
                        else
                        {
                            this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed = false;
                            ctrlAmendSingleGroup1.ChangeEnableStatus(true);
                            EditGroupDetails(group);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CtrlAmendmend_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                    WireEvents();
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    splitter1.SplitPosition = 0;
                    ultraExpandableGroupBox1.Expanded = false;
                    ultraExpandableGroupBox1.Visible = false;
                    ultraExpandableGroupBox2.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
                    ultraExpandableGroupBox2.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.None;
                    ultraExpandableGroupBox2.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
                    ultraExpandableGroupBox2.Text = "";
                }
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExit.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExit.ForeColor = System.Drawing.Color.White;
                btnExit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExit.UseAppStyling = false;
                btnExit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Wires events on form load
        /// </summary>
        private void WireEvents()
        {
            try
            {
                timerClear.Tick += new EventHandler(timerClear_Tick);
                ctrlRecalculate1.RecalculateCommission += new EventHandler(ctrlRecalculate1_RecalculateCommission);
                ctrlRecalculate1.BulkChangeOnGroupLevel += new EventHandler(ctrlRecalculate1_BulkChangeOnGroupLevel);
                ctrlRecalculate1.DisplayMessage += new EventHandler(ctrlRecalculate1_DisplayMessage);
                ctlTradeAttributes1.bandGrid(grdAmendmend, grdCommission);
                ctlTradeAttributes1.DisplayMessage += displayMessage;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Unwires events
        /// </summary>
        internal void UnWireEvents()
        {
            try
            {
                timerClear.Tick -= new EventHandler(timerClear_Tick);
                ctrlRecalculate1.RecalculateCommission -= new EventHandler(ctrlRecalculate1_RecalculateCommission);
                ctrlRecalculate1.BulkChangeOnGroupLevel -= new EventHandler(ctrlRecalculate1_BulkChangeOnGroupLevel);
                ctrlRecalculate1.DisplayMessage -= new EventHandler(ctrlRecalculate1_DisplayMessage);
                ctlTradeAttributes1.DisplayMessage -= displayMessage;
                ctrlAmendSingleGroup1.CloseEditSingleGroup -= new EventHandler(ctrlAmendSingleGroup1_CloseEditSingleGroup);
                _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                ctrlAmendSingleGroup1._securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(ctrlAmendSingleGroup1._securityMaster_SecMstrDataResponse);
                grdAmendmend.CellChange -= new CellEventHandler(grdAmendmend_CellChange);
                grdAmendmend.AfterCellUpdate -= new CellEventHandler(grdAmendmend_AfterCellUpdate);
                grdCommission.AfterCellUpdate -= new CellEventHandler(grdCommission_AfterCellUpdate);
                grdCommission.CellChange -= new CellEventHandler(grdCommission_CellChange);
                this.grdAmendmend.BeforeColumnChooserDisplayed -= new BeforeColumnChooserDisplayedEventHandler(grdAmendmend_BeforeColumnChooserDisplayed);
                this.grdCommission.BeforeColumnChooserDisplayed -= new BeforeColumnChooserDisplayedEventHandler(grdCommission_BeforeColumnChooserDisplayed);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void timerClear_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            timerClear.Stop();
        }

        /// <summary>
        /// Clears Databinding and sets default values on ctrlAmendGroup
        /// </summary>

        internal void ClearAmendSingleGroupControl()
        {
            try
            {
                ctrlAmendSingleGroup1.ClearDataBindings();
                ctrlAmendSingleGroup1.SetDefaultValuetoControl();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdAmendmend_ClickCell(object sender, ClickCellEventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                if (grdAmendmend.ActiveRow != null)
                {
                    if (grdAmendmend.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    ctrlAmendSingleGroup1.ChangeEnableStatus(false);
                    group = (AllocationGroup)grdAmendmend.ActiveRow.ListObject;
                    if (group != null && !this.ultraDockManager1.DockAreas[2].DockAreaPane.DockAreaPane.Panes[0].Closed)
                    {
                        EditGroupDetails(group);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// cell Click even of the gridCommission on Edit Trades Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCommission_ClickCell(object sender, ClickCellEventArgs e)
        {
            AllocationGroup group = null;
            try
            {
                //if the cell is editable then dont allow user to edit if the trade account's lock is not withcurrent user
                if (e.Cell.Activation == Activation.AllowEdit)
                {
                    //Account locking if the logged in user is of CH
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType().Equals(PranaReleaseViewType.CHMiddleWare))
                    {
                        if (!e.Cell.Row.Cells.Exists("AccountName"))
                        {
                            grdCommission.ActiveCell = null;
                            return;
                        }
                        string rowAccountname = e.Cell.Row.Cells["AccountName"].Text;
                        int accountID = CachedDataManager.GetInstance.GetAccountID(rowAccountname);
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                        //Account Lock error should not be given if account id is not retrieved
                        if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                        {
                            if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking " + rowAccountname + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                List<int> newAccountsToBelocked = new List<int>();
                                newAccountsToBelocked.Add(accountID);
                                newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                                if (AllocationManager.SetAccountsLockStatus(newAccountsToBelocked))
                                {
                                    MessageBox.Show("The lock for " + rowAccountname + " has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(rowAccountname + " is currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //set active cell to null so that it cannot be modified
                                    grdCommission.ActiveCell = null;
                                    return;
                                }
                            }
                            else
                            {
                                // user clicked no     
                                grdCommission.ActiveCell = null;
                                return;
                            }
                        }
                    }
                }
                if (grdCommission.ActiveRow != null)
                {
                    if (grdCommission.ActiveRow is UltraGridGroupByRow)
                    {
                        return;
                    }
                    group = grdCommission.ActiveRow.ListObject as AllocationGroup;
                    if (group != null)
                    {
                        ctrlAmendSingleGroup1.ChangeEnableStatus(true);
                        EditGroupDetails(group);
                        //CHMW-3277	[Edit Trades] Trades are editable for NAV locked trades in a scenario
                        // Check for NAV Lock only for CH Release type.
                        if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                        {
                            if (grdCommission.ActiveRow.Cells[COL_NAVLOCKSTATUS].Value != null && grdCommission.ActiveRow.Cells[COL_NAVLOCKSTATUS].Value.ToString().Equals("Locked"))
                            {
                                MessageBox.Show("NAV is locked for the following account." + System.Environment.NewLine + "Amendments will be disabled.",
                                    "Edit Trades", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, true);
                            }
                            else
                            {
                                ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, false);
                            }
                        }
                    }
                    if (e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRateCalc)
                    || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SettCurrFXRate) || e.Cell.Column.Key.Equals(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                    {
                        if (e.Cell.Row.ListObject.GetType().Equals(typeof(TaxLot)))
                        {
                            TaxLot taxlot = (TaxLot)e.Cell.Row.ListObject;
                            if (taxlot.ClosingStatus == ClosingStatus.Closed || taxlot.ClosingStatus == ClosingStatus.PartiallyClosed)
                            {
                                MessageBox.Show("Settlement currency fields cannot be changed on partially or fully closed trades, Please unwind before changing the settlement currency fields.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.CancelUpdate();
                            }
                        }
                        else if (e.Cell.Row.ListObject.GetType().Equals(typeof(AllocationGroup)))
                        {
                            AllocationGroup allocationGroup = (AllocationGroup)e.Cell.Row.ListObject;
                            if (allocationGroup.ClosingStatus == ClosingStatus.Closed || allocationGroup.ClosingStatus == ClosingStatus.PartiallyClosed)
                            {
                                MessageBox.Show("Settlement currency fields cannot be changed on partially or fully closed trades, Please unwind before changing the settlement currency fields.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                e.Cell.CancelUpdate();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCommission_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch (Exception)
            {
                //Do Nothing as user can try again
            }
        }

        private void ultraDockManager1_PaneHidden(object sender, Infragistics.Win.UltraWinDock.PaneHiddenEventArgs e)
        {
            ctrlAmendSingleGroup1.ClearDataBindings();
        }

        /// <summary>
        /// return selected groups for grdCommisson
        /// </summary>
        /// <returns></returns>
        public List<AllocationGroup> GetSelectedAllocatedGroups()
        {
            List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
            try
            {
                allocationGroups = GetSelectedGroups(grdCommission);
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
            return allocationGroups;
        }

        /// <summary>
        /// Get selected groups for the respectie ultargrid
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private List<AllocationGroup> GetSelectedGroups(UltraGrid grid)
        {
            List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                //Added By Faisal Shah
                //Purpose to get all the NAV Locked Rows and add only those rows that have NAV Unlocked
                if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                {
                    if (rows.Count() > 0)
                    {
                        StringBuilder accountsWithNAVLock = new StringBuilder();
                        int countOfRowsWithNAVLock = 0;
                        foreach (UltraGridRow row in rows)
                        {
                            if (((bool)(row.Cells["checkBox"].Value)) == true)
                            {
                                if (row.Cells[COL_NAVLOCKSTATUS].Value.ToString().Equals("Locked"))
                                {
                                    if (!accountsWithNAVLock.ToString().Contains(row.Cells[COL_FUNDNAME].Text.ToString()))
                                    {
                                        accountsWithNAVLock.Append(row.Cells[COL_FUNDNAME].Text.ToString() + ", ");
                                    }
                                    countOfRowsWithNAVLock++;
                                }
                                else
                                {
                                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2256
                                    //As discussed with Gaurav here we are adding multiple items from the grid by reference and we are deleting these items simultaneously, so we have to add the items by clone to remove the red cross.
                                    //As we cannot update generic binding list from the backgroud worker thread
                                    AllocationGroup group = row.ListObject as AllocationGroup;
                                    if (group != null)
                                    {
                                        allocationGroups.Add((AllocationGroup)group.Clone());
                                    }
                                }
                            }
                        }
                        if (accountsWithNAVLock.Length > 0 && rows.Count() != countOfRowsWithNAVLock)
                        {
                            if (MessageBox.Show(" Account(s)  " + accountsWithNAVLock.ToString().Substring(0, accountsWithNAVLock.Length - 2) + " has/have NAV locked.\n Do you want to delete the rest of the trades?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                AllocationManager.GetInstance().ClearData();
                                allocationGroups.Clear();
                            }
                        }
                        else if (rows.Count() == countOfRowsWithNAVLock)
                        {
                            MessageBox.Show("You can not delete these trades as all the accounts have NAV locked.\n Please contact admin.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                    foreach (UltraGridRow row in rows)
                    {
                        if (((bool)(row.Cells["checkBox"].Value)) == true)
                        {
                            allocationGroups.Add((AllocationGroup)row.ListObject);
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
            return allocationGroups;
        }

        /// <summary>
        /// Check if all accounts For the Trade to be deleted are  locked by current user or not
        /// </summary>
        /// <returns></returns>
        internal bool checkForlockedAccounts()
        {
            //Added by: sachin mishra 22/jan/2015 adding here try-catch block
            try
            {
                StringBuilder errMsg = new StringBuilder();
                List<int> newAccountsToBelocked = new List<int>();
                foreach (UltraGridRow row in grdCommission.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value) == true)
                    {
                        if (row.Cells.Exists("AccountName"))
                        {
                            string accountName = row.Cells["AccountName"].Value.ToString();
                            //get account ID from Account Name
                            int accountID = CachedDataManager.GetInstance.GetAccountID(accountName);
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1164
                            //Account Lock error should not be given if account id is not retrieved
                            if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue && !errMsg.ToString().Contains(accountName))
                            {
                                //add Account Name in Error message if Account is not locked
                                errMsg.Append(", ").Append(accountName);
                                newAccountsToBelocked.Add(accountID);
                            }
                        }
                    }
                }
                if (errMsg.Length != 0)
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + errMsg.ToString().Substring(1) + ".", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AllocationManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //set active cell to null so that it cannot be modified
                            grdCommission.ActiveCell = null;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
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
            return true;
        }

        /// <summary>
        /// Added by : sachin mishra -for checking is grid items are selected or not 
        /// JIRA No-CHMW-2390  date-20/jan/2015
        /// </summary>
        /// <returns></returns>
        internal bool checkForSelectItem()
        {
            try
            {
                int counter = 0;
                StringBuilder errMsg = new StringBuilder();
                List<int> newAccountsToBelocked = new List<int>();
                foreach (UltraGridRow row in grdCommission.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBox"].Value) == true)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            return true;
                        }
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


            return false;
        }

        /// <summary>
        /// added by: Bharat Raturi, 18 jun 2014
        /// set the access of the grid and the controls to read only for the user with read permission
        /// </summary>
        internal void SetGridAccessToReadOnly()
        {
            try
            {
                foreach (UltraGridBand band in grdAmendmend.DisplayLayout.Bands)
                {
                    band.Override.AllowUpdate = DefaultableBoolean.False;
                }
                foreach (UltraGridBand band in grdCommission.DisplayLayout.Bands)
                {
                    band.Override.AllowUpdate = DefaultableBoolean.False;
                }
                ctrlAmendSingleGroup1.MakeControlsReadOnly(ctrlAmendSingleGroup1, true);
                //unwire the event as user wouldn't need the lock on account in case of read only access
                grdCommission.ClickCell -= grdCommission_ClickCell;
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
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCommission_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (_accountUnlocked.Length > 0)
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + _accountUnlocked.ToString().Substring(0, _accountUnlocked.Length - 1) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<int> newAccountsToBelocked = new List<int>();
                        newAccountsToBelocked.AddRange(_accountIDUnlocked);
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AllocationManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //update Locks in cache
                            CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                        }
                        else
                        {
                            MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                //_isHeaderCheckBoxChecked = false;
                _accountUnlocked = new StringBuilder();
                _accountIDUnlocked = new List<int>();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCommission_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                //_isHeaderCheckBoxChecked = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        protected void bindAttrbLists()
        {
            BindableValueList[] bvl = TradeAttributesCache.getValueList(grdAmendmend);
            bindAttrbListsToGrid(grdAmendmend, bvl);
            bvl = TradeAttributesCache.getValueList(grdCommission);
            bindAttrbListsToGrid(grdCommission, bvl);
        }

        protected void bindAttrbListsToGrid(UltraGrid grid, BindableValueList[] bvl)
        {

            UltraGridBand band = grid.DisplayLayout.Bands[0];
            band.Columns[COL_TradeAttribute1].ValueList = bvl[0];
            band.Columns[COL_TradeAttribute1].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            band.Columns[COL_TradeAttribute2].ValueList = bvl[1];
            band.Columns[COL_TradeAttribute2].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            band.Columns[COL_TradeAttribute3].ValueList = bvl[2];
            band.Columns[COL_TradeAttribute3].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            band.Columns[COL_TradeAttribute4].ValueList = bvl[3];
            band.Columns[COL_TradeAttribute4].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            band.Columns[COL_TradeAttribute5].ValueList = bvl[4];
            band.Columns[COL_TradeAttribute5].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            band.Columns[COL_TradeAttribute6].ValueList = bvl[5];
            band.Columns[COL_TradeAttribute6].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
        }

        protected void updateAttribList(CellEventArgs e)
        {
            string newText = e.Cell.Text;
            if (newText != null && newText.Length > 0)
            {
                BindableValueList bvl = (BindableValueList)e.Cell.Column.ValueList;
                BindingSource bs = (BindingSource)bvl.DataSource;
                DataView dw = (DataView)bs.DataSource;
                if (dw.Find(newText) < 0)
                {
                    DataRowView drw = dw.AddNew();
                    drw.BeginEdit();
                    drw[0] = newText;
                    drw.EndEdit();
                }
            }
        }

        public void updateAttriblists()
        {
            List<string>[] attribLists = AllocationManager.GetInstance().getAttributeList();
            if (attribLists != null)
            {

                for (int i = 1; i <= 6; i++)
                {
                    UltraGridBand band = grdAmendmend.DisplayLayout.Bands[0];
                    BindableValueList bvl = (BindableValueList)band.Columns["TradeAttribute" + i].ValueList;
                    if (bvl != null)
                    {
                        BindingSource bs = (BindingSource)bvl.DataSource;
                        DataView dw = (DataView)bs.DataSource;
                        foreach (string item in attribLists[i - 1])
                        {
                            if (dw.Find(item) < 0)
                            {
                                DataRowView drw = dw.AddNew();
                                drw.BeginEdit();
                                drw[0] = item;
                                drw.EndEdit();
                            }
                        }
                    }
                }
            }
        }

        private void updateAttriblistsForGroup(UltraGrid grid, AllocationGroup group)
        {
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            string attribute = group.TradeAttribute1;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute1], attribute);
            }
            attribute = group.TradeAttribute2;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute2], attribute);
            }
            attribute = group.TradeAttribute3;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute3], attribute);
            }
            attribute = group.TradeAttribute4;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute4], attribute);
            }
            attribute = group.TradeAttribute5;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute5], attribute);
            }
            attribute = group.TradeAttribute6;
            if (attribute != null && attribute.Length > 0)
            {
                updateTradeAttributeList(band.Columns[COL_TradeAttribute6], attribute);
            }
        }

        private void updateTradeAttributeList(UltraGridColumn column, string attribute)
        {
            BindableValueList bvl = (BindableValueList)column.ValueList;
            BindingSource bs = (BindingSource)bvl.DataSource;
            DataView dw = (DataView)bs.DataSource;
            if (dw.Find(attribute) < 0)
            {
                DataRowView drw = dw.AddNew();
                drw.BeginEdit();
                drw[0] = attribute;
                drw.EndEdit();
            }
        }

        private void ultraDockManager1_PaneDisplayed(object sender, Infragistics.Win.UltraWinDock.PaneDisplayedEventArgs e)
        {
            if (e.Pane.Control == ctlTradeAttributes1)
            {
                ctlTradeAttributes1.updateUI();
            }
        }

        private void displayMessage(string msg, bool timely)
        {
            try
            {
                toolStripStatusLabel1.Text = msg;
                if (timely)
                    timerClear.Start();
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
        /// Handles the Click event of the mnuCostAdjustment control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mnuCostAdjustment_Click(object sender, EventArgs e)
        {
            try
            {
                CostAdjustmentForm form = new CostAdjustmentForm();

                List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
                allocationGroups = GetSelectedGroups(grdCommission);
                List<string> uniqueSymbol = (from c in allocationGroups
                                             select c.Symbol).Distinct().ToList();
                string csv = GeneralUtilities.GetStringFromList(uniqueSymbol, ',');
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(form);

                // Done changes to Bind data to new tab which shows saved cost adjustment taxlots
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                List<CostAdjustmentTaxlot> openTaxlots = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetAllOpenTaxlots(Convert.ToDateTime("1/1/1800").Date, DateTime.Now.Date, false, "", "", csv, "").AdjustedTaxlots;
                List<string> openTaxlotIds = new List<string>();
                openTaxlots.ForEach(x => openTaxlotIds.Add(x.TaxlotId));
                List<CostAdjustmentTaxlotForUndo> costAdjustmentSavedTaxlots = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetCostAdjustmentSavedTaxlotsFromId(openTaxlotIds);
                form.BindData(openTaxlots, costAdjustmentSavedTaxlots);

                CustomThemeHelper.SetThemeProperties(form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
                form.ShowSaveButton(true);
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Auto Unwind Code when quantity changes http://jira.nirvanasolutions.com:8080/browse/CHMW-1793
        //private ClosingTemplate CreateTemplate(string columnName, string symbol, DateTime date, int accountID)
        //{
        //    ClosingTemplate template = new ClosingTemplate();
        //    try
        //    {
        //        template.FromDate = date;
        //        template.ToDate = DateTime.Now;
        //        template.ListAccountFliters.Add(accountID);
        //        CustomCondition condition = new CustomCondition();
        //        condition.ColumnName = columnName;
        //        condition.ConditionOperatorType = EnumDescriptionAttribute.ConditionOperator.Equals;
        //        condition.compareValue = symbol;
        //        List<CustomCondition> lstCustomConditions = new List<CustomCondition>();
        //        lstCustomConditions.Add(condition);
        //        template.DictCustomConditions.Add(columnName, lstCustomConditions);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return template;
        //}
        ///// <summary>
        ///// closing of symbols which are unwound for amendments
        ///// </summary>
        //internal void AutoCloseAmendedSymbol()
        //{
        //    try
        //    {
        //        List<ClosingTemplate> lstClosingTemplates = new List<ClosingTemplate>();
        //        if (_dictAmendments.Count > 0)
        //        {
        //            _dictAmendments.Keys.ToList().ForEach(symbolAccount =>
        //                     {
        //                         ClosingTemplate template = CreateTemplate("Symbol", symbolAccount.Split(Seperators.SEPERATOR_6)[0], _dictAmendments[symbolAccount].Value, Convert.ToInt32(symbolAccount.Split(Seperators.SEPERATOR_6)[1]));
        //                         #region Set Closing Algo
        //                         DataSet ds = _closingServices.InnerChannel.GetPreferences().ClosingMethodology.AccountingMethodsTable;
        //                         DataRow[] results = ds.Tables[0].Select("AccountID = " + Convert.ToInt32(symbolAccount.Split(Seperators.SEPERATOR_6)[1]) + " AND AssetName = '" + _dictAmendments[symbolAccount].Key + "'");
        //                         if (results.Length > 0)
        //                         {
        //                             template.ClosingMeth.ClosingAlgo = ((PostTradeEnums.CloseTradeAlogrithm)(int.Parse(results[0]["ClosingAlgo"].ToString())));
        //                         }
        //                         #endregion
        //                         lstClosingTemplates.Add(template);
        //                     });
        //            _closingServices.InnerChannel.AutomaticClosingBasedOnTemplates(lstClosingTemplates);
        //            //clear dictionary after closing
        //            _dictAmendments.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        ///// <summary>
        ///// unwind the trade from the date
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void bgworkerUnwinding_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        List<ClosingTemplate> lstClosingTemplates = (List<ClosingTemplate>)e.Argument;
        //        _closingServices.InnerChannel.UnwindClosingBasedOnTemplates(lstClosingTemplates);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion

        public bool IsReadOnlyPermission { get; set; }

        private void grdAmendmend_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdCommission_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }

    [XmlRoot("CancelAmendPreferences")]
    [Serializable]
    public class CancelAmendPreferences
    {
        [XmlArray("GrdAmendColums"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> GrdAmendColums = new List<ColumnData>();

        [XmlArray("GrdCommissionColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> GrdCommissionColumns = new List<ColumnData>();

        [XmlElement("SplitPanelSize")]
        public string SplitPanelSize = string.Empty;

    }
}


