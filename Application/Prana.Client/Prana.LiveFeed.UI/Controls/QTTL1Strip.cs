using Infragistics.Win.UltraWinToolTip;
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
    public partial class QTTL1Strip : UserControl
    {
        private static bool _isComplianceModuleEnabled = false;
        private readonly Color UPCOLOR;
        private readonly Color DOWNCOLOR;
        private readonly Color NOCHANGECOLOR;

        public delegate void SnapshotResponseDelegate(SymbolData data);

        public delegate void L1DataResponseDelegate(SymbolData data);
        private L1DataResponseDelegate _l1DataResponseDelegate;
        public L1DataResponseDelegate L1DataResponse;
        public L1DataResponseDelegate FXDataResponse;

        protected double _prevBid = 0;
        protected double _prevAsk = 0;

        private MarketDataHelper _marketDataHelperInstance = MarketDataHelper.GetInstance();
        protected bool _isListening = false;
        private List<string> _requestedSymbolsComplianceSnapshot = new List<string>();
        private string _requestedFXSymbol = string.Empty;
        private string _position = string.Empty;
        private string _exposure = string.Empty;
        private string _dayPL = string.Empty;

        private string _symbol = string.Empty;
        public string Symbol
        {
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

        /// <summary>
        /// Request FX Snapshot For Notional Calculation
        /// </summary>
        /// <param name="symbol"></param>
        public void RequestFXForNotional(string fxSymbol)
        {
            if (_marketDataHelperInstance != null)
            {
                _requestedFXSymbol = fxSymbol;
                _marketDataHelperInstance.RequestSingleSymbol(fxSymbol, true);
            }
        }

        public QTTL1Strip()
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
                this.lblLast.ForeColor = NOCHANGECOLOR;
                this.lblPosition.ForeColor = NOCHANGECOLOR;
                this.lblExposure.ForeColor = NOCHANGECOLOR;
                this.lblDayPL.ForeColor = NOCHANGECOLOR;
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
            _l1DataResponseDelegate = new L1DataResponseDelegate(onL1Response);
            ClearUI();
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
                    Color tikColor = (l1Data.LastTick == "+" || l1Data.LastTick == "UP_TICK" || l1Data.LastTick == "UP_UNCHANGED") ? UPCOLOR :
                        ((l1Data.LastTick == "-" || l1Data.LastTick == "DOWN_TICK" || l1Data.LastTick == "DOWN_UNCHANGED") ? DOWNCOLOR : NOCHANGECOLOR);

                    lblLast.Text = l1Data.LastPrice == double.MinValue ? string.Empty : Math.Round(l1Data.LastPrice, 2).ToString();
                    lblLast.ForeColor = tikColor;

                    lblChange.Text = l1Data.Change == double.MinValue ? string.Empty : Math.Round(l1Data.Change, 2).ToString();
                    lblChange.ForeColor = l1Data.Change > 0 ? UPCOLOR : (l1Data.Change < 0 ? DOWNCOLOR : NOCHANGECOLOR);

                    lblBid.Text = l1Data.Bid == double.MinValue ? string.Empty : Math.Round(l1Data.Bid, 2).ToString();
                    lblBid.ForeColor = (_prevBid > 0 && l1Data.Bid > _prevBid) ? UPCOLOR : (l1Data.Bid < _prevBid ? DOWNCOLOR : NOCHANGECOLOR);
                    _prevBid = l1Data.Bid;

                    lblAsk.Text = l1Data.Ask == double.MinValue ? string.Empty : Math.Round(l1Data.Ask, 2).ToString();
                    lblAsk.ForeColor = (_prevAsk > 0 && l1Data.Ask > _prevAsk) ? UPCOLOR : (l1Data.Ask < _prevAsk ? DOWNCOLOR : NOCHANGECOLOR);
                    _prevAsk = l1Data.Ask;

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
                else if (symbol == _requestedFXSymbol && FXDataResponse != null)
                {
                    FXDataResponse(data);
                    _requestedFXSymbol = string.Empty;
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


        protected delegate void UpdatePositionExposureAndPNL(double position, double exposure, double dayPNL);
        public void updatePositionExposureAndPNL(double position, double exposure, double dayPNL)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new UpdatePositionExposureAndPNL(updatePositionExposureAndPNL), new object[] { position, exposure, dayPNL });
                    return;
                }
                if (Math.Abs(position) > 999999999d)
                {
                    lblPosition.Text = (Math.Round((position / 1000000000d), 2)).ToString("#,##0.00") + "B";
                }
                else
                {
                    lblPosition.Text = Math.Round(position, 2).ToString("#,##0.00");
                }
                lblPosition.ForeColor = position > 0 ? UPCOLOR : (position < 0 ? DOWNCOLOR : NOCHANGECOLOR);

                if (exposure > 999999999d)
                {
                    lblExposure.Text = (Math.Round((exposure / 1000000000d), 2)).ToString("#,##0.00") + "B";
                }
                else
                {
                    lblExposure.Text = Math.Round(exposure).ToString("#,#");
                }

                if (Math.Abs(dayPNL) > 999999999d)
                {
                    lblDayPL.Text = (Math.Round(dayPNL / 1000000000d, 2)).ToString("#,##0.00") + "B";
                }
                else if (Math.Abs(dayPNL) > 999999d)
                {
                    lblDayPL.Text = (Math.Round(dayPNL / 1000000d, 2)).ToString("#,##0.00") + "M";
                }
                else
                {
                    lblDayPL.Text = Math.Round(dayPNL).ToString("#,#");
                }
                _position = position.ToString("#,#.00");
                _exposure = exposure.ToString("#,#");
                _dayPL = dayPNL.ToString("#,#");
                lblExposure.ForeColor = exposure > 0 ? UPCOLOR : (exposure < 0 ? DOWNCOLOR : NOCHANGECOLOR);
                lblDayPL.ForeColor = dayPNL > 0 ? UPCOLOR : (dayPNL < 0 ? DOWNCOLOR : NOCHANGECOLOR);
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
            lblPosition.Text = string.Empty;
            lblExposure.Text = string.Empty;
            lblDayPL.Text = string.Empty;

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

        private void lblPosition_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblPosition.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(_position, Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
                    ultraToolTipManager1.SetUltraToolTip(lblPosition, toolTipInfo);
                    ultraToolTipManager1.ShowToolTip(lblPosition);
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

        private void lblExposure_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblExposure.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(_exposure, Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
                    ultraToolTipManager1.SetUltraToolTip(lblExposure, toolTipInfo);
                    ultraToolTipManager1.ShowToolTip(lblExposure);
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

        private void lblDayPL_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblDayPL.Text))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo(_dayPL, Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
                    ultraToolTipManager1.SetUltraToolTip(lblDayPL, toolTipInfo);
                    ultraToolTipManager1.ShowToolTip(lblDayPL);
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
    }
}
