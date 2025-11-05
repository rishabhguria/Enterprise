using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.LiveFeed.UI.Controls
{
    public partial class OneSymbolL1Strip : UserControl
    {
        private static bool _isComplianceModuleEnabled = false;
        private readonly Color UPCOLOR;
        private readonly Color DOWNCOLOR;
        private readonly Color NOCHANGECOLOR;

        public delegate void SnapshotResponseDelegate(SymbolData data);
        private SnapshotResponseDelegate _snapshotResponseDelegate;

        public delegate void L1DataResponseDelegate(SymbolData data);
        private L1DataResponseDelegate _l1DataResponseDelegate;
        public L1DataResponseDelegate L1DataResponse;

        public delegate double GetTotalQtyDelegate();
        public GetTotalQtyDelegate GetTotalQty;

        public delegate void ToggleRoundLoteSwitchDelegate(bool isUseRoundLot);
        public ToggleRoundLoteSwitchDelegate ToggleRoundLotSwitch;

        protected double _prevBid = 0;
        protected double _prevAsk = 0;

        private MarketDataHelper _marketDataHelperInstance = MarketDataHelper.GetInstance();
        protected bool _isListening = false;
        private List<string> _requestedSymbolsComplianceSnapshot = new List<string>();

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                if (_symbol != null && _symbol != string.Empty)
                {
                    StopMarketData();
                }

                _symbol = value;

                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    if (_symbol != null && _symbol != string.Empty && _marketDataHelperInstance != null)
                    {
                        if (!_isListening)
                        {
                            _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LevelOne_OnResponse);
                            _isListening = true;
                        }
                        if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            _marketDataHelperInstance.RequestSingleSymbol(_symbol, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// RequestSnapshotForCompliance
        /// </summary>
        /// <param name="symbol"></param>
        public void RequestSnapshotForCompliance(List<string> requestedSymbols)
        {
            if (_isComplianceModuleEnabled && requestedSymbols != null && requestedSymbols.Count > 0 && _marketDataHelperInstance != null)
            {
                _requestedSymbolsComplianceSnapshot = requestedSymbols;
                _marketDataHelperInstance.RequestSnapshotForCompliance(_requestedSymbolsComplianceSnapshot);
            }
        }

        public OneSymbolL1Strip()
        {
            InitializeComponent();
            _isComplianceModuleEnabled = ComplianceCacheManager.GetPreTradeCheck(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
            if (CustomThemeHelper.ApplyTheme && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                UPCOLOR = Color.FromArgb(39, 174, 96);
                DOWNCOLOR = Color.FromArgb(192, 57, 43);
                NOCHANGECOLOR = Color.Black;

                this.lblChange.ForeColor = NOCHANGECOLOR;
                this.lblBid.ForeColor = NOCHANGECOLOR;
                this.lblAsk.ForeColor = NOCHANGECOLOR;
                this.lblHigh.ForeColor = NOCHANGECOLOR;
                this.lblLow.ForeColor = NOCHANGECOLOR;
                this.lblVolumn.ForeColor = NOCHANGECOLOR;
                this.lblPosition.ForeColor = NOCHANGECOLOR;
                this.lblExposure.ForeColor = NOCHANGECOLOR;
                this.lblNotation.ForeColor = NOCHANGECOLOR;
                this.lblVWAP.ForeColor = NOCHANGECOLOR;
            }
            else
            {
                UPCOLOR = Color.GreenYellow;
                DOWNCOLOR = Color.Red;
                NOCHANGECOLOR = Color.White;
            }
        }

        private void OneSymbolL1Strip_Load(object sender, EventArgs e)
        {
            _snapshotResponseDelegate = new SnapshotResponseDelegate(onSnapshotResponse);
            _l1DataResponseDelegate = new L1DataResponseDelegate(onL1Response);
            ClearUI();
        }

        public void onSnapshotResponse(SymbolData data)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(_snapshotResponseDelegate, new object[] { data });
                    return;
                }

                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled && _symbol == data.Symbol && data.VWAP != 0)
                {
                    lblVWAP.Text = Math.Round(data.VWAP, 4).ToString();
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

        private void onL1Response(SymbolData l1Data)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(_l1DataResponseDelegate, new object[] { l1Data });
                    return;
                }
                if (!UIValidation.GetInstance().validate(this))
                {
                    return;
                }

                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    Color tikColor = (l1Data.LastTick == "+") ? UPCOLOR : ((l1Data.LastTick == "-") ? DOWNCOLOR : NOCHANGECOLOR);

                    lblLast.Text = l1Data.LastPrice == double.MinValue ? string.Empty : Math.Round(l1Data.LastPrice, 4).ToString();

                    lblChange.Text = l1Data.Change == double.MinValue ? string.Empty : Math.Round(l1Data.Change, 4).ToString();
                    lblChange.ForeColor = l1Data.Change > 0 ? UPCOLOR : (l1Data.Change < 0 ? DOWNCOLOR : NOCHANGECOLOR);

                    lblBid.Text = l1Data.Bid == double.MinValue ? string.Empty : Math.Round(l1Data.Bid, 4).ToString();
                    lblBid.ForeColor = (_prevBid > 0 && l1Data.Bid > _prevBid) ? UPCOLOR : (l1Data.Bid < _prevBid ? DOWNCOLOR : NOCHANGECOLOR);
                    _prevBid = l1Data.Bid;

                    lblAsk.Text = l1Data.Ask == double.MinValue ? string.Empty : Math.Round(l1Data.Ask, 4).ToString();
                    lblAsk.ForeColor = (_prevAsk > 0 && l1Data.Ask > _prevAsk) ? UPCOLOR : (l1Data.Ask < _prevAsk ? DOWNCOLOR : NOCHANGECOLOR);
                    _prevAsk = l1Data.Ask;

                    lblHigh.Text = l1Data.High == double.MinValue ? string.Empty : Math.Round(l1Data.High, 4).ToString();
                    lblLow.Text = l1Data.Low == double.MinValue ? string.Empty : Math.Round(l1Data.Low, 4).ToString();
                    lblVolumn.Text = l1Data.TotalVolume == long.MinValue ? string.Empty : l1Data.TotalVolume.ToString("#,#");
                    lblNotation.Text = GetTotalQty != null ? Math.Round(GetTotalQty() * l1Data.LastPrice, 2).ToString("#,##0.00") : string.Empty;
                    lblVWAP.Text = l1Data.VWAP == double.MinValue ? string.Empty : Math.Round(l1Data.VWAP, 4).ToString();
                }
                else
                {
                    lblLast.Text = string.Empty;
                    lblChange.Text = string.Empty;
                    lblChange.ForeColor = NOCHANGECOLOR;

                    lblBid.Text = string.Empty;
                    lblBid.ForeColor = NOCHANGECOLOR;

                    lblAsk.Text = string.Empty;
                    lblAsk.ForeColor = NOCHANGECOLOR;

                    lblHigh.Text = string.Empty;
                    lblLow.Text = string.Empty;
                    lblVolumn.Text = string.Empty;
                    lblNotation.Text = string.Empty;
                    lblVWAP.Text = string.Empty;
                }

                if (L1DataResponse != null)
                {
                    L1DataResponse(l1Data);
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
            }
        }

        private void LevelOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                SymbolData data = args.Value;
                string symbol = data.Symbol;
                if (symbol == _symbol)
                {
                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: OneSymbolL1Strip.LevelOne_OnResponse() > TT level1 strip response received for Symbol: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }
                    onL1Response(data);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: OneSymbolL1Strip.LevelOne_OnResponse() > TT level1 strip response binded on UI for Symbol: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
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
            }
        }

        public void UpdateNotionalValue(double notional)
        {
            try
            {
                if (notional != 0 && CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    lblNotation.Text = Math.Round(notional, 2).ToString("#,##0.00");
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

        protected delegate void UpdatePositionAndExposure(double position, double exposure);
        public void updatePositionAndExposure(double position, double exposure)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new UpdatePositionAndExposure(updatePositionAndExposure), new object[] { position, exposure });
                    return;
                }
                lblPosition.Text = Math.Round(position, 4).ToString("#,#.#");
                lblExposure.Text = Math.Round(exposure, 4).ToString("#,#.#");
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
            }
        }

        #region Private Methods
        private void ClearUI()
        {
            lblLast.Text = string.Empty;
            lblChange.Text = string.Empty;
            lblBid.Text = string.Empty;
            lblAsk.Text = string.Empty;
            lblHigh.Text = string.Empty;
            lblLow.Text = string.Empty;
            lblVolumn.Text = string.Empty;
            lblPosition.Text = string.Empty;
            lblExposure.Text = string.Empty;
            lblNotation.Text = string.Empty;
            lblVWAP.Text = string.Empty;

            _prevBid = 0;
            _prevAsk = 0;
        }

        private void CleanUp()
        {
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                StopMarketData();
            }
        }
        #endregion

        #region Public Methods
        public void StopMarketData()
        {
            try
            {
                if (_symbol != null && _symbol != string.Empty && _marketDataHelperInstance != null)
                {
                    _marketDataHelperInstance.RemoveSingleSymbol(_symbol);
                    if (_isComplianceModuleEnabled && _requestedSymbolsComplianceSnapshot != null && _requestedSymbolsComplianceSnapshot.Count > 0)
                        _marketDataHelperInstance.RemoveSnapshotForCompliance(_requestedSymbolsComplianceSnapshot);
                    _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LevelOne_OnResponse);
                    _isListening = false;
                }
                _symbol = null;
                ClearUI();
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
            }
        }
        #endregion

        public void setRoundLotButton(bool isUseRoundLot, decimal roundLot = 0)
        {
            string strRoundLot = roundLot.ToString();
            strRoundLot = strRoundLot.Contains(".") ? strRoundLot.TrimEnd('0').TrimEnd('.') : strRoundLot;
            this.toggleSwitchRoundLot.Checked = isUseRoundLot;
            this.lblRoundLot.Text = (roundLot > 0) ? strRoundLot : string.Empty;
            if(ToggleRoundLotSwitch != null)
            {
                ToggleRoundLotSwitch(isUseRoundLot);
            }
        }


        public void setL1InvisibleExceptRoundLot()
        {
            try
            {
                for (int i = ultraPanel.ClientArea.Controls.Count - 1; i >= 0; i--)
                {
                    Label label = ultraPanel.ClientArea.Controls[i] as Label;
                    if ((label != null) && (label.Name != "lblRoundLot" && label.Name != "label10"))
                    {
                        ultraPanel.ClientArea.Controls[i].Visible = false;
                    }
                }
                label10.Location = new Point(2, 8);
                lblRoundLot.Location = new Point(4, 21);
                toggleSwitchRoundLot.Location = new Point(60, 8);
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
            }
        }

        private void toggleSwitchRoundLot_Click(object sender, EventArgs e)
        {
            ToggleRoundLotSwitch(!toggleSwitchRoundLot.Checked);
        }
    }
}
