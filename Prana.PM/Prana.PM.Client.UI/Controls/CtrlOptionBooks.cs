//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;
////using Windale.Options;
//using Prana.BusinessObjects;
//using Prana.PM.BLL;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;

//namespace Prana.PM.Client.UI.Controls
//{
//    public partial class CtrlOptionBooks : UserControl
//    {
//        public CtrlOptionBooks()
//        {
//            InitializeComponent();
//        }

//        UltraGridBand _gridBandOptionsBookPositions = null;

//        #region Grid Columns

//        const string CAPTION_ID = "ID";
//        const string CAPTION_StartTaxLotID = "StartTaxLotID";
//        const string CAPTION_Side = "Side";
//        const string CAPTION_ClosingQuantity = "ClosedQty";
//        const string CAPTION_OpenQuantity = "OpenQty";
//        const string CAPTION_StartDate = "StartDate";
//        const string CAPTION_LastActivityDate = "LastActivityDate";
//        const string CAPTION_PositionType = "PositionType";
//        const string CAPTION_Quantity = "Quantity";
//        const string CAPTION_PNL = "PNL";
//        const string CAPTION_AccountValue = "AccountValue";
//        const string CAPTION_Strategy = "StrategyID";
//        const string CAPTION_Symbol = "Symbol";
//        const string CAPTION_AveragePrice = "AveragePrice";
//        const string CAPTION_SymbolConvention = "SymbolConvention";
//        const string CAPTION_Description = "Description";
//        const string CAPTION_CounterPartyName = "CounterPartyName";
//        const string CAPTION_VenueName = "VenueName";
//        const string CAPTION_Multiplier = "Multiplier";
//        const string CAPTION_Commission = "Commission";
//        const string CAPTION_Fees = "Fees";
//        const string CAPTION_InstrumentType = "InstrumentType";
//        const string CAPTION_CurrencyName = "CurrencyName";
//        const string CAPTION_ExchangeName = "ExchangeName";
//        const string CAPTION_UnderlyingName = "UnderlyingName";
//        const string CAPTION_AssetName = "AssetName";
//        const string CAPTION_SymbolConventionID = "SymbolConventionID";
//        const string CAPTION_AccountID = "AccountID";
//        const string CAPTION_StrategyID = "StrategyID";
//        const string CAPTION_PositionStartQuantity = "PositionStartQuantity";
//        const string CAPTION_PositionTaxLots = "PositionTaxLots";
//        const string CAPTION_ToBeIncluded = "ToBeIncluded";
//        const string CAPTION_CounterPartyID = "CounterPartyID";

//        const string CAPTION_UnderLyingID = "UnderLyingID";
//        const string CAPTION_AssetID = "AssetID";
//        const string CAPTION_VenueID = "VenueID";
//        const string CAPTION_AUECID = "AUECID";

//        //Columns added after refactoring
//        const string CAPTION_RealizedPNL = "RealizedPNL";
//        const string CAPTION_NotionalValue = "NotionalValue";
//        const string CAPTION_RecordType = "RecordType";
//        const string CAPTION_CounterParty = "CounterParty";
//        const string CAPTION_Status = "Status";
//        const string CAPTION_Venue = "Venue";
//        const string CAPTION_EndDate = "EndDate";
//        const string CAPTION_CurrencyID = "CurrencyID";
//        const string CAPTION_PNLwhenTaxLotsPopulated = "PNLwhenTaxLotsPopulated";

//        const string CAPTION_MonthToDateRealizedProfit = "MonthToDateRealizedProfit";
//        const string CAPTION_MarkPriceForMonth = "MarkPriceForMonth";
//        const string CAPTION_ExchangeID = "ExchangeID";

//        #endregion

//        public void SetupControl(CompanyUser loginUser)
//        {
//            ExposurePnlCache.ExposurePnlCacheManager exInstance = ExposurePnlCache.ExposurePnlCacheManager.GetInstance();
//            ExposurePnlCache.ExposurePnlCacheBindableDictionary exCacheList = exInstance.PMAccountView;
//            double totalDelta = double.MinValue;
//            double refDelta = double.MinValue;
//           // Windale.Options.OptionsNET optNet = new OptionsNET();
//            foreach (ExposurePnlCacheItem item in exCacheList.Values)
//            {
//                //if (item.ConsolidationInfoType == Prana.BusinessObjects.AppConstants.ConsolidationInfoType.Position)
//                //{
//                    refDelta = item.Delta;
////                    optNet.BSDelta(Convert.ToDouble(item.AvgPrice), Convert.ToDouble(item.AvgPrice) + 20, 10.00, Convert.ToDouble(DateTime.Now), 20.00, 10.00, ref refDelta, ref refDelta);
//                    totalDelta += item.Delta;
//                //}
//            }
//            lblBookDeltaValue.Text = totalDelta.ToString();
//            OTCPositionList addPositionList = new OTCPositionList();
//            grdOptionsBook.DataSource = null;
//            grdOptionsBook.DataSource = addPositionList;
//        }

