using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.Client.UI.Classes;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class PMTaxLotsDisplayForm : Form
    {
        const string _taxLotDisplayLayout = "TaxLotDisplayLayout";
        private const string SUB_MODULE_NAME = "CustomView";
        private string _filePath = PMPrefrenceManager.GetInstance(SUB_MODULE_NAME).GetPreferenceDirectory() + "\\" + _taxLotDisplayLayout;

        private string _CompressedRowID;

        public string CompressedRowID
        {
            get { return _CompressedRowID; }
            set { _CompressedRowID = value; }
        }

        PMAppearances _pmAppearances = null;


        public PMTaxLotsDisplayForm()
        {
            InitializeComponent();

        }





        public void SetInputDataSource(ExposurePnlCacheItemList taxlotList, CustomViewPreferences prefs)
        {
            _pmAppearances = PMAppearanceManager.PMAppearance;
            ultraGrid1.DataSource = taxlotList;

            if (System.IO.File.Exists(_filePath))
            {
                ultraGrid1.DisplayLayout.LoadFromXml(_filePath);

            }
            else if (prefs != null)
            {
                List<PreferenceGridColumn> listOfColumns = prefs.SelectedColumnsCollection;
                // ultraGrid1.DisplayLayout

                foreach (UltraGridColumn existingCol in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    existingCol.Hidden = true;
                }

                foreach (PreferenceGridColumn col in listOfColumns)
                {
                    if (ultraGrid1.DisplayLayout.Bands[0].Columns.Exists(col.Name))
                    {
                        ultraGrid1.DisplayLayout.Bands[0].Columns[col.Name].Hidden = false;
                    }
                }
            }
            else
            {
                foreach (UltraGridColumn existingCol in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    existingCol.Hidden = true;
                }


                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].Header.Caption = PMConstants.CAP_LeveragedFactor;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].Header.VisiblePosition = 42;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].CellAppearance.TextHAlign = HAlign.Right;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_LeveragedFactor].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LeveragedFactor);

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Header.Caption = PMConstants.CAP_SelectedFeedPrice;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Header.VisiblePosition = 0;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_SelectedFeedPrice);
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].AllowGroupBy = DefaultableBoolean.False;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Width = 80;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SelectedFeedPrice].CellAppearance.TextHAlign = HAlign.Right;

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Header.Caption = PMConstants.CAP_UnderlyingStockPrice;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Header.VisiblePosition = 1;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingStockPrice);
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].AllowGroupBy = DefaultableBoolean.False;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Width = 80;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingStockPrice].CellAppearance.TextHAlign = HAlign.Right;


                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Header.Caption = PMConstants.CAP_Quantity;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Header.VisiblePosition = 3;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Format = PMConstantsHelper.GetFormatStringByCaption(ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Header.Caption);
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Width = 80;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Quantity].CellAppearance.TextHAlign = HAlign.Right;

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Header.Caption = PMConstants.CAP_AvgPrice;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Header.VisiblePosition = 4;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AvgPrice);
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Width = 80;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AvgPrice].CellAppearance.TextHAlign = HAlign.Right;

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Header.Caption = PMConstants.CAP_TradeDate;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Header.VisiblePosition = 5;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Width = 100;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].CellAppearance.TextHAlign = HAlign.Right;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_TradeDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Header.Caption = PMConstants.CAP_AverageVolume20DayUnderlyingSymbol;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Header.VisiblePosition = 41;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AverageVolume20DayUnderlyingSymbol);

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].Header.Caption = PMConstants.CAP_SideName;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].Header.VisiblePosition = 6;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].Hidden = false;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_SideName].CellAppearance.TextHAlign = HAlign.Right;


                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].Header.Caption = PMConstants.CAP_Symbol;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].Header.VisiblePosition = 7;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].CellAppearance.FontData.Bold = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].Width = 80;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_Symbol].Hidden = false;

                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Header.Caption = PMConstants.CAP_UnderlyingValueForOptions;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Header.VisiblePosition = 8;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingValueForOptions);
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].AllowGroupBy = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[PMConstants.COL_UnderlyingValueForOptions].Hidden = false;

            }
            SetColumnCaption();
            SetGridFontSize();
            this.BringToFront();
        }

        private void SetColumnCaption()
        {
            foreach (UltraGridColumn col in ultraGrid1.DisplayLayout.Bands[0].Columns)
            {
                col.Header.Caption = PMConstantsHelper.GetCaptionByColumnName(col.Key.ToString());
            }
        }

        private void SetUIColumns()
        {
            UltraGridBand band = ultraGrid1.DisplayLayout.Bands[0];

            #region Visible Columns

            band.Override.HeaderClickAction = HeaderClickAction.SortMulti;

            band.Columns[PMConstants.COL_Symbol].Header.Caption = PMConstants.CAP_Symbol;
            band.Columns[PMConstants.COL_Symbol].Header.VisiblePosition = 7;
            band.Columns[PMConstants.COL_Symbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Symbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Symbol].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Symbol].Width = 80;
            band.Columns[PMConstants.COL_Symbol].Hidden = false;
            band.Columns[PMConstants.COL_Symbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_SelectedFeedPrice].Header.Caption = PMConstants.CAP_SelectedFeedPrice;
            band.Columns[PMConstants.COL_SelectedFeedPrice].Header.VisiblePosition = 0;
            band.Columns[PMConstants.COL_SelectedFeedPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_SelectedFeedPrice);
            band.Columns[PMConstants.COL_SelectedFeedPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SelectedFeedPrice].AllowGroupBy = DefaultableBoolean.False;
            band.Columns[PMConstants.COL_SelectedFeedPrice].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SelectedFeedPrice].Width = 80;
            band.Columns[PMConstants.COL_SelectedFeedPrice].Hidden = false;
            band.Columns[PMConstants.COL_SelectedFeedPrice].CellAppearance.TextHAlign = HAlign.Right;



            band.Columns[PMConstants.COL_UnderlyingStockPrice].Header.Caption = PMConstants.CAP_UnderlyingStockPrice;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].Header.VisiblePosition = 1;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingStockPrice);
            band.Columns[PMConstants.COL_UnderlyingStockPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].AllowGroupBy = DefaultableBoolean.False;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].Width = 80;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].Hidden = false;
            band.Columns[PMConstants.COL_UnderlyingStockPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UnderlyingValueForOptions].Header.Caption = PMConstants.CAP_UnderlyingValueForOptions;
            band.Columns[PMConstants.COL_UnderlyingValueForOptions].Header.VisiblePosition = 8;
            band.Columns[PMConstants.COL_UnderlyingValueForOptions].Hidden = false;
            band.Columns[PMConstants.COL_UnderlyingValueForOptions].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_UnderlyingValueForOptions);
            band.Columns[PMConstants.COL_UnderlyingValueForOptions].AllowGroupBy = DefaultableBoolean.True;

            band.Columns[PMConstants.COL_FullSecurityName].Header.Caption = PMConstants.CAP_FullSecurityName;
            band.Columns[PMConstants.COL_FullSecurityName].Header.VisiblePosition = 9;
            band.Columns[PMConstants.COL_FullSecurityName].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FullSecurityName].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FullSecurityName].Width = 80;
            band.Columns[PMConstants.COL_FullSecurityName].Hidden = false;
            band.Columns[PMConstants.COL_FullSecurityName].CellAppearance.TextHAlign = HAlign.Right;


            band.Columns[PMConstants.COL_SettlementDate].Header.Caption = PMConstants.CAP_SettlementDate;
            band.Columns[PMConstants.COL_SettlementDate].Header.VisiblePosition = 10;
            band.Columns[PMConstants.COL_SettlementDate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SettlementDate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SettlementDate].Width = 80;
            band.Columns[PMConstants.COL_SettlementDate].Hidden = false;
            band.Columns[PMConstants.COL_SettlementDate].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_SettlementDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

            band.Columns[PMConstants.COL_OrderSideTagValue].Header.Caption = PMConstants.CAP_OrderSideTagValue;
            band.Columns[PMConstants.COL_OrderSideTagValue].Header.VisiblePosition = 11;
            band.Columns[PMConstants.COL_OrderSideTagValue].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_OrderSideTagValue].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_OrderSideTagValue].Width = 80;
            band.Columns[PMConstants.COL_OrderSideTagValue].Hidden = false;
            band.Columns[PMConstants.COL_OrderSideTagValue].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[PMConstants.COL_OrderSideTagValue].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_TransactionSide].Hidden = true;
            band.Columns[PMConstants.COL_TransactionSide].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[PMConstants.COL_TransactionSide].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_TradeDayPnl].Header.Caption = PMConstants.CAP_TradeDayPnl;
            band.Columns[PMConstants.COL_TradeDayPnl].Header.VisiblePosition = 26;
            band.Columns[PMConstants.COL_TradeDayPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TradeDayPnl);
            band.Columns[PMConstants.COL_TradeDayPnl].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_TradeDayPnl].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_TradeDayPnl].Width = 80;
            band.Columns[PMConstants.COL_TradeDayPnl].Hidden = false;
            band.Columns[PMConstants.COL_TradeDayPnl].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_TradeDayPnl].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_FxDayPnl].Header.Caption = PMConstants.CAP_FxDayPnl;
            band.Columns[PMConstants.COL_FxDayPnl].Header.VisiblePosition = 27;
            band.Columns[PMConstants.COL_FxDayPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FxDayPnl);
            band.Columns[PMConstants.COL_FxDayPnl].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FxDayPnl].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FxDayPnl].Width = 80;
            band.Columns[PMConstants.COL_FxDayPnl].Hidden = false;
            band.Columns[PMConstants.COL_FxDayPnl].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_FxDayPnl].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_FxCostBasisPnl].Header.Caption = PMConstants.CAP_FxCostBasisPnl;
            band.Columns[PMConstants.COL_FxCostBasisPnl].Header.VisiblePosition = 28;
            band.Columns[PMConstants.COL_FxCostBasisPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FxCostBasisPnl);
            band.Columns[PMConstants.COL_FxCostBasisPnl].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FxCostBasisPnl].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FxCostBasisPnl].Width = 80;
            band.Columns[PMConstants.COL_FxCostBasisPnl].Hidden = false;
            band.Columns[PMConstants.COL_FxCostBasisPnl].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_FxCostBasisPnl].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_TradeCostBasisPnl].Header.Caption = PMConstants.CAP_TradeCostBasisPnl;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].Header.VisiblePosition = 29;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TradeCostBasisPnl);
            band.Columns[PMConstants.COL_TradeCostBasisPnl].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].Width = 80;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].Hidden = false;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_TradeCostBasisPnl].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_IDCOSymbol].Header.Caption = PMConstants.CAP_IDCOSymbol;
            band.Columns[PMConstants.COL_IDCOSymbol].Header.VisiblePosition = 30;
            band.Columns[PMConstants.COL_IDCOSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_IDCOSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_IDCOSymbol].Hidden = false;
            band.Columns[PMConstants.COL_IDCOSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_OSISymbol].Header.Caption = PMConstants.CAP_OSISymbol;
            band.Columns[PMConstants.COL_OSISymbol].Header.VisiblePosition = 31;
            band.Columns[PMConstants.COL_OSISymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_OSISymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_OSISymbol].Hidden = false;
            band.Columns[PMConstants.COL_OSISymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_SEDOLSymbol].Header.Caption = PMConstants.CAP_SEDOLSymbol;
            band.Columns[PMConstants.COL_SEDOLSymbol].Header.VisiblePosition = 32;
            band.Columns[PMConstants.COL_SEDOLSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SEDOLSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SEDOLSymbol].Hidden = false;
            band.Columns[PMConstants.COL_SEDOLSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CUSIPSymbol].Header.Caption = PMConstants.CAP_CUSIPSymbol;
            band.Columns[PMConstants.COL_CUSIPSymbol].Header.VisiblePosition = 33;
            band.Columns[PMConstants.COL_CUSIPSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CUSIPSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CUSIPSymbol].Hidden = false;
            band.Columns[PMConstants.COL_CUSIPSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_BloombergSymbol].Header.Caption = PMConstants.CAP_BloombergSymbol;
            band.Columns[PMConstants.COL_BloombergSymbol].Header.VisiblePosition = 34;
            band.Columns[PMConstants.COL_BloombergSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_BloombergSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_BloombergSymbol].Hidden = false;
            band.Columns[PMConstants.COL_BloombergSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ISINSymbol].Header.Caption = PMConstants.CAP_ISINSymbol;
            band.Columns[PMConstants.COL_ISINSymbol].Header.VisiblePosition = 35;
            band.Columns[PMConstants.COL_ISINSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ISINSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ISINSymbol].Hidden = false;
            band.Columns[PMConstants.COL_ISINSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ReutersSymbol].Header.Caption = PMConstants.CAP_ReutersSymbol;
            band.Columns[PMConstants.COL_ReutersSymbol].Header.VisiblePosition = 134;
            band.Columns[PMConstants.COL_ReutersSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ReutersSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ReutersSymbol].Hidden = false;
            band.Columns[PMConstants.COL_ReutersSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Header.Caption = PMConstants.CAP_AverageVolume20DayUnderlyingSymbol;
            band.Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Header.VisiblePosition = 41;
            band.Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_AverageVolume20DayUnderlyingSymbol].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AverageVolume20DayUnderlyingSymbol);

            band.Columns[PMConstants.COL_LeveragedFactor].Header.Caption = PMConstants.CAP_LeveragedFactor;
            band.Columns[PMConstants.COL_LeveragedFactor].Header.VisiblePosition = 42;
            band.Columns[PMConstants.COL_LeveragedFactor].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_LeveragedFactor].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_LeveragedFactor].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_LeveragedFactor].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LeveragedFactor);

            band.Columns[PMConstants.COL_Quantity].Header.Caption = PMConstants.CAP_Quantity;
            band.Columns[PMConstants.COL_Quantity].Header.VisiblePosition = 3;
            band.Columns[PMConstants.COL_Quantity].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Quantity].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Quantity].Format = PMConstantsHelper.GetFormatStringByCaption(band.Columns[PMConstants.COL_Quantity].Header.Caption);
            band.Columns[PMConstants.COL_Quantity].Width = 80;
            band.Columns[PMConstants.COL_Quantity].Hidden = false;
            band.Columns[PMConstants.COL_Quantity].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_AvgPrice].Header.Caption = PMConstants.CAP_AvgPrice;
            band.Columns[PMConstants.COL_AvgPrice].Header.VisiblePosition = 4;
            band.Columns[PMConstants.COL_AvgPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AvgPrice);
            band.Columns[PMConstants.COL_AvgPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_AvgPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_AvgPrice].Width = 80;
            band.Columns[PMConstants.COL_AvgPrice].Hidden = false;
            band.Columns[PMConstants.COL_AvgPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_AverageVolume20Day].Header.Caption = PMConstants.CAP_AverageVolume20Day;
            band.Columns[PMConstants.COL_AverageVolume20Day].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AverageVolume20Day);
            band.Columns[PMConstants.COL_AverageVolume20Day].Header.VisiblePosition = 54;
            band.Columns[PMConstants.COL_AverageVolume20Day].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_AverageVolume20Day].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_AverageVolume20Day].Width = 80;
            band.Columns[PMConstants.COL_AverageVolume20Day].Hidden = false;
            band.Columns[PMConstants.COL_AverageVolume20Day].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_AverageVolume20Day].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Header.Caption = PMConstants.CAP_PercentageAverageVolumeDeltaAdjusted;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageAverageVolumeDeltaAdjusted);
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Header.VisiblePosition = 55;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Width = 80;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].Hidden = false;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_PercentageAverageVolumeDeltaAdjusted].CellAppearance.TextHAlign = HAlign.Right;


            band.Columns[PMConstants.COL_SharesOutstanding].Header.Caption = PMConstants.CAP_SharesOutstanding;
            band.Columns[PMConstants.COL_SharesOutstanding].Header.VisiblePosition = 56;
            band.Columns[PMConstants.COL_SharesOutstanding].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SharesOutstanding].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SharesOutstanding].Width = 80;
            band.Columns[PMConstants.COL_SharesOutstanding].Hidden = true;
            band.Columns[PMConstants.COL_SharesOutstanding].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_SharesOutstanding].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MarketCapitalization].Header.Caption = PMConstants.CAP_MarketCapitalization;
            band.Columns[PMConstants.COL_MarketCapitalization].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketCapitalization);
            band.Columns[PMConstants.COL_MarketCapitalization].Header.VisiblePosition = 57;
            band.Columns[PMConstants.COL_MarketCapitalization].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MarketCapitalization].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MarketCapitalization].Width = 80;
            band.Columns[PMConstants.COL_MarketCapitalization].Hidden = true;
            band.Columns[PMConstants.COL_MarketCapitalization].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_MarketCapitalization].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageAverageVolume].Header.Caption = PMConstants.CAP_PercentageAverageVolume;
            band.Columns[PMConstants.COL_PercentageAverageVolume].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageAverageVolume);
            band.Columns[PMConstants.COL_PercentageAverageVolume].Header.VisiblePosition = 58;
            band.Columns[PMConstants.COL_PercentageAverageVolume].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageAverageVolume].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageAverageVolume].Width = 80;
            band.Columns[PMConstants.COL_PercentageAverageVolume].Hidden = false;
            band.Columns[PMConstants.COL_PercentageAverageVolume].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_PercentageAverageVolume].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_NetExposure].Header.Caption = PMConstants.CAP_NetExposure;
            band.Columns[PMConstants.COL_NetExposure].Header.VisiblePosition = 59;
            band.Columns[PMConstants.COL_NetExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetExposure);
            band.Columns[PMConstants.COL_NetExposure].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_NetExposure].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_NetExposure].Width = 100;
            band.Columns[PMConstants.COL_NetExposure].Hidden = false;
            band.Columns[PMConstants.COL_NetExposure].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DeltaAdjPosition].Header.Caption = PMConstants.CAP_DeltaAdjPosition;
            band.Columns[PMConstants.COL_DeltaAdjPosition].Header.VisiblePosition = 60;
            band.Columns[PMConstants.COL_DeltaAdjPosition].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DeltaAdjPosition);
            band.Columns[PMConstants.COL_DeltaAdjPosition].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DeltaAdjPosition].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DeltaAdjPosition].Width = 100;
            band.Columns[PMConstants.COL_DeltaAdjPosition].Hidden = false;
            band.Columns[PMConstants.COL_DeltaAdjPosition].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_DeltaAdjPosition].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DayPnL].Header.Caption = PMConstants.CAP_DayPnL;
            band.Columns[PMConstants.COL_DayPnL].Header.VisiblePosition = 61;
            band.Columns[PMConstants.COL_DayPnL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayPnL);
            band.Columns[PMConstants.COL_DayPnL].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DayPnL].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DayPnL].Width = 100;
            band.Columns[PMConstants.COL_DayPnL].Hidden = false;
            band.Columns[PMConstants.COL_DayPnL].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].Header.Caption = PMConstants.CAP_NetExposureInBaseCurrency;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].Header.VisiblePosition = 63;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_NetExposureInBaseCurrency);
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].Width = 100;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_NetExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_GrossExposure].Header.Caption = PMConstants.CAP_GrossExposure;
            band.Columns[PMConstants.COL_GrossExposure].Header.VisiblePosition = 64;
            band.Columns[PMConstants.COL_GrossExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GrossExposure);
            band.Columns[PMConstants.COL_GrossExposure].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_GrossExposure].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_GrossExposure].Width = 100;
            band.Columns[PMConstants.COL_GrossExposure].Hidden = false;
            band.Columns[PMConstants.COL_GrossExposure].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_GrossExposure].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].Header.Caption = PMConstants.CAP_DayPnLInBaseCurrency;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].Header.VisiblePosition = 65;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayPnLInBaseCurrency);
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].Width = 100;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_DayPnLInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_TradeDate].Header.Caption = PMConstants.CAP_TradeDate;
            band.Columns[PMConstants.COL_TradeDate].Header.VisiblePosition = 5;
            band.Columns[PMConstants.COL_TradeDate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_TradeDate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_TradeDate].Width = 100;
            band.Columns[PMConstants.COL_TradeDate].Hidden = false;
            band.Columns[PMConstants.COL_TradeDate].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_TradeDate].Format = DateTimeConstants.NirvanaDateTimeFormat;

            band.Columns[PMConstants.COL_ExDividendDate].Header.Caption = PMConstants.CAP_ExDividendDate;
            band.Columns[PMConstants.COL_ExDividendDate].Header.VisiblePosition = 150;
            band.Columns[PMConstants.COL_ExDividendDate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ExDividendDate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ExDividendDate].Width = 100;
            band.Columns[PMConstants.COL_ExDividendDate].Hidden = false;
            band.Columns[PMConstants.COL_ExDividendDate].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_ExDividendDate].Format = "G";

            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].Header.Caption = PMConstants.CAP_YesterdayMarkPriceStr;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].Header.VisiblePosition = 67;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarkPriceStr);
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].Width = 80;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].Hidden = false;
            band.Columns[PMConstants.COL_YesterdayMarkPriceStr].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Header.Caption = PMConstants.CAP_YesterdayUnderlyingMarkPriceStr;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Header.VisiblePosition = 68;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayUnderlyingMarkPriceStr);
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Width = 80;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].Hidden = false;
            band.Columns[PMConstants.COL_YesterdayUnderlyingMarkPriceStr].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_LastPrice].Header.Caption = PMConstants.CAP_LastPrice;
            band.Columns[PMConstants.COL_LastPrice].Header.VisiblePosition = 69;
            band.Columns[PMConstants.COL_LastPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_LastPrice);
            band.Columns[PMConstants.COL_LastPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_LastPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_LastPrice].Width = 80;
            band.Columns[PMConstants.COL_LastPrice].Hidden = false;
            band.Columns[PMConstants.COL_LastPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Delta].Header.Caption = PMConstants.CAP_Delta;
            band.Columns[PMConstants.COL_Delta].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Delta);
            band.Columns[PMConstants.COL_Delta].Header.VisiblePosition = 70;
            band.Columns[PMConstants.COL_Delta].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Delta].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Delta].Hidden = false;
            band.Columns[PMConstants.COL_Delta].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_FXRate].Header.Caption = PMConstants.CAP_FXRate;
            band.Columns[PMConstants.COL_FXRate].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRate);
            band.Columns[PMConstants.COL_FXRate].Header.VisiblePosition = 71;
            band.Columns[PMConstants.COL_FXRate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FXRate].Hidden = false;
            band.Columns[PMConstants.COL_FXRate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FXRate].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_YesterdayFXRate].Header.Caption = PMConstants.CAP_YesterdayFXRate;
            band.Columns[PMConstants.COL_YesterdayFXRate].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayFXRate);
            band.Columns[PMConstants.COL_YesterdayFXRate].Header.VisiblePosition = 132;
            band.Columns[PMConstants.COL_YesterdayFXRate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_YesterdayFXRate].Hidden = false;
            band.Columns[PMConstants.COL_YesterdayFXRate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_YesterdayFXRate].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_VsCurrencySymbol].Header.Caption = PMConstants.CAP_VsCurrencySymbol;
            band.Columns[PMConstants.COL_VsCurrencySymbol].Header.VisiblePosition = 72;
            band.Columns[PMConstants.COL_VsCurrencySymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_VsCurrencySymbol].Hidden = false;
            band.Columns[PMConstants.COL_VsCurrencySymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_VsCurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_LeadCurrencySymbol].Header.Caption = PMConstants.CAP_LeadCurrencySymbol;
            band.Columns[PMConstants.COL_LeadCurrencySymbol].Header.VisiblePosition = 73;
            band.Columns[PMConstants.COL_LeadCurrencySymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_LeadCurrencySymbol].Hidden = false;
            band.Columns[PMConstants.COL_LeadCurrencySymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_LeadCurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].Header.Caption = Prana.Global.OrderFields.CAPTION_LEVEL1NAME;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].Header.VisiblePosition = 74;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].CellActivation = Activation.NoEdit;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].Width = 80;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].Hidden = false;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL1NAME].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].Header.Caption = Prana.Global.OrderFields.CAPTION_LEVEL2NAME;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].Header.VisiblePosition = 75;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].CellActivation = Activation.NoEdit;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].Width = 80;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].Hidden = false;
            band.Columns[Prana.Global.OrderFields.PROPERTY_LEVEL2NAME].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Multiplier].Header.Caption = PMConstants.CAP_Multiplier;
            band.Columns[PMConstants.COL_Multiplier].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Multiplier);
            band.Columns[PMConstants.COL_Multiplier].Header.VisiblePosition = 76;
            band.Columns[PMConstants.COL_Multiplier].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Multiplier].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Multiplier].Hidden = false;
            band.Columns[PMConstants.COL_Multiplier].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CurrencySymbol].Header.Caption = PMConstants.CAP_CurrencySymbol;
            band.Columns[PMConstants.COL_CurrencySymbol].Header.VisiblePosition = 77;
            band.Columns[PMConstants.COL_CurrencySymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CurrencySymbol].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CurrencySymbol].Width = 120;
            band.Columns[PMConstants.COL_CurrencySymbol].Hidden = true;
            band.Columns[PMConstants.COL_CurrencySymbol].GroupByMode = GroupByMode.Text;
            band.Columns[PMConstants.COL_CurrencySymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UserName].Header.Caption = PMConstants.CAP_UserName;
            band.Columns[PMConstants.COL_UserName].Header.VisiblePosition = 78;
            band.Columns[PMConstants.COL_UserName].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UserName].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UserName].Width = 80;
            band.Columns[PMConstants.COL_UserName].Hidden = false;
            band.Columns[PMConstants.COL_UserName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_UserName].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CounterPartyName].Header.Caption = PMConstants.CAP_CounterPartyName;
            band.Columns[PMConstants.COL_CounterPartyName].Header.VisiblePosition = 79;
            band.Columns[PMConstants.COL_CounterPartyName].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CounterPartyName].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CounterPartyName].Width = 80;
            band.Columns[PMConstants.COL_CounterPartyName].Hidden = false;
            band.Columns[PMConstants.COL_CounterPartyName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_CounterPartyName].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ContractType].Header.Caption = PMConstants.CAP_ContractType;
            band.Columns[PMConstants.COL_ContractType].Header.VisiblePosition = 80;
            band.Columns[PMConstants.COL_ContractType].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ContractType].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ContractType].Width = 80;
            band.Columns[PMConstants.COL_ContractType].Hidden = false;
            band.Columns[PMConstants.COL_ContractType].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[PMConstants.COL_ContractType].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_StrikePrice].Header.Caption = PMConstants.CAP_StrikePrice;
            band.Columns[PMConstants.COL_StrikePrice].Header.VisiblePosition = 81;
            band.Columns[PMConstants.COL_StrikePrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_StrikePrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_StrikePrice);
            band.Columns[PMConstants.COL_StrikePrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_StrikePrice].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_StrikePrice].Width = 80;
            band.Columns[PMConstants.COL_StrikePrice].Hidden = false;
            band.Columns[PMConstants.COL_StrikePrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ExpirationDate].Header.Caption = PMConstants.CAP_ExpirationDate;
            band.Columns[PMConstants.COL_ExpirationDate].Header.VisiblePosition = 82;
            band.Columns[PMConstants.COL_ExpirationDate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ExpirationDate].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ExpirationDate].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ExpirationDate].Width = 80;
            band.Columns[PMConstants.COL_ExpirationDate].Hidden = false;
            band.Columns[PMConstants.COL_ExpirationDate].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_ExpirationDate].NullText = ApplicationConstants.C_NotAvailable;

            band.Columns[PMConstants.COL_SideName].Header.Caption = PMConstants.CAP_SideName;
            band.Columns[PMConstants.COL_SideName].Header.VisiblePosition = 6;
            band.Columns[PMConstants.COL_SideName].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SideName].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SideName].Hidden = true;
            band.Columns[PMConstants.COL_SideName].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Asset].Header.Caption = PMConstants.CAP_Asset;
            band.Columns[PMConstants.COL_Asset].Header.VisiblePosition = 83;
            band.Columns[PMConstants.COL_Asset].CellActivation = Activation.NoEdit;
            //band.Columns[PMConstants.COL_Asset].Header.Appearance = headerAppearance;
            band.Columns[PMConstants.COL_Asset].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Asset].Width = 120;
            band.Columns[PMConstants.COL_Asset].Hidden = true;
            band.Columns[PMConstants.COL_Asset].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Underlying].Header.Caption = PMConstants.CAP_Underlying;
            band.Columns[PMConstants.COL_Underlying].Header.VisiblePosition = 84;
            band.Columns[PMConstants.COL_Underlying].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Underlying].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Underlying].Width = 120;
            band.Columns[PMConstants.COL_Underlying].Hidden = true;
            band.Columns[PMConstants.COL_Underlying].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Exchange].Header.Caption = PMConstants.CAP_Exchange;
            band.Columns[PMConstants.COL_Exchange].Header.VisiblePosition = 85;
            band.Columns[PMConstants.COL_Exchange].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Exchange].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Exchange].Width = 120;
            band.Columns[PMConstants.COL_Exchange].Hidden = true;
            band.Columns[PMConstants.COL_Exchange].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ClosingPrice].Header.Caption = PMConstants.CAP_ClosingPrice;
            band.Columns[PMConstants.COL_ClosingPrice].Header.VisiblePosition = 86;
            band.Columns[PMConstants.COL_ClosingPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ClosingPrice);
            band.Columns[PMConstants.COL_ClosingPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ClosingPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ClosingPrice].Hidden = true;
            band.Columns[PMConstants.COL_ClosingPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DividendYield].Header.Caption = PMConstants.CAP_DividendYield;
            band.Columns[PMConstants.COL_DividendYield].Header.VisiblePosition = 87;
            band.Columns[PMConstants.COL_DividendYield].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DividendYield);
            band.Columns[PMConstants.COL_DividendYield].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DividendYield].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DividendYield].Hidden = true;
            band.Columns[PMConstants.COL_DividendYield].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_BidPrice].Header.Caption = PMConstants.CAP_BidPrice;
            band.Columns[PMConstants.COL_BidPrice].Header.VisiblePosition = 88;
            band.Columns[PMConstants.COL_BidPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BidPrice);
            band.Columns[PMConstants.COL_BidPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_BidPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_BidPrice].Hidden = true;
            band.Columns[PMConstants.COL_BidPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MidPrice].Header.Caption = PMConstants.CAP_MidPrice;
            band.Columns[PMConstants.COL_MidPrice].Header.VisiblePosition = 89;
            band.Columns[PMConstants.COL_MidPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MidPrice);
            band.Columns[PMConstants.COL_MidPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MidPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MidPrice].Hidden = true;
            band.Columns[PMConstants.COL_MidPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_AskPrice].Header.Caption = PMConstants.CAP_AskPrice;
            band.Columns[PMConstants.COL_AskPrice].Header.VisiblePosition = 90;
            band.Columns[PMConstants.COL_AskPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_AskPrice);
            band.Columns[PMConstants.COL_AskPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_AskPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_AskPrice].Hidden = true;
            band.Columns[PMConstants.COL_AskPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UnderlyingSymbol].Header.Caption = PMConstants.CAP_UnderlyingSymbol;
            band.Columns[PMConstants.COL_UnderlyingSymbol].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UnderlyingSymbol].Header.VisiblePosition = 91;
            band.Columns[PMConstants.COL_UnderlyingSymbol].Hidden = true;
            band.Columns[PMConstants.COL_UnderlyingSymbol].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DataSourceNameIDValue].Header.Caption = PMConstants.CAP_DataSourceNameIDValue;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].Header.VisiblePosition = 92;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].Width = 100;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].Hidden = false;
            band.Columns[PMConstants.COL_DataSourceNameIDValue].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].Header.Caption = PMConstants.CAP_FXRateOnTradeDateStr;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRateOnTradeDateStr);
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].Header.VisiblePosition = 93;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].Width = 80;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].Hidden = true;
            band.Columns[PMConstants.COL_FXRateOnTradeDateStr].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].Header.Caption = PMConstants.CAP_CostBasisUnrealizedPnL;
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].Header.VisiblePosition = 94;
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CostBasisUnrealizedPnL);
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].Hidden = false;
            band.Columns[PMConstants.COL_CostBasisUnRealizedPNL].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Header.Caption = PMConstants.CAP_CostBasisUnrealizedPnLInBaseCurrency;
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Header.VisiblePosition = 95;
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CostBasisUnrealizedPnLInBaseCurrency);
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_CostBasisUnrealizedPnLInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_EarnedDividendLocal].Header.Caption = PMConstants.CAP_EarnedDividendLocal;
            band.Columns[PMConstants.COL_EarnedDividendLocal].Header.VisiblePosition = 96;
            band.Columns[PMConstants.COL_EarnedDividendLocal].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_EarnedDividendLocal);
            band.Columns[PMConstants.COL_EarnedDividendLocal].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_EarnedDividendLocal].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_EarnedDividendLocal].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_EarnedDividendLocal].Width = 80;
            band.Columns[PMConstants.COL_EarnedDividendLocal].Hidden = false;
            band.Columns[PMConstants.COL_EarnedDividendLocal].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_EarnedDividendBase].Header.Caption = PMConstants.CAP_EarnedDividendBase;
            band.Columns[PMConstants.COL_EarnedDividendBase].Header.VisiblePosition = 97;
            band.Columns[PMConstants.COL_EarnedDividendBase].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_EarnedDividendBase);
            band.Columns[PMConstants.COL_EarnedDividendBase].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_EarnedDividendBase].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_EarnedDividendBase].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_EarnedDividendBase].Width = 80;
            band.Columns[PMConstants.COL_EarnedDividendBase].Hidden = false;
            band.Columns[PMConstants.COL_EarnedDividendBase].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_StartTradeDate].Header.Caption = PMConstants.CAP_StartTradeDate;
            band.Columns[PMConstants.COL_StartTradeDate].Header.VisiblePosition = 100;
            band.Columns[PMConstants.COL_StartTradeDate].Hidden = false;
            band.Columns[PMConstants.COL_StartTradeDate].Format = DateTimeConstants.DateFormat;
            band.Columns[PMConstants.COL_StartTradeDate].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_StartTradeDate].AllowGroupBy = DefaultableBoolean.True;

            band.Columns[PMConstants.COL_CostBasisBreakEven].Header.Caption = PMConstants.CAP_CostBasisBreakEven;
            band.Columns[PMConstants.COL_CostBasisBreakEven].Header.VisiblePosition = 101;
            band.Columns[PMConstants.COL_CostBasisBreakEven].Hidden = false;
            band.Columns[PMConstants.COL_CostBasisBreakEven].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CostBasisBreakEven);
            band.Columns[PMConstants.COL_CostBasisBreakEven].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CostBasisBreakEven].AllowGroupBy = DefaultableBoolean.True;

            #region Visible Columns
            band.Columns[PMConstants.COL_UDAAsset].Header.Caption = PMConstants.CAP_UDAAsset;
            band.Columns[PMConstants.COL_UDAAsset].Header.VisiblePosition = 102;
            band.Columns[PMConstants.COL_UDAAsset].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UDAAsset].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDAAsset].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDAAsset].Width = 80;
            band.Columns[PMConstants.COL_UDAAsset].Hidden = true;
            band.Columns[PMConstants.COL_UDAAsset].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UDACountry].Header.Caption = PMConstants.CAP_UDACountry;
            band.Columns[PMConstants.COL_UDACountry].Header.VisiblePosition = 103;
            band.Columns[PMConstants.COL_UDACountry].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UDACountry].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDACountry].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDACountry].Width = 80;
            band.Columns[PMConstants.COL_UDACountry].Hidden = true;
            band.Columns[PMConstants.COL_UDACountry].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UDASector].Header.Caption = PMConstants.CAP_UDASector;
            band.Columns[PMConstants.COL_UDASector].Header.VisiblePosition = 104;
            band.Columns[PMConstants.COL_UDASector].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UDASector].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASector].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASector].Width = 80;
            band.Columns[PMConstants.COL_UDASector].Hidden = true;
            band.Columns[PMConstants.COL_UDASector].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UDASecurityType].Header.Caption = PMConstants.CAP_UDASecurityType;
            band.Columns[PMConstants.COL_UDASecurityType].Header.VisiblePosition = 105;
            band.Columns[PMConstants.COL_UDASecurityType].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UDASecurityType].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASecurityType].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASecurityType].Width = 80;
            band.Columns[PMConstants.COL_UDASecurityType].Hidden = true;
            band.Columns[PMConstants.COL_UDASecurityType].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UDASubSector].Header.Caption = PMConstants.CAP_UDASubSector;
            band.Columns[PMConstants.COL_UDASubSector].Header.VisiblePosition = 106;
            band.Columns[PMConstants.COL_UDASubSector].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UDASubSector].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASubSector].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UDASubSector].Width = 80;
            band.Columns[PMConstants.COL_UDASubSector].Hidden = true;
            band.Columns[PMConstants.COL_UDASubSector].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_DayInterest].Header.Caption = PMConstants.CAP_DayInterest;
            band.Columns[PMConstants.COL_DayInterest].Header.VisiblePosition = 107;
            band.Columns[PMConstants.COL_DayInterest].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_DayInterest);
            band.Columns[PMConstants.COL_DayInterest].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_DayInterest].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_DayInterest].Hidden = false;
            band.Columns[PMConstants.COL_DayInterest].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_TotalInterest].Header.Caption = PMConstants.CAP_TotalInterest;
            band.Columns[PMConstants.COL_TotalInterest].Header.VisiblePosition = 108;
            band.Columns[PMConstants.COL_TotalInterest].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_TotalInterest);
            band.Columns[PMConstants.COL_TotalInterest].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_TotalInterest].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_TotalInterest].Hidden = false;
            band.Columns[PMConstants.COL_TotalInterest].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MarketValue].Header.Caption = PMConstants.CAP_MarketValue;
            band.Columns[PMConstants.COL_MarketValue].Header.VisiblePosition = 109;
            band.Columns[PMConstants.COL_MarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketValue);
            band.Columns[PMConstants.COL_MarketValue].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MarketValue].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MarketValue].Hidden = false;
            band.Columns[PMConstants.COL_MarketValue].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_YesterdayMarketValue].Header.Caption = PMConstants.CAP_YesterdayMarketValue;
            band.Columns[PMConstants.COL_YesterdayMarketValue].Header.VisiblePosition = 132;
            band.Columns[PMConstants.COL_YesterdayMarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarketValue);
            band.Columns[PMConstants.COL_YesterdayMarketValue].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_YesterdayMarketValue].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_YesterdayMarketValue].Hidden = false;
            band.Columns[PMConstants.COL_YesterdayMarketValue].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_MarketValueInBaseCurrency;
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].Header.VisiblePosition = 110;
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_MarketValueInBaseCurrency);
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_MarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Header.Caption = PMConstants.CAP_YesterdayMarketValueInBaseCurrency;
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Header.VisiblePosition = 131;
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_YesterdayMarketValueInBaseCurrency);
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_YesterdayMarketValueInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_GrossMarketValue].Header.Caption = PMConstants.CAP_GrossMarketValue;
            band.Columns[PMConstants.COL_GrossMarketValue].Header.VisiblePosition = 111;
            band.Columns[PMConstants.COL_GrossMarketValue].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GrossMarketValue);
            band.Columns[PMConstants.COL_GrossMarketValue].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_GrossMarketValue].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_GrossMarketValue].Hidden = false;
            band.Columns[PMConstants.COL_GrossMarketValue].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageGainLoss].Header.Caption = PMConstants.CAP_PercentageGainLoss;
            band.Columns[PMConstants.COL_PercentageGainLoss].Header.VisiblePosition = 112;
            band.Columns[PMConstants.COL_PercentageGainLoss].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageGainLoss);
            band.Columns[PMConstants.COL_PercentageGainLoss].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageGainLoss].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageGainLoss].Hidden = false;
            band.Columns[PMConstants.COL_PercentageGainLoss].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_BetaAdjExposure].Header.Caption = PMConstants.CAP_BetaAdjExposure;
            band.Columns[PMConstants.COL_BetaAdjExposure].Header.VisiblePosition = 113;
            band.Columns[PMConstants.COL_BetaAdjExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjExposure);
            band.Columns[PMConstants.COL_BetaAdjExposure].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_BetaAdjExposure].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_BetaAdjExposure].Width = 100;
            band.Columns[PMConstants.COL_BetaAdjExposure].Hidden = false;
            band.Columns[PMConstants.COL_BetaAdjExposure].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Header.Caption = PMConstants.CAP_BetaAdjExposureInBaseCurrency;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Header.VisiblePosition = 114;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjExposureInBaseCurrency);
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Width = 100;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_BetaAdjExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_BetaAdjGrossExposure].Header.Caption = PMConstants.CAP_BetaAdjGrossExposure;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].Header.VisiblePosition = 133;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_BetaAdjGrossExposure);
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].Width = 100;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].Hidden = false;
            band.Columns[PMConstants.COL_BetaAdjGrossExposure].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].Header.Caption = PMConstants.CAP_GainLossIfExerciseAssign;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].Header.VisiblePosition = 150;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_GainLossIfExerciseAssign);
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].Width = 100;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].Hidden = true;
            band.Columns[PMConstants.COL_GainLossIfExerciseAssign].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Volatility].Header.Caption = PMConstants.CAP_Volatility;
            band.Columns[PMConstants.COL_Volatility].Header.VisiblePosition = 115;
            band.Columns[PMConstants.COL_Volatility].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Volatility);
            band.Columns[PMConstants.COL_Volatility].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Volatility].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Volatility].Width = 100;
            band.Columns[PMConstants.COL_Volatility].Hidden = false;
            band.Columns[PMConstants.COL_Volatility].CellAppearance.TextHAlign = HAlign.Right;



            band.Columns[PMConstants.COL_LastUpdatedUTC].Header.Caption = PMConstants.CAP_LastUpdatedUTC;
            band.Columns[PMConstants.COL_LastUpdatedUTC].Header.VisiblePosition = 116;
            band.Columns[PMConstants.COL_LastUpdatedUTC].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_LastUpdatedUTC].AllowGroupBy = DefaultableBoolean.False;
            band.Columns[PMConstants.COL_LastUpdatedUTC].Width = 100;
            band.Columns[PMConstants.COL_LastUpdatedUTC].Hidden = false;
            band.Columns[PMConstants.COL_LastUpdatedUTC].CellAppearance.TextHAlign = HAlign.Right;
            band.Columns[PMConstants.COL_LastUpdatedUTC].Format = DateTimeConstants.NirvanaDateTimeFormat;

            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].Header.Caption = PMConstants.CAP_ExposureBPInBaseCurrency;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ExposureBPInBaseCurrency);
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].Header.VisiblePosition = 117;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].AllowGroupBy = DefaultableBoolean.False;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].Width = 100;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_ExposureBPInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;


            band.Columns[PMConstants.COL_Beta].Header.Caption = PMConstants.CAP_Beta;
            band.Columns[PMConstants.COL_Beta].Header.VisiblePosition = 118;
            band.Columns[PMConstants.COL_Beta].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Beta);
            band.Columns[PMConstants.COL_Beta].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Beta].Width = 50;
            band.Columns[PMConstants.COL_Beta].Hidden = false;
            band.Columns[PMConstants.COL_Beta].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CashImpact].Header.Caption = PMConstants.CAP_CashImpact;
            band.Columns[PMConstants.COL_CashImpact].Header.VisiblePosition = 119;
            band.Columns[PMConstants.COL_CashImpact].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CashImpact);
            band.Columns[PMConstants.COL_CashImpact].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CashImpact].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CashImpact].Hidden = false;
            band.Columns[PMConstants.COL_CashImpact].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].Header.Caption = PMConstants.CAP_CashImpactInBaseCurrency;
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].Header.VisiblePosition = 120;
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_CashImpactInBaseCurrency);
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_CashImpactInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_SideMultiplier].Header.VisiblePosition = 121;
            band.Columns[PMConstants.COL_SideMultiplier].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_SideMultiplier].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[PMConstants.COL_SideMultiplier].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_SideMultiplier].Hidden = true;
            band.Columns[PMConstants.COL_SideMultiplier].Header.Caption = PMConstants.CAP_SideMultiplier;
            band.Columns[PMConstants.COL_SideMultiplier].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageChange].Header.Caption = PMConstants.CAP_PercentageChange;
            band.Columns[PMConstants.COL_PercentageChange].Header.VisiblePosition = 122;
            band.Columns[PMConstants.COL_PercentageChange].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageChange);
            band.Columns[PMConstants.COL_PercentageChange].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageChange].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageChange].Hidden = true;
            band.Columns[PMConstants.COL_PercentageChange].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageUnderlyingChange].Header.Caption = PMConstants.CAP_PercentageUnderlyingChange;
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].Header.VisiblePosition = 123;
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageUnderlyingChange);
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].Hidden = true;
            band.Columns[PMConstants.COL_PercentageUnderlyingChange].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].Header.Caption = PMConstants.CAP_ChangeInUnderlyingPrice;
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].Header.VisiblePosition = 133;
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ChangeInUnderlyingPrice);
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].Hidden = true;
            band.Columns[PMConstants.COL_ChangeInUnderlyingPrice].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MasterFund].Header.VisiblePosition = 124;
            band.Columns[PMConstants.COL_MasterFund].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MasterFund].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MasterFund].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].Header.Caption = PMConstants.CAP_PercentageGainLossCostBasis;
            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].Header.VisiblePosition = 125;
            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_PercentageGainLossCostBasis);
            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_PercentageGainLossCostBasis].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_InternalComments].Header.Caption = PMConstants.CAP_InternalComments;

            band.Columns[PMConstants.COL_Exposure].Header.Caption = PMConstants.CAP_Exposure;
            band.Columns[PMConstants.COL_Exposure].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_Exposure);
            band.Columns[PMConstants.COL_Exposure].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Exposure].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Exposure].Hidden = false;
            band.Columns[PMConstants.COL_Exposure].CellAppearance.TextHAlign = HAlign.Right;


            band.Columns[PMConstants.COL_ExposureInBaseCurrency].Header.Caption = PMConstants.CAP_ExposureInBaseCurrency;
            band.Columns[PMConstants.COL_ExposureInBaseCurrency].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_ExposureInBaseCurrency);
            band.Columns[PMConstants.COL_ExposureInBaseCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_ExposureInBaseCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_ExposureInBaseCurrency].Hidden = false;
            band.Columns[PMConstants.COL_ExposureInBaseCurrency].CellAppearance.TextHAlign = HAlign.Right;

            bool showMasterFund = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MasterFundAssociation").ToString());
            if (showMasterFund)
            {
                band.Columns[PMConstants.COL_MasterFund].Hidden = false;
                band.Columns[PMConstants.COL_MasterFund].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                band.Columns[PMConstants.COL_MasterFund].Hidden = true;
                band.Columns[PMConstants.COL_MasterFund].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            band.Columns[PMConstants.COL_MasterStrategy].Header.Caption = PMConstants.CAP_MasterStrategy;
            band.Columns[PMConstants.COL_MasterStrategy].Header.VisiblePosition = 130;
            band.Columns[PMConstants.COL_MasterStrategy].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MasterStrategy].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MasterStrategy].CellAppearance.TextHAlign = HAlign.Right;

            bool showMasterStrategy = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MasterStrategyAssociation").ToString());
            if (showMasterStrategy)
            {
                band.Columns[PMConstants.COL_MasterStrategy].Hidden = false;
                band.Columns[PMConstants.COL_MasterStrategy].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                band.Columns[PMConstants.COL_MasterStrategy].Hidden = true;
                band.Columns[PMConstants.COL_MasterStrategy].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            band.Columns[PMConstants.COL_Analyst].Header.Caption = PMConstants.CAP_Analyst;
            band.Columns[PMConstants.COL_Analyst].Header.VisiblePosition = 134;
            band.Columns[PMConstants.COL_Analyst].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Analyst].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Analyst].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Analyst].Width = 80;
            band.Columns[PMConstants.COL_Analyst].Hidden = false;
            band.Columns[PMConstants.COL_Analyst].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CountryOfRisk].Header.Caption = PMConstants.CAP_CountryOfRisk;
            band.Columns[PMConstants.COL_CountryOfRisk].Header.VisiblePosition = 135;
            band.Columns[PMConstants.COL_CountryOfRisk].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CountryOfRisk].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CountryOfRisk].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CountryOfRisk].Width = 80;
            band.Columns[PMConstants.COL_CountryOfRisk].Hidden = false;
            band.Columns[PMConstants.COL_CountryOfRisk].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA1].Header.Caption = PMConstants.CAP_CustomUDA1;
            band.Columns[PMConstants.COL_CustomUDA1].Header.VisiblePosition = 136;
            band.Columns[PMConstants.COL_CustomUDA1].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA1].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA1].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA1].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA1].Hidden = false;
            band.Columns[PMConstants.COL_CustomUDA1].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA2].Header.Caption = PMConstants.CAP_CustomUDA2;
            band.Columns[PMConstants.COL_CustomUDA2].Header.VisiblePosition = 137;
            band.Columns[PMConstants.COL_CustomUDA2].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA2].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA2].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA2].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA2].Hidden = false;
            band.Columns[PMConstants.COL_CustomUDA2].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA3].Header.Caption = PMConstants.CAP_CustomUDA3;
            band.Columns[PMConstants.COL_CustomUDA3].Header.VisiblePosition = 138;
            band.Columns[PMConstants.COL_CustomUDA3].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA3].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA3].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA3].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA3].Hidden = false;
            band.Columns[PMConstants.COL_CustomUDA3].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA4].Header.Caption = PMConstants.CAP_CustomUDA4;
            band.Columns[PMConstants.COL_CustomUDA4].Header.VisiblePosition = 139;
            band.Columns[PMConstants.COL_CustomUDA4].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA4].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA4].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA4].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA4].Hidden = false;
            band.Columns[PMConstants.COL_CustomUDA4].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA5].Header.Caption = PMConstants.CAP_CustomUDA5;
            band.Columns[PMConstants.COL_CustomUDA5].Header.VisiblePosition = 140;
            band.Columns[PMConstants.COL_CustomUDA5].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA5].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA5].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA5].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA5].Hidden = true;
            band.Columns[PMConstants.COL_CustomUDA5].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA6].Header.Caption = PMConstants.CAP_CustomUDA6;
            band.Columns[PMConstants.COL_CustomUDA6].Header.VisiblePosition = 141;
            band.Columns[PMConstants.COL_CustomUDA6].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA6].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA6].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA6].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA6].Hidden = true;
            band.Columns[PMConstants.COL_CustomUDA6].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_CustomUDA7].Header.Caption = PMConstants.CAP_CustomUDA7;
            band.Columns[PMConstants.COL_CustomUDA7].Header.VisiblePosition = 142;
            band.Columns[PMConstants.COL_CustomUDA7].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_CustomUDA7].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA7].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_CustomUDA7].Width = 80;
            band.Columns[PMConstants.COL_CustomUDA7].Hidden = true;
            band.Columns[PMConstants.COL_CustomUDA7].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Issuer].Header.Caption = PMConstants.CAP_Issuer;
            band.Columns[PMConstants.COL_Issuer].Header.VisiblePosition = 143;
            band.Columns[PMConstants.COL_Issuer].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Issuer].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Issuer].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Issuer].Width = 80;
            band.Columns[PMConstants.COL_Issuer].Hidden = true;
            band.Columns[PMConstants.COL_Issuer].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_LiquidTag].Header.Caption = PMConstants.CAP_LiquidTag;
            band.Columns[PMConstants.COL_LiquidTag].Header.VisiblePosition = 144;
            band.Columns[PMConstants.COL_LiquidTag].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_LiquidTag].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_LiquidTag].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_LiquidTag].Width = 80;
            band.Columns[PMConstants.COL_LiquidTag].Hidden = true;
            band.Columns[PMConstants.COL_LiquidTag].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_MarketCap].Header.Caption = PMConstants.CAP_MarketCap;
            band.Columns[PMConstants.COL_MarketCap].Header.VisiblePosition = 145;
            band.Columns[PMConstants.COL_MarketCap].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_MarketCap].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MarketCap].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_MarketCap].Width = 80;
            band.Columns[PMConstants.COL_MarketCap].Hidden = true;
            band.Columns[PMConstants.COL_MarketCap].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_Region].Header.Caption = PMConstants.CAP_Region;
            band.Columns[PMConstants.COL_Region].Header.VisiblePosition = 146;
            band.Columns[PMConstants.COL_Region].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_Region].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Region].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_Region].Width = 80;
            band.Columns[PMConstants.COL_Region].Hidden = true;
            band.Columns[PMConstants.COL_Region].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_RiskCurrency].Header.Caption = PMConstants.CAP_RiskCurrency;
            band.Columns[PMConstants.COL_RiskCurrency].Header.VisiblePosition = 147;
            band.Columns[PMConstants.COL_RiskCurrency].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_RiskCurrency].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_RiskCurrency].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_RiskCurrency].Width = 80;
            band.Columns[PMConstants.COL_RiskCurrency].Hidden = true;
            band.Columns[PMConstants.COL_RiskCurrency].CellAppearance.TextHAlign = HAlign.Right;

            band.Columns[PMConstants.COL_UcitsEligibleTag].Header.Caption = PMConstants.CAP_UcitsEligibleTag;
            band.Columns[PMConstants.COL_UcitsEligibleTag].Header.VisiblePosition = 148;
            band.Columns[PMConstants.COL_UcitsEligibleTag].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_UcitsEligibleTag].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UcitsEligibleTag].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_UcitsEligibleTag].Width = 80;
            band.Columns[PMConstants.COL_UcitsEligibleTag].Hidden = true;
            band.Columns[PMConstants.COL_UcitsEligibleTag].CellAppearance.TextHAlign = HAlign.Right;
            #endregion

            band.Columns[PMConstants.COL_FXRateDisplay].Header.Caption = PMConstants.CAP_FXRateDisplay;
            band.Columns[PMConstants.COL_FXRateDisplay].Format = PMConstantsHelper.GetFormatStringByCaption(PMConstants.CAP_FXRateDisplay);
            band.Columns[PMConstants.COL_FXRateDisplay].Header.VisiblePosition = 149;
            band.Columns[PMConstants.COL_FXRateDisplay].CellActivation = Activation.NoEdit;
            band.Columns[PMConstants.COL_FXRateDisplay].Hidden = false;
            band.Columns[PMConstants.COL_FXRateDisplay].AllowGroupBy = DefaultableBoolean.True;
            band.Columns[PMConstants.COL_FXRateDisplay].CellAppearance.TextHAlign = HAlign.Right;
            #endregion

            GridMarketDataColumnUtil.hideNonPermitMarketDataColumns(PranaModules.PORTFOLIO_MANAGEMENT_MODULE, ultraGrid1);

        }

        private void SetGridFontSize()
        {
            try
            {
                float fontSize = Convert.ToSingle(_pmAppearances.FontSizeGrid);
                Font oldFont = ultraGrid1.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                ultraGrid1.Font = newFont;
                ultraGrid1.Font = newFont;
                ultraGrid1.DisplayLayout.Override.CellPadding = 3;
                if (!_pmAppearances.IsDefaultRowBackColor)
                    ultraGrid1.DisplayLayout.Override.RowAppearance.BackColor = Color.FromArgb(_pmAppearances.RowBgColor);

                if (!_pmAppearances.IsDefaultAlternateRowColor)
                    ultraGrid1.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(_pmAppearances.AlternateColor);

                if (!_pmAppearances.ShowGridLines)
                {
                    ultraGrid1.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.None;
                    ultraGrid1.DisplayLayout.Override.BorderStyleRow = UIElementBorderStyle.None;
                }
                else
                {
                    ultraGrid1.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.Default;
                    ultraGrid1.DisplayLayout.Override.BorderStyleRow = UIElementBorderStyle.Default;
                }
                foreach (UltraGridColumn col in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    string caption = PMConstantsHelper.GetCaptionByColumnName(col.Key);
                    if (caption != PMConstants.CAP_TradeDate && caption != PMConstants.CAP_SettlementDate && caption != PMConstants.CAP_ExDividendDate && caption != PMConstants.CAP_ExpirationDate && caption != PMConstants.CAP_LastUpdatedUTC)
                        col.Format = PMConstantsHelper.GetFormatStringByCaption(caption);
                    if (col.Key == PMConstants.COL_StartTradeDate)
                    {
                        col.Format = DateTimeConstants.DateFormat;
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

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                SetUIColumns();

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


        private void ultraGrid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {

                if (e == null || e.Row == null || !e.Row.Band.Columns.Exists(PMConstants.COL_OrderSideTagValue))
                {
                    return;
                }

                if (e.Row.Band.Columns.Exists(PMConstants.COL_ExpirationMonth))
                {
                    e.Row.Band.Columns[PMConstants.COL_ExpirationMonth].Format = "MMMM yyyy";
                    e.Row.Band.Columns[PMConstants.COL_ExpirationMonth].NullText = "Non-Expiring Positions";
                    e.Row.Band.Columns[PMConstants.COL_ExpirationMonth].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                }

                object cellValue = null;
                if (_pmAppearances.RowColorbasis.Equals("1")) // Day Pnl
                {
                    cellValue = e.Row.GetCellValue(e.Row.Band.Columns[PMConstants.COL_DayPnLInBaseCurrency]);
                    if (cellValue != null)
                    {
                        double dayPnL = Convert.ToDouble(cellValue);
                        if (dayPnL > 0.0)
                        {
                            e.Row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.DayPnlPositiveColor);
                        }
                        else if (dayPnL < 0.0)
                        {
                            e.Row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.DayPnlNegativeColor);
                        }
                        else
                        {
                            e.Row.Appearance.ForeColor = Color.White;
                        }

                    }
                }
                else if (_pmAppearances.RowColorbasis.Equals("0"))
                {
                    cellValue = e.Row.GetCellValue(e.Row.Band.Columns[PMConstants.COL_OrderSideTagValue]);

                    if (cellValue != null)
                    {
                        string orderSideTagValue = cellValue.ToString();
                        switch (orderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_BuyMinus:
                                e.Row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.OrderSideBuyColor);
                                break;
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_Sell_Closed:
                            case FIXConstants.SIDE_SellPlus:
                            case FIXConstants.SIDE_SellShortExempt:
                                e.Row.Appearance.ForeColor = Color.FromArgb(_pmAppearances.OrderSideSellColor);
                                break;
                            default:
                                e.Row.Appearance.ForeColor = Color.White;
                                break;
                        }
                    }
                }
                else
                {
                    e.Row.Appearance.ForeColor = Color.White;
                }
                e.Row.RefreshSortPosition();

                //we have removed refresh sort as all the row collapses after calling refresh sort
                //  grdConsolidation.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);   
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

        private void saveLayoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (ultraGrid1.DataSource != null)
            {
                ultraGrid1.DisplayLayout.SaveAsXml(_filePath);//, PropertyCategories.All);
            }
        }

        private void ultraGrid1_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.ultraGrid1);
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

        private void PMTaxLotsDisplayForm_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void ultraGrid1_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}