using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Analytics.Classes;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlStressTest : UserControl, ILiveFeedCallback
    {
        #region Variables
        private Dictionary<string, List<PranaRiskObj>> _dictRiskObjs = new Dictionary<string, List<PranaRiskObj>>();
        private PranaRiskObjColl _riskObjcollection = new PranaRiskObjColl();
        private bool isAlreadyStarted = false;
        private DataTable _dtCalculatedRisks = new DataTable();
        private DataTable _dataPortfolio = new DataTable();
        private CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        public event EventHandler RefreshCompleted;
        DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        ProxyBase<IRiskServices> _riskServiceProxy = null;
        private delegate void UIThreadMarshellerUpdateToolStrip(string statusMessage);
        private Dictionary<string, List<string>> _dictInternationalSymbols = new Dictionary<string, List<string>>();
        private Dictionary<string, List<PranaRiskObj>> _dictOptUnderlyingWise = new Dictionary<string, List<PranaRiskObj>>();
        private List<string> _listRequestedSymbols = new List<string>();
        private BackgroundWorker _bgCalculate = null;
        private bool _isGroupRequest = false;
        private int _companyID = 0;
        double _currentMktValuePortfolio = 0.0;
        double _yesterdayMarkPrice = 0.0;
        Dictionary<int, double> _dictAccountWiseCash = new Dictionary<int, double>();
        GroupSortComparer _groupSortComparer = new GroupSortComparer();
        UltraGridColumn _columnSorted = null;
        List<string> GroupByColumnsCollection = new List<string>();
        private bool _isReloadingLayout = false;

        string _firstLevelGroup = string.Empty;
        string _secondLevelGroup = string.Empty;

        bool _isRiskCalculatedGroup = true;
        bool _isRiskCalculatedIndividual = true;
        private string tabStressTest = "Stress Test";
        private int _responseReceivedCount = 0;
        private int _requestedCallsCount = 0;
        private static readonly object _locker = new object();
        private RiskParamameter _riskParams = new RiskParamameter();
        #region Column Names
        private const string COL_IsChecked = "IsChecked";
        private const string COL_Symbol = "Symbol";
        private const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        private const string COL_Level1Name = "Level1Name";
        private const string COL_MasterFund = "MasterFund";
        private const string COL_Level2Name = "Level2Name";
        private const string COL_AssetName = "AssetName";
        private const string COL_Quantity = "Quantity";
        private const string COL_AvgPrice = "AvgPrice";
        private const string COL_SectorName = "SectorName";
        private const string COL_CountryName = "CountryName";
        private const string COL_UDAAsset = "UDAAsset";
        private const string COL_SecurityTypeName = "SecurityTypeName";
        private const string COL_AUECLocalDate = "AUECLocalDate";
        private const string COL_CompanyUserName = "CompanyUserName";
        private const string COL_CounterPartyName = "CounterPartyName";
        private const string COL_CurrencyName = "CurrencyName";
        private const string COL_ExchangeName = "ExchangeName";
        private const string COL_ExpirationDate = "ExpirationDate";
        private const string COL_ContractMultiplier = "ContractMultiplier";
        private const string COL_PositionType = "PositionType";
        private const string COL_PSSymbol = "PSSymbol";
        private const string COL_PutOrCall = "PutOrCall";
        private const string COL_SubSectorName = "SubSectorName";
        private const string COL_StrikePrice = "StrikePrice";
        private const string COL_CompanyName = "CompanyName";
        private const string COL_UnderlyingName = "UnderlyingName";
        private const string COL_TradeAttribute1 = "TradeAttribute1";
        private const string COL_TradeAttribute2 = "TradeAttribute2";
        private const string COL_TradeAttribute3 = "TradeAttribute3";
        private const string COL_TradeAttribute4 = "TradeAttribute4";
        private const string COL_TradeAttribute5 = "TradeAttribute5";
        private const string COL_TradeAttribute6 = "TradeAttribute6";
        private const string COL_BloombergSymbol = "BloombergSymbol";
        private const string COL_BloombergSymbolWithExchangeCode = "BloombergSymbolWithExchangeCode";
        private const string COL_FactSet = "FactSetSymbol";
        private const string COL_Activ = "ActivSymbol";
        private const string COL_IDCOSymbol = "IDCOSymbol";
        private const string COL_ISINSymbol = "ISINSymbol";
        private const string COL_SedolSymbol = "SedolSymbol";
        private const string COL_OSISymbol = "OSISymbol";
        private const string COL_CusipSymbol = "CusipSymbol";
        private const string COL_Delta = "Delta";
        private const string COL_DeltaAdjPosition = "DeltaAdjPosition";
        private const string COL_CurrentValue = "CurrentValue";
        private const string COL_ProjectedValue = "ProjectedValue";
        private const string COL_Beta = "Beta";
        private const string COL_Correlation = "Correlation";
        private const string COL_PercentChange = "PercentChange";
        private const string COL_PnlImpact = "PnlImpact";
        #endregion

        private delegate void UIThreadMarshellerSnapShotResponse(SymbolData symbolData, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData);
        private delegate void UIThreadMarsheller(PranaRequestCarrier pranaRequestCarrier);
        private delegate void UIThreadMarshellerForComplete(QueueMessage qMsg);
        public delegate void MsgreceivedInvokeDelegate(PranaRiskObjColl posList);

        public event EventHandler IncludeCashCheckChanged = null;
        #endregion

        #region Private
        private void SetDataTable()
        {
            try
            {
                _dataPortfolio.Columns.Add(new DataColumn("Current Portfolio Value (Local)", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Current Benchmark Value", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Projected Portfolio Value (Local)", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Projected Benchmark Value", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Beta", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("P&L Impact", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Correlation", typeof(string)));
                _dataPortfolio.Columns.Add(new DataColumn("Percentage Change in Portfolio Value", typeof(string)));
                portfolioResultsCtrl1.SetData(_dataPortfolio);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCalculatedRiskTable()
        {
            try
            {
                _dtCalculatedRisks.Columns.Add(new DataColumn("Grouping Criterion"));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_Correlation, typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_Beta, typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_PnlImpact, typeof(string)));
                DataColumn[] pkColArray = new DataColumn[1];
                pkColArray[0] = _dtCalculatedRisks.Columns["Grouping Criterion"];
                _dtCalculatedRisks.PrimaryKey = pkColArray;
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

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreateRiskServiceProxy()
        {
            try
            {
                _riskServiceProxy = new ProxyBase<IRiskServices>("PricingRiskServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCompanyID()
        {
            try
            {
                DataTable Company = CachedDataManager.GetInstance.GetCompany();
                foreach (DataRow dr in Company.Rows)
                {
                    _companyID = int.Parse(dr["CompanyID"].ToString());
                    break;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool ValidateRequest()
        {
            try
            {
                if (GetSelectedRows().Count < 1)
                {
                    MessageBox.Show("Select some rows !", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (DateTime.Compare(datePickerStartdate.DateTime, datePickerEnddate.DateTime) == 0)
                {
                    MessageBox.Show("Please select a Date Range");
                    return false;
                }
                if (cmbbxBenchMark.Value == null)
                {
                    MessageBox.Show("Please select a Benchmark");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        private PranaRiskObjColl GetSelectedRows()
        {
            PranaRiskObjColl riskObjcollection = new PranaRiskObjColl();
            try
            {
                UltraGridRow[] rows = grdData.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                    {
                        PranaRiskObj pranaPosWithGreeks = (PranaRiskObj)row.ListObject;
                        riskObjcollection.Add(pranaPosWithGreeks);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return riskObjcollection;
        }

        private void GetData()
        {
            try
            {
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Refreshing Positions...";
                _dictRiskObjs.Clear();
                _riskObjcollection.Clear();
                _dtCalculatedRisks.Clear();
                GetPositionsforRisk();
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

        public async void GetPositionsforRisk()
        {
            try
            {
                bool isCashIncluded = false;
                if (!isAlreadyStarted)
                {
                    isCashIncluded = RiskPreferenceManager.RiskPrefernece.IncludeCash;
                }
                else
                {
                    isCashIncluded = chkIncludeCash.Checked;
                }
                List<Object> riskData = await CentralRiskPositionsManager.GetInstance.GetPSPositionsAsRiskPref(isCashIncluded, _dictAccountWiseCash);
                PranaRiskObjColl riskObjColl = (PranaRiskObjColl)riskData[0];
                _dictAccountWiseCash = (Dictionary<int, double>)riskData[1];
                GetInstance_PositionReceived(riskObjColl);
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

        private void AddPositionToDict(PranaRiskObj riskObj)
        {
            try
            {
                if (!_dictRiskObjs.ContainsKey(riskObj.Symbol))
                {
                    List<PranaRiskObj> list = new List<PranaRiskObj>();
                    list.Add(riskObj);
                    _dictRiskObjs.Add(riskObj.Symbol, list);
                }
                else
                {
                    _dictRiskObjs[riskObj.Symbol].Add(riskObj);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdatePortfolioRisk(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    UIThreadMarsheller mi = new UIThreadMarsheller(UpdatePortfolioRisk);
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        if (pranaRequestCarrier != null)
                        {
                            CalculatePortfolioCurrentMktValue();

                            if (chkIncludeCash.Checked)
                            {
                                foreach (double cashImpact in _dictAccountWiseCash.Values)
                                {
                                    _currentMktValuePortfolio += cashImpact;
                                }
                            }
                            double percentageChangesInPortfolio = 0;
                            double percentageChangeInBenchMark = Convert.ToDouble(numericUpDownIndex.Value);
                            pranaRequestCarrier.PortFolioValue = Math.Round(_currentMktValuePortfolio);
                            if (pranaRequestCarrier.PortFolioValue != 0)
                            {
                                percentageChangesInPortfolio = ((pranaRequestCarrier.PNLImpact) / Math.Abs(pranaRequestCarrier.PortFolioValue)) * 100;
                            }
                            double changeinPortfolioValue = pranaRequestCarrier.PNLImpact;
                            double projectedPortfolioValue = Math.Round(pranaRequestCarrier.PortFolioValue);
                            if (percentageChangeInBenchMark != 0)
                            {
                                projectedPortfolioValue = Math.Round(pranaRequestCarrier.PortFolioValue + changeinPortfolioValue);
                            }
                            else
                            {
                                percentageChangesInPortfolio = 0;
                            }
                            DataRow dr = _dataPortfolio.NewRow();
                            dr["Beta"] = pranaRequestCarrier.Beta.ToString("#,0.0000");
                            dr["Correlation"] = pranaRequestCarrier.Correlation.ToString("#,0.0000");
                            dr["Current Portfolio Value (Local)"] = pranaRequestCarrier.PortFolioValue.ToString("#,0.00");
                            dr["Current Benchmark Value"] = pranaRequestCarrier.BenchMarkValue.ToString("#,0.00");
                            dr["Percentage Change in Portfolio Value"] = percentageChangesInPortfolio.ToString("#,0.00");
                            dr["Projected Portfolio Value (Local)"] = projectedPortfolioValue.ToString("#,0.00");
                            dr["Projected Benchmark Value"] = (pranaRequestCarrier.BenchMarkValue * (1 + (percentageChangeInBenchMark / 100.0))).ToString("#,0.00");
                            dr["P&L Impact"] = pranaRequestCarrier.PNLImpact.ToString("#,0.0000");
                            _dataPortfolio.Rows.Add(dr);
                            portfolioResultsCtrl1.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateIndividualRisk(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    UIThreadMarsheller mi = new UIThreadMarsheller(UpdateIndividualRisk);
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        if (pranaRequestCarrier != null)
                        {
                            double percentChange = 0;
                            double currentValue = 0;
                            double projectedValue = 0;
                            List<int> cashUsedAccountIds = new List<int>();
                            double percentageChangeInBenchMark = Convert.ToDouble(numericUpDownIndex.Value);

                            foreach (KeyValuePair<string, PranaRiskResult> riskReply in pranaRequestCarrier.IndividualSymbolList)
                            {
                                int symbolLen = riskReply.Value.Symbol.Length;
                                string positionType = riskReply.Key.Substring(symbolLen);
                                List<PranaRiskObj> list = _dictRiskObjs[riskReply.Value.Symbol];
                                foreach (PranaRiskObj riskObjToUpdate in list)
                                {
                                    if (riskObjToUpdate.IsChecked && riskObjToUpdate.PositionType.Equals(positionType))
                                    {
                                        currentValue = CalculateCurrentMarketValue(riskObjToUpdate);
                                        if (chkIncludeCash.Checked && !cashUsedAccountIds.Contains(riskObjToUpdate.Level1ID) && _dictAccountWiseCash.ContainsKey(riskObjToUpdate.Level1ID))
                                        {
                                            currentValue += _dictAccountWiseCash[riskObjToUpdate.Level1ID];
                                            cashUsedAccountIds.Add(riskObjToUpdate.Level1ID);
                                        }
                                        if (currentValue != 0)
                                        {
                                            percentChange = ((riskReply.Value.PNLImpact / Math.Abs(Math.Round(currentValue))) * 100);
                                        }
                                        if (percentageChangeInBenchMark == 0)
                                        {
                                            projectedValue = Math.Round(currentValue);
                                            percentChange = 0;
                                        }
                                        else
                                        {
                                            projectedValue = Math.Round(currentValue + riskReply.Value.PNLImpact);
                                        }
                                        riskObjToUpdate.SetStressTestData(riskReply.Value, percentChange, currentValue, projectedValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateGroupRisk(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    UIThreadMarsheller mi = new UIThreadMarsheller(UpdateGroupRisk);
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        if (pranaRequestCarrier != null)
                        {
                            if (!_dtCalculatedRisks.Rows.Cast<DataRow>().Any(dataRow => dataRow["Grouping Criterion"].ToString() == pranaRequestCarrier.GroupingName))
                            {
                                DataRow dr = _dtCalculatedRisks.NewRow();
                                dr["Grouping Criterion"] = pranaRequestCarrier.GroupingName;
                                dr[COL_Correlation] = pranaRequestCarrier.Correlation;
                                dr[COL_Beta] = pranaRequestCarrier.Beta;
                                dr[COL_PnlImpact] = pranaRequestCarrier.PNLImpact;
                                _dtCalculatedRisks.Rows.Add(dr);
                                dr.AcceptChanges();
                            }
                            else
                            {
                                Logger.HandleException(new Exception("Duplicate entry for column Grouping Criterion:- "
                                    + pranaRequestCarrier.GroupingName), LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
        }

        private void grdData_ExternalSummaryValueRequested(object sender, ExternalSummaryValueEventArgs e)
        {
            if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_Correlation || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_Beta || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_PnlImpact)
            {
                DataRow dr;
                string columnKey = e.SummaryValue.SummarySettings.SourceColumn.Key;
                if (columnKey == COL_PnlImpact)
                {
                    columnKey = "P&L Impact";
                }
                Infragistics.Win.UltraWinGrid.RowsCollection rows = e.SummaryValue.ParentRows;
                if (rows != null && rows.ParentRow != null && _dtCalculatedRisks.Columns.Contains("Grouping Criterion"))
                {
                    string groupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow)).ValueAsDisplayText;

                    if (!string.IsNullOrEmpty(_secondLevelGroup) && rows.ParentRow.HasParent())
                    {
                        string parentGroupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow.ParentRow)).ValueAsDisplayText;
                        dr = _dtCalculatedRisks.Rows.Find(parentGroupByValue + "|" + groupByValue);
                    }
                    else
                    {
                        dr = _dtCalculatedRisks.Rows.Find(_firstLevelGroup + "|" + groupByValue);
                    }
                    if (dr != null && _dtCalculatedRisks.Columns.Contains(columnKey))
                    {
                        e.SummaryValue.SetExternalSummaryValue(dr[columnKey]);
                    }
                    else
                    {
                        e.SummaryValue.SetExternalSummaryValue(0);
                    }
                }
                else
                {
                    if (rows != null && rows.ParentRow == null && _dataPortfolio.Rows.Count > 0)
                    {
                        dr = _dataPortfolio.Rows[0];

                        if (dr != null && _dataPortfolio.Columns.Contains(columnKey))
                        {
                            e.SummaryValue.SetExternalSummaryValue(dr[columnKey]);
                        }
                        else
                        {
                            e.SummaryValue.SetExternalSummaryValue(0);
                        }
                    }
                    else
                    {
                        e.SummaryValue.SetExternalSummaryValue(0);
                    }
                }
            }
            else
            {
                e.SummaryValue.SetExternalSummaryValue(0);
            }
        }

        private void DisableForm()
        {
            try
            {
                datePickerStartdate.Enabled = false;
                datePickerEnddate.Enabled = false;
                btnCalculate.Enabled = false;
                cmbbxBenchMark.Enabled = false;
                chkIncludeCash.Enabled = false;
                numericUpDownIndex.Enabled = false;

                //When we disable Grid then Summary of summary rows got cleared so we are not disabling grid (disabling only editable columns)
                //grdData.Enabled = false;
                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.Disabled;
                grdData.CreationFilter = null;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void EnableForm()
        {
            try
            {
                grdData.ResumeRowSynchronization();
                grdData.ResumeSummaryUpdates(true);
                grdData.Rows.Refresh(RefreshRow.ReloadData);

                btnCalculate.BackColor = Color.FromArgb(104, 156, 46);
                btnCalculate.Text = "Calculate";
                btnCalculate.Enabled = true;
                cmbbxBenchMark.Enabled = true;
                chkIncludeCash.Enabled = true;
                datePickerStartdate.Enabled = true;
                datePickerEnddate.Enabled = true;
                numericUpDownIndex.Enabled = true;
                timerSnapShot.Stop();

                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.AllowEdit;
                grdData.CreationFilter = headerCheckBox;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumns()
        {
            try
            {
                if (RiskLayoutManager.RiskLayout.StressTestColumns.Count > 0)
                {
                    LoadColumnsFromXML();
                }
                else
                {
                    LoadColumns();
                }
                SetColumnFormatting();
                SetColumnCustomizations();
                SetColumnSummaries(grdData);
                SetGroupByColumns(RiskPreferenceManager.RiskPrefernece.IncludeCash);
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

        private void LoadColumnsFromXML()
        {
            try
            {
                List<ColumnData> listColData = RiskLayoutManager.RiskLayout.StressTestColumns;
                List<SortedColumnData> listGroupByColumnsCollection = RiskLayoutManager.RiskLayout.StressTestGroupByColumnsCollection;
                RiskLayoutManager.SetGridColumnLayout(grdData, listColData, listGroupByColumnsCollection, GetAllDisplayableColumns());
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

        private void LoadColumns()
        {
            try
            {
                List<string> colAll = GetAllDisplayableColumns();
                List<string> colDefault = GetAllDefaultColumns();
                List<string> colVisible = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(RiskPreferenceManager.RiskPrefernece.StressTestColumns, ',');

                //Pref File Has No Columns
                if (colVisible.Count < 1)
                {
                    colVisible.AddRange(colDefault);
                }

                ColumnsCollection gridColumns = grdData.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                    if (!colAll.Contains(gridCol.Key))
                    {
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }
                int visiblePos = 1;
                foreach (string col in colVisible)
                {
                    if (!String.IsNullOrEmpty(col) && gridColumns.Exists(col))
                    {
                        gridColumns[col].Hidden = false;
                        gridColumns[col].Header.VisiblePosition = visiblePos;
                        gridColumns[col].Width = 100;
                        visiblePos++;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> GetAllDisplayableColumns()
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns();
                colAll.AddRange(colDefault);
                colAll.Add(COL_AUECLocalDate);
                colAll.Add(COL_CompanyUserName);
                colAll.Add(COL_CounterPartyName);
                colAll.Add(COL_CurrencyName);
                colAll.Add(COL_ExchangeName);
                colAll.Add(COL_ExpirationDate);
                colAll.Add(COL_ContractMultiplier);
                colAll.Add(COL_PositionType);
                colAll.Add(COL_PSSymbol);
                colAll.Add(COL_PutOrCall);
                colAll.Add(COL_SubSectorName);
                colAll.Add(COL_StrikePrice);
                colAll.Add(COL_CompanyName);
                colAll.Add(COL_UnderlyingName);
                colAll.Add(COL_TradeAttribute1);
                colAll.Add(COL_TradeAttribute2);
                colAll.Add(COL_TradeAttribute3);
                colAll.Add(COL_TradeAttribute4);
                colAll.Add(COL_TradeAttribute5);
                colAll.Add(COL_TradeAttribute6);
                colAll.Add(COL_BloombergSymbol);
                colAll.Add(COL_FactSet);
                colAll.Add(COL_Activ);
                colAll.Add(COL_IDCOSymbol);
                colAll.Add(COL_ISINSymbol);
                colAll.Add(COL_SedolSymbol);
                colAll.Add(COL_OSISymbol);
                colAll.Add(COL_CusipSymbol);
                colAll.Add(COL_Delta);
                colAll.Add(COL_DeltaAdjPosition);
                colAll.Add(COL_CurrentValue);
                colAll.Add(COL_ProjectedValue);
                colAll.Add(COL_Beta);
                colAll.Add(COL_Correlation);
                colAll.Add(COL_PercentChange);
                colAll.Add(COL_PnlImpact);
                colAll.Add(COL_BloombergSymbolWithExchangeCode);
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
            return colAll;
        }

        private List<string> GetAllDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                colDefault.Add(COL_IsChecked);
                colDefault.Add(COL_Symbol);
                colDefault.Add(COL_UnderlyingSymbol);
                colDefault.Add(COL_Level1Name);
                colDefault.Add(COL_MasterFund);
                colDefault.Add(COL_Level2Name);
                colDefault.Add(COL_AssetName);
                colDefault.Add(COL_Quantity);
                colDefault.Add(COL_AvgPrice);
                colDefault.Add(COL_SectorName);
                colDefault.Add(COL_CountryName);
                colDefault.Add(COL_UDAAsset);
                colDefault.Add(COL_SecurityTypeName);
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
            return colDefault;
        }

        private void SetColumnFormatting()
        {
            try
            {
                ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
                columns[COL_Quantity].Format = "#,#.#";
                columns[COL_AvgPrice].Format = "#.00";
                columns[COL_Correlation].Format = "#.0000";
                columns[COL_Beta].Format = "#.0000";
                columns[COL_Delta].Format = "#.0000";
                columns[COL_DeltaAdjPosition].Format = "#,#.#";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnCustomizations()
        {
            try
            {
                grdData.CreationFilter = headerCheckBox;
                UltraGridBand band = grdData.DisplayLayout.Bands[0];
                band.Columns[COL_IsChecked].CellClickAction = CellClickAction.Edit;
                band.Columns[COL_IsChecked].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[COL_IsChecked].Header.Caption = "";
                band.Columns[COL_IsChecked].Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                band.Columns[COL_IsChecked].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[COL_IsChecked].AllowGroupBy = DefaultableBoolean.False;
                band.Columns[COL_PutOrCall].CellActivation = Activation.NoEdit;
                band.Columns[COL_Level1Name].Header.Caption = "Account";
                band.Columns[COL_MasterFund].Header.Caption = "Master Fund";
                band.Columns[COL_Level2Name].Header.Caption = "Strategy";
                band.Columns[COL_AssetName].Header.Caption = "Asset";
                band.Columns[COL_AUECLocalDate].Header.Caption = "Trade Date";
                band.Columns[COL_AvgPrice].Header.Caption = "Cost Basis (Local)";
                band.Columns[COL_CompanyUserName].Header.Caption = "User";
                band.Columns[COL_CounterPartyName].Header.Caption = ApplicationConstants.CONST_BROKER;
                band.Columns[COL_CountryName].Header.Caption = "Country";
                band.Columns[COL_CurrencyName].Header.Caption = "Currency";
                band.Columns[COL_ExchangeName].Header.Caption = "Exchange";
                band.Columns[COL_ExpirationDate].Header.Caption = "Expiration Date";
                band.Columns[COL_PositionType].Header.Caption = "Position Type";
                band.Columns[COL_SectorName].Header.Caption = "Sector";
                band.Columns[COL_CompanyName].Header.Caption = "Security Name";
                band.Columns[COL_SecurityTypeName].Header.Caption = "Security Type";
                band.Columns[COL_StrikePrice].Header.Caption = "Strike Price";
                band.Columns[COL_SubSectorName].Header.Caption = "Sub Sector";
                band.Columns[COL_UnderlyingName].Header.Caption = "Underlying";
                band.Columns[COL_UnderlyingSymbol].Header.Caption = "Underlying Symbol";
                band.Columns[COL_PSSymbol].Header.Caption = "PS Symbol";
                band.Columns[COL_PutOrCall].Header.Caption = "Put/Call";
                band.Columns[COL_ContractMultiplier].Header.Caption = "Multiplier";
                band.Columns[COL_TradeAttribute1].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 1");
                band.Columns[COL_TradeAttribute2].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 2");
                band.Columns[COL_TradeAttribute3].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 3");
                band.Columns[COL_TradeAttribute4].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 4");
                band.Columns[COL_TradeAttribute5].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 5");
                band.Columns[COL_TradeAttribute6].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 6");
                band.Columns[COL_BloombergSymbol].Header.Caption = "Bloomberg";
                band.Columns[COL_BloombergSymbolWithExchangeCode].Header.Caption = "Bloomberg Symbol(with Exchange Code)";
                band.Columns[COL_FactSet].Header.Caption = "FactSet Symbol";
                band.Columns[COL_Activ].Header.Caption = "ACTIV Symbol";
                band.Columns[COL_IDCOSymbol].Header.Caption = "IDCO";
                band.Columns[COL_ISINSymbol].Header.Caption = "ISIN";
                band.Columns[COL_SedolSymbol].Header.Caption = "SEDOL";
                band.Columns[COL_OSISymbol].Header.Caption = "OSI";
                band.Columns[COL_CusipSymbol].Header.Caption = "CUSIP";
                band.Columns[COL_UDAAsset].Header.Caption = "User Asset";
                band.Columns[COL_DeltaAdjPosition].Header.Caption = "Delta Position";
                band.Columns[COL_CurrentValue].Header.Caption = "Current Portfolio Value (Local)";
                band.Columns[COL_ProjectedValue].Header.Caption = "Projected Portfolio Value (Local)";
                band.Columns[COL_Beta].Header.Caption = "Beta";
                band.Columns[COL_Correlation].Header.Caption = "Correlation";
                band.Columns[COL_PercentChange].Header.Caption = "% Change";
                band.Columns[COL_Quantity].Header.Caption = "Position";
                band.Columns[COL_PnlImpact].Header.Caption = "P&L Impact";

                band.Columns[COL_AUECLocalDate].CellActivation = Activation.NoEdit;
                band.Columns[COL_ExpirationDate].CellActivation = Activation.NoEdit;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnSummaries(UltraGrid grid)
        {
            try
            {
                UltraGridBand band = grid.DisplayLayout.Bands[0];
                foreach (SummarySettings summary in band.Summaries)
                {
                    summary.DisplayFormat = "{0}";
                }
                RiskSummaryFactory summFactory = new RiskSummaryFactory();
                foreach (UltraGridColumn ultraGridColumn in band.Columns)
                {
                    switch (ultraGridColumn.Key)
                    {
                        case COL_Symbol:
                        case COL_UnderlyingSymbol:
                        case COL_Level1Name:
                        case COL_MasterFund:
                        case COL_Level2Name:
                        case COL_AssetName:
                        case COL_SectorName:
                        case COL_CountryName:
                        case COL_UDAAsset:
                        case COL_SecurityTypeName:
                        case COL_CompanyUserName:
                        case COL_CounterPartyName:
                        case COL_CurrencyName:
                        case COL_ExchangeName:
                        case COL_PositionType:
                        case COL_PSSymbol:
                        case COL_PutOrCall:
                        case COL_SubSectorName:
                        case COL_CompanyName:
                        case COL_UnderlyingName:
                        case COL_TradeAttribute1:
                        case COL_TradeAttribute2:
                        case COL_TradeAttribute3:
                        case COL_TradeAttribute4:
                        case COL_TradeAttribute5:
                        case COL_TradeAttribute6:
                        case COL_BloombergSymbol:
                        case COL_BloombergSymbolWithExchangeCode:
                        case COL_Activ:
                        case COL_FactSet:
                        case COL_IDCOSymbol:
                        case COL_ISINSymbol:
                        case COL_SedolSymbol:
                        case COL_OSISymbol:
                        case COL_CusipSymbol:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_DeltaAdjPosition:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_ContractMultiplier:
                        case COL_Delta:
                        case COL_PercentChange:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_StrikePrice:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.##}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_CurrentValue:
                        case COL_ProjectedValue:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Quantity:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_AvgPrice:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcWeightedSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_AUECLocalDate:
                        case COL_ExpirationDate:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:MM/dd/yyyy}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Beta:
                        case COL_Correlation:
                        case COL_PnlImpact:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.External, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;
                    }
                }
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
                grid.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                grid.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;
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

        private void BindBenchMarkCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("BenchMarkSymbol");
                dt.Columns.Add("SymbolDisplayName");
                DataSet ds = PositionDataManager.GetBenchMarks();
                dt = ds.Tables[0];
                cmbbxBenchMark.DataSource = dt;
                cmbbxBenchMark.DisplayLayout.Bands[0].Columns["SymbolDisplayName"].Width = 148;
                cmbbxBenchMark.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbbxBenchMark.DisplayLayout.Bands[0].Columns["BenchMarkSymbol"].Hidden = true;
                cmbbxBenchMark.DisplayMember = "SymbolDisplayName";
                cmbbxBenchMark.ValueMember = "BenchMarkSymbol";
                SetDefaultBenchMark(dt);
                cmbbxBenchMark.DataBind();
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

        private List<string> GetAllowedGroupByColumns(bool cashInclusion)
        {
            List<string> allowedGroupByColumns = new List<string>();
            try
            {
                allowedGroupByColumns.Add(COL_Level1Name);
                allowedGroupByColumns.Add(COL_MasterFund);
                if (!cashInclusion)
                {
                    allowedGroupByColumns.Add(COL_Level2Name);
                    allowedGroupByColumns.Add(COL_Symbol);
                    allowedGroupByColumns.Add(COL_SectorName);
                    allowedGroupByColumns.Add(COL_CountryName);
                    allowedGroupByColumns.Add(COL_SecurityTypeName);
                    allowedGroupByColumns.Add(COL_TradeAttribute1);
                    allowedGroupByColumns.Add(COL_TradeAttribute2);
                    allowedGroupByColumns.Add(COL_TradeAttribute3);
                    allowedGroupByColumns.Add(COL_TradeAttribute4);
                    allowedGroupByColumns.Add(COL_TradeAttribute5);
                    allowedGroupByColumns.Add(COL_TradeAttribute6);
                    allowedGroupByColumns.Add(COL_UnderlyingSymbol);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allowedGroupByColumns;
        }

        private void SetGroupByColumns(bool cashInclusion)
        {
            try
            {
                ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    column.AllowGroupBy = DefaultableBoolean.False;
                    if (GetAllowedGroupByColumns(cashInclusion).Contains(column.Key))
                    {
                        column.AllowGroupBy = DefaultableBoolean.True;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LoadPreferences()
        {
            try
            {
                if (RiskPreferenceManager.RiskPrefernece.StressTestTabBenchMarkSymbolName != null)
                {
                    cmbbxBenchMark.Value = RiskPreferenceManager.RiskPrefernece.StressTestTabBenchMarkSymbolName;
                }
                numericUpDownIndex.Value = RiskPreferenceManager.RiskPrefernece.BenchMarkPercentMove;
                chkIncludeCash.Checked = RiskPreferenceManager.RiskPrefernece.IncludeCash;
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

        private void SetDefaultBenchMark(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    cmbbxBenchMark.Value = dt.Rows[0].ItemArray[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["SymbolDisplayName"].ToString().Contains("S&P"))
                        {
                            cmbbxBenchMark.Value = row.ItemArray[0];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (ValidateRequest())
                    {
                        lock (_locker)
                        {
                            _responseReceivedCount = 0;
                            _requestedCallsCount = 0;
                        }
                        portfolioResultsCtrl1.Refresh();
                        btnCalculate.BackColor = Color.Red;
                        btnCalculate.Text = "Calculating...";
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Calculating...";
                        DisableForm();
                        SendSnapShotRequest();
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

        private bool CheckRiskServiceConnected()
        {
            try
            {
                return _riskServiceProxy.InnerChannel.CheckRiskServiceConnected();

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
            return false;
        }

        private void Calculate()
        {
            try
            {
                if (!CheckRiskServiceConnected())
                {
                    UpdateToolStripStatus("Risk Server not connected to Portfolio Science.");
                    return;
                }
                // Updated the risk preferences as the preferences are only kept at the client side in a xml file and
                // the preferences has to be communicated to the pricing server as it is using these in several calculations
                _riskServiceProxy.InnerChannel.UpdateRiskPreferences(RiskPreferenceManager.RiskPrefernece);

                _isGroupRequest = false;
                _dataPortfolio.Rows.Clear();

                double percentageChangeInBenchMark = Convert.ToDouble(numericUpDownIndex.Value);
                string benchmark = cmbbxBenchMark.Value.ToString();
                object[] arguments = new object[5];
                arguments[0] = benchmark;
                arguments[1] = percentageChangeInBenchMark;

                #region Individual
                _isRiskCalculatedIndividual = false;
                PranaRiskObjColl selectedRows = GetSelectedRows();
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                _riskParams.IsCorrelationRequired = gridBand.Columns["Correlation"].IsVisibleInLayout ? true : false;
                _riskParams.IsBetaRequired = gridBand.Columns["Beta"].IsVisibleInLayout ? true : false;
                _riskParams.IsImpactRequired = gridBand.Columns["PnlImpact"].IsVisibleInLayout ? true : false;

                PranaRequestCarrier pranaRequestCarrierPortfolio = new PranaRequestCarrier(selectedRows, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchmark, "1", percentageChangeInBenchMark, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParams);
                pranaRequestCarrierPortfolio.GroupingName = "Portfolio";
                arguments[2] = pranaRequestCarrierPortfolio;

                PranaRequestCarrier pranaRequestCarrierForBeta = new PranaRequestCarrier(selectedRows, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchmark, "1", percentageChangeInBenchMark, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, true, _riskParams);
                arguments[3] = pranaRequestCarrierForBeta;
                #endregion

                #region Group
                if (!string.IsNullOrEmpty(_firstLevelGroup))
                {
                    _isGroupRequest = true;
                    _isRiskCalculatedGroup = false;
                    _dtCalculatedRisks.Rows.Clear();
                    Dictionary<string, PranaRiskObjColl> dictGroupedRiskColl = new Dictionary<string, PranaRiskObjColl>();
                    foreach (UltraGridRow groupRow in grdData.Rows)
                    {
                        if (!groupRow.IsFilteredOut)
                        {
                            UltraGridGroupByRow groupByRow = (UltraGridGroupByRow)groupRow;
                            if (!string.IsNullOrEmpty(_secondLevelGroup))
                            {
                                foreach (UltraGridRow groupRowSecondLevel in groupRow.ChildBands[0].Rows)
                                {
                                    if (!groupRowSecondLevel.IsFilteredOut)
                                    {
                                        PranaRiskObjColl groupedRiskCollSecondLevel = new PranaRiskObjColl();
                                        UltraGridRow[] rowsSecondLevel = groupRowSecondLevel.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
                                        foreach (UltraGridRow rowSecondLevel in rowsSecondLevel)
                                        {
                                            PranaRiskObj riskObjIndividualSecondLevel = (PranaRiskObj)rowSecondLevel.ListObject;
                                            if (riskObjIndividualSecondLevel.IsChecked)
                                            {
                                                groupedRiskCollSecondLevel.Add((PranaRiskObj)riskObjIndividualSecondLevel.Clone());
                                            }
                                        }
                                        UltraGridGroupByRow groupByRowSecondLevel = (UltraGridGroupByRow)groupRowSecondLevel;
                                        if (groupedRiskCollSecondLevel.Count > 0 && !dictGroupedRiskColl.ContainsKey(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString()))
                                        {
                                            dictGroupedRiskColl.Add(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString(), groupedRiskCollSecondLevel);
                                        }
                                    }
                                }
                            }
                            PranaRiskObjColl groupedRiskColl = new PranaRiskObjColl();
                            UltraGridRow[] rows = groupRow.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
                            foreach (UltraGridRow row in rows)
                            {
                                PranaRiskObj riskObjIndividual = (PranaRiskObj)row.ListObject;
                                if (riskObjIndividual.IsChecked)
                                {
                                    groupedRiskColl.Add((PranaRiskObj)riskObjIndividual.Clone());
                                }
                            }
                            if (groupedRiskColl.Count > 0 && !dictGroupedRiskColl.ContainsKey(_firstLevelGroup + "|" + groupByRow.Value.ToString()))
                            {
                                dictGroupedRiskColl.Add(_firstLevelGroup + "|" + groupByRow.Value.ToString(), groupedRiskColl);
                            }
                        }
                    }
                    arguments[4] = dictGroupedRiskColl;
                }
                #endregion
                CalculatePNLStressImpactAndBeta(arguments);
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

        private void SendSnapShotRequest()
        {
            try
            {
                _listRequestedSymbols.Clear();
                PranaRiskObjColl riskObjColl = GetSelectedRows();

                List<string> symbols = new List<string>();
                Dictionary<string, fxInfo> dictfxSymbols = new Dictionary<string, fxInfo>();
                _dictInternationalSymbols.Clear();
                _dictOptUnderlyingWise.Clear();
                _currentMktValuePortfolio = 0;

                foreach (PranaRiskObj riskObj in riskObjColl)
                {
                    if (riskObj.AssetID == (int)AssetCategory.EquityOption || riskObj.AssetID == (int)AssetCategory.FutureOption)
                    {
                        if (!_dictOptUnderlyingWise.ContainsKey(riskObj.UnderlyingSymbol))
                        {
                            List<PranaRiskObj> listOptions = new List<PranaRiskObj>();
                            listOptions.Add(riskObj);
                            _dictOptUnderlyingWise.Add(riskObj.UnderlyingSymbol, listOptions);
                        }
                        else
                        {
                            _dictOptUnderlyingWise[riskObj.UnderlyingSymbol].Add(riskObj);
                        }
                        if (!symbols.Contains(riskObj.UnderlyingSymbol))
                        {
                            symbols.Add(riskObj.UnderlyingSymbol);
                        }
                    }
                    if (riskObj.AssetID == (int)AssetCategory.FX || riskObj.AssetID == (int)AssetCategory.FXForward)
                    {
                        if (!dictfxSymbols.ContainsKey(riskObj.Symbol))
                        {
                            fxInfo fxReqObj = new fxInfo();
                            fxReqObj.PranaSymbol = riskObj.Symbol;
                            fxReqObj.FromCurrencyID = riskObj.LeadCurrencyID;
                            fxReqObj.ToCurrencyID = riskObj.VsCurrencyID;
                            fxReqObj.CategoryCode = (AssetCategory)riskObj.AssetID;
                            dictfxSymbols.Add(riskObj.Symbol, fxReqObj);
                        }
                    }
                    else if (!symbols.Contains(riskObj.Symbol))
                    {
                        symbols.Add(riskObj.Symbol);
                    }
                    //CHMW-3132	Account wise fx rate handling for expiration settlement           
                    int accountBaseCurrencyID;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(riskObj.Level1ID))
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[riskObj.Level1ID];
                    }
                    else
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    if (riskObj.CurrencyID != accountBaseCurrencyID)
                    {
                        string forexSymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencies(riskObj.CurrencyID, accountBaseCurrencyID);
                        if (!dictfxSymbols.ContainsKey(forexSymbol))
                        {
                            fxInfo fxReqObj = new fxInfo();
                            fxReqObj.PranaSymbol = forexSymbol;
                            fxReqObj.FromCurrencyID = riskObj.CurrencyID;
                            fxReqObj.ToCurrencyID = accountBaseCurrencyID;
                            fxReqObj.CategoryCode = AssetCategory.Forex;
                            dictfxSymbols.Add(forexSymbol, fxReqObj);
                        }
                        if (_dictInternationalSymbols.ContainsKey(forexSymbol))
                        {
                            List<string> listSymbols = _dictInternationalSymbols[forexSymbol];
                            if (!(listSymbols.Contains(riskObj.Symbol)))
                            {
                                listSymbols.Add(riskObj.Symbol);
                            }
                        }
                        else
                        {
                            List<string> listSymbolsNew = new List<string>();
                            listSymbolsNew.Add(riskObj.Symbol);
                            _dictInternationalSymbols.Add(forexSymbol, listSymbolsNew);
                        }
                    }
                }
                _pricingServiceProxy.InnerChannel.RequestSnapshot(symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                if (dictfxSymbols.Count > 0)
                {
                    List<fxInfo> listFxSymbols = new List<fxInfo>(dictfxSymbols.Values);
                    _pricingServiceProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                }

                _listRequestedSymbols.AddRange(symbols);
                _listRequestedSymbols.AddRange(dictfxSymbols.Keys);

                timerSnapShot.Interval = 15000;
                timerSnapShot.Start();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        async void CalculatePNLStressImpactAndBeta(object[] arguments)
        {
            try
            {
                if (arguments != null)
                {
                    string benchMarkSymbol = arguments[0].ToString();
                    double percentageChangeInBenchMark = double.Parse(arguments[1].ToString());

                    PranaRequestCarrier pranaRequestCarrierPortfolio = arguments[2] as PranaRequestCarrier;
                    PranaRequestCarrier pranaRequestCarrierForBeta = arguments[3] as PranaRequestCarrier;
                    lock (_locker)
                    {
                        _requestedCallsCount = _requestedCallsCount + 1;
                    }
                    var pranaRequestCarrierTask = _riskServiceProxy.InnerChannel.CalculateStressTestData(pranaRequestCarrierPortfolio, pranaRequestCarrierForBeta).ContinueWith(CompleteWorkForNonGroupAfterRiskResponse);

                    if (_isGroupRequest)
                    {
                        _dtCalculatedRisks.Rows.Clear();
                        Dictionary<string, PranaRiskObjColl> dictgroupedRiskColl = (Dictionary<string, PranaRiskObjColl>)arguments[4];
                        List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>(dictgroupedRiskColl.Count);
                        lock (_locker)
                        {
                            _requestedCallsCount = _requestedCallsCount + dictgroupedRiskColl.Count;
                        }
                        foreach (KeyValuePair<string, PranaRiskObjColl> kp in dictgroupedRiskColl)
                        {
                            PranaRiskObjColl groupedRiskColl = kp.Value;
                            PranaRequestCarrier pranaRequestCarrierGroup = new PranaRequestCarrier(groupedRiskColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchMarkSymbol, "1", percentageChangeInBenchMark, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParams);
                            pranaRequestCarrierGroup.GroupingName = kp.Key.ToString();

                            PranaRequestCarrier pranaRequestCarrierForBetaGroup = new PranaRequestCarrier(groupedRiskColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchMarkSymbol, "1", percentageChangeInBenchMark, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, true, _riskParams);

                            var pranaRequestCarrierGroupTask = _riskServiceProxy.InnerChannel.CalculateStressTestData(pranaRequestCarrierGroup, pranaRequestCarrierForBetaGroup).ContinueWith(CompleteWorkAfterRiskResponse);
                            tasks.Add(pranaRequestCarrierGroupTask);
                        }
                        await System.Threading.Tasks.Task.WhenAll(tasks.ToArray());
                        _isRiskCalculatedGroup = true;
                    }

                    if (_isRiskCalculatedGroup && _isRiskCalculatedIndividual)
                    {
                        UpdateToolStripStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
        }

        private void CompleteWorkAfterRiskResponse(System.Threading.Tasks.Task<PranaRequestCarrier> completedTask)
        {
            try
            {
                UpdateProgressBarAndRequestCount(completedTask);
                UpdateGroupRisk(completedTask.Result);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
        }

        private void UpdateProgressBarAndRequestCount(System.Threading.Tasks.Task<PranaRequestCarrier> completedTask)
        {
            try
            {
                lock (_locker)
                {
                    _responseReceivedCount++;
                    _requestedCallsCount--;
                }
                if (!completedTask.IsFaulted)
                {
                    int percentageCompletion = (_requestedCallsCount + _responseReceivedCount) <= 0
                   ? 0 : (_responseReceivedCount * 100 / (_requestedCallsCount + _responseReceivedCount));
                    toolStripStatusLabel.Text = DateTime.Now + ": " + percentageCompletion + "% Completed";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in completedTask.Exception.InnerExceptions)
                    {
                        sb.Append(item.Message + " ");
                    }
                    _isRiskCalculatedIndividual = false;
                    _isRiskCalculatedGroup = false;
                    UpdateToolStripStatus(DateTime.Now + ": " + sb.ToString());
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

        private void CompleteWorkForNonGroupAfterRiskResponse(System.Threading.Tasks.Task<PranaRequestCarrier> pranaRequestCarrierTask)
        {

            try
            {
                UpdateProgressBarAndRequestCount(pranaRequestCarrierTask);
                PranaRequestCarrier finalIndividualResult = pranaRequestCarrierTask.Result;
                UpdateIndividualRisk(finalIndividualResult);
                UpdatePortfolioRisk(finalIndividualResult);
                _isRiskCalculatedIndividual = true;
                if (_isRiskCalculatedGroup && _isRiskCalculatedIndividual)
                {
                    UpdateToolStripStatus();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
        }

        private void SetGridFontSize()
        {
            try
            {
                float fontSize = Convert.ToSingle(RiskPreferenceManager.RiskPrefernece.FontSize);
                Font oldFont = grdData.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                grdData.Font = newFont;
                grdData.DisplayLayout.Override.SummaryValueAppearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ColorSummaryText);
                portfolioResultsCtrl1.SetGridFonts(newFont);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void UpdateHeaderWrapHeader(bool isUpdatePreference)
        {
            try
            {
                bool wrapHeader = Convert.ToBoolean(RiskPreferenceManager.RiskPrefernece.WrapHeader);
                if (wrapHeader.Equals(true))
                    this.grdData.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;
                else
                    this.grdData.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.False;

                if (wrapHeader)
                {
                    foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                    {
                        Graphics g = CreateGraphics();
                        string maxLengthWord = col.Header.Caption.Split(' ').ToArray().OrderBy(w => w.Length).LastOrDefault();
                        SizeF stringSize = new SizeF();
                        Font font = new Font("Tahoma", 13);
                        stringSize = g.MeasureString(maxLengthWord, font);
                        col.Width = Convert.ToInt32(stringSize.Width) + 30;
                    }
                }
                else if (isUpdatePreference)
                {
                    if (RiskLayoutManager.RiskLayout.StressTestColumns.Count > 0)
                    {
                        List<ColumnData> listColData = RiskLayoutManager.RiskLayout.StressTestColumns;
                        RiskLayoutManager.LoadColumnsWidthFromXML(grdData, listColData);
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

        private void grdData_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                List<EnumerationValue> PutOrCallType = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(OptionType));
                ValueList PutOrCallValueList = new ValueList();
                foreach (EnumerationValue value in PutOrCallType)
                {
                    PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText.Substring(0, 1));
                }
                PutOrCallValueList.ValueListItems.Add(int.MinValue, " ");
                e.Row.Cells[COL_PutOrCall].ValueList = PutOrCallValueList;
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

        private void grdData_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                if (!e.Row.HasParent())
                {
                    //Top Level Group Row
                    e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel1);
                    e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel1);
                }
                else
                {
                    //Intermediate Group Row
                    if (!e.Row.ParentRow.HasParent())
                    {
                        e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel2);
                        e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel2);
                    }
                    else //Bottom Level Group Row
                    {
                        e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel3);
                        e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel3);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        void grdData_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(COL_AUECLocalDate) || e.Column.Key.Equals(COL_ExpirationDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdData.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdData.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        void grdData_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(COL_AUECLocalDate) || e.Column.Key.Equals(COL_ExpirationDate))
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


        private double CalculateCurrentMarketValue(PranaRiskObj riskobj)
        {
            double mktValueBase = 0;
            try
            {
                AssetCategory Asset = (AssetCategory)riskobj.AssetID;
                if (!(Asset.Equals(AssetCategory.Future)))
                {
                    double mktValue = 0.0;
                    //if (Asset.Equals(AssetCategory.FixedIncome) || Asset.Equals(AssetCategory.ConvertibleBond))
                    //{
                    //    mktValue = Formulae.Formulae.GetMarketValue(Math.Abs(riskobj.Quantity), riskobj.LastPrice, riskobj.ContractMultiplier / 100, riskobj.SideMultiplier);
                    //}
                    //else
                    {
                        mktValue = BusinessLogic.Calculations.GetMarketValue(Math.Abs(riskobj.Quantity), riskobj.LastPrice, riskobj.ContractMultiplier, riskobj.SideMultiplier);
                    }

                    mktValueBase = mktValue;
                    //CHMW-3132	Account wise fx rate handling for expiration settlement                    
                    int accountBaseCurrencyID;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(riskobj.Level1ID))
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[riskobj.Level1ID];
                    }
                    else
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    if (riskobj.CurrencyID != accountBaseCurrencyID)
                    {
                        mktValueBase = mktValue * riskobj.FXRate;
                    }
                }
                else
                {
                    double DayPnl = 0.0;
                    DateTime AuecTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(riskobj.AUECID));
                    DateTime yesterdayAuecDate = AuecTime.AddDays(-1);
                    if (riskobj.AUECLocalDate.Date.Equals(AuecTime.Date))
                    {
                        _yesterdayMarkPrice = riskobj.AvgPrice;
                    }
                    else
                    {
                        bool isHoliday = BusinessDayCalculator.CheckForHoliday(yesterdayAuecDate); ;
                        while (isHoliday)
                        {
                            yesterdayAuecDate = yesterdayAuecDate.AddDays(-1);
                            isHoliday = BusinessDayCalculator.CheckForHoliday(yesterdayAuecDate);
                        }
                        _yesterdayMarkPrice = _pricingServiceProxy.InnerChannel.GetMarkPriceForDateAndSymbol(yesterdayAuecDate.Date, riskobj.Symbol);
                    }
                    if (_yesterdayMarkPrice == 0 || riskobj.LastPrice == 0)
                    {
                        return 0;
                    }
                    //CHMW-3132	Account wise fx rate handling for expiration settlement                    
                    int accountBaseCurrencyID;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(riskobj.Level1ID))
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[riskobj.Level1ID];
                    }
                    else
                    {
                        accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    if (riskobj.CurrencyID != accountBaseCurrencyID)
                    {
                        ConversionRate yesterDayConversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(riskobj.CurrencyID, yesterdayAuecDate, riskobj.Level1ID);
                        if (yesterDayConversionRate != null)
                        {
                            double yesterdayFXRateToBase = yesterDayConversionRate.ConversionMethod == Operator.M ? yesterDayConversionRate.RateValue : 1 / yesterDayConversionRate.RateValue;
                            double currentFXRateToBase = riskobj.FXRate;
                            DayPnl = BusinessLogic.Calculations.GetPnLInBaseCurrency(Math.Abs(riskobj.Quantity), riskobj.LastPrice, _yesterdayMarkPrice, riskobj.ContractMultiplier, riskobj.SideMultiplier, currentFXRateToBase, yesterdayFXRateToBase);
                        }
                    }
                    else
                    {
                        DayPnl = BusinessLogic.Calculations.GetPnL(Math.Abs(riskobj.Quantity), riskobj.LastPrice, _yesterdayMarkPrice, riskobj.ContractMultiplier, riskobj.SideMultiplier);
                    }
                    mktValueBase = DayPnl;
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
            return mktValueBase;
        }

        private void CalculatePortfolioCurrentMktValue()
        {
            try
            {
                PranaRiskObjColl selectedRows = GetSelectedRows();
                foreach (PranaRiskObj riskobj in selectedRows)
                {
                    _currentMktValuePortfolio += CalculateCurrentMarketValue(riskobj);
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

        private void timerSnapShot_Tick(object sender, EventArgs e)
        {
            try
            {
                timerSnapShot.Stop();
                //send portfolio science request once market value is Calculated
                Calculate();
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

        private void UpdateToolStripStatus()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker marsheller = new MethodInvoker(UpdateToolStripStatus);
                        this.BeginInvoke(marsheller);
                    }
                    else
                    {
                        toolStripStatusLabel.ForeColor = Color.Green;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Success";
                        EnableForm();
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

        private void UpdateToolStripStatus(string message)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UIThreadMarshellerUpdateToolStrip marsheller = new UIThreadMarshellerUpdateToolStrip(UpdateToolStripStatus);
                        this.BeginInvoke(marsheller, new object[] { message });
                    }
                    else
                    {
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": " + message;
                        EnableForm();
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

        private void UnwireEvents()
        {
            try
            {
                if (_bgCalculate != null)
                {
                    if (_bgCalculate.IsBusy)
                    {
                        if (_bgCalculate.WorkerSupportsCancellation)
                        {
                            _bgCalculate.CancelAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void saveColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RiskPrefernece riskPrefs = RiskPreferenceManager.RiskPrefernece;
                riskPrefs.StressTestColumns = UltraWinGridUtils.GetColumnsString(grdData);
                riskPrefs.StressTestTabBenchMarkSymbolName = cmbbxBenchMark.Value.ToString();
                riskPrefs.BenchMarkPercentMove = numericUpDownIndex.Value;
                riskPrefs.IncludeCash = chkIncludeCash.Checked;
                RiskPreferenceManager.SavePreferences(riskPrefs);

                RiskLayoutManager.RiskLayout.StressTestColumns = RiskLayoutManager.GetGridColumnLayout(grdData);
                RiskLayoutManager.RiskLayout.StressTestGroupByColumnsCollection = RiskLayoutManager.GetGridGroupByColumnLayout(grdData);
                RiskLayoutManager.SaveRiskLayout();
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Layout Saved";
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

        void GetInstance_PositionReceived(PranaRiskObjColl riskObjColl)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MsgreceivedInvokeDelegate msgreceivedInvokeDelegate = new MsgreceivedInvokeDelegate(GetInstance_PositionReceived);
                        this.BeginInvoke(msgreceivedInvokeDelegate, new object[] { riskObjColl });
                    }
                    else
                    {
                        if (riskObjColl != null)
                        {
                            foreach (PranaRiskObj riskObj in riskObjColl)
                            {
                                double value;
                                double.TryParse(riskObj.PnlImpact, out value);
                                if (value == 0)
                                {
                                    riskObj.PnlImpact = "N/A";
                                }
                                _riskObjcollection.Add(riskObj);
                                AddPositionToDict(riskObj);
                            }
                        }
                        grdData.DataSource = _riskObjcollection;
                        grdData.DataBind();

                        _dataPortfolio.Rows.Clear();
                        portfolioResultsCtrl1.Refresh();
                        _dtCalculatedRisks.Rows.Clear();

                        toolStripStatusLabel.ForeColor = Color.Green;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Ready";
                        if (RefreshCompleted != null)
                        {
                            RefreshCompleted(null, null);
                        }
                        EnableForm();
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

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.FindForm().AddCustomColumnChooser(this.grdData);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void mnuClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ColumnFilter colFilters in grdData.DisplayLayout.Bands[0].ColumnFilters)
                {
                    colFilters.ClearFilterConditions();
                }
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Filters Cleared";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkIncludeCash_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (IncludeCashCheckChanged != null && isAlreadyStarted)
                {
                    IncludeCashCheckChanged(this, EventArgs.Empty);
                }
                SetGroupByColumns(chkIncludeCash.Checked);

                UltraGridBand band = grdData.DisplayLayout.Bands[0];
                if (chkIncludeCash.Checked)
                {
                    grdData.DisplayLayout.Bands[0].SortedColumns.Clear();
                    grdData.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;

                    band.Columns[COL_IsChecked].CellActivation = Activation.NoEdit;
                    band.Columns[COL_IsChecked].Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    grdData.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
                    grdData.DisplayLayout.Bands[0].Columns[COL_Level1Name].AllowRowFiltering = DefaultableBoolean.True;
                    grdData.DisplayLayout.Bands[0].Columns[COL_MasterFund].AllowRowFiltering = DefaultableBoolean.True;
                    headerCheckBox.SelectUnSelectAll(grdData, true, COL_IsChecked);
                    grdData.CreationFilter = null;
                }
                else
                {
                    band.Columns[COL_IsChecked].CellActivation = Activation.AllowEdit;
                    grdData.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
                    grdData.CreationFilter = headerCheckBox;
                }

                if (isAlreadyStarted)
                {
                    foreach (ColumnFilter colFilters in band.ColumnFilters)
                    {
                        colFilters.ClearFilterConditions();
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

        private void grdData_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                int counter = 0;
                _firstLevelGroup = string.Empty;
                _secondLevelGroup = string.Empty;

                foreach (UltraGridColumn var in e.SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        if (counter == 0)
                        {
                            _firstLevelGroup = var.Key;
                        }
                        else
                        {
                            _secondLevelGroup = var.Key;
                        }
                        counter++;
                    }
                }
                //Bharat Kumar Jangir (1 September, 2014)
                //Risk Max Grouping level = 2, Otherwise Risk calculations might be very slow due to more PS API calls
                if (counter > 2)
                {
                    MessageBox.Show("Positions can not be grouped by more than 2 columns.", "Option Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Public
        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                UIThreadMarshellerSnapShotResponse mi = new UIThreadMarshellerSnapShotResponse(SnapshotResponse);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { data });
                    }
                    else
                    {
                        if (_listRequestedSymbols.Contains(data.Symbol))
                        {
                            _listRequestedSymbols.Remove(data.Symbol);
                        }
                        if (timerSnapShot.Enabled)
                        {
                            timerSnapShot.Stop();
                            timerSnapShot.Start();

                            if (_dictRiskObjs.ContainsKey(data.Symbol) || _dictOptUnderlyingWise.ContainsKey(data.Symbol))
                            {
                                List<PranaRiskObj> listRiskObj = new List<PranaRiskObj>();
                                if (_dictRiskObjs.ContainsKey(data.Symbol))
                                {
                                    listRiskObj = _dictRiskObjs[data.Symbol];
                                }

                                switch (data.CategoryCode)
                                {
                                    case AssetCategory.EquityOption:
                                    case AssetCategory.FutureOption:
                                        OptionSymbolData optData = data as OptionSymbolData;
                                        foreach (PranaRiskObj riskObj in listRiskObj)
                                        {
                                            if (riskObj.IsChecked)
                                            {
                                                riskObj.Delta = (float)optData.Delta;
                                                riskObj.LastPrice = optData.SelectedFeedPrice;
                                                if (riskObj.FXRate == 0.0)
                                                {
                                                    riskObj.FXRate = 1;
                                                }
                                            }
                                        }
                                        break;

                                    case AssetCategory.FX:
                                    case AssetCategory.FXForward:
                                        foreach (PranaRiskObj riskObj in listRiskObj)
                                        {
                                            if (riskObj.IsChecked)
                                            {
                                                riskObj.LastPrice = data.SelectedFeedPrice;
                                                riskObj.FXRate = data.SelectedFeedPrice;
                                                riskObj.UnderlyingLastPrice = data.SelectedFeedPrice;
                                            }
                                        }
                                        break;

                                    default:
                                        if (_dictOptUnderlyingWise.ContainsKey(data.Symbol))
                                        {
                                            List<PranaRiskObj> listOptions = _dictOptUnderlyingWise[data.Symbol];
                                            foreach (PranaRiskObj riskObj in listOptions)
                                            {
                                                riskObj.UnderlyingLastPrice = data.SelectedFeedPrice;
                                            }

                                            _dictOptUnderlyingWise.Remove(data.Symbol);
                                        }
                                        foreach (PranaRiskObj riskObj in listRiskObj)
                                        {
                                            if (riskObj.IsChecked)
                                            {
                                                riskObj.LastPrice = data.SelectedFeedPrice;
                                                riskObj.UnderlyingLastPrice = data.SelectedFeedPrice;
                                                if (riskObj.FXRate == 0.0)
                                                {
                                                    riskObj.FXRate = 1;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (data.CategoryCode.Equals(AssetCategory.Forex))
                                {
                                    if (_dictInternationalSymbols.ContainsKey(data.Symbol))
                                    {
                                        List<string> Symbols = _dictInternationalSymbols[data.Symbol];
                                        foreach (string symbol in Symbols)
                                        {
                                            if (_dictRiskObjs.ContainsKey(symbol))
                                            {
                                                List<PranaRiskObj> listRiskObjects = _dictRiskObjs[symbol];

                                                foreach (PranaRiskObj riskObj in listRiskObjects)
                                                {
                                                    if (riskObj.IsChecked)
                                                    {
                                                        riskObj.FXRate = data.SelectedFeedPrice;
                                                    }
                                                }
                                            }
                                        }
                                        _dictInternationalSymbols.Remove(data.Symbol);
                                    }
                                }
                            }
                        }
                        if (_listRequestedSymbols.Count == 0)
                        {
                            timerSnapShot_Tick(null, null);
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

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        public CtrlStressTest()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DisposeProxy()
        {
            try
            {
                if (_riskServiceProxy != null)
                {
                    _riskServiceProxy.Dispose();
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetUp(bool isOnlyLoadPositions, ref bool isTabBusy)
        {
            try
            {
                if (isAlreadyStarted && isOnlyLoadPositions)
                {
                    GetData();
                    isTabBusy = true;
                }
                if (!isAlreadyStarted)
                {
                    SetCalculatedRiskTable();
                    SetGridFontSize();
                    if (RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        GetData();
                        isTabBusy = true;
                    }
                    foreach (PranaRiskObj riskObj in _riskObjcollection)
                    {
                        double value;
                        double.TryParse(riskObj.PnlImpact, out value);
                        if (value == 0)
                        {
                            riskObj.PnlImpact = "N/A";
                        }
                    }
                    grdData.DataSource = _riskObjcollection;
                    grdData.DataBind();
                    BindBenchMarkCombo();
                    SetColumns();
                    SetDataTable();
                    datePickerStartdate.DateTime = DateTime.Now.AddDays(-RiskPreferenceManager.RiskPrefernece._stressTestDateRange);
                    datePickerEnddate.DateTime = DateTime.Now;
                    if (_riskServiceProxy != null)
                    {
                        _riskServiceProxy.Dispose();
                    }
                    CreateRiskServiceProxy();
                    CreatePricingServiceProxy();
                    SetCompanyID();

                    LoadPreferences();
                    UpdateHeaderWrapHeader(false);
                    isAlreadyStarted = true;
                    // Sets status on Risk UI status bar
                    if (bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("PortfolioScienceLogoOnRisk")))
                    {
                        lblStatusPortfolioScience.Text = "Intra day VaR/Stress Tests calculated by Portfolio Science";
                    }
                    DisableForm();
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnCalculate.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnCalculate.ForeColor = System.Drawing.Color.White;
                btnCalculate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCalculate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCalculate.UseAppStyling = false;
                btnCalculate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public void UpdateGridAsPref()
        {
            try
            {
                SetGridFontSize();
                UpdateHeaderWrapHeader(true);
                grdData.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void ExportToExcel(string filename)
        {
            try
            {
                List<UltraGrid> lstGrids = new List<UltraGrid>();
                lstGrids.Add(grdData);
                lstGrids.Add(portfolioResultsCtrl1.grdData);
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.SetExcelLayoutAndWriteForRisk(lstGrids, true, filename);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public string GetBenchmarkValue()
        {
            try
            {
                return numericUpDownIndex.Value.ToString();
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
            return String.Empty;
        }

        public void RefreshPositions()
        {
            try
            {
                DisableForm();
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Refreshing Positions...";
                GetData();
                if (_bgCalculate != null)
                {
                    if (_bgCalculate.IsBusy)
                    {
                        _bgCalculate.CancelAsync();
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
        #endregion

        private void CtrlStressTest_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.toolStripStatusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel.Font = new Font("Century Gothic", 9F);
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

        private void grdData_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (!isAlreadyStarted || _isReloadingLayout)
                {
                    return;
                }

                int sortCount = grdData.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    //Correction made as it was not returning the column that has been sorted.
                    //Now the sorted column will be detected by mouse click event on column header.
                    UltraGridColumn sortColumn;
                    if (grdData.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grdData.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        //Set the Group by row summary display settings 
                        RowSummarySettings();
                        return;
                    }
                    if (sortColumn.Formula != null && !(sortColumn.DataType.Equals(typeof(System.Double))))
                    {
                        //Set the Group by row summary display settings 
                        RowSummarySettings();
                        return;
                    }

                    if (!sortColumn.IsGroupByColumn && !GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                        sortColumn.GroupByComparer = _groupSortComparer;
                    }
                    else if (sortColumn.IsGroupByColumn && GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                    this.grdData.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                }
                RowSummarySettings();
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

        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid grid = sender as UltraGrid;
                UIElement controlElement = grid.DisplayLayout.UIElement;
                UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
                while (elementAtPoint != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridUIElement uiElement = elementAtPoint.ControlElement as Infragistics.Win.UltraWinGrid.UltraGridUIElement;
                    HeaderUIElement headerElement = uiElement.ElementWithMouseCapture as HeaderUIElement;
                    if (headerElement != null &&
                         headerElement.Header is Infragistics.Win.UltraWinGrid.ColumnHeader)
                    {
                        _columnSorted = headerElement.GetContext(typeof(UltraGridColumn)) as UltraGridColumn;
                        break;
                    }
                    elementAtPoint = elementAtPoint.Parent;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = this.grdData.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;

                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                        string columnSortedName = string.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.TextUIElement)(thisElem)).Text;
                        }
                        else if (thisElem is Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)(thisElem)).ToString();
                        }
                        else if (thisElem is GroupByButtonUIElement)
                        {
                            foreach (object thisElemChild in thisElem.ChildElements)
                            {
                                if (thisElemChild is Infragistics.Win.TextUIElement)
                                {
                                    columnSortedName = ((Infragistics.Win.TextUIElement)(thisElemChild)).Text;
                                    break;
                                }
                            }
                        }
                        for (int counter = 0; counter < grdData.DisplayLayout.Bands[0].SortedColumns.Count; counter++)
                        {
                            if (columnSortedName.Equals(grdData.DisplayLayout.Bands[0].SortedColumns[counter].Header.Caption))
                            {
                                _columnSorted = grdData.DisplayLayout.Bands[0].SortedColumns[grdData.DisplayLayout.Bands[0].SortedColumns[counter].Key];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RowSummarySettings()
        {
            try
            {
                bool groupedBySomeColumn = false;
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        groupedBySomeColumn = true;
                        break;
                    }
                }
                if (!groupedBySomeColumn)
                {
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
                else
                {
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        public void SetGridExportSettings(Dictionary<string, UltraGrid> gridDict, Dictionary<string, string> riskReportDetails)
        {
            try
            {
                UltraGrid grid = new UltraGrid();
                Form UI = new Form();
                UI.Controls.Add(grid);
                grid.DataSource = _riskObjcollection;
                grid.DisplayLayout.Bands[0].Override.AllowGroupBy = DefaultableBoolean.True;
                grid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Bands[0].ColumnFilters.CopyFrom(grdData.DisplayLayout.Bands[0].ColumnFilters);

                //gets the layout set as preferences for 
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption != string.Empty)
                    {
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Header.Caption = col.Header.Caption;
                    }
                    grid.DisplayLayout.Bands[0].Columns[col.Key].Hidden = true;
                }

                UltraGridBand band = this.grdData.DisplayLayout.Bands[0];
                SortedColumnsCollection sortedcolColl = band.SortedColumns;
                grid.DisplayLayout.Bands[0].SortedColumns.Clear();
                if (sortedcolColl != null && sortedcolColl.Count > 0)
                {
                    foreach (UltraGridColumn col in sortedcolColl)
                    {
                        bool isDescending = col.SortIndicator == SortIndicator.Descending;
                        if (col.IsGroupByColumn)
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, isDescending, true);
                        }
                        else
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, isDescending, false);
                        }
                    }
                }
                //Provides the detail of stress test being run on the current view.
                int rowCount = portfolioResultsCtrl1.grdData.DisplayLayout.Rows.Count;
                StringBuilder sbRiskLowerGridValues = new StringBuilder();
                UltraGridLayout gridDisplayLayout = portfolioResultsCtrl1.grdData.DisplayLayout;
                foreach (UltraGridColumn col in gridDisplayLayout.Bands[0].Columns)
                {
                    if (rowCount == 0)
                    {
                        sbRiskLowerGridValues.Append(col.Key + "=" + "0" + Seperators.SEPERATOR_6);
                    }
                    else
                    {
                        sbRiskLowerGridValues.Append(col.Key + "=" + gridDisplayLayout.Rows[0].Cells[col.Key].Value + Seperators.SEPERATOR_6);
                    }
                }
                sbRiskLowerGridValues.Remove(sbRiskLowerGridValues.Length - 1, 1);
                int count = 0;
                foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    grid.ExternalSummaryValueRequested += grdData_ExternalSummaryValueRequested;
                    SetColumnSummaries(grid);
                }
                grid.DisplayLayout.Bands[0].ColHeadersVisible = false;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                grid.DisplayLayout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                grid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows
                | SummaryDisplayAreas.RootRowsFootersOnly
                | SummaryDisplayAreas.Bottom;
                grid.DisplayLayout.Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
                grid.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;

                riskReportDetails.Add(tabStressTest, sbRiskLowerGridValues.ToString());
                SortedDictionary<int, string> positionMapping = new SortedDictionary<int, string>();
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                {
                    if (!col.Hidden)
                    {
                        positionMapping[col.Header.VisiblePosition] = col.Key;
                    }
                }
                foreach (KeyValuePair<int, string> pair in positionMapping)
                {
                    if (pair.Value == "IsChecked")
                    {
                        grid.DisplayLayout.Bands[0].Columns[pair.Value].Hidden = true;
                    }
                    else
                    {
                        UltraGridColumn columnExportGrid = grid.DisplayLayout.Bands[0].Columns[pair.Value];
                        grid.DisplayLayout.Bands[0].Columns[pair.Value].Hidden = false;
                        columnExportGrid.Header.VisiblePosition = pair.Key;
                    }
                }
                gridDict.Add(tabStressTest, grid);
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
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                UnwireEvents();
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (_dtCalculatedRisks != null)
                    {
                        _dtCalculatedRisks.Dispose();
                    }
                    if (_dataPortfolio != null)
                    {
                        _dataPortfolio.Dispose();
                    }
                    if (_bgCalculate != null)
                    {
                        _bgCalculate.Dispose();
                    }
                    if (_columnSorted != null)
                    {
                        _columnSorted.Dispose();
                    }
                    if (_pricingServiceProxy != null)
                    {
                        _pricingServiceProxy.Dispose();
                    }
                    if (_riskServiceProxy != null)
                    {
                        _riskServiceProxy.Dispose();
                    }
                    if (headerCheckBox != null)
                    {
                        headerCheckBox.Dispose();
                    }
                    if (grdData.DisplayLayout.Bands[0] != null && grdData.DisplayLayout.Bands[0].Summaries.Count > 0)
                    {
                        grdData.DisplayLayout.Bands[0].Summaries.Clear();
                    }
                    if (grdData != null)
                    {
                        grdData.Dispose();
                    }
                    if (portfolioResultsCtrl1 != null)
                    {
                        portfolioResultsCtrl1 = null;
                    }
                }
                base.Dispose(disposing);
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