//        private void grdOptionsBook_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            UltraGridLayout gridLayout = grdOptionsBook.DisplayLayout;

//            // Set the appearance for template add-rows. 
//            e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.Black;
//            e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;

//            // Once  the user modifies the contents of a template add-row, it becomes
//            // an add-row and the AddRowAppearance gets applied to such rows.            
//            e.Layout.Override.AddRowAppearance.BackColor = Color.Black;
//            e.Layout.Override.AddRowAppearance.ForeColor = Color.White;

//            SetOptionsBookGridAppearanceAndLayout(ref gridLayout);
//            e.Layout.Bands[1].Hidden = true;

//            _gridBandOptionsBookPositions = grdOptionsBook.DisplayLayout.Bands[0];

//            UltraGridColumn colGUID = _gridBandOptionsBookPositions.Columns[CAPTION_ID];
//            colGUID.Hidden = true;

//            UltraGridColumn colStartTaxLotID = _gridBandOptionsBookPositions.Columns[CAPTION_StartTaxLotID];
//            colStartTaxLotID.Hidden = true;

//            UltraGridColumn colClosingQuantity = _gridBandOptionsBookPositions.Columns[CAPTION_ClosingQuantity];
//            colClosingQuantity.Hidden = true;

//            UltraGridColumn colOpenQuantity = _gridBandOptionsBookPositions.Columns[CAPTION_OpenQuantity];
//            colOpenQuantity.Hidden = true;

//            //UltraGridColumn colGeneratedPNL = _gridBandOptionsBookPositions.Columns[CAPTION_PNL]; BB 14th Jun
//            //colGeneratedPNL.Hidden = true; BB 14th Jun


//            //UltraGridColumn colPositionType = _gridBandAddPositions.Columns[CAPTION_PositionType];
//            //colPositionType.Hidden = true;


//            //UltraGridColumn colInstrumentType = _gridBandAddPositions.Columns[CAPTION_InstrumentType];
//            //colInstrumentType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            //colInstrumentType.Width = 45;
//            ////colInstrumentType.ValueList = cmbInstrumentType; BB
//            //colInstrumentType.Header.Caption = "Instrument Type";
//            //colInstrumentType.Header.VisiblePosition = 1;

//            //UltraGridColumn colToBeIncluded = _gridBandOptionsBookPositions.Columns[CAPTION_ToBeIncluded];BB
//            //colToBeIncluded.Width = 45;
//            //colToBeIncluded.Header.Caption = "Include";
//            //colToBeIncluded.Header.VisiblePosition = 1;
//            //colToBeIncluded.Hidden = true;

//            UltraGridColumn colSymbol = _gridBandOptionsBookPositions.Columns[CAPTION_Symbol];
//            colSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
//            colSymbol.CharacterCasing = CharacterCasing.Upper;
//            colSymbol.Width = 50;
//            colSymbol.Header.Caption = "Symbol";
//            colSymbol.Header.VisiblePosition = 2;

//            //UltraGridColumn colSymbolConvention = _gridBandAddPositions.Columns[CAPTION_SymbolConvention];
//            ////colSymbolConvention.Width = 60;
//            ////colSymbolConvention.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            ////colSymbolConvention.ValueList = cmbSymbolConvention;
//            ////colSymbolConvention.Header.Caption = "Symbol Convention";
//            ////colSymbolConvention.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            //colSymbolConvention.Hidden = true;

//            UltraGridColumn colSymbolConventionID = _gridBandOptionsBookPositions.Columns[CAPTION_SymbolConventionID];
//            colSymbolConventionID.Width = 110;
//            colSymbolConventionID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            //colSymbolConventionID.ValueList = cmbSymbolConvention; BB
//            colSymbolConventionID.Header.Caption = "Symbol Convention";
//            //colSymbolConvention.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colSymbolConventionID.Header.VisiblePosition = 3;
//            //colSymbolConventionID.Hidden = true;

