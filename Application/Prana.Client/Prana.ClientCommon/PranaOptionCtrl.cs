#region Author Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR   		 : Bharat Jangir
// CREATION DATE	 : 28 April 2014
// PURPOSE	    	 : Manual Option Control
///////////////////////////////////////////////////////////////////////////////
#endregion

#region NameSpaces
using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion

namespace Prana.ClientCommon
{
    public partial class PranaOptionCtrl : UserControl
    {
        public event EventHandler<EventArgs<string>> OptionGenerated;
        public event EventHandler<EventArgs<string, string>> ValidateSymbol;
        private ISecurityMasterServices _securityMaster = null;
        private ApplicationConstants.SymbologyCodes _symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
        private SecMasterBaseObj _secMasterObj = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                if (_securityMaster == null)
                {
                    _securityMaster = value;
                    _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                }
            }
        }

        public PranaOptionCtrl()
        {
            InitializeComponent();
        }

        #region Public Methods
        public void SetUp(ApplicationConstants.SymbologyCodes defaultSymbology, int defaultOptionType)
        {
            try
            {
                this.Disposed += new EventHandler(pranaOptionCtrl_Disposed);
                BindOptionTypeCombo(defaultOptionType);
                _symbology = defaultSymbology;
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

        private int _defaultOptionType = 1;
        public void RefreshControl(ApplicationConstants.SymbologyCodes defaultSymbology, int defaultOptionType)
        {
            _defaultOptionType = defaultOptionType;
            try
            {
                txtUnderlyingSymbol.Text = string.Empty;
                txtStrikePrice.Value = 0.00;
                cmbOptionType.Value = defaultOptionType;
                dtExpirationDate.Value = DateTime.Now.Date;
                _symbology = defaultSymbology;
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
        #endregion

        #region Private Methods
        private void BindOptionTypeCombo(int defaultOptionType)
        {
            try
            {
                List<EnumerationValue> ds = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(OptionType));
                ds.RemoveAt(ds.Count - 1); //remove the NONE
                cmbOptionType.DataSource = ds;
                cmbOptionType.DataBind();
                cmbOptionType.DisplayMember = "DisplayText";
                cmbOptionType.ValueMember = "Value";
                cmbOptionType.Value = (defaultOptionType == (int)OptionType.PUT) ? OptionType.PUT : OptionType.CALL;
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

        private bool isValidated()
        {
            bool isValid = false;
            try
            {
                if (!string.IsNullOrEmpty(txtUnderlyingSymbol.Text.Trim()) && !string.IsNullOrEmpty(txtStrikePrice.Value.ToString().Trim()) && Convert.ToInt32(cmbOptionType.Value) != -1 && _secMasterObj != null)
                {
                    //Date Validity Check
                    if (dtExpirationDate.DateTime.Year >= DateTime.MinValue.Year && dtExpirationDate.DateTime.Year <= DateTime.MaxValue.Year && dtExpirationDate.DateTime.Month >= DateTime.MinValue.Month && dtExpirationDate.DateTime.Month <= DateTime.MaxValue.Month && dtExpirationDate.DateTime.Day >= DateTime.MinValue.Day && dtExpirationDate.DateTime.Day <= DateTime.MaxValue.Day)
                    {
                        isValid = true;
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
            return isValid;
        }

        private void GenerateSymbol()
        {
            try
            {
                if (OptionGenerated != null)
                {
                    OptionDetail optionDetail = new OptionDetail();
                    optionDetail.ExpirationDate = dtExpirationDate.DateTime;
                    optionDetail.StrikePrice = txtStrikePrice.Value;
                    optionDetail.UnderlyingSymbol = txtUnderlyingSymbol.Text;
                    optionDetail.Symbology = _symbology;
                    optionDetail.AssetCategory = _secMasterObj.AssetCategory;
                    optionDetail.AUECID = _secMasterObj.AUECID;
                    optionDetail.OptionType = (OptionType)cmbOptionType.Value;
                    optionDetail.StrikePriceMultiplier = _secMasterObj.StrikePriceMultiplier;
                    optionDetail.EsignalOptionRoot = _secMasterObj.EsignalOptionRoot;
                    optionDetail.BloombergOptionRoot = _secMasterObj.BloombergOptionRoot;
                    OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                    if (!string.IsNullOrEmpty(optionDetail.Symbol))
                        OptionGenerated(this, new EventArgs<string>(optionDetail.Symbol));
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
        #endregion

        #region Events
        private void txtUnderlyingSymbol_SymbolEntered(object sender, EventArgs<string, string> e)
        {
            try
            {
                _secMasterObj = null;
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();

                    reqObj.AddData(e.Value.Trim(), (ApplicationConstants.SymbologyCodes)_symbology);
                    reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    reqObj.HashCode = this.GetHashCode();
                    _securityMaster.SendRequest(reqObj);
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

        private void txtStrikePrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isValidated())
                    GenerateSymbol();
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

        private void cmbOptionType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isValidated())
                    GenerateSymbol();
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

        private void dtExpirationDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isValidated())
                    GenerateSymbol();
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

        private void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                _secMasterObj = e.Value;
                if (isValidated())
                {
                    GenerateSymbol();
                    startValidateTimer();
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

        private void pranaOptionCtrl_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (_securityMaster != null)
                {
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
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
        #endregion

        private void event_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                validateGenertatedSymbol();
            }
        }

        private string _privUnderlyingSymbol = string.Empty;
        private double _privStrikePrice = 0;
        private OptionType _privOptionType = OptionType.CALL;
        private DateTime _privExpirationDate = DateTime.MinValue;

        private void validateGenertatedSymbol()
        {
            if (string.IsNullOrEmpty(txtUnderlyingSymbol.Text))
            {
                txtUnderlyingSymbol.Focus();
                return;
            }
            if (ValidateSymbol != null && _secMasterObj != null)
            {
                try
                {
                    OptionDetail optionDetail = new OptionDetail();
                    optionDetail.ExpirationDate = dtExpirationDate.DateTime;
                    optionDetail.StrikePrice = txtStrikePrice.Value;
                    optionDetail.UnderlyingSymbol = txtUnderlyingSymbol.Text;
                    optionDetail.Symbology = _symbology;
                    optionDetail.AssetCategory = _secMasterObj.AssetCategory;
                    optionDetail.AUECID = _secMasterObj.AUECID;
                    optionDetail.OptionType = (OptionType)cmbOptionType.Value;
                    optionDetail.StrikePriceMultiplier = _secMasterObj.StrikePriceMultiplier;
                    optionDetail.EsignalOptionRoot = _secMasterObj.EsignalOptionRoot;
                    optionDetail.BloombergOptionRoot = _secMasterObj.BloombergOptionRoot;
                    if (_privUnderlyingSymbol != optionDetail.UnderlyingSymbol
                        || _privStrikePrice != optionDetail.StrikePrice
                        || _privOptionType != optionDetail.OptionType
                        || _privExpirationDate != optionDetail.ExpirationDate)
                    {
                        OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                        if (!string.IsNullOrEmpty(optionDetail.Symbol))
                        {
                            _privUnderlyingSymbol = optionDetail.UnderlyingSymbol;
                            _privStrikePrice = optionDetail.StrikePrice;
                            _privOptionType = optionDetail.OptionType;
                            _privExpirationDate = optionDetail.ExpirationDate;
                            ValidateSymbol(this, new EventArgs<string, string>(optionDetail.Symbol, optionDetail.UnderlyingSymbol));
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
        }

        private void event_Leave(object sender, EventArgs e)
        {

            if ((sender == txtUnderlyingSymbol && !txtStrikePrice.Focused && !cmbOptionType.Focused && !dtExpirationDate.Focused)
                || (sender == txtStrikePrice && !txtUnderlyingSymbol.SymbolFocused && !cmbOptionType.Focused && !dtExpirationDate.Focused)
                || (sender == cmbOptionType && !txtUnderlyingSymbol.SymbolFocused && !txtStrikePrice.Focused && !dtExpirationDate.Focused)
                || (sender == dtExpirationDate && !txtUnderlyingSymbol.SymbolFocused && !txtStrikePrice.Focused && !cmbOptionType.Focused))
            {
                tmValidate.Stop();
                tmValidate.Start();
            }
        }

        private void tmValidate_Tick(object sender, EventArgs e)
        {
            tmValidate.Stop();
            if (_enableValidate)
            {
                if (!txtUnderlyingSymbol.SymbolFocused && !txtStrikePrice.Focused && !cmbOptionType.Focused && !dtExpirationDate.Focused)
                {
                    validateGenertatedSymbol();
                }
            }
        }

        private bool _enableValidate = false;

        public bool EnableValidate
        {
            get { return _enableValidate; }
            set
            {
                if (_enableValidate != value)
                {
                    _enableValidate = value;
                    _privUnderlyingSymbol = string.Empty;
                    _privStrikePrice = 0;
                    _privOptionType = (OptionType)_defaultOptionType;
                    _privExpirationDate = DateTime.MinValue;
                    txtUnderlyingSymbol.Text = string.Empty;
                    txtStrikePrice.Value = 0;
                    cmbOptionType.Value = _defaultOptionType;
                    dtExpirationDate.Value = DateTime.Now.Date;
                }
            }
        }

        private delegate void StartValidateTimerDelegate();
        public void startValidateTimer()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new StartValidateTimerDelegate(startValidateTimer));
                    return;
                }
                tmValidate.Stop();
                tmValidate.Start();
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

        public PranaSymbolCtrl UnderlyingSymbolControl
        {
            get { return txtUnderlyingSymbol; }
        }

        public void setSymbol(string symbol)
        {
            if (symbol == null || !symbol.StartsWith("O:"))
            {
                return;
            }

            try
            {
                int pos = symbol.IndexOf(' ');
                if (pos <= 0)
                {
                    return;
                }
                string root = symbol.Substring(2, pos - 2);
                string optInfo = symbol.Substring(pos + 1);

                int year = 2000 + int.Parse(optInfo.Substring(0, 2));
                char typeMonth = optInfo.Substring(2, 1).ToCharArray()[0];
                bool isCall = true;
                int month = 0;
                if (typeMonth >= 'A' && typeMonth <= 'L')
                {
                    month = 1 + typeMonth - 'A';
                }
                else if (typeMonth >= 'M' && typeMonth <= 'X')
                {
                    isCall = false;
                    month = 1 + typeMonth - 'M';
                }
                else
                {
                    return;
                }
                pos = optInfo.LastIndexOf('D');
                if (pos <= 0)
                {
                    return;
                }
                int day = int.Parse(optInfo.Substring(pos + 1));

                double strickPrice = double.Parse(optInfo.Substring(3, pos - 3));
                DateTime dt = new DateTime(year, month, day);
                txtUnderlyingSymbol.Text = root;
                txtStrikePrice.Value = strickPrice;
                cmbOptionType.Value = isCall ? (int)OptionType.CALL : (int)OptionType.PUT;
                dtExpirationDate.Value = dt;
                _privUnderlyingSymbol = root;
                _privStrikePrice = strickPrice;
                _privOptionType = isCall ? OptionType.CALL : OptionType.PUT;
                _privExpirationDate = dt;

            }
            catch
            { }
        }
    }
}