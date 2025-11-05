using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;

using Infragistics.Win.UltraWinGrid;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlMainConsolidationView : UserControl
    {
        #region Grid Column Names

        const string COL_DataSourceNameIDValue = "DataSourceNameIDValue";
        const string COL_FundValue = "FundValue";
        const string COL_Symbol = "Symbol";
        const string COL_Qty = "Qty";
        const string COL_Cost = "Cost";
        const string COL_Multiplier = "Multiplier";
        const string COL_CurrencyValue = "CurrencyValue";
        const string COL_FXRate = "FXRate";
        const string COL_DaysTradedQty = "DaysTradedQty";
        const string COL_DayAveragePrice = "DayAveragePrice";
        const string COL_NetExposure = "NetExposure";
        const string COL_NetPosition = "NetPosition";
        const string COL_LastPrice = "LastPrice";
        const string COL_WeightedLongPrice = "WeightedLongPrice";
        const string COL_WeightedShortPrice = "WeightedShortPrice";
        const string COL_NetLong = "NetLong";
        const string COL_NetShort = "NetShort";
        const string COL_NetPNL = "NetPNL";
        const string COL_RealisedPNL = "RealisedPNL";
        const string COL_UnrealisedPNL = "UnrealisedPNL";
        const string COL_NetLongExposure = "NetLongExposure";
        const string COL_NetShortExposure = "NetShortExposure";
        const string COL_SecurityWeightLong = "SecurityWeightLong";
        const string COL_SecurityWeightShort = "SecurityWeightShort";
        const string COL_PNLLong = "PNLLong";
        const string COL_PNLShort = "PNLShort";
        const string COL_LongExposure = "LongExposure";
        const string COL_ShortExposure = "ShortExposure";

        #endregion Grid Column Names

        BindingSource _formBindingSource = new BindingSource();
        ConsolidationSummary _consolidationSummary = null;

        public CtrlMainConsolidationView()
        {
            InitializeComponent();
            //Layout.Override.RowSelectors = DefaultableBoolean.True;
            //.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
        }

        public void PopulateDataInGrid()
        {
            // This is just dummy code to see grid goes to initialize object
            // don't get furiuos over the obejct being bound.  :) 
            DataSource dummyobject = new DataSource();

            //commented by ram!
            
            //grdConsolidation.DataMember = "DataSourceAddressDetails";
            //grdConsolidation.DataSource = dummyobject;

            
        }
       

        /// <summary>
        /// Handles the InitializeLayout event of the grdConsolidation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        //private void grdConsolidation_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        //{
        //    e.Layout.Override.RowSelectors = DefaultableBoolean.True;
        //    e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


        //}

        private void grdConsolidation_Layout(object sender, LayoutEventArgs e)
        {
            
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl(bool isGroupByAllowed)
        {
            _isGroupByAllowed = isGroupByAllowed;
            if (!_isInitialized)
            {
                SetupBinding();
                _isInitialized = true;
            }
        }

        #endregion

        private void SetupBinding()
        {
            _formBindingSource.DataSource = RetrieveConsolidationSummary();

            lblPNLLongTotal.DataBindings.Add("Text", _formBindingSource, "PNLLongTotal");
            lblPNLShortTotal.DataBindings.Add("Text", _formBindingSource, "PNLShortTotal");
            lblPNLNetTotal.DataBindings.Add("Text", _formBindingSource, "PNLNetTotal");

            lblLongExposureTotal.DataBindings.Add("Text", _formBindingSource, "LongExposureTotal");
            lblShortExposureTotal.DataBindings.Add("Text", _formBindingSource, "ShortExposureTotal");
            lblNetExposureTotal.DataBindings.Add("Text", _formBindingSource, "NetExposureTotal");

            lblCashInflow.DataBindings.Add("Text", _formBindingSource, "CashInflow");
            lblCashOutflow.DataBindings.Add("Text", _formBindingSource, "CashOutflow");

            grdConsolidation.DataMember = "ConsolidatedInfoList";
            grdConsolidation.DataSource = _formBindingSource;
        }


        private ConsolidationSummary RetrieveConsolidationSummary()
        {
            _consolidationSummary = new ConsolidationSummary();
            _consolidationSummary.PNLLongTotal = 2051632.905;
            _consolidationSummary.PNLShortTotal = -26289.38;
            _consolidationSummary.PNLNetTotal = 2025343.525;
            _consolidationSummary.LongExposureTotal = 20137687.35;
            _consolidationSummary.ShortExposureTotal = -1562844.19;
            _consolidationSummary.NetExposureTotal = 18574843.16;
            _consolidationSummary.CashInflow = 0;
            _consolidationSummary.CashOutflow = 0;

            _consolidationSummary.ConsolidatedInfoList = RetrieveConsolidatedInfoList();
            return _consolidationSummary;
        }

        private BindingList<ConsolidatedInfo> RetrieveConsolidatedInfoList()
        {
            BindingList<ConsolidatedInfo> list = new BindingList<ConsolidatedInfo>();

            SortableSearchableList<DataSourceNameID> fullList = DataSourceNameIDList.GetInstance().Retrieve;
            foreach (DataSourceNameID dataSourceNameID in fullList)
            {
                if (!dataSourceNameID.FullName.Equals(Constants.C_COMBO_SELECT) || !dataSourceNameID.FullName.Equals(Constants.C_COMBO_ALL))
                {
                    if (dataSourceNameID.ID == 1)
                    {
                        ConsolidatedInfo _consolidatedInfo = new ConsolidatedInfo();
                        _consolidatedInfo.DataSourceNameIDValue = dataSourceNameID;
                        _consolidatedInfo.FundValue.Name = "Long";
                        _consolidatedInfo.Symbol = "MSFT";
                        _consolidatedInfo.Qty = 100000;
                        _consolidatedInfo.Cost = 25.54;
                        _consolidatedInfo.Multiplier = 1;
                        _consolidatedInfo.CurrencyValue.Name = "USD";
                        _consolidatedInfo.FXRate = 1;
                        _consolidatedInfo.DaysTradedQty = 1000;
                        _consolidatedInfo.DayAveragePrice = 25.55;
                        _consolidatedInfo.NetPosition = 100000;
                        _consolidatedInfo.LastPrice = 25.53;
                        _consolidatedInfo.NetExposure = 1453470;
                        _consolidatedInfo.WeightedLongPrice = 25.7156000;
                        _consolidatedInfo.WeightedShortPrice = 25;
                        _consolidatedInfo.NetLong = 14444544;
                        _consolidatedInfo.NetShort = 17423;
                        _consolidatedInfo.NetPNL = -456412;
                        _consolidatedInfo.RealisedPNL = 214456.1547;
                        _consolidatedInfo.UnrealisedPNL = -45647645.44;
                        _consolidatedInfo.NetLongExposure = 5;
                        _consolidatedInfo.NetShortExposure = 259979.458;
                        _consolidatedInfo.SecurityWeightLong = 0;
                        _consolidatedInfo.PNLLong = -10000;
                        _consolidatedInfo.PNLShort = 0;
                        _consolidatedInfo.LongExposure = 144400;
                        _consolidatedInfo.ShortExposure = 0;

                        list.Add(_consolidatedInfo);
                    }
                    if (dataSourceNameID.ID == 32)
                    {
                        ConsolidatedInfo _consolidatedInfo = new ConsolidatedInfo();
                        _consolidatedInfo.DataSourceNameIDValue = dataSourceNameID;
                        _consolidatedInfo.FundValue.Name = "Long";
                        _consolidatedInfo.Symbol = "AAPL";
                        _consolidatedInfo.Qty = 100000;
                        _consolidatedInfo.Cost = 14.54;
                        _consolidatedInfo.Multiplier = 1;
                        _consolidatedInfo.CurrencyValue.Name = "USD";
                        _consolidatedInfo.FXRate = 1;
                        _consolidatedInfo.DaysTradedQty = 1000;
                        _consolidatedInfo.DayAveragePrice = 14.55;
                        _consolidatedInfo.NetPosition = 100000;
                        _consolidatedInfo.LastPrice = 14.53;
                        _consolidatedInfo.NetExposure = 14444000;
                        _consolidatedInfo.WeightedLongPrice = 14.7156000;
                        _consolidatedInfo.WeightedShortPrice = 16;
                        _consolidatedInfo.NetLong = 123300;
                        _consolidatedInfo.NetShort = 20437;
                        _consolidatedInfo.NetPNL = -10000;
                        _consolidatedInfo.RealisedPNL = 26318.1254;
                        _consolidatedInfo.UnrealisedPNL = -28002.453;
                        _consolidatedInfo.NetLongExposure = 1780452;
                        _consolidatedInfo.NetShortExposure = 259979.458;
                        _consolidatedInfo.SecurityWeightLong = 0;
                        _consolidatedInfo.PNLLong = -10000;
                        _consolidatedInfo.PNLShort = 0;
                        _consolidatedInfo.LongExposure = 14445500;
                        _consolidatedInfo.ShortExposure = 0;

                        list.Add(_consolidatedInfo);
                    }
                    if (dataSourceNameID.ID == 35)
                    {
                        ConsolidatedInfo _consolidatedInfo = new ConsolidatedInfo();
                        _consolidatedInfo.DataSourceNameIDValue = dataSourceNameID;
                        _consolidatedInfo.FundValue.Name = "Long";
                        _consolidatedInfo.Symbol = "DELL";
                        _consolidatedInfo.Qty = 100000;
                        _consolidatedInfo.Cost = 14.54;
                        _consolidatedInfo.Multiplier = 1;
                        _consolidatedInfo.CurrencyValue.Name = "USD";
                        _consolidatedInfo.FXRate = 1;
                        _consolidatedInfo.DaysTradedQty = 1000;
                        _consolidatedInfo.DayAveragePrice = 14.55;
                        _consolidatedInfo.NetPosition = 100000;
                        _consolidatedInfo.LastPrice = 14.53;
                        _consolidatedInfo.NetExposure = 14444000;
                        _consolidatedInfo.WeightedLongPrice = 14.7156000;
                        _consolidatedInfo.WeightedShortPrice = 16;
                        _consolidatedInfo.NetLong = 123300;
                        _consolidatedInfo.NetShort = 20437;
                        _consolidatedInfo.NetPNL = -10000;
                        _consolidatedInfo.RealisedPNL = 26318.1254;
                        _consolidatedInfo.UnrealisedPNL = -28002.453;
                        _consolidatedInfo.NetLongExposure = 1780452;
                        _consolidatedInfo.NetShortExposure = 259979.458;
                        _consolidatedInfo.SecurityWeightLong = 0;
                        _consolidatedInfo.SecurityWeightShort = 0;
                        _consolidatedInfo.PNLLong = -10000;
                        _consolidatedInfo.PNLShort = 0;
                        _consolidatedInfo.LongExposure = 144400;
                        _consolidatedInfo.ShortExposure = 0;

                        list.Add(_consolidatedInfo);
                    }  

                }
            }


            return list;
        }

        private bool _isGroupByAllowed = false;

        /// <summary>
        /// Gets or sets a value indicating whether group is allowed  on grid or not by allowed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is group by allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsGroupByAllowed
        {
            set 
            { 
                _isGroupByAllowed = value;
            }
        }


        private void grdConsolidation_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridLayout gridLayout = grdConsolidation.DisplayLayout;
            UltraGridBand band = e.Layout.Bands[0];
            e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            
            if (_isGroupByAllowed)
            {
                gridLayout.GroupByBox.Hidden = false;
                gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            }
            else
            {
                gridLayout.GroupByBox.Hidden = true;
                gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            }
            
            gridLayout.AutoFitStyle = AutoFitStyle.None;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            gridLayout.Override.AllowAddNew = AllowAddNew.No;
            gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            UltraGridColumn colDataSourceNameIDValue = band.Columns[COL_DataSourceNameIDValue];
            colDataSourceNameIDValue.Header.Caption = "Data Source";
            colDataSourceNameIDValue.Header.VisiblePosition = 1;

            UltraGridColumn colFundValue = band.Columns[COL_FundValue];
            colFundValue.Header.Caption = "A/c Name/Fund";
            colFundValue.Header.VisiblePosition = 2;

            UltraGridColumn colSymbol = band.Columns[COL_Symbol];
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;

            UltraGridColumn colQty = band.Columns[COL_Qty];
            colQty.Header.Caption = "Qty";
            colQty.Header.VisiblePosition = 4;

            UltraGridColumn colCost = band.Columns[COL_Cost];
            colCost.Header.Caption = "Cost";
            colCost.Header.VisiblePosition = 5;

            UltraGridColumn colMultiplier = band.Columns[COL_Multiplier];
            colMultiplier.Header.Caption = "Multiplier";
            colMultiplier.Header.VisiblePosition = 6;

            UltraGridColumn colCurrencyValue = band.Columns[COL_CurrencyValue];
            colCurrencyValue.Header.Caption = "CCY";
            colCurrencyValue.Header.VisiblePosition = 7;

            UltraGridColumn colFXRate = band.Columns[COL_FXRate];
            colFXRate.Header.Caption = "FX Rate";
            colFXRate.Header.VisiblePosition = 8;

            UltraGridColumn colDaysTradedQty = band.Columns[COL_DaysTradedQty];
            colDaysTradedQty.Header.Caption = "Days traded qty";
            colDaysTradedQty.Header.VisiblePosition = 9;

            UltraGridColumn colDayAveragePrice = band.Columns[COL_DayAveragePrice];
            colDayAveragePrice.Header.Caption = "Days average price";
            colDayAveragePrice.Header.VisiblePosition = 10;

            UltraGridColumn colNetPosition = band.Columns[COL_NetPosition];
            colNetPosition.Header.Caption = "Net position";
            colNetPosition.Header.VisiblePosition = 11;

            UltraGridColumn colLastPrice = band.Columns[COL_LastPrice];
            colLastPrice.Header.Caption = "Last Price";
            colLastPrice.Header.VisiblePosition = 12;

            UltraGridColumn colNetExposure = band.Columns[COL_NetExposure];
            colNetExposure.Header.Caption = "Net Exposure";
            colNetExposure.Header.VisiblePosition = 13;

            UltraGridColumn colWeightedLongPrice = band.Columns[COL_WeightedLongPrice];
            colWeightedLongPrice.Header.Caption = "Weighted Long Price";
            colWeightedLongPrice.Header.VisiblePosition = 14;

            UltraGridColumn colWeightedShortPrice = band.Columns[COL_WeightedShortPrice];
            colWeightedShortPrice.Header.Caption = "Weighted Short Price";
            colWeightedShortPrice.Header.VisiblePosition = 15;

            UltraGridColumn colNetLong = band.Columns[COL_NetLong];
            colNetLong.Header.Caption = "Net Long";
            colNetLong.Header.VisiblePosition = 16;

            UltraGridColumn colNetShort = band.Columns[COL_NetShort];
            colNetShort.Header.Caption = "Net Short";
            colNetShort.Header.VisiblePosition = 17;

            UltraGridColumn colNetPNL = band.Columns[COL_NetPNL];
            colNetPNL.Header.Caption = "Net PNL";
            colNetPNL.Header.VisiblePosition = 18;

            UltraGridColumn colRealisedPNL = band.Columns[COL_RealisedPNL];
            colRealisedPNL.Header.Caption = "Est Realized PNL";
            colRealisedPNL.Header.VisiblePosition = 19;

            UltraGridColumn colUnrealisedPNL = band.Columns[COL_UnrealisedPNL];
            colUnrealisedPNL.Header.Caption = "Unrealized PNL";
            colUnrealisedPNL.Header.VisiblePosition = 20;

            UltraGridColumn colNetLongExposure = band.Columns[COL_NetLongExposure];
            colNetLongExposure.Header.Caption = "Net Long Exposure";
            colNetLongExposure.Header.VisiblePosition = 21;

            UltraGridColumn colNetShortExposure = band.Columns[COL_NetShortExposure];
            colNetShortExposure.Header.Caption = "Net Short Exposure";
            colNetShortExposure.Header.VisiblePosition = 22;

            UltraGridColumn colSecurityWeightLong = band.Columns[COL_SecurityWeightLong];
            colSecurityWeightLong.Header.Caption = "Security Weight Long";
            colSecurityWeightLong.Header.VisiblePosition = 23;

            UltraGridColumn colSecurityWeightShort = band.Columns[COL_SecurityWeightShort];
            colSecurityWeightShort.Header.Caption = "Security Weight Short";
            colSecurityWeightShort.Header.VisiblePosition = 24;

            UltraGridColumn colPNLLong = band.Columns[COL_PNLLong];
            colPNLLong.Header.Caption = "PNL Long";
            colPNLLong.Header.VisiblePosition = 25;

            UltraGridColumn colPNLShort = band.Columns[COL_PNLShort];
            colPNLShort.Header.Caption = "PNL Short";
            colPNLShort.Header.VisiblePosition = 26;

            UltraGridColumn colLongExposure = band.Columns[COL_LongExposure];
            colLongExposure.Header.Caption = "Long Exposure";
            colLongExposure.Header.VisiblePosition = 27;

            UltraGridColumn colShortExposure = band.Columns[COL_ShortExposure];
            colShortExposure.Header.Caption = "Short Exposure";
            colShortExposure.Header.VisiblePosition = 28;

        }
    }
}
