using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects.AppConstants;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.Utilities.UIUtilities;
using Prana.BusinessObjects.LiveFeed;

namespace Prana.Analytics
{
    public partial class PranaOptionGreekAnalysis : UserControl 
    {
        bool isAlreadyStarted = false;
        List<string> _listRequest = new List<string>();
        private delegate void UIThreadMarsheller(OptionResponseObj optionResponseObj);
        private delegate void Level1DataUpdateHandler(Level1Data level1Data);
        Dictionary<string, List<PranaPositionWithGreeks>> _dictBindedData = new Dictionary<string, List<PranaPositionWithGreeks>>();
        PranaPositionWithGreekColl _pranaPositionWithGreekColl = new PranaPositionWithGreekColl();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        public PranaOptionGreekAnalysis()
        {
            InitializeComponent();
          
        }
        public void SetUp()
        {
            if (!isAlreadyStarted)
            {
                GetData();
                SetColumns();
                OptionClientManager.GetInstance.GreeksCalculated += new OptionClientManager.GreeksCalculaterHandler(GetInstance_GreeksCalculated);
                OptionClientManager.GetInstance.Level1SnapShotReceived += new OptionClientManager.Level1SnapShotReceivedHandler(GetInstance_Level1SnapShotReceived);
                isAlreadyStarted = true;
            }
        }

       

        private void DisableForm()
        {
            if (btnCalculateGreeks.Enabled)
            {
                btnCalculateGreeks.Enabled = false;
                btnSimulation.Enabled = false;
                timerRefresh.Start();
                timerRefresh.Interval = 3000 + GetSelectedRows().Count * 1000;
            }
        }

        private void EnableForm()
        {
            btnCalculateGreeks.BackColor = Color.FromArgb(192, 192, 255);
            btnCalculateGreeks.Text = "CalculateGreeks";
            btnCalculateGreeks.Enabled = true;
            btnSimulation.BackColor = Color.FromArgb(192, 192, 255);
            btnSimulation.Text = "Simulate";
            btnSimulation.Enabled = true;
            timerRefresh.Stop();
            grdData.DataBind();
        }