//            UltraGridColumn colPositionType = _gridBandOptionsBookPositions.Columns[CAPTION_PositionType];
//            colPositionType.Width = 65;
//            colPositionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colPositionType.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colPositionType.ValueList = cmbPositionType;
//            colPositionType.Header.Caption = "Side";
//            colPositionType.Header.VisiblePosition = 4;



//            UltraGridColumn ColSide = _gridBandOptionsBookPositions.Columns[CAPTION_Side];
//            ColSide.Hidden = true;

//            UltraGridColumn colPositionStartQuantity = _gridBandOptionsBookPositions.Columns[CAPTION_PositionStartQuantity];
//            colPositionStartQuantity.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
//            colPositionStartQuantity.Width = 60;
//            colPositionStartQuantity.Header.Caption = "Quantity";
//            colPositionStartQuantity.Header.VisiblePosition = 5;

//            UltraGridColumn colAveragePrice = _gridBandOptionsBookPositions.Columns[CAPTION_AveragePrice];
//            colAveragePrice.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
//            colAveragePrice.Width = 60;
//            colAveragePrice.Header.Caption = "Average Price";
//            colAveragePrice.Header.VisiblePosition = 6;

//            UltraGridColumn colTradeDate = _gridBandOptionsBookPositions.Columns[CAPTION_StartDate];
//            colTradeDate.Width = 80;
//            colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
//            colTradeDate.MaskInput = "mm/dd/yyyy";
//            colTradeDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
//            colTradeDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
//            colTradeDate.Header.Caption = "Trade Date";
//            colTradeDate.Header.VisiblePosition = 7;
//            //colTradeDate.Hidden = true;

//            //UltraGridColumn colCounterParty = _gridBandOptionsBookPositions.Columns[CAPTION_CounterPartyName];BB
//            //colCounterParty.Width = 90;
//            //colCounterParty.Header.Caption = "CounterParty";
//            //colCounterParty.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            ////colCounterParty.ValueList = cmbCounterParty; BB
//            ////colCounterParty.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            ////colCounterParty.EditorControl = ultraTextEditor1;
//            //colCounterParty.Header.VisiblePosition = 8;
//            ////colCounterParty.Hidden = true;

//            //UltraGridColumn colVenue = _gridBandOptionsBookPositions.Columns[CAPTION_VenueName];BB
//            //colVenue.Width = 90;
//            //colVenue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//            ////colVenue.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            ////colVenue.ValueList = cmbVenue; BB
//            //colVenue.Header.Caption = "Venue";
//            //colVenue.Header.VisiblePosition = 9;
//            ////colVenue.Hidden = true;


//            UltraGridColumn colDescription = _gridBandOptionsBookPositions.Columns[CAPTION_Description];
//            colDescription.Width = 150;
//            colDescription.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
//            colDescription.Header.Caption = "Description";
//            colDescription.Header.VisiblePosition = 10;
//            //colDescription.Hidden = true;

//            UltraGridColumn colAccount = _gridBandOptionsBookPositions.Columns[CAPTION_AccountValue];
//            //colAccount.Width = 65;
//            //colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            //colAccount.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colAccount.ValueList = _accountsList;                       
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            //colAccount.Header.Caption = "Account";
//            colAccount.Hidden = true;

//            UltraGridColumn colAccountID = _gridBandOptionsBookPositions.Columns[CAPTION_AccountID];
//            colAccountID.Width = 70;
//            colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colAccountID.ValueList = _accountsList; BB
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colAccountID.Header.Caption = "Account";
//            colAccountID.Header.VisiblePosition = 11;
//            //colAccountID.Hidden = true;



//            UltraGridColumn colstrategy = _gridBandOptionsBookPositions.Columns[CAPTION_Strategy];
//            //colstrategy.Width = 70;
//            //colstrategy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//            //colstrategy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colstrategy.ValueList = cmbStrategy;
//            //colstrategy.Header.Caption = "Strategy";
//            colstrategy.Hidden = true;

//            UltraGridColumn colstrategyID = _gridBandOptionsBookPositions.Columns[CAPTION_StrategyID];
//            colstrategyID.Width = 80;
//            colstrategyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            //colstrategy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colstrategyID.ValueList = cmbStrategy; BB
//            colstrategyID.Header.Caption = "Strategy";
//            colstrategyID.Header.VisiblePosition = 12;
//            //colstrategyID.Hidden = true;

