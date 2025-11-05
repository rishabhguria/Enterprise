using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.PositionManagement;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.Forms;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class ctrlExpiryandSettlementNew : UserControl, ILiveFeedCallback
    {
        #region public variable
        //int _hashCode = 0;
        //int _userID;
        CompanyUser _user = new CompanyUser();
        const string _currencyColumnFormat = "N4";
        const string CAPTION_IsExpired_Settled = "IsExpired_Settled";
        //UltraGridBand _grdBandPositionsTaxlots = null;
        UltraGridBand _grdBandNetPositions = null;
        bool _isCloseAtSpotPrice = false;
        static int _userIDforLayout = int.MinValue;
        static string _expirySettlementNewLayoutFilePath = string.Empty;
        static string _expirySettlementNewLayoutDirectoryPath = string.Empty;

        public delegate void DisableEnableFormHandler(string message, bool Flag, bool TimerFlag);
        public event EventHandler<EventArgs<string, bool, bool>> DisableEnableParentForm;

        string _statusMessage = string.Empty;
        bool _isExpirationError = false;

        List<TradeAuditEntry> _tradeAuditCollection_Closing = new List<TradeAuditEntry>();

        # endregion

        private bool _isNavLockValidationFailed = false;

        ProxyBase<IClosingServices> _closingServices = null;
        BackgroundWorker _bgExpireAndSettle = new BackgroundWorker();
        BackgroundWorker _bgUnwind = new BackgroundWorker();
        ErrorTextBox myerror = null;

        public ProxyBase<IClosingServices> ClosingServices
        {
            set
            {
                _closingServices = value;
                ctrlCreateAndImportPosition1.ClosingServices = value;
            }
        }

        public IPricingService PricingServices
        {
            get
            {
                return _pricingServicesProxy.InnerChannel;
            }
        }

        #region constructor
        public ctrlExpiryandSettlementNew()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    List<string> excludedCheckBoxColumns = UnexpiredExcludedCheckBoxColumns();
                    headerCheckBoxUnExpired = new CheckBoxOnHeader_CreationFilter(excludedCheckBoxColumns);
                    //_hashCode = this.GetHashCode();
                    CreatePricingServiceProxy();
                }
                SetSavedLayoutPath();
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
        #endregion

        void ctrlExpiryandSettlementNew_Load(object sender, System.EventArgs e)
        {
            //Added by Faisal Shah 22/08/14
            //Apply Theme
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);
                SetUp();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExpireSettle.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExpireSettle.ForeColor = System.Drawing.Color.White;
                btnExpireSettle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExpireSettle.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExpireSettle.UseAppStyling = false;
                btnExpireSettle.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAutoExercise.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAutoExercise.ForeColor = System.Drawing.Color.White;
                btnAutoExercise.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAutoExercise.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAutoExercise.UseAppStyling = false;
                btnAutoExercise.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void SetUp()
        {
            try
            {
                ClosingPreferences preferences = _closingServices.InnerChannel.GetPreferences();
                chkCopyOpeningTradeAttributes.Checked = preferences.CopyOpeningTradeAttributes;

                headerCheckBoxUnExpired._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnExpired__CLICKED);
                headerCheckBoxUnWind._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnWind__CLICKED);

                //setup for expiration and settlement thread
                _bgExpireAndSettle = new BackgroundWorker();
                _bgExpireAndSettle.DoWork += new DoWorkEventHandler(_bgExpireAndSettle_DoWork);
                _bgExpireAndSettle.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgExpireAndSettle_RunWorkerCompleted);
                _bgExpireAndSettle.WorkerSupportsCancellation = true;

                //background worker setup for unwind
                _bgUnwind = new BackgroundWorker();
                _bgUnwind.DoWork += new DoWorkEventHandler(_bgUnwind_DoWork);
                _bgUnwind.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgUnwind_RunWorkerCompleted);
                _bgUnwind.WorkerSupportsCancellation = true;
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

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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

        ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set
            {
                try
                {
                    _allocationServices = value;
                    ctrlCreateAndImportPosition1.AllocationServices = value;
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
        }

        # region events
        private void btnExpireSettle_Click(object sender, EventArgs e)
        {
            try
            {
                bool saveAllData = true;
                if (grdCashandExpire.Rows.FilteredInNonGroupByRowCount != grdCashandExpire.Rows.Count && grdCashandExpire.Rows.FilteredInNonGroupByRowCount > 0)
                {
                    if (MessageBox.Show("You have filtered some data but whole data will be saved.\n Do you want to proceed with saving all data?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        saveAllData = true;
                    else
                        saveAllData = false;
                }
                if (saveAllData)
                {
                    object[] arguments = new object[7];
                    arguments[0] = (GenericBindingList<TaxLot>)grdCashandExpire.DataSource;
                    arguments[1] = grdCashandExpire.Rows.Count;
                    arguments[3] = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();
                    if (grdCashandExpire.Rows.Count > 0)
                    {

                    }
                    else if (ctrlCreateAndImportPosition1.Visible)
                    {
                        UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();
                        OTCPositionList otcPositions = ctrlCreateAndImportPosition1.CreateTransactions();
                        List<int> zeroQtyIndexes = new List<int>();
                        if (otcPositions != null)
                        {
                            Dictionary<string, double> expiredTaxlotQtyDict = new Dictionary<string, double>();
                            for (int i = 0; i < otcPositions.Count; i++)
                            {
                                OTCPosition pos = otcPositions[i];
                                if (pos.PositionStartQuantity <= double.Epsilon)
                                    zeroQtyIndexes.Add(i);
                                if (expiredTaxlotQtyDict.ContainsKey(pos.ExpiredTaxlotID))
                                {
                                    expiredTaxlotQtyDict[pos.ExpiredTaxlotID] += pos.PositionStartQuantity;
                                }
                                else
                                    expiredTaxlotQtyDict.Add(pos.ExpiredTaxlotID, pos.PositionStartQuantity);
                            }

                            List<TaxLot> taxlots = new List<TaxLot>();
                            foreach (UltraGridRow row in rows)
                            {
                                TaxLot taxLot = row.ListObject as TaxLot;
                                decimal underlyingQty = taxLot.AssetCategoryValue == AssetCategory.FutureOption ?
                                    (decimal)taxLot.SettledQty : (decimal)(taxLot.SettledQty * taxLot.ContractMultiplier);
                                if (expiredTaxlotQtyDict.ContainsKey(taxLot.TaxLotID) && (decimal)expiredTaxlotQtyDict[taxLot.TaxLotID] != underlyingQty)
                                {
                                    taxlots.Add(taxLot);
                                }
                            }
                            if (taxlots.Count > 0)
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Symbol");
                                dt.Columns.Add("Account Name");
                                taxlots.ForEach(tax => dt.Rows.Add(tax.Symbol, tax.Level1Name));

                                DisplayItemsPopUp.ShowDialog("The following option(s) cannot be exercised because of incorrect quantity generated:", dt, "Incorrect underlying quantity", MessageBoxButtons.OK);
                                return;
                            }
                        }



                        arguments[4] = otcPositions;
                        List<TaxLot> taxLots = new List<TaxLot>();
                        ctrlCreateAndImportPosition1.HideConfirmationMessage = true;
                        DialogResult dlgResult = MessageBox.Show("Do you want to save the transactions?", "Create Transactions Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dlgResult.Equals(DialogResult.No))
                        {
                            return;
                        }
                        foreach (int index in zeroQtyIndexes)
                            otcPositions.RemoveAt(index);
                    }
                    else if (_isSwap) //handling for equity swaps 
                    {
                        arguments[2] = ctrlSwapClosing1.GetSelectedParams(SwapValidate.Expire);
                    }
                    else //return if save button is clicked without chosing an option to expire or settle
                        return;
                    if (!_bgExpireAndSettle.IsBusy && !ctrlCreateAndImportPosition1.IsSavingCanceled)
                    {
                        //wingrid performance improve
                        //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html
                        this.grdAccountExpired.Enabled = false;
                        Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdAccountExpired);
                        this.grdAccountExpired.BeginUpdate();
                        this.grdAccountExpired.SuspendRowSynchronization();

                        this.grdCashandExpire.Enabled = false;
                        Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdCashandExpire);
                        this.grdCashandExpire.BeginUpdate();
                        this.grdCashandExpire.SuspendRowSynchronization();

                        this.grdAccountUnexpired.Enabled = false;
                        Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdAccountUnexpired);
                        this.grdAccountUnexpired.BeginUpdate();
                        this.grdAccountUnexpired.SuspendRowSynchronization();

                        //disable this form
                        DisableEnableForm(false);

                        //disable parent form CloseTrade

                        //condition to check if it has atleast one listner..
                        if (DisableEnableParentForm != null)
                        {
                            _statusMessage = "Expiring Data Please Wait";
                            DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, false, true));
                        }
                        _bgExpireAndSettle.RunWorkerAsync(arguments);
                    }
                    ctrlCreateAndImportPosition1.IsSavingCanceled = false;
                }
                else
                    return;
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

        private void MultipleExpirationOrCashSettle(GenericBindingList<TaxLot> alist)
        {
            try
            {
                List<TaxLot> taxlots = new List<TaxLot>();
                foreach (TaxLot trade in alist)
                {
                    // int hashCode = trade.GetHashCode();
                    if (_actionClick == false)
                    {
                        trade.AUECModifiedDate = trade.ExpirationDate;
                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(trade.AUECModifiedDate))
                        {
                            _isNavLockValidationFailed = true;
                            MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (trade.SettledQty == 0 || trade.SettledQty > trade.TaxLotQty)
                        {
                            trade.SettledQty = trade.TaxLotQty;
                        }
                        taxlots.Add(trade);
                        UpdateAuditCollection(trade.TaxLotID, trade.ExpirationDate);
                    }
                    switch (trade.ClosingMode)
                    {
                        case ClosingMode.Cash:
                        case ClosingMode.CashSettleinBaseCurrency:
                        case ClosingMode.Expire:
                            if (trade.AUECLocalDate.Date > trade.ExpirationDate.Date)
                            {
                                StringBuilder message = new StringBuilder();
                                message.Append("Taxlot ID :");
                                message.Append(trade.TaxLotID);
                                message.Append(" Trade Date cannot be greater than the Expiration Date");
                                message.Append(Environment.NewLine);
                                MessageBox.Show(message.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                taxlots.Remove(trade);
                                _isExpirationError = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                List<AllocationGroup> allocationGroupList = _allocationServices.InnerChannel.CreateandSaveGroupFromTaxlot(taxlots, true);
                List<TaxLot> closedTrades = ClosingClientSideMapper.GetTaxlots(allocationGroupList, taxlots, true);
                ClosingParameters closingParams = new ClosingParameters();
                closingParams.BuyTaxLotsAndPositions = taxlots;
                closingParams.SellTaxLotsAndPositions = closedTrades;
                closingParams.Algorithm = PostTradeEnums.CloseTradeAlogrithm.NONE;
                closingParams.IsShortWithBuyAndBuyToCover = ClosingPrefManager.IsShortWithBuyAndBuyToCover;
                closingParams.IsSellWithBuyToClose = ClosingPrefManager.IsSellWithBuyToClose;
                closingParams.IsManual = false;
                closingParams.IsDragDrop = true;
                closingParams.IsFromServer = false;
                closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                closingParams.IsVirtualClosingPopulate = false;
                closingParams.IsOverrideWithUserClosing = false;
                closingParams.IsMatchStrategy = true;
                closingParams.ClosingField = PostTradeEnums.ClosingField.Default;
                closingParams.IsCopyOpeningTradeAttributes = true;
                ClosingData closingdata = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                if (closingdata.IsNavLockFailed)
                {
                    MessageBox.Show(closingdata.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (TaxLot taxlot in alist)
                    {
                        taxlot.CashSettledPrice = 0;
                        taxlot.SettledQty = 0;
                        taxlot.ClosingMode = ClosingMode.None;
                    }
                    if (!closingdata.ErrorMsg.ToString().Equals(string.Empty))
                    {
                        MessageBox.Show(closingdata.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                _actionClick = true;
                alist.Clear();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dlgResult = MessageBox.Show("All the Unsaved Changes will be Lost, Do you wish to Proceed?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult.Equals(DialogResult.Yes))
                {
                    grpCreatePosition.Expanded = false;
                    ctrlCreateAndImportPosition1.Visible = true;
                    grdCashandExpire.Visible = false;
                    grpCreatePosition.Text = "Create Transaction";
                    _isControlInitialized = false;
                    ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                    _isSwap = false;
                    _actionClick = true;
                    _tradeAuditCollection_Closing.Clear();
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
        /// Handles the Click event of the btnAutoExercise control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnAutoExercise_Click(object sender, EventArgs e)
        {
            try
            {
                _actionClick = false;
                List<TaxLot> TaxLots = new List<TaxLot>();
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();

                ClosingPreferences preferences = _closingServices.InnerChannel.GetPreferences();
                foreach (UltraGridRow row in rows)
                {
                    TaxLot taxlot = row.ListObject as TaxLot;
                    if (taxlot.IntrinsicValue >= preferences.AutoOptExerciseValue && taxlot.DaysToExpiry <= 0)
                    {
                        row.Cells["checkBox"].Value = true;
                        row.Appearance.BackColor.GetBrightness();
                        taxlot.IsExerciseAtZero = false;
                        taxlot.ClosingMode = ClosingMode.Exercise;
                        TaxLots.Add(taxlot);
                    }
                }
                AddOptionExpirationorSettlementAuditEntry(TaxLots, TradeAuditActionType.ActionType.Exercise_Assignment, "Exercise/Assignment for Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                if (TaxLots.Count == 0)
                {
                    MessageBox.Show("No taxlots for Auto Exercise/Assignment", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcerciseandPhysical(TaxLots);
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

        private void menuUnexpired_Popup(object sender, EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = false;
                MenuUnExpiredPopup();
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
        /// show menu according to asset class
        /// </summary>
        private void MenuUnExpiredPopup()
        {
            try
            {
                menuExpire.Enabled = false;
                menuExercise.Enabled = false;
                menuExerciseAssign.Enabled = false;
                menuCashSettlementAtZeroPrice.Enabled = false;
                menuCashSettlementAtClosingDateSpotPx.Enabled = false;
                menuCashSettlementAtCost.Enabled = false;
                menuPhysicalSettlement.Enabled = false;
                menuSwapExpire.Enabled = false;
                menuSwapRolloverandExpire.Enabled = false;
                //menuDeliverFXAtCost.Visible = false;
                //menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Visible = false;
                // bool swapAndOtherRecordSelected = false;

                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();
                List<TaxLot> TaxLotList = GetSelectedTaxlots(rows);

                if (TaxLotList.Count == 0)
                {
                    return;
                }

                List<AssetCategory> assetCatagoryList = new List<AssetCategory>();
                int validationCode = ValidateTaxlotData(TaxLotList, assetCatagoryList);

                if (validationCode != 0)
                {
                    if (assetCatagoryList.Count == 1)
                    {
                        AssetCategory baseAssetCategory = assetCatagoryList[0];
                        switch (baseAssetCategory)
                        {
                            case AssetCategory.Option:
                                menuExpire.Enabled = true;
                                menuExercise.Enabled = true;
                                menuExerciseAssign.Enabled = true;
                                menuPhysicalSettlement.Enabled = true;
                                break;

                            case AssetCategory.Future:
                                if (TaxLotList[0].AssetCategoryValue != AssetCategory.FXForward)
                                {
                                    menuCashSettlementAtZeroPrice.Enabled = true;
                                    menuCashSettlementAtCost.Enabled = true;
                                }
                                menuExpire.Enabled = true;
                                menuCashSettlementAtClosingDateSpotPx.Enabled = true;
                                break;

                            case AssetCategory.FX:
                            case AssetCategory.FXForward:
                                menuCashSettlementAtClosingDateSpotPx.Enabled = true;
                                break;

                            case AssetCategory.Equity:
                                menuSwapExpire.Enabled = true;
                                menuSwapRolloverandExpire.Enabled = true;
                                break;

                            default:
                                break;
                        }
                        //if (baseAssetCategory.Equals(AssetCategory.FX) || baseAssetCategory.Equals(AssetCategory.FXForward))
                        //{
                        //    menuDeliverFXAtCost.Visible = true;
                        //    menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Visible = true;
                        //}
                        //else
                        //{
                        //    menuDeliverFXAtCost.Visible = false;
                        //    menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Visible = false;
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Please select taxlots of same asset class.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private int ValidateTaxlotData(List<TaxLot> TaxLotList, List<AssetCategory> assetCatagoryList)
        {

            DateTime CurrentDate = DateTime.Now.Date;
            try
            {
                foreach (TaxLot taxlot in TaxLotList)
                {
                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(taxlot.AssetCategoryValue);
                    if (!assetCatagoryList.Contains(baseAssetCategory))
                        assetCatagoryList.Add(baseAssetCategory);

                    if (taxlot.SettledQty <= 0 || taxlot.SettledQty > taxlot.TaxLotQty)
                    {
                        MessageBox.Show("Settlement Quantity cannot be zero or greater than traded quantity. Please enter a valid value", "Expiration/Settlement Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return 0;
                    }

                    string eligible = _closingServices.InnerChannel.ArePositionElligibleToExpire(taxlot.TaxLotID, taxlot.AUECID, ClosingPrefManager.IsCurrentDateClosing, CurrentDate);
                    if (!string.IsNullOrEmpty(eligible))
                    {
                        InformationMessageBox.Display(eligible.ToString());
                        return 0;
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
            return 1;
        }

        private void menuExpired_Popup(object sender, EventArgs e)
        {
            try
            {
                if (grdAccountExpired.ActiveRow == null)
                {
                    return;
                }
                UltraGridRow row = grdAccountExpired.ActiveRow;

                MenuExpiredPopup(row);
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

        private void MenuExpiredPopup(UltraGridRow row)
        {
            try
            {
                if (row != null)
                {
                    Position selectedPos = (Position)row.ListObject;
                    if (selectedPos == null)
                    {
                        return;
                    }
                    if (selectedPos.ClosingMode == ClosingMode.CorporateAction)
                    {
                        menuUnwind.Enabled = false;
                    }
                    else
                    {
                        menuUnwind.Enabled = true;
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

        bool _actionClick = true;
        private void menuCashSettlementAtZeroPrice_Click(object sender, EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = false;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Cash, PostTradeEnums.CashSettleType.ZeroPrice);
                AddCashSettlementAuditEntry(TaxLotList, TradeAuditActionType.ActionType.CashSettlementAtZeroPrice, "Cash Settlement At Zero Price", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
                UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType.ZeroPrice);
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
        /// Fill Details For Cash Settle
        /// </summary>
        /// <param name="CashSettleType"></param>
        /// <param name="TaxLotList"></param>
        private void FillDetailsForCashSettle(PostTradeEnums.CashSettleType CashSettleType, GenericBindingList<TaxLot> TaxLotList)
        {
            Dictionary<string, DateTime> dictSymbolWithSettlementDate = new Dictionary<string, DateTime>();
            try
            {
                DataTable markPrices = new DataTable();
                if (CashSettleType.Equals(PostTradeEnums.CashSettleType.ClosingDateSpotPx) && !TaxLotList[0].AssetCategoryValue.Equals(AssetCategory.Future))
                {
                    markPrices = GetMarkPricesForTaxlotList(TaxLotList);
                }
                foreach (TaxLot taxlotToSettle in TaxLotList)
                {
                    if (taxlotToSettle.ClosingMode.Equals(ClosingMode.CashSettleinBaseCurrency) || taxlotToSettle.ClosingMode.Equals(ClosingMode.Cash))
                    {
                        _actionClick = false;

                        //setting the closing mode to cash...
                        taxlotToSettle.ClosingMode = ClosingMode.Cash;
                        if (taxlotToSettle.AssetCategoryValue.Equals(AssetCategory.FXForward) || taxlotToSettle.AssetCategoryValue.Equals(AssetCategory.FX) && taxlotToSettle.IsNDF)
                        {
                            taxlotToSettle.ClosingMode = ClosingMode.CashSettleinBaseCurrency;
                        }
                        DateTime dateCashSettle = DateTime.UtcNow;

                        if (!taxlotToSettle.ExpirationDate.Date.Equals(DateTimeConstants.MinValue.Date))
                        {
                            dateCashSettle = taxlotToSettle.ExpirationDate;
                        }
                        else
                        {

                            taxlotToSettle.ExpirationDate = dateCashSettle;
                        }

                        taxlotToSettle.AUECModifiedDate = dateCashSettle;

                        switch (CashSettleType)
                        {
                            case PostTradeEnums.CashSettleType.Cost:
                                taxlotToSettle.CashSettledPrice = taxlotToSettle.AvgPrice;
                                //Set TransactionType
                                taxlotToSettle.TransactionType = TradingTransactionType.CSCost.ToString();

                                taxlotToSettle.TransactionSource = TransactionSource.Closing;
                                taxlotToSettle.TransactionSourceTag = (int)TransactionSource.Closing;
                                break;
                            case PostTradeEnums.CashSettleType.DeliverFXAtCost:
                                taxlotToSettle.CashSettledPrice = taxlotToSettle.AvgPrice;
                                //Set TransactionType
                                taxlotToSettle.TransactionType = TradingTransactionType.DLCost.ToString();

                                taxlotToSettle.TransactionSource = TransactionSource.Closing;
                                taxlotToSettle.TransactionSourceTag = (int)TransactionSource.Closing;
                                break;
                            case PostTradeEnums.CashSettleType.ZeroPrice:
                                taxlotToSettle.CashSettledPrice = 0;
                                //Set TransactionType
                                taxlotToSettle.TransactionType = TradingTransactionType.CSZero.ToString();

                                taxlotToSettle.TransactionSource = TransactionSource.Closing;
                                taxlotToSettle.TransactionSourceTag = (int)TransactionSource.Closing;

                                break;
                            case PostTradeEnums.CashSettleType.DeliverFXAtCostandPNLAtClosingDateSpotPx:
                                //CHMW-3132	Account wise fx rate handling for expiration settlement                        		
                                int accountCurrencyID;
                                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(taxlotToSettle.Level1ID))
                                {
                                    accountCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[taxlotToSettle.Level1ID];
                                }
                                else
                                {
                                    accountCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();//ClientsCommonDataManager.GetCompanyBaseCurrency(_companyID);
                                }
                                double closingDateSpotRate = ForexConverter.GetInstance(accountCurrencyID).GetFxRateFromCurrenciesForGivenDateOnly(taxlotToSettle.LeadCurrencyID, taxlotToSettle.VsCurrencyID, dateCashSettle, taxlotToSettle.Level1ID);
                                taxlotToSettle.CashSettledPrice = closingDateSpotRate;
                                taxlotToSettle.TransactionType = TradingTransactionType.DLCostAndPNL.ToString();

                                taxlotToSettle.TransactionSource = TransactionSource.Closing;
                                taxlotToSettle.TransactionSourceTag = (int)TransactionSource.Closing;
                                break;
                            case PostTradeEnums.CashSettleType.ClosingDateSpotPx:
                                //add future symbols to the dictionary dictSymbolWithSettlementDate
                                if (taxlotToSettle.AssetCategoryValue.Equals(AssetCategory.Future) && !dictSymbolWithSettlementDate.ContainsKey(taxlotToSettle.Symbol))
                                {
                                    dictSymbolWithSettlementDate.Add(taxlotToSettle.Symbol, dateCashSettle);
                                    taxlotToSettle.CashSettledPrice = 0;
                                }
                                else
                                {
                                    //Mukul: This is for Forwards settlement where the cash settled price is the EOD CashSettleDate/ClosingDate Spot Rate...
                                    //CHMW-3132	Account wise fx rate handling for expiration settlement                        			
                                    int accountBaseCurrencyID;
                                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(taxlotToSettle.Level1ID))
                                    {
                                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[taxlotToSettle.Level1ID];
                                    }
                                    else
                                    {
                                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();//ClientsCommonDataManager.GetCompanyBaseCurrency(_companyID);
                                    }
                                    double closingDateSpotRate1 = 0;
                                    if (markPrices != null && markPrices.Rows.Count > 0)
                                    {
                                        DataRow mpRow = markPrices.AsEnumerable().FirstOrDefault(r => r.Field<string>("Symbol") == taxlotToSettle.Symbol && r.Field<DateTime>("Date") == dateCashSettle.Date.AddDays(-1));
                                        if (mpRow != null)
                                            closingDateSpotRate1 = Convert.ToDouble(mpRow["FinalMarkPrice"]);
                                    }
                                    taxlotToSettle.CashSettledPrice = closingDateSpotRate1;

                                    //PRANA-9035 As discuss with Narendra, I have applied these checks here. In Cross Currency trade, Fx Rate will be same as Trade FxRate, For other it will come from daily Valuation. 
                                    if (taxlotToSettle.AssetCategoryValue.Equals(AssetCategory.Future) || (taxlotToSettle.LeadCurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()) || taxlotToSettle.VsCurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())))
                                        taxlotToSettle.FXRate = ForexConverter.GetInstance(accountBaseCurrencyID).GetFxRateFromCurrenciesForGivenDateOnly(taxlotToSettle.LeadCurrencyID, taxlotToSettle.VsCurrencyID, dateCashSettle.AddDays(-1), taxlotToSettle.Level1ID);
                                }
                                //Set TransactionType
                                taxlotToSettle.TransactionType = TradingTransactionType.CSClosingPx.ToString();

                                taxlotToSettle.TransactionSource = TransactionSource.Closing;
                                taxlotToSettle.TransactionSourceTag = (int)TransactionSource.Closing;
                                break;
                            default:
                                break;
                        }
                    }
                    //Narendra Kumar jangir,Sept 13 2013
                    //IsCurrencySettle is used for fx forwards to identify that trade is settled by exchanging currencies.
                    //if (CashSettleType.Equals(PostTradeEnums.CashSettleType.DeliverFXAtCostandPNLAtClosingDateSpotPx) || CashSettleType.Equals(PostTradeEnums.CashSettleType.DeliverFXAtCost))
                    //{
                    //    taxlotToSettle.IsCurrencySettle = true;
                    //}
                }
                if (CashSettleType.Equals(PostTradeEnums.CashSettleType.ClosingDateSpotPx) || CashSettleType.Equals(PostTradeEnums.CashSettleType.DeliverFXAtCostandPNLAtClosingDateSpotPx))
                {
                    //dictionary which retrieves symbols with their corresponding markprice from updated cache _latestMarkPrices of Pricing.
                    //_latestMarkPrices contains sumbols with their markprice for actual date.
                    Dictionary<string, double> dictSymbolWithMarkPrice = null;
                    //There was unnecessary call to pricing if dictSymbolWithSettlementDate.Count is zero
                    if (dictSymbolWithSettlementDate != null && dictSymbolWithSettlementDate.Count > 0)
                        dictSymbolWithMarkPrice = _pricingServicesProxy.InnerChannel.GetMarkPricesForSymbolAndExactDate(dictSymbolWithSettlementDate);
                    if (dictSymbolWithMarkPrice != null && dictSymbolWithMarkPrice.Count > 0)
                    {
                        foreach (TaxLot taxlot in TaxLotList)
                        {
                            if (taxlot.AssetCategoryValue.Equals(AssetCategory.Future) && dictSymbolWithMarkPrice.ContainsKey(taxlot.Symbol))
                            {
                                taxlot.CashSettledPrice = dictSymbolWithMarkPrice[taxlot.Symbol];
                            }
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
        /// Gets the mark prices for taxlot list.
        /// </summary>
        /// <param name="TaxLotList">The tax lot list.</param>
        /// <returns></returns>
        private DataTable GetMarkPricesForTaxlotList(GenericBindingList<TaxLot> TaxLotList)
        {
            DataTable dt = new DataTable();
            try
            {
                DataTable dtMarkPrices = new DataTable();
                dtMarkPrices.TableName = "SymbolDate";
                dtMarkPrices.Columns.Add(new DataColumn("Symbol"));
                dtMarkPrices.Columns.Add(new DataColumn("Date"));
                dtMarkPrices.Columns.Add(new DataColumn("AccountId"));
                foreach (TaxLot taxlotToSettle in TaxLotList)
                {
                    DateTime dateCashSettle = DateTime.UtcNow;
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-36116
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-36759
                    if (taxlotToSettle.AssetCategoryValue.Equals(AssetCategory.FX))
                    {
                        taxlotToSettle.ExpirationDate = taxlotToSettle.SettlementDate;
                    }
                    if (!taxlotToSettle.ExpirationDate.Date.Equals(DateTimeConstants.MinValue.Date) && dateCashSettle > taxlotToSettle.ExpirationDate)
                    {
                        dateCashSettle = taxlotToSettle.ExpirationDate;
                    }

                    dtMarkPrices.Rows.Add(taxlotToSettle.Symbol, dateCashSettle.AddDays(-1), taxlotToSettle.GetAccountID());
                }
                dt = WindsorContainerManager.GetMarkPricesForSymbolsAndDates(dtMarkPrices);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    string symbol = dr[0].ToString();
                //    DateTime date = Convert.ToDateTime(dr[1].ToString());
                //    double markPrice = Convert.ToDouble(dr[2].ToString());
                //    if (!markPrices.ContainsKey(symbol))
                //        markPrices[symbol] = new Dictionary<DateTime, double>();
                //    markPrices[symbol][date] = markPrice;
                //}
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
            return dt;
        }

        private void UpdateUnExpiredgridForCashSettle()
        {
            try
            {
                UltraGridColumn colCashSettledPrice = grdAccountUnexpired.DisplayLayout.Bands[0].Columns[CAPTION_CashSettledPrice];
                colCashSettledPrice.Hidden = false;
                colCashSettledPrice.CellActivation = Activation.AllowEdit;
                colCashSettledPrice.Header.VisiblePosition = 6;
                UltraGridColumn colFxrate = grdAccountUnexpired.DisplayLayout.Bands[0].Columns["FXRate"];
                colFxrate.Hidden = false;
                colFxrate.CellActivation = Activation.AllowEdit;
                colFxrate.Header.VisiblePosition = 7;
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

        private void menuCashSettlementAtCost_Click(object sender, EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = false;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Cash, PostTradeEnums.CashSettleType.Cost);
                AddCashSettlementAuditEntry(TaxLotList, TradeAuditActionType.ActionType.CashSettlementAtCost, "Cash Settlement At Cost", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
                UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType.Cost);
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

        private void menuCashSettlementAtClosingDateSpotPx_Click(object sender, EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = true;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Cash, PostTradeEnums.CashSettleType.ClosingDateSpotPx);
                AddCashSettlementAuditEntry(TaxLotList, TradeAuditActionType.ActionType.CashSettlementAtClosingDateSpotPx, "Cash Settlement At Closing Date Spot Px", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
                UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType.ClosingDateSpotPx);
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

        void menuDeliverFXAtCost_Click(object sender, System.EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = false;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Cash, PostTradeEnums.CashSettleType.DeliverFXAtCost);
                AddCashSettlementAuditEntry(TaxLotList, TradeAuditActionType.ActionType.DeliverFXAtCost, "Deliver FX at Cost", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
                UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType.DeliverFXAtCost);
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

        void menuDeliverFXAtCostandPNLAtClosingDateSpotPx_Click(object sender, System.EventArgs e)
        {
            try
            {
                _isCloseAtSpotPrice = true;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Cash, PostTradeEnums.CashSettleType.DeliverFXAtCostandPNLAtClosingDateSpotPx);
                AddCashSettlementAuditEntry(TaxLotList, TradeAuditActionType.ActionType.DeliverFXAtCostandPNLAtClosingDateSpotPx, "Deliver FX At Cost and PNL at Closing Date Spot Px", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
                UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType.DeliverFXAtCostandPNLAtClosingDateSpotPx);
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

        private void UpdateCashAndExpireGridForCashSettle(PostTradeEnums.CashSettleType cashSettleType)
        {
            try
            {
                UltraGridBand grdBand = grdCashandExpire.DisplayLayout.Bands[0];
                UltraGridColumn columnCashSettleType = null;
                //UltraGridColumn columnTransactionType = null;
                if (grdBand.Columns.IndexOf("CashSettleType") == -1)
                {
                    columnCashSettleType = grdBand.Columns.Add("CashSettleType");
                    columnCashSettleType.Header.Caption = "CashSettle Type";
                }
                else
                {
                    columnCashSettleType = grdBand.Columns["CashSettleType"];
                }


                foreach (UltraGridRow row in grdCashandExpire.Rows)
                {
                    row.Cells[columnCashSettleType].Value = cashSettleType;
                }

                //grdCashandExpire.DataBind();
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

        private void PopulateCashandExpireGrid(GenericBindingList<TaxLot> TaxLotList)
        {
            try
            {
                grpCreatePosition.Expanded = true;
                grpCreatePosition.Text = "Settlement Price For Expiration And Cash Settlement";
                ctrlCreateAndImportPosition1.Visible = false;
                grdCashandExpire.DisplayLayout.Appearance.BackColor = Color.Black;
                grdCashandExpire.Visible = true;
                grdCashandExpire.DataSource = TaxLotList;
                grpCreatePosition.Height = 120;
                UltraGridBand grdBand = grdCashandExpire.DisplayLayout.Bands[0];
                foreach (UltraGridColumn dc in grdBand.Columns)
                {
                    dc.CellActivation = Activation.NoEdit;
                    dc.Hidden = true;

                }

                if (grdBand.Columns.Exists(OrderFields.PROPERTY_SETTLEMENTCURRENCYID))
                {
                    ValueList vlCurrencies = new ValueList();
                    Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                    foreach (KeyValuePair<int, string> item in dictCurrencies)
                    {
                        vlCurrencies.ValueListItems.Add(item.Key, item.Value);
                    }
                    grdBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                    grdBand.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = vlCurrencies;
                }
                grdBand.Columns[CAPTION_TradeDate].Hidden = false;
                grdBand.Columns[CAPTION_TradeDate].Header.Caption = CAP_TradeDate;
                grdBand.Columns[CAPTION_Symbol].Hidden = false;
                grdBand.Columns[CAPTION_Side].Hidden = false;
                grdBand.Columns[CAPTION_Side].Header.Caption = CAP_Side;
                grdBand.Columns[CAPTION_OpenQuantity].Hidden = false;
                grdBand.Columns[CAPTION_OpenQuantity].Header.Caption = "Open Qty";
                grdBand.Columns[CAPTION_AveragePrice].Hidden = false;
                grdBand.Columns[CAPTION_AveragePrice].Header.Caption = CAP_AvgPrice;
                grdBand.Columns[CAPTION_ExpiryDate].Hidden = false;
                grdBand.Columns[CAPTION_ExpiryDate].CellActivation = Activation.AllowEdit;
                grdBand.Columns[CAPTION_ExpiryDate].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdBand.Columns[CAPTION_ExpiryDate].CellAppearance.ForeColor = Color.Black;
                grdBand.Columns[CAPTION_ExpiryDate].Header.Caption = CAP_CloseDt;
                //Mask mode is set to IncludeLiterals so that date can be copied in multiple cells
                grdBand.Columns[CAPTION_ExpiryDate].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;

                grdBand.Columns[CAPTION_SettledQty].Hidden = false;
                grdBand.Columns[CAPTION_SettledQty].CellActivation = Activation.AllowEdit;
                grdBand.Columns[CAPTION_SettledQty].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdBand.Columns[CAPTION_SettledQty].CellAppearance.ForeColor = Color.Black;
                grdBand.Columns[CAPTION_CashSettledPrice].Hidden = false;
                grdBand.Columns[CAPTION_CashSettledPrice].CellActivation = Activation.AllowEdit;
                grdBand.Columns[CAPTION_CashSettledPrice].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdBand.Columns[CAPTION_CashSettledPrice].CellAppearance.ForeColor = Color.Black;
                grdBand.Columns["FXRate"].Hidden = false;
                grdBand.Columns["FXRate"].CellActivation = Activation.AllowEdit;
                //Narendra Kumar jangir, Aug 27 2013
                //Set value list for Transactiontype column
                grdBand.Columns[COLUMN_TransactionType].ValueList = ValueListHelper.GetInstance.GetTransactionTypeValueList().Clone();
                grdBand.Columns[COLUMN_TransactionType].CellActivation = Activation.NoEdit;

                grdBand.Columns[COLUMN_PutOrCalls].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdBand.Columns[COLUMN_PutOrCalls].Hidden = true;

                grdBand.Columns[CAPTION_OpenQuantity].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_AveragePrice].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_SettledQty].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_UnrealizedPNL].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_QUANTITY].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_ExecutedQuantity].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_UnitCost].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_OpenCommission].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_NetNotionalValue].Format = _currencyColumnFormat;
                grdBand.Columns[COLUMN_StrikePrice].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_UnitCostBase].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_AvgPriceBase].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_CumQty].Format = _currencyColumnFormat;
                grdBand.Columns[CAPTION_FXRate].Format = _currencyColumnFormat;

                if (grdBand.Columns.Exists(ClosingConstants.COL_AssetCategoryValue))
                {
                    UltraGridColumn colAssetCategoryValue = grdBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                    colAssetCategoryValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                if (grdBand.Columns.Exists(ClosingConstants.COL_AssetName))
                {
                    UltraGridColumn colAssetName = grdBand.Columns[ClosingConstants.COL_AssetName];
                    colAssetName.Header.Caption = ClosingConstants.CAP_AssetCategory;
                }

                if (grdBand.Columns.Exists(CAPTION_PutOrCall))
                {
                    UltraGridColumn colPutOrCall = grdBand.Columns[CAPTION_PutOrCall];
                    List<EnumerationValue> PutOrCallType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(OptionType));
                    ValueList PutOrCallValueList = new ValueList();
                    foreach (EnumerationValue value in PutOrCallType)
                    {
                        PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText);
                    }
                    colPutOrCall.ValueList = PutOrCallValueList;
                    colPutOrCall.Header.Caption = CAPTION_PutOrCall;
                    colPutOrCall.CellActivation = Activation.NoEdit;
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

        private void menuPhysicalSettlement_Click(object sender, EventArgs e)
        {
            try
            {
                _actionClick = false;
                List<TaxLot> taxLots = new List<TaxLot>();
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();

                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        row.Appearance.BackColor.GetBrightness();
                        TaxLot taxLot = row.ListObject as TaxLot;
                        taxLot.ClosingMode = ClosingMode.Physical;
                        taxLot.IsExerciseAtZero = false;
                        taxLots.Add(taxLot);
                    }
                }
                AddOptionExpirationorSettlementAuditEntry(taxLots, TradeAuditActionType.ActionType.PhysicalSettlement, "Physical Settlement for Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                if (taxLots.Count == 0)
                {
                    MessageBox.Show("Please select taxlots for settlement", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcerciseandPhysical(taxLots);
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

        bool _isControlInitialized = false;
        private void ExcerciseandPhysical(List<TaxLot> TaxLots, bool settQtyUpdate = false)
        {
            try
            {
                if (settQtyUpdate == _isControlInitialized)
                {
                    grpCreatePosition.Expanded = true;
                    grdCashandExpire.Visible = false;
                    ctrlCreateAndImportPosition1.Visible = true;
                    ctrlCreateAndImportPosition1.IsClosingTaxlots = true;
                    grpCreatePosition.Height = 120;
                    if (!ctrlCreateAndImportPosition1.ExerciseAssignTaxlots(TaxLots, chkCopyOpeningTradeAttributes.Checked))
                        grpCreatePosition.Expanded = false;
                    else
                        _isControlInitialized = true;
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

        private void menuExercise_Click(object sender, EventArgs e)
        {
            try
            {
                _actionClick = false;
                List<TaxLot> TaxLots = new List<TaxLot>();
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();

                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        row.Appearance.BackColor.GetBrightness();
                        TaxLot TaxLot = row.ListObject as TaxLot;
                        TaxLot.IsExerciseAtZero = false;
                        TaxLot.ClosingMode = ClosingMode.Exercise;
                        TaxLots.Add(TaxLot);
                    }
                }
                AddOptionExpirationorSettlementAuditEntry(TaxLots, TradeAuditActionType.ActionType.Exercise_Assignment, "Exercise/Assignment for Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                if (TaxLots.Count == 0)
                {
                    MessageBox.Show("Please select taxlots for settlement", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcerciseandPhysical(TaxLots);
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
        void menuExerciseAssign_Click(object sender, System.EventArgs e)
        {
            try
            {
                _actionClick = false;
                List<TaxLot> TaxLots = new List<TaxLot>();
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();

                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        row.Appearance.BackColor.GetBrightness();
                        TaxLot TaxLot = row.ListObject as TaxLot;
                        TaxLots.Add(TaxLot);
                        TaxLot.ClosingMode = ClosingMode.Exercise;
                        TaxLot.IsExerciseAtZero = true;
                    }
                }
                AddOptionExpirationorSettlementAuditEntry(TaxLots, TradeAuditActionType.ActionType.Exercise_AssignmentatZero, "Exercise/Assignment At Zero for Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                if (TaxLots.Count == 0)
                {
                    MessageBox.Show("Please select taxlots for settlement", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExcerciseandPhysical(TaxLots);
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

        bool _isSwap = false;
        private void menuSwapExpire_Click(object sender, EventArgs e)
        {
            try
            {
                _isSwap = true;
                _actionClick = false;
                TaxLot TaxLot = null;
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        TaxLot = row.ListObject as TaxLot;
                        break;
                    }
                }
                if (TaxLot == null)
                {
                    return;
                }
                TaxLot.ClosingMode = ClosingMode.SwapExpire;
                TaxLot.TransactionType = TradingTransactionType.CSSwp.ToString();
                TaxLot.TransactionSource = TransactionSource.Closing;
                TaxLot.TransactionSourceTag = (int)TransactionSource.Closing;
                if (TaxLot.SettledQty == 0)
                {
                    TaxLot.SettledQty = TaxLot.TaxLotQty;
                }
                if (TaxLot != null)
                {
                    SwapExpireAndRollover(TaxLot, "Swap Expire", SwapValidate.Expire);
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

        private void menuSwapRolloverandExpire_Click(object sender, EventArgs e)
        {
            try
            {
                _isSwap = true;
                _actionClick = false;
                TaxLot TaxLot = null;
                UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        TaxLot = row.ListObject as TaxLot;
                        break;
                    }
                }
                if (TaxLot == null)
                {
                    return;
                }
                TaxLot.ClosingMode = ClosingMode.SwapExpireAndRollover;
                TaxLot.TransactionType = TradingTransactionType.CSSwpRl.ToString();

                TaxLot.TransactionSource = TransactionSource.Closing;
                TaxLot.TransactionSourceTag = (int)TransactionSource.Closing;

                TaxLot.SettledQty = TaxLot.TaxLotQty;
                if (TaxLot != null)
                {
                    SwapExpireAndRollover(TaxLot, "Swap Expire and Rollover", SwapValidate.ExpireAndRollover);
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

        private void SwapExpireAndRollover(TaxLot TaxLot, string grpText, SwapValidate swapValidate)
        {
            try
            {
                grpCreatePosition.Text = grpText;
                grpCreatePosition.Expanded = true;
                grdCashandExpire.Visible = false;
                ctrlCreateAndImportPosition1.Visible = false;
                ctrlSwapClosing1.Visible = true;
                grpCreatePosition.Height = 140;
                if (TaxLot.SwapParameters != null)
                {
                    ctrlSwapClosing1.Set(TaxLot.SwapParameters, swapValidate);
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

        private void menuExpire_Click(object sender, System.EventArgs e)
        {
            try
            {
                _actionClick = false;
                GenericBindingList<TaxLot> TaxLotList = GetSelectedTaxLotsAdjustedForExpireCashSettle(grdAccountUnexpired, ClosingMode.Expire, PostTradeEnums.CashSettleType.ZeroPrice);
                List<TaxLot> taxlotList = TaxLotList.ToList();
                AddOptionExpirationorSettlementAuditEntry(taxlotList, TradeAuditActionType.ActionType.Expire, "Expiration for Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                PopulateCashandExpireGrid(TaxLotList);
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

        private void menuUnwind_Click(object sender, EventArgs e)
        {
            try
            {
                grdAccountExpired.UpdateData();
                UltraGridRow[] filteredRows = grdAccountExpired.Rows.GetFilteredInNonGroupByRows();
                object[] arguments = new object[1];
                arguments[0] = filteredRows;
                if (!_bgUnwind.IsBusy)
                {
                    //wingrid performance improve
                    //http://help.infragistics.com/Help/NetAdvantage/WinForms/2012.1/CLR2.0/html/WinGrid_Formatting_and_Appearance_based_Performance_Improvement.html
                    this.grdAccountExpired.Enabled = false;
                    Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdAccountExpired);
                    this.grdAccountExpired.BeginUpdate();
                    this.grdAccountExpired.SuspendRowSynchronization();

                    this.grdAccountUnexpired.Enabled = false;
                    Prana.ClientCommon.SafeNativeMethods.ControlDrawing.SuspendDrawing(grdAccountUnexpired);
                    this.grdAccountUnexpired.BeginUpdate();
                    this.grdAccountUnexpired.SuspendRowSynchronization();

                    //disable this form
                    DisableEnableForm(false);

                    //condition to check if it has atleast one listner..
                    if (DisableEnableParentForm != null)
                    {
                        _statusMessage = string.Empty;
                        DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, false, false));
                    }

                    _bgUnwind.RunWorkerAsync(arguments);
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

        private string Unexpire(UltraGridRow[] filteredRows)
        {
            try
            {
                GenericBindingList<Position> posList = new GenericBindingList<Position>();
                Dictionary<string, DateTime> dictTaxlotIds = new Dictionary<string, DateTime>();
                foreach (UltraGridRow rows in filteredRows)
                {
                    if (rows.Cells["checkBox"].Value.ToString().ToLower() == "true")
                    {
                        Position pos = (Position)rows.ListObject;
                        switch (pos.ClosingMode)
                        {
                            case ClosingMode.Cash:
                                AddUnwindingAuditEntry(pos, Prana.BusinessObjects.TradeAuditActionType.ActionType.Unwinding_for_Settlement_FX, "Data Unwinded for Cash Settled FX", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                break;

                            case ClosingMode.Expire:
                                AddUnwindingAuditEntry(pos, Prana.BusinessObjects.TradeAuditActionType.ActionType.Unwinding_for_Expired_Option, "Data Unwinded for Expired Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                break;

                            case ClosingMode.Exercise:
                                AddUnwindingAuditEntry(pos, Prana.BusinessObjects.TradeAuditActionType.ActionType.Unwinding_for_Exercised_Option, "Data Unwinded for Exercised Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                break;

                            case ClosingMode.Physical:
                                AddUnwindingAuditEntry(pos, Prana.BusinessObjects.TradeAuditActionType.ActionType.Unwinding_for_PhysicalSettled_Option, "Data Unwinded for Physical Settled Option", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                                _tradeAuditCollection_Closing.Clear();
                                break;

                            default:
                                break;
                        }

                        if (!posList.Contains(pos))
                        {
                            posList.Add(pos);
                        }
                        if (pos.ClosingMode.Equals(ClosingMode.Exercise) || pos.ClosingMode.Equals(ClosingMode.Physical))
                        {
                            if (!dictTaxlotIds.ContainsKey(pos.ID))
                                dictTaxlotIds.Add(pos.ID, pos.ClosingTradeDate);
                        }
                    }
                }
                if (posList.Count == 0)
                {
                    MessageBox.Show("Please select atleast one row to unwind.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }

                StringBuilder s = new StringBuilder();
                StringBuilder taxlotId = new StringBuilder();
                StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();
                List<string> taxlotidList = new List<string>();
                DialogResult userChoice = MessageBox.Show("This will unwind the closing. Would you like to proceed?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (userChoice == DialogResult.Yes)
                {
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            if (DisableEnableParentForm != null)
                            {
                                _statusMessage = "Unwinding Data Please Wait";
                                this.BeginInvoke(DisableEnableParentForm, new EventArgs<string, bool, bool>(_statusMessage, false, true));
                            }
                        }
                    }
                    StringBuilder message = new StringBuilder();
                    Dictionary<string, StatusInfo> positionStatusDict = new Dictionary<string, StatusInfo>();
                    List<DateTime> closingDates = new List<DateTime>();
                    if (dictTaxlotIds.Count > 0)
                    {
                        positionStatusDict = _closingServices.InnerChannel.ArePositionEligibletoUnwind(dictTaxlotIds);
                    }
                    foreach (Position position in posList)
                    {
                        if (position != null)
                        {
                            if (!positionStatusDict.ContainsKey(position.ID))
                            {
                                s.Append(position.TaxLotClosingId.ToString());
                                s.Append(",");

                                taxlotClosingIDWithClosingDate.Append(position.TaxLotClosingId.ToString());
                                taxlotClosingIDWithClosingDate.Append('_');
                                taxlotClosingIDWithClosingDate.Append(position.ClosingTradeDate.ToString());
                                taxlotClosingIDWithClosingDate.Append('_');
                                taxlotClosingIDWithClosingDate.Append(position.AccountValue.ID.ToString());
                                taxlotClosingIDWithClosingDate.Append(",");
                                if (!taxlotidList.Contains(position.ID.ToString()))
                                {
                                    taxlotId.Append(position.ID.ToString());
                                    taxlotId.Append(",");
                                }
                                if (!taxlotidList.Contains(position.ClosingID.ToString()))
                                {
                                    taxlotId.Append(position.ClosingID.ToString());
                                    taxlotId.Append(",");
                                }
                                closingDates.Add(position.ClosingTradeDate);
                            }
                            else
                            {
                                if (positionStatusDict.ContainsKey(position.ID))
                                {
                                    Dictionary<string, PostTradeEnums.Status> dictUnderlyingExercised = positionStatusDict[position.ID].ExercisedUnderlying;
                                    foreach (KeyValuePair<string, PostTradeEnums.Status> kp in dictUnderlyingExercised)
                                    {
                                        message.Append("Underlying IDs : ");

                                        foreach (string key in dictUnderlyingExercised.Keys)
                                        {
                                            string id = key;
                                            message.Append(id);
                                            message.Append(", ");
                                        }
                                        message.Append("TaxlotID : ");
                                        message.Append(position.ID.ToString());
                                        message.Append(Environment.NewLine);
                                    }
                                }
                            }
                        }
                    }
                    if (message.Length > 0)
                    {
                        return message.ToString();
                    }
                    else
                    {
                        Dictionary<string, StatusInfo> dictFutureDateClosedInfo = _closingServices.InnerChannel.GetFutureDateClosingInfo(s.ToString());
                        if (dictFutureDateClosedInfo != null && dictFutureDateClosedInfo.Count > 0)
                        {
                            foreach (KeyValuePair<string, StatusInfo> kp in dictFutureDateClosedInfo)
                            {
                                message.Append(kp.Value.Details);
                                if (kp.Value.Status.Equals(PostTradeEnums.Status.CorporateAction))
                                {
                                    message.Append("  (Corporate Action)");
                                }
                                else
                                {
                                    message.Append("  (closed)");
                                }
                                message.Append(Environment.NewLine);
                            }

                            return message.ToString();
                        }
                        else
                        {
                            if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                            {
                                foreach (var closingDate in closingDates)
                                {
                                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closingDate))
                                    {
                                        MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                            + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return null;
                                    }
                                } 
                            }
                            ClosingData closingData = _allocationServices.InnerChannel.UnWindClosing(s.ToString(), taxlotId.ToString(), taxlotClosingIDWithClosingDate.ToString());
                            ClosingClientSideMapper.UpdateRepository(closingData);
                            _statusMessage = "Data unwound successfully";
                        }
                    }
                }
                else
                {
                    _statusMessage = "Unwinding Canceled";
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
            return null;
        }

        #endregion

        #region Column Captions

        const string CAP_TaxlotId = "Taxlot ID";
        const string CAP_Account = "Account";
        const string CAP_Strategy = "Strategy";
        const string CAP_TradeDate = "Trade Date";
        const string CAP_ProcessDate = "Process Date";
        const string CAP_OriginalPurchaseDate = "OriginalPurchase Date";
        const string CAP_PositionType = "Position Type";
        const string CAP_Symbol = "Symbol";
        const string CAP_StartQty = "Start Quantity";
        const string CAP_CloseQty = "Close Quantity";
        const string CAP_OpenQty = "Open Quantity";
        const string CAP_AvgPrice = "Average Price";
        const string CAP_Commission = "TotalCommission&Fees";
        const string CAP_Fees = "Fees";
        const string CAP_OtherFees = "Other Fees";
        const string CAP_RealizedPNL = "Realized PNL(C.B.)";
        const string CAP_AUECCloseDt = "AUEC Close Date";
        const string CAP_CloseDt = "Close Date";
        const string CAP_Side = "Original Side";
        const string CAP_NetNotional = "Net Notional";
        const string CAP_CashSettlePrice = "Settlement Price";
        const string CAP_SettledQty = "Settlement Qty ";
        const string CAP_SettlementMode = "Settlement Mode";
        const string CAP_SecurityFullName = "Security Name";
        const string CAP_TransactionType = "Transaction Type";
        const string CAP_ItmOtm = "ITM/OTM";
        const string CAP_PercentOfITMOTM = "% of ITM/OTM";
        const string CAP_IntrinsicValue = "Intrinsic value";
        const string CAP_DaysToExpiry = "Days to Expiry";
        const string CAP_GainLossIfExerciseAssign = "Gain/Loss if Ex./Assign";


        #endregion

        #region Allocated Trades Grid columns

        const string CAPTION_AllocationID = "TaxLotID";
        const string CAPTION_TradeDate = "AUECLocalDate";
        const string CAPTION_ProcessDate = "ProcessDate";
        const string CAPTION_OriginalPurchaseDate = "OriginalPurchaseDate";
        const string CAPTION_TradeDateUTC = "TradeDateUTC";
        const string CAPTION__ClosingTradeDate = "ClosingTradeDate";
        const string CAPTION_Side = "OrderSide";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_SecurityFullName = "CompanyName";
        const string CAPTION_OpenQuantity = "TaxLotQty";
        const string CAPTION_ClosedQty = "ClosedQty";

        const string CAPTION_AveragePrice = "AvgPrice";
        const string CAPTION_UnitCost = "UnitCost";
        const string CAPTION_Account = "Level1Name";
        const string CAPTION_SideID = "OrderSideTagValue";
        const string CAPTION_IsPosition = "IsPosition";
        const string CAPTION_AUEC = "AUECID";
        const string CAPTION_PositionTaxlotID = "PositionTaxlotID";
        const string CAPTION_StrikePrice = "Strike Price";

        const string CAPTION_OpenCommission = "OpenTotalCommissionandFees";
        const string CAPTION_OpenTotalCommissionandFees = "PositionTotalCommissionandFees";
        const string CAPTION_OpenFees = "OtherBrokerOpenFees";
        const string CAPTION_ClosedCommission = "ClosedTotalCommissionandFees";
        const string CAPTION_ClosingTotalCommissionandFees = "ClosedTotalCommissionandFees";
        const string CAPTION_ClosedFees = "ClosedOtherBrokerFees";
        const string CAPTION_OpenOtherFees = "OpenOtherFees";
        const string CAPTION_ClosedOtherFees = "ClosedOtherFees";
        const string CAPTION_NetNotionalValue = "NetNotionalValue";
        const string CAPTION_UnitCostBase = "UnitCostBase";
        const string CAPTION_ExecutedQuantity = "ExecutedQty";
        const string CAPTION_UnrealizedPNL = "UnrealizedPNL";
        const string CAPTION_AvgPriceBase = "AvgPriceBase";
        const string CAPTION_ParentPositionBalanceQuantity = "ParentPositionBalanceQuantity";
        const string CAPTION_PositionStartDate = "PositionStartDate";
        const string CAPTION_ParentPositionEndDate = "ParentPositionEndDate";
        const string CAPTION_StrategyValue = "Level2Name";
        const string CAPTION_MonthsProfitForParentPosition = "MonthsProfitForParentPosition";
        const string CAPTION_SettledQty = "SettledQty";
        const string CAPTION_CashSettledPrice = "CashSettledPrice";
        const string CAPTION_IntClosingMode = "IntClosingMode";
        const string CAPTION_ClosingMode = "ClosingMode";
        const string CAPTION_AssetCategoryValue = "AssetCategoryValue";
        const string CAPTION_ExpiryDate = "ExpirationDate";
        const string CAPTION_UnderLying = "UnderlyingName";
        const string CAPTION_GeneratedTaxlotQty = "GeneratedTaxlotQty";
        const string CAPTION_ExpirationID = "ExpirationID";
        const string CAPTION_IsSwap = "ISSwap";
        const string CAPTION_PositionTag = "PositionTag";
        const string CAPTION_ClosingTag = "ClosingPositionTag";
        const string CAPTION_OpenAveragePrice = "OpenAveragePrice";
        const string CAPTION_ClosedAveragePrice = "ClosedAveragePrice";
        const string CAPTION_ClosedTotalCommissionandFees = "ClosingTotalCommissionandFees";
        const string CAPTION_PositionCommission = "PositionTotalCommissionandFees";
        const string CAPTION_IsSwapped = "IsSwapped";
        const string CAPTION_LeadCurrencyID = "LeadCurrencyID";
        const string CAPTION_VsCurrencyID = "VsCurrencyID";
        const string COLUMN_TransactionType = "TransactionType";
        const string COLUMN_StrikePrice = "StrikePrice";
        const string COLUMN_PutOrCalls = "PutOrCall";
        const string CAPTION_CumQty = "CumQty";
        const string CAPTION_FXRate = "FXRate";
        const string CAPTION_NotionalChange = "NotionalChange";
        const string CAPTION_CostBasisGrossPNL = "CostBasisGrossPNL";
        const string CAPTION_QUANTITY = "Quantity";
        const string CAPTION_ITMOTM = "ItmOtm";
        const string CAPTION_PERCENTOFITMOTM = "PercentOfITMOTM";
        const string CAPTION_INTRINSICVALUE = "IntrinsicValue";
        const string CAPTION_DAYSTOEXPIRY = "DaysToExpiry";
        const string CAPTION_GAINLOSSIFEXERCISEASSIGN = "GainLossIfExerciseAssign";
        const string COLUMN_FACTSETSYMBOL = "FactSetSymbol";
        const string COLUMN_ACTIVSYMBOL = "ActivSymbol";
        const string CAP_FACTSETSYMBOL = "FactSet Symbol";
        const string CAP_ACTIVSYMBOL = "ACTIV Symbol";

        const string CAPTION_TradeAttribute1 = "Trade Attribute 1";
        const string CAPTION_TradeAttribute2 = "Trade Attribute 2";
        const string CAPTION_TradeAttribute3 = "Trade Attribute 3";
        const string CAPTION_TradeAttribute4 = "Trade Attribute 4";
        const string CAPTION_TradeAttribute5 = "Trade Attribute 5";
        const string CAPTION_TradeAttribute6 = "Trade Attribute 6";
        const string COLUMN_TradeAttribute1 = "TradeAttribute1";
        const string COLUMN_TradeAttribute2 = "TradeAttribute2";
        const string COLUMN_TradeAttribute3 = "TradeAttribute3";
        const string COLUMN_TradeAttribute4 = "TradeAttribute4";
        const string COLUMN_TradeAttribute5 = "TradeAttribute5";
        const string COLUMN_TradeAttribute6 = "TradeAttribute6";
        public List<string> OpenTaxlotGridColumns
        {
            get
            {
                List<string> TaxlotGridColumns = new List<string>();
                //TaxlotGridColumns = new List<string>();

                //TaxlotGridColumns.Add(CAPTION_AllocationID);
                TaxlotGridColumns.Add(CAPTION_TradeDate);
                TaxlotGridColumns.Add(CAPTION_IsSwap);
                TaxlotGridColumns.Add(CAPTION_Side);
                TaxlotGridColumns.Add(CAPTION_Symbol);
                TaxlotGridColumns.Add(CAPTION_SecurityFullName);
                TaxlotGridColumns.Add(CAPTION_OpenQuantity);
                TaxlotGridColumns.Add(CAPTION_AssetCategoryValue);
                TaxlotGridColumns.Add(CAPTION_AveragePrice);
                TaxlotGridColumns.Add(CAPTION_NetNotionalValue);
                //TaxlotGridColumns.Add(COL_SideID);
                //TaxlotGridColumns.Add(COL_AUEC);
                TaxlotGridColumns.Add(CAPTION_OpenCommission);
                TaxlotGridColumns.Add(CAPTION_UnderLying);
                TaxlotGridColumns.Add(CAPTION_UnitCost);
                TaxlotGridColumns.Add(CAPTION_PositionTag);
                TaxlotGridColumns.Add(CAPTION_StrategyValue);
                TaxlotGridColumns.Add(CAPTION_Account);
                TaxlotGridColumns.Add(CAPTION_ExpiryDate);
                TaxlotGridColumns.Add(CAPTION_ClosingMode);
                TaxlotGridColumns.Add(CAPTION_SettledQty);
                TaxlotGridColumns.Add(CAPTION_ProcessDate);
                TaxlotGridColumns.Add(CAPTION_OriginalPurchaseDate);
                TaxlotGridColumns.Add("AssetDerivative");
                TaxlotGridColumns.Add(COLUMN_StrikePrice);
                return TaxlotGridColumns;
            }
        }

        #endregion

        #region Grid Columns for Net Positions

        const string CAPTION_ID = "ID";

        const string CAPTION_StartDate = "StartDate";
        const string CAPTION_LastActivityDate = "LastActivityDate";
        const string CAPTION_PositionType = "PositionType";
        const string CAPTION_PositionalTag = "PositionalTag";
        const string CAPTION_PNLPOSITION = "PNLWhenTaxLotsPopulated";
        const string CAPTION_PNL = "CostBasisRealizedPNL";
        const string CAPTION_StartTaxLotID = "StartTaxLotID";
        const string CAPTION_PositionStartQuantity = "PositionStartQty";
        const string CAPTION_AccountID = "AccountID";
        const string CAPTION_Multiplier = "Multiplier";
        const string CAPTION_AUECID = "AUECID";
        const string CAPTION_RealizedPNL = "CostBasisRealizedPNL";
        const string CAPTION_RecordType = "RecordType";
        const string CAPTION_Status = "Status";
        const string CAPTION_EndDate = "EndDate";
        const string CAPTION_Description = "Description";
        const string CAPTION_Strategy = "Strategy";
        const string CAPTION_StrategyID = "StrategyID";
        const string CAPTION_MarkPriceForMonth = "MarkPriceForMonth";
        const string CAPTION_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
        const string CAPTION_NotionalValue = "NotionalValue";
        const string CAPTION_AvgPriceRealizedPL = "AvgPriceRealizedPL";
        const string CAPTION_SymbolAveragePrice = "SymbolAveragePrice";
        const string CAPTION_AUECLocalCloseDate = "AUECLocalCloseDate";
        const string CAPTION_CloseDate = "ClosingDate";
        const string CAPTION_ParentClosedQty = "ParentClosedQty";
        const string CAPTION_TimeofSaveUTC = "TimeOfSaveUTC";
        const string CAPTION_PositionSide = "Side";
        const string CAPTION_TradeDatePosition = "TradeDate";
        const string CAPTION_AccountValue = "AccountValue";
        const string CAPTION_PutOrCall = "PutOrCall";
        #endregion

        const string PositionalSide_Long = "Long";
        const string PositionalSide_Short = "Short";

        # region set Grids
        delegate void SetDataHandler(bool refresh);

        /// <summary>
        /// This method is being called on a separate thread, hence marshalled the calls on main thread.
        /// </summary>
        internal void SetGridDataSources()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(SetGridDataSources);
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        //_isInitialized = false;

                        //grdAccountUnexpired.DataSource = null;

                        //grdAccountUnexpired.DataSource = _closingServices.InnerChannel.GetTaxLots();
                        grdAccountUnexpired.DataSource = ClosingClientSideMapper.OpenTaxlots;
                        // grdAccountExpired.DataSource = null;
                        //grdAccountExpired.DataSource = _closingServices.InnerChannel.GetNetPositions();
                        grdAccountExpired.DataSource = ClosingClientSideMapper.Netpositions;
                        if (grdAccountUnexpired.DisplayLayout.Bands[0].Columns.Exists(ClosingConstants.COL_BloombergExCode)) 
                        {
                            grdAccountUnexpired.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_BloombergExCode].Header.Caption = ClosingConstants.Header_BloombergExCode;
                            if(!grdAccountUnexpired.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_BloombergExCode].IsVisibleInLayout)
                                grdAccountUnexpired.DisplayLayout.Bands[0].Columns[ClosingConstants.COL_BloombergExCode].Hidden = true;
                        }
                        // SetDefaultFilters();
                        grpCreatePosition.Expanded = false;
                        ctrlCreateAndImportPosition1.Visible = true;
                        grdCashandExpire.Visible = false;
                        grpCreatePosition.Text = "Create Transaction";
                        _isControlInitialized = false;
                        ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                        //_dictSymbolTaxLotList.Clear();
                        _isSwap = false;
                        _actionClick = true;
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

        internal void SetDefaultFiltersGrdAccountUnexpired()
        {
            try
            {

                UltraGridLayout gridLUnexpiredLayout = grdAccountUnexpired.DisplayLayout;
                //gridLUnexpiredLayout.Bands[0].ColumnFilters.ClearAllFilters();

                // Asset derivative is a new column made by combining asset category and is swapped, 
                //so that it is easy to apply filters and those equities that are not swapped doesnt look up in the filter dropdown
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].LogicalOperator = FilterLogicalOperator.Or;
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.EquityOption);
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, "Equity Swap");
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.Future);
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.FXForward);
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.FutureOption);
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.FX);
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, "FXForward-NDF");
                gridLUnexpiredLayout.Bands[0].ColumnFilters["AssetDerivative"].FilterConditions.Add(FilterComparisionOperator.Equals, "FX-NDF");
                //   gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].LogicalOperator = FilterLogicalOperator.Or;
                //   gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.EquityOption);
                //gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.Equity);
                //   gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.Future);
                //   gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.FXForward);
                //   gridLUnexpiredLayout.Bands[0].ColumnFilters[CAPTION_AssetCategoryValue].FilterConditions.Add(FilterComparisionOperator.Equals, AssetCategory.FutureOption);

                UltraGridColumn colAssetName = gridLUnexpiredLayout.Bands[0].Columns[ClosingConstants.COL_AssetName];
                colAssetName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                headerCheckBoxUnExpired.Checked = CheckState.Unchecked;


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

        internal void SetDefaultFiltersGrdAccountExpired()
        {
            try
            {
                UltraGridLayout gridLayout = grdAccountExpired.DisplayLayout;
                gridLayout.Bands[0].ColumnFilters.ClearAllFilters();
                gridLayout.Bands[0].ColumnFilters[CAPTION_IsExpired_Settled].FilterConditions.Add(FilterComparisionOperator.Equals, true);
                gridLayout.Bands[0].ColumnFilters[CAPTION_ClosingMode].FilterConditions.Add(FilterComparisionOperator.NotEquals, ClosingMode.CorporateAction);
                //modified by sachin mishra 20-04-15  JIRA-PRANA-7400 PRANA-6888
                gridLayout.Bands[0].ColumnFilters[CAPTION_ClosingMode].FilterConditions.Add(FilterComparisionOperator.NotEquals, ClosingMode.CostBasisAdjustment);

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

        public void PopulateCloseTradesInterfaceForExpirataionandSettlement(CompanyUser user)
        {
            try
            {
                // _userID = user.CompanyUserID;
                this._user = user;

                // SetGridDataSources();
                ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                ctrlCreateAndImportPosition1.SecurityMaster = _securityMaster;
                ctrlCreateAndImportPosition1.PopulateCreatePositionInterface(_user);
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
        /// Gets the filtered data.
        /// </summary>
        void grdAccountUnexpired_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //UltraGridLayout gridLayout = grdAccountUnexpired.DisplayLayout;
                //_grdBandPositionsTaxlots = grdAccountUnexpired.DisplayLayout.Bands[0];

                if (!String.IsNullOrEmpty(_expirySettlementNewLayoutFilePath) && File.Exists(_expirySettlementNewLayoutFilePath))
                    grdAccountUnexpired.DisplayLayout.LoadFromXml(_expirySettlementNewLayoutFilePath);
                else
                    SetGridColumns(grdAccountUnexpired);

                SetColumnFormatting();
                SetColumnCustomizations(grdAccountUnexpired);
                // Turn on all of the Cut, Copy, and Paste functionality. 
                e.Layout.Override.AllowMultiCellOperations = AllowMultiCellOperation.All;
                // In order to cut or copy, the user needs to select cells or rows. 
                // So set CellClickAction so that clicking on a cell selects that cell
                // instead of going into edit mode.
                e.Layout.Override.SelectTypeRow = SelectType.Extended;
                e.Layout.Override.CellClickAction = CellClickAction.CellSelect;
                e.Layout.Override.SelectTypeCell = SelectType.Extended;

                SetDefaultFiltersGrdAccountUnexpired();
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
        void grdAccountExpired_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                _grdBandNetPositions = grdAccountExpired.DisplayLayout.Bands[0];
                SetColumnsForPositionGrid(_grdBandNetPositions);
                if (!_grdBandNetPositions.Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdAccountExpired, headerCheckBoxUnWind);
                }
                SetDefaultFiltersGrdAccountExpired();
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

        void grdCashandExpire_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                // Turn on all of the Cut, Copy, and Paste functionality. 
                e.Layout.Override.AllowMultiCellOperations = AllowMultiCellOperation.All;
                // In order to cut or copy, the user needs to select cells or rows. 
                // So set CellClickAction so that clicking on a cell selects that cell
                // instead of going into edit mode.
                e.Layout.Override.SelectTypeRow = SelectType.Extended;
                e.Layout.Override.CellClickAction = CellClickAction.CellSelect;
                e.Layout.Override.SelectTypeCell = SelectType.Extended;
                UltraGridBand _grdBandCashAndExpire = grdCashandExpire.DisplayLayout.Bands[0];
                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttrCol = _grdBandCashAndExpire.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttrCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    tradeAttrCol.Hidden = true;
                }
                //grdCashandExpire.BeforeMultiCellOperation += new BeforeMultiCellOperationEventHandler(grdCashandExpire_BeforeMultiCellOperation);
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

        CheckBoxOnHeader_CreationFilter headerCheckBoxUnWind = new CheckBoxOnHeader_CreationFilter();


        void grdAccountUnexpired_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {

            GrdUnExpiredIniRow(e);
        }


        void grd_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {

            try
            {

                if (e.Column.ToString().Equals("AssetDerivative"))
                {
                    ///Rahul 20120206
                    ///Commented this filter becuase of equity filter was not appearing on this tab
                    ///in case of equity swaps.
                    ///see details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1738
                    if (e.ValueList.FindStringExact(AssetCategory.Equity.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.Equity.ToString()));
                    }
                    if (e.ValueList.FindStringExact(AssetCategory.FixedIncome.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.FixedIncome.ToString()));
                    }
                    if (e.ValueList.FindStringExact(AssetCategory.ConvertibleBond.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.ConvertibleBond.ToString()));
                    }
                    if (e.ValueList.FindStringExact(AssetCategory.Indices.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.Indices.ToString()));
                    }
                    if (e.ValueList.FindStringExact(AssetCategory.PrivateEquity.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.PrivateEquity.ToString()));
                    }
                    if (e.ValueList.FindStringExact(AssetCategory.CreditDefaultSwap.ToString()) >= 0)
                    {
                        e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.CreditDefaultSwap.ToString()));
                    }
                    //if (e.ValueList.FindStringExact(AssetCategory.FX.ToString()) >= 0)
                    //{
                    //    e.ValueList.ValueListItems.Remove(e.ValueList.FindStringExact(AssetCategory.FX.ToString()));
                    //}

                }
                if (e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ProcessDate) || e.Column.Key.Equals(ClosingConstants.COL_ExpiryDate) || e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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
        // Dictionary<string, List<TaxLot>> _dictSymbolTaxLotList = new Dictionary<string, List<TaxLot>>();


        void grdAccountUnexpired_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {

            try
            {
                if (e.Column.ToString().Equals("AssetDerivative"))
                {
                    if (e.NewColumnFilter.FilterConditions.Count == 0)
                    {
                        SetDefaultFiltersGrdAccountUnexpired();
                    }
                }
                if ((e.Column.Key.Equals(ClosingConstants.COL_TradeDate) || e.Column.Key.Equals(ClosingConstants.COL_ProcessDate) || e.Column.Key.Equals(ClosingConstants.COL_ExpiryDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdAccountUnexpired.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdAccountUnexpired.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        void grdAccountExpired_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(ClosingConstants.COL_ClosingTradeDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdAccountExpired.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdAccountExpired.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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
        /// With out any use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void grdAccountUnexpired_FilterRow(object sender, Infragistics.Win.UltraWinGrid.FilterRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells[CAPTION_AssetCategoryValue].Value == null)
                {
                    return;
                }

                if (e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.Equity) && e.Row.Cells[CAPTION_IsSwap].Value.Equals(false))
                {
                    e.RowFilteredOut = true;

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

        void grdAccountUnexpired_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                grdAccountUnexpired.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdAccountUnexpired_InitializeRow);
                if (e.Cell.Column.Key.Equals(CAPTION_SettledQty))
                {
                    TaxLot TaxLot = grdAccountUnexpired.ActiveRow.ListObject as TaxLot;
                    if (TaxLot == null)
                    {
                        return;
                    }
                    TaxLot alltrade = e.Cell.Row.ListObject as TaxLot;
                    if (alltrade == null)
                    {
                        return;
                    }
                    double settlementQty = double.Parse(e.Cell.Text);

                    if (settlementQty > alltrade.TaxLotQty)
                    {

                        MessageBox.Show("Settlement Quantity cannot be greater than the traded Quantity. Please enter a valid value.", "Expiration/Settlement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cell.Value = alltrade.TaxLotQty;
                        return;

                    }
                    if (settlementQty <= 0)
                    {

                        MessageBox.Show("Settlement Quantity cannot have a zero or negative value. Please enter a valid value.", "Expiration/Settlement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cell.Value = alltrade.TaxLotQty;
                        return;

                    }

                    if (TaxLot.ClosingMode.Equals(ClosingMode.Physical) || TaxLot.ClosingMode.Equals(ClosingMode.Exercise))
                    {

                        List<TaxLot> taxLots = new List<TaxLot>();
                        UltraGridRow[] rows = grdAccountUnexpired.Rows.GetFilteredInNonGroupByRows();

                        foreach (UltraGridRow row in rows)
                        {
                            if (row.Cells["checkBox"].Text == true.ToString())
                            {
                                row.Appearance.BackColor.GetBrightness();
                                TaxLot taxLot = row.ListObject as TaxLot;
                                taxLot.IsExerciseAtZero = false;
                                taxLots.Add(taxLot);
                            }
                        }
                        if (taxLots.Count > 0)
                        {
                            ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                            ExcerciseandPhysical(taxLots, true);
                        }
                    }
                }


                if (e.Cell.Column.Key.Equals("checkBox"))
                {
                    //_isInitialized = true;
                    TaxLot TaxLot = e.Cell.Row.ListObject as TaxLot;
                    if (TaxLot == null)
                    {
                        return;
                    }
                    string symbol = TaxLot.Symbol;
                    if (e.Cell.Value.Equals(true))
                    {
                        e.Cell.Row.Appearance.BackColor = Color.Gold;
                        e.Cell.Row.Appearance.BackColor2 = Color.Gold;
                        e.Cell.Row.Appearance.ForeColor = Color.Black;

                        if (!String.IsNullOrEmpty(symbol))
                        {
                            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(TaxLot.AssetCategoryValue);
                            if (baseAssetCategory.Equals(AssetCategory.Option)
                                || baseAssetCategory.Equals(AssetCategory.Future)
                                || baseAssetCategory.Equals(AssetCategory.FX)
                                )
                            {
                                if (TaxLot.SettledQty == 0)
                                {
                                    TaxLot.SettledQty = TaxLot.TaxLotQty;
                                }
                                //if (TaxLot.SideID.Equals(FIXConstants.SIDE_Buy_Open) || TaxLot.SideID.Equals(FIXConstants.SIDE_Buy) || TaxLot.SideID.Equals(FIXConstants.SIDE_Sell_Open) || TaxLot.SideID.Equals(FIXConstants.SIDE_SellShort))
                                //{
                                //    TaxLot.SettledQty = TaxLot.OpenQty;
                                //}
                            }
                        }


                    }

                    else if (e.Cell.Value.Equals(false))
                    {

                        string side = e.Cell.Row.Cells[CAPTION_SideID].Value.ToString();
                        int auecID = Convert.ToInt32(e.Cell.Row.Cells[CAPTION_AUECID].Text);
                        DateTime auecDate = DateTimeConstants.MinValue;
                        DateTime expirationDate = DateTimeConstants.MinValue;
                        expirationDate = Convert.ToDateTime(e.Cell.Row.Cells[CAPTION_ExpiryDate].Value.ToString());

                        //if (preferences.IsCurrentDateClosing)

                        if (ClosingPrefManager.IsCurrentDateClosing)
                        {
                            auecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                        }
                        else
                        {
                            //auecDate = preferences.CloseTradeDate;
                            auecDate = ClosingPrefManager.CloseTradeDate;
                        }
                        if (auecDate > expirationDate)
                        {
                            e.Cell.Row.Appearance.BackColor = Color.Transparent;
                        }
                        else
                        {
                            e.Cell.Row.Appearance.BackColor = Color.Black;
                        }

                        if (side.Equals(FIXConstants.SIDE_Buy) || side.Equals(FIXConstants.SIDE_Buy_Open))
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.Green;
                            }
                            else
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.FromArgb(39, 174, 96);
                            }
                        }
                        if (string.Equals(side, FIXConstants.SIDE_SellShort) || string.Equals(side, FIXConstants.SIDE_Sell_Open))
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.Yellow;
                            }
                            else
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.FromArgb(164, 164, 0);
                            }
                        }
                        else
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.DarkTurquoise;
                            }
                            else
                            {
                                e.Cell.Row.Appearance.ForeColor = Color.FromArgb(0, 118, 118);
                            }
                        }

                        TaxLot.SettledQty = 0.0;
                        TaxLot.ClosingMode = ClosingMode.None;


                    }
                }
                grdAccountUnexpired.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdAccountUnexpired_InitializeRow);
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

        void grdAccountUnexpired_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                TaxLot alltrade = e.Cell.Row.ListObject as TaxLot;
                if (alltrade == null)
                {
                    return;
                }

                if (e.Cell.Column.Key.Equals(CAPTION_SettledQty))
                {
                    if (e.Cell.Text.Equals(string.Empty))
                    {
                        e.Cell.CancelUpdate();
                        return;
                    }

                }

                //if (e.Cell.Column.Key == "checkBox")
                //{

                //    if (alltrade.ISSwap)
                //    {
                //        e.Cell.Value = true;
                //    }
                //}
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
        /// AfterCellUpdate of grdCashandExpire
        /// It updates cash settlement price corresponding to the closingdate/expirydate changed if expiry date is greater than trade date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool check = true;
        void grdCashandExpire_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (check)
                {
                    if (e.Cell.Column.Key.Equals(CAPTION_ExpiryDate) && _isCloseAtSpotPrice)
                    {
                        DateTime expiryDate = DateTime.Parse(e.Cell.Value.ToString());
                        DateTime tradeDate = DateTime.Parse(e.Cell.Row.Cells[CAPTION_TradeDate].Value.ToString());
                        int LeadCurrencyID = int.Parse(e.Cell.Row.Cells[CAPTION_LeadCurrencyID].Value.ToString());
                        int VsCurrencyID = int.Parse(e.Cell.Row.Cells[CAPTION_VsCurrencyID].Value.ToString());
                        string assetCategory = e.Cell.Row.Cells[CAPTION_AssetCategoryValue].Value.ToString();
                        //e.Cell.Row.Cells[CAPTION_].Value.ToString()
                        if (expiryDate.Date < tradeDate.Date)
                        {
                            this.check = false;
                            e.Cell.Value = e.Cell.OriginalValue;
                            return;
                        }
                        else if (assetCategory.Equals(AssetCategory.FX.ToString()) || assetCategory.Equals(AssetCategory.FXForward.ToString()))
                        {
                            ////CHMW-3132	Account wise fx rate handling for expiration settlement
                            int accountID = CachedDataManager.GetInstance.GetAccountID(e.Cell.Row.Cells[CAPTION_Account].Value.ToString());
                            int accountCurrencyID;
                            if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountID))
                            {
                                accountCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountID];
                            }
                            else
                            {
                                accountCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();//ClientsCommonDataManager.GetCompanyBaseCurrency(_companyID);
                            }
                            //for forward symbols get cash settlement price from forex converter
                            e.Cell.Row.Cells["FXRate"].Value = ForexConverter.GetInstance(accountCurrencyID).GetFxRateFromCurrenciesForGivenDateOnly(LeadCurrencyID, VsCurrencyID, expiryDate.AddDays(-1), CachedDataManager.GetInstance.GetAccountID(e.Cell.Row.Cells[CAPTION_Account].Value.ToString()));
                            e.Cell.Row.Cells[CAPTION_CashSettledPrice].Value = WindsorContainerManager.GetMarkPriceForSymbolAndDate(e.Cell.Row.Cells[CAPTION_Symbol].Value.ToString(), expiryDate.Date.AddDays(-1), accountID);
                        }
                        else if (assetCategory.Equals(AssetCategory.Future.ToString()))
                        {
                            //dictionary that passes symbol and cash settlement date to pricing server, to get mark price for that symbol
                            Dictionary<string, DateTime> dictSymbolWithSettlementDate = new Dictionary<string, DateTime>();
                            dictSymbolWithSettlementDate.Add(e.Cell.Row.Cells[CAPTION_Symbol].Value.ToString(), DateTime.Parse(e.Cell.Value.ToString()));
                            //dictionary that gets updated symbol and markprices from the pricing server 
                            Dictionary<string, double> dictSymbolWithMarkPrice = _pricingServicesProxy.InnerChannel.GetMarkPricesForSymbolAndExactDate(dictSymbolWithSettlementDate);
                            e.Cell.Row.Cells[CAPTION_CashSettledPrice].Value = dictSymbolWithMarkPrice[e.Cell.Row.Cells[CAPTION_Symbol].Value.ToString()];
                        }
                    }
                    if (e.Cell.Column.Key.Equals(CAPTION_SettledQty))
                    {
                        if (Convert.ToInt64(e.Cell.Value) > Convert.ToInt64(e.Cell.Row.Cells[CAPTION_OpenQuantity].Value))
                        {
                            e.Cell.Value = e.Cell.OriginalValue;
                            MessageBox.Show("Settlement Quantity cannot be zero or greater than traded quantity. Please enter a valid value", "Expiration/Settlement Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        if (Convert.ToInt64(e.Cell.Value) <= 0)
                        {
                            e.Cell.Value = e.Cell.OriginalValue;
                            MessageBox.Show("Settlement Quantity cannot have a Zero or negative value. Please enter a valid value", "Expiration/Settlement Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    this.check = true;
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

        //void grdCashandExpire_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //{

        //}




        private void GrdUnExpiredIniRow(Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                //if (!_isInitialized)
                //{
                string side = string.Empty;
                DateTime expirationDate = DateTimeConstants.MinValue;
                int auecID = int.MinValue;
                DateTime auecDate = DateTimeConstants.MinValue;
                UltraGridLayout gridLayout = grdAccountUnexpired.DisplayLayout;
                if (e.Row.Cells[CAPTION_PutOrCall].Text.Equals((int.MinValue).ToString()))
                {
                    e.Row.Cells[CAPTION_PutOrCall].Value = 0;
                }
                if (!gridLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdAccountUnexpired, headerCheckBoxUnExpired);
                }
                if (e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.Equity) && e.Row.Cells[CAPTION_IsSwap].Value.Equals(true))
                {
                    string swap = "Equity Swap";
                    e.Row.Cells["AssetDerivative"].Value = (object)swap;

                }
                else if ((e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.FXForward) || (e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.FX)) && e.Row.Cells["IsNDF"].Value.Equals(true)))
                {
                    string ndf = string.Empty;
                    if (e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.FXForward))
                    {
                        ndf = "FXForward-NDF";
                        e.Row.Cells["AssetDerivative"].Value = (object)ndf;
                    }
                    if (e.Row.Cells[CAPTION_AssetCategoryValue].Value.Equals(AssetCategory.FX))
                    {
                        ndf = "FX-NDF";
                        e.Row.Cells["AssetDerivative"].Value = (object)ndf;
                    }

                }
                else
                {
                    e.Row.Cells["AssetDerivative"].Value = e.Row.Cells[CAPTION_AssetCategoryValue].Value;
                }

                if (e.Row.Cells[CAPTION_Side] != null)
                {
                    side = e.Row.Cells[CAPTION_SideID].Value.ToString();
                    auecID = Convert.ToInt32(e.Row.Cells[CAPTION_AUECID].Text);

                    if (ClosingPrefManager.IsCurrentDateClosing)
                    {
                        auecDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    }
                    else
                    {
                        //auecDate = preferences.CloseTradeDate;
                        auecDate = ClosingPrefManager.CloseTradeDate;
                    }
                    if (auecDate > expirationDate)
                    {
                        e.Row.Appearance.BackColor = Color.Transparent;
                    }
                    else
                    {
                        e.Row.Appearance.BackColor = Color.Gray;
                    }

                    if (side.Equals(FIXConstants.SIDE_Buy) || side.Equals(FIXConstants.SIDE_Buy_Open))
                    {
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            e.Row.Appearance.ForeColor = Color.LightGreen;
                        }
                        else
                        {
                            e.Row.Appearance.ForeColor = Color.FromArgb(34, 209, 0);
                        }
                    }
                    else
                    {

                        if (string.Equals(side, FIXConstants.SIDE_SellShort) || string.Equals(side, FIXConstants.SIDE_Sell_Open))
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Row.Appearance.ForeColor = Color.Yellow;
                            }
                            else
                            {
                                e.Row.Appearance.ForeColor = Color.FromArgb(164, 164, 0);
                            }
                        }
                        else
                        {
                            if (!CustomThemeHelper.ApplyTheme)
                            {
                                e.Row.Appearance.ForeColor = Color.DarkTurquoise;
                            }
                            else
                            {
                                e.Row.Appearance.ForeColor = Color.FromArgb(0, 118, 118);
                            }
                        }
                    }

                }

                //}
                //else
                //{
                //    if (grdAccountUnexpired.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                //    {

                //    }
                //}
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
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        /// <param name="isAccountData">if set to <c>true</c> [is account data].</param>
        private void SetGridColumns(UltraGrid grid)
        {
            try
            {
                UltraGridBand gridBand = grid.DisplayLayout.Bands[0];

                UltraGridColumn colTradeDate = gridBand.Columns[CAPTION_TradeDate];
                colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colTradeDate.Width = 140;
                colTradeDate.Header.VisiblePosition = 1;
                colTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colProcessDate = gridBand.Columns[CAPTION_ProcessDate];
                colProcessDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colProcessDate.Width = 140;
                colProcessDate.Header.VisiblePosition = 2;
                colProcessDate.CellClickAction = CellClickAction.Default;
                colProcessDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colOriginalPurchaseDate = gridBand.Columns[CAPTION_OriginalPurchaseDate];
                colOriginalPurchaseDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colOriginalPurchaseDate.Width = 140;
                colOriginalPurchaseDate.Header.VisiblePosition = 3;
                colOriginalPurchaseDate.CellClickAction = CellClickAction.Default;
                colOriginalPurchaseDate.CellActivation = Activation.NoEdit;

                UltraGridColumn ColSide = gridBand.Columns[CAPTION_Side];
                ColSide.Width = 65;
                ColSide.Header.VisiblePosition = 4;
                ColSide.CellActivation = Activation.NoEdit;

                UltraGridColumn colSymbol = gridBand.Columns[CAPTION_Symbol];
                colSymbol.Width = 50;
                colSymbol.Header.VisiblePosition = 5;
                colSymbol.CellActivation = Activation.NoEdit;

                UltraGridColumn colOpenQuantity = gridBand.Columns[CAPTION_OpenQuantity];
                colOpenQuantity.Width = 65;
                colOpenQuantity.Header.VisiblePosition = 6;
                colOpenQuantity.CellActivation = Activation.NoEdit;

                UltraGridColumn colSettledQty = gridBand.Columns[CAPTION_SettledQty];
                colSettledQty.Width = 80;
                colSettledQty.Header.VisiblePosition = 7;
                colSettledQty.CellActivation = Activation.AllowEdit;

                UltraGridColumn colAveragePrice = gridBand.Columns[CAPTION_AveragePrice];
                colAveragePrice.Width = 80;
                colAveragePrice.Header.VisiblePosition = 8;
                colAveragePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colUnitCost = gridBand.Columns[CAPTION_UnitCost];
                colUnitCost.Width = 80;
                colUnitCost.Header.VisiblePosition = 9;
                colUnitCost.CellActivation = Activation.NoEdit;

                UltraGridColumn colOpenCommision = gridBand.Columns[CAPTION_OpenCommission];
                colOpenCommision.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                colOpenCommision.Width = 50;
                colOpenCommision.Header.VisiblePosition = 10;
                colOpenCommision.CellActivation = Activation.NoEdit;

                //Added By : Manvendra Prajapati
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-11539

                //UltraGridColumn colPutOrCalls = gridBand.Columns[COLUMN_PutOrCalls];
                //colPutOrCalls.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                //colPutOrCalls.Hidden = true;

                UltraGridColumn colNetNotionalValue = gridBand.Columns[CAPTION_NetNotionalValue];
                colNetNotionalValue.Width = 60;
                colNetNotionalValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                colNetNotionalValue.Header.VisiblePosition = 11;
                colNetNotionalValue.CellActivation = Activation.NoEdit;

                UltraGridColumn colAccount = gridBand.Columns[CAPTION_Account];
                colAccount.Width = 80;
                colAccount.Header.VisiblePosition = 12;
                colAccount.CellActivation = Activation.NoEdit;

                UltraGridColumn colISSwap = gridBand.Columns[CAPTION_IsSwap];
                colISSwap.Hidden = true;

                UltraGridColumn colISNDF = gridBand.Columns["IsNDF"];
                colISNDF.Hidden = true;
                //Column Added By faisal Shah
                UltraGridColumn colStrikePrice = gridBand.Columns[COLUMN_StrikePrice];
                colStrikePrice.Width = 80;
                colStrikePrice.Header.VisiblePosition = 13;
                colStrikePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingMode = gridBand.Columns[CAPTION_ClosingMode];
                colClosingMode.Width = 80;
                colClosingMode.Header.VisiblePosition = 14;
                colClosingMode.CellActivation = Activation.NoEdit;

                UltraGridColumn colAssetcategoryValue = gridBand.Columns[CAPTION_AssetCategoryValue];
                colAssetcategoryValue.Width = 80;
                colAssetcategoryValue.CellActivation = Activation.NoEdit;
                colAssetcategoryValue.Hidden = true;

                UltraGridColumn colExpiryDate = gridBand.Columns[CAPTION_ExpiryDate];
                colExpiryDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colExpiryDate.Width = 140;
                colExpiryDate.Header.VisiblePosition = 16;
                colExpiryDate.CellActivation = Activation.NoEdit;

                gridBand.Columns.Add("AssetDerivative");
                UltraGridColumn colAsset = gridBand.Columns["AssetDerivative"];
                colAsset.Width = 80;
                colAsset.Header.VisiblePosition = 15;
                colAsset.CellActivation = Activation.NoEdit;

                UltraGridColumn colUnderLyingName = gridBand.Columns[CAPTION_UnderLying];
                colUnderLyingName.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[CAPTION_StrategyValue];
                colStrategyIDValue.CellActivation = Activation.NoEdit;

                UltraGridColumn colSecurityName = gridBand.Columns[CAPTION_SecurityFullName];
                colSecurityName.CellActivation = Activation.NoEdit;

                UltraGridColumn colPostionTag = gridBand.Columns[CAPTION_PositionTag];
                colPostionTag.CellActivation = Activation.NoEdit;

                UltraGridColumn colIsSwapped = gridBand.Columns[CAPTION_IsSwap];
                colIsSwapped.CellActivation = Activation.ActivateOnly;
                colIsSwapped.Header.VisiblePosition = 17;
                colIsSwapped.AllowRowFiltering = DefaultableBoolean.False;

                if (gridBand.Columns.Exists(ClosingConstants.COL_AssetCategoryValue))
                {
                    UltraGridColumn colAssetClass = gridBand.Columns[ClosingConstants.COL_AssetCategoryValue];
                    List<EnumerationValue> assetCategoryType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(Prana.BusinessObjects.AppConstants.AssetCategory));
                    ValueList assetClassValueList = new ValueList();
                    foreach (EnumerationValue value in assetCategoryType)
                    {
                        assetClassValueList.ValueListItems.Add(value.Value, value.DisplayText);
                    }
                    colAssetClass.ValueList = assetClassValueList;
                    colAssetClass.Hidden = false;
                }

                if (gridBand.Columns.Exists(CAPTION_PutOrCall))
                {
                    UltraGridColumn colPutOrCall = gridBand.Columns[CAPTION_PutOrCall];
                    List<EnumerationValue> PutOrCallType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(OptionType));
                    ValueList PutOrCallValueList = new ValueList();
                    foreach (EnumerationValue value in PutOrCallType)
                    {
                        PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText);
                    }
                    colPutOrCall.ValueList = PutOrCallValueList;
                    colPutOrCall.Header.Caption = CAPTION_PutOrCall;
                    colPutOrCall.CellActivation = Activation.NoEdit;
                }
                UltraWinGridUtils.SetColumns(OpenTaxlotGridColumns, grid);
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
        /// Sets the columns for position grid.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        /// <param name="childGridBand">The child grid band.</param>
        /// <param name="grd">The GRD.</param>
        /// <param name="isAccountData">if set to <c>true</c> [is account data].</param>
        private void SetColumnsForPositionGrid(UltraGridBand gridBand)
        {
            try
            {
                UltraGridColumn colAccountID = gridBand.Columns[CAPTION_AccountID];
                colAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colAccountID.Hidden = true;
                colAccountID.CellActivation = Activation.NoEdit;

                UltraGridColumn colNotionalValue = gridBand.Columns[CAPTION_NotionalValue];
                colNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colNotionalValue.Hidden = true;
                colNotionalValue.CellActivation = Activation.NoEdit;

                UltraGridColumn ColSide = gridBand.Columns[CAPTION_PositionSide];
                ColSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColClosingSide = gridBand.Columns["ClosingSide"];
                ColSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                ColClosingSide.Hidden = true;
                ColClosingSide.CellActivation = Activation.NoEdit;

                UltraGridColumn ColMultiplier = gridBand.Columns[CAPTION_Multiplier];
                ColMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColMultiplier.Hidden = true;
                ColMultiplier.CellActivation = Activation.NoEdit;

                UltraGridColumn ColAUECID = gridBand.Columns[CAPTION_AUECID];
                ColAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColAUECID.Hidden = true;
                ColAUECID.CellActivation = Activation.NoEdit;

                UltraGridColumn ColRealizedPNL = gridBand.Columns[CAPTION_RealizedPNL];
                ColRealizedPNL.Header.Caption = CAP_RealizedPNL;
                ColRealizedPNL.Format = _currencyColumnFormat;
                ColRealizedPNL.CellActivation = Activation.NoEdit;

                UltraGridColumn ColDescription = gridBand.Columns[CAPTION_Description];
                ColDescription.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                ColDescription.Hidden = true;
                ColDescription.CellActivation = Activation.NoEdit;

                UltraGridColumn colPositionID = gridBand.Columns[CAPTION_ID];
                colPositionID.Header.Caption = CAP_TaxlotId;
                colPositionID.Header.VisiblePosition = 1;
                colPositionID.CellActivation = Activation.NoEdit;

                UltraGridColumn colAccount = gridBand.Columns[CAPTION_AccountValue];
                colAccount.Header.Caption = CAP_Account;
                colAccount.CellActivation = Activation.NoEdit;
                colAccount.Header.VisiblePosition = 3;

                //Modified By : Manvendra P.
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9017

                UltraGridColumn colTradeDate = gridBand.Columns[CAPTION_TradeDatePosition];
                colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colTradeDate.Header.Caption = ClosingPrefManager.GetCaptionBasedonClosingDateType(); //CAP_TradeDate;
                colTradeDate.Header.VisiblePosition = 4;
                colTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colclosingTradeDate = gridBand.Columns[CAPTION__ClosingTradeDate];
                colclosingTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTimeWithoutDropDown;
                colclosingTradeDate.Header.Caption = CAP_CloseDt;
                colclosingTradeDate.Header.VisiblePosition = 5;
                colclosingTradeDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colPositionTag = gridBand.Columns[CAPTION_PositionalTag];
                colPositionTag.Header.Caption = CAP_PositionType;
                colPositionTag.Header.VisiblePosition = 6;
                colPositionTag.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colPositionTag.Hidden = false;
                colPositionTag.CellActivation = Activation.NoEdit;

                UltraGridColumn colClosingTag = gridBand.Columns[CAPTION_ClosingTag];
                colClosingTag.Header.VisiblePosition = 7;
                colClosingTag.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colClosingTag.Hidden = false;
                colClosingTag.CellActivation = Activation.NoEdit;

                UltraGridColumn colSymbol = gridBand.Columns[CAPTION_Symbol];
                colSymbol.Header.Caption = CAP_Symbol;
                colSymbol.Header.VisiblePosition = 8;
                colSymbol.CellActivation = Activation.NoEdit;

                UltraGridColumn colOpenAveragePrice = gridBand.Columns[CAPTION_OpenAveragePrice];
                colOpenAveragePrice.Format = _currencyColumnFormat;
                colOpenAveragePrice.Header.VisiblePosition = 9;
                colOpenAveragePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn ClosedQty = gridBand.Columns[CAPTION_ClosedQty];
                ClosedQty.Format = _currencyColumnFormat;

                UltraGridColumn NotionalChange = gridBand.Columns[CAPTION_NotionalChange];
                NotionalChange.Format = _currencyColumnFormat;

                UltraGridColumn CostBasisGrossPNL = gridBand.Columns[CAPTION_CostBasisGrossPNL];
                CostBasisGrossPNL.Format = _currencyColumnFormat;

                UltraGridColumn colClosedAveragePrice = gridBand.Columns[CAPTION_ClosedAveragePrice];
                colClosedAveragePrice.Format = _currencyColumnFormat;
                colClosedAveragePrice.Header.VisiblePosition = 9;
                colClosedAveragePrice.CellActivation = Activation.NoEdit;

                UltraGridColumn colCommision = gridBand.Columns[CAPTION_ClosedTotalCommissionandFees];
                colCommision.Format = _currencyColumnFormat;
                colCommision.Header.VisiblePosition = 10;
                colCommision.CellActivation = Activation.NoEdit;

                UltraGridColumn colPositionCommision = gridBand.Columns[CAPTION_PositionCommission];
                colPositionCommision.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colPositionCommision.Hidden = false;
                colPositionCommision.CellActivation = Activation.NoEdit;

                UltraGridColumn colSettleExpire = gridBand.Columns[CAPTION_IsExpired_Settled];
                colSettleExpire.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSettleExpire.Hidden = true;
                colSettleExpire.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategyID = gridBand.Columns[CAPTION_StrategyID];
                colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colStrategyID.Hidden = true;
                colStrategyID.CellActivation = Activation.NoEdit;

                UltraGridColumn colStrategyIDValue = gridBand.Columns[CAPTION_Strategy];
                colStrategyIDValue.Header.Caption = CAP_Strategy;
                colStrategyIDValue.CellActivation = Activation.NoEdit;

                UltraGridColumn colTimeOfSaveUTC = gridBand.Columns[CAPTION_TimeofSaveUTC];
                colTimeOfSaveUTC.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colTimeOfSaveUTC.Hidden = true;
                colTimeOfSaveUTC.CellActivation = Activation.NoEdit;

                UltraGridColumn colCurrencyID = gridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
                colCurrencyID.Hidden = true;
                colCurrencyID.CellActivation = Activation.NoEdit;

                // UltraGridColumn colClosedQty = gridBand.Columns[CAPTION_ClosedQty];
                colCurrencyID.CellActivation = Activation.NoEdit;

                gridBand.Columns[ClosingConstants.COL_AssetCategoryValue].Header.Caption = ClosingConstants.CAP_AssetCategory;

                gridBand.Columns["ClosingId"].CellActivation = Activation.NoEdit;
                gridBand.Columns["AssetCategoryValue"].CellActivation = Activation.NoEdit;
                gridBand.Columns["UnderlyingName"].CellActivation = Activation.NoEdit;
                gridBand.Columns["Exchange"].CellActivation = Activation.NoEdit;
                gridBand.Columns["ClosingMode"].CellActivation = Activation.NoEdit;
                gridBand.Columns["PositionSide"].CellActivation = Activation.NoEdit;
                gridBand.Columns["Currency"].CellActivation = Activation.NoEdit;
                gridBand.Columns["Side"].CellActivation = Activation.NoEdit;

                //added by amit 24.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3136
                if (gridBand.Columns.Exists(ClosingConstants.COL_ClosingAlgo))
                {
                    UltraGridColumn colClosingAlgo = gridBand.Columns[ClosingConstants.COL_ClosingAlgo];
                    List<EnumerationValue> closingAlgoType = EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(PostTradeEnums.CloseTradeAlogrithm));
                    ValueList closingAlgoValueList = new ValueList();
                    foreach (EnumerationValue value in closingAlgoType)
                    {
                        closingAlgoValueList.ValueListItems.Add(value.Value, value.DisplayText);
                    }
                    colClosingAlgo.ValueList = closingAlgoValueList;
                    colClosingAlgo.Header.Caption = ClosingConstants.CAP_ClosingAlgo;
                    colClosingAlgo.CellActivation = Activation.NoEdit;
                }

                for (int i = 1; i <= 45; i++)
                {
                    UltraGridColumn tradeAttributeCol = gridBand.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                    tradeAttributeCol.Hidden = true;
                    tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
                    tradeAttributeCol.CellActivation = Activation.NoEdit;
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

        private void SetGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {
            gridLayout.Appearance.BackColor = Color.Black;
            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
            gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
            gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
            gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
            gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
            gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;
            gridLayout.AutoFitStyle = AutoFitStyle.None;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            gridLayout.Override.AllowAddNew = AllowAddNew.Yes;
            gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;
            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;
        }
        void grpExpired_ExpandedStateChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (grpExpired.Expanded)
                {
                    grpUnexpired.Height -= 200;
                    grpExpired.Height += 200;
                }
                else
                {
                    grpUnexpired.Height += 200;
                    grpExpired.Height -= 200;
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

        private List<string> UnexpiredExcludedCheckBoxColumns()
        {
            List<string> unexpiredExcludedColumns = new List<string>();
            unexpiredExcludedColumns.Add(CAPTION_IsSwap);
            return unexpiredExcludedColumns;
        }

        # region check box inseration and filter
        CheckBoxOnHeader_CreationFilter headerCheckBoxUnExpired = null;
        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            grid.CreationFilter = headerCheckBox;
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1545
            // Filters are visible on blank tab            
            grid.DisplayLayout.Bands[0].Columns["checkBox"].AllowRowFiltering = DefaultableBoolean.False;
        }

        /// <summary>
        /// Get SelectedTaxLots Adjusted For ExpireCashSettle
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="closingMode"></param>
        /// <param name="cashSettleType"></param>
        /// <returns>GenericBindingList</returns>
        private GenericBindingList<TaxLot> GetSelectedTaxLotsAdjustedForExpireCashSettle(UltraGrid grid, ClosingMode closingMode, PostTradeEnums.CashSettleType cashSettleType)
        {
            GenericBindingList<TaxLot> TaxLotList = new GenericBindingList<TaxLot>();
            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        TaxLot taxlot = row.ListObject as TaxLot;
                        if (taxlot == null)
                        {
                            continue;
                        }
                        if (taxlot.SettledQty <= 0 || taxlot.SettledQty > taxlot.TaxLotQty)
                        {
                            taxlot.SettledQty = taxlot.TaxLotQty;
                        }
                        taxlot.ClosingMode = closingMode;
                        //int hashCode = taxlot.GetHashCode();
                        switch (taxlot.ClosingMode)
                        {
                            case ClosingMode.Cash:
                                break;
                            case ClosingMode.CashSettleinBaseCurrency:
                                //former FillDetailsForCashSettle() was called for each taxlot.
                                break;

                            case ClosingMode.Expire:
                                taxlot.ClosingMode = ClosingMode.Expire;
                                taxlot.AUECModifiedDate = taxlot.ExpirationDate;
                                //Set TransactionType Expire in case of trade is expired
                                taxlot.TransactionType = TradingTransactionType.Expire.ToString();

                                taxlot.TransactionSource = TransactionSource.Closing;
                                taxlot.TransactionSourceTag = (int)TransactionSource.Closing;
                                break;

                            default:
                                continue;
                        }
                        TaxLotList.Add(taxlot);
                    }
                }
                if (TaxLotList.Count > 0)
                {
                    FillDetailsForCashSettle(cashSettleType, TaxLotList);
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
            return TaxLotList;
        }

        /// <summary>
        /// Added By Faisal Shah
        /// Purpose To get Account Lock Status whether all accounts are available or not.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private bool GetAccountLockValidationStatus(UltraGridRow[] rows)
        {
            try
            {
                StringBuilder lockedAccounts = new StringBuilder();
                List<int> allAccounts = new List<int>();
                List<int> accountIds = new List<int>();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        TaxLot taxlot = row.ListObject as TaxLot;
                        if (CachedDataManager.GetInstance.isAccountLocked(taxlot.Level1ID) && !accountIds.Contains(taxlot.Level1ID))
                        {
                            accountIds.Add(taxlot.Level1ID);
                        }
                        if (!allAccounts.Contains(taxlot.Level1ID))
                        {
                            allAccounts.Add(taxlot.Level1ID);
                            if (!lockedAccounts.ToString().Contains(taxlot.Level1Name) && !accountIds.Contains(taxlot.Level1ID))
                            {
                                lockedAccounts.Append(taxlot.Level1Name + ", ");
                            }
                        }
                    }
                }
                //accountIds.Clear();
                accountIds.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                if (accountIds.Count < allAccounts.Count)
                {
                    if (MessageBox.Show("This process is subject to having account lock\nDo you want to proceed in locking account(s) " + lockedAccounts.ToString().Substring(0, lockedAccounts.ToString().Length - 2) + " ?", "Account Lock Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (AccountLockManager.SetAccountsLockStatus(allAccounts))
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Some/All accounts are currently locked by other user/users.\nPlease refer to account lock UI for more information", "Account Lock Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                    else
                        return false;
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
            return true;
        }

        private List<TaxLot> GetSelectedTaxlots(UltraGridRow[] rows)
        {
            List<TaxLot> listSelectedTaxlots = new List<TaxLot>();
            try
            {
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["checkBox"].Text == true.ToString())
                    {
                        TaxLot taxlot = row.ListObject as TaxLot;
                        if (taxlot == null)
                        {
                            continue;
                        }
                        if (taxlot.SettledQty <= 0 || taxlot.SettledQty > taxlot.TaxLotQty)
                        {
                            taxlot.SettledQty = taxlot.TaxLotQty;
                        }
                        taxlot.TaxLotClosingId = null;
                        listSelectedTaxlots.Add(taxlot);
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
            return listSelectedTaxlots;
        }

        void headerCheckBoxUnExpired__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {
                if (e.CurrentCheckState.Equals(CheckState.Checked))
                {
                    foreach (UltraGridRow row in e.Rows)
                    {
                        TaxLot allTrade = row.ListObject as TaxLot;
                        if (allTrade == null)
                        {
                            continue;
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

        void headerCheckBoxUnWind__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {

        }
        #endregion
        # endregion

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                // _securityMaster.SecMstrDataResponse += new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
            }
        }

        /// <summary>
        /// Added this function in order to get settlement dates for Swap expire and rollover for new transaction.
        /// </summary>
        /// <param name="auecID"></param>
        /// <param name="sideText"></param>
        /// <param name="tradeDate"></param>
        /// <returns></returns>
        private DateTime GetSettlementDate(int auecID, string sideText, DateTime tradeDate)
        {
            int auecSettlementPeriod = int.MinValue;
            if (sideText != "0")
            {
                string sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);
                if (auecSettlementPeriod.Equals(int.MinValue))
                {
                    auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                }
                if (auecSettlementPeriod == 0)
                {
                    return tradeDate;
                }
                else
                {
                    return BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID, true);
                }
            }
            else
                return tradeDate;
        }

        #region BackGroundWorker ExpireAndSettle
        //narendra kumar jangir 2012 Nov 16
        /// <summary>
        /// Handle Expiration, settlement, and swap expiration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bgExpireAndSettle_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                GenericBindingList<TaxLot> alist = arguments[0] as GenericBindingList<TaxLot>;
                int RowCountGrdCashandExpire = (int)arguments[1];
                if (RowCountGrdCashandExpire > 0)
                {
                    MultipleExpirationOrCashSettle(alist);//when user selects check Boxes 
                }
                else
                {
                    AllocationGroup underlyingGroup = null;
                    List<TaxLot> taxlotsToExerciseOrSettle = new List<TaxLot>();
                    List<AllocationGroup> AllocationGroupList = null;
                    UltraGridRow[] rows = arguments[3] as UltraGridRow[];
                    taxlotsToExerciseOrSettle = GetSelectedTaxlots(rows);

                    Dictionary<String, OTCPosition> positonDict = new Dictionary<string, OTCPosition>();

                    if (_isSwap)
                    {
                        TaxLot alloTrade = new TaxLot();
                        alloTrade.SwapParameters = arguments[2] as SwapParameters;
                        if (alloTrade.SwapParameters == null)
                        {
                            _isSwap = false;
                            return;
                        }
                    }
                    else
                    {
                        OTCPositionList positionsToBeSaved = arguments[4] as OTCPositionList;
                        AllocationGroupList = arguments[5] as List<AllocationGroup>;

                        //creating position dict with key taxlotId to get respective closing id for taxlot
                        if (positionsToBeSaved != null)
                        {
                            foreach (OTCPosition OTC in positionsToBeSaved)
                            {
                                if (!positonDict.ContainsKey(OTC.ExpiredTaxlotID))
                                    positonDict.Add(OTC.ExpiredTaxlotID, OTC);
                            }
                        }
                    }
                    bool isExerciseError = false;
                    bool isExerciseOrphysical = GetTaxlotsToExerciseSettle(arguments, taxlotsToExerciseOrSettle, positonDict, ref underlyingGroup, ref isExerciseError);
                    if (taxlotsToExerciseOrSettle.Count > 0 && !isExerciseError)
                    {
                        if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                        {
                            foreach(var taxlot in taxlotsToExerciseOrSettle)
                            {
                                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(taxlot.AUECModifiedDate))
                                {
                                    _isNavLockValidationFailed = true;
                                    MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                        + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }

                        bool shouldCopyTradeAttrbs = taxlotsToExerciseOrSettle[0].ClosingMode == ClosingMode.Exercise ? chkCopyOpeningTradeAttributes.Checked : true;
                        List<AllocationGroup> allocationGroupList = _allocationServices.InnerChannel.CreateandSaveGroupFromTaxlot(taxlotsToExerciseOrSettle, shouldCopyTradeAttrbs);
                        List<TaxLot> closedTrades = ClosingClientSideMapper.GetTaxlots(allocationGroupList, taxlotsToExerciseOrSettle, shouldCopyTradeAttrbs);

                        ClosingParameters closingParams = new ClosingParameters();
                        closingParams.BuyTaxLotsAndPositions = taxlotsToExerciseOrSettle;
                        closingParams.SellTaxLotsAndPositions = closedTrades;
                        closingParams.Algorithm = PostTradeEnums.CloseTradeAlogrithm.NONE;
                        closingParams.IsShortWithBuyAndBuyToCover = ClosingPrefManager.IsShortWithBuyAndBuyToCover;
                        closingParams.IsSellWithBuyToClose = ClosingPrefManager.IsSellWithBuyToClose;
                        closingParams.IsManual = false;
                        closingParams.IsDragDrop = true;
                        closingParams.IsFromServer = false;
                        closingParams.SecondarySort = PostTradeEnums.SecondarySortCriteria.None;
                        closingParams.IsVirtualClosingPopulate = false;
                        closingParams.IsOverrideWithUserClosing = false;
                        closingParams.IsMatchStrategy = true;
                        closingParams.ClosingField = PostTradeEnums.ClosingField.Default;
                        closingParams.IsCopyOpeningTradeAttributes = shouldCopyTradeAttrbs;
                        ClosingData ClosedData = _closingServices.InnerChannel.AutomaticClosingOnManualOrPresetBasis(closingParams);
                        if (ClosedData.IsNavLockFailed)
                        {
                            MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Nav Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                            return;
                        }

                        //// TODO : Ask Mukul and need good amount of testing for commenting the line.
                        //// AutomaticClosingOnManualOrPresetBasis is calling the method "SaveCloseTradesData" from server. So why we have called it again.
                        //// Due to this " Primary key violation" error is coming...
                        ////_closingServices.InnerChannel.SaveCloseTradesData(ClosedData);

                        if (!ClosedData.ErrorMsg.ToString().Equals(string.Empty))
                        {
                            MessageBox.Show(ClosedData.ErrorMsg.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                            return;
                        }
                        ClosingClientSideMapper.UpdateRepository(ClosedData);

                        if (ctrlCreateAndImportPosition1.Visible)
                        {
                            AllocationGroupList = ctrlCreateAndImportPosition1.SaveCreateTransactions();
                            ClearCreatePositionControl(isExerciseOrphysical);

                            if (AllocationGroupList.Count == 0)
                            {
                                MessageBox.Show("Error in excercise/Assignment. Please contact to admin.", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }

                        foreach (TaxLot taxlot in taxlotsToExerciseOrSettle)
                        {
                            if (isExerciseOrphysical)
                            {
                                AllocationGroup grp = null;
                                if (AllocationGroupList != null)
                                {
                                    grp = GetAllocationgroupforTaxlot(taxlot.TaxLotClosingId, AllocationGroupList);
                                }
                                else
                                {
                                    grp = underlyingGroup;
                                }
                                TaxLot closedTaxlot = GetClosedTaxlotforTaxlot(taxlot.TaxLotClosingId, closedTrades);

                                if (grp != null && closedTaxlot != null)
                                {
                                    // As taxlots will not be generated in case if check side validation fails.
                                    if (grp.TaxLots == null || grp.TaxLots.Count == 0)
                                    {
                                        MessageBox.Show("Cannot proceed Expiration/Settlement as check side validation fails.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        _closingServices.InnerChannel.UpdateExercisedTaxlotsDictionary(taxlot.TaxLotID, grp.TaxLots[0].TaxLotID, closedTaxlot.TaxLotID);
                                        UpdateAuditCollection(taxlot.TaxLotID, grp.TaxLots[0].AUECLocalDate);
                                    }
                                }
                            }
                            taxlot.CashSettledPrice = 0;
                            taxlot.SettledQty = 0;
                            taxlot.ClosingMode = ClosingMode.None;
                        }
                    }
                    _isControlInitialized = false;
                    _actionClick = true;
                }
                AuditManager.Instance.SaveAuditList(_tradeAuditCollection_Closing);
                _tradeAuditCollection_Closing.Clear();
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clear CreatePosition Control
        /// </summary>
        /// <param name="isExerciseOrphysical"></param>
        private void ClearCreatePositionControl(bool isExerciseOrphysical)
        {
            if (isExerciseOrphysical)
            {
                if (UIValidation.GetInstance().validate(ctrlCreateAndImportPosition1))
                {
                    if (ctrlCreateAndImportPosition1.InvokeRequired)
                    {
                        MethodInvoker mi = new MethodInvoker(ctrlCreateAndImportPosition1.ClearSavedRows);
                        ctrlCreateAndImportPosition1.BeginInvoke(mi);
                    }
                    else
                    {
                        ctrlCreateAndImportPosition1.ClearSavedRows();
                    }
                }
            }
        }

        private void UpdateAuditCollection(string parentTaxlotID, DateTime ExcerciseExpireDate)
        {
            try
            {
                if (_tradeAuditCollection_Closing != null && _tradeAuditCollection_Closing.Count > 0)
                {
                    for (int counter = 0; counter < _tradeAuditCollection_Closing.Count; counter++)
                    {
                        if (_tradeAuditCollection_Closing[counter].TaxLotID == parentTaxlotID)
                        {
                            _tradeAuditCollection_Closing[counter].OriginalDate = ExcerciseExpireDate;
                            break;
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
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="taxlotsToExerciseOrSettle"></param>
        /// <param name="positonDict"></param>
        /// <param name="underlyingGroup"></param>
        /// <returns></returns>
        private bool GetTaxlotsToExerciseSettle(object[] arguments, List<TaxLot> taxlotsToExerciseOrSettle, Dictionary<String, OTCPosition> positonDict, ref AllocationGroup underlyingGroup, ref bool isExerciseError)
        {
            bool isExerciseOrphysical = false;
            try
            {

                foreach (TaxLot Taxlot in taxlotsToExerciseOrSettle)
                {
                    TaxLot TaxLotClone = Taxlot.Clone() as TaxLot;

                    if (_actionClick == false)
                    {
                        switch (Taxlot.ClosingMode)
                        {
                            case ClosingMode.Exercise:
                            case ClosingMode.Physical:

                                DateTime dateExercise = DateTime.UtcNow;
                                if (dateExercise > Taxlot.ExpirationDate)
                                {
                                    dateExercise = Taxlot.ExpirationDate;
                                }

                                string taxlotID = Taxlot.TaxLotID.ToString();

                                StringBuilder alreadyClosedError = _closingServices.InnerChannel.ArePositionElligibleToExercise(taxlotID, dateExercise);
                                if (alreadyClosedError.ToString().Equals(string.Empty))
                                {
                                    //List<AllocationGroup> AllocationGroupList = ctrlCreateAndImportPosition1.SaveCreateTransactions();
                                    if (positonDict.ContainsKey(taxlotID))
                                    {
                                        OTCPosition otcPosition = positonDict[taxlotID];

                                        isExerciseOrphysical = true;

                                        Taxlot.TaxLotClosingId = otcPosition.TaxLotClosingId;
                                        //TaxLot.AUECModifiedDate = AllocationGroupList[0].AUECLocalDate;
                                        Taxlot.AUECModifiedDate = otcPosition.ProcessDate;
                                        Taxlot.IsManualyExerciseAssign = otcPosition.IsExerciseAssignManual;

                                    }

                                }
                                else
                                {
                                    MessageBox.Show(alreadyClosedError.ToString(), "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    isExerciseError = true;
                                }
                                break;

                            case ClosingMode.SwapExpire:
                                //TaxLotClone.SwapParameters = ctrlSwapClosing1.GetSelectedParams(SwapValidate.Expire);
                                TaxLotClone.SwapParameters = arguments[2] as SwapParameters;
                                Taxlot.SwapParameters = TaxLotClone.SwapParameters;
                                Taxlot.AUECModifiedDate = Taxlot.SwapParameters.ClosingDate;//TimeZoneHelper.GetAUECLocalDateFromUTC(TaxLot.AUECID, DateTime.UtcNow);
                                if (TaxLotClone.SwapParameters != null)
                                {
                                    _isSwap = false;
                                    TaxLotClone.SettledQty = TaxLotClone.TaxLotQty;
                                    Taxlot.ISSwap = false;
                                    //ctrlSwapClosing1.ResetControl();
                                    if (UIValidation.GetInstance().validate(ctrlSwapClosing1))
                                    {
                                        if (ctrlSwapClosing1.InvokeRequired)
                                        {
                                            MethodInvoker mi = new MethodInvoker(ctrlSwapClosing1.ResetControl);
                                            ctrlSwapClosing1.BeginInvoke(mi);
                                        }
                                        else
                                        {
                                            ctrlSwapClosing1.ResetControl();
                                        }
                                    }
                                }

                                break;

                            case ClosingMode.SwapExpireAndRollover:
                                isExerciseOrphysical = true;
                                //TaxLotClone.SwapParameters = ctrlSwapClosing1.GetSelectedParams(SwapValidate.Expire);
                                TaxLotClone.SwapParameters = arguments[2] as SwapParameters;
                                Taxlot.SwapParameters = TaxLotClone.SwapParameters;
                                if (TaxLotClone.SwapParameters != null)
                                {
                                    _isSwap = false;
                                    TaxLotClone.SettledQty = TaxLotClone.TaxLotQty;
                                    TaxLotClone.TaxLotClosingId = Guid.NewGuid().ToString();
                                    Taxlot.TaxLotClosingId = TaxLotClone.TaxLotClosingId;

                                    //TaxLot.AUECLocalDate = TimeZoneHelper.GetAUECLocalDateFromUTC(TaxLot.AUECID, DateTime.UtcNow);
                                    TaxLotClone.AUECLocalDate = Taxlot.SwapParameters.ClosingDate;
                                    TaxLotClone.SettlementDate = GetSettlementDate(Taxlot.AUECID, Taxlot.OrderSide, TaxLotClone.AUECLocalDate);

                                    //Buy Transaction will be generated..
                                    List<TaxLot> taxlotList = new List<TaxLot>();
                                    taxlotList.Add(TaxLotClone);
                                    List<AllocationGroup> AllocationGroups = _allocationServices.InnerChannel.CreateandSaveGroupFromTaxlot(taxlotList, true);

                                    Taxlot.ISSwap = false;
                                    if (AllocationGroups.Count > 0)
                                    {
                                        underlyingGroup = AllocationGroups[0];
                                        Taxlot.AUECModifiedDate = AllocationGroups[0].SwapParameters.ClosingDate;
                                        //ctrlSwapClosing1.ResetControl();
                                        if (UIValidation.GetInstance().validate(ctrlSwapClosing1))
                                        {
                                            if (ctrlSwapClosing1.InvokeRequired)
                                            {
                                                MethodInvoker mi = new MethodInvoker(ctrlSwapClosing1.ResetControl);
                                                ctrlSwapClosing1.BeginInvoke(mi);
                                            }
                                            else
                                            {
                                                ctrlSwapClosing1.ResetControl();
                                            }
                                        }
                                    }
                                }
                                break;

                            // now cash settlement is being handled along with expiration..
                            case ClosingMode.Cash:
                            case ClosingMode.CashSettleinBaseCurrency:
                                //return;

                                break;

                            default:
                                // return;
                                break;
                        }
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
            return isExerciseOrphysical;
        }

        private AllocationGroup GetAllocationgroupforTaxlot(string TaxLotClosingId, List<AllocationGroup> allocationGroupList)
        {
            AllocationGroup requiredGrp = null;

            try
            {
                foreach (AllocationGroup grp in allocationGroupList)
                {

                    if (grp.TaxLotClosingId.Equals(TaxLotClosingId))
                    {
                        requiredGrp = grp;
                        break;

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
            return requiredGrp;
        }

        private TaxLot GetClosedTaxlotforTaxlot(string TaxLotClosingId, List<TaxLot> ClosedTaxlots)
        {
            TaxLot requiredTaxlot = null;
            try
            {
                foreach (TaxLot taxlot in ClosedTaxlots)
                {

                    if (taxlot.TaxLotClosingId.Equals(TaxLotClosingId))
                    {
                        requiredTaxlot = taxlot;
                        break;

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
            return requiredTaxlot;
        }





        void _bgExpireAndSettle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets canceled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been canceled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (!_isNavLockValidationFailed)
                {
                    ctrlCreateAndImportPosition1.CleanCtrlCreatePosition();
                    grdCashandExpire.Visible = false;
                    ctrlSwapClosing1.Visible = false;
                    grpCreatePosition.Expanded = false;
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
            finally
            {
                //condition to check if it has atleast one listner..

                if (DisableEnableParentForm != null)
                {
                    if (_isExpirationError)
                    {
                        _statusMessage = "Error While Expiration/Settlement!!!";
                        _isExpirationError = false;
                    }
                    else
                        _statusMessage = "Data Expired/Exercised Successfully";
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, true, false));
                }

                //enable this form
                DisableEnableForm(true);

                ResumePainting();
            }
        }

        #endregion

        #region BackGroundWorker Unwind
        //narendra kumar jangir 2012 Nov 16
        void _bgUnwind_DoWork(object sender, DoWorkEventArgs e)
        {
            string result = String.Empty;
            try
            {
                object[] arguments = e.Argument as object[];
                UltraGridRow[] filteredRows = arguments[0] as UltraGridRow[];
                if (filteredRows != null && filteredRows.Length > 0)
                {
                    result = Unexpire(filteredRows);
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            e.Result = result;
        }
        void _bgUnwind_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (e.Result != null && e.Error == null)
                {
                    if (myerror != null)
                    {
                        myerror.Close();
                    }
                    myerror = new ErrorTextBox();
                    if (e.Result.ToString().Contains("Underlying"))
                    {
                        myerror.SetUp("First unwind the underlying to continue.", e.Result.ToString());
                    }
                    else
                    {
                        myerror.SetUp("There are some closing trade in future date. Unwind them first.", e.Result.ToString());
                    }
                    myerror.ShowDialog();
                    myerror.BringToFront();
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
            finally
            {
                //condition to check if it has atleast one listner..
                if (DisableEnableParentForm != null)
                {
                    DisableEnableParentForm(this, new EventArgs<string, bool, bool>(_statusMessage, true, false));
                }

                //enable this form
                DisableEnableForm(true);

                ResumePainting();


            }
        }
        #endregion

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region ILiveFeedCallback Members

        void ILiveFeedCallback.SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void ILiveFeedCallback.LiveFeedConnected()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void ILiveFeedCallback.LiveFeedDisConnected()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
        //this method disables or enables control based on bool value, false=>disable, true=>enable
        private void DisableEnableForm(bool Flag)
        {
            btnExpireSettle.Enabled = Flag;
            btnCancel.Enabled = Flag;
        }
        public void ResumePainting()
        {
            try
            {
                this.grdAccountExpired.Enabled = true;
                this.grdAccountExpired.ResumeRowSynchronization();
                this.grdAccountExpired.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdAccountExpired);
                //this.grdAccountExpired.EndUpdate();

                this.grdAccountUnexpired.Enabled = true;
                this.grdAccountUnexpired.ResumeRowSynchronization();
                this.grdAccountUnexpired.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdAccountUnexpired);
                //this.grdAccountUnexpired.EndUpdate();

                this.grdCashandExpire.Enabled = true;
                this.grdCashandExpire.ResumeRowSynchronization();
                this.grdCashandExpire.EndUpdate();
                Prana.ClientCommon.SafeNativeMethods.ControlDrawing.ResumeDrawing(grdCashandExpire);
                //this.grdCashandExpire.EndUpdate();
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

        void grdAccountUnexpired_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                object val = e.Cell.Value;
                if (val.GetType().Equals(typeof(bool)))
                {
                    bool flag = (bool)val;
                    e.Cell.Value = (flag ? false : true);
                }
                else
                {

                    string caption = e.Cell.Column.Header.Caption;
                    if (caption.ToString().Equals(CAP_SettledQty))
                    {
                        grdAccountUnexpired.PerformAction(UltraGridAction.EnterEditMode, false, false);
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

        void grdCashandExpire_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                string caption = e.Cell.Column.Header.Caption;
                int assetId = Convert.ToInt32(e.Cell.Row.Cells["AssetCategoryValue"].Value);
                if (caption.ToString().Equals(CAP_CloseDt) || ((assetId == (int)AssetCategory.FX || assetId == (int)AssetCategory.FXForward) && (caption.ToString().Equals("FXRate") || caption.ToString().Equals("CashSettledPrice"))))
                    grdCashandExpire.PerformAction(UltraGridAction.EnterEditMode, false, false);
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
        /// Added By faisal Shah
        /// Saves the Layout of AccountUnExpired Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLayoutMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (grdAccountUnexpired != null && !String.IsNullOrEmpty(_expirySettlementNewLayoutFilePath))
                    grdAccountUnexpired.DisplayLayout.SaveAsXml(_expirySettlementNewLayoutFilePath, PropertyCategories.All);
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
        /// Sets _expirySettlementNewLayoutFilePath
        /// </summary>
        private void SetSavedLayoutPath()
        {
            try
            {
                _userIDforLayout = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _expirySettlementNewLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userIDforLayout;
                _expirySettlementNewLayoutFilePath = _expirySettlementNewLayoutDirectoryPath + @"\ExpirySettlementLayout_New.xml";
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
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
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
                    colData.Format = gridCol.Format;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;
                    colData.CellActivation = gridCol.CellActivation;

                    // Sorted Columns
                    colData.SortIndicator = gridCol.SortIndicator;

                    //// Summary Settings
                    //if (band.Summaries.Exists(gridCol.Key))
                    //{
                    //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
                    //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
                    //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
                    //}

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        FilterCondition filterCondClone = new FilterCondition(fCond.ComparisionOperator, fCond.CompareValue);
                        if (((gridCol.Key.Equals(ClosingConstants.COL_TradeDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (gridCol.Key.Equals(ClosingConstants.COL_ProcessDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing))) || (gridCol.Key.Equals(ClosingConstants.COL_ExpiryDate) && filterCondClone.CompareValue.Equals(DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing)))) && filterCondClone.ComparisionOperator == FilterComparisionOperator.StartsWith)
                        {
                            filterCondClone.CompareValue = "(Today)";
                        }
                        colData.FilterConditionList.Add(filterCondClone);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

                    listGridCols.Add(colData);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }

        /// <summary>
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        public void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();

            try
            {

                // Hide Grid columns  if not present in layout, otherwise Column set according to Layout
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    ColumnData colData = listColData.FirstOrDefault(col => col.Key.Equals(gridCol.Key) && col.Hidden == false);


                    if (colData != null)   // If column present in layout
                    {
                        //Set Columns Properties
                        gridCol.Width = colData.Width;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;

                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }

                        //Summary Settings
                        //if (colData.ColSummaryKey != String.Empty)
                        //{
                        //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
                        //    summary.DisplayFormat = colData.ColSummaryFormat;
                        //}

                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                if ((colData.Key.Equals(ClosingConstants.COL_TradeDate) || colData.Key.Equals(ClosingConstants.COL_ProcessDate) || colData.Key.Equals(ClosingConstants.COL_ExpiryDate)) && colData.FilterConditionList.Count == 1 && colData.FilterConditionList[0].ComparisionOperator == FilterComparisionOperator.StartsWith && colData.FilterConditionList[0].CompareValue.Equals("(Today)"))
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.ToString(DateTimeConstants.DateformatForClosing));
                                }
                                else
                                {
                                    band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                                }
                            }
                        }
                    }

                    else
                    {
                        gridCol.Hidden = true;
                    }

                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            // Sorted Columns are returned as they need to be handled after data is binded.
            //  return listSortedGridCols;
        }

        /// <summary>
        /// Added By faisal Shah
        /// Function allows to Set Formatting of Columns after loading preferences from XML
        /// </summary>
        private void SetColumnFormatting()
        {
            try
            {
                ColumnsCollection columns = grdAccountUnexpired.DisplayLayout.Bands[0].Columns;
                columns[CAPTION_OpenQuantity].Format = _currencyColumnFormat;
                columns[CAPTION_SettledQty].Format = _currencyColumnFormat;
                columns[CAPTION_AveragePrice].Format = _currencyColumnFormat;
                columns[CAPTION_UnitCost].Format = _currencyColumnFormat;
                columns[CAPTION_OpenCommission].Format = _currencyColumnFormat;
                columns[CAPTION_NetNotionalValue].Format = _currencyColumnFormat;
                columns[COLUMN_StrikePrice].Format = _currencyColumnFormat;
                columns[CAPTION_UnitCostBase].Format = _currencyColumnFormat;
                columns[CAPTION_ExecutedQuantity].Format = _currencyColumnFormat;
                columns[CAPTION_UnrealizedPNL].Format = _currencyColumnFormat;
                columns[CAPTION_AvgPriceBase].Format = _currencyColumnFormat;
                columns[CAPTION_CumQty].Format = _currencyColumnFormat;
                columns[CAPTION_FXRate].Format = _currencyColumnFormat;
                columns[CAPTION_QUANTITY].Format = _currencyColumnFormat;
                columns[CAPTION_PERCENTOFITMOTM].Format = _currencyColumnFormat;
                columns[CAPTION_INTRINSICVALUE].Format = _currencyColumnFormat;
                columns[CAPTION_GAINLOSSIFEXERCISEASSIGN].Format = _currencyColumnFormat;
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
        /// Added By faisal Shah
        /// Function allows overwriting the prefernce properties . Here we are setting Captions in present Case
        /// </summary>
        /// <param name="grdPosition"></param>
        private void SetColumnCustomizations(UltraGrid grdPosition)
        {
            try
            {

                UltraGridBand band = grdPosition.DisplayLayout.Bands[0];
                band.Columns[CAPTION_TradeDate].Header.Caption = CAP_TradeDate;
                band.Columns[CAPTION_ProcessDate].Header.Caption = CAP_ProcessDate;
                band.Columns[CAPTION_OriginalPurchaseDate].Header.Caption = CAP_OriginalPurchaseDate;
                band.Columns[CAPTION_Side].Header.Caption = CAP_Side;
                band.Columns[CAPTION_Symbol].Header.Caption = CAP_Symbol;
                band.Columns[CAPTION_OpenQuantity].Header.Caption = "Quantity";
                band.Columns[CAPTION_SettledQty].Header.Caption = CAP_SettledQty;
                band.Columns[CAPTION_AveragePrice].Header.Caption = CAP_AvgPrice;
                band.Columns[CAPTION_OpenCommission].Header.Caption = CAP_Commission;
                band.Columns[CAPTION_NetNotionalValue].Header.Caption = CAP_NetNotional;
                band.Columns[CAPTION_Account].Header.Caption = CAP_Account;
                band.Columns[CAPTION_IsSwap].Header.Caption = CAPTION_IsSwapped;
                band.Columns[COLUMN_StrikePrice].Header.Caption = CAPTION_StrikePrice;
                band.Columns[CAPTION_ClosingMode].Header.Caption = CAP_SettlementMode;
                band.Columns[CAPTION_ExpiryDate].Header.Caption = CAPTION_ExpiryDate;
                band.Columns[CAPTION_AssetCategoryValue].Header.Caption = ClosingConstants.CAP_AssetCategory;
                band.Columns[CAPTION_StrategyValue].Header.Caption = CAP_Strategy;
                band.Columns[CAPTION_UnderLying].Header.Caption = CAPTION_UnderLying;
                band.Columns[CAPTION_SecurityFullName].Header.Caption = CAP_SecurityFullName;
                band.Columns[CAPTION_PositionTag].Header.Caption = CAPTION_PositionTag;
                band.Columns[CAPTION_ITMOTM].Header.Caption = CAP_ItmOtm;
                band.Columns[CAPTION_PERCENTOFITMOTM].Header.Caption = CAP_PercentOfITMOTM;
                band.Columns[CAPTION_INTRINSICVALUE].Header.Caption = CAP_IntrinsicValue;
                band.Columns[CAPTION_DAYSTOEXPIRY].Header.Caption = CAP_DaysToExpiry;
                band.Columns[CAPTION_GAINLOSSIFEXERCISEASSIGN].Header.Caption = CAP_GainLossIfExerciseAssign;
                band.Columns[COLUMN_FACTSETSYMBOL].Header.Caption = CAP_FACTSETSYMBOL;
                band.Columns[COLUMN_ACTIVSYMBOL].Header.Caption = CAP_ACTIVSYMBOL;


                UltraGridColumn colAssetDerivative = band.Columns[ClosingConstants.COL_AssetDerivative];
                colAssetDerivative.Header.Caption = ClosingConstants.CAPTION_AssetDerivative;


                if (band.Columns.Exists("CounterPartyName"))
                {
                    band.Columns["CounterPartyName"].Header.Caption = ApplicationConstants.CONST_BROKER;
                }
                if (band.Columns.Exists(COLUMN_TradeAttribute1))
                {
                    for (int i = 1; i <= 45; i++)
                    {
                        band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i].Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
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

        internal void SetControlsAsReadOnly()
        {
            try
            {
                grdCashandExpire.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                grdAccountExpired.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                grdAccountUnexpired.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                btnCancel.Enabled = false;
                btnExpireSettle.Enabled = false;
                ultraExpandableGroupBoxPanel1.Enabled = false;
                grpUnexpired.Enabled = false;
                ultraExpandableGroupBoxPanel2.Enabled = false;
                grdAccountUnexpired.Enabled = false;
                grpExpired.Enabled = false;
                grpCreatePosition.Enabled = false;
                foreach (MenuItem item in menuUnexpired.MenuItems)
                {
                    item.Enabled = false;
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

        private void grdAccountUnexpired_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAccountUnexpired);
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

        #region  code for custom column chooser

        /// <summary>
        /// Apply custom column chooser on grdCashandExpire grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdCashandExpire_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdCashandExpire);
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

        #endregion

        private void grdAccountExpired_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdAccountExpired);
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

        private void grdAccountExpired_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdAccountUnexpired_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdCashandExpire_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// Adds entry to the Audit List for the Expiration/Settlement of FX from Closing UI
        /// </summary>
        /// <param name="taxlot">Not Null, Generic Binding List from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddCashSettlementAuditEntry(GenericBindingList<TaxLot> taxlot, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (taxlot != null && comment != null)
                {
                    for (int i = 0; i < taxlot.Count; i++)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = taxlot[i].Symbol;
                        newEntry.Level1ID = taxlot[i].Level1ID;
                        newEntry.GroupID = taxlot[i].GroupID;
                        newEntry.TaxLotClosingId = "";
                        newEntry.TaxLotID = taxlot[i].TaxLotID;
                        newEntry.OrderSideTagValue = taxlot[i].OrderSideTagValue;
                        newEntry.OriginalValue = "";
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.OriginalDate = taxlot[i].AUECLocalDate;
                        newEntry.Level1AllocationID = taxlot[i].Level1AllocationID;
                        newEntry.Source = TradeAuditActionType.ActionSource.Closing;
                        _tradeAuditCollection_Closing.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Generic Binding List Data to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Adds entry to the Audit List for the Expiration/Settlement of Option from Closing UI
        /// </summary>
        /// <param name="taxlot">Not Null, Generic Binding List from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddOptionExpirationorSettlementAuditEntry(List<TaxLot> taxlot, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (taxlot != null && comment != null)
                {
                    for (int i = 0; i < taxlot.Count; i++)
                    {
                        TradeAuditEntry newEntry = new TradeAuditEntry();
                        newEntry.Action = action;
                        newEntry.Comment = comment;
                        newEntry.CompanyUserId = currentUserID;
                        newEntry.Symbol = taxlot[i].Symbol;
                        newEntry.Level1ID = taxlot[i].Level1ID;
                        newEntry.GroupID = taxlot[i].GroupID;
                        newEntry.TaxLotClosingId = "";
                        newEntry.TaxLotID = taxlot[i].TaxLotID;
                        newEntry.OrderSideTagValue = taxlot[i].OrderSideTagValue;
                        newEntry.OriginalValue = "OPEN";
                        newEntry.NewValue = "CLOSED";
                        newEntry.AUECLocalDate = DateTime.Now;
                        newEntry.OriginalDate = taxlot[i].AUECLocalDate;
                        newEntry.Level1AllocationID = taxlot[i].Level1AllocationID;
                        newEntry.Source = TradeAuditActionType.ActionSource.Closing;
                        _tradeAuditCollection_Closing.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Taxlot List Data to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Adds entry to the Audit List for the Un-Winding from Closing UI
        /// </summary>
        /// <param name="taxlot">Not Null, Generic Binding List from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddUnwindingAuditEntry(Position pos, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            TradeAuditEntry newEntry = new TradeAuditEntry();
            try
            {
                if (pos != null && comment != null)
                {
                    newEntry.Action = action;
                    newEntry.Comment = comment;
                    newEntry.CompanyUserId = currentUserID;
                    newEntry.Symbol = pos.Symbol;
                    newEntry.Level1ID = pos.AccountValue.ID;
                    newEntry.TaxLotClosingId = pos.TaxLotClosingId;
                    newEntry.TaxLotID = pos.ID;
                    newEntry.GroupID = pos.ID.Substring(0, 13);
                    newEntry.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(pos.Side);
                    newEntry.OriginalValue = "CLOSED";
                    newEntry.NewValue = "OPEN";
                    newEntry.AUECLocalDate = DateTime.Now;
                    newEntry.OriginalDate = pos.ClosingTradeDate;
                    newEntry.Level1AllocationID = pos.ID.Substring(0, 13) + pos.AccountValue.ID;
                    newEntry.Source = TradeAuditActionType.ActionSource.Closing;
                    _tradeAuditCollection_Closing.Add(newEntry);
                }
                else
                    throw new NullReferenceException("The Data to add in audit dictionary is null");
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
            return true;
        }
        /// <summary>
        /// This method is use to clear all the filters from net postion grid of closing UI.
        /// </summary>
        public void ClearAccountExpiredGridFiltersStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdAccountExpired.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
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
        /// This method is use to clear all the filters from net postion grid of closing UI.
        /// </summary>
        public void ClearAccountUnexpiredGridFiltersStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdAccountUnexpired.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                SetDefaultFiltersGrdAccountUnexpired();
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

        public void ExportDataForAutomation(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "grdAccountUnexpired")
                {
                    exporter.Export(grdAccountUnexpired, filePath);
                }
                else if (gridName == "grdAccountExpired")
                {
                    exporter.Export(grdAccountExpired, filePath);
                }
                else if (gridName == "grdCreatePosition")
                {
                    this.ctrlCreateAndImportPosition1.ExportDataForAutomation(gridName, filePath);
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

    }
}