        void GetInstance_GreeksCalculated(OptionResponseObj optionResponseObj)
        {
            // send to main UI thread
            UIThreadMarsheller mi = new UIThreadMarsheller(GetInstance_GreeksCalculated);

            if (grdData.InvokeRequired)
            {
                this.Invoke(mi, new object[] { optionResponseObj });

            }
            else
            {
                UpdateData(optionResponseObj);
            }
        }
        void GetInstance_Level1SnapShotReceived(Level1Data l1Data)
        {
            try
            {
                Level1DataUpdateHandler mi = new Level1DataUpdateHandler(GetInstance_Level1SnapShotReceived);

                if (grdData.InvokeRequired)
                {
                    this.Invoke(mi, new object[] { l1Data });
                }
                else
                {
                    if (_dictBindedData.ContainsKey(l1Data.Symbol))
                    {
                        List<PranaPositionWithGreeks> list = _dictBindedData[l1Data.Symbol];
                        foreach (PranaPositionWithGreeks pranaPositionWithGreeks in list)
                        {
                            if (l1Data.Mid == double.MinValue)
                            {
                                l1Data.Mid = 0;
                            }

                            pranaPositionWithGreeks.MarketPrice = l1Data.Mid;
                            pranaPositionWithGreeks.LastPrice = l1Data.Last;
                            double price = 0;

                            if (l1Data.Mid != 0)
                            {
                                price = l1Data.Mid;
                            }
                            else
                            {
                                price = l1Data.Last;
                            }
                            if (pranaPositionWithGreeks.AssetID != (int)AssetCategory.EquityOption)
                            {
                                pranaPositionWithGreeks.StockPrice = price;
                            }
                            CalculateDeltaExposure(pranaPositionWithGreeks);
                        }
                        grdData.Refresh();
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
       

        private void UpdateData(OptionResponseObj optionResponseObj)
        {
            try
            {
                foreach (KeyValuePair<string, OptionGreeks> item in optionResponseObj.CalculatedGreeks)
                {
                    if (_dictBindedData.ContainsKey(item.Key))
                    {
                        List<PranaPositionWithGreeks> list = _dictBindedData[item.Key];
                        foreach (PranaPositionWithGreeks pranaPositionWithGreeks in list)
                        {
                            if (pranaPositionWithGreeks.IsChecked)
                            {
                                pranaPositionWithGreeks.UpdatePranaPositionWithGreeks(item.Value);                                                         
                            }
                        }
                    }
                }
                CalculateDeltaExposure();
                grdData.Refresh();
                EnableForm();
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
        private void CalculateDeltaExposure()
        {
            try
            {
                foreach (PranaPositionWithGreeks pranaPositionWithGreeks in _pranaPositionWithGreekColl)
                {
                    CalculateDeltaExposure(pranaPositionWithGreeks);
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
        private void CalculateDeltaExposure(PranaPositionWithGreeks pranaPositionWithGreeks)
        {
            try
            {
                pranaPositionWithGreeks.DeltaAdjExposure = Formulae.Formulae.GetNetExposure(pranaPositionWithGreeks.Quantity, pranaPositionWithGreeks.StockPrice, pranaPositionWithGreeks.Multiplier, pranaPositionWithGreeks.SideMultiplier, pranaPositionWithGreeks.Delta);
                pranaPositionWithGreeks.CostBasisPnl = Formulae.Formulae.GetPnL(pranaPositionWithGreeks.Quantity, pranaPositionWithGreeks.MarketPrice, pranaPositionWithGreeks.AvgPrice, pranaPositionWithGreeks.Multiplier, pranaPositionWithGreeks.SideMultiplier);
                pranaPositionWithGreeks.DayPnl = Formulae.Formulae.GetPnL(pranaPositionWithGreeks.Quantity, pranaPositionWithGreeks.MarketPrice, pranaPositionWithGreeks.LastPrice, pranaPositionWithGreeks.Multiplier, pranaPositionWithGreeks.SideMultiplier);
                pranaPositionWithGreeks.DeltaAdjPosition = pranaPositionWithGreeks.DeltaAdjExposure / pranaPositionWithGreeks.StockPrice;
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
        private void btnRefreshPositions_Click(object sender, EventArgs e)
        {
            PositionDataManager.Refresh();
            GetData();
        }
        private void GetData()
        {
            _pranaPositionWithGreekColl = PositionDataManager.GetPositionsWithGreeks();
            _dictBindedData = new Dictionary<string, List<PranaPositionWithGreeks>>();
            foreach (PranaPositionWithGreeks obj in _pranaPositionWithGreekColl)
            {
                    if (_dictBindedData.ContainsKey(obj.Symbol))
                    {
                        _dictBindedData[obj.Symbol].Add(obj);
                    }
                    else
                    {
                        List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
                        list.Add(obj);
                        _dictBindedData.Add(obj.Symbol, list);
                    }
            }
           
            grdData.DataSource = _pranaPositionWithGreekColl;            
            grdData.DataBind();
        }
        private List<PranaPositionWithGreeks> GetSelectedRows()
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            UltraGridRow[] rows = grdData.Rows.GetFilteredInNonGroupByRows();
            foreach (UltraGridRow row in rows)
            {
                if (row.Cells["IsChecked"].Text.ToUpper().Equals("TRUE"))
                {
                    PranaPositionWithGreeks pranaPosWithGreeks = (PranaPositionWithGreeks)row.ListObject;
                    list.Add(pranaPosWithGreeks);
                }
            }
            return list;
        }
        private void btnCalculateGreeks_Click(object sender, EventArgs e)
        {

            try
            {
                if (ValidateRequest())
                {
                    List<PranaPositionWithGreeks> list = GetSelectedRows();
                    InputParametersCollection inputParametersCollection = new InputParametersCollection();
                    List<string> symbolList = new List<string>();
                    foreach (PranaPositionWithGreeks position in list)
                    {
                        symbolList.Add(position.Symbol);
                        if (position.AssetID == (int)AssetCategory.EquityOption)
                        {
                            InputParametersForGreeks inputParams = position.GetBasicInputParams();
                            inputParametersCollection.Add(inputParams);
                        }
                    }
                    inputParametersCollection.UserID = "1";
                    string requestID = System.Guid.NewGuid().ToString();
                    _listRequest.Add(requestID);
                    OptionClientManager.GetInstance.SendRequest(inputParametersCollection, requestID);
                    string requestIDSymbolData = System.Guid.NewGuid().ToString();
                    _listRequest.Add(requestIDSymbolData);
                    OptionClientManager.GetInstance.SendRequest(symbolList, requestIDSymbolData);
                    DisableForm();

                    btnCalculateGreeks.BackColor = Color.Red;
                    btnCalculateGreeks.Text = "Calculating...";
                   

                    btnCalculateGreeks.Enabled = false;
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
        private void CalculateChanges()
        {
            foreach (KeyValuePair<string,List<PranaPositionWithGreeks>> item in _dictBindedData)
            {
               List<PranaPositionWithGreeks> posList= item.Value;
               foreach (PranaPositionWithGreeks position in posList)
               {
                   if (chkbxVol.Checked)
                   {
                       position.ImpliedVol = position.ImpliedVol + Convert.ToDouble((numericUpDownVol.Value));
                   }
                   if (chkbxUnderLyingPrice.Checked)
                   {
                       position.StockPrice = (1 + Convert.ToDouble((numericUpDownUnderLyingPrice.Value / 100))) * (position.StockPrice);
                   }
                   if (chkbxInterestRate.Checked)
                   {
                       position.InterestRate = (1 + Convert.ToDouble((numericUpDownIntRate.Value / 100))) * (position.InterestRate);
                   }
                   if (ckhbxExpiration.Checked)
                   {
                       position.DaysToExpiration = int.Parse(txtbxDaysToExpiration.Text);
                   }
               }
            }
        }
        private void btnSimulation_Click(object sender, EventArgs e)
        {

            try
            {
                if (ValidateRequest())
                {
                    CalculateChanges();
                    InputParametersCollection inputParametersCollection = new InputParametersCollection();
                    List<PranaPositionWithGreeks> list = GetSelectedRows();
                    List<string> symbolList = new List<string>();
                    foreach (PranaPositionWithGreeks position in list)
                    {
                        symbolList.Add(position.Symbol);
                        switch (position.AssetID)
                        {
                            case (int)AssetCategory.EquityOption:
                                InputParametersForGreeks inputParams = position.GetBasicInputParams();
                                if (position.ImpliedVol != 0.0)
                                {
                                    inputParams.ImpliedVol = (position.ImpliedVol) / 100;
                                }
                                if (position.StockPrice != 0.0)
                                {
                                    inputParams.StockPrice = position.StockPrice;
                                }
                                if (position.InterestRate != 0.0)
                                {
                                    inputParams.InterestRate = position.InterestRate;
                                }
                                if (position.DaysToExpiration != 0.0)
                                {
                                    inputParams.DaysToExpiration = position.DaysToExpiration;
                                }
                                
                                inputParametersCollection.Add(inputParams);
                                break;
                        }
                    }
                    string requestID = System.Guid.NewGuid().ToString();
                    _listRequest.Add(requestID);
                    inputParametersCollection.UserID = "1";
                    OptionClientManager.GetInstance.SendRequest(inputParametersCollection, requestID);

                    DisableForm();
                    btnSimulation.BackColor = Color.Red;

                    string requestIDSymbolData = System.Guid.NewGuid().ToString();
                    _listRequest.Add(requestIDSymbolData);
                    OptionClientManager.GetInstance.SendRequest(symbolList, requestIDSymbolData);

                    btnSimulation.Text = "Simulating...";
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

        private bool ValidateRequest()
        {
            if (RiskClientManager.GetInstance.ConnectionStatus != PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                MessageBox.Show("Server is not connected !", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (GetSelectedRows().Count < 1)
            {
                MessageBox.Show("Select some rows !", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void numericUpDownVol_ValueChanged(object sender, EventArgs e)
        {
            chkbxVol.Checked = true;
        }

        private void numericUpDownUnderLyingPrice_ValueChanged(object sender, EventArgs e)
        {
            chkbxUnderLyingPrice.Checked = true;
        }

        private void numericUpDownIntRate_ValueChanged(object sender, EventArgs e)
        {
            chkbxInterestRate.Checked = true;
        }

        private void txtbxDaysToExpiration_TextChanged(object sender, EventArgs e)
        {
            ckhbxExpiration.Checked = true;
        }
        private void UnwireEvents()
        {
            OptionClientManager.GetInstance.Level1SnapShotReceived -= new OptionClientManager.Level1SnapShotReceivedHandler(GetInstance_Level1SnapShotReceived);
            OptionClientManager.GetInstance.GreeksCalculated -= new OptionClientManager.GreeksCalculaterHandler(GetInstance_GreeksCalculated);
        }

        private void SetColumns()
        {
            List<string> columns = new List<string>();
            columns.Add("IsChecked");
            columns.Add("Symbol");
            columns.Add("SecurityName");
            columns.Add("UnderLyingSymbol");
            columns.Add("Fund");
            columns.Add("Strategy");
            columns.Add("Asset");
            columns.Add("Quantity");
            columns.Add("AvgPrice");

            columns.Add("Delta");
            columns.Add("Gamma");
            columns.Add("Thetha");
            columns.Add("Vega");
           
            columns.Add("ImpliedVol");
            columns.Add("StockPrice");
            columns.Add("StrikePrice");
            columns.Add("OptionPrice");
            columns.Add("InterestRate");
            columns.Add("DaysToExpiration");

            columns.Add("DeltaAdjExposure");
            columns.Add("DeltaAdjPosition");
            columns.Add("CostBasisPnl");
            columns.Add("DayPnl");

            //columns.Add("SectorName");
            //columns.Add("CountryName");
            //columns.Add("SecurityTypeName");

            Prana.Utilities.UIUtilities.UltraWinGridUtils.SetColumns(columns,grdData);
            SetColumnFormatting(grdData.DisplayLayout.Bands[0].Columns);
            SetColumnCustomizations();
        }
        private void SetColumnFormatting(ColumnsCollection columns)
        {
            columns["Quantity"].Format = "#,#.#";
            columns["AvgPrice"].Format = "#.00";
            columns["Delta"].Format = "#.0000";
            columns["Gamma"].Format = "#.0000";
            columns["Thetha"].Format = "#.0000";
            columns["Vega"].Format = "#.0000";
            columns["DeltaAdjExposure"].Format = "#,#.00";
            columns["DeltaAdjPosition"].Format = "#,#.00";
            columns["ImpliedVol"].Format = "#.0000";
            columns["StockPrice"].Format = "#.0000";
            columns["StrikePrice"].Format = "#.00";
            columns["OptionPrice"].Format = "#.0000";
        }
        private void SetColumnCustomizations()
        {
            grdData.CreationFilter = headerCheckBox;
            grdData.DisplayLayout.Bands[0].Columns["IsChecked"].CellClickAction = CellClickAction.Edit;
            grdData.DisplayLayout.Bands[0].Columns["IsChecked"].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            grdData.DisplayLayout.Bands[0].Columns["IsChecked"].Header.Caption = "";
            grdData.DisplayLayout.Bands[0].ColumnFilters["Asset"].FilterConditions.Add(FilterComparisionOperator.Equals, "EquityOption");
            SetColumnSummaries();
        }
        private void SetColumnSummaries()
        {
            UltraGridBand band = grdData.DisplayLayout.Bands[0];
            band.Summaries.Add(SummaryType.Sum, band.Columns["DeltaAdjExposure"]);
            band.Summaries.Add(SummaryType.Sum, band.Columns["DeltaAdjPosition"]);
            band.Summaries.Add(SummaryType.Sum, band.Columns["CostBasisPnl"]);
            band.Summaries.Add(SummaryType.Sum, band.Columns["DayPnl"]);

            foreach (SummarySettings var in band.Summaries)
            {
                var.DisplayFormat = "{0:#,#.00}";
            }
        }
        
        private void grdData_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            EnableForm();
        }
        
    }
}