//            //After taking the latest changes on 22nd May, 07
//            UltraGridColumn colMultiplier = _gridBandOptionsBookPositions.Columns[CAPTION_Multiplier];
//            colMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
//            colMultiplier.Width = 60;
//            colMultiplier.Header.Caption = "Multiplier";
//            colMultiplier.Header.VisiblePosition = 13;
//            colMultiplier.Hidden = true;

//            UltraGridColumn colCommission = _gridBandOptionsBookPositions.Columns[CAPTION_Commission];
//            colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
//            colCommission.Width = 75;
//            colCommission.Header.Caption = "Commission";
//            colCommission.Header.VisiblePosition = 14;
//            //colCommission.Hidden = true;

//            UltraGridColumn colFees = _gridBandOptionsBookPositions.Columns[CAPTION_Fees];
//            colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
//            colFees.Width = 45;
//            colFees.Header.Caption = "Fees";
//            colFees.Header.VisiblePosition = 15;
//            //colFees.Hidden = true;

//            UltraGridColumn colAsset = _gridBandOptionsBookPositions.Columns[CAPTION_AssetName];
//            colAsset.Width = 75;
//            colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colAsset.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colAsset.ValueList = _assets;
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colAsset.Header.Caption = "Asset";
//            colAsset.Header.VisiblePosition = 16;

//            UltraGridColumn colUnderLying = _gridBandOptionsBookPositions.Columns[CAPTION_UnderlyingName];
//            colUnderLying.Width = 75;
//            colUnderLying.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colUnderLying.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colUnderLying.ValueList = _underLying; BB
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colUnderLying.Header.Caption = "UnderLying";
//            colUnderLying.Header.VisiblePosition = 17;

//            UltraGridColumn colExchange = _gridBandOptionsBookPositions.Columns[CAPTION_ExchangeName];
//            colExchange.Width = 65;
//            colExchange.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colExchange.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colExchange.ValueList = _exchanges; BB
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colExchange.Header.Caption = "Exchange";
//            colExchange.Header.VisiblePosition = 18;
//            //colExchange.Hidden = true;


//            UltraGridColumn colCurrency = _gridBandOptionsBookPositions.Columns[CAPTION_CurrencyName];
//            colCurrency.Width = 65;
//            colCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            colCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
//            //colCurrency.ValueList = _currencies; BB
//            //colAccount.DefaultCellValue = new Account();
//            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
//            colCurrency.Header.Caption = "Currency";
//            colCurrency.Header.VisiblePosition = 19;
//            //colCurrency.Hidden = true;

//            //UltraGridColumn colPositionStartQuantity = _gridBandAddPositions.Columns[CAPTION_PositionStartQuantity];
//            //colPositionStartQuantity.Width = 45;
//            //colPositionStartQuantity.Header.Caption = "PositionStartQuantity";
//            //colPositionStartQuantity.Header.VisiblePosition = 20;
//            //colPositionStartQuantity.Hidden = true;

//            UltraGridColumn colPositionTaxLots = _gridBandOptionsBookPositions.Columns[CAPTION_PositionTaxLots];
//            colPositionTaxLots.Width = 45;
//            colPositionTaxLots.Header.Caption = "PositionTaxLots";
//            colPositionTaxLots.Header.VisiblePosition = 21;
//            colPositionTaxLots.Hidden = true;



//            //UltraGridColumn colCounterPartyID = _gridBandOptionsBookPositions.Columns[CAPTION_CounterPartyID];BB
//            //colCounterPartyID.Width = 45;
//            //colCounterPartyID.Header.Caption = "CounterPartyID";
//            //colCounterPartyID.Header.VisiblePosition = 20;
//            //colCounterPartyID.Hidden = true;

//            UltraGridColumn ColAssetID = _gridBandOptionsBookPositions.Columns[CAPTION_AssetID];
//            ColAssetID.Hidden = true;
//            UltraGridColumn ColUnderLyingID = _gridBandOptionsBookPositions.Columns[CAPTION_UnderLyingID];
//            ColUnderLyingID.Hidden = true;
//            //UltraGridColumn ColVenueID = _gridBandOptionsBookPositions.Columns[CAPTION_VenueID]; BB
//            //ColVenueID.Hidden = true;
//            UltraGridColumn ColAUECID = _gridBandOptionsBookPositions.Columns[CAPTION_AUECID];
//            ColAUECID.Hidden = true;

//            //Columns added after refactoring
//            UltraGridColumn ColRealizedPNL = _gridBandOptionsBookPositions.Columns[CAPTION_RealizedPNL];
//            ColRealizedPNL.Hidden = true;
//            UltraGridColumn ColNotionalValue = _gridBandOptionsBookPositions.Columns[CAPTION_NotionalValue];
//            ColNotionalValue.Hidden = true;
//            UltraGridColumn ColRecordType = _gridBandOptionsBookPositions.Columns[CAPTION_RecordType];
//            ColRecordType.Hidden = true;
//            UltraGridColumn ColCounterParty = _gridBandOptionsBookPositions.Columns[CAPTION_CounterParty];
//            ColCounterParty.Hidden = true;
//            UltraGridColumn ColStatus = _gridBandOptionsBookPositions.Columns[CAPTION_Status];
//            ColStatus.Hidden = true;
//            UltraGridColumn ColInstrumentType = _gridBandOptionsBookPositions.Columns[CAPTION_InstrumentType];
//            ColInstrumentType.Hidden = true;
//            UltraGridColumn ColVenue = _gridBandOptionsBookPositions.Columns[CAPTION_Venue];
//            ColVenue.Hidden = true;
//            UltraGridColumn ColEndDate = _gridBandOptionsBookPositions.Columns[CAPTION_EndDate];
//            ColEndDate.Hidden = true;
//            UltraGridColumn ColCurrencyID = _gridBandOptionsBookPositions.Columns[CAPTION_CurrencyID];
//            ColCurrencyID.Hidden = true;
//            UltraGridColumn ColPNLwhenTaxLotsPopulated = _gridBandOptionsBookPositions.Columns[CAPTION_PNLwhenTaxLotsPopulated];
//            ColPNLwhenTaxLotsPopulated.Hidden = true;
//            UltraGridColumn ColMonthToDateRealizedProfit = _gridBandOptionsBookPositions.Columns[CAPTION_MonthToDateRealizedProfit];
//            ColMonthToDateRealizedProfit.Hidden = true;
//            UltraGridColumn ColMarkPriceForMonth = _gridBandOptionsBookPositions.Columns[CAPTION_MarkPriceForMonth];
//            ColMarkPriceForMonth.Hidden = true;
//            UltraGridColumn ColExchangeID = _gridBandOptionsBookPositions.Columns[CAPTION_ExchangeID];
//            ColExchangeID.Hidden = true;
//            UltraGridColumn colSymbolConvention = _gridBandOptionsBookPositions.Columns[CAPTION_SymbolConvention];
//            colSymbolConvention.Hidden = true;
//        }

//        /// <summary>
//        /// Sets the grid appearance and layout.
//        /// </summary>
//        /// <param name="gridLayout">The grid layout.</param>
//        private void SetOptionsBookGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
//        {

//            gridLayout.Appearance.BackColor = Color.Black; //BB
//            gridLayout.Override.RowAppearance.BackColor = Color.Black; //BB
//            //gridLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(64, 64, 64);

//            gridLayout.Override.SelectedRowAppearance.BackColor = Color.Black;
//            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
//            gridLayout.Override.SelectedRowAppearance.ForeColor = Color.White;
//            gridLayout.Override.ActiveRowAppearance.ForeColor = Color.White;
//            //gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
//            //gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
//            //gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
//            ////gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
//            ////gridLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
//            //gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
//            //gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
//            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
//            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
//            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

//            gridLayout.AutoFitStyle = AutoFitStyle.None;
//            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;

//            //gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
//            //gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
//            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
//            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
//            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

//            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
//            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;

//            //UltraGridLayout gridLayoutHeader = this.grdCreatePosition.DisplayLayout;
//            UltraGridBand band = this.grdOptionsBook.DisplayLayout.Bands[0];
//            gridLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

//            //band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
//            band.Override.HeaderAppearance.BackColor = Color.WhiteSmoke;
//            //band.Override.HeaderAppearance.BackColor2 = Color.DarkGray;
//            //band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;
//            band.Override.HeaderAppearance.ForeColor = Color.Black;
//            //band.Override.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

//            grdOptionsBook.DisplayLayout.Appearance.ForeColor = Color.White;
//        }

//        OTCPositionList _positions = new OTCPositionList();
//        public OTCPositionList Positions
//        {
//            set
//            {
//                _positions = value;
//                grdOptionsBook.DataSource = null;
//                grdOptionsBook.DataSource = _positions;
//            }
//        }
//    }
//}
