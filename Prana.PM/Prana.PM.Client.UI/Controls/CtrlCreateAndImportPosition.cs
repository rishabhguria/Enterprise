using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinSchedule;
using Prana.BusinessLogic;
using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PostTrade.BusinessObjects;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlCreateAndImportPosition : UserControl
    {

        private CreatePositionUserInterface _createPositionUserInterface = new CreatePositionUserInterface();

        UltraGridBand _gridBandOTCPositions = null;
        bool _isOTCPositionsGridInitialized = false;
        private bool _isCloseTradePopUp;

        ProxyBase<IClosingServices> _closingServices = null;
        public ProxyBase<IClosingServices> ClosingServices
        {
            set { _closingServices = value; }

        }
        ProxyBase<ICashManagementService> _cashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            set { _cashManagementServices = value; }

        }

        /// <summary>
        /// Allocation manager proxy(new).
        /// </summary>
        ProxyBase<IAllocationManager> _allocationProxy = null;

        /// <summary>
        /// Allocation manager proxy(new).
        /// </summary>
        public ProxyBase<IAllocationManager> AllocationProxy
        {
            set { _allocationProxy = value; }

        }

        int _pMTradingAccountID = int.MinValue;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is close trade popup.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is close trade popup; otherwise, <c>false</c>.
        /// </value>
        public bool IsCloseTradePopup
        {
            get { return _isCloseTradePopUp; }
            set { _isCloseTradePopUp = value; }
        }
        int _userID;
        int _companyID;
        int _companyBaseCurrencyID;

        #region Grid Columns for OTC Positions
        const string CAPTION_ID = "ID";
        const string CAPTION_Side = "Side";
        const string CAPTION_PositionStartQuantity = "PositionStartQuantity";
        const string CAPTION_ClosingQuantity = "ClosedQty";
        const string CAPTION_OpenQuantity = "OpenQty";
        const string CAPTION_StartDate = "StartDate";
        const string CAPTION_AUECLocalDateToday = "AUECLocalDateToday";
        const string CAPTION_LastActivityDate = "LastActivityDate";
        const string CAPTION_PositionType = "IntPositionType";
        const string CAPTION_ValuePositionType = "PositionType";
        const string CAPTION_Quantity = "Quantity";
        const string CAPTION_AccountValue = "AccountValue";
        const string CAPTION_Strategy = "Strategy";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_AveragePrice = "AveragePrice";
        const string CAPTION_StartTaxLotID = "StartTaxLotID";
        const string CAPTION_SymbolConvention = "SymbolConvention";
        const string CAPTION_Description = "Description";
        const string CAPTION_CounterPartyID = "CounterPartyID";
        const string CAPTION_VenueID = "VenueID";
        const string CAPTION_Multiplier = "Multiplier";
        const string CAPTION_Fees = "Fees";
        const string CAPTION_InstrumentType = "IntInstrumentType";
        const string CAPTION_AssetID = "AssetID";
        const string CAPTION_ExchangeID = "ExchangeID";
        const string CAPTION_UnderlyingID = "UnderlyingID";
        const string CAPTION_SymbolConventionID = "SymbolConventionID";
        const string CAPTION_AccountID = "AccountID";
        const string CAPTION_StrategyID = "StrategyID";
        const string CAPTION_AUECID = "AUECID";
        const string CAPTION_MarkPriceForMonth = "MarkPriceForMonth";
        const string CAPTION_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
        const string CAPTION_ExpiredTaxlotID = "ExpiredTaxlotID";
        const string CAPTION_ExpirationQty = "ExpiredQty";
        const string CAPTION_RealizedPNL = "CostBasisRealizedPNL";
        const string CAPTION_EndDate = "EndDate";
        const string CAPTION_Status = "Status";
        const string CAPTION_RecordType = "RecordType";
        const string CAPTION_NotionalValue = "NotionalValue";
        const string CAPTION_UnRealizedPNL = "UnRealizedPNL";
        const string CAPTION_AvgPriceRealizedPL = "AvgPriceRealizedPL";
        const string CAPTION_SymbolAveragePrice = "SymbolAveragePrice";
        const string CAPTION_PayReceive = "PayReceive";
        const string CAPTION_SettlementDate = "SettlementDate";
        const string CAPTION_SideTypeID = "SideTagValue";
        const string CAPTION_Currency = "Currency";
        const string COLUMN_CurrencyID = "CurrencyID";
        const string CAPTION_PositionTaxLots = "PositionTaxLots";
        const string CAPTION_ForexConversion = "ForexConversion";
        const string COLUMN_ExpirationDate = "ExpirationDate";
        const string CAPTION_ExpirationDate = "Expiration Date";
        const string CAPTION_FXRate = "FX Rate";
        const string COLUMN_FXRate = "FXRate";
        const string CAPTION_FxRateCalc = "FX Rate Operator";
        const string COLUMN_FxRateCalc = "FXConversionMethodOperator";
        const string CAPTION_IfPayReceiveChanges = "IfPayReceiveChanges";
        const string CAPTION_LeadCurrencyID = "LeadCurrencyID";
        const string COL_COMMISSIONSOURCE = "CommissionSource";
        const string CAPTION_COMMISSIONSOURCE = "Commission Source";
        const string COL_SOFTCOMMISSIONSOURCE = "SoftCommissionSource";
        const string CAPTION_SOFTCOMMISSIONSOURCE = "Soft Commission Source";
        const string COLUMN_TransactionType = "TransactionType";
        const string CAPTION_TransactionType = "Transaction Type";
        const string COLUMN_IsOptionActivated = "IsOptionActivated";
        const string CAPTION_IsOptionActivated = "Option Activated";
        const string COLUMN_OptionType = "OptionType";
        const string CAPTION_OptionType = "Option Type";
        const string COLUMN_UnderlyingSymbol = "UnderlyingSymbol";
        const string CAPTION_UnderlyingSymbol = "Underlying Symbol";
        const string COLUMN_StrikePrice = "StrikePrice";
        const string CAPTION_StrikePrice = "Strike Price";
        const string COLUMN_TransactionSource = "TransactionSource";
        #endregion

        public CtrlCreateAndImportPosition()
        {
            InitializeComponent();
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                CreateAllocationProxy();

            }
        }


        /// <summary>
        /// Allcoation proxy(new) for Create transaction trades.
        /// </summary>
        private void CreateAllocationProxy()
        {
            try
            {
                _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");

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

        List<OTCPosition> _otcPosList = new List<OTCPosition>();
        public void PhysicalSettlement(OTCPosition otcPos, TaxLot allocatedTrade, bool isCurrentDayClosing, DateTime dateExercise)
        {
            try
            {
                AddNewRow(otcPos, 0);
                UpdateOtcPos(allocatedTrade, otcPos);

                double currentPositionUnderlying = 0;
                if (isCurrentDayClosing)
                {
                    currentPositionUnderlying = ClosingClientSideMapper.GetSymbolAccountPositionForGivenDate(allocatedTrade.UnderlyingSymbol, allocatedTrade.Level1ID, dateExercise);
                }
                string transactionType = TradingTransactionType.Trade.ToString().ToUpper();
                //Populate TransactionType based on Long or Short Option
                otcPos.SideTagValue = _closingServices.InnerChannel.GetOrderSideTagValueForPhysicalSettlement(allocatedTrade.AssetCategoryValue, allocatedTrade.OrderSideTagValue, currentPositionUnderlying, allocatedTrade.PutOrCall, dateExercise, allocatedTrade.UnderlyingSymbol, allocatedTrade.Level1ID.ToString(), isCurrentDayClosing, ref transactionType);
                otcPos.TransactionType = transactionType;
                allocatedTrade.TransactionType = transactionType;
                SetupGridRowForOtcPos(allocatedTrade, otcPos);
                _otcPosList.Add(otcPos);

                //grdCreatePosition.AfterCellUpdate += new CellEventHandler(grdCreatePosition_AfterCellUpdate);
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

        public bool ExerciseAssignTaxlots(List<TaxLot> TaxLots, bool isCopyTradeAttrbsPrefUsed)
        {
            try
            {
                _otcPosList.Clear();
                isExerciseAssignManualStatus.Clear();
                IEnumerable<IGrouping<dynamic, TaxLot>> groupedTaxlots = TaxLots.GroupBy(x => new { x.Level1ID, x.UnderlyingSymbol, x.AssetCategoryValue });
                foreach (var group in groupedTaxlots)
                {
                    Dictionary<string, Dictionary<string, PhysicalSettlementDto>> dictTaxlotSideTagValueQty = null;
                    if (ClosingPrefManager.GetPreferences().ClosingMethodology.SplitunderlyingBasedOnPosition)
                    {
                        grdCreatePosition.ContextMenuStrip = mnuExerciseAssign;
                        dictTaxlotSideTagValueQty = _closingServices.InnerChannel.GetOrderSideTagValueForPhysicalSettlementGroup(group.ToList(), false);
                    }
                    foreach (TaxLot TaxLot in group)
                    {
                        if (!TaxLot.UnderlyingSymbol.Equals(string.Empty))
                        {

                            if (TaxLot.SettledQty == 0)
                            {
                                TaxLot.SettledQty = TaxLot.TaxLotQty;
                            }
                            OTCPosition otcPos = new OTCPosition();
                            otcPos.AUECLocalDateToday = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(TaxLot.AUECID));
                            otcPos.ExpiredTaxlotID = TaxLot.TaxLotID;
                            otcPos.ExpiredQty = TaxLot.SettledQty;
                            otcPos.Strategy = TaxLot.Level2Name;
                            otcPos.StrategyID = TaxLot.Level2ID;
                            otcPos.ParentTradeDate = TaxLot.OriginalPurchaseDate;

                            DateTime dateExercise = DateTime.UtcNow;
                            if (dateExercise > TaxLot.ExpirationDate)
                            {
                                dateExercise = TaxLot.ExpirationDate;
                            }

                            otcPos.IsOptionActivated = false;

                            if (dateExercise.Date < otcPos.ParentTradeDate.Date)
                            {
                                otcPos.StartDate = otcPos.ParentTradeDate;
                            }
                            else
                            {
                                otcPos.StartDate = dateExercise;
                            }
                            TaxLot.AUECModifiedDate = otcPos.StartDate;
                            if (TaxLot.Symbol.Equals(TaxLot.UnderlyingSymbol))
                            {
                                DialogResult result = MessageBox.Show("The underlying symbol is same as the security being exercised/settled. This will lead to circular reference. Are you sure you want to proceed? ", "Circular Reference Issue", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result.ToString().Equals(DialogResult.No.ToString()))
                                {
                                    return false;
                                }
                            }
                            otcPos.CounterPartyID = TaxLot.CounterPartyID;
                            otcPos.SettlementCurrencyID = TaxLot.SettlementCurrencyID;
                            string transactionType = TradingTransactionType.Trade.ToString().ToUpper();
                            //Assigning TaxLotClosingId so that it can be same for splitted taxlots.
                            otcPos.TaxLotClosingId = Guid.NewGuid().ToString();

                            otcPos.AccountValue.ID = TaxLot.Level1ID;
                            otcPos.AccountValue.ShortName = TaxLot.Level1Name;
                            otcPos.AccountValue.FullName = TaxLot.Level1Name;
                            otcPos.ExpiredTaxlotID = TaxLot.TaxLotID;
                            otcPos.SettlementCurrencyID = TaxLot.SettlementCurrencyID;
                            //as we are using TaxLot.ContractMultiplier in calculations so commenting this,PRANA-32815
                            //otcPos.Multiplier = TaxLot.ContractMultiplier;

                            if (TaxLot.ClosingMode != ClosingMode.Exercise || isCopyTradeAttrbsPrefUsed)
                            {
                                otcPos.TradeAttribute1 = TaxLot.TradeAttribute1;
                                otcPos.TradeAttribute2 = TaxLot.TradeAttribute2;
                                otcPos.TradeAttribute3 = TaxLot.TradeAttribute3;
                                otcPos.TradeAttribute4 = TaxLot.TradeAttribute4;
                                otcPos.TradeAttribute5 = TaxLot.TradeAttribute5;
                                otcPos.TradeAttribute6 = TaxLot.TradeAttribute6;
                                otcPos.SetTradeAttribute(TaxLot.GetTradeAttributesAsDict()); 
                            }

                            otcPos.Strategy = TaxLot.Level2Name;
                            otcPos.StrategyID = TaxLot.Level2ID;
                            otcPos.Symbol = TaxLot.UnderlyingSymbol;
                            if (ClosingPrefManager.GetPreferences().ClosingMethodology.SplitunderlyingBasedOnPosition)
                            {
                                if (dictTaxlotSideTagValueQty != null && dictTaxlotSideTagValueQty.ContainsKey(TaxLot.TaxLotID))
                                {
                                    Dictionary<string, PhysicalSettlementDto> dictSideTagValueQty = dictTaxlotSideTagValueQty[TaxLot.TaxLotID];
                                    PhysicalSettlementNew(otcPos, TaxLot, ClosingPrefManager.IsCurrentDateClosing, dateExercise, dictSideTagValueQty);
                                }
                            }
                            else
                            {
                                PhysicalSettlement(otcPos, TaxLot, ClosingPrefManager.IsCurrentDateClosing, dateExercise);
                            }
                            CalculatePayReceive();
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
                return false;
            }
            return true;
        }

        private void PhysicalSettlementNew(OTCPosition otcPos, TaxLot allocatedTrade, bool isCurrentDayClosing, DateTime dateExercise, Dictionary<string, PhysicalSettlementDto> dictSideTagValueQty)
        {
            try
            {
                List<OTCPosition> otcPosList = new List<OTCPosition>();
                int row = 0;
                foreach (KeyValuePair<string, PhysicalSettlementDto> sideTagAndQty in dictSideTagValueQty)
                {
                    OTCPosition otcCpy = otcPos.Clone();
                    otcPosList.Add(otcCpy);
                    otcPos.PositionStartQuantity = sideTagAndQty.Value.Quantity;
                    AddNewRow(otcCpy, row);
                    otcPos.ExpiredQty = sideTagAndQty.Value.Quantity;
                    //Modified By faisal Shah
                    //Needed to ommit Multiplier in case of FutureOptions
                    otcPos.SideTagValue = sideTagAndQty.Key;

                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(allocatedTrade.AssetCategoryValue);
                    switch (baseAssetCategory)
                    {
                        case AssetCategory.Option:
                            otcPos.AveragePrice = allocatedTrade.StrikePrice;
                            if (!allocatedTrade.IsExerciseAtZero)
                            {
                                switch (allocatedTrade.OrderSideTagValue)
                                {
                                    case FIXConstants.SIDE_Buy:
                                    case FIXConstants.SIDE_Buy_Open:
                                    case FIXConstants.SIDE_Buy_Cover:
                                    case FIXConstants.SIDE_Buy_Closed:
                                        otcPos.OptionPremiumAdjustment = (allocatedTrade.AvgPrice * sideTagAndQty.Value.Quantity) + (allocatedTrade.OpenTotalCommissionandFees * (sideTagAndQty.Value.Quantity / allocatedTrade.ContractMultiplier) / allocatedTrade.TaxLotQty);
                                        otcPos.MiscFees = 0;
                                        break;
                                    default:
                                        otcPos.OptionPremiumAdjustment = (-1) * (allocatedTrade.AvgPrice * sideTagAndQty.Value.Quantity) + (allocatedTrade.OpenTotalCommissionandFees * (sideTagAndQty.Value.Quantity / allocatedTrade.ContractMultiplier) / allocatedTrade.TaxLotQty);
                                        otcPos.MiscFees = 0;
                                        break;
                                }
                                otcCpy.OptionPremiumAdjustmentUnit = otcPos.OptionPremiumAdjustment / sideTagAndQty.Value.Quantity;
                            }
                            break;
                        default:
                            otcPos.AveragePrice = allocatedTrade.AvgPrice;
                            break;
                    }

                    //Populate TransactionType based on Long or Short Option
                    otcPos.TransactionType = sideTagAndQty.Value.TransactionType;
                    allocatedTrade.TransactionType = sideTagAndQty.Value.TransactionType;

                    otcPos.TransactionSource = TransactionSource.Closing;
                    SetupGridRowForOtcPos(allocatedTrade, otcPos);
                    row++;
                }
                _otcPosList.AddRange(otcPosList);
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
        /// Handles the Click event of the deleteSymbolBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DuplicateRowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdCreatePosition.ActiveRow != null)
                {
                    OTCPosition otcPos = (OTCPosition)grdCreatePosition.ActiveRow.ListObject;
                    if (_otcPosList.Count(x => x.ExpiredTaxlotID.Equals(otcPos.ExpiredTaxlotID)) == 1)
                    {
                        OTCPosition otcCpy = otcPos.Clone();

                        AddNewRow(otcCpy, _otcPosList.Count);
                        SetupGridRowForOtcPos(null, otcPos);
                        _otcPosList.Add(otcCpy);
                    }
                    else
                    {
                        MessageBox.Show("The parent option for this taxlot has already been split in multiple rows.", "Unable to split");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to duplicate.", "No row selected");
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
        /// Handles the InitializeLayout event of the grdCreatePosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdCreatePosition_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (bool.Equals(_isOTCPositionsGridInitialized, false))
                {
                    UltraGridLayout gridLayout = grdCreatePosition.DisplayLayout;

                    //--------------Add row by default feature commented on 14th March.-----------
                    //// Add-Row Feature Related Settings
                    //// --------------------------------------------------------------------------------
                    //// To enable the add-row functionality set the AllowAddNew. 
                    //e.Layout.Override.AllowAddNew = AllowAddNew.FixedAddRowOnTop;

                    //// Set the appearance for template add-rows. 
                    //e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.Black;
                    //e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;

                    //// Once  the user modifies the contents of a template add-row, it becomes
                    //// an add-row and the AddRowAppearance gets applied to such rows.            
                    //e.Layout.Override.AddRowAppearance.BackColor = Color.Black;
                    //e.Layout.Override.AddRowAppearance.ForeColor = Color.White;

                    //// set the SpecialRowSeparator to a value with TemplateAddRow flag
                    //// turned on to display a separator ui element after the add-row.            
                    //e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.TemplateAddRow;
                    //e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = SystemColors.Control;

                    //// display a prompt in the add-row by setting the TemplateAddRowPrompt 
                    //// proeprty. By default UltraGrid does not display any add-row prompt.            
                    //e.Layout.Override.TemplateAddRowPrompt = "Click here to add a new record...";

                    //// You can control the appearance of the prompt using the Override's
                    //// TemplateAddRowPromptAppearance property. 
                    //e.Layout.Override.TemplateAddRowPromptAppearance.ForeColor = Color.White;
                    //e.Layout.Override.TemplateAddRowPromptAppearance.FontData.Bold = DefaultableBoolean.True;
                    //--------------Add row by default feature commented on 14th March.-----------



                    // By default the prompt is displayed across multiple cells. You can confine
                    // the prompt a particular cell by setting the SpecialRowPromptField property
                    // of the band to the key of the column that you want to display the prompt in.
                    //
                    //e.Layout.Bands[0].SpecialRowPromptField = e.Layout.Bands[0].Columns[0].Key;

                    // You can set the default value of an add-row field by using the DefaultCellValue 
                    // property of the column. Also you can initialize a template add-row to dynamic
                    // default values using the InitializeTemplateAddRow event of the UltraGrid.
                    //e.Layout.Bands[0].Columns[5].DefaultCellValue = "(DefaultValue)";
                    // --------------------------------------------------------------------------------

                    // Other miscellaneous settings
                    // --------------------------------------------------------------------------------
                    // Set the scroll style to immediate so the rows get scrolled immediately
                    // when the vertical scrollbar thumb is dragged.            
                    e.Layout.ScrollStyle = ScrollStyle.Immediate;
                    // ScrollBounds of ScrollToFill will prevent the user from scrolling the
                    // grid further down once the last row becomes fully visible.            
                    e.Layout.ScrollBounds = ScrollBounds.ScrollToFill;


                    SetGridAppearanceAndLayout(ref gridLayout);
                    //e.Layout.Bands[1].Hidden = true;

                    _gridBandOTCPositions = grdCreatePosition.DisplayLayout.Bands[0];

                    _gridBandOTCPositions.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                    UltraGridColumn colNotionalValue = _gridBandOTCPositions.Columns[CAPTION_NotionalValue];
                    colNotionalValue.Hidden = true;


                    UltraGridColumn colRealizedPNL = _gridBandOTCPositions.Columns[CAPTION_RealizedPNL];
                    colRealizedPNL.Hidden = true;

                    UltraGridColumn colTransactionSource = _gridBandOTCPositions.Columns[COLUMN_TransactionSource];
                    colTransactionSource.Hidden = true;

                    UltraGridColumn colEndDate = _gridBandOTCPositions.Columns[CAPTION_EndDate];
                    colEndDate.Hidden = true;

                    UltraGridColumn colStatus = _gridBandOTCPositions.Columns[CAPTION_Status];
                    colStatus.Hidden = true;

                    UltraGridColumn colRecordType = _gridBandOTCPositions.Columns[CAPTION_RecordType];
                    colRecordType.Hidden = true;


                    UltraGridColumn colAllocationID = _gridBandOTCPositions.Columns[CAPTION_ID];
                    colAllocationID.Hidden = true;

                    UltraGridColumn colStartTaxLotID = _gridBandOTCPositions.Columns[CAPTION_StartTaxLotID];
                    colStartTaxLotID.Hidden = true;

                    UltraGridColumn colClosingQuantity = _gridBandOTCPositions.Columns[CAPTION_ClosingQuantity];
                    colClosingQuantity.Hidden = true;

                    UltraGridColumn colOpenQuantity = _gridBandOTCPositions.Columns[CAPTION_OpenQuantity];
                    colOpenQuantity.Hidden = true;

                    //UltraGridColumn colGeneratedPNL = _gridBandOTCPositions.Columns[CAPTION_PNL];
                    //colGeneratedPNL.Hidden = true;

                    UltraGridColumn ColAUECID = _gridBandOTCPositions.Columns[CAPTION_AUECID];
                    ColAUECID.Hidden = true;

                    UltraGridColumn colMarkPriceForMonth = _gridBandOTCPositions.Columns[CAPTION_MarkPriceForMonth];
                    colMarkPriceForMonth.Hidden = true;

                    UltraGridColumn colMonthToDateRealizedProfit = _gridBandOTCPositions.Columns[CAPTION_MonthToDateRealizedProfit];
                    colMonthToDateRealizedProfit.Hidden = true;

                    UltraGridColumn colTaxlotClosingID = _gridBandOTCPositions.Columns["TaxlotClosingID"];
                    colTaxlotClosingID.Hidden = true;
                    //_gridBandOTCPositions.Columns[CAPTION_UnRealizedPNL].Hidden = true;
                    _gridBandOTCPositions.Columns[CAPTION_AvgPriceRealizedPL].Hidden = true;
                    _gridBandOTCPositions.Columns[CAPTION_SymbolAveragePrice].Hidden = true;

                    UltraGridColumn colValuePositionType = _gridBandOTCPositions.Columns[CAPTION_ValuePositionType];
                    colValuePositionType.Hidden = true;

                    UltraGridColumn colLeadCurrencyID = _gridBandOTCPositions.Columns[CAPTION_LeadCurrencyID];
                    colLeadCurrencyID.Hidden = true;

                    int visiblePosition = 1;

                    UltraGridColumn colSymbol = _gridBandOTCPositions.Columns[CAPTION_Symbol];
                    colSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.Width = 100;
                    colSymbol.Header.Caption = CAPTION_Symbol;
                    colSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    colSymbol.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colAsset = _gridBandOTCPositions.Columns[CAPTION_AssetID];
                    colAsset.Width = 75;
                    colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colAsset.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAsset.Header.Caption = "Asset";
                    colAsset.Header.VisiblePosition = visiblePosition++;
                    colAsset.CellActivation = Activation.NoEdit;




                    UltraGridColumn colCurrency = _gridBandOTCPositions.Columns[COLUMN_CurrencyID];
                    colCurrency.Width = 75;
                    colCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //colCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colCurrency.Header.Caption = CAPTION_Currency;
                    colCurrency.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colSideTypeID = _gridBandOTCPositions.Columns[CAPTION_SideTypeID];
                    colSideTypeID.Width = 65;
                    colSideTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colSideTypeID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //colSideTypeID.ValueList = _allSides; // cmbSide;
                    colSideTypeID.Header.Caption = "Side";
                    colSideTypeID.Header.VisiblePosition = visiblePosition++;

                    //Narendra Kumar Jangir, Sep 02,2013
                    //Make it compulsory for user to enter TransactionType,
                    //By default TransactionType will be Trade
                    UltraGridColumn colTransactionType = _gridBandOTCPositions.Columns[COLUMN_TransactionType];
                    colTransactionType.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colTransactionType.Width = 125;
                    colTransactionType.Header.Caption = CAPTION_TransactionType;
                    colTransactionType.Header.VisiblePosition = visiblePosition++;
                    colTransactionType.CellActivation = Activation.AllowEdit;

                    ValueList valueList = Prana.Utilities.UI.MiscUtilities.EnumHelper.TransactionTypeHasToDisplay(ValueListHelper.GetInstance.GetTransactionTypeValueList().Clone());
                    colTransactionType.ValueList = valueList;
                    //valueList.ValueListItems.Add(string.Empty, ApplicationConstants.C_COMBO_SELECT);
                    //colTransactionType.ValueList = CommonDataCache.CachedDataManager.GetInstance.GetTransactionTypeValueList().Clone();

                    colTransactionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;


                    UltraGridColumn colAccountID = _gridBandOTCPositions.Columns[CAPTION_AccountID];
                    colAccountID.Width = 70;
                    colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colAccountID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colAccountID.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colAccountID.ValueList = GetAccountsValueList();
                    colAccountID.Editor.BeforeExitEditMode += Editor_BeforeExitEditMode;


                    //colAccount.DefaultCellValue = new Account();
                    //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    colAccountID.Header.Caption = "Account";
                    colAccountID.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colstrategyID = _gridBandOTCPositions.Columns[CAPTION_StrategyID];
                    colstrategyID.Width = 80;
                    colstrategyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colstrategyID.ValueList = cmbStrategy;
                    colstrategyID.Header.Caption = "Strategy";
                    colstrategyID.Header.VisiblePosition = visiblePosition++;


                    UltraGridColumn colPositionStartQuantity = _gridBandOTCPositions.Columns[CAPTION_PositionStartQuantity];
                    colPositionStartQuantity.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleNonNegative;
                    colPositionStartQuantity.MaskInput = "nnnnnnnnnnnn.nnnnnn";
                    colPositionStartQuantity.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colPositionStartQuantity.Width = 60;
                    colPositionStartQuantity.Header.Caption = "Quantity";
                    colPositionStartQuantity.Header.VisiblePosition = visiblePosition++;



                    UltraGridColumn colAveragePrice = _gridBandOTCPositions.Columns[CAPTION_AveragePrice];
                    //colAveragePrice.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
                    //colAveragePrice.Format = "0.000000000000";
                    colAveragePrice.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colAveragePrice.Width = 60;
                    colAveragePrice.Header.Caption = "Average Price";
                    colAveragePrice.Header.VisiblePosition = visiblePosition++;


                    UltraGridColumn colCommissionSource = _gridBandOTCPositions.Columns[COL_COMMISSIONSOURCE];
                    colCommissionSource.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colCommissionSource.Width = 60;
                    colCommissionSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colCommissionSource.ValueList = _commissionSources;
                    colCommissionSource.Header.Caption = CAPTION_COMMISSIONSOURCE;
                    colCommissionSource.Header.VisiblePosition = visiblePosition++;
                    colCommissionSource.Hidden = true;
                    colCommissionSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    UltraGridColumn colSoftCommissionSource = _gridBandOTCPositions.Columns[COL_SOFTCOMMISSIONSOURCE];
                    colSoftCommissionSource.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colSoftCommissionSource.Width = 60;
                    colSoftCommissionSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colSoftCommissionSource.ValueList = _softCommissionSources;
                    colSoftCommissionSource.Header.Caption = CAPTION_SOFTCOMMISSIONSOURCE;
                    colSoftCommissionSource.Header.VisiblePosition = visiblePosition++;
                    colSoftCommissionSource.Hidden = true;
                    colSoftCommissionSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    UltraGridColumn colCommission = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_COMMISSION];
                    //colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colCommission.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colCommission.Width = 75;
                    colCommission.Header.Caption = OrderFields.CAPTION_COMMISSION;
                    colCommission.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colSoftCommission = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_SOFTCOMMISSION];
                    colSoftCommission.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colSoftCommission.Width = 75;
                    colSoftCommission.Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                    colSoftCommission.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colFees = _gridBandOTCPositions.Columns[CAPTION_Fees];
                    //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colFees.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colFees.Width = 45;
                    colFees.Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                    colFees.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colClearingBrokerFee = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE];
                    colClearingBrokerFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colClearingBrokerFee.Width = 45;
                    colClearingBrokerFee.Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                    colClearingBrokerFee.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colStampDuty = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_STAMPDUTY];
                    //colStampDuty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colStampDuty.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colStampDuty.Width = 70;
                    colStampDuty.Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                    colStampDuty.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colTransactionLevy = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY];
                    //colTransactionLevy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colTransactionLevy.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colTransactionLevy.Width = 70;
                    colTransactionLevy.Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                    colTransactionLevy.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colClearingFee = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_CLEARINGFEE];
                    //colClearingFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colClearingFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colClearingFee.Width = 70;
                    //Clearing Fee used as AUEC Fee1
                    colClearingFee.Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                    colClearingFee.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colTaxOnCommissions = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS];
                    //colTaxOnCommissions.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colTaxOnCommissions.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colTaxOnCommissions.Width = 70;
                    colTaxOnCommissions.Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                    colTaxOnCommissions.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colMiscFees = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_MISCFEES];
                    //colMiscFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colMiscFees.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colMiscFees.Width = 70;
                    //MiscFees used as AUEC Fee2
                    colMiscFees.Header.Caption = OrderFields.CAPTION_MISCFEES;
                    colMiscFees.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colSecFee = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_SECFEE];
                    //colSecFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colSecFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colSecFee.Width = 70;
                    colSecFee.Header.Caption = OrderFields.CAPTION_SECFEE;
                    colSecFee.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colOccFee = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_OCCFEE];
                    //colOccFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colOccFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colOccFee.Width = 70;
                    colOccFee.Header.Caption = OrderFields.CAPTION_OCCFEE;
                    colOccFee.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colOrfFee = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_ORFFEE];
                    //colOrfFee.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
                    colOrfFee.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colOrfFee.Width = 70;
                    colOrfFee.Header.Caption = OrderFields.CAPTION_ORFFEE;
                    colOrfFee.Header.VisiblePosition = visiblePosition++;

                    //colCommissionSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

                    UltraGridColumn colMultiplier = _gridBandOTCPositions.Columns[CAPTION_Multiplier];
                    //colMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
                    //colMultiplier.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colMultiplier.Width = 60;
                    colMultiplier.Header.Caption = "Multiplier";
                    colMultiplier.Header.VisiblePosition = visiblePosition++;
                    colMultiplier.CellActivation = Activation.NoEdit;


                    UltraGridColumn colPayReceive = _gridBandOTCPositions.Columns[CAPTION_PayReceive];
                    colPayReceive.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colPayReceive.Width = 60;
                    colPayReceive.Header.Caption = "Pay/Receive";
                    colPayReceive.Header.VisiblePosition = visiblePosition++;



                    UltraGridColumn colpayReceiveChanges = grdCreatePosition.DisplayLayout.Bands[0].Columns[CAPTION_IfPayReceiveChanges];
                    colpayReceiveChanges.ValueList = _ifPayReceiveChanges;
                    colpayReceiveChanges.Header.Caption = "If Pay/Receive Changes";
                    colpayReceiveChanges.Hidden = false;
                    colpayReceiveChanges.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colpayReceiveChanges.Header.VisiblePosition = visiblePosition++;


                    //UltraGridColumn colForexConversion = _gridBandOTCPositions.Columns[CAPTION_ForexConversion];
                    //colForexConversion.Width = 60;
                    //colForexConversion.Header.Caption = "Forex Conversion";
                    //colForexConversion.Header.VisiblePosition = 21;
                    //colForexConversion.CellActivation = Activation.Disabled;

                    UltraGridColumn colTradeDate = _gridBandOTCPositions.Columns[CAPTION_StartDate];
                    colTradeDate.Width = 80;
                    colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                    colTradeDate.MaskInput = "mm/dd/yyyy";
                    colTradeDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colTradeDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colTradeDate.Header.Caption = "Trade Date";
                    colTradeDate.Header.VisiblePosition = 28;

                    UltraGridColumn colSettlementDate = _gridBandOTCPositions.Columns[CAPTION_SettlementDate];
                    colSettlementDate.Width = 80;
                    //colSettlementDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                    UltraCalendarCombo tempCmb2 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
                    this.Controls.Add(tempCmb2);
                    colSettlementDate.EditorComponent = tempCmb2;
                    colSettlementDate.MaskInput = "mm/dd/yyyy";
                    colSettlementDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colSettlementDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colSettlementDate.Header.Caption = "Settlement Date";
                    colSettlementDate.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colCounterParty = _gridBandOTCPositions.Columns[CAPTION_CounterPartyID];
                    colCounterParty.Width = 90;
                    colCounterParty.Header.Caption = ApplicationConstants.CONST_BROKER;
                    colCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    colCounterParty.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    colCounterParty.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    colCounterParty.ValueList = GetCounterPartyValueList();
                    colCounterParty.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colVenue = _gridBandOTCPositions.Columns[CAPTION_VenueID];
                    colVenue.Width = 90;
                    colVenue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colVenue.ValueList = cmbVenue;
                    colVenue.Header.Caption = "Venue";
                    colVenue.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colUnderLying = _gridBandOTCPositions.Columns[CAPTION_UnderlyingID];
                    colUnderLying.Hidden = true;
                    //colUnderLying.Width = 75;
                    //colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //colUnderLying.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //colUnderLying.Header.Caption = "UnderLying";
                    //colUnderLying.Header.VisiblePosition = 27;
                    //colUnderLying.CellActivation = Activation.NoEdit;


                    UltraGridColumn colExchange = _gridBandOTCPositions.Columns[CAPTION_ExchangeID];
                    colExchange.Hidden = true;
                    //colExchange.Width = 75;
                    //colExchange.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    //colExchange.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //colExchange.Header.Caption = "Exchange";
                    //colExchange.Header.VisiblePosition = 28;
                    //colExchange.CellActivation = Activation.NoEdit;



                    UltraGridColumn colDescription = _gridBandOTCPositions.Columns[CAPTION_Description];
                    colDescription.Width = 150;
                    colDescription.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    colDescription.Header.Caption = "Description";
                    colDescription.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    colDescription.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn ColSide = _gridBandOTCPositions.Columns[CAPTION_Side];
                    ColSide.Hidden = true;

                    UltraGridColumn ColPositionType = _gridBandOTCPositions.Columns[CAPTION_PositionType];
                    ColPositionType.Hidden = true;

                    UltraGridColumn colAccount = _gridBandOTCPositions.Columns[CAPTION_AccountValue];
                    colAccount.Hidden = true;


                    UltraGridColumn colstrategy = _gridBandOTCPositions.Columns[CAPTION_Strategy];
                    colstrategy.Hidden = true;

                    UltraGridColumn colAUECLocalDateToday = _gridBandOTCPositions.Columns[CAPTION_AUECLocalDateToday];
                    colAUECLocalDateToday.Hidden = true;

                    UltraGridColumn colExpiredTaxlotID = _gridBandOTCPositions.Columns[CAPTION_ExpiredTaxlotID];
                    colExpiredTaxlotID.Hidden = true;


                    UltraGridColumn colExpiredQty = _gridBandOTCPositions.Columns[CAPTION_ExpirationQty];
                    colExpiredQty.Hidden = true;


                    UltraGridColumn colFXRate = _gridBandOTCPositions.Columns[COLUMN_FXRate];
                    colFXRate.MaskInput = "nnnnnnnnnn.nnnnnnnn";
                    colFXRate.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colFXRate.Width = 60;
                    colFXRate.Header.Caption = CAPTION_FXRate;
                    colFXRate.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colFXRateOperator = _gridBandOTCPositions.Columns[COLUMN_FxRateCalc];
                    colFXRateOperator.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colFXRateOperator.Width = 60;
                    colFXRateOperator.ValueList = _fxConversionMethodOperator;
                    colFXRateOperator.Header.Caption = CAPTION_FxRateCalc;
                    colFXRateOperator.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colSettlCurrency = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCYID];
                    colSettlCurrency.Width = 100;
                    colSettlCurrency.Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                    colSettlCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colSettlCurrency.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colDelta = _gridBandOTCPositions.Columns["Delta"];
                    colDelta.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                    colDelta.Width = 60;
                    colDelta.Header.Caption = "Leveraged Factor";
                    colDelta.Header.VisiblePosition = visiblePosition++;
                    colDelta.CellActivation = Activation.NoEdit;

                    UltraGridColumn colIsOptionActivated = _gridBandOTCPositions.Columns[COLUMN_IsOptionActivated];
                    colIsOptionActivated.Header.Caption = CAPTION_IsOptionActivated;
                    colIsOptionActivated.Header.VisiblePosition = visiblePosition++;

                    ///Rahul 20120305
                    ///Set to non editable.
                    UltraGridColumn colUnderlyingSymbol = _gridBandOTCPositions.Columns[COLUMN_UnderlyingSymbol];
                    colUnderlyingSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    colUnderlyingSymbol.CharacterCasing = CharacterCasing.Upper;
                    colUnderlyingSymbol.Width = 120;
                    colUnderlyingSymbol.Header.Caption = CAPTION_UnderlyingSymbol;
                    colUnderlyingSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    colUnderlyingSymbol.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colOptionType = _gridBandOTCPositions.Columns[COLUMN_OptionType];
                    colOptionType.Header.Caption = CAPTION_OptionType;
                    colOptionType.ValueList = cmbOptionType;
                    colOptionType.Width = 80;
                    colOptionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colOptionType.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colStrikePrice = _gridBandOTCPositions.Columns[COLUMN_StrikePrice];
                    colStrikePrice.Width = 100;
                    colStrikePrice.Header.Caption = CAPTION_StrikePrice;
                    colStrikePrice.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colExpirationDate = _gridBandOTCPositions.Columns[COLUMN_ExpirationDate];
                    colExpirationDate.Width = 100;
                    UltraCalendarCombo tempCmb3 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
                    this.Controls.Add(tempCmb3);
                    colTradeDate.EditorComponent = tempCmb3;
                    colExpirationDate.MaskInput = "mm/dd/yyyy";
                    colExpirationDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colExpirationDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colExpirationDate.Header.Caption = CAPTION_ExpirationDate;
                    colExpirationDate.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colProcessDate = _gridBandOTCPositions.Columns["ProcessDate"];
                    colProcessDate.Header.VisiblePosition = visiblePosition++;

                    UltraGridColumn colIsUnderlyingValidated = _gridBandOTCPositions.Columns["IsUnderlyingValidated"];
                    colIsUnderlyingValidated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colIsUnderlyingValidated.Hidden = true;

                    UltraGridColumn colUnderlyingAssetCategory = _gridBandOTCPositions.Columns["UnderlyingAssetCategory"];
                    colUnderlyingAssetCategory.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colUnderlyingAssetCategory.Hidden = true;

                    UltraGridColumn colStrikePriceMultiplier = _gridBandOTCPositions.Columns["StrikePriceMultiplier"];
                    colStrikePriceMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colStrikePriceMultiplier.Hidden = true;

                    UltraGridColumn colEsignalOptionRoot = _gridBandOTCPositions.Columns["EsignalOptionRoot"];
                    colEsignalOptionRoot.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colEsignalOptionRoot.Hidden = true;

                    UltraGridColumn colBloombergOptionRoot = _gridBandOTCPositions.Columns["BloombergOptionRoot"];
                    colBloombergOptionRoot.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colBloombergOptionRoot.Hidden = true;

                    UltraGridColumn colUnderlyingAUECID = _gridBandOTCPositions.Columns["UnderlyingAUECID"];
                    colUnderlyingAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colUnderlyingAUECID.Hidden = true;

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn col in this.grdCreatePosition.DisplayLayout.Bands[0].Columns)
                    {
                        col.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                        switch (col.DataType.Name)
                        {
                            case "Int32":
                            case "Int64":
                            case "Double":
                            case "Single":
                            case "Decimal":
                                col.Format = "#,###0.00";
                                break;
                        }
                    }
                    colAllocationID.Hidden = true;
                    _isOTCPositionsGridInitialized = true;

                    for (int i = 1; i <= 45; i++)
                    {
                        UltraGridColumn tradeAttributeCol = _gridBandOTCPositions.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i];
                        tradeAttributeCol.Hidden = true;
                        tradeAttributeCol.Header.Caption = CachedDataManager.GetInstance.GetAttributeNameForValue(ClosingConstants.CAPTION_TRADEATTRIBUTE + i);
                        tradeAttributeCol.CellActivation = Activation.NoEdit;
                    }

                    CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSE_TRADE);
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
        /// Handles the BeforeExitEditMode event of the Editor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.BeforeExitEditModeEventArgs"/> instance containing the event data.</param>
        void Editor_BeforeExitEditMode(object sender, Infragistics.Win.BeforeExitEditModeEventArgs e)
        {
            try
            {
                var temp = (EditorWithCombo)sender;
                if (temp.SelectedIndex < 0)
                {
                    temp.SelectedIndex = 0;
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
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="gridLayout">The grid layout.</param>
        private void SetGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {
            try
            {
                grdCreatePosition.DisplayLayout.Bands[0].Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;

                gridLayout.Appearance.BackColor = Color.Black; //BB
                gridLayout.Override.RowAppearance.BackColor = Color.Black; //BB

                gridLayout.Override.SelectedRowAppearance.BackColor = Color.Gold;
                gridLayout.Override.SelectedRowAppearance.BorderColor = Color.Black;
                gridLayout.Override.SelectedRowAppearance.ForeColor = Color.Black;

                gridLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
                gridLayout.Override.ActiveRowAppearance.BackColor = Color.Gold;
                gridLayout.Override.ActiveRowAppearance.BorderColor = Color.Black;


                gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
                gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
                gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

                gridLayout.AutoFitStyle = AutoFitStyle.None;
                gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;

                gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

                gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
                gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;

                UltraGridBand band = this.grdCreatePosition.DisplayLayout.Bands[0];
                gridLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

                band.Override.HeaderAppearance.BackColor = Color.WhiteSmoke;
                band.Override.HeaderAppearance.ForeColor = Color.Black;

                grdCreatePosition.DisplayLayout.Appearance.ForeColor = Color.White;
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

        //Infragistics.Win.Misc.UltraButton btnAddToCloseTrade;
        /// <summary>
        /// Populates the close trades interface.
        /// input parameters to be added in some time...
        /// </summary>
        /// <param name="isInternal">if set to <c>true</c> </param>
        public void PopulateCreatePositionInterface(CompanyUser companyUser)
        {
            try
            {
                this._userID = companyUser.CompanyUserID;
                this._companyID = companyUser.CompanyID;
                this._companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                _defaultSelectValListItem = new ValueListItem(_defaultID, ApplicationConstants.C_COMBO_SELECT);
                grdCreatePosition.DataSource = null;
                grdCreatePosition.DataMember = "OTCPositions";
                grdCreatePosition.DataSource = _createPositionUserInterface;
                BindComboBoxes(_userID, _companyID);
                BindFxRateConvertor();
                BindCommissionSources();
                BindSoftCommissionSources();
                BindifPayReceiveChanges();
                foreach (Prana.BusinessObjects.TradingAccount tradingAccount in companyUser.TradingAccounts)
                {
                    _pMTradingAccountID = tradingAccount.TradingAccountID;
                    break;
                }
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                    //new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
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

        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {

            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                        ValidateAndMarshal(sender, e);
                }
                else
                {
                    ValidateAndMarshal(sender, e);
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

        private void ValidateAndMarshal(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SecMasterObjHandler secObjhandler = new SecMasterObjHandler(UpdateValue);
                        this.BeginInvoke(secObjhandler, new object[] { sender, e });
                    }
                    else
                    {
                        UpdateValue(sender, e);
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


        private void UpdateValue(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj == null)
                {
                    Logger.LoggerWrite(@"SecMasterObj is null in UpdateValue function in \Prana_CA\Prana.PM\Prana.PM.Client.UI\Controls\CtrlCreateAndImportPosition.cs class.");
                    return;
                }

                if (grdCreatePosition.Rows == null)
                {
                    Logger.LoggerWrite(@"grdCreatePosition.Rows is null.");
                    return;
                }

                UltraGridRow[] rows = grdCreatePosition.Rows.GetFilteredInNonGroupByRows();
                grdCreatePosition.BeginUpdate();
                foreach (UltraGridRow row in rows)
                {
                    if (!row.Activation.Equals(Activation.Disabled))
                    {
                        OTCPosition otcPos = (OTCPosition)row.ListObject;
                        if (otcPos.TransactionSource.Equals(TransactionSource.Closing))
                            row.Activate();
                        else if (!row.Activated)
                            continue;
                        #region Symbol Response
                        if (otcPos.Symbol == secMasterObj.TickerSymbol && (!otcPos.IsOptionActivated || otcPos.IsUnderlyingValidated))
                        {
                            otcPos.AssetID = secMasterObj.AssetID;
                            otcPos.UnderlyingID = secMasterObj.UnderLyingID;
                            otcPos.AUECID = secMasterObj.AUECID;
                            if (otcPos.TransactionSource == TransactionSource.Closing)
                            {
                                Venue exAssignVenue = CachedDataManager.GetInstance.GetExerciseAssignVenue();
                                if (exAssignVenue != null)
                                {
                                    ValueList venues = GetVenuesValueList();
                                    venues.ValueListItems.Add(exAssignVenue.VenueID, exAssignVenue.Name);
                                    row.Cells[CAPTION_VenueID].ValueList = venues;
                                    otcPos.VenueID = exAssignVenue.VenueID;
                                }
                                BasicOrderInfo basicOrderInfo = new BasicOrderInfo(otcPos);
                                basicOrderInfo = _closingServices.InnerChannel.CalculateOtherFeesForBasicOrderInfo(basicOrderInfo);
                                otcPos.Commission = basicOrderInfo.Commission;
                                otcPos.SoftCommission = basicOrderInfo.SoftCommission;
                                otcPos.SecFee = basicOrderInfo.SecFee;
                            }
                            if (otcPos.AssetID != (int)AssetCategory.FX && otcPos.AssetID != (int)AssetCategory.FXForward)
                            {
                                otcPos.CurrencyID = secMasterObj.CurrencyID;
                                ValueList settlementCurrencies = new ValueList();
                                if (CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(secMasterObj.CurrencyID))
                                    settlementCurrencies.ValueListItems.Add(secMasterObj.CurrencyID, CachedDataManager.GetInstance.GetAllCurrencies()[secMasterObj.CurrencyID]);

                                if (secMasterObj.CurrencyID != _companyBaseCurrencyID && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(_companyBaseCurrencyID))
                                {
                                    settlementCurrencies.ValueListItems.Add(_companyBaseCurrencyID, CachedDataManager.GetInstance.GetAllCurrencies()[_companyBaseCurrencyID]);
                                }
                                row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = settlementCurrencies;
                                row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Value = secMasterObj.CurrencyID;
                            }

                            otcPos.ExchangeID = secMasterObj.ExchangeID;
                            otcPos.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                            UpdateFXRateColumn(otcPos);

                            // Modified By : Manvendra Prajapati
                            // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9684
                            // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9480
                            string orderSide = otcPos.TransactionSource == TransactionSource.CreateTransactionUI ? null : otcPos.SideTagValue;
                            ValueList sideList = GetSideFromAssetID(otcPos.AssetID, orderSide);
                            //if (otcPos.TransactionSource.Equals(TransactionSource.CreateTransactionUI))
                            //{
                            row.Cells[CAPTION_SideTypeID].ValueList = sideList;
                            if (otcPos == null || string.IsNullOrWhiteSpace(otcPos.SideTagValue))
                            {
                                row.Cells[CAPTION_SideTypeID].Value = sideList.ValueListItems[0].DataValue;
                            }
                            ValueList transactionTypeValueList = Prana.Utilities.UI.MiscUtilities.EnumHelper.TransactionTypeHasToDisplay(ValueListHelper.GetInstance.GetTransactionTypeValueList().Clone());
                            if (row.Cells[COLUMN_TransactionType].ValueList == null)
                            {
                                row.Cells[COLUMN_TransactionType].ValueList = transactionTypeValueList;

                                if (otcPos != null)
                                {//TODO:Need to verify this
                                    row.Cells[COLUMN_TransactionType].Value = otcPos.TransactionType;
                                }
                                else
                                {
                                    string transactionTypeBasedonSideTagValue = GetTransactionTypeBasedonSideTagValue(row.Cells[CAPTION_SideTypeID].Value.ToString());
                                    row.Cells[COLUMN_TransactionType].Value = transactionTypeBasedonSideTagValue;
                                }
                            }
                            if (otcPos.AUECLocalDateToday.Equals(DateTimeConstants.MinValue))
                            {
                                otcPos.AUECLocalDateToday = GetAUECTradeDate(otcPos.AUECID);
                                row.Cells[CAPTION_StartDate].Activation = Activation.AllowEdit;
                                row.Cells[CAPTION_SettlementDate].Activation = Activation.AllowEdit;

                                row.Cells[CAPTION_AUECLocalDateToday].Value = otcPos.AUECLocalDateToday;
                                row.Cells[CAPTION_StartDate].Value = otcPos.AUECLocalDateToday;
                            }
                            otcPos.Multiplier = secMasterObj.Multiplier;
                            otcPos.RoundLot = secMasterObj.RoundLot;

                            UltraGridCell cellCurrency = row.Cells[COLUMN_CurrencyID];
                            cellCurrency.Activation = Activation.NoEdit;

                            switch (secMasterObj.AssetCategory)
                            {
                                case AssetCategory.EquityOption:
                                case AssetCategory.FutureOption:

                                    SecMasterOptObj optionObj = (SecMasterOptObj)secMasterObj;
                                    otcPos.Call_Put = optionObj.PutOrCall;
                                    otcPos.StrikePrice = optionObj.StrikePrice;
                                    otcPos.ExpirationDate = optionObj.ExpirationDate;
                                    GetSettlementDate(otcPos);
                                    break;
                                case AssetCategory.Equity:
                                case AssetCategory.PrivateEquity:
                                case AssetCategory.CreditDefaultSwap:
                                    SecMasterEquityObj equityObj = (SecMasterEquityObj)secMasterObj;
                                    otcPos.Delta = equityObj.Delta;
                                    GetSettlementDate(otcPos);
                                    break;

                                case AssetCategory.Future:
                                    SecMasterFutObj futObj = (SecMasterFutObj)secMasterObj;
                                    otcPos.ExpirationDate = futObj.ExpirationDate;
                                    GetSettlementDate(otcPos);
                                    break;

                                case AssetCategory.FXForward:
                                case AssetCategory.FX:

                                    int LeadCurrencyID = 0;
                                    int VsCurrencyID = 0;
                                    if (secMasterObj is SecMasterFxObj)
                                    {
                                        LeadCurrencyID = ((SecMasterFxObj)secMasterObj).LeadCurrencyID;
                                        VsCurrencyID = ((SecMasterFxObj)secMasterObj).VsCurrencyID;
                                        otcPos.LeadCurrencyID = LeadCurrencyID;
                                        otcPos.VsCurrencyID = VsCurrencyID;
                                        GetSettlementDate(otcPos);
                                    }
                                    else if (secMasterObj is SecMasterFXForwardObj)
                                    {
                                        LeadCurrencyID = ((SecMasterFXForwardObj)secMasterObj).LeadCurrencyID;
                                        VsCurrencyID = ((SecMasterFXForwardObj)secMasterObj).VsCurrencyID;
                                        otcPos.LeadCurrencyID = LeadCurrencyID;
                                        otcPos.VsCurrencyID = VsCurrencyID;
                                        otcPos.SettlementDate = ((SecMasterFXForwardObj)secMasterObj).ExpirationDate;
                                    }
                                    cellCurrency.Activation = Activation.AllowEdit;
                                    row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Activation = Activation.NoEdit;

                                    ValueList transactionCurrency = new ValueList();
                                    transactionCurrency.ValueListItems.Add(LeadCurrencyID, CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(LeadCurrencyID));
                                    transactionCurrency.ValueListItems.Add(VsCurrencyID, CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(VsCurrencyID));
                                    AddtransactionCurrencyField(transactionCurrency);
                                    otcPos.FXConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M;
                                    grdCreatePosition.ActiveRow.Cells[COLUMN_FxRateCalc].Activation = Activation.Disabled;
                                    grdCreatePosition.ActiveRow.Cells[COLUMN_FXRate].Activation = Activation.Disabled;
                                    break;

                                case AssetCategory.FixedIncome:
                                case AssetCategory.ConvertibleBond:
                                    SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)secMasterObj;
                                    otcPos.Coupon = fixedIncomeObj.Coupon;
                                    otcPos.MaturityDate = fixedIncomeObj.MaturityDate;
                                    otcPos.FirstCouponDate = fixedIncomeObj.FirstCouponDate;
                                    otcPos.IssueDate = fixedIncomeObj.IssueDate;
                                    otcPos.AccrualBasis = fixedIncomeObj.AccrualBasis;
                                    otcPos.BondType = fixedIncomeObj.BondType;
                                    otcPos.Freq = fixedIncomeObj.Frequency;
                                    otcPos.IsZero = fixedIncomeObj.IsZero;
                                    otcPos.Description = fixedIncomeObj.BondDescription;
                                    otcPos.DaysToSettlement = fixedIncomeObj.DaysToSettlement;
                                    otcPos.ExpirationDate = fixedIncomeObj.MaturityDate;
                                    GetSettlementDate(otcPos);
                                    CalculateAccruedInterest();
                                    break;
                            }
                        }
                        #endregion

                        #region UnderlyingSymbol Response
                        else if (otcPos.UnderlyingSymbol == secMasterObj.TickerSymbol && otcPos.IsOptionActivated)
                        {
                            OptionDetail optionDetail = new OptionDetail();
                            optionDetail.ExpirationDate = otcPos.ExpirationDate;
                            optionDetail.StrikePrice = otcPos.StrikePrice;
                            optionDetail.UnderlyingSymbol = otcPos.UnderlyingSymbol;
                            optionDetail.Symbology = ApplicationConstants.PranaSymbology;
                            optionDetail.AssetCategory = secMasterObj.AssetCategory;
                            optionDetail.AUECID = secMasterObj.AUECID;
                            optionDetail.OptionType = (OptionType)otcPos.OptionType;
                            optionDetail.StrikePriceMultiplier = secMasterObj.StrikePriceMultiplier;
                            optionDetail.EsignalOptionRoot = secMasterObj.EsignalOptionRoot;
                            optionDetail.BloombergOptionRoot = secMasterObj.BloombergOptionRoot;

                            OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                            otcPos.Symbol = optionDetail.Symbol;
                            otcPos.UnderlyingAssetCategory = secMasterObj.AssetCategory;
                            otcPos.StrikePriceMultiplier = secMasterObj.StrikePriceMultiplier;
                            otcPos.UnderlyingAUECID = secMasterObj.AUECID;
                            otcPos.EsignalOptionRoot = secMasterObj.EsignalOptionRoot;
                            otcPos.BloombergOptionRoot = secMasterObj.BloombergOptionRoot;
                            otcPos.IsUnderlyingValidated = true;

                            SymbolChanged();
                        }
                        #endregion
                    }
                }
                grdCreatePosition.EndUpdate();
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
        /// bind the currency list to settlement and currency column according to fx.
        /// </summary>
        /// <param name="transactionCurrency">The transaction currency.</param>
        private void AddtransactionCurrencyField(ValueList transactionCurrency)
        {
            try
            {
                grdCreatePosition.ActiveRow.Cells[COLUMN_CurrencyID].ValueList = transactionCurrency;
                grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ValueList = transactionCurrency;
                if (grdCreatePosition.ActiveRow != null && (int)grdCreatePosition.ActiveRow.Cells[COLUMN_CurrencyID].Value == 0)
                {
                    grdCreatePosition.ActiveRow.Cells[COLUMN_CurrencyID].Value = transactionCurrency.ValueListItems[0].DataValue;
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

        private ValueList GetSideFromAssetID(int assetID, string orderSide = null)
        {
            ValueList sideValueList = new ValueList();
            try
            {
                DataTable dt = CachedDataManager.GetInstance.GetOrderSides(assetID).Copy();
                foreach (DataRow row in dt.Rows)
                {
                    // https://jira.nirvanasolutions.com:8443/browse/PRANA-36770

                    string orderSideTagValue = String.IsNullOrEmpty(row[0].ToString()) ? String.Empty : TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(row[0].ToString());
                    if (orderSide == null ||
                        ((orderSide.Equals(FIXConstants.SIDE_Buy) || orderSide.Equals(FIXConstants.SIDE_Buy_Closed)) && (orderSideTagValue.Equals(FIXConstants.SIDE_Buy) || orderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))) ||
                        ((orderSide.Equals(FIXConstants.SIDE_Sell) || orderSide.Equals(FIXConstants.SIDE_SellShort)) && (orderSideTagValue.Equals(FIXConstants.SIDE_Sell) || orderSideTagValue.Equals(FIXConstants.SIDE_SellShort))))
                    {
                        sideValueList.ValueListItems.Add(orderSideTagValue, row[1].ToString());
                    }

                    // sideValueList.ValueListItems.Add(row[0], row[1].ToString());
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
            return sideValueList;
        }

        private bool _addButtonClicked;

        /// <summary>
        /// Gets or sets a value indicating whether [add button clicked].
        /// </summary>
        /// <value><c>true</c> if [add button clicked]; otherwise, <c>false</c>.</value>
        public bool AddButtonClicked
        {
            get { return _addButtonClicked; }
            set { _addButtonClicked = value; }
        }
        private NetPositionList _netPositions = new NetPositionList();

        public NetPositionList NetPositions
        {
            get { return _netPositions; }
            set { _netPositions = value; }
        }

        private OTCPositionList _otcPositionList = new OTCPositionList();

        public OTCPositionList OTCPositionList
        {
            get { return _otcPositionList; }
            set { _otcPositionList = value; }
        }

        private void SetUpAllValueListsForAddNewRow()
        {
            try
            {
                grdCreatePosition.ActiveRow.Cells[CAPTION_AccountID].Value = int.MinValue;
                grdCreatePosition.ActiveRow.Cells[CAPTION_StrategyID].Value = int.MinValue;
                //Narendra Kumar Jangir 2013 Feb 20
                //CounterPartyID should be auto selected while expiring trades.
                //http://jira.nirvanasolutions.com:8080/browse/MON-37
                //grdCreatePosition.ActiveRow.Cells[CAPTION_CounterPartyID].Value = 0;
                grdCreatePosition.ActiveRow.Cells[CAPTION_VenueID].Value = 0;
                //grdCreatePosition.ActiveRow.Cells[CAPTION_SymbolConventionID].Value = 1;
                grdCreatePosition.ActiveRow.Cells[COLUMN_ExpirationDate].Value = DateTime.UtcNow;
                grdCreatePosition.ActiveRow.Cells[CAPTION_SettlementDate].Value = DateTime.UtcNow;

                if (TradingTktPrefs.TTGeneralPrefs.IsShowOptionDetails && grdCreatePosition.ActiveRow.Cells[CAPTION_ExpiredTaxlotID].Value.ToString() == "")
                    grdCreatePosition.ActiveRow.Cells[COLUMN_IsOptionActivated].Value = true;
                else
                    grdCreatePosition.ActiveRow.Cells[COLUMN_IsOptionActivated].Value = false;

                OptionActivatedCheckChanged();

                grdCreatePosition.ActiveRow.Update();
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

        DateTime _tempDate = DateTimeConstants.MinValue;

        public void AddNewRow(OTCPosition otcPositionNew, int rowNo)
        {
            try
            {
                if (string.IsNullOrEmpty(otcPositionNew.Symbol))
                {
                    _tempDate = otcPositionNew.StartDate;
                    grdCreatePosition.DisplayLayout.Bands[0].Override.SelectedRowAppearance.BackColor = Color.DarkSlateGray;
                    CreatePositionUserInterface createPositionUserInterface = (CreatePositionUserInterface)grdCreatePosition.DataSource;
                    OTCPositionList otcPositionList = createPositionUserInterface.OTCPositions;

                    grdCreatePosition.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.DarkSlateGray;

                    otcPositionList.Insert(rowNo, otcPositionNew);
                    grdCreatePosition.Rows[rowNo].Activate();
                    grdCreatePosition.Rows[rowNo].Cells[CAPTION_Symbol].Activate();

                    SetUpAllValueListsForAddNewRow();
                    UpdateCommissionFeesColumns();
                    grdCreatePosition.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.DarkSlateGray;
                    grdCreatePosition.ActiveRow.Cells[CAPTION_StartDate].Value = _tempDate;
                    //Narendra Kumar Jangir 2013 Feb 13
                    //Apply auto generated counter party and venue id
                    //http://jira.nirvanasolutions.com:8080/browse/MON-37
                    grdCreatePosition.ActiveRow.Cells[CAPTION_CounterPartyID].Value = ((otcPositionNew.CounterPartyID > 0) ? otcPositionNew.CounterPartyID : 0);
                    grdCreatePosition.ActiveRow.Cells[CAPTION_StrategyID].Value = otcPositionNew.StrategyID;
                    grdCreatePosition.ActiveRow.Cells[CAPTION_Strategy].Value = otcPositionNew.Strategy;
                    grdCreatePosition.ActiveRow.Cells[CAPTION_StrategyID].Activation = Activation.Disabled;
                    this.grdCreatePosition.PerformAction(UltraGridAction.EnterEditMode);

                }
                else
                {
                    CreatePositionUserInterface createPositionUserInterface = (CreatePositionUserInterface)grdCreatePosition.DataSource;
                    OTCPositionList otcPositionList = createPositionUserInterface.OTCPositions;
                    if (grdCreatePosition.DisplayLayout.Bands[0].SortedColumns.Count > 0)
                    {
                        grdCreatePosition.DisplayLayout.Bands[0].ResetSortedColumns();
                    }
                    otcPositionList.Insert(rowNo, otcPositionNew);
                    grdCreatePosition.Focus();
                    grdCreatePosition.Rows[rowNo].Activate();
                    grdCreatePosition.Rows[rowNo].Cells[CAPTION_Symbol].Activate();
                    grdCreatePosition.Rows[rowNo].Cells[CAPTION_SideTypeID].ValueList = GetSideFromAssetID(otcPositionNew.AssetID);
                    SetUpAllValueListsForAddNewRow();
                    UpdateCommissionFeesColumns();
                    this.grdCreatePosition.PerformAction(UltraGridAction.EnterEditMode);
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

        private StrategyCollection _strategies = new StrategyCollection();
        private BindingList<Prana.BusinessObjects.CounterParty> _counterParties = new BindingList<Prana.BusinessObjects.CounterParty>();
        private BindingList<Prana.BusinessObjects.Venue> _venue = new BindingList<Prana.BusinessObjects.Venue>();
        DataTable _accounts = new DataTable();
        //private SymbolConventionList _symbolConventions = new SymbolConventionList();
        //private AccountCollection _accountsList = new AccountCollection();
        private ValueList _templates = new ValueList();
        private ValueList _fileType = new ValueList();
        private ValueList _importType = new ValueList();
        //private ValueList _allSides = new ValueList();
        //Dictionary<string, string> allSides = KeyValueDataManager.GetInstance().GetAllSides();
        private EnumerationValueList _optionTypes = new EnumerationValueList();

        private int _defaultID = -1;
        ValueListItem _defaultSelectValListItem = new ValueListItem(-1, ApplicationConstants.C_COMBO_SELECT);
        /// <summary>
        /// Binds the combo boxes.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="companyID">The company ID.</param>
        private void BindComboBoxes(int userID, int companyID)
        {

            try
            {
                ValueList assets = CreatePositionManager.GetAssets(companyID);
                assets.ValueListItems.Add(0, ApplicationConstants.C_COMBO_SELECT);
                // Dictionary<int, string> dictUnderlyings = CachedDataManager.GetInstance.GetAllUnderlyings();
                // Dictionary<int, string> dictExchanges = CachedDataManager.GetInstance.GetAllExchanges();
                Dictionary<int, string> dictCurrency = CachedDataManager.GetInstance.GetAllCurrencies();
                // ValueList underLying = new ValueList();
                ValueList currencies = new ValueList();


                //ValueList exchanges = new ValueList();
                //underLying.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                //foreach (KeyValuePair<int, string> item in dictUnderlyings)
                //{
                //    underLying.ValueListItems.Add(item.Key, item.Value);
                //}

                //exchanges.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                //foreach (KeyValuePair<int, string> item in dictExchanges)
                //{
                //    exchanges.ValueListItems.Add(item.Key, item.Value);
                //}

                currencies.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (KeyValuePair<int, string> item in dictCurrency)
                {
                    currencies.ValueListItems.Add(item.Key, item.Value);
                }

                //foreach (KeyValuePair<string, string> kvp in allSides)
                //{
                //    if (!_allSides.ValueListItems.Contains(kvp.Value))
                //    {
                //        _allSides.ValueListItems.Add(kvp.Key, kvp.Value);
                //    }
                //}

                // _accountsList = CachedDataManager.GetInstance.GetUserAccounts();
                _strategies = CachedDataManager.GetInstance.GetUserStrategies();

                // assets
                UltraGridColumn colAsset = _gridBandOTCPositions.Columns[CAPTION_AssetID];
                colAsset.ValueList = assets;

                //underlying
                //UltraGridColumn colUnderLying = _gridBandOTCPositions.Columns[CAPTION_UnderlyingID];
                //colUnderLying.ValueList = underLying;

                //// Exchanges
                //UltraGridColumn colExchange = _gridBandOTCPositions.Columns[CAPTION_ExchangeID];
                //colExchange.ValueList = exchanges;

                UltraGridColumn colCurrency = _gridBandOTCPositions.Columns[COLUMN_CurrencyID];
                colCurrency.ValueList = currencies;

                cmbStrategy.DataSource = null;
                cmbStrategy.DataSource = _strategies; //CreateSourceColumns(_mapAccounts.CompanyNameID.ID, _mapAccounts.DataSourceNameID.ID);
                cmbStrategy.DisplayMember = "Name";
                cmbStrategy.ValueMember = "StrategyID";
                Utils.UltraDropDownFilter(cmbStrategy, "Name");
                //_cashAccounts = CachedDataManager.GetInstance.GetAccountsAndAllocationRules();
                _accounts = GetAccountAndAllocationPrefTable(); //Gets accounts through new allocation manager proxy.
                cmbAccounts.DataSource = null;
                cmbAccounts.DataSource = _accounts; //CreateSourceColumns(_mapAccounts.CompanyNameID.ID, _mapAccounts.DataSourceNameID.ID);
                cmbAccounts.DisplayMember = OrderFields.PROPERTY_LEVEL1NAME;
                cmbAccounts.ValueMember = OrderFields.PROPERTY_LEVEL1ID;
                Utils.UltraDropDownFilter(cmbAccounts, OrderFields.PROPERTY_LEVEL1NAME);
                _counterParties = CreatePositionManager.GetCounterParties(userID);
                _counterParties.Insert(0, new CounterParty(0, ApplicationConstants.C_COMBO_SELECT));
                cmbCounterParty.DisplayMember = "Name";
                cmbCounterParty.ValueMember = "CounterPartyID";
                cmbCounterParty.DataSource = null;
                cmbCounterParty.DataSource = _counterParties; //CreateSourceColumns(_mapAccounts.CompanyNameID.ID, _mapAccounts.DataSourceNameID.ID);          
                Utils.UltraDropDownFilter(cmbCounterParty, "Name");
                cmbCounterParty.Text = ApplicationConstants.C_COMBO_SELECT;

                _venue = CreatePositionManager.GetVenuesbyUserID(userID);
                _venue.Insert(0, new Venue(0, ApplicationConstants.C_COMBO_SELECT));
                cmbVenue.DisplayMember = "Name";
                cmbVenue.ValueMember = "VenueID";
                cmbVenue.DataSource = null;
                cmbVenue.DataSource = _venue; //CreateSourceColumns(_mapAccounts.CompanyNameID.ID, _mapAccounts.DataSourceNameID.ID);
                Utils.UltraDropDownFilter(cmbVenue, "Name");

                _optionTypes = EnumHelper.ConvertEnumForBindingWitouthSelect(typeof(OptionType));
                cmbOptionType.DataSource = _optionTypes;
                cmbOptionType.DisplayMember = "DisplayText";
                cmbOptionType.ValueMember = "Value";
                //cmbOptionType.DataBind();
                Utils.UltraDropDownFilter(cmbOptionType, "DisplayText");
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
        /// Gets allocation accounts and preferences 
        /// </summary>
        /// <returns></returns>
        private DataTable GetAccountAndAllocationPrefTable()
        {
            try
            {
                DataTable accountsAndAllocationRules = new DataTable();
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1ID);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1NAME);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE);

                AccountCollection _userAccounts = CachedDataManager.GetInstance.GetUserAccounts();


                if (_userAccounts != null)
                {
                    foreach (Prana.BusinessObjects.Account userAccount in _userAccounts)
                    {
                        DataRow accountRow = accountsAndAllocationRules.NewRow();
                        accountRow[OrderFields.PROPERTY_LEVEL1ID] = userAccount.AccountID;
                        accountRow[OrderFields.PROPERTY_LEVEL1NAME] = userAccount.Name;
                        if (userAccount.AccountID != int.MinValue)
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = false;
                        }
                        else
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                        }
                        accountsAndAllocationRules.Rows.Add(accountRow);
                    }

                    // Prana-9688: If trade server hang at the very same moment when TT is opened, then below pref is not able to be fetched via proxy and gives error.
                    // which interrupts further flow and accounts are not binded which led to Object reference error while accessing value of binded accounts.
                    // So applied handling here not to bother the further executions.

                    try
                    {
                        //This method is returning preferences which are created from Edit Allocation preferences UI, so no need to add starts with check, PRANA-23524
                        Dictionary<int, string> preferences = _allocationProxy.InnerChannel.GetAllocationPreferences(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID,
                            AllocationSubModulePermission.IsLevelingPermitted,
                            AllocationSubModulePermission.IsProrataByNavPermitted);
                        if (preferences != null)
                        {
                            foreach (int prefId in preferences.Keys)
                            {
                                DataRow accountRow = accountsAndAllocationRules.NewRow();
                                accountRow[OrderFields.PROPERTY_LEVEL1ID] = prefId;
                                accountRow[OrderFields.PROPERTY_LEVEL1NAME] = preferences[prefId];
                                accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                                accountsAndAllocationRules.Rows.Add(accountRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                return accountsAndAllocationRules;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }

        }

        //private void cmbAccounts_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    int width = 120;
        //    int newWidth = 0;
        //    System.Drawing.Font font = cmbAccounts.Font;
        //    System.Drawing.Graphics graphics = cmbAccounts.CreateGraphics();
        //    cmbAccounts.DropDownWidth = 120;
        //    int vertScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
        //    foreach (DataRow dr in _accounts.Rows)
        //    {
        //        string account = dr[1].ToString();
        //        newWidth = (int)graphics.MeasureString(account, font).Width + (2 * vertScrollBarWidth);
        //        if (width < newWidth)
        //            width = newWidth;
        //    }
        //    cmbAccounts.DropDownWidth = width;
        //}

        //TODO


        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void btnClear_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //DialogResult result = MessageBox.Show("Choose yes to delete all the rows from the grid, Choose No to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        //        DialogResult result = MessageBox.Show("Choose yes to delete all the saved rows from the grid, Choose No to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        //        if (result == DialogResult.Yes)
        //        {
        //            //do
        //            //{
        //            //    if (!grdCreatePosition.Rows.Count.Equals(0))
        //            //        grdCreatePosition.Rows[0].Delete(false);
        //            //} while (!int.Equals(grdCreatePosition.Rows.Count, 1));

        //            //int indexOfRow = 0;
        //            //int counter = 0;
        //            //int[] arrIndexRowsToBeDeleted = new int[grdCreatePosition.Rows.Count];
        //            //foreach (UltraGridRow row in grdCreatePosition.Rows)
        //            //{
        //            //    //if (!((Guid)row.Cells[CAPTION_ID].Value).Equals(Guid.Empty))
        //            //    //{
        //            //    //    indexOfRow = grdCreatePosition.Rows.IndexOf(row);
        //            //    //    arrIndexRowsToBeDeleted[counter] = indexOfRow;
        //            //    //    counter++;
        //            //    //    //grdCreatePosition.Rows[0].Delete(false);    
        //            //    //}
        //            //    if (row.Activation.Equals(Activation.Disabled))
        //            //    {
        //            //        arrIndexRowsToBeDeleted[counter] = indexOfRow;
        //            //        counter++;
        //            //    }
        //            //    indexOfRow++;
        //            //}

        //            //int rowToBeDeleted = 0;
        //            //if (counter > 0)
        //            //{
        //            //    for (int i = (counter-1); i >= 0; i--)
        //            //    {
        //            //        rowToBeDeleted = arrIndexRowsToBeDeleted[i];
        //            //        grdCreatePosition.Rows[rowToBeDeleted].Delete(false);
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    MessageBox.Show(this, "No saved rows to delete !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            //}

        //            ClearSavedRows();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public void ClearSavedRows()
        {
            try
            {
                int rowsCount = grdCreatePosition.Rows.Count;
                //int counter = 0;
                Guid idToBeChecked = Guid.Empty;
                if (rowsCount > 0)
                {
                    for (int i = (rowsCount - 1); i >= 0; i--)
                    {
                        idToBeChecked = (Guid)((OTCPosition)(grdCreatePosition.Rows[i].ListObject)).ID;
                        if (grdCreatePosition.Rows[i].Activation.Equals(Activation.Disabled) && !idToBeChecked.Equals(Guid.Empty))
                        {
                            grdCreatePosition.Rows[i].Delete(false);
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
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<AllocationGroup> result = SaveCreateTransactions();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set
            {
                _allocationServices = value;

            }
        }

        public List<AllocationGroup> SaveCreateTransactions()
        {
            List<AllocationGroup> allocationGroupList = new List<AllocationGroup>();
            try
            {
                OTCPositionList currentPositions = CreateTransactions();

                bool isValidToSave = ValidateTradeForNavLock(currentPositions);
                if (isValidToSave)
                {
                    allocationGroupList = SaveifValid(currentPositions);
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

            return allocationGroupList;
        }


        /// <summary>
        /// Validate Trade For Nav Lock
        /// Omshiv,March 2014
        /// </summary>
        /// <param name="currentPositions"></param>
        /// <returns></returns>
        private bool ValidateTradeForNavLock(OTCPositionList currentPositions)
        {
            bool isValidToTrade = true;
            try
            {
                #region NAV lock validation - Created by Omshiv, MArch 2014

                //get IsNAVLockingEnabled or not from cache
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                    foreach (OTCPosition Position in currentPositions)
                    {
                        if (Position.AccountID != int.MinValue)
                        {
                            DateTime tradeDate = Position.OriginalPurchaseDate; //TODO Ask to rajat sir which date is suitable - omshiv
                            //if manual trade then get date from date control on TT

                            bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(Position.AccountID, tradeDate);
                            if (!isTradeAllowed)
                            {
                                MessageBox.Show("NAV is locked for selected account. You can not allow to trade on this trade date.", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isValidToTrade = false;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select account from Trade!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            isValidToTrade = false;
                            break;
                        }
                    }
                }

                #endregion

                if (isValidToTrade && CachedDataManager.GetInstance.NAVLockDate.HasValue)
                {
                    foreach (OTCPosition position in currentPositions)
                    {
                        bool isValidOriginalPurchaseDate = CachedDataManager.GetInstance.ValidateNAVLockDate(position.OriginalPurchaseDate);
                        bool isValidSettleMentDate = CachedDataManager.GetInstance.ValidateNAVLockDate(position.SettlementDate);
                        bool isValidTradeDate = CachedDataManager.GetInstance.ValidateNAVLockDate(position.StartDate);
                        if ((!isValidSettleMentDate || !isValidTradeDate) ||
                           ((position.SideTagValue.Equals("2") || position.SideTagValue.Equals("B") || position.SideTagValue.Equals("D")) && !isValidOriginalPurchaseDate))
                        {
                            MessageBox.Show("For some of the orders the date you’ve chosen to for this action precedes your NAV Lock date ("
                                + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isValidToTrade = false;
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
            return isValidToTrade;
        }

        public OTCPositionList CreateTransactions()
        {
            CreatePositionUserInterface currentCreatePositionUserInterface = new CreatePositionUserInterface();
            try
            {
                if (grdCreatePosition.DataSource != null)
                {
                    currentCreatePositionUserInterface = (CreatePositionUserInterface)grdCreatePosition.DataSource;
                    return currentCreatePositionUserInterface.OTCPositions;
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
            return null;
        }


        private List<AllocationGroup> SaveifValid(OTCPositionList currentPositions)
        {

            List<AllocationGroup> allocationGroupList = new List<AllocationGroup>();
            try
            {
                if (currentPositions.IsValid.Equals(true))
                {
                    int count = currentPositions.Count;
                    if (count > 0)
                    {

                        bool isValid = true;
                        int rowIndex = 1;
                        bool confirmationSave = false;

                        // By sandeep as on 07-Dec-2007
                        List<PositionMaster> positionMasterCollection = new List<PositionMaster>();

                        OTCPositionList positionsToBeSaved = new OTCPositionList();
                        foreach (OTCPosition position in currentPositions)
                        {
                            if (position.ID.Equals(Guid.Empty))
                            {
                                positionsToBeSaved.Add(position);
                            }
                        }

                        foreach (OTCPosition OTCposition in positionsToBeSaved)
                        {
                            if (OTCposition.ID.Equals(Guid.Empty))
                            {
                                OTCposition.IsManuallyCreatedPosition = true;
                                // By sandeep as on 07-Dec-2007
                                PositionMaster positionMaster = new PositionMaster();
                                positionMaster.UnderlyingSymbol = OTCposition.UnderlyingSymbol;

                                if (OTCposition.PositionStartQuantity <= 0)
                                {
                                    isValid = false;
                                    MessageBox.Show("Please fill the Quantity in row no. " + rowIndex);
                                    break;
                                }

                                // Added By Sandeep as on 12-Dec-2007
                                CreatePositionMasterObject(OTCposition, positionMaster);

                                if (rowIndex.Equals(positionsToBeSaved.Count))
                                {
                                    if (HideConfirmationMessage.Equals(false))
                                    {
                                        DialogResult dlgResult = MessageBox.Show("Do you want to save the transactions?", "Create Transactions Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (dlgResult.Equals(DialogResult.Yes))
                                        {
                                            foreach (OTCPosition positionToBeMarked in positionsToBeSaved)
                                            {
                                                positionToBeMarked.ID = Guid.NewGuid();
                                            }
                                            confirmationSave = true;
                                        }
                                        else if (dlgResult.Equals(DialogResult.No))
                                        {
                                            _isSavingCanceled = true;
                                        }
                                    }
                                    else
                                    {
                                        foreach (OTCPosition positionToBeMarked in positionsToBeSaved)
                                        {
                                            positionToBeMarked.ID = Guid.NewGuid();
                                        }
                                        confirmationSave = true;
                                    }
                                }
                                rowIndex++;
                                if (positionMaster.NirvanaProcessDate.Equals(DateTime.MinValue))
                                {
                                    positionMaster.NirvanaProcessDate = DateTime.Now;
                                }
                                positionMasterCollection.Add(positionMaster);

                            }
                        }

                        if (isValid && positionMasterCollection.Count > 0 && confirmationSave.Equals(true))
                        {
                            try
                            {
                                allocationGroupList = _allocationServices.InnerChannel.CreateAndSavePositions(positionMasterCollection);
                            }
                            catch
                            {
                                MessageBox.Show("TradeService not connected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return allocationGroupList;
                            }

                            if (allocationGroupList.Count > 0)
                            {
                                DisableSavedRows();
                            }
                        }
                        else if (isValid == true && positionMasterCollection.Count.Equals(0))
                        {
                            MessageBox.Show(this, "Nothing to save !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
                else
                {
                    MessageBox.Show(this, "Columns marked with (!) are mandatory. Please check them before saving!", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            HideConfirmationMessage = false;
            return allocationGroupList;
        }

        private void CreatePositionMasterObject(OTCPosition OTCposition, PositionMaster positionMaster)
        {
            try
            {
                positionMaster.AccountID = OTCposition.AccountID;
                positionMaster.AssetID = OTCposition.AssetID;
                positionMaster.AUECID = OTCposition.AUECID;
                positionMaster.Commission = OTCposition.Commission;
                positionMaster.SoftCommission = OTCposition.SoftCommission;
                positionMaster.CompanyID = _companyID;
                positionMaster.CostBasis = OTCposition.AveragePrice;
                positionMaster.CounterPartyID = OTCposition.CounterPartyID;
                positionMaster.CurrencyID = OTCposition.CurrencyID;
                // positionMaster.AccountName = AccountName;
                positionMaster.ExpiredTaxlotID = OTCposition.ExpiredTaxlotID;
                positionMaster.ExpiredQty = OTCposition.ExpiredQty;

                positionMaster.Symbol = OTCposition.Symbol;

                positionMaster.ExchangeID = OTCposition.ExchangeID;
                positionMaster.Fees = OTCposition.Fees;
                positionMaster.ClearingBrokerFee = OTCposition.ClearingBrokerFee;
                positionMaster.Multiplier = OTCposition.Multiplier;
                positionMaster.PutCall = OTCposition.Call_Put;
                positionMaster.StrikePrice = OTCposition.StrikePrice;
                positionMaster.NetPosition = OTCposition.PositionStartQuantity;
                positionMaster.PositionStartDate = Convert.ToString(OTCposition.StartDate.ToString());
                //string sidetagvalue =TagDatabaseManager.GetInstance.GetOrderSideValue(OTCposition.SideType.ToString()); // BB
                positionMaster.SideTagValue = OTCposition.SideTagValue;
                positionMaster.TradingAccountID = _pMTradingAccountID;
                positionMaster.UnderlyingID = OTCposition.UnderlyingID;
                positionMaster.UserID = _userID;
                positionMaster.VenueID = OTCposition.VenueID;
                positionMaster.StrategyID = OTCposition.StrategyID;
                positionMaster.AUECLocalDate = Convert.ToString(OTCposition.StartDate.ToString());
                positionMaster.PositionSettlementDate = Convert.ToString(OTCposition.SettlementDate.ToString());
                positionMaster.ProcessDate = OTCposition.ProcessDate.ToString();
                positionMaster.OriginalPurchaseDate = OTCposition.OriginalPurchaseDate.ToString();
                positionMaster.Description = OTCposition.Description;
                positionMaster.GroupID = Guid.NewGuid().ToString();
                positionMaster.PranaMsgType = OrderFields.PranaMsgTypes.CreatePosition;
                positionMaster.ExternalOrderID = OrderIDGenerator.GenerateExternalOrderID();
                positionMaster.TradeAttribute1 = OTCposition.TradeAttribute1;
                positionMaster.TradeAttribute2 = OTCposition.TradeAttribute2;
                positionMaster.TradeAttribute3 = OTCposition.TradeAttribute3;
                positionMaster.TradeAttribute4 = OTCposition.TradeAttribute4;
                positionMaster.TradeAttribute5 = OTCposition.TradeAttribute5;
                positionMaster.TradeAttribute6 = OTCposition.TradeAttribute6;
                positionMaster.SetTradeAttribute(OTCposition.GetTradeAttributesAsDict());

                positionMaster.StampDuty = OTCposition.StampDuty;
                positionMaster.TransactionLevy = OTCposition.TransactionLevy;
                positionMaster.ClearingFee = OTCposition.ClearingFee;
                positionMaster.TaxOnCommissions = OTCposition.TaxOnCommissions;
                positionMaster.MiscFees = OTCposition.MiscFees;
                positionMaster.SecFee = OTCposition.SecFee;
                positionMaster.OccFee = OTCposition.OccFee;
                positionMaster.OrfFee = OTCposition.OrfFee;
                positionMaster.FXRate = OTCposition.FXRate;
                positionMaster.FXConversionMethodOperator = OTCposition.FXConversionMethodOperator;
                positionMaster.TaxLotClosingId = OTCposition.TaxLotClosingId;
                positionMaster.PositionExpirationDate = Convert.ToString(OTCposition.ExpirationDate.ToString());
                positionMaster.AccruedInterest = OTCposition.AccruedInterest;

                positionMaster.AccrualBasis = OTCposition.AccrualBasis;
                positionMaster.BondType = OTCposition.BondType;
                positionMaster.Freq = OTCposition.Freq;
                positionMaster.FirstCouponDate = OTCposition.FirstCouponDate;
                positionMaster.MaturityDate = OTCposition.MaturityDate;
                positionMaster.IsZero = OTCposition.IsZero;
                if (isExerciseAssignManualStatus.ContainsKey(OTCposition.ExpiredTaxlotID))
                {
                    positionMaster.IsManualyExerciseAssign = isExerciseAssignManualStatus[OTCposition.ExpiredTaxlotID];
                }
                positionMaster.IssueDate = OTCposition.IssueDate;
                positionMaster.Coupon = OTCposition.Coupon;
                positionMaster.LeadCurrencyID = OTCposition.LeadCurrencyID;
                positionMaster.VsCurrencyID = OTCposition.VsCurrencyID;
                positionMaster.CommissionSource = OTCposition.CommissionSource;
                positionMaster.SoftCommissionSource = OTCposition.SoftCommissionSource;
                positionMaster.TransactionType = OTCposition.TransactionType;
                positionMaster.TransactionSource = OTCposition.TransactionSource;

                positionMaster.SettlementCurrencyID = OTCposition.SettlementCurrencyID;
                if (positionMaster.SettlementCurrencyID > 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(positionMaster.SettlementCurrencyID)
                    && positionMaster.SettlCurrencyName != CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID])
                {
                    positionMaster.SettlCurrencyName = CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID];
                }
                positionMaster.OptionPremiumAdjustment = OTCposition.OptionPremiumAdjustment;
                positionMaster.RoundLot = OTCposition.RoundLot;
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

        private void DisableSavedRows()
        {
            try
            {
                foreach (UltraGridRow row in grdCreatePosition.Rows)
                {
                    if (!((Guid)row.Cells[CAPTION_ID].Value).Equals(Guid.Empty))
                    {
                        row.Activation = Activation.Disabled;
                        row.Appearance.BackColor = Color.BlanchedAlmond;
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

        private void BindFxRateConvertor()
        {
            try
            {
                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {
                        _fxConversionMethodOperator.ValueListItems.Add(var.Value, var.DisplayText);
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

        private ValueList _commissionSources = new ValueList();
        private ValueList _softCommissionSources = new ValueList();
        private void BindCommissionSources()
        {
            UltraGridColumn colCommSource = _gridBandOTCPositions.Columns[COL_COMMISSIONSOURCE];
            List<EnumerationValue> commSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.CommisionSource));
            foreach (EnumerationValue var in commSource)
            {
                if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                {
                    _commissionSources.ValueListItems.Add(var.Value, var.DisplayText);
                }
            }
            colCommSource.ValueList = _commissionSources;
        }

        private void BindSoftCommissionSources()
        {
            UltraGridColumn colSoftCommSource = _gridBandOTCPositions.Columns[COL_SOFTCOMMISSIONSOURCE];
            List<EnumerationValue> softCommSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.CommisionSource));
            foreach (EnumerationValue var in softCommSource)
            {
                if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                {
                    _softCommissionSources.ValueListItems.Add(var.Value, var.DisplayText);
                }
            }
            colSoftCommSource.ValueList = _softCommissionSources;
        }

        private void BindifPayReceiveChanges()
        {
            try
            {
                List<EnumerationValue> ifPayReceiveChanges = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.PayReceiveChanges));
                foreach (EnumerationValue var in ifPayReceiveChanges)
                {
                    _ifPayReceiveChanges.ValueListItems.Add(var.Value, var.DisplayText);
                }

                _ifPayReceiveChanges.Appearance.BackColor = Color.White;
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
        private ValueList _fxConversionMethodOperator = new ValueList();
        private ValueList _ifPayReceiveChanges = new ValueList();

        private ValueList _positionType = new ValueList();

        private bool _quantityChanged = false;
        private bool _payReceiveChanged = false;


        private void grdCreatePosition_AfterCellUpdate(object sender, CellEventArgs e)
        {
            //string s = e.Cell.Value.ToString();

            int AssetID = int.Parse(e.Cell.Row.Cells[CAPTION_AssetID].Value.ToString());
            try
            {
                if (_updatedQuantityPayFiled.Equals(false))
                {
                    switch (e.Cell.Column.Key)
                    {
                        case CAPTION_PayReceive:
                            _payReceiveChanged = true;
                            // _quantityChanged = false;
                            if ((e.Cell.Row.Cells[CAPTION_IfPayReceiveChanges].Text).Equals(Prana.BusinessObjects.AppConstants.PayReceiveChanges.AdjustQty.ToString()))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            else
                            {
                                CalculateAveragePriceFromPayReceive();
                            }

                            break;
                        case CAPTION_PositionStartQuantity:
                            _quantityChanged = true;
                            OTCPosition otcPosition = (OTCPosition)e.Cell.Row.ListObject;
                            if (isExerciseAssignManualStatus.ContainsKey(otcPosition.ExpiredTaxlotID))
                                otcPosition.IsExerciseAssignManual = isExerciseAssignManualStatus[otcPosition.ExpiredTaxlotID];
                            otcPosition.OptionPremiumAdjustment = otcPosition.OptionPremiumAdjustmentUnit * otcPosition.PositionStartQuantity;
                            CalculatePayReceiveFromQuantity();
                            if (AssetID.Equals((int)AssetCategory.FixedIncome) || AssetID.Equals((int)AssetCategory.ConvertibleBond))
                            {
                                CalculateAccruedInterest();
                            }
                            //if (_startDateChanged && _averagePriceChanged && _symbolValid)
                            //{


                            //}
                            break;
                        case CAPTION_SideTypeID:
                            if (_quantityChanged.Equals(true))
                            {
                                CalculatePayReceiveFromQuantity();
                            }
                            else if (_payReceiveChanged.Equals(true))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            OTCPosition otcPosSide = (OTCPosition)e.Cell.Row.ListObject;
                            if (isExerciseAssignManualStatus.ContainsKey(otcPosSide.ExpiredTaxlotID))
                                otcPosSide.IsExerciseAssignManual = isExerciseAssignManualStatus[otcPosSide.ExpiredTaxlotID];
                            GetSettlementDate(otcPosSide);
                            string transactionType = grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].Value == null ? string.Empty : grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].Value.ToString();
                            if (transactionType != TradingTransactionType.Exercise.ToString()
                                && transactionType != TradingTransactionType.Assignment.ToString())
                            {
                                string transactionTypeBasedonSideTagValue = GetTransactionTypeBasedonSideTagValue(grdCreatePosition.ActiveRow.Cells[CAPTION_SideTypeID].Value.ToString());

                                grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].Value = transactionTypeBasedonSideTagValue;
                            }
                            break;

                        case CAPTION_AveragePrice:

                            if (_quantityChanged.Equals(true))
                            {
                                CalculatePayReceiveFromQuantity();
                            }
                            else if (_payReceiveChanged.Equals(true))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            UpdateFXRateColumn((OTCPosition)e.Cell.Row.ListObject);
                            if (AssetID.Equals((int)AssetCategory.FixedIncome) || AssetID.Equals((int)AssetCategory.ConvertibleBond))
                            {
                                CalculateAccruedInterest();
                            }
                            //if (_quantityChanged && _startDateChanged && _symbolValid)
                            //{

                            //}
                            break;
                        case OrderFields.PROPERTY_COMMISSION:
                            OTCPosition otPosCommission = (OTCPosition)e.Cell.Row.ListObject;
                            if (otPosCommission.Commission > 0.0)
                            {
                                otPosCommission.CommissionSource = CommisionSource.Manual;
                            }

                            // Modified by: Ankit Gupta
                            // On: March 05, 2012
                            // Pay/Receive column is reflected with all the commission and fees


                            if (_quantityChanged.Equals(true))
                            {
                                CalculatePayReceiveFromQuantity();
                            }
                            else if (_payReceiveChanged.Equals(true))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            break;
                        case OrderFields.PROPERTY_SOFTCOMMISSION:
                            OTCPosition otPosForSoftComm = (OTCPosition)e.Cell.Row.ListObject;
                            if (otPosForSoftComm.SoftCommission > 0.0)
                            {
                                otPosForSoftComm.SoftCommissionSource = CommisionSource.Manual;
                            }
                            if (_quantityChanged.Equals(true))
                            {
                                CalculatePayReceiveFromQuantity();
                            }
                            else if (_payReceiveChanged.Equals(true))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            break;
                        /// Rahul 20120305
                        /// Same code for all cases. Hence Commented.
                        case CAPTION_Fees:
                        case OrderFields.PROPERTY_STAMPDUTY:
                        case OrderFields.PROPERTY_TRANSACTIONLEVY:
                        case OrderFields.PROPERTY_CLEARINGFEE:
                        case OrderFields.PROPERTY_TAXONCOMMISSIONS:
                        case OrderFields.PROPERTY_MISCFEES:
                        case OrderFields.PROPERTY_SECFEE:
                        case OrderFields.PROPERTY_OCCFEE:
                        case OrderFields.PROPERTY_ORFFEE:
                        case OrderFields.PROPERTY_CLEARINGBROKERFEE:
                        //Added By : Manvendra P.
                        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3459
                        case OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT:
                        case CAPTION_Multiplier:
                            if (_quantityChanged.Equals(true))
                            {
                                CalculatePayReceiveFromQuantity();
                            }
                            else if (_payReceiveChanged.Equals(true))
                            {
                                CalculateQuantityFromPayReceive();
                            }
                            break;

                        case CAPTION_Symbol:
                            {
                                if (e.Cell.Text.Length > 100)
                                {
                                    e.Cell.CancelUpdate();
                                    MessageBox.Show("Symbol Length cannot be more than 100 characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //clear old details 
                                OTCPosition otcPosSymbol = (OTCPosition)e.Cell.Row.ListObject;
                                otcPosSymbol.AssetID = 0;
                                otcPosSymbol.UnderlyingID = 0;

                                // send request to secmaster
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(e.Cell.Value.ToString(), ApplicationConstants.PranaSymbology);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();
                                _securityMaster.SendRequest(reqObj);
                            }
                            break;

                        case COLUMN_UnderlyingSymbol:
                            {
                                if (e.Cell.Text.Length > 100)
                                {
                                    e.Cell.CancelUpdate();
                                    MessageBox.Show("Underlying Symbol Length cannot be more than 100 characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //clear old details 
                                OTCPosition otcPos = (OTCPosition)e.Cell.Row.ListObject;
                                otcPos.AssetID = 0;
                                otcPos.UnderlyingID = 0;
                                otcPos.IsUnderlyingValidated = false;
                                otcPos.UnderlyingAssetCategory = AssetCategory.None;
                                otcPos.StrikePriceMultiplier = 1;
                                otcPos.EsignalOptionRoot = string.Empty;
                                otcPos.BloombergOptionRoot = string.Empty;
                                otcPos.UnderlyingAUECID = 0;
                                // send request to secmaster
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(e.Cell.Value.ToString(), ApplicationConstants.PranaSymbology);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();
                                _securityMaster.SendRequest(reqObj);
                            }
                            break;

                        case COLUMN_OptionType:
                            GenerateSymbol();
                            break;

                        case COLUMN_ExpirationDate:
                            GenerateSymbol();
                            break;

                        case COLUMN_StrikePrice:
                            GenerateSymbol();
                            break;

                        case CAPTION_StartDate:
                            OTCPosition otcPosStartDate = (OTCPosition)e.Cell.Row.ListObject;
                            //Added By : Manvendra Prajapati
                            //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-9955
                            DateTime underlyingTradeDate = DateTimeConstants.MinValue;
                            if (!string.IsNullOrWhiteSpace(e.Cell.Text.ToString())
                        && DateTime.TryParse(e.Cell.Text.ToString(), out underlyingTradeDate)
                        && DateTime.Compare(underlyingTradeDate, DateTimeConstants.MinValue) > 0)
                            {
                                underlyingTradeDate = DateTime.Parse(e.Cell.Text.ToString());

                                DateTime optionTradeDate = DateTime.Parse(e.Cell.Row.Cells["ParentTradeDate"].Value.ToString());
                                int compareFlag = DateTime.Compare(underlyingTradeDate.Date, optionTradeDate.Date);
                                if (compareFlag < 0)
                                {
                                    MessageBox.Show(this, "Trade Date of underlying can not be less than trade date of respective derivative.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    otcPosStartDate.AUECLocalDate = (DateTime)e.Cell.OriginalValue;
                                    //Added to solve issue, PRANA-12127
                                    grdCreatePosition.AfterCellUpdate -= new CellEventHandler(grdCreatePosition_AfterCellUpdate);
                                    e.Cell.Value = e.Cell.OriginalValue;
                                    grdCreatePosition.AfterCellUpdate += new CellEventHandler(grdCreatePosition_AfterCellUpdate);
                                }
                            }
                            GetSettlementDate(otcPosStartDate);
                            if (AssetID.Equals((int)AssetCategory.FixedIncome) || AssetID.Equals((int)AssetCategory.ConvertibleBond))
                            {
                                CalculateAccruedInterest();
                            }
                            UltraGridCell processDateCell = e.Cell.Row.Cells["ProcessDate"];
                            processDateCell.Value = e.Cell.Value;
                            UltraGridCell OriginalPurchaseDateCell = e.Cell.Row.Cells["OriginalPurchaseDate"];
                            OriginalPurchaseDateCell.Value = e.Cell.Value;

                            break;

                        case CAPTION_SettlementDate:
                            if (AssetID.Equals((int)AssetCategory.FixedIncome) || AssetID.Equals((int)AssetCategory.ConvertibleBond))
                            {
                                CalculateAccruedInterest();
                            }
                            break;

                        case CAPTION_AccountID:

                            if (e.Cell.Value != null)
                            {
                                UltraGridCell strategyCell = e.Cell.Row.Cells[CAPTION_StrategyID];
                                DataRow[] rows = _accounts.Select(OrderFields.PROPERTY_LEVEL1ID + " = '" + e.Cell.Value + "'");
                                if (Convert.ToBoolean(rows[0][OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE].ToString()))
                                {

                                    strategyCell.Value = int.MinValue;
                                    strategyCell.Activation = Activation.NoEdit;
                                }
                                else
                                {
                                    strategyCell.Activation = Activation.AllowEdit;
                                }
                            }
                            break;

                        case COLUMN_IsOptionActivated:
                            OptionActivatedCheckChanged();
                            break;

                        case COLUMN_TransactionType:
                            CalculatePayReceiveFromQuantity();
                            break;

                        case COLUMN_CurrencyID:
                            if (AssetID == (int)AssetCategory.FX || AssetID == (int)AssetCategory.FXForward)
                            {
                                e.Cell.Row.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Value = e.Cell.Row.Cells[COLUMN_CurrencyID].Value;
                            }
                            break;
                        default:
                            break;
                    }
                }
                _updatedQuantityPayFiled = false;
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

        private string GetTransactionTypeBasedonSideTagValue(string sideTagValue)
        {
            string transactionTypeBasedOnSideTagValue = string.Empty;
            try
            {
                transactionTypeBasedOnSideTagValue = CachedDataManager.GetInstance.GetTransactionTypeAcronymByOrderSideTagValue(sideTagValue);
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
            return transactionTypeBasedOnSideTagValue;
        }

        private void GenerateSymbol()
        {
            try
            {
                if (bool.Parse(grdCreatePosition.ActiveRow.Cells["IsUnderlyingValidated"].Value.ToString()))
                {
                    OptionDetail optionDetail = new OptionDetail();
                    optionDetail.ExpirationDate = Convert.ToDateTime(grdCreatePosition.ActiveRow.Cells[COLUMN_ExpirationDate].Value);
                    optionDetail.StrikePrice = Convert.ToDouble(grdCreatePosition.ActiveRow.Cells[COLUMN_StrikePrice].Value);
                    optionDetail.UnderlyingSymbol = grdCreatePosition.ActiveRow.Cells[COLUMN_UnderlyingSymbol].Value.ToString();
                    optionDetail.Symbology = ApplicationConstants.PranaSymbology;
                    optionDetail.AssetCategory = (AssetCategory)grdCreatePosition.ActiveRow.Cells["UnderlyingAssetCategory"].Value;
                    optionDetail.AUECID = (int)grdCreatePosition.ActiveRow.Cells["UnderlyingAUECID"].Value;
                    optionDetail.OptionType = (OptionType)grdCreatePosition.ActiveRow.Cells[COLUMN_OptionType].Value;
                    optionDetail.StrikePriceMultiplier = Convert.ToDouble(grdCreatePosition.ActiveRow.Cells["StrikePriceMultiplier"].Value);
                    optionDetail.EsignalOptionRoot = grdCreatePosition.ActiveRow.Cells["EsignalOptionRoot"].Value.ToString();
                    optionDetail.BloombergOptionRoot = grdCreatePosition.ActiveRow.Cells["BloombergOptionRoot"].Value.ToString();

                    OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                    grdCreatePosition.ActiveRow.Cells[CAPTION_Symbol].Value = optionDetail.Symbol;

                    grdCreatePosition.ActiveRow.Update();

                    SymbolChanged();
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

        private void SymbolChanged()
        {
            //clear old details 
            OTCPosition otcPos = (OTCPosition)grdCreatePosition.ActiveRow.ListObject;
            otcPos.AssetID = 0;
            otcPos.UnderlyingID = 0;

            // send request to secmaster
            SecMasterRequestObj reqObj = new SecMasterRequestObj();
            reqObj.AddData(otcPos.Symbol, ApplicationConstants.PranaSymbology);
            reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
            reqObj.HashCode = this.GetHashCode();
            _securityMaster.SendRequest(reqObj);
        }

        private void CalculateAccruedInterest()
        {
            try
            {

                OTCPosition OtcPosition = (OTCPosition)grdCreatePosition.ActiveRow.ListObject;
                //condition from avg price is removed, as it calculates accrued interst as zero, if avg price is zero.
                if ((!OtcPosition.Symbol.Equals(string.Empty)) && OtcPosition.PositionStartQuantity != 0.0 && (OtcPosition.AssetID.Equals((int)AssetCategory.FixedIncome) || OtcPosition.AssetID.Equals((int)AssetCategory.ConvertibleBond))) //&& OtcPosition.AveragePrice != 0.0))
                {
                    PositionMaster positionMaster = new PositionMaster();
                    CreatePositionMasterObject(OtcPosition, positionMaster);
                    AllocationGroup allocationGroup = _allocationServices.InnerChannel.CreateAllcationGroupFromPositionMaster(positionMaster);

                    List<PranaBasicMessage> lstAllocationGroup = new List<PranaBasicMessage>();
                    lstAllocationGroup.Add(allocationGroup);

                    lstAllocationGroup = _cashManagementServices.InnerChannel.CalculateAccruedInterest(lstAllocationGroup);
                    foreach (PranaBasicMessage obj in lstAllocationGroup)
                    {
                        OtcPosition.AccruedInterest = obj.AccruedInterest;
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
        /// Handles the BeforeCellUpdate event of the grdCreatePosition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs"/> instance containing the event data.</param>
        private void grdCreatePosition_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {

        }

        // PMClientForms.CounterParty frmCounterParty = null;
        //Prana.BusinessObjects.CounterParty _

        //private Prana.BusinessObjects.CounterParty Prana.BusinessObjects.CounterParty;

        ///// <summary>
        ///// Gets or sets my property.
        ///// </summary>
        ///// <value>My property.</value>
        //public Prana.BusinessObjects.CounterParty MyProperty
        //{
        //    get { return Prana.BusinessObjects.CounterParty;}
        //    set { Prana.BusinessObjects.CounterParty = value;}
        //}

        //OTCPosition _activeRow = new OTCPosition();

        /// <summary>
        /// Handles the EditorButtonClick event of the ultraTextEditor1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinEditors.EditorButtonEventArgs"/> instance containing the event data.</param>
        private void ultraTextEditor1_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                //EditorWithText textEditor = e.Button.Editor as EditorWithText;
                //OTCPosition _activeRow = (e.Context as UltraGridCell).Row.ListObject as OTCPosition;


                //if (textEditor != null && textEditor.TextBox != null)
                //{
                //    if (frmCounterParty == null)
                //    {
                //        frmCounterParty = new PMClientForms.CounterParty(_userID);
                //        frmCounterParty.Owner = this.FindForm();
                //        frmCounterParty.ShowInTaskbar = true;
                //    }
                //    frmCounterParty.ShowDialog();
                //    frmCounterParty.Activate();
                //    //frmCounterParty

                //   // frmCounterParty.Disposed += new EventHandler(frmCounterParty_Disposed);
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
        /// Handles the Disposed event of the frmSendToDesk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //private void frmCounterParty_Disposed(object sender, EventArgs e)
        //{
        //    PMClientForms.CounterParty senderForm = sender as PMClientForms.CounterParty;
        //    Prana.BusinessObjects.CounterParty counterParty = senderForm.GetSelectedCounterParty;
        //    _activeRow.CounterPartyID = counterParty.CounterPartyID;
        //    _activeRow.CounterParty = counterParty.Name;
        //    ultraTextEditor1.DataBindings.Clear();
        //    ultraTextEditor1.DataBindings.Add(new Binding("Text", _activeRow, "CounterParty"));
        //    frmCounterParty = null;

        //}

        private void grdCreatePosition_CellListSelect(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case CAPTION_AccountID:
                        var x = ((EditorWithCombo)e.Cell.EditorResolved);
                        if (x.SelectedIndex < 0)
                            x.SelectedIndex = 1;
                        UpdateCommissionFeesColumns();
                        break;
                    default:
                        break;
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

        private void OptionActivatedCheckChanged()
        {
            try
            {
                if (bool.Parse(grdCreatePosition.ActiveRow.Cells[COLUMN_IsOptionActivated].Value.ToString()))
                {
                    grdCreatePosition.ActiveRow.Cells[CAPTION_Symbol].Activation = Activation.NoEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_OptionType].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_UnderlyingSymbol].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_ExpirationDate].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_StrikePrice].Activation = Activation.AllowEdit;
                }
                else
                {
                    grdCreatePosition.ActiveRow.Cells[CAPTION_Symbol].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_OptionType].Activation = Activation.NoEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_UnderlyingSymbol].Activation = Activation.NoEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_ExpirationDate].Activation = Activation.NoEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_StrikePrice].Activation = Activation.NoEdit;
                }
                grdCreatePosition.ActiveRow.Update();
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

        double payReceive = 0;
        double quantity = 0;
        bool _updatedQuantityPayFiled = false;

        double multiplier = 0;
        double avgPrice = 0;
        double commission = 0;
        double softCommission = 0;
        double fee = 0;
        double clearingBrokerFee = 0;
        double stampDuty = 0;
        double transactionLevy = 0;
        double clearingFee = 0;
        double taxOnCommissions = 0;
        double miscFees = 0;
        double secFee = 0;
        double occFee = 0;
        double orfFee = 0;
        double optionPremiumAdjustment = 0;
        private void CalculateAveragePriceFromPayReceive()
        {
            try
            {
                _updatedQuantityPayFiled = true;
                OTCPosition otcPosition = (OTCPosition)this.grdCreatePosition.ActiveRow.ListObject;
                payReceive = double.Parse(otcPosition.PayReceive.ToString());
                multiplier = double.Parse(otcPosition.Multiplier.ToString());
                quantity = double.Parse(otcPosition.PositionStartQuantity.ToString());
                commission = double.Parse(otcPosition.Commission.ToString());
                softCommission = double.Parse(otcPosition.SoftCommission.ToString());
                fee = double.Parse(otcPosition.Fees.ToString());
                clearingBrokerFee = double.Parse(otcPosition.ClearingBrokerFee.ToString());
                stampDuty = double.Parse(otcPosition.StampDuty.ToString());
                transactionLevy = double.Parse(otcPosition.TransactionLevy.ToString());
                clearingFee = double.Parse(otcPosition.ClearingFee.ToString());
                taxOnCommissions = double.Parse(otcPosition.TaxOnCommissions.ToString());
                miscFees = double.Parse(otcPosition.MiscFees.ToString());
                secFee = double.Parse(otcPosition.SecFee.ToString());
                occFee = double.Parse(otcPosition.OccFee.ToString());
                orfFee = double.Parse(otcPosition.OrfFee.ToString());
                optionPremiumAdjustment = double.Parse(otcPosition.OptionPremiumAdjustment.ToString());
                int sideMul = Calculations.GetSideMultilpier(otcPosition.SideTagValue);
                avgPrice = ((payReceive - (sideMul) * (commission + softCommission + fee + clearingBrokerFee + stampDuty + transactionLevy + clearingFee + taxOnCommissions + miscFees + secFee + occFee + orfFee + optionPremiumAdjustment)) / (multiplier * quantity));
                //if (otcPosition.AssetID.Equals((int)AssetCategory.FixedIncome) || otcPosition.AssetID.Equals((int)AssetCategory.ConvertibleBond))
                //{                    
                //    avgPrice = avgPrice * 100;
                //}
                grdCreatePosition.ActiveRow.Cells[CAPTION_AveragePrice].Value = avgPrice;
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

        private void CalculatePayReceiveFromQuantity()
        {
            try
            {
                _updatedQuantityPayFiled = true;
                OTCPosition otcPosition = (OTCPosition)this.grdCreatePosition.ActiveRow.ListObject;
                quantity = double.Parse(otcPosition.PositionStartQuantity.ToString());
                multiplier = double.Parse(otcPosition.Multiplier.ToString());
                avgPrice = double.Parse(otcPosition.AveragePrice.ToString());
                //if (otcPosition.AssetID.Equals((int)AssetCategory.FixedIncome) || otcPosition.AssetID.Equals((int)AssetCategory.ConvertibleBond))
                //{
                //    avgPrice = avgPrice / 100;
                //}
                commission = double.Parse(otcPosition.Commission.ToString());
                softCommission = double.Parse(otcPosition.SoftCommission.ToString());
                fee = double.Parse(otcPosition.Fees.ToString());
                clearingBrokerFee = double.Parse(otcPosition.ClearingBrokerFee.ToString());
                stampDuty = double.Parse(otcPosition.StampDuty.ToString());
                transactionLevy = double.Parse(otcPosition.TransactionLevy.ToString());
                clearingFee = double.Parse(otcPosition.ClearingFee.ToString());
                taxOnCommissions = double.Parse(otcPosition.TaxOnCommissions.ToString());
                miscFees = double.Parse(otcPosition.MiscFees.ToString());
                secFee = double.Parse(otcPosition.SecFee.ToString());
                occFee = double.Parse(otcPosition.OccFee.ToString());
                orfFee = double.Parse(otcPosition.OrfFee.ToString());
                int sideMul = Calculations.GetSideMultilpier(otcPosition.SideTagValue);
                //Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3459

                optionPremiumAdjustment = double.Parse(otcPosition.OptionPremiumAdjustment.ToString());
                payReceive = (multiplier * avgPrice * quantity) + (sideMul) * (commission + softCommission + fee + clearingBrokerFee + stampDuty + transactionLevy + clearingFee + taxOnCommissions + miscFees + secFee + occFee + orfFee + optionPremiumAdjustment);

                string tradingTransactionType = grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].Value.ToString();

                if (CheckTransactionType(tradingTransactionType))
                {
                    grdCreatePosition.ActiveRow.Cells[CAPTION_PayReceive].Value = 0;
                }
                else
                {
                    grdCreatePosition.ActiveRow.Cells[CAPTION_PayReceive].Value = payReceive;
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

        private bool CheckTransactionType(string tradingTransactionType)
        {
            try
            {
                if (
                    tradingTransactionType.Equals(TradingTransactionType.LongAddition.ToString()) ||
                    tradingTransactionType.Equals(TradingTransactionType.LongWithdrawal.ToString()) ||
                    tradingTransactionType.Equals(TradingTransactionType.ShortAddition.ToString()) ||
                    tradingTransactionType.Equals(TradingTransactionType.ShortWithdrawal.ToString())
                    )
                {
                    return true;
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
            return false;
        }

        private void CalculateQuantityFromPayReceive()
        {
            try
            {
                _updatedQuantityPayFiled = true;
                OTCPosition otcPosition = (OTCPosition)this.grdCreatePosition.ActiveRow.ListObject;
                payReceive = double.Parse(otcPosition.PayReceive.ToString());
                multiplier = double.Parse(otcPosition.Multiplier.ToString());

                avgPrice = double.Parse(otcPosition.AveragePrice.ToString());
                //if (otcPosition.AssetID.Equals((int)AssetCategory.FixedIncome) || otcPosition.AssetID.Equals((int)AssetCategory.ConvertibleBond))
                //{
                //    avgPrice = avgPrice / 100;
                //}
                commission = double.Parse(otcPosition.Commission.ToString());
                softCommission = double.Parse(otcPosition.SoftCommission.ToString());
                fee = double.Parse(otcPosition.Fees.ToString());
                clearingBrokerFee = double.Parse(otcPosition.ClearingBrokerFee.ToString());
                stampDuty = double.Parse(otcPosition.StampDuty.ToString());
                transactionLevy = double.Parse(otcPosition.TransactionLevy.ToString());
                clearingFee = double.Parse(otcPosition.ClearingFee.ToString());
                taxOnCommissions = double.Parse(otcPosition.TaxOnCommissions.ToString());
                miscFees = double.Parse(otcPosition.MiscFees.ToString());
                secFee = double.Parse(otcPosition.SecFee.ToString());
                occFee = double.Parse(otcPosition.OccFee.ToString());
                orfFee = double.Parse(otcPosition.OrfFee.ToString());
                int sideMul = Calculations.GetSideMultilpier(otcPosition.SideTagValue);
                //Added By : Manvendra P.
                // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3459
                optionPremiumAdjustment = double.Parse(otcPosition.OptionPremiumAdjustment.ToString());
                quantity = ((payReceive - (sideMul) * (commission + softCommission + fee + clearingBrokerFee + stampDuty + transactionLevy + clearingFee + taxOnCommissions + miscFees + secFee + occFee + orfFee + optionPremiumAdjustment)) / (multiplier * avgPrice));
                grdCreatePosition.ActiveRow.Cells[CAPTION_PositionStartQuantity].Value = quantity;
                if ((quantity.ToString().IndexOf('.')) > 0)
                {
                    MessageBox.Show("Quantity is in fraction,Please check.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

        public void Removerow()
        {
            try
            {
                if (grdCreatePosition.ActiveRow != null)
                {
                    if (!grdCreatePosition.ActiveRow.Activation.Equals(Activation.Disabled))
                    {
                        grdCreatePosition.ActiveRow.Delete(true);
                        if (grdCreatePosition.Rows.Count > 0)
                        {
                            grdCreatePosition.Rows[0].Activate();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Please select some unsaved row to delete!", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public bool HideConfirmationMessage { get; set; }
        public bool IsDataLeftToBeSaved
        {
            get
            {
                bool result = false;
                try
                {
                    HideConfirmationMessage = true;
                    CreatePositionUserInterface currentCreatePositionUserInterface = new CreatePositionUserInterface();
                    currentCreatePositionUserInterface = (CreatePositionUserInterface)grdCreatePosition.DataSource;
                    OTCPositionList currentPositions = currentCreatePositionUserInterface.OTCPositions;

                    foreach (OTCPosition position in currentPositions)
                    {
                        if (position.ID.Equals(Guid.Empty))
                        {
                            result = true;
                            break;
                        }
                    }
                    if (currentPositions.IsValid.Equals(false))
                    {
                        return result;
                    }
                    else
                    {
                        return result;
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
                return false;
            }
        }

        /// <summary>
        /// //Returns settlement date on basis of AUEC selection.
        /// </summary>
        private void GetSettlementDate(OTCPosition otcPos)
        {
            try
            {

                int auecSettlementPeriod = int.MinValue;
                int AssetID = otcPos.AssetID;

                int auecID = otcPos.AUECID;
                string sideText = otcPos.Side;
                if (sideText != "0" && auecID > 0 && AssetID > 0)
                {
                    string sideTagValue = otcPos.SideTagValue;
                    if (string.IsNullOrWhiteSpace(otcPos.SideTagValue))
                        sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);

                    if (AssetID.Equals((int)AssetCategory.FixedIncome) || AssetID.Equals((int)AssetCategory.ConvertibleBond))
                    {
                        OTCPosition OTCobject = (OTCPosition)grdCreatePosition.ActiveRow.ListObject;

                        auecSettlementPeriod = OTCobject.DaysToSettlement;
                    }

                    if (auecSettlementPeriod.Equals(int.MinValue))
                    {
                        auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                    }

                    DateTime tradeDate = DateTime.Parse(grdCreatePosition.ActiveRow.Cells[CAPTION_StartDate].Value.ToString());
                    if (auecSettlementPeriod == 0)
                    {
                        otcPos.SettlementDate = tradeDate;
                    }
                    else
                    {
                        otcPos.SettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID, true);
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
        /// //Returns trade date on basis of AUEC selection.
        /// </summary>
        private DateTime GetAUECTradeDate(int selectedAUECID)
        {
            return Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(selectedAUECID));
        }


        private void grdCreatePosition_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void UpdateCommissionFeesColumns()
        {
            grdCreatePosition.ActiveRow.Update();

            // Modified By: Ankit Gupta
            // On March 05, 2013
            // All the seven fields of Commission and fees have been made editable for Unallocated trades, so that these fees can be saved for the unallocated trades also.
            // Previously only commission was editable

            //int accountID = int.Parse(grdCreatePosition.ActiveRow.Cells[CAPTION_AccountID].Value.ToString());
            //if (accountID <= 0)
            //{
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_Commission].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_Fees].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_StampDuty].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TransactionLevy].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_ClearingFee].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TaxOnCommissions].Value = 0;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_MiscFees].Value = 0;
            //    //grdCreatePosition.ActiveRow.Cells[CAPTION_Commission].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_Fees].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_StampDuty].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TransactionLevy].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_ClearingFee].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TaxOnCommissions].Activation = Activation.Disabled;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_MiscFees].Activation = Activation.Disabled;

            //}
            //else
            //{
            //    //grdCreatePosition.ActiveRow.Cells[CAPTION_Commission].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_Fees].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_StampDuty].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TransactionLevy].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_ClearingFee].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_TaxOnCommissions].Activation = Activation.AllowEdit;
            //    grdCreatePosition.ActiveRow.Cells[CAPTION_MiscFees].Activation = Activation.AllowEdit;
            //}

            grdCreatePosition.ActiveRow.Cells[CAPTION_Fees].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_CLEARINGBROKERFEE].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_STAMPDUTY].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_TRANSACTIONLEVY].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_CLEARINGFEE].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_TAXONCOMMISSIONS].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_MISCFEES].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_COMMISSION].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_SOFTCOMMISSION].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_SECFEE].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_OCCFEE].Activation = Activation.AllowEdit;
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_ORFFEE].Activation = Activation.AllowEdit;
        }


        public void CleanCtrlCreatePosition()
        {
            //_createPositionUserInterface = new CreatePositionUserInterface();
            int rowsCount = grdCreatePosition.Rows.Count;
            if (rowsCount > 0)
            {
                for (int i = (rowsCount - 1); i >= 0; i--)
                {
                    grdCreatePosition.Rows[i].Activation = Activation.Disabled;
                    grdCreatePosition.Rows[i].Delete(false);

                }
            }
        }

        private void UpdateFXRateColumn(OTCPosition otcPosition)
        {
            try
            {
                if (otcPosition.AssetID == (int)AssetCategory.FX || otcPosition.AssetID == (int)AssetCategory.FXForward)
                {
                    if (otcPosition.LeadCurrencyID != _companyBaseCurrencyID)
                        otcPosition.FXRate = otcPosition.AveragePrice;
                    else
                        otcPosition.FXRate = Math.Round(otcPosition.AveragePrice != 0 ? 1 / otcPosition.AveragePrice : 0, 4);
                }
                else if (_companyBaseCurrencyID == otcPosition.CurrencyID)
                {
                    otcPosition.FXRate = 1;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_FxRateCalc].Activation = Activation.Disabled;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_FXRate].Activation = Activation.Disabled;
                    grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Activation = Activation.Disabled;
                }
                else
                {
                    switch (CachedDataManager.GetInstance.GetCurrencyText(otcPosition.CurrencyID))
                    {
                        case "GBP":
                        case "NZD":
                        case "AUD":
                        case "EUR":
                            otcPosition.FXConversionMethodOperator = Operator.M;
                            break;
                        default:
                            otcPosition.FXConversionMethodOperator = Operator.D;
                            break;

                    }
                    grdCreatePosition.ActiveRow.Cells[COLUMN_FxRateCalc].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[COLUMN_FXRate].Activation = Activation.AllowEdit;
                    grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].Activation = Activation.AllowEdit;
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

        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                // _securityMaster.SecMstrDataResponse += new SecMasterDataHandler(_securityMaster_SecMstrDataResponse);
            }
        }
        public bool _isSavingCanceled = false;
        public bool IsSavingCanceled
        {
            get
            {
                return _isSavingCanceled;
            }
            set
            {
                _isSavingCanceled = value;
            }
        }

        Dictionary<string, bool> isExerciseAssignManualStatus = new Dictionary<string, bool>();
        //Created by omshiv,on closing trade date of underlying should not be less than trade date of option
        private void grdCreatePosition_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(CAPTION_PositionStartQuantity) || e.Cell.Column.Key.Equals(CAPTION_SideTypeID))
                {
                    string taxlotId = e.Cell.Row.Cells[CAPTION_ExpiredTaxlotID].Value.ToString();

                    OTCPosition otcPosition = (OTCPosition)e.Cell.Row.ListObject;
                    switch (e.Cell.Column.Key)
                    {
                        case CAPTION_PositionStartQuantity:
                            isExerciseAssignManualStatus[taxlotId] = true;
                            break;
                        case CAPTION_SideTypeID:
                            isExerciseAssignManualStatus[taxlotId] = true;
                            break;
                    }
                }

                if (IsClosingTaxlots && e.Cell.Column.Key == "StartDate")
                {
                    //Raturi: process the input only if that is a valid date
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-7703
                    DateTime underlyingTradeDate = DateTimeConstants.MinValue;
                    if (string.IsNullOrWhiteSpace(e.Cell.Text.ToString())
                        || !DateTime.TryParse(e.Cell.Text.ToString(), out underlyingTradeDate)
                        || DateTime.Compare(underlyingTradeDate, DateTimeConstants.MinValue) <= 0)
                    {
                        return;
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

        public bool IsClosingTaxlots { get; set; }

        private void grdCreatePosition_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdCreatePosition);
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

        private void grdCreatePosition_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdCreatePosition_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            try
            {
                if (!e.Rows[0].Activation.Equals(Activation.Disabled))
                {
                    e.DisplayPromptMsg = false;
                    DialogResult result = MessageBox.Show("You have selected 1 row for deletion Choose Yes to delete the row or No to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult.No == result)
                        e.Cancel = true;
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

        private void SetupGridRowForOtcPos(TaxLot allocatedTrade, OTCPosition otcPos)
        {
            if (grdCreatePosition.ActiveRow == null)
            {
                return;
            }
            _tempDate = otcPos.StartDate;
            grdCreatePosition.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.DarkSlateGray;
            grdCreatePosition.ActiveRow.Cells[CAPTION_Symbol].SetValue(otcPos.Symbol, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_AccountID].SetValue(otcPos.AccountValue.ID, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_AveragePrice].SetValue(otcPos.AveragePrice, false);
            grdCreatePosition.ActiveRow.Cells[OrderFields.PROPERTY_MISCFEES].SetValue(otcPos.MiscFees, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_PositionStartQuantity].SetValue(otcPos.PositionStartQuantity, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_ExpiredTaxlotID].SetValue(otcPos.ExpiredTaxlotID, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_ExpirationQty].SetValue(otcPos.ExpiredQty, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_StartDate].Value = _tempDate;
            grdCreatePosition.ActiveRow.Cells[CAPTION_StrategyID].SetValue(otcPos.StrategyID, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_Strategy].SetValue(otcPos.Strategy, false);
            grdCreatePosition.ActiveRow.Cells["TaxlotClosingID"].SetValue(otcPos.TaxLotClosingId, false);
            grdCreatePosition.ActiveRow.Cells["OptionPremiumAdjustment"].SetValue(otcPos.OptionPremiumAdjustment, false);
            grdCreatePosition.ActiveRow.Cells[COLUMN_IsOptionActivated].SetValue(otcPos.IsOptionActivated, false);
            // Added By : Manvendra P.
            // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3666
            this.grdCreatePosition.ActiveRow.Cells["OptionPremiumAdjustment"].Activation = Activation.Disabled;

            otcPos.TransactionSource = TransactionSource.Closing;
            if (allocatedTrade != null)
            {
                allocatedTrade.TransactionSource = TransactionSource.Closing;
                allocatedTrade.TransactionSourceTag = (int)TransactionSource.Closing;
            }

            grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].SetValue(otcPos.TransactionType, false);

            grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionSource].SetValue(otcPos.TransactionSource, false);
            //   otcPos = _closingServices.InnerChannel.CreatePositionForPhysicalSettlement(otcPos, allocatedTrade.AssetCategoryValue, allocatedTrade.OrderSideTagValue, allocatedTrade.PutOrCall);
            grdCreatePosition.DisplayLayout.Bands[0].Columns[CAPTION_SideTypeID].Header.Caption = "Original Side";
            grdCreatePosition.DisplayLayout.Bands[0].Columns[CAPTION_SideTypeID].Width = 75;
            grdCreatePosition.ActiveRow.Cells[CAPTION_SideTypeID].SetValue(otcPos.SideTagValue, false);
            grdCreatePosition.ActiveRow.Cells[CAPTION_AssetID].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[CAPTION_UnderlyingID].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[CAPTION_Symbol].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[CAPTION_AccountID].Activation = Activation.NoEdit;
            //grdCreatePosition.ActiveRow.Cells[CAPTION_SideTypeID].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[CAPTION_AveragePrice].Activation = Activation.AllowEdit;
            //grdCreatePosition.ActiveRow.Cells[CAPTION_PositionStartQuantity].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[COLUMN_TransactionType].Activation = Activation.NoEdit;
            grdCreatePosition.ActiveRow.Cells[COLUMN_IsOptionActivated].Activation = Activation.NoEdit;
            grdCreatePosition.DisplayLayout.Bands[0].Override.SelectedRowAppearance.BackColor = Color.DarkSlateGray;
            UpdateCommissionFeesColumns();
        }

        private static void UpdateOtcPos(TaxLot allocatedTrade, OTCPosition otcPos)
        {
            otcPos.ExpiredQty = allocatedTrade.SettledQty;
            //Modified By faisal Shah
            //Needed to ommit Multiplier in case of FutureOptions
            if (allocatedTrade.AssetCategoryValue == AssetCategory.FutureOption)
            {
                otcPos.PositionStartQuantity = allocatedTrade.SettledQty;
            }
            else
            {
                otcPos.PositionStartQuantity = allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty;
            }
            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(allocatedTrade.AssetCategoryValue);
            switch (baseAssetCategory)
            {
                case AssetCategory.Option:
                    otcPos.AveragePrice = allocatedTrade.StrikePrice;
                    if (!allocatedTrade.IsExerciseAtZero)
                    {
                        switch (allocatedTrade.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Buy_Cover:
                            case FIXConstants.SIDE_Buy_Closed:
                                otcPos.OptionPremiumAdjustment = (allocatedTrade.AvgPrice * allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty) + (allocatedTrade.OpenTotalCommissionandFees * allocatedTrade.SettledQty / allocatedTrade.TaxLotQty);
                                otcPos.MiscFees = 0;
                                break;
                            default:
                                otcPos.OptionPremiumAdjustment = (-1) * (allocatedTrade.AvgPrice * allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty) + (allocatedTrade.OpenTotalCommissionandFees * allocatedTrade.SettledQty / allocatedTrade.TaxLotQty);
                                otcPos.MiscFees = 0;
                                break;
                        }
                        otcPos.OptionPremiumAdjustmentUnit = otcPos.OptionPremiumAdjustment / (allocatedTrade.ContractMultiplier * allocatedTrade.SettledQty);
                    }
                    break;
                default:
                    otcPos.AveragePrice = allocatedTrade.AvgPrice;
                    break;

            }
        }

        #region AutoFill Functionality

        private Dictionary<int, string> AccountCollectionDictionary()
        {
            Dictionary<int, string> accountCollection = new Dictionary<int, string>();
            try
            {
                foreach (DataRow dr in GetAccountAndAllocationPrefTable().Rows)
                {
                    accountCollection.Add(Convert.ToInt32(dr[OrderFields.PROPERTY_LEVEL1ID]), dr[OrderFields.PROPERTY_LEVEL1NAME].ToString());
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
            return accountCollection;
        }

        private ValueList GetAccountsValueList()
        {
            try
            {
                Dictionary<int, string> accountCollection = AccountCollectionDictionary();
                ValueList accountsValList = new ValueList();
                foreach (int accountID in accountCollection.Keys)
                {
                    accountsValList.ValueListItems.Add(accountID, accountCollection[accountID]);
                }
                return accountsValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }


        private Dictionary<int, string> CounterPartyCollectionDictionary()
        {
            Dictionary<int, string> counterPartyCollection = new Dictionary<int, string>();
            try
            {
                counterPartyCollection.Add(0, ApplicationConstants.C_COMBO_SELECT);
                BindingList<CounterParty> counterPartyList = CreatePositionManager.GetCounterParties(this._userID);
                for (int count = 0; count < counterPartyList.Count; count++)
                {
                    counterPartyCollection.Add(counterPartyList[count].CounterPartyID, counterPartyList[count].Name);
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
            return counterPartyCollection;
        }

        private ValueList GetCounterPartyValueList()
        {
            try
            {
                Dictionary<int, string> CounterPartyCollection = CounterPartyCollectionDictionary();
                ValueList counterPartyValList = new ValueList();
                foreach (int CounterPartyID in CounterPartyCollection.Keys)
                {
                    counterPartyValList.ValueListItems.Add(CounterPartyID, CounterPartyCollection[CounterPartyID]);
                }
                return counterPartyValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the venues value list.
        /// </summary>
        /// <returns></returns>
        private ValueList GetVenuesValueList()
        {
            ValueList venueValList = new ValueList();
            try
            {
                foreach (Venue venue in _venue)
                {
                    venueValList.ValueListItems.Add(venue.VenueID, venue.Name);
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
            return venueValList;
        }

        private void grdCreatePosition_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = true;
                e.RaiseErrorEvent = false;
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

        /// <summary>
        /// Calculates the pay receive,PRANA-32815
        /// </summary>
        internal void CalculatePayReceive()
        {
            try
            {
                if (_updatedQuantityPayFiled.Equals(false))
                {
                    _quantityChanged = true;
                    CalculatePayReceiveFromQuantity();
                    _updatedQuantityPayFiled = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
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
                if (gridName == "grdCreatePosition")
                {
                    exporter.Export(grdCreatePosition, filePath);
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
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(grdCreatePosition, exportFilePath);
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