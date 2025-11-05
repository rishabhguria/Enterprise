using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinForm;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinToolTip;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SM.OTC.View;
using Prana.Tools.PL;
using Prana.Tools.PL.SecMaster;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Prana.Tools
{
    public partial class SymbolLookUp : Form, IPluggableTools, ISecurityMasterControl, ILaunchForm, IExportGridData
    {
        string _smViewName = string.Empty;
        const string CONST_ADVANCED_SEARCH = "Advanced Search";
        const string CONST_ADD_SECURITY = "Add Security";
        const string CONST_EXPORT_TO_EXCEL = "Export To Excel";
        const string CONST_SAVE_LAYOUT = "Save Layout";
        const string CONST_CLEAR_LAYOUT = "Clear Layout";
        const string CONST_AUTO_CLEAR_DATA_ON_SEARCH = "Clear";
        const string CONST_VALIDATE_SYMBOL_FROM_LIVE_FEED = "Validate Symbol From Live Feed";
        const string CONST_EDIT_UDA = "Edit UDA";
        const string CONST_FUTURE_ROOT_DATA = "Future Root Data";
        const string CONST_AUEC_Mappings = "AUEC Mappings";
        const string CONST_FUND_WISE_UDA = "Account Wise UDA";
        const string CONST_DYNAMIC_UDA = "DYNAMIC_UDA";
        const string CONST_OTCTemplate = "OTC Template";
        const string CONST_InstrumentFields = "Instrument Type Fields";
        private bool _isUpdateRequestFromTradingTicket = false;
        private string _missingSymbologyCode = string.Empty;
        PranaPricingSource _pricingSource = PranaPricingSource.Esignal;
        /// <summary>
        /// Is New OTC Work flow Enabled 
        /// </summary>
        private bool _isNewOTCWorkflowEnabled;

        /// <summary>
        /// dictonary to store auec's multiplier
        /// </summary>
        Dictionary<int, double> _dictMultipliers = CommonDataCache.CachedDataManager.GetInstance.AuecMultipliers;
        /// <summary>
        /// dictionary to store auec's roundlot
        /// </summary>
        Dictionary<int, decimal> _dictRoundLots = CommonDataCache.CachedDataManager.GetInstance.AuecRoundLot;
        /// <summary>
        /// Cache to store dynamic UDAs data
        /// </summary>
        SerializableDictionary<string, DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, DynamicUDA>();

        public SymbolLookUp()
        {
            try
            {
                CreateSMSyncPoxy();
                InitializeComponent();
                _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                SetDynamicUDA(_dynamicUDACache);
                InitializeSearchLayout();
                SetUserPermissions();
                SetExportPermission();
                //Bind data, PRANA-12724
                grdData.DataSource = _secMasterUIobj;
                InitGridRowEditTemplate();

                _isNewOTCWorkflowEnabled = CachedDataManager.GetInstance.IsNewOTCWorkflow;
                SetViewForOTCPreference(_isNewOTCWorkflowEnabled);
                tradeSymbolToolStripMenuItem.Enabled = ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE);
                addPricingInputToolStripMenuItem.Enabled = ModuleManager.CheckModulePermissioning(PranaModules.PRICING_INPUTS_MODULE, PranaModules.PRICING_INPUTS_MODULE);
                InstanceManager.RegisterInstance(this);

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

        private void SetViewForOTCPreference(bool _isNewOTCWorkflowEnabled)
        {
            try
            {

                this.toolBarManager.Tools[CONST_OTCTemplate].SharedProps.Visible = _isNewOTCWorkflowEnabled;
                this.toolBarManager.Tools[CONST_InstrumentFields].SharedProps.Visible = _isNewOTCWorkflowEnabled;

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
        /// Create Proxy for Dynamic UDA
        /// </summary>
        private void CreateSMSyncPoxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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
        /// Set user based permission on symbol lookup UI
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                toolBarManager.Tools[CONST_FUND_WISE_UDA].SharedProps.Visible = false;
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
        /// To disable the Export when Market data provide is SAPI.
        /// </summary>
        private void SetExportPermission()
        {
            try
            {
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    toolBarManager.Tools[CONST_EXPORT_TO_EXCEL].SharedProps.Enabled = false;
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

        private void InitializeSearchLayout()
        {
            try
            {

                BindDataSource();

                btnNext.Visible = false;
                btnPrev.Visible = false;
                rdBtnSearchSymbols.Checked = true;
                //set Auto clear data on search to true (default) 
                StateButtonTool autoClearBtn = toolBarManager.Tools[CONST_AUTO_CLEAR_DATA_ON_SEARCH] as StateButtonTool;
                if (autoClearBtn != null)
                {
                    autoClearBtn.Checked = true;
                }
                toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;
                String SMUISearchParamsFilePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SMUISearchParams.xml";

                if (File.Exists(SMUISearchParamsFilePath))
                {
                    Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();

                    SMUISearchParams searchParams = new SMUISearchParams();
                    _xml.ReadXml(SMUISearchParamsFilePath, searchParams);
                    SetSearchParamsOnUI(searchParams);
                }
                else
                {
                    cmbbxSMView.Value = SecMasterConstants.SmViewType.AllColumns.ToString();
                }

                _pageSize = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SMChunkSize").ToString());

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
        /// Bind Datasource to comboBox, PRANA-12783
        /// </summary>
        private void BindDataSource()
        {
            try
            {
                cmbbxSearchCriteria.DataSource = Prana.Utilities.UI.MiscUtilities.EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SearchCriteria));
                cmbbxSearchCriteria.DataBind();
                cmbbxSearchCriteria.DisplayMember = "DisplayText";
                cmbbxSearchCriteria.ValueMember = "Value";
                cmbbxSearchCriteria.Value = SecMasterConstants.SearchCriteria.Ticker;
                cmbbxSearchCriteria.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbbxMatchOn.DataSource = Prana.Utilities.UI.MiscUtilities.EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SearchMatchOn));
                cmbbxMatchOn.DataBind();
                cmbbxMatchOn.DisplayMember = "DisplayText";
                cmbbxMatchOn.ValueMember = "Value";
                cmbbxMatchOn.Value = SecMasterConstants.SearchMatchOn.Contains;
                cmbbxMatchOn.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbbxSMView.DataSource = Prana.Utilities.UI.MiscUtilities.EnumHelper.ConvertEnumForBindingWithCaption(typeof(SecMasterConstants.SmViewType));
                cmbbxSMView.DataBind();
                cmbbxSMView.DisplayMember = "DisplayText";
                cmbbxSMView.ValueMember = "Value";
                cmbbxSMView.Value = SecMasterConstants.SmViewType.CustomView;
                cmbbxSMView.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
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

        #region Variables Section

        //changed the datatype from list to bindinglist, PRANA-12724
        BindingList<SecMasterUIObj> _secMasterUIobj = new BindingList<SecMasterUIObj>();
        List<SecMasterUIObj> _secMasterUIobjOld = new List<SecMasterUIObj>();
        List<string> _equityColumns = null;
        List<string> _optionColumns = null;
        List<string> _futureColumns = null;
        List<string> _fxColumns = null;
        List<string> _fixedIncomeColumns = null;
        List<string> _indexColumns = null;
        List<string> _fxForwardColumns = null;

        SymbolLookupRequestObject symbolLookupRequestObject = null;
        bool _isAlreadyRequested = true;
        private int pageIndex = 0;
        private int _pageSize = 20;
        FutureRootSymbolUI futureRootSymUI = null;
        UDAUIForm udaUI = null;
        AdvSearchFilterUI advSearchFilertUI = null;
        bool _isAddedRow = false;
        bool _isAdvncdSearching = false;
        String _advncdSearchQuery = string.Empty;
        bool _isSearchInProcess = false;
        bool _isLocalDBSearchInProcess = false;
        bool _isLayoutChanged = false;
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        int _loggedInUserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        public Boolean _isClosingFromExtSource = false;
        private Boolean _isSecurityAutoApproved = false;

        public delegate void ConnectionInvokeDelegate(object sender, EventArgs e);
        public delegate void SymbolLookUpRequest(object sender, EventArgs<DataSet> e);
        public delegate void DelegateSecMasterRes(object sender, EventArgs<SecMasterBaseObj> e);
        public delegate void AllTradedSymbolsRequest(object sender, EventArgs<List<SecMasterBaseObj>> e);
        public delegate void SMQMsgInvokeDelegate(object sender, EventArgs<QueueMessage> e);

        // Added by Bhavana on July 9, 2014
        // Purpose : To add Validation from LiveFeed option
        LiveFeedValidationForm liveFeedValidateUI = null;

        /// <summary>
        /// Form to hold the control for Account Wise UDA
        /// </summary>
        Form frmAccountWiseUDA;

        /// <summary>
        /// Form to hold the control for Dynamic UDA
        /// </summary>
        DynamicUDAForm _frmDynamicUDA;

        /// <summary>
        /// Control to generate new batch
        /// </summary>
        ctrlAccountWiseUDA accountWiseUDA;

        AUECMappingUI auecMappingUI = null;

        /// <summary>
        /// set this to true is underlying symbol request is in process from server
        /// </summary>
        bool _isSetIssuer = false;
        #endregion

        private void DisableColumns(UltraGridRow row, List<string> visibleColumns)
        {
            try
            {
                foreach (UltraGridColumn colvar in row.Band.Columns)
                {
                    UltraGridCell cell = row.Cells[colvar];

                    string name = colvar.Key.ToString();
                    if (!visibleColumns.Contains(name))
                    {
                        cell.Activation = Activation.Disabled;
                    }
                    else
                    {
                        cell.Activation = Activation.AllowEdit;
                    }
                    if (colvar.Key.ToString() == "CountryCode" || colvar.Key.ToString() == "Select")
                    {
                        cell.Activation = Activation.AllowEdit;
                    }

                }
                row.Cells["CreationDate"].Activation = Activation.NoEdit;
                row.Cells["ModifiedDate"].Activation = Activation.NoEdit;

                // Kuldeep A.: disabling Leveraged factor on Security Information Template for options and covertible bonds as in case of these there is no relavance of Leveraged factor.
                if (((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.EquityOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FutureOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FXOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.ConvertibleBond)
                {
                    row.Cells["Delta"].Activation = Activation.Disabled;
                    row.Cells["StrikePriceMultiplier"].Activation = Activation.Disabled;
                    row.Cells["EsignalOptionRoot"].Activation = Activation.Disabled;
                    row.Cells["BloombergOptionRoot"].Activation = Activation.Disabled;
                }
                row.Cells["DataSource"].Activation = Activation.Disabled;
                row.Cells["CreationDate"].Activation = Activation.Disabled;
                row.Cells["ApprovedBy"].Activation = Activation.Disabled;
                row.Cells["SecApprovalStatus"].Activation = Activation.Disabled;
                row.Cells["ModifiedDate"].Activation = Activation.Disabled;
                row.Cells["CreatedBy"].Activation = Activation.Disabled;
                row.Cells["ApprovalDate"].Activation = Activation.Disabled;
                row.Cells["ModifiedBy"].Activation = Activation.Disabled;
                row.Cells["IsSecApproved"].Activation = Activation.Disabled;
                row.Cells["IsCurrencyFuture"].Activation = Activation.Disabled;
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
        private void RequestData()
        {
            try
            {
                if (symbolLookupRequestObject != null)
                {
                    SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbbxSearchCriteria.Value.ToString());
                    _pageSize = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SMChunkSize").ToString());
                    //Condition modified to check if search criteria is company name or underlying symbol, PRANA-13894
                    _pageSize = (pageIndex == 0 && searchCriteria != SecMasterConstants.SearchCriteria.CompanyName && searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol) ? (_pageSize - 1) : _pageSize;
                    symbolLookupRequestObject.StartIndex = pageIndex * _pageSize + 1;
                    symbolLookupRequestObject.EndIndex = (pageIndex + 1) * _pageSize;
                    symbolLookupRequestObject.RequestID = System.Guid.NewGuid().ToString();
                    _securityMaster.GetSymbolLookupRequestedData(symbolLookupRequestObject);

                    SetGetButtonDetails("Getting data ..", false);
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
        private string GetCompleteText(string text)
        {
            if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.Contains.ToString()))
            {
                text = "%" + text + "%";
            }
            else if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.Exact.ToString()))
            {

            }
            else if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.StartsWith.ToString()))
            {
                text = text + "%";
            }
            return text;
        }
        private void SetGetButtonDetails(string text, bool enabled)
        {
            btnGetData.Text = text;
            btnGetData.Enabled = enabled;
            btnNext.Enabled = enabled;
            btnPrev.Enabled = enabled;
            if (_securityMaster.IsConnected)
            {
                toolStripStatusLabel1.ForeColor = toolStripStatusLabel1.GetCurrentParent().ForeColor;
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
            if (!enabled)
            {
                toolStripStatusLabel1.Text = "Getting data......";
                if (_isClearDataOnSearch)
                {
                    _secMasterUIobj.Clear();
                    _secMasterUIobjOld.Clear();
                    //Commented as grid is already bind to datasource, PRANA-12724
                    //grdData.DataBind();
                }
            }
        }

        private bool ValidateSymbolForSave(SecMasterUIObj uiObj)
        {
            if (string.IsNullOrEmpty(uiObj.TickerSymbol) || string.IsNullOrWhiteSpace(uiObj.TickerSymbol))
            {
                lblStatus.Text = toolStripStatusLabel1.Text = " Not Saved, Please enter Ticker Symbol";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(uiObj.LongName) || string.IsNullOrWhiteSpace(uiObj.LongName))
            {
                lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Description";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (uiObj.CurrencyID == int.MinValue)
            {
                lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Currency ";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(uiObj.UnderLyingSymbol) || string.IsNullOrWhiteSpace(uiObj.UnderLyingSymbol))
            {
                lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter UnderLyingSymbol ";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }

            if (Math.Round(uiObj.RoundLot, 10) <= 0)
            {
                lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, RoundLots can not be less than equal to 0 ";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }

            if (Math.Round(uiObj.Multiplier, 10) <= 0)
            {
                lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Multiplier can not be blank or less than equal to zero. ";
                toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }

            AssetCategory assetCategory = (AssetCategory)uiObj.AssetID;

            // FXForward ticker must conatain date part, PRANA-10852
            if (assetCategory == AssetCategory.FXForward)
            {
                string[] tickerSym = uiObj.TickerSymbol.Split(' ');
                DateTime dt = DateTime.MinValue;
                bool hasDatePart = false;
                foreach (string namePart in tickerSym)
                {
                    if (DateTime.TryParse(namePart, out dt))
                        hasDatePart = true;
                }
                if (!hasDatePart)
                {
                    lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Ticker Symbol must contain date";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    return false;
                }
            }

            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);
            switch (baseAssetCategory)
            {
                case AssetCategory.Equity:
                    break;

                case AssetCategory.Option:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.PutOrCall == int.MinValue)
                    {
                        lblStatus.Text = toolStripStatusLabel1.Text = "TickerSymbol: " + uiObj.TickerSymbol + ": Not Saved,Please select OptionType ";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.StrikePrice == 0)
                    {
                        lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Strike Price ";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }

                    break;

                case AssetCategory.Future:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }

                    if (assetCategory == AssetCategory.FXForward)
                    {
                        if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                        {
                            lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Expiration date";
                            toolStripStatusLabel1.ForeColor = Color.Red;
                            return false;
                        }
                    }

                    break;
                case AssetCategory.FixedIncome:
                case AssetCategory.ConvertibleBond:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.IsZero.Equals(false))
                    {
                        if (uiObj.Coupon == 0)
                        {
                            lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Coupon";
                            toolStripStatusLabel1.ForeColor = Color.Red;
                            return false;
                        }
                    }
                    break;
                case AssetCategory.PrivateEquity:
                case AssetCategory.CreditDefaultSwap:
                    break;
            }

            // Added regular expression validation for dynamic uda value input 
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-9369
            foreach (string uda in uiObj.DynamicUDA.Keys)
            {
                if (!Regex.IsMatch(uiObj.DynamicUDA[uda].ToString(), SecMasterConstants.CONST_DynamicUDAValueRegex))
                {
                    lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please correct value of " + uda + ". It should contain only alphanumeric and some special characters";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    return false;
                }
            }

            toolStripStatusLabel1.ForeColor = toolStripStatusLabel1.GetCurrentParent().ForeColor;
            return true;
        }

        private void SetEditableRowColors(UltraGridRow row)
        {
            row.Appearance.BackColor = Color.Gray;

            if (row.Activation == Activation.AllowEdit)
            {
                row.Appearance.ForeColor = Color.Black;
            }
            else
            {
                if (row.IsAlternate)
                    row.Appearance.BackColor = Color.Black;
                else
                    row.Appearance.BackColor = Color.Black;

            }
            grdData.PerformAction(UltraGridAction.EnterEditMode, false, false);

        }

        private void SetSelectedRowColors(UltraGridRow row)
        {
            row.Appearance.BackColor = Color.Gray;
            if (row.Selected)
            {
                row.Appearance.BackColor = Color.Gold;
                row.Appearance.ForeColor = Color.Black;

            }
            else
            {
                if (row.IsAlternate)
                    row.Appearance.BackColor = Color.Black;
                else
                    row.Appearance.BackColor = Color.Black;
                row.Appearance.ForeColor = Color.Orange;

            }
        }

        private void Update(object sender, EventArgs<DataSet> e)
        {
            try
            {
                DataSet ds = e.Value;
                //Need to Clear the UiObj before filling new data
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3250
                if (pageIndex != 0)
                    _secMasterUIobj.Clear();
                if (ds == null || ds.Tables.Count == 0)
                {
                    if (_secMasterUIobj.Count == 0)
                        toolStripStatusLabel1.Text = "No data found. Please refine your search!";
                    return;
                }
                else
                {
                    if (ds.Tables[0].Rows.Count >= _pageSize)
                    {
                        btnNext.Visible = true;
                        if (pageIndex == 0)
                        {
                            btnPrev.Visible = false;
                        }
                        else
                        {
                            btnPrev.Visible = true;
                        }
                    }
                    else
                    {
                        btnNext.Visible = false;
                        if (pageIndex == 0)
                        {
                            btnPrev.Visible = false;
                        }
                        else
                        {
                            btnPrev.Visible = true;
                        }
                    }

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SecMasterUIObj secMasterUIobj = new SecMasterUIObj();
                        Transformer.CreateObjThroughReflection(dr, secMasterUIobj);
                        AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)secMasterUIobj.AssetID);

                        switch (baseAssetCategory)
                        {
                            case BusinessObjects.AppConstants.AssetCategory.Option:

                                DateTime expirationDate = DateTimeConstants.MinValue; ;
                                DateTime.TryParse(dr["OPTExpiration"].ToString(), out expirationDate);
                                secMasterUIobj.ExpirationDate = expirationDate;
                                if (dr["OptionName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["OptionName"].ToString();
                                }
                                if (dr["Type"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.PutOrCall = (Convert.ToInt32(dr["Type"]));
                                }
                                if (dr["OSISymbol"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.OSIOptionSymbol = dr["OSISymbol"].ToString();
                                }
                                if (dr["IDCOSymbol"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.IDCOOptionSymbol = dr["IDCOSymbol"].ToString();
                                }
                                if (dr["OPRASymbol"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.OPRAOptionSymbol = dr["OPRASymbol"].ToString();
                                }
                                if (dr["OPTMultiplier"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.Multiplier = double.Parse(dr["OPTMultiplier"].ToString());
                                }
                                if (dr["OPTIsCurrencyFuture"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.IsCurrencyFuture = Convert.ToBoolean(dr["OPTIsCurrencyFuture"].ToString());
                                }

                                break;

                            case BusinessObjects.AppConstants.AssetCategory.Future:

                                if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
                                {
                                    DateTime futExpirationDate = DateTime.MinValue;
                                    DateTime.TryParse(dr["FUTExpiration"].ToString(), out futExpirationDate);
                                    secMasterUIobj.ExpirationDate = futExpirationDate;
                                    if (dr["FxContractName"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.LongName = dr["FxContractName"].ToString();
                                    }
                                    if (dr["FxForwardMultiplier"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.Multiplier = double.Parse(dr["FxForwardMultiplier"].ToString());
                                    }
                                    if (dr["IsNDF"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
                                    }
                                    if (dr["FixingDate"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
                                    }
                                    if (dr["LeadCurrencyID"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.LeadCurrencyID = Convert.ToInt32(dr["LeadCurrencyID"].ToString());
                                    }
                                    if (dr["VsCurrencyID"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.VsCurrencyID = Convert.ToInt32(dr["VsCurrencyID"].ToString());
                                    }
                                }
                                else
                                {
                                    DateTime futExpirationDate = DateTime.MinValue;
                                    DateTime.TryParse(dr["FUTExpiration"].ToString(), out futExpirationDate);
                                    secMasterUIobj.ExpirationDate = futExpirationDate;
                                    if (dr["FUTMultiplier"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.Multiplier = double.Parse(dr["FUTMultiplier"].ToString());
                                    }
                                    if (dr["FutureName"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.LongName = dr["FutureName"].ToString();
                                    }
                                    if (dr["FUTIsCurrencyFuture"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.IsCurrencyFuture = Convert.ToBoolean(dr["FUTIsCurrencyFuture"].ToString());
                                    }
                                }

                                break;
                            case BusinessObjects.AppConstants.AssetCategory.FX:

                                if (dr["FxContractName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["FxContractName"].ToString();
                                }
                                if (dr["FxMultiplier"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.Multiplier = Convert.ToDouble(dr["FxMultiplier"].ToString());
                                }
                                if (dr["IsNDF"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.IsNDF = Convert.ToBoolean(dr["IsNDF"].ToString());
                                }
                                if (dr["FixingDate"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.FixingDate = Convert.ToDateTime(dr["FixingDate"].ToString());
                                }
                                if (dr["FxExpirationDate"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["FxExpirationDate"].ToString());
                                }
                                if (dr["LeadCurrencyID"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LeadCurrencyID = Convert.ToInt32(dr["LeadCurrencyID"].ToString());
                                }
                                if (dr["VsCurrencyID"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.VsCurrencyID = Convert.ToInt32(dr["VsCurrencyID"].ToString());
                                }
                                break;
                            case BusinessObjects.AppConstants.AssetCategory.Indices:
                                if (dr["IndexLongName"] != System.DBNull.Value)
                                {
                                    secMasterUIobj.LongName = dr["IndexLongName"].ToString();
                                }
                                break;
                            case BusinessObjects.AppConstants.AssetCategory.FixedIncome:
                            case BusinessObjects.AppConstants.AssetCategory.ConvertibleBond:
                                {
                                    if (dr["FixedIncomeLongName"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.LongName = dr["FixedIncomeLongName"].ToString();
                                    }
                                    if (dr["FIMultiplier"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.Multiplier = Convert.ToDouble(dr["FIMultiplier"].ToString());
                                    }
                                    if (dr["MaturityDate"] != System.DBNull.Value)
                                    {
                                        secMasterUIobj.ExpirationDate = Convert.ToDateTime(dr["MaturityDate"].ToString());
                                    }
                                    break;
                                }

                        }

                        // update risk currency field automatically
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-9214
                        if (secMasterUIobj.DynamicUDA.ContainsKey("RiskCurrency"))
                            secMasterUIobj.DynamicUDA["RiskCurrency"] = GetRiskCurrency(secMasterUIobj);
                        else
                            secMasterUIobj.DynamicUDA.Add("RiskCurrency", GetRiskCurrency(secMasterUIobj));

                        _secMasterUIobj.Insert(_secMasterUIobj.Count, secMasterUIobj);

                    }

                    //TODO- Use paging contraol instead of getting data by Index, Check server side paging available or not. -om
                    StringBuilder msgSB = new StringBuilder();
                    if (_secMasterUIobj.Count == 0)
                    {
                        if (pageIndex > 0)
                        {
                            msgSB.Append("No More Matching");
                        }
                        else
                        {
                            msgSB.Append("0 Matching");
                        }
                    }
                    else
                    {
                        msgSB.Append(pageIndex * _pageSize + 1);
                        msgSB.Append(" - ");
                        msgSB.Append(pageIndex * _pageSize + _secMasterUIobj.Count);
                        msgSB.Append(" Matching");
                    }

                    if (!_isClearDataOnSearch && !btnNext.Visible)
                    {
                        SecMasterUIObjAddRange();
                        msgSB.Append(" and ");
                        msgSB.Append(_secMasterUIobjOld.Count);
                        msgSB.Append(" Old");
                    }
                    msgSB.Append(" Records");
                    toolStripStatusLabel1.Text = msgSB.ToString();

                    SetGetButtonDetails("Get Data", true);
                    InitGridRowEditTemplate();

                    if (grdData.Rows.Count > 0)
                    {
                        FxColumnSettings(grdData.Rows[0]);
                    }
                    if (_isUpdateRequestFromTradingTicket)
                    {
                        SetupSecurityInformationDialog();
                    }
                }
                grdData.Enabled = true;
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
        /// Add items in bindinglist from list,  PRANA-12724
        /// </summary>
        private void SecMasterUIObjAddRange()
        {
            try
            {
                if (_secMasterUIobjOld != null)
                {
                    foreach (SecMasterUIObj secMasterUIObj in _secMasterUIobjOld)
                    {
                        if (!IsContain(secMasterUIObj))
                            _secMasterUIobj.Add(secMasterUIObj);
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
        /// Check if list contains SecMasterUIObj object or not, PRANA-12724
        /// </summary>
        /// <param name="secMasterUIObj"></param>
        /// <returns></returns>
        private bool IsContain(SecMasterUIObj secMasterUIObj)
        {
            bool isContain = false;
            try
            {
                Parallel.ForEach(_secMasterUIobj, (x, state) =>
                {
                    if (x.TickerSymbol.Equals(secMasterUIObj.TickerSymbol))
                    {
                        isContain = true;
                        state.Break();
                    }
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isContain;
        }

        /// <summary>
        /// Gets value of risk currency
        /// </summary>
        /// <param name="secMasterUIobj"></param>
        /// <returns></returns>
        private string GetRiskCurrency(SecMasterUIObj secMasterUIobj)
        {
            try
            {
                int baseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                string riskCurrency = string.Empty;
                if ((AssetCategory)secMasterUIobj.AssetID == AssetCategory.FX || (AssetCategory)secMasterUIobj.AssetID == AssetCategory.FXForward)
                {
                    if (secMasterUIobj.VsCurrencyID == baseCurrency)
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(secMasterUIobj.LeadCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUIobj.LeadCurrencyID] : string.Empty;
                    else
                        riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(secMasterUIobj.VsCurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUIobj.VsCurrencyID] : string.Empty;
                }
                else
                    riskCurrency = CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(secMasterUIobj.CurrencyID) ? CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUIobj.CurrencyID] : string.Empty;

                return riskCurrency;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        private void SetupSecurityInformationDialog()
        {
            if (grdData.Rows.Count > 0)
            {
                this.grdData.Rows[0].Activate();
                if (grdData.ActiveRow != null)
                {
                    switch (_missingSymbologyCode)
                    {
                        case "SEDOL":
                            grdData.ActiveRow.Cells[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].Activate();
                            break;
                        case "CUSIP":
                        case "CINS":
                            grdData.ActiveRow.Cells[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Activate();
                            break;
                        case "RIC":
                            grdData.ActiveRow.Cells[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].Activate();
                            break;
                        case "ISIN":
                            grdData.ActiveRow.Cells[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].Activate();
                            break;
                        case "OSI":
                            grdData.ActiveRow.Cells[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].Activate();
                            break;
                    }
                    this.grdData.DisplayLayout.Override.RowEditTemplateUIType |= RowEditTemplateUIType.OnEnterEditMode;
                    grdData.PerformAction(UltraGridAction.EnterEditMode);
                    _missingSymbologyCode = string.Empty;
                }
            }
        }

        private void UnwireEvents()
        {
            try
            {
                if (_dynamicUDACache != null)
                {
                    _dynamicUDACache.Clear();
                    _dynamicUDACache = null;
                }
                if (futureRootSymUI != null)
                {
                    futureRootSymUI.Close();
                }
                if (_frmDynamicUDA != null)
                {
                    _frmDynamicUDA.SaveDynamicUDA -= frmDynamicUDA_SaveDynamicUDA;
                    _frmDynamicUDA.CheckMasterValueAssigned -= _frmDynamicUDA_CheckMasterValueAssigned;
                    _frmDynamicUDA.Close();
                }
                if (advSearchFilertUI != null)
                {
                    advSearchFilertUI.Close();
                }
                if (udaUI != null)
                {
                    udaUI.Close();
                }
                if (auecMappingUI != null)
                {
                    auecMappingUI.Close();
                }
                if (liveFeedValidateUI != null)
                {
                    liveFeedValidateUI.Close();
                }

                if (_securityMaster != null)
                {
                    _securityMaster.SymbolLookUpDataResponse -= new EventHandler<EventArgs<DataSet>>(_securityMaster_SymbolLookUpDataResponse);
                    _securityMaster.ResponseCompleted -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                    _securityMaster.Disconnected -= new EventHandler(_securityMaster_Disconnected);
                    _securityMaster.Connected -= new EventHandler(_securityMaster_Connected);

                    _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                    _securityMaster.EventTradedSMDataUIRes -= new EventHandler<EventArgs<List<SecMasterBaseObj>>>(_securityMaster_EventTradedSMDataUIRes);
                    _securityMaster.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);

                    _securityMaster.FutSymbolRootDataRes -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_FutSymbolRootDataRes);
                    _securityMaster.FutRootDataSaveRes -= new EventHandler<EventArgs<QueueMessage>>(_securityMaster_FutRootDataSaveRes);
                }
                if (this.LaunchPricingInput != null)
                {
                    this.LaunchPricingInput -= SymbolLookup_LaunchPricingInput;
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

        private void SymbolLookup_LaunchPricingInput(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void SetGridColumns(UltraGridBand gridBand)
        {
            try
            {
                gridBand.Columns[OrderFields.PROPERTY_ASSET_ID].Header.VisiblePosition = 1;
                gridBand.Columns[OrderFields.PROPERTY_ASSET_ID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns[OrderFields.PROPERTY_ASSET_ID].Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
                gridBand.Columns[OrderFields.PROPERTY_ASSET_ID].ValueList = SecMasterHelper.getInstance().Assets.Clone();
                gridBand.Columns[OrderFields.PROPERTY_ASSET_ID].Header.Column.Width = 70;

                gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID].Header.VisiblePosition = 2;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID].Header.Caption = OrderFields.CAPTION_UNDERLYING_NAME;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID].ValueList = SecMasterHelper.getInstance().UnderLyings.Clone();
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYING_ID].Header.Column.Width = 70;

                gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID].Header.VisiblePosition = 3;
                gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID].Header.Caption = OrderFields.CAPTION_EXCHANGE;
                gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID].ValueList = SecMasterHelper.getInstance().Exchanges.Clone();
                gridBand.Columns[OrderFields.PROPERTY_EXCHANGEID].Header.Column.Width = 70;

                gridBand.Columns[OrderFields.PROPERTY_CURRENCYID].Header.VisiblePosition = 4;
                gridBand.Columns[OrderFields.PROPERTY_CURRENCYID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns[OrderFields.PROPERTY_CURRENCYID].Header.Caption = "Currency";
                gridBand.Columns[OrderFields.PROPERTY_CURRENCYID].ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                gridBand.Columns[OrderFields.PROPERTY_CURRENCYID].Header.Column.Width = 70;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Header.VisiblePosition = 5;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Header.Column.Width = 100;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].NullText = String.Empty;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].SortIndicator = SortIndicator.Ascending;

                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].Width = 100;
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].Header.VisiblePosition = 6;
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].Header.Column.Width = 150;
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].Header.Caption = "Description";
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[OrderFields.PROPERTY_LONGNAME].NullText = String.Empty;

                gridBand.Columns["ProxySymbol"].Header.VisiblePosition = 7;
                gridBand.Columns["ProxySymbol"].Header.Column.Width = 100;
                gridBand.Columns["ProxySymbol"].Header.Caption = "Proxy Symbol";
                gridBand.Columns["ProxySymbol"].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns["ProxySymbol"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["ProxySymbol"].NullText = String.Empty;

                gridBand.Columns["FactSetSymbol"].Header.VisiblePosition = 8;
                gridBand.Columns["FactSetSymbol"].Header.Column.Width = 100;
                gridBand.Columns["FactSetSymbol"].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns["FactSetSymbol"].Header.Caption = "FactSet Symbol";
                gridBand.Columns["FactSetSymbol"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["FactSetSymbol"].NullText = String.Empty;

                gridBand.Columns["ActivSymbol"].Header.VisiblePosition = 9;
                gridBand.Columns["ActivSymbol"].Header.Column.Width = 100;
                gridBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns["ActivSymbol"].Header.Caption = "ACTIV Symbol";
                gridBand.Columns["ActivSymbol"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["ActivSymbol"].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].Header.VisiblePosition = 10;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].Header.Column.Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].Header.VisiblePosition = 11;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].Header.Column.Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL_WithCompositeCode;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Header.VisiblePosition = 12;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Header.Column.Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].Header.VisiblePosition = 13;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].Header.Column.Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].Header.VisiblePosition = 14;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].Header.Column.Width = 70;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].Header.VisiblePosition = 15;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].Header.Column.Width = 150;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].Header.Caption = OrderFields.CAPTION_OSIOPTIONSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].Header.VisiblePosition = 16;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].Header.Column.Width = 150;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()].NullText = String.Empty;

                gridBand.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].Header.Column.Width = 70;
                gridBand.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].Width = 70;
                gridBand.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL_WithExchangeCode;
                gridBand.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE].NullText = String.Empty;

                gridBand.Columns[ApplicationConstants.SymbologyCodes.OPRAOptionSymbol.ToString()].Hidden = true;

                gridBand.Columns[OrderFields.PROPERTY_SECTOR].Hidden = true;
                gridBand.Columns[OrderFields.PROPERTY_SECTOR].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                gridBand.Columns["Multiplier"].Header.VisiblePosition = 17;
                gridBand.Columns["Multiplier"].Header.Column.Width = 70;
                gridBand.Columns["Multiplier"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                gridBand.Columns["Multiplier"].NullText = null;

                gridBand.Columns["IssueDate"].Header.VisiblePosition = 18;
                gridBand.Columns["IssueDate"].Header.Column.Width = 70;

                gridBand.Columns["FirstCouponDate"].Header.VisiblePosition = 19;
                gridBand.Columns["FirstCouponDate"].Header.Column.Width = 70;

                gridBand.Columns["ExpirationDate"].Header.VisiblePosition = 20;
                gridBand.Columns["ExpirationDate"].Header.Caption = OrderFields.CAPTION_EXPIRATIONDATE;
                gridBand.Columns["ExpirationDate"].Header.Column.Width = 70;
                gridBand.Columns["ExpirationDate"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["ExpirationDate"].NullText = "1/1/1800";

                gridBand.Columns["FixingDate"].Header.VisiblePosition = 21;
                gridBand.Columns["FixingDate"].Header.Caption = "Fixing Date";
                gridBand.Columns["FixingDate"].Header.Column.Width = 70;
                gridBand.Columns["FixingDate"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["FixingDate"].NullText = "1/1/1800";

                gridBand.Columns["IsNDF"].Header.VisiblePosition = 22;
                gridBand.Columns["IsNDF"].Header.Caption = "Is NDF";
                gridBand.Columns["IsNDF"].Header.Column.Width = 70;

                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Header.VisiblePosition = 23;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Header.Column.Width = 70;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL].NullText = String.Empty;

                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].Header.VisiblePosition = 24;
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].ValueList = SecMasterHelper.getInstance().OptionTypes.Clone();
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].Header.Column.Width = 70;
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].Header.Caption = OrderFields.CAPTION_PUT_CALL;
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns[OrderFields.PROPERTY_PUT_CALL].NullText = String.Empty;

                gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Header.VisiblePosition = 25;
                gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Header.Column.Width = 70;
                gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                gridBand.Columns[OrderFields.PROPERTY_STRIKE_PRICE].NullText = null;

                gridBand.Columns["Delta"].Header.VisiblePosition = 26;
                gridBand.Columns["Delta"].Header.Column.Width = 70;
                gridBand.Columns["Delta"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                gridBand.Columns["Delta"].NullText = null;
                gridBand.Columns["Delta"].Header.Caption = "Leveraged Factor";

                gridBand.Columns["BondTypeID"].Header.VisiblePosition = 27;
                gridBand.Columns["BondTypeID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["BondTypeID"].ValueList = SecMasterHelper.getInstance().SecurityTypes.Clone();
                gridBand.Columns["BondTypeID"].Header.Column.Width = 100;
                gridBand.Columns["BondTypeID"].Header.Caption = "Bond Type";

                gridBand.Columns["AccrualBasisID"].Header.VisiblePosition = 28;
                gridBand.Columns["AccrualBasisID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["AccrualBasisID"].Header.Caption = OrderFields.CAPTION_ACCRUALBASIS;
                gridBand.Columns["AccrualBasisID"].ValueList = SecMasterHelper.getInstance().AccrualBasis.Clone();
                gridBand.Columns["AccrualBasisID"].Header.Column.Width = 100;

                gridBand.Columns["CouponFrequencyID"].Header.VisiblePosition = 29;
                gridBand.Columns["CouponFrequencyID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["CouponFrequencyID"].Header.Caption = "Coupon Frequency";
                gridBand.Columns["CouponFrequencyID"].ValueList = SecMasterHelper.getInstance().Frequencies.Clone();
                gridBand.Columns["CouponFrequencyID"].Header.Column.Width = 100;

                gridBand.Columns["CollateralTypeID"].Header.VisiblePosition = 30;
                gridBand.Columns["CollateralTypeID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["CollateralTypeID"].ValueList = SecMasterHelper.getInstance().CollateralType.Clone();
                gridBand.Columns["CollateralTypeID"].Header.Column.Width = 100;
                gridBand.Columns["CollateralTypeID"].Header.Caption = "Collateral Type";

                gridBand.Columns["Coupon"].Header.Caption = "Coupon (%)";
                gridBand.Columns["Coupon"].Header.Column.Width = 100;

                gridBand.Columns["CreationDate"].Hidden = true;
                gridBand.Columns["CreationDate"].Header.Caption = "Creation Date";
                gridBand.Columns["CreationDate"].Header.Column.Width = 100;
                gridBand.Columns["CreationDate"].CellActivation = Activation.NoEdit;

                gridBand.Columns["ModifiedDate"].Hidden = true;
                gridBand.Columns["ModifiedDate"].Header.Caption = "Modified Date";
                gridBand.Columns["ModifiedDate"].Header.Column.Width = 100;
                gridBand.Columns["ModifiedDate"].CellActivation = Activation.NoEdit;

                gridBand.Columns["CountryCode"].Hidden = true;

                gridBand.Columns["RoundLot"].Header.Caption = "RoundLots";
                gridBand.Columns["RoundLot"].Header.Column.Width = 100;

                #region Security approval status related fields
                if (!gridBand.Columns.Exists("Select"))
                {
                    gridBand.Columns.Add("Select");
                    gridBand.Columns["Select"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    gridBand.Columns["Select"].Header.Caption = "";
                    gridBand.Columns["Select"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    gridBand.Columns["Select"].DataType = typeof(Boolean);
                    gridBand.Columns["Select"].Header.Column.Width = 40;
                    gridBand.Columns["Select"].CellActivation = Activation.AllowEdit;
                    gridBand.Columns["Select"].Header.VisiblePosition = 0;
                    gridBand.Columns["Select"].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    gridBand.Columns["Select"].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    gridBand.Columns["Select"].Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    gridBand.Columns["Select"].AllowRowFiltering = DefaultableBoolean.False;
                }

                SetGridUDAColumns(_dynamicUDACache);

                gridBand.Columns[ApplicationConstants.CONST_IS_SECURITY_APPROVED].Hidden = true;

                gridBand.Columns[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Header.Caption = "Approval Status";
                gridBand.Columns[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Header.Column.Width = 70;

                gridBand.Columns["ApprovedBy"].Header.Caption = "Last Approved By ";
                gridBand.Columns["ApprovedBy"].Header.Column.Width = 100;

                #endregion

                gridBand.Columns["UDAAssetClassID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["UDAAssetClassID"].Header.Caption = "UDA Asset";
                gridBand.Columns["UDAAssetClassID"].ValueList = SecMasterHelper.getInstance().UDAAssets;
                gridBand.Columns["UDAAssetClassID"].Header.Column.Width = 100;

                gridBand.Columns["UDASectorID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["UDASectorID"].Header.Caption = "UDA Sector";
                gridBand.Columns["UDASectorID"].ValueList = SecMasterHelper.getInstance().UDASectors;
                gridBand.Columns["UDASectorID"].Header.Column.Width = 100;

                gridBand.Columns["UDASubSectorID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["UDASubSectorID"].Header.Caption = "UDA SubSector";
                gridBand.Columns["UDASubSectorID"].ValueList = SecMasterHelper.getInstance().UDASubSectors;
                gridBand.Columns["UDASubSectorID"].Header.Column.Width = 100;

                gridBand.Columns["UDASecurityTypeID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["UDASecurityTypeID"].Header.Caption = "UDA Security";
                gridBand.Columns["UDASecurityTypeID"].ValueList = SecMasterHelper.getInstance().UDASecurityTypes;
                gridBand.Columns["UDASecurityTypeID"].Header.Column.Width = 100;

                gridBand.Columns["UDACountryID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                gridBand.Columns["UDACountryID"].Header.Caption = "UDA Country";
                gridBand.Columns["UDACountryID"].ValueList = SecMasterHelper.getInstance().UDACountries;
                gridBand.Columns["UDACountryID"].Header.Column.Width = 100;

                gridBand.Columns["StrikePriceMultiplier"].Header.Caption = "Strike Price Multiplier";
                gridBand.Columns["StrikePriceMultiplier"].Header.Column.Width = 150;
                gridBand.Columns["StrikePriceMultiplier"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                gridBand.Columns["StrikePriceMultiplier"].NullText = null;

                gridBand.Columns["EsignalOptionRoot"].Header.Caption = "Esignal Option Root";
                gridBand.Columns["EsignalOptionRoot"].Header.Column.Width = 150;
                gridBand.Columns["EsignalOptionRoot"].CharacterCasing = CharacterCasing.Upper;
                gridBand.Columns["EsignalOptionRoot"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["EsignalOptionRoot"].NullText = String.Empty;

                gridBand.Columns["BloombergOptionRoot"].Header.Caption = "Bloomberg Option Root";
                gridBand.Columns["BloombergOptionRoot"].Header.Column.Width = 150;
                gridBand.Columns["BloombergOptionRoot"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                gridBand.Columns["BloombergOptionRoot"].NullText = String.Empty;

                gridBand.Columns["SharesOutstanding"].Header.Caption = "Shares Outstanding";
                gridBand.Columns["SharesOutstanding"].Header.Column.Width = 150;

                gridBand.Columns["IsCurrencyFuture"].Header.Caption = "Currency Future";
                gridBand.Columns["IsCurrencyFuture"].CellActivation = Activation.NoEdit;

                gridBand.Columns["RequestedSymbology"].Hidden = true;

                gridBand.Columns["RequestedSymbol"].Hidden = true;

                gridBand.Columns["PrimarySymbology"].Hidden = true;

                gridBand.Columns["UseUDAFromUnderlyingOrRoot"].Hidden = true;
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

        private void CreateTickerAndDescription(SecMasterUIObj secMasterUiObj)
        {
            AssetCategory assetCategory = (AssetCategory)secMasterUiObj.AssetID;
            if ((secMasterUiObj.LeadCurrencyID != int.MinValue && secMasterUiObj.VsCurrencyID != int.MinValue) && (secMasterUiObj.LeadCurrencyID != secMasterUiObj.VsCurrencyID) && secMasterUiObj.CurrencyID != int.MinValue)
            {
                string leadCurrency = CachedDataManager.GetInstance.GetCurrencyText(secMasterUiObj.LeadCurrencyID);
                string vsCurrency = CachedDataManager.GetInstance.GetCurrencyText(secMasterUiObj.VsCurrencyID);
                string dateFormat = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FXTickerSymbolDateFormat).ToString();
                string bbgDateFormat = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_FXBloombergSymbolDateFormat).ToString();
                if (assetCategory == AssetCategory.FX)
                {
                    secMasterUiObj.TickerSymbol = string.Empty;
                    if (_pricingSource != PranaPricingSource.Bloomberg)
                    {
                        secMasterUiObj.TickerSymbol = leadCurrency + "/" + vsCurrency;
                    }
                    else
                    {
                        secMasterUiObj.TickerSymbol = leadCurrency + vsCurrency + " CURNCY";
                    }
                    secMasterUiObj.UnderLyingSymbol = secMasterUiObj.TickerSymbol;
                    secMasterUiObj.LongName = leadCurrency + " " + vsCurrency + " SPOT";
                    secMasterUiObj.BloombergSymbol = leadCurrency + vsCurrency + " CURNCY";
                }
                else if (assetCategory == AssetCategory.FXForward)
                {
                    if (_pricingSource != PranaPricingSource.Bloomberg)
                    {
                        if (!string.IsNullOrWhiteSpace(dateFormat))
                            secMasterUiObj.TickerSymbol = leadCurrency + "/" + vsCurrency + " " + secMasterUiObj.ExpirationDate.ToString(dateFormat).ToUpper();
                        else
                            secMasterUiObj.TickerSymbol = leadCurrency + "/" + vsCurrency + " " + secMasterUiObj.ExpirationDate.ToString("MM/dd/yyyy").ToUpper();
                    }
                    else
                    {
                        secMasterUiObj.TickerSymbol = leadCurrency + "/" + vsCurrency + " N " + secMasterUiObj.ExpirationDate.ToString("dd/MM/yy").ToUpper() + " CURNCY";
                    }
                    secMasterUiObj.UnderLyingSymbol = leadCurrency + "/" + vsCurrency;
                    secMasterUiObj.LongName = leadCurrency + " " + vsCurrency + " " + secMasterUiObj.ExpirationDate.ToString("MMMM dd yyyy").ToUpper() + " FORWARD";

                    if (!string.IsNullOrWhiteSpace(dateFormat))
                        secMasterUiObj.BloombergSymbol = leadCurrency + "/" + vsCurrency + " N " + secMasterUiObj.ExpirationDate.ToString(bbgDateFormat).ToUpper() + " CURNCY";
                    else
                        secMasterUiObj.BloombergSymbol = leadCurrency + "/" + vsCurrency + " N " + secMasterUiObj.ExpirationDate.ToString("dd/MM/yy").ToUpper() + " CURNCY";
                }

                if (secMasterUiObj.SymbolType == BusinessObjects.AppConstants.SymbolType.New)
                {
                    if (secMasterUiObj.DynamicUDA.ContainsKey("Issuer"))
                        secMasterUiObj.DynamicUDA["Issuer"] = GetIssuerValueFromUnderlying(secMasterUiObj);
                    else
                        secMasterUiObj.DynamicUDA.Add("Issuer", GetIssuerValueFromUnderlying(secMasterUiObj));
                }
            }
            else
            {
                if (assetCategory == AssetCategory.FXForward || assetCategory == AssetCategory.FX)
                {
                    secMasterUiObj.TickerSymbol = string.Empty;
                    secMasterUiObj.LongName = string.Empty;
                    secMasterUiObj.UnderLyingSymbol = string.Empty;
                    secMasterUiObj.BloombergSymbol = string.Empty;
                }
            }
        }

        private List<string> GetProperties(Type type)
        {
            List<string> listProperties = new List<string>();
            PropertyInfo[] propInfo = type.GetProperties();

            foreach (PropertyInfo prop in propInfo)
            {
                listProperties.Add(prop.Name);

            }
            return listProperties;
        }

        private void SetGridBasedOnAsset(UltraGridRow row)
        {

            try
            {
                row.Cells[OrderFields.PROPERTY_TICKERSYMBOL].Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

                int assetID = int.Parse(row.Cells[OrderFields.PROPERTY_ASSET_ID].Value.ToString());
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)assetID);

                gbSymbology.Location = new Point(3, 72);
                gbFixedIncome.Visible = false;
                gbfx.Visible = true;
                gbOptions.Visible = false;
                gbFuture.Visible = false;
                gbSymbology.Height = 130;
                lblExpirationDateTemplate.Visible = true;
                ugcpExpirationDate.Visible = true;
                lblFixedDateTemplate.Visible = false;
                ugcpFixedDate.Visible = false;

                lblExpirationDateTemplate.Location = new Point(595, 100);
                ugcpExpirationDate.Location = new Point(722, 95);
                gbSymbology.Controls.Add(lblExpirationDateTemplate);
                gbSymbology.Controls.Add(ugcpExpirationDate);

                row.Cells["ExpirationDate"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

                List<string> _dynamicColumnList = GetDynamicColumnList();
                switch (baseAssetCategory)
                {
                    case AssetCategory.PrivateEquity:
                    case AssetCategory.CreditDefaultSwap:
                    case AssetCategory.Equity:
                        if (_equityColumns == null)
                        {
                            _equityColumns = GetProperties(typeof(SecMasterEquityObj));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_equityColumns.Contains(x)) _equityColumns.Add(x); });
                        DisableColumns(row, _equityColumns);

                        gbSymbology.Height = 210;
                        gbfx.Visible = false;
                        break;

                    case AssetCategory.Option:
                        if (_optionColumns == null)
                        {
                            _optionColumns = GetProperties(typeof(SecMasterOptObj));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_optionColumns.Contains(x)) _optionColumns.Add(x); });
                        DisableColumns(row, _optionColumns);

                        gbOptions.Visible = true;
                        gbfx.Visible = false;
                        break;

                    case AssetCategory.FXOption:
                        if (_optionColumns == null)
                        {
                            _optionColumns = GetProperties(typeof(SecMasterOptObj));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_optionColumns.Contains(x)) _optionColumns.Add(x); });
                        DisableColumns(row, _optionColumns);
                        row.Cells["Multiplier"].Activation = Activation.NoEdit;

                        gbOptions.Visible = true;
                        gbfx.Visible = false;
                        break;

                    case AssetCategory.Future:

                        if (assetID.Equals((int)(AssetCategory.FXForward)))
                        {
                            if (_fxForwardColumns == null)
                            {
                                _fxForwardColumns = GetProperties(typeof(SecMasterFXForwardObj));
                            }
                            _dynamicColumnList.ForEach(x => { if (!_fxForwardColumns.Contains(x)) _fxForwardColumns.Add(x); });
                            DisableColumns(row, _fxForwardColumns);
                            row.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Activation = Activation.ActivateOnly;
                            row.Cells[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Activation = Activation.ActivateOnly;
                            gbfx.Visible = true;
                            gbfx.Location = new Point(3, 72);
                            gbSymbology.Location = new Point(3, 143);

                            lblExpirationDateTemplate.Location = new Point(461, 37);
                            ugcpExpirationDate.Location = new Point(555, 32);
                            gbfx.Controls.Add(ugcpExpirationDate);
                            gbfx.Controls.Add(lblExpirationDateTemplate);

                            lblFixedDateTemplate.Visible = true;
                            ugcpFixedDate.Visible = true;
                        }

                        else
                        {
                            if (_futureColumns == null)
                            {
                                _futureColumns = GetProperties(typeof(SecMasterFutObj));
                            }
                            _dynamicColumnList.ForEach(x => { if (!_futureColumns.Contains(x)) _futureColumns.Add(x); });
                            DisableColumns(row, _futureColumns);
                            row.Cells["CutOffTime"].Activation = Activation.Disabled;

                            gbFuture.Visible = true;
                            gbfx.Visible = false;
                        }

                        break;
                    case AssetCategory.FX:
                        if (_fxColumns == null)
                        {
                            _fxColumns = GetProperties(typeof(SecMasterFxObj));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_fxColumns.Contains(x)) _fxColumns.Add(x); });
                        DisableColumns(row, _fxColumns);
                        row.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Activation = Activation.ActivateOnly;
                        row.Cells[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Activation = Activation.ActivateOnly;
                        gbfx.Visible = true;
                        gbfx.Location = new Point(3, 72); ;
                        gbSymbology.Location = new Point(3, 143);
                        //modified by amit. changes for http://jira.nirvanasolutions.com:8080/browse/PRANA-8614
                        lblExpirationDateTemplate.Visible = false;
                        ugcpExpirationDate.Visible = false;
                        ultraLabel3.Visible = false;
                        ultraGridCellProxy4.Visible = false;
                        break;

                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        if (_fixedIncomeColumns == null)
                        {
                            _fixedIncomeColumns = GetProperties(typeof(SecMasterFixedIncome));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_fixedIncomeColumns.Contains(x)) _fixedIncomeColumns.Add(x); });
                        DisableColumns(row, _fixedIncomeColumns);

                        row.Cells["ExpirationDate"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

                        gbFixedIncome.Visible = true;
                        gbfx.Visible = false;
                        break;

                    case AssetCategory.Indices:
                        if (_indexColumns == null)
                        {
                            _indexColumns = GetProperties(typeof(SecMasterIndexObj));
                        }
                        _dynamicColumnList.ForEach(x => { if (!_indexColumns.Contains(x)) _indexColumns.Add(x); });
                        DisableColumns(row, _indexColumns);

                        gbSymbology.Height = 210;
                        gbfx.Visible = false;
                        break;
                    default:

                        foreach (UltraGridColumn colvar in row.Band.Columns)
                        {
                            colvar.CellActivation = Activation.AllowEdit;
                        }

                        row.Cells["CreationDate"].Activation = Activation.NoEdit;
                        row.Cells["ModifiedDate"].Activation = Activation.NoEdit;

                        gbSymbology.Height = 210;

                        break;
                }
                if (Boolean.Parse(row.Cells["UseUDAFromUnderlyingOrRoot"].Value.ToString()) == false)
                {
                    row.Cells["UDAAssetClassID"].Activation = Activation.AllowEdit;
                    row.Cells["UDASectorID"].Activation = Activation.AllowEdit;
                    row.Cells["UDASubSectorID"].Activation = Activation.AllowEdit;
                    row.Cells["UDACountryID"].Activation = Activation.AllowEdit;
                    row.Cells["UDASecurityTypeID"].Activation = Activation.AllowEdit;
                    EnableDisableDynamicUDARows(true, row);
                }
                else
                {
                    row.Cells["UDAAssetClassID"].Activation = Activation.Disabled;
                    row.Cells["UDASectorID"].Activation = Activation.Disabled;
                    row.Cells["UDASubSectorID"].Activation = Activation.Disabled;
                    row.Cells["UDACountryID"].Activation = Activation.Disabled;
                    row.Cells["UDASecurityTypeID"].Activation = Activation.Disabled;
                    EnableDisableDynamicUDARows(false, row);
                }
                SetUpUIForFXAndFWD(baseAssetCategory, assetID, row);

                //Set the Allow Camelcase checkbox as per the value of Activ Symbol
                SetAllowCamelCaseCheckBox(row.Cells["ActivSymbol"].Value.ToString());
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

        private void SetUpUIForFXAndFWD(AssetCategory baseAssetCategory, int assetID, UltraGridRow row)
        {
            try
            {
                if (baseAssetCategory == AssetCategory.FX || assetID == (int)AssetCategory.FXForward)
                {
                    lblLeadCurrencyValue.Text = int.Parse(row.Cells[OrderFields.PROPERTY_LEADCURRENCYID].Value.ToString()) != int.MinValue ? CachedDataManager.GetInstance.GetCurrencyText(int.Parse(row.Cells[OrderFields.PROPERTY_LEADCURRENCYID].Value.ToString())) : CachedDataManager.GetInstance.GetCurrencyText(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID());
                    lblVSCurrencyValue.Text = row.Cells[OrderFields.PROPERTY_CURRENCYID].Text;
                    if (row.Cells.Exists("RiskCurrency"))
                        row.Cells["RiskCurrency"].Activation = Activation.Disabled;

                    this.lblCurrency.Location = new Point(10, 30);
                    this.lblCurrency.Margin = new Padding(5, 3, 5, 3);
                    this.lblCurrency.Size = new Size(88, 18);
                    this.lblCurrency.TabIndex = 87;

                    this.ugcpCurrencyID.Location = new Point(155, 24);
                    this.ugcpCurrencyID.Margin = new Padding(5, 3, 5, 3);
                    this.ugcpCurrencyID.Size = new Size(125, 24);
                    this.ugcpCurrencyID.TabIndex = 4;

                    this.lblTickerSymbol.Location = new Point(344, 30);
                    this.lblTickerSymbol.Margin = new Padding(5, 3, 5, 3);
                    this.lblTickerSymbol.Size = new Size(61, 18);
                    this.lblTickerSymbol.TabIndex = 111;

                    this.ugcpTickerSymbol.Location = new Point(435, 24);
                    this.ugcpTickerSymbol.Margin = new Padding(5, 3, 5, 3);
                    this.ugcpTickerSymbol.Size = new Size(125, 24);
                    this.ugcpTickerSymbol.TabIndex = 5;
                }
                else
                {
                    row.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Activation = Activation.AllowEdit;
                    row.Cells[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Activation = Activation.AllowEdit;

                    this.lblTickerSymbol.Location = new Point(10, 30);
                    this.lblTickerSymbol.Margin = new Padding(5, 3, 5, 3);
                    this.lblTickerSymbol.Size = new Size(88, 18);
                    this.lblTickerSymbol.TabIndex = 87;

                    this.ugcpTickerSymbol.Location = new Point(155, 24);
                    this.ugcpTickerSymbol.Margin = new Padding(5, 3, 5, 3);
                    this.ugcpTickerSymbol.Size = new Size(125, 24);
                    this.ugcpTickerSymbol.TabIndex = 4;
                    this.ugcpTickerSymbol.Appearance.TextHAlign = HAlign.Left;

                    this.lblCurrency.Location = new Point(315, 30);
                    this.lblCurrency.Margin = new Padding(5, 3, 5, 3);
                    this.lblCurrency.Size = new Size(61, 18);
                    this.lblCurrency.TabIndex = 111;

                    this.ugcpCurrencyID.Location = new Point(445, 24);
                    this.ugcpCurrencyID.Margin = new Padding(5, 3, 5, 3);
                    this.ugcpCurrencyID.Size = new Size(125, 24);
                    this.ugcpCurrencyID.TabIndex = 5;
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

        private void FxColumnSettings(UltraGridRow row)
        {
            try
            {
                int assetID = int.Parse(row.Cells[OrderFields.PROPERTY_ASSET_ID].Value.ToString());
                AssetCategory assetCategory = (AssetCategory)assetID;
                UltraGridColumn colExpSett = grdData.DisplayLayout.Bands[0].Columns["ExpirationDate"];
                colExpSett.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                colExpSett.Hidden = false;

                if (assetCategory == AssetCategory.FX)
                {

                    colExpSett.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colExpSett.Hidden = true;
                }
                else if (assetCategory == AssetCategory.FXForward)
                {
                    colExpSett.Header.VisiblePosition = 7;
                    colExpSett.Hidden = false;

                    UltraGridColumn colUnderlyingSymbol = grdData.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_UNDERLYINGSYMBOL];
                    colUnderlyingSymbol.Header.VisiblePosition = 8;
                    colUnderlyingSymbol.Hidden = false;
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

        private ValueList GetValueList(Dictionary<int, string> values)
        {
            ValueList list = new ValueList();
            list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> value in values)
            {
                list.ValueListItems.Add(value.Key, value.Value);
            }
            return list;
        }

        private ValueList GetValueList(Dictionary<string, string> values)
        {
            ValueList list = new ValueList();
            try
            {
                list.ValueListItems.Add(int.MinValue, "Undefined");
                foreach (string key in values.Keys)
                {
                    list.ValueListItems.Add(values[key]);
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
            return list;
        }

        public void GetDataForSymbol(string symbol)
        {
            try
            {
                _isAdvncdSearching = false;
                _advncdSearchQuery = string.Empty;
                if (!_securityMaster.IsConnected)
                {
                    _securityMaster.ConnectToServer();
                    return;
                }
                symbolLookupRequestObject = new SymbolLookupRequestObject();
                string input = symbol.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    symbolLookupRequestObject.TickerSymbol = input;
                }
                txtbxInput.Text = symbol;
                cmbbxSearchCriteria.Value = SecMasterConstants.SearchCriteria.Ticker;
                cmbbxMatchOn.Value = SecMasterConstants.SearchMatchOn.Exact;
                _isClearDataOnSearch = true;
                SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbbxSearchCriteria.Value.ToString());
                SearchSecurityInCacheThenDB(searchCriteria);
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

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                // Before getting data check if there are any changes on grid, and show message to save
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9552

                grdData.Enabled = false;
                int unsavedSymbolsCount = 0;
                Parallel.ForEach(_secMasterUIobj, x => { if (x.SymbolType != BusinessObjects.AppConstants.SymbolType.Unchanged) unsavedSymbolsCount++; });
                if (unsavedSymbolsCount > 0)
                {
                    DialogResult userChoice = MessageBox.Show(this, "Would you like to save Security changes?", "Nirvana", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (userChoice == DialogResult.Yes)
                    {
                        this.btnSave_Click(null, null);
                        btnSave.Focus();
                        return;
                    }
                    else if (userChoice == DialogResult.No)
                        btnGetData.Focus();
                    else if (userChoice == DialogResult.Cancel)
                        return;
                }

                // If user add a row but doesn't save it, then on get data set _isAddedRow to false
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7970
                _isAddedRow = false;
                _isAdvncdSearching = false;
                _advncdSearchQuery = string.Empty;
                if (!_securityMaster.IsConnected)
                {
                    _securityMaster.ConnectToServer();
                    return;
                }
                String requestID = System.Guid.NewGuid().ToString();
                if (rdBtnSearchSymbols.Checked)
                {
                    //Modified by omshiv, if exact search then search in Cache and then DB other wise search in DB only
                    SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbbxSearchCriteria.Value.ToString());

                    if (cmbbxMatchOn.Value.ToString().Equals(SecMasterConstants.SearchMatchOn.Exact.ToString())
                    && searchCriteria != SecMasterConstants.SearchCriteria.CompanyName && searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol
                    && !chkbxUnApprovedSec.Checked)
                    {
                        SearchSecurityInCacheThenDB(searchCriteria);
                    }
                    else
                    {
                        //Added to get first exact data then contains data, PRANA-12692
                        //To do: need to implement better search algorithm to get data according to search criteria
                        if (pageIndex == 0)
                        {
                            if (_isClearDataOnSearch)
                                _secMasterUIobj.Clear();
                            if (searchCriteria != SecMasterConstants.SearchCriteria.CompanyName && searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol && !chkbxUnApprovedSec.Checked)
                                SearchSecurityInCacheThenDB(searchCriteria);
                            CreateReqObjAndSendReqToServer(null);
                        }
                        else
                            CreateReqObjAndSendReqToServer(null);
                    }
                }
                else if (rdBtnOpenSymbols.Checked || rdBtnHistSymbols.Checked)
                {
                    btnNext.Visible = false;
                    btnPrev.Visible = false;
                    _secMasterUIobj.Clear();
                    SetGetButtonDetails("Getting data ..", false);
                    _isSearchInProcess = true;
                    //if  rdBtnOpenSymbols radio button selected then fetch All open symbols
                    if (rdBtnOpenSymbols.Checked)
                    {
                        _securityMaster.DataReqByKeyFrmServer(SecMasterConstants.CONST_AllOpenSymbolsReq, requestID);
                    }

                    //if  rdBtnOpenSymbols radio button selected then fetch All historicals symbols
                    else if (rdBtnHistSymbols.Checked)
                    {
                        _securityMaster.DataReqByKeyFrmServer(SecMasterConstants.CONST_AllHistTradedSymbolsReq, requestID);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please, Select Search Criteria!", "Nirvana Security Master", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// Search Security In Cache Then DB for exact search
        /// </summary>
        /// <param name="searchCriteria"></param>
        private void SearchSecurityInCacheThenDB(SecMasterConstants.SearchCriteria searchCriteria)
        {
            try
            {
                string input = txtbxInput.Text.Trim();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3250
                //Only Ticker was being Selected as symbology as Symbol was not Appended So appended that manually.
                StringBuilder symbologyToSelect = new StringBuilder();
                symbologyToSelect.Append(searchCriteria.ToString());
                // clear UI cache before sending search request to server, PRANA-17016
                if (_isClearDataOnSearch)
                    _secMasterUIobj.Clear();
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                // it search in local only,not sent request to BB or esignal
                reqObj.IsSearchInLocalOnly = true;
                // create request based on BBGID or Symbology
                if (searchCriteria == SecMasterConstants.SearchCriteria.BBGID)
                {
                    reqObj.AddData(input);
                }
                else
                {
                    ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                    symbology = (ApplicationConstants.SymbologyCodes)SecMasterHelper.GetSymbology(symbologyToSelect.ToString());

                    reqObj.AddData(input, symbology);
                }
                reqObj.HashCode = this.GetHashCode();
                reqObj.RequestID = System.Guid.NewGuid().ToString();
                _securityMaster.SendRequest(reqObj);

                btnNext.Visible = false;
                btnPrev.Visible = false;
                _isLocalDBSearchInProcess = true;
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
        /// creating SM reqest object and send to sever to fetch data. 
        /// </summary>
        private void CreateReqObjAndSendReqToServer(SymbolLookupRequestObject symbolLookUpReqObj)
        {
            try
            {
                pageIndex = 0;
                Boolean isProcessToSearch = false;
                if (symbolLookUpReqObj != null)
                {
                    isProcessToSearch = true;
                    chkbxUnApprovedSec.Checked = false;
                }
                else
                {
                    symbolLookupRequestObject = new SymbolLookupRequestObject();
                    string input = txtbxInput.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(input) && _isAlreadyRequested == false)
                    {
                        isProcessToSearch = true;
                        SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbbxSearchCriteria.Value.ToString());

                        input = GetCompleteText(input);
                        switch (searchCriteria)
                        {
                            case SecMasterConstants.SearchCriteria.Ticker:
                                symbolLookupRequestObject.TickerSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.Bloomberg:
                                symbolLookupRequestObject.BloombergSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.CUSIP:
                                symbolLookupRequestObject.CUSIPSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.ISIN:
                                symbolLookupRequestObject.ISINSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.SEDOL:
                                symbolLookupRequestObject.SEDOLSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.ReutersSymbol:
                                symbolLookupRequestObject.ReutersSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.OSIOption:
                                symbolLookupRequestObject.OSIOptionSymbol = input;
                                break;
                            case SecMasterConstants.SearchCriteria.IDCOOption:
                                symbolLookupRequestObject.IDCOOptionSymbol = input;
                                break;

                            case SecMasterConstants.SearchCriteria.OPRAOption:
                                symbolLookupRequestObject.OPRAOptionSymbol = input;
                                break;

                            case SecMasterConstants.SearchCriteria.CompanyName:
                                symbolLookupRequestObject.Name = GetCompleteText(input);
                                break;

                            case SecMasterConstants.SearchCriteria.UnderlyingSymbol:
                                symbolLookupRequestObject.Underlying = GetCompleteText(input); ;
                                break;

                            case SecMasterConstants.SearchCriteria.BBGID:
                                symbolLookupRequestObject.BBGID = GetCompleteText(input); ;
                                break;

                            case SecMasterConstants.SearchCriteria.ActivSymbol:
                                symbolLookupRequestObject.ActivSymbol = input;
                                break;

                            case SecMasterConstants.SearchCriteria.FactSetSymbol:
                                symbolLookupRequestObject.FactSetSymbol = input;
                                break;
                        }
                    }
                }

                //set search unapproved securities or approved
                if (chkbxUnApprovedSec.Checked)
                {
                    isProcessToSearch = true;
                    symbolLookupRequestObject.IsSecApproved = false;
                }
                //set is full scan or quick scan
                if (chkBxIsFullSearch.Checked)
                    symbolLookupRequestObject.IsFullScan = true;

                else
                    symbolLookupRequestObject.IsFullScan = false;

                //if search parameter found the send request for fetch data 
                if (isProcessToSearch)
                {
                    //Changing the value true to false because it was creating the problem in case of search with contains and click on a checkbox of security information UI
                    _isLocalDBSearchInProcess = false;
                    RequestData();
                    _secMasterUIobjOld.Clear();
                    _secMasterUIobjOld.AddRange(_secMasterUIobj);
                }
                else
                {
                    //set mesaage to set parameters 
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;

                }
                //clear data from grid if "auto clear data on search" is checked
                if (_isClearDataOnSearch)
                {
                    _secMasterUIobj.Clear();
                }

                _isAddedRow = false;
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

        void txtbxSymbol_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (this.txtbxInput.Focused && e.KeyChar == '\r')
            {
                // click the Filter button
                this.btnGetData_Click(null, null);
                // don't allow the Enter key to pass to textbox
                e.Handled = true;
            }
        }

        private void grdData_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
        }

        private void SymbolLookUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PluggableToolsClosed != null)
            {
                PluggableToolsClosed(this, EventArgs.Empty);
            }
            InstanceManager.ReleaseInstance(typeof(SymbolLookUp));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SecMasterbaseList lst = new SecMasterbaseList();
                lst.RequestID = System.Guid.NewGuid().ToString();
                foreach (SecMasterUIObj uiObj in _secMasterUIobj)
                {

                    if (uiObj.SymbolType != BusinessObjects.AppConstants.SymbolType.Unchanged)
                    {
                        uiObj.AUECID = CachedDataManager.GetInstance.GetAUECID(uiObj.AssetID, uiObj.UnderLyingID, uiObj.ExchangeID);

                        if (uiObj.AUECID == int.MinValue)
                        {
                            MessageBox.Show(SecMasterConstants.MSG_NotValidAUECError, "Error", MessageBoxButtons.OK);
                            return;
                        }
                        if (!ValidateSymbolForSave(uiObj))
                        {
                            return;
                        }
                        SecMasterBaseObj secMasterBaseObj = GetSecMasterObjFromUIObject(uiObj);
                        if (secMasterBaseObj != null)
                        {
                            secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.SymbolLookup;
                            //TODO - add modified date - omshiv
                            //Modified by Bhavana - set ModifiedBy and ModifiedDate in case of updation
                            if (uiObj.SymbolType == BusinessObjects.AppConstants.SymbolType.Updated)
                            {
                                secMasterBaseObj.ModifiedBy = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                                secMasterBaseObj.ModifiedDate = DateTime.Now;
                            }
                            lst.Add(secMasterBaseObj);

                            // update symbol type to unchanged for new symbol after it is saved sucessfully, PRANA-9269
                            if (uiObj.SymbolType != SymbolType.New)
                                uiObj.SymbolType = BusinessObjects.AppConstants.SymbolType.Unchanged;
                        }
                    }
                }

                toolStripStatusLabel1.Text = string.Empty;
                if (lst.Count > 0)
                {
                    // disable add security tool button while sm data save in process, PRANA-9269
                    toolBarManager.Tools[CONST_ADD_SECURITY].SharedProps.Enabled = false;
                    _securityMaster.SaveNewSymbols(lst);

                    _isAddedRow = false;

                }
                else
                {
                    lblStatus.Text = toolStripStatusLabel1.Text = SecMasterConstants.MSG_NoDataToSave;
                }

                if (_isUpdateRequestFromTradingTicket)
                {
                    _isUpdateRequestFromTradingTicket = false;
                    this.Close();
                }
                this.grdRowEditTemplate.Close(false);
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

        private SecMasterBaseObj GetSecMasterObjFromUIObject(SecMasterUIObj uiObj)
        {
            SecMasterBaseObj secMasterBaseObj = null;
            try
            {
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);

                switch (baseAssetCategory)
                {
                    case AssetCategory.Equity:
                    case AssetCategory.PrivateEquity:
                    case AssetCategory.CreditDefaultSwap:
                        secMasterBaseObj = new SecMasterEquityObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Option:
                    case AssetCategory.FXOption:
                        secMasterBaseObj = new SecMasterOptObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Future:
                        if ((AssetCategory)uiObj.AssetID == AssetCategory.FXForward)
                        {
                            secMasterBaseObj = new SecMasterFXForwardObj();
                            secMasterBaseObj.FillUIData(uiObj);
                        }
                        else
                        {
                            secMasterBaseObj = new SecMasterFutObj();
                            secMasterBaseObj.FillUIData(uiObj);
                        }
                        break;
                    case AssetCategory.FX:
                        secMasterBaseObj = new SecMasterFxObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Indices:
                        secMasterBaseObj = new SecMasterIndexObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        secMasterBaseObj = new SecMasterFixedIncome();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
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
            return secMasterBaseObj;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdData.ActiveRow != null)
            {
                grdData.ActiveRow.ShowEditTemplate();
            }
        }

        /// <summary>
        /// Handle add symbol flow when user click on "add security"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewSymbolToSM("");
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
        /// Handle on adding security from external UI
        /// </summary>
        /// <param name="secMasterUIObj"></param>
        private void AddNewSymbolToSM(SecMasterUIObj secMasterUIObj)
        {
            try
            {
                _isAddedRow = true;
                txtbxInput.Text = string.Empty;

                ValidAUECs validauecs = ValidAUECs.GetInstance();
                if (secMasterUIObj.TickerSymbol.IndexOf('-') > 0)
                {
                    String exchange = secMasterUIObj.TickerSymbol.Substring((secMasterUIObj.TickerSymbol.IndexOf('-')) + 1);
                    validauecs.SearchAUECOnLoad(exchange);
                }

                validauecs.ShowDialog();

                ValidAUEC loadauec = validauecs._selectedAUEC;
                SetValuesForNewSecuirty(secMasterUIObj, loadauec);
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
        /// Handle Add new symbol request. select AUEC,  then fill values and save.
        /// </summary>
        /// <param name="RequestedSymbolToAdd"></param>
        private void AddNewSymbolToSM(string RequestedSymbolToAdd)
        {
            try
            {
                _isAddedRow = true;
                txtbxInput.Text = string.Empty;
                SecMasterUIObj secMasterUIObj = new SecMasterUIObj();
                ValidAUECs validauecs = ValidAUECs.GetInstance();
                validauecs.ShowDialog();
                ValidAUEC loadauec = validauecs._selectedAUEC;

                if (!string.IsNullOrWhiteSpace(RequestedSymbolToAdd))
                    secMasterUIObj.TickerSymbol = RequestedSymbolToAdd;

                //condition added so that no new row is added and popup appear when AUEC is not selected, PRANA-10573
                if (loadauec != null && loadauec.AssetID != int.MinValue)
                {
                    SetValuesForNewSecuirty(secMasterUIObj, loadauec);
                    validauecs._selectedAUEC = null;
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

        private void SetValuesForNewSecuirty(SecMasterUIObj secMasterUIObj, ValidAUEC loadauec)
        {
            try
            {
                secMasterUIObj.Symbol_PK = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                secMasterUIObj.SymbolType = BusinessObjects.AppConstants.SymbolType.New;

                //On adding new security manually - by default it will be approved - om (Ashish)
                //Now we setting it based on config
                secMasterUIObj.IsSecApproved = _isSecurityAutoApproved;

                if (_isSecurityAutoApproved)
                {
                    secMasterUIObj.ApprovalDate = DateTime.Now;
                    secMasterUIObj.ApprovedBy = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                }
                secMasterUIObj.CreatedBy = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;

                //setting source of sec Master data.
                secMasterUIObj.DataSource = SecMasterConstants.SecMasterSourceOfData.SymbolLookup;

                if (loadauec != null)
                {
                    secMasterUIObj.UnderLyingID = loadauec.UnderlyingID;
                    secMasterUIObj.ExchangeID = loadauec.ExchangeID;
                    secMasterUIObj.CurrencyID = loadauec.CurrencyID;
                    secMasterUIObj.Multiplier = loadauec.Multiplier;
                    secMasterUIObj.RoundLot = loadauec.RoundLot;

                    if (loadauec.AssetID == (int)AssetCategory.FixedIncome || loadauec.AssetID == (int)AssetCategory.ConvertibleBond)
                    {
                        secMasterUIObj.Multiplier = 0.01;
                        secMasterUIObj.CouponFrequencyID = (int)(CouponFrequency.None);
                    }

                    // Set default round lot for FXForward and FXSpot as "0.00000001", PRANA-10834
                    if (loadauec.AssetID == (int)AssetCategory.FXForward || loadauec.AssetID == (int)AssetCategory.FX)
                    {
                        secMasterUIObj.VsCurrencyID = secMasterUIObj.CurrencyID;
                    }
                }
                if (_secMasterUIobj != null && !_isClearDataOnSearch)
                {
                    _secMasterUIobj.Insert(0, secMasterUIObj);
                    // reload grid rows so that newly added row comes at top, PRANA-13832
                    grdData.DataSource = null;
                    grdData.DataSource = _secMasterUIobj;
                }
                else
                {
                    _secMasterUIobj.Clear();
                    _secMasterUIobj.Add(secMasterUIObj);
                }

                //Commented as grid is already bind to datasource, PRANA-12724
                //grdData.DataSource = _secMasterUIobj;
                //grdData.DataBind();
                InitGridRowEditTemplate();
                grdData.Rows[0].Activate();
                grdData.ActiveRow.Cells["CreationDate"].Value = DateTime.Now;
                grdData.ActiveRow.Cells["ModifiedDate"].Value = DateTime.Now;

                //added to clear all filter in grid's columns, PRANA-9932
                grdData.DisplayLayout.Bands[0].Override.RowFilterMode = RowFilterMode.AllRowsInBand;
                grdData.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                if (loadauec != null && loadauec.AssetID != int.MinValue)
                {
                    grdData.ActiveRow.Cells["AssetID"].Value = loadauec.AssetID;
                    grdData.ActiveRow.Cells["TickerSymbol"].Activate();
                }
                else
                {
                    grdData.ActiveRow.Cells["AssetID"].Activate();
                }
                CreateTickerAndDescription(secMasterUIObj);

                // Update UDA User Asset automatically to Asset Class if it is undefined
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7970
                if (grdData.ActiveRow.Cells["UDAAssetClassID"].Text == "Undefined")
                    grdData.ActiveRow.Cells["UDAAssetClassID"].Value = GetValueFromText(grdData.ActiveRow.Cells["UDAAssetClassID"].ValueListResolved, grdData.ActiveRow.Cells["AssetID"].Text, grdData.ActiveRow.Cells["PutOrCall"].Text);

                // Update Risk Currency and Issuer automatically
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9813
                if (grdData.DisplayLayout.Bands[0].Columns.Exists("RiskCurrency"))
                    grdData.ActiveRow.Cells["RiskCurrency"].Value = GetRiskCurrency(secMasterUIObj);

                if (grdData.DisplayLayout.Bands[0].Columns.Exists("Issuer"))
                    grdData.ActiveRow.Cells["Issuer"].Value = GetIssuerValueFromUnderlying(secMasterUIObj);

                grdData.PerformAction(UltraGridAction.EnterEditMode);
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
        /// On load UI bind security master events and set default UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbolLookUp_Load(object sender, EventArgs e)
        {
            _securityMaster.SymbolLookUpDataResponse += new EventHandler<EventArgs<DataSet>>(_securityMaster_SymbolLookUpDataResponse);
            try
            {
                _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                _securityMaster.Disconnected += new EventHandler(_securityMaster_Disconnected);
                _securityMaster.Connected += new EventHandler(_securityMaster_Connected);
                _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                _securityMaster.EventTradedSMDataUIRes += new EventHandler<EventArgs<List<SecMasterBaseObj>>>(_securityMaster_EventTradedSMDataUIRes);
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                _securityMaster.FutSymbolRootDataRes += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_FutSymbolRootDataRes);

                _securityMaster.FutRootDataSaveRes += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_FutRootDataSaveRes);
                string histPricing = CachedDataManager.GetInstance.GetPranaPreferenceByKey("PricingSource");
                Enum.TryParse<PranaPricingSource>(histPricing, true, out _pricingSource);

                if (_securityMaster != null)
                {
                    _securityMaster.GetAllUDAAtrributes();

                }
                if (!_securityMaster.IsConnected)
                {
                    SetGetButtonDetails("Connect", true);
                    toolStripStatusLabel1.Text = "Trade Server : Disconnected";
                }
                this.ActiveControl = txtbxInput;
                txtbxInput.Focus();
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.LaunchPricingInput += SymbolLookup_LaunchPricingInput;
                }
                _isLayoutChanged = false;

                _isSecurityAutoApproved = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SecurityAutoApproved").ToString());
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

        private void _securityMaster_FutRootDataSaveRes(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (futureRootSymUI != null)
                    futureRootSymUI.AfterDataSaved(e.Value.Message.ToString());
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

        private void SetButtonsColor()
        {
            try
            {
                btnSelectAUEC.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSelectAUEC.ForeColor = System.Drawing.Color.White;
                btnSelectAUEC.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSelectAUEC.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSelectAUEC.UseAppStyling = false;
                btnSelectAUEC.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnEditRootData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnEditRootData.ForeColor = System.Drawing.Color.White;
                btnEditRootData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnEditRootData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnEditRootData.UseAppStyling = false;
                btnEditRootData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPreTab.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPreTab.ForeColor = System.Drawing.Color.White;
                btnPreTab.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPreTab.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPreTab.UseAppStyling = false;
                btnPreTab.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnEditSecurity.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnEditSecurity.ForeColor = System.Drawing.Color.White;
                btnEditSecurity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnEditSecurity.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnEditSecurity.UseAppStyling = false;
                btnEditSecurity.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNextTab.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnNextTab.ForeColor = System.Drawing.Color.White;
                btnNextTab.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNextTab.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNextTab.UseAppStyling = false;
                btnNextTab.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnOK.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOK.ForeColor = System.Drawing.Color.White;
                btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOK.UseAppStyling = false;
                btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnUDAUI.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnUDAUI.ForeColor = System.Drawing.Color.White;
                btnUDAUI.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnUDAUI.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnUDAUI.UseAppStyling = false;
                btnUDAUI.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetData.ForeColor = System.Drawing.Color.White;
                btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetData.UseAppStyling = false;
                btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPrev.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnPrev.ForeColor = System.Drawing.Color.White;
                btnPrev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPrev.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPrev.UseAppStyling = false;
                btnPrev.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNext.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnNext.ForeColor = System.Drawing.Color.White;
                btnNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNext.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNext.UseAppStyling = false;
                btnNext.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Handle futur root Data 
        /// </summary>
        /// <param name="ds"></param>
        void _securityMaster_FutSymbolRootDataRes(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                //TODO - need analyse here kuldeep
                //if (UIValidation.GetInstance().validate(this))
                if (this.InvokeRequired)
                {
                    SMQMsgInvokeDelegate invokeDelegate = new SMQMsgInvokeDelegate(_securityMaster_FutSymbolRootDataRes);
                    this.BeginInvoke(invokeDelegate, new object[] { sender, e });
                }
                else
                {
                    FutureRootData rootData = binaryFormatter.DeSerialize(e.Value.Message.ToString()) as FutureRootData;

                    if (grdData.ActiveRow != null)
                    {
                        UltraGridRow gridRow = grdData.ActiveRow;
                        gridRow.Cells["UDASectorID"].Value = rootData.UDASectorID;
                        gridRow.Cells["UDASubSectorID"].Value = rootData.UDASubSectorID;
                        gridRow.Cells["UDASecurityTypeID"].Value = rootData.UDASecurityTypeID;
                        gridRow.Cells["UDACountryID"].Value = rootData.UDACountryID;
                        gridRow.Cells["Multiplier"].Value = rootData.Multiplier.ToString();
                        gridRow.Cells["CutOffTime"].Value = rootData.CutoffTime;
                        gridRow.Cells["IsCurrencyFuture"].Value = rootData.IsCurrencyFuture;
                        lblStatus.Text = "Future root data has been set.";
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

        void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        DelegateSecMasterRes objectHandeler = new DelegateSecMasterRes(_securityMaster_SecMstrDataResponse);
                        this.BeginInvoke(objectHandeler, new object[] { sender, e });
                    }
                    else
                    {
                        if (_isLocalDBSearchInProcess)
                        {
                            _isLocalDBSearchInProcess = false;
                            SetGetButtonDetails("Get Data", true);
                            toolStripStatusLabel1.Text = "Result found.";


                            SecMasterUIObj smUIObj = new SecMasterUIObj();
                            Transformer.CreateObjFromObjThroughReflection(secMasterObj, smUIObj);
                            SecMasterHelper.SetAssetWiseSMFileds(secMasterObj, smUIObj);
                            smUIObj.SymbolType = SymbolType.Unchanged;
                            _secMasterUIobj.Add(smUIObj);

                            if (string.Equals(this.cmbbxMatchOn.Text, SecMasterConstants.SearchMatchOn.Exact.ToString()))
                                grdData.Enabled = true;

                            InitGridRowEditTemplate();
                        }
                        else
                        {
                            // check weather to update alll uda fields or only issuer, PRANA-9838
                            if (UseUDAFromUnderlyingOrRoot)
                            {
                                if (secMasterObj.SymbolUDAData != null)
                                {
                                    UDAData udaOfUnderlying = secMasterObj.SymbolUDAData;
                                    if (grdData.ActiveRow != null)
                                    {
                                        grdData.ActiveRow.Cells["UDASectorID"].Value = udaOfUnderlying.SectorID;
                                        grdData.ActiveRow.Cells["UDASubSectorID"].Value = udaOfUnderlying.SubSectorID;
                                        grdData.ActiveRow.Cells["UDASecurityTypeID"].Value = udaOfUnderlying.SecurityTypeID;
                                        grdData.ActiveRow.Cells["UDACountryID"].Value = udaOfUnderlying.CountryID;

                                        // Update dynamic UDAs from Underlying
                                        //SerializableDictionary<string, DynamicUDA> _dynamicUDAcache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                                        UpdateDynamicUDAFromUnderlying(grdData.ActiveRow, secMasterObj.DynamicUDA);
                                    }
                                }
                            }
                            else if (_isSetIssuer)
                            {
                                if (grdData.ActiveRow != null)
                                {
                                    if (grdData.ActiveRow.Cells.Exists("Issuer") && grdData.ActiveRow.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Value != null
                                        && grdData.ActiveRow.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Value.ToString().Equals(secMasterObj.TickerSymbol))
                                        grdData.ActiveRow.Cells["Issuer"].Value = secMasterObj.LongName;

                                    _isSetIssuer = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (ObjectDisposedException ex1)
            {
                bool rethrow = Logger.HandleException(ex1, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
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
        /// Handle recieved data for hist/open trades SM data
        /// </summary>
        /// <param name="SMData"></param>
        void _securityMaster_EventTradedSMDataUIRes(object sender, EventArgs<List<SecMasterBaseObj>> e)
        {
            try
            {
                List<SecMasterBaseObj> SMData = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        AllTradedSymbolsRequest symbolLookup = new AllTradedSymbolsRequest(_securityMaster_EventTradedSMDataUIRes);
                        this.BeginInvoke(symbolLookup, new object[] { sender, e });
                    }
                    else
                    {
                        foreach (SecMasterBaseObj SMObj in SMData)
                        {
                            SecMasterUIObj smUIObj = new SecMasterUIObj();
                            Transformer.CreateObjFromObjThroughReflection(SMObj, smUIObj);
                            SecMasterHelper.SetAssetWiseSMFileds(SMObj, smUIObj);
                            smUIObj.SymbolType = SymbolType.Unchanged;
                            if (IsContain(smUIObj))
                                _secMasterUIobj.Remove(smUIObj);
                            _secMasterUIobj.Add(smUIObj);
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
        /// <summary>
        /// Handle UDA attributes response
        /// </summary>
        /// <param name="UDADataCol"></param>
        void _securityMaster_UDAAttributesResponse(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                SecMasterHelper.getInstance().SetUDAValueLists(e.Value);
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
        /// Intialize edit row template based on is adding new security or updating security
        /// </summary>
        private void InitGridRowEditTemplate()
        {
            try
            {
                this.grdData.DisplayLayout.Override.RowEditTemplateUIType = RowEditTemplateUIType.None;
                if (_isAddedRow)
                {
                    this.grdData.DisplayLayout.Override.RowEditTemplateUIType = RowEditTemplateUIType.OnDoubleClickRow;
                    this.grdData.DisplayLayout.Override.RowEditTemplateUIType |= RowEditTemplateUIType.OnEnterEditMode;
                }
                else
                {
                    this.grdData.DisplayLayout.Override.RowEditTemplateUIType = RowEditTemplateUIType.OnDoubleClickRow;
                }

                this.grdRowEditTemplate.DialogSettings.Caption = "Security Information";
                this.grdRowEditTemplate.DisplayMode = RowEditTemplateDisplayMode.Modal;
                this.grdRowEditTemplate.Visible = false;
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.ApplyTheme)
                {
                    grdRowEditTemplate.Size = new System.Drawing.Size(877, 400);
                }
                this.grdData.DisplayLayout.Bands[0].RowEditTemplate = null;
                this.grdData.DisplayLayout.Bands[0].RowEditTemplate = this.grdRowEditTemplate;

                //Instantiate an UltraGridBagLayout component.
                UltraGridBagLayoutManager gridBag = new UltraGridBagLayoutManager();
                gridBag.ContainerControl = grdRowEditTemplate;

                //These two properties will space the controls out
                //evenly in the available space.
                gridBag.ExpandToFitHeight = true;
                gridBag.ExpandToFitWidth = true;

                ctrlDynamicUDASymbol1.BindDynamicUDAs(_dynamicUDACache);
                CustomThemeHelper.SetThemeProperties(ctrlDynamicUDASymbol1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
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

        void accept_Click(object sender, EventArgs e)
        {
        }

        void cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdRowEditTemplate.Close(false);

                if (futureRootSymUI != null)
                {
                    futureRootSymUI.Close();
                }

                if (udaUI != null)
                {
                    udaUI.Close();
                }
                if (liveFeedValidateUI != null)
                {
                    liveFeedValidateUI.Close();
                }
                if (_isUpdateRequestFromTradingTicket)
                {
                    _isUpdateRequestFromTradingTicket = false;
                    this.Close();
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
        /// handle on SM server connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Connected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        SetGetButtonDetails("Get Data", true);
                        toolStripStatusLabel1.Text = "Trade Server : Connected";
                        //Added to bind datasource, PRANA-12783
                        if (cmbbxSMView.Value == null || cmbbxSearchCriteria.Value == null || cmbbxMatchOn.Value == null)
                        {
                            BindDataSource();
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

        /// <summary>
        /// handle on SM server Disconnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Disconnected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        SetGetButtonDetails("Connect", true);
                        toolStripStatusLabel1.Text = "Trade Server : Disconnected";
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
        /// Handle on completed status recieved
        /// </summary>
        /// <param name="qMsg"></param>
        void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SMQMsgInvokeDelegate invokeDelegate = new SMQMsgInvokeDelegate(_securityMaster_ResponseCompleted);
                        this.BeginInvoke(invokeDelegate, new object[] { this, e });
                    }
                    else
                    {
                        if (!toolStripStatusLabel1.Text.Contains("Success"))
                        {
                            grdData.Enabled = false;
                            _isSearchInProcess = false;
                            _isSetIssuer = false;
                            SetGetButtonDetails("Get Data", true);

                            lblStatus.Text = toolStripStatusLabel1.Text += ": " + e.Value.Message.ToString();

                            // update symbol type to unchanged for new symbol after it is saved sucessfully, PRANA-9269
                            if (e.Value.Message.ToString().Contains("Success"))
                            {
                                Parallel.ForEach(_secMasterUIobj, x => { if (x.SymbolType == SymbolType.New) x.SymbolType = SymbolType.Unchanged; });
                            }
                            // enable add security tool button after sm data is saved sucessfully, PRANA-9269
                            toolBarManager.Tools[CONST_ADD_SECURITY].SharedProps.Enabled = true;
                            grdData.Enabled = true;
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

        /// <summary>
        /// Handle on normal search filter data recieved
        /// </summary>
        /// <param name="ds"></param>
        void _securityMaster_SymbolLookUpDataResponse(object sender, EventArgs<DataSet> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SymbolLookUpRequest symbolLookup = new SymbolLookUpRequest(Update);
                        this.BeginInvoke(symbolLookup, new object[] { sender, e });
                    }
                    else
                    {
                        Update(sender, e);
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
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
        /// Grid row view intialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {

                SecMasterUIObj secMasterUIObj = (SecMasterUIObj)e.Row.ListObject;
                BindDynamicUDAValue(_dynamicUDACache, secMasterUIObj, e.Row);
                if (secMasterUIObj.AssetID == (int)AssetCategory.FX || secMasterUIObj.AssetID == (int)AssetCategory.FXForward)
                {
                    e.Row.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Activation = Activation.ActivateOnly;
                    e.Row.Cells[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()].Activation = Activation.ActivateOnly;
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

        private void txtbxInput_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbxInput.Text))
            {
                _isAlreadyRequested = false;
            }

        }

        private void grdData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand grdDataBand = null;
                SetDynamicUDA(_dynamicUDACache);
                grdData.DisplayLayout.Bands[1].Hidden = true;
                grdDataBand = grdData.DisplayLayout.Bands[0];
                SetGridColumns(grdDataBand);
                SetColumnsForSelectedSMView();
                cmbbxSMView.Value = _smViewName;

                grdDataBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Normal;

                // Kuldeep A.: disabling Leveraged factor for options and covertible bonds as in case of these there is no relavance of Leveraged factor.
                foreach (UltraGridRow row in grdData.DisplayLayout.Bands[0].GetRowEnumerator(GridRowType.DataRow))
                {
                    if (((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.EquityOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FutureOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FXOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.ConvertibleBond)
                    {
                        row.Cells["Delta"].Activation = Activation.Disabled;
                        row.Cells["StrikePriceMultiplier"].Activation = Activation.Disabled;
                        row.Cells["EsignalOptionRoot"].Activation = Activation.Disabled;
                        row.Cells["BloombergOptionRoot"].Activation = Activation.Disabled;
                    }

                    row.Cells["DataSource"].Activation = Activation.Disabled;
                    row.Cells["CreationDate"].Activation = Activation.Disabled;
                    row.Cells["ApprovedBy"].Activation = Activation.Disabled;
                    row.Cells["SecApprovalStatus"].Activation = Activation.Disabled;
                    row.Cells["ModifiedDate"].Activation = Activation.Disabled;
                    row.Cells["CreatedBy"].Activation = Activation.Disabled;
                    row.Cells["ApprovalDate"].Activation = Activation.Disabled;
                    row.Cells["ModifiedBy"].Activation = Activation.Disabled;
                    row.Cells["IsSecApproved"].Activation = Activation.Disabled;
                    row.Cells["IsCurrencyFuture"].Activation = Activation.Disabled;
                    //Added to disable CutOffTime Column, PRANA-11970
                    row.Cells["CutOffTime"].Activation = Activation.Disabled;
                }

                grdDataBand.Columns[OrderFields.PROPERTY_VSCURRENCYID].Hidden = true;
                grdDataBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID].Hidden = true;
                grdDataBand.Columns[OrderFields.PROPERTY_LEADCURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdDataBand.Columns[OrderFields.PROPERTY_VSCURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        private void grdData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                grdData.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdData_InitializeRow);
                SecMasterUIObj secMasterUiObj = (SecMasterUIObj)e.Cell.Row.ListObject;
                BindDynamicUDAValueToSecMasterUIObj(ref secMasterUiObj, e.Cell.Row);

                if (secMasterUiObj.LeadCurrencyID == int.MinValue)
                    secMasterUiObj.LeadCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (secMasterUiObj.CurrencyID == int.MinValue)
                    lblVSCurrencyValue.Text = "";
                if (secMasterUiObj.SymbolType != BusinessObjects.AppConstants.SymbolType.New)
                {
                    secMasterUiObj.SymbolType = BusinessObjects.AppConstants.SymbolType.Updated;
                }
                switch (e.Cell.Column.Key)
                {
                    case OrderFields.PROPERTY_ASSET_ID:
                        int assetID = int.Parse(e.Cell.Value.ToString());
                        SetGridBasedOnAsset(grdData.ActiveRow);
                        AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)assetID);
                        switch (baseAssetCategory)
                        {
                            case AssetCategory.Equity:
                            case AssetCategory.PrivateEquity:
                            case AssetCategory.CreditDefaultSwap:
                                {
                                    break;
                                }
                            case AssetCategory.Option:
                                {

                                    break;
                                }
                            case AssetCategory.Future:
                                if (assetID.Equals((int)(AssetCategory.FXForward)))
                                {
                                    CreateTickerAndDescription(secMasterUiObj);
                                }

                                break;
                            case AssetCategory.FX:
                                CreateTickerAndDescription(secMasterUiObj);
                                break;

                            default:
                                break;

                        }

                        FxColumnSettings(grdData.ActiveRow);
                        if (!_isAddedRow)
                        {
                            if (secMasterUiObj.Comments.Contains(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString()))
                            {
                                secMasterUiObj.Comments = secMasterUiObj.Comments.Replace(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString(), string.Empty);
                            }
                            else if (secMasterUiObj.Comments.Contains(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString() + ","))
                            {
                                secMasterUiObj.Comments = secMasterUiObj.Comments.Replace(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString() + ",", string.Empty);
                            }
                        }
                        break;

                    case OrderFields.PROPERTY_EXCHANGEID:
                    case OrderFields.PROPERTY_UNDERLYING_ID:

                        if (!_isAddedRow)
                        {
                            if (secMasterUiObj.Comments.Contains(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString()))
                            {
                                secMasterUiObj.Comments = secMasterUiObj.Comments.Replace(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString(), string.Empty);
                            }
                            else if (secMasterUiObj.Comments.Contains(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString() + ","))
                            {
                                secMasterUiObj.Comments = secMasterUiObj.Comments.Replace(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString() + ",", string.Empty);
                            }
                        }
                        break;

                    case "ExpirationDate":
                        if ((DateTime.Parse(e.Cell.Value.ToString())) < DateTimeConstants.MinValue)
                        {
                            e.Cell.CancelUpdate();
                            toolStripStatusLabel1.Text = "Expiration Date was invalid";
                            e.Cell.Value = DateTimeConstants.MinValue;
                            break;
                        }
                        else
                        {
                            CreateTickerAndDescription(secMasterUiObj);
                            break;
                        }
                    case "IsZero":
                        if (e.Cell.Value.Equals(true))
                        {
                            grdData.ActiveRow.Cells["Coupon"].Value = 0.0;
                            grdData.ActiveRow.Cells["Coupon"].Activation = Activation.Disabled;
                        }
                        else
                        {
                            grdData.ActiveRow.Cells["Coupon"].Activation = Activation.AllowEdit;
                        }
                        break;
                    case "IsNDF":
                        if (e.Cell.Value.Equals(false))
                        {
                            grdData.ActiveRow.Cells["FixingDate"].Activation = Activation.Disabled;
                        }
                        else
                        {
                            grdData.ActiveRow.Cells["FixingDate"].Activation = Activation.AllowEdit;
                        }
                        break;
                    case OrderFields.PROPERTY_TICKERSYMBOL:
                        AssetCategory baseAsset = Mapper.GetBaseAssetCategory((AssetCategory)secMasterUiObj.AssetID);
                        if (!baseAsset.Equals(AssetCategory.Option))
                        {
                            grdData.ActiveRow.Cells[OrderFields.PROPERTY_UNDERLYINGSYMBOL].Value = e.Cell.Value.ToString().Trim();
                            secMasterUiObj.UnderLyingSymbol = secMasterUiObj.TickerSymbol.Trim();
                        }
                        if (AssetCategory.FutureOption.Equals((AssetCategory)secMasterUiObj.AssetID) || AssetCategory.Future.Equals((AssetCategory)secMasterUiObj.AssetID))
                        {
                            SetUDAfromFuturRootSymbol(e.Cell.Value.ToString().Trim());
                        }
                        // Set default Description as ticker name - omshiv, Dec 2013
                        if (String.IsNullOrEmpty(secMasterUiObj.LongName))
                        {
                            grdData.ActiveRow.Cells["LongName"].Value = e.Cell.Value.ToString().Trim();
                        }

                        // below line is written to trim the spaces from UnderLying Symbol
                        secMasterUiObj.UnderLyingSymbol = secMasterUiObj.UnderLyingSymbol.Trim();

                        break;

                    // set value of issuer same as its description for base security
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9509
                    case OrderFields.PROPERTY_LONGNAME:
                        if (secMasterUiObj.TickerSymbol.Trim().ToUpper().Equals(secMasterUiObj.UnderLyingSymbol.Trim().ToUpper()))
                        {
                            if (grdData.DisplayLayout.Bands[0].Columns.Exists("Issuer"))
                            {
                                //update issuer when description is changed if its same as default value, PRANA-10435
                                if (e.Cell.OriginalValue.ToString().Trim().Equals(grdData.ActiveRow.Cells["Issuer"].Value.ToString().Trim()) || grdData.ActiveRow.Cells["Issuer"].Value.ToString() == string.Empty || grdData.ActiveRow.Cells["Issuer"].Value.ToString().Trim().Equals(_dynamicUDACache["Issuer"].DefaultValue.ToString().Trim()))
                                {
                                    grdData.ActiveRow.Cells["Issuer"].Value = secMasterUiObj.LongName.ToString().Trim();
                                    SetSameUDAforDerivatives(secMasterUiObj.TickerSymbol.Trim(), "Issuer", secMasterUiObj.LongName.ToString().Trim());
                                }
                            }
                        }
                        break;

                    // update risk currency, http://jira.nirvanasolutions.com:8080/browse/PRANA-9768
                    case OrderFields.PROPERTY_CURRENCYID:
                        AssetCategory assetCategory = (AssetCategory)secMasterUiObj.AssetID;
                        if (assetCategory == AssetCategory.FX || assetCategory == AssetCategory.FXForward)
                        {
                            secMasterUiObj.LeadCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            lblLeadCurrencyValue.Text = CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.LeadCurrencyID];

                            if (secMasterUiObj.CurrencyID != int.MinValue)
                            {
                                string currentTradeCurrency = CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID];
                                if (currentTradeCurrency.Equals(lblLeadCurrencyValue.Text))
                                {
                                    MessageBox.Show("Trade Currency cannot be same as Lead Currency.", "Security Master", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    secMasterUiObj.CurrencyID = secMasterUiObj.VsCurrencyID;
                                    return;
                                }
                                lblVSCurrencyValue.Text = CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID];
                            }
                            secMasterUiObj.VsCurrencyID = secMasterUiObj.CurrencyID;

                            if (secMasterUiObj.CurrencyID != int.MinValue && (CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID].Equals("EUR") || CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID].Equals("GBP") || CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID].Equals("AUD") || CachedDataManager.GetInstance.GetAllCurrencies()[secMasterUiObj.CurrencyID].Equals("NZD")))
                            {
                                secMasterUiObj.LeadCurrencyID = secMasterUiObj.CurrencyID;
                                secMasterUiObj.CurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                                secMasterUiObj.VsCurrencyID = secMasterUiObj.CurrencyID;

                                lblLeadCurrencyValue.Text = CachedDataManager.GetInstance.GetCurrencyText(secMasterUiObj.LeadCurrencyID);
                                lblVSCurrencyValue.Text = CachedDataManager.GetInstance.GetCurrencyText(secMasterUiObj.VsCurrencyID);
                            }
                        }

                        if (grdData.DisplayLayout.Bands[0].Columns.Exists("RiskCurrency"))
                        {
                            grdData.ActiveRow.Cells["RiskCurrency"].Value = GetRiskCurrency(secMasterUiObj);
                        }

                        if (assetCategory == AssetCategory.FX || assetCategory == AssetCategory.FXForward)
                        {
                            CreateTickerAndDescription(secMasterUiObj);
                        }
                        break;

                    // update issuer, http://jira.nirvanasolutions.com:8080/browse/PRANA-9768
                    case "UnderLyingSymbol":

                        // below line is written to trim the spaces from ticket symbol
                        secMasterUiObj.TickerSymbol = secMasterUiObj.TickerSymbol.Trim();

                        if (secMasterUiObj.SymbolType == BusinessObjects.AppConstants.SymbolType.New)
                        {
                            if (secMasterUiObj.DynamicUDA.ContainsKey("Issuer"))
                                secMasterUiObj.DynamicUDA["Issuer"] = GetIssuerValueFromUnderlying(secMasterUiObj);
                            else
                                secMasterUiObj.DynamicUDA.Add("Issuer", GetIssuerValueFromUnderlying(secMasterUiObj));
                        }
                        break;
                }

                if (grdData.DisplayLayout.Bands[0].RowEditTemplate != null)
                    grdData.DisplayLayout.Bands[0].RowEditTemplate.Refresh();
                grdData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(grdData_InitializeRow);
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
        /// Set UDA data from Futur RootSymbol, 
        /// Request to server for get data of root symbol
        /// </summary>
        /// <param name="tickerSymbol"></param>
        private void SetUDAfromFuturRootSymbol(String tickerSymbol)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage();
                qMsg.RequestID = System.Guid.NewGuid().ToString();
                qMsg.Message = tickerSymbol;
                qMsg.MsgType = SecMasterConstants.CONST_SymbolRootData;
                _securityMaster.SendMessage(qMsg);

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

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                pageIndex++;

                if (_isAdvncdSearching)
                {
                    if (!string.IsNullOrEmpty(_advncdSearchQuery))
                    {
                        SendReqForAdvncedSearch(_advncdSearchQuery);
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;
                    }
                }
                else
                {

                    RequestData();
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (pageIndex > 0)
                {
                    pageIndex--;

                }

                if (_isAdvncdSearching)
                {
                    if (!string.IsNullOrEmpty(_advncdSearchQuery))
                    {
                        SendReqForAdvncedSearch(_advncdSearchQuery);
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;
                    }
                }
                else
                {
                    //Added to get exact data, PRANA-12692
                    //Added condition so that exact search does not does for company name or underlying symbol search criteria, PRANA-13894
                    SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), cmbbxSearchCriteria.Value.ToString());
                    if ((searchCriteria != SecMasterConstants.SearchCriteria.CompanyName) && (searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol))
                        SearchSecurityInCacheThenDB(searchCriteria);
                    RequestData();

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
        /// Open Future Root data UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRootData_Click(object sender, EventArgs e)
        {
            try
            {
                if (futureRootSymUI != null)
                {
                    futureRootSymUI.Close();
                }

                futureRootSymUI = new FutureRootSymbolUI();
                futureRootSymUI.FormClosed += new FormClosedEventHandler(futureRootSymUI_FormClosed);
                futureRootSymUI.FurureRootDataSaved += futureRootSymUI_FurureRootDataSaved;
                futureRootSymUI.SetDynamicUDAEvent += futureRootSymUI_SetDynamicUDAEvent;
                futureRootSymUI.UpdateDynamicUDACache += futureRootSymUI_UpdateDynamicUDACache;
                futureRootSymUI.SetUp(_securityMaster);
                String filterString = string.Empty;
                if (grdData.ActiveRow != null)
                {
                    String ticker = grdData.ActiveRow.Cells["TickerSymbol"].Value.ToString();
                    filterString = ticker;
                    String[] stringArray = ticker.Split(' ');
                    if (stringArray.Length > 1)
                    {
                        filterString = stringArray[0];
                    }

                }
                futureRootSymUI.HandleOnLoadRequest(filterString);
                futureRootSymUI.StartPosition = FormStartPosition.Manual;
                futureRootSymUI.Show();
                BringFormToFront(futureRootSymUI);
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
        /// Handle root data recieved from cache and set to active Sm obj
        /// </summary>
        /// <param name="dt"></param>
        void futureRootSymUI_FurureRootDataSaved(object sender, EventArgs<DataTable> e)
        {
            try
            {
                DataTable dt = e.Value;
                foreach (UltraGridRow gridRow in grdData.Rows)
                {
                    if (int.Parse(gridRow.Cells["AssetID"].Value.ToString()) == (int)AssetCategory.Future || int.Parse(gridRow.Cells["AssetID"].Value.ToString()) == (int)AssetCategory.FutureOption)
                    {
                        String ticker = gridRow.Cells["TickerSymbol"].Text;

                        string[] tickerArray = ticker.Split(' ');
                        if (tickerArray.Length > 1)
                        {
                            String rootSymbol = tickerArray[0].Trim();
                            String exchange = string.Empty;

                            string[] tickerWithExchangeArray = ticker.Split('-');
                            if (tickerWithExchangeArray.Length > 1)
                            {
                                exchange = tickerWithExchangeArray[1].Trim();
                            }
                            //TODO get better solution to search in DT - om
                            DataRow[] foundRows = dt.Select("symbol = '" + rootSymbol + "'");
                            if (foundRows.Length > 0)
                            {
                                bool shouldMergeUDAfromRootData = false;

                                foreach (DataRow rootRow in foundRows)
                                {
                                    // Added check for root symbol exchange to be null, so that all symbols should not be updated when root with specific root is updated
                                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9299
                                    if (string.IsNullOrEmpty(exchange) && string.IsNullOrEmpty(rootRow["Exchange"].ToString()) && rootRow["Symbol"].ToString().Equals(rootSymbol, StringComparison.OrdinalIgnoreCase))
                                    {
                                        shouldMergeUDAfromRootData = true;
                                    }
                                    else if (!string.IsNullOrEmpty(exchange) && rootRow["Symbol"].ToString().Equals(rootSymbol, StringComparison.OrdinalIgnoreCase) && rootRow["Exchange"].ToString().Equals(exchange, StringComparison.OrdinalIgnoreCase))
                                    {
                                        shouldMergeUDAfromRootData = true;
                                    }

                                    if (shouldMergeUDAfromRootData)
                                    {

                                        gridRow.Cells["UDASectorID"].Value = int.Parse(rootRow["UDASectorID"].ToString());
                                        gridRow.Cells["UDASubSectorID"].Value = int.Parse(rootRow["UDASubSectorID"].ToString());
                                        gridRow.Cells["UDASecurityTypeID"].Value = int.Parse(rootRow["UDASecurityTypeID"].ToString());
                                        gridRow.Cells["UDACountryID"].Value = int.Parse(rootRow["UDACountryID"].ToString());
                                        gridRow.Cells["Multiplier"].Value = rootRow["Multiplier"].ToString();
                                        gridRow.Cells["CutOffTime"].Value = rootRow["CutOffTime"].ToString();
                                        gridRow.Cells["IsCurrencyFuture"].Value = Convert.ToBoolean(rootRow["IsCurrencyFuture"].ToString());
                                        //Update dynamic UDA on UI of symbol lookup
                                        foreach (string uda in _dynamicUDACache.Keys)
                                        {
                                            gridRow.Cells[uda].Value = rootRow[uda].ToString();
                                        }

                                        break;
                                    }
                                }
                            }
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

        void futureRootSymUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (futureRootSymUI != null)
            {
                futureRootSymUI.FurureRootDataSaved -= futureRootSymUI_FurureRootDataSaved;
                futureRootSymUI.SetDynamicUDAEvent -= futureRootSymUI_SetDynamicUDAEvent;
                futureRootSymUI.UpdateDynamicUDACache -= futureRootSymUI_UpdateDynamicUDACache;
                futureRootSymUI = null;
            }
        }

        private void BringFormToFront(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;

            }
            form.Location = new Point(this.Location.X + 30, this.Location.Y + 30);
            form.BringToFront();
        }

        private CustomColumnChooser _customColumnChooserDialog = new CustomColumnChooser();
        private void grdData_ShowCustomColumnChooserDialog()
        {
            try
            {
                if (this._customColumnChooserDialog == null || this._customColumnChooserDialog.IsDisposed)
                {
                    _customColumnChooserDialog = new CustomColumnChooser();
                }
                if (this._customColumnChooserDialog.Owner == null)
                {
                    this._customColumnChooserDialog.Owner = this.FindForm();
                }
                if (this._customColumnChooserDialog.Grid == null)
                {
                    this._customColumnChooserDialog.Grid = this.grdData;
                }
                this._customColumnChooserDialog.Show();
                this._customColumnChooserDialog.DesktopLocation = this._customColumnChooserDialog.Owner.DesktopLocation;
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

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);
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

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;
        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        /// <summary>
        /// Proxy for Dynamic UDA
        /// </summary>
        ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;
        public ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set { _secMasterSyncService = value; }
        }

        public event EventHandler SymbolDoubleClicked;
        public void SetUP()
        {

        }
        public IPostTradeServices PostTradeServices
        {
            set {; }
        }
        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.Rows.Count > 0)
                {
                    CreateExcel(ExcelUtilities.FindSavePathForExcel());
                }
                else
                    toolStripStatusLabel1.Text = "No data to Export";

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
                this.Cursor = Cursors.Default;
            }
        }

        private void CreateExcel(String filepath)
        {
            try
            {
                if (filepath != null)
                {

                    symbolLookupUltraGridExcelExporter.Export(grdData, filepath);
                    MessageBox.Show("Grid data successfully downloaded to " + filepath, "Security Master Excel Exporter", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this);
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
        /// Show AUES selection UI for select per defined AUEC combinations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuecs_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {
                    SecMasterUIObj secMasterUIObj = (SecMasterUIObj)grdData.ActiveRow.ListObject;
                    ValidAUECs validAuecs = ValidAUECs.GetInstance();

                    if (!string.IsNullOrEmpty(secMasterUIObj.TickerSymbol) && secMasterUIObj.TickerSymbol.IndexOf('-') > 0)
                    {
                        String exchange = secMasterUIObj.TickerSymbol.Substring((secMasterUIObj.TickerSymbol.IndexOf('-')) + 1);
                        validAuecs.SearchAUECOnLoad(exchange);
                    }
                    validAuecs.ShowDialog();

                    ValidAUEC loadauec = validAuecs._selectedAUEC;

                    if (loadauec != null && loadauec.AssetID != int.MinValue)
                    {
                        //update/select AUEC of active row only
                        if (secMasterUIObj != null && loadauec != null)
                        {
                            grdData.ActiveRow.Cells["UnderLyingID"].Value = loadauec.UnderlyingID;
                            grdData.ActiveRow.Cells["ExchangeID"].Value = loadauec.ExchangeID;
                            grdData.ActiveRow.Cells["CurrencyID"].Value = loadauec.CurrencyID;
                            grdData.ActiveRow.Cells["AssetID"].Value = loadauec.AssetID;
                            //Added to Set the value of multiplier and roundlot, PRANA-10856
                            SetAUECValue(secMasterUIObj.AUECID, loadauec);
                            grdData.ActiveRow.Cells["TickerSymbol"].Activate();
                        }
                        else
                        {
                            grdData.ActiveRow.Cells["AssetID"].Activate();
                            grdData.ActiveRow.Cells["UnderLyingID"].Value = int.MinValue;
                            grdData.ActiveRow.Cells["ExchangeID"].Value = loadauec.ExchangeID;
                            grdData.ActiveRow.Cells["CurrencyID"].Value = loadauec.CurrencyID;
                            grdData.ActiveRow.Cells["AssetID"].Value = loadauec.AssetID;
                            grdData.ActiveRow.Cells["RoudnLot"].Value = loadauec.RoundLot;
                            grdData.ActiveRow.Cells["Multiplier"].Value = loadauec.Multiplier;
                        }
                    }

                    // Update UDA User Asset automatically to Asset Class if it is undefined
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7970
                    if (_isAddedRow)
                        grdData.ActiveRow.Cells["UDAAssetClassID"].Value = GetValueFromText(grdData.ActiveRow.Cells["UDAAssetClassID"].ValueListResolved, grdData.ActiveRow.Cells["AssetID"].Text, grdData.ActiveRow.Cells["PutOrCall"].Text);
                    else if (grdData.ActiveRow.Cells["UDAAssetClassID"].Text == "Undefined")
                        grdData.ActiveRow.Cells["UDAAssetClassID"].Value = GetValueFromText(grdData.ActiveRow.Cells["UDAAssetClassID"].ValueListResolved, grdData.ActiveRow.Cells["AssetID"].Text, grdData.ActiveRow.Cells["PutOrCall"].Text);

                    //condition added so that no new row is added and popup appear when AUEC is not selected, PRANA-10573
                    if (loadauec != null && loadauec.AssetID != int.MinValue)
                    {
                        grdData.PerformAction(UltraGridAction.EnterEditMode);
                        validAuecs._selectedAUEC = null;

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
        /// Set the value of multiplier and roundlot, PRANA-10856
        /// </summary>
        /// <param name="auecID">AUEC ID</param>
        /// <param name="loadauec">Selected AUEC</param>
        public void SetAUECValue(int auecID, ValidAUEC loadauec)
        {
            try
            {
                //added check if dictionary of multipliers/roundlots conatins auecID, PRANA-11675
                if (_dictMultipliers.ContainsKey(auecID) && _dictMultipliers[auecID] == Convert.ToDouble(grdData.ActiveRow.Cells["Multiplier"].Value))
                {
                    grdData.ActiveRow.Cells["Multiplier"].Value = loadauec.Multiplier;
                }
                if (_dictRoundLots.ContainsKey(auecID) && _dictRoundLots[auecID] == Convert.ToDecimal(grdData.ActiveRow.Cells["RoundLot"].Value))
                {
                    grdData.ActiveRow.Cells["RoundLot"].Value = loadauec.RoundLot;
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
        /// Set view of edit row template before opening it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_BeforeRowEditTemplateDisplayed(object sender, BeforeRowEditTemplateDisplayedEventArgs e)
        {
            try
            {
                var formPropInfo = e.Template.GetType().GetProperty("Form", BindingFlags.CreateInstance | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
                var formTemplate = (Form)formPropInfo.GetValue(e.Template);

                formTemplate.MinimumSize = new System.Drawing.Size(877, 450);
                formTemplate.MaximumSize = new System.Drawing.Size(877, 450);

                UltraFormManager formManager = Infragistics.Win.UltraWinForm.UltraFormManager.FromForm(formTemplate);
                if (formManager == null)
                {
                    formManager = new Infragistics.Win.UltraWinForm.UltraFormManager();
                    formManager.Form = formTemplate;
                    formTemplate.Disposed += delegate (object sen, EventArgs ev)
                    {
                        formManager.Dispose();
                    };
                    // Since FormManager uses form Handle to manipulates the form, if it is destroyed  we should remove and the manager
                    formTemplate.HandleDestroyed += delegate (object se, EventArgs ev)
                    {
                        formManager.Form = null;
                        formManager.Dispose();
                    };
                    if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana") && CustomThemeHelper.ApplyTheme)
                    {
                        formManager.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        formManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.grdRowEditTemplate.DialogSettings.Caption, CustomThemeHelper.UsedFont);
                    }
                }

                if (grdData.ActiveRow != null)
                {
                    lblStatus.Text = grdData.ActiveRow.Cells["TickerSymbol"].Value.ToString();
                    btnEditSecurity.Visible = false;
                    btnSelectAUEC.Visible = true;
                    SetGridBasedOnAsset(grdData.ActiveRow);
                    if (!_isUpdateRequestFromTradingTicket)
                    {
                        tabCntrlSecurity.SelectedTab = tabCntrlSecurity.Tabs[0];
                    }
                    else
                    {
                        tabCntrlSecurity.SelectedTab = tabCntrlSecurity.Tabs[1];
                    }
                    SetEditTemplateView();
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
        /// Save grid layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                String filePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SymbolLookUp.xml";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                grdData.DisplayLayout.Save(filePath,
                PropertyCategories.AppearanceCollection |
                PropertyCategories.Bands |
                PropertyCategories.ColScrollRegions |
                PropertyCategories.ColumnFilters |
                PropertyCategories.General |
                PropertyCategories.Groups |
                PropertyCategories.RowScrollRegions |
                PropertyCategories.SortedColumns |
                PropertyCategories.Summaries |
                PropertyCategories.UnboundColumns

                );

                String SMUISearchParamsFilePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SMUISearchParams.xml";
                if (File.Exists(SMUISearchParamsFilePath))
                {
                    File.Delete(SMUISearchParamsFilePath);
                }
                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();

                SMUISearchParams smUISearchParam = new SMUISearchParams();
                GetSearchParamsToSave(smUISearchParam);

                _xml.WriteFile(smUISearchParam, SMUISearchParamsFilePath);

                toolStripStatusLabel1.Text = "Layout has been Saved!";
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

        private void GetSearchParamsToSave(SMUISearchParams smUISearchParam)
        {
            try
            {
                smUISearchParam.SearchCriteria = cmbbxSearchCriteria.Text;
                smUISearchParam.MatchOn = cmbbxMatchOn.Text;
                smUISearchParam.SMColumnsView = cmbbxSMView.Text;
                smUISearchParam.EnteredText = txtbxInput.Text;
                smUISearchParam.SearchUnApprovedSec = chkbxUnApprovedSec.Checked;
                if (rdBtnSearchSymbols.Checked)
                {
                    smUISearchParam.SearchType = 1;
                }
                else if (rdBtnOpenSymbols.Checked)
                {
                    smUISearchParam.SearchType = 2;
                }
                else if (rdBtnHistSymbols.Checked)
                {
                    smUISearchParam.SearchType = 3;
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
        /// Set Search Parameters on symbol lookup UI
        /// modified by: sachin mishra 02 Feb 2015
        /// Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="smUISearchParam"></param>
        private void SetSearchParamsOnUI(SMUISearchParams smUISearchParam)
        {
            try
            {
                cmbbxMatchOn.Text = smUISearchParam.MatchOn;
                cmbbxSearchCriteria.Text = smUISearchParam.SearchCriteria;
                txtbxInput.Text = smUISearchParam.EnteredText;
                chkbxUnApprovedSec.Checked = smUISearchParam.SearchUnApprovedSec;
                switch (smUISearchParam.SearchType)
                {
                    case 1:
                        rdBtnSearchSymbols.Checked = true;
                        break;
                    case 2:
                        rdBtnOpenSymbols.Checked = true;
                        break;
                    case 3:
                        rdBtnHistSymbols.Checked = true;
                        break;

                }

                if (!string.IsNullOrEmpty(txtbxInput.Text) || chkbxUnApprovedSec.Checked)
                {
                    toolStripStatusLabel1.Text = "Click \"GetData\" To fetch data or modify search Parameters.";

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
        /// btnAdvncdSearch_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdvncdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (advSearchFilertUI == null)
                {
                    // Get the list for columns to be removed, renamed or ambigous columns from SecMasterHelper to set up advance search UI, PRANA-11280
                    advSearchFilertUI = new AdvSearchFilterUI();
                    Dictionary<String, ValueList> dataValuesList = SecMasterHelper.getInstance().GetRequiredValueListDict();
                    List<String> removeColumnsList = SecMasterHelper.getInstance().GetRemoveColumnsList();
                    Dictionary<string, string> dbAmbigousColumnsDictionary = SecMasterHelper.getInstance().GetAmbigousColumnsDictionary();
                    Dictionary<string, string> renameColumnsDictionary = SecMasterHelper.getInstance().GetRenameColumnsDictionary();
                    advSearchFilertUI.SetUp(typeof(SecMasterUIObj), dataValuesList, removeColumnsList, dbAmbigousColumnsDictionary, renameColumnsDictionary);
                    advSearchFilertUI.FormClosed += new FormClosedEventHandler(advSearchFilertUI_FormClosed);
                    advSearchFilertUI.SearchDataEvent += advSearchFilertUI_SearchDataEvent;
                }
                advSearchFilertUI.StartPosition = FormStartPosition.Manual;
                advSearchFilertUI.Show();
                BringFormToFront(advSearchFilertUI);

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

        void advSearchFilertUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (advSearchFilertUI != null)
                {
                    advSearchFilertUI = null;
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

        void advSearchFilertUI_SearchDataEvent(object sender, EventArgs<string> e)
        {
            try
            {
                grdData.Enabled = false;
                _isAdvncdSearching = true;
                _advncdSearchQuery = e.Value;
                toolStripStatusLabel1.Text = "Getting Data...";
                pageIndex = 0;
                //clear data from grid if "auto clear data on search" is checked
                if (_isClearDataOnSearch)
                {
                    _secMasterUIobj.Clear();
                }
                SendReqForAdvncedSearch(e.Value);

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

        private void SendReqForAdvncedSearch(string Queury)
        {
            try
            {
                int startIndex = pageIndex * _pageSize + 1;
                int endIndex = (pageIndex + 1) * _pageSize;

                StringBuilder sb = new StringBuilder();
                sb.Append(Queury);
                sb.Append(Seperators.SEPERATOR_5);
                sb.Append(startIndex);
                sb.Append(Seperators.SEPERATOR_5);
                sb.Append(endIndex);

                QueueMessage qMsq = new QueueMessage();
                qMsq.RequestID = System.Guid.NewGuid().ToString();
                qMsq.MsgType = SecMasterConstants.CONST_SMAdvncdSearch;
                qMsq.Message = sb.ToString();

                _securityMaster.SendMessage(qMsq);
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

        private void mnuItmExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
        /// Show previous tab on Row edit template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreTab_Click(object sender, EventArgs e)
        {

            try
            {
                tabCntrlSecurity.PerformAction(Infragistics.Win.UltraWinTabControl.UltraTabControlAction.NavigatePreviousTab);
                SetEditTemplateView();
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
        /// Show next tab on Row edit template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextTab_Click(object sender, EventArgs e)
        {
            try
            {

                tabCntrlSecurity.PerformAction(Infragistics.Win.UltraWinTabControl.UltraTabControlAction.NavigateNextTab);
                SetEditTemplateView();
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
        private void SetEditTemplateView()
        {
            try
            {
                int SelectedTabIndex = tabCntrlSecurity.SelectedTab.Index;

                switch (SelectedTabIndex)
                {
                    case 0:
                        lblPageDetail.Text = "Page: 1 of 4";
                        break;
                    case 1:
                        lblPageDetail.Text = "Page: 2 of 4";
                        break;
                    case 2:
                        lblPageDetail.Text = "Page: 3 of 4";
                        break;
                    case 3:
                        lblPageDetail.Text = "Page: 4 of 4";
                        break;

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

        public event EventHandler LaunchForm;

        private bool _useUDAFromUnderlyingOrRoot;
        public bool UseUDAFromUnderlyingOrRoot
        {
            get { return _useUDAFromUnderlyingOrRoot; }
            set { _useUDAFromUnderlyingOrRoot = value; }
        }

        private void grdData_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridColumn gridColumn = e.Cell.Column;
                SecMasterUIObj secMasterUiObj = (SecMasterUIObj)grdData.ActiveRow.ListObject;
                String ColumnText = e.Cell.Text;
                EmbeddableEditorBase editor = e.Cell.EditorResolved;
                object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                if (secMasterUiObj.SymbolType != SymbolType.New)
                    secMasterUiObj.SymbolType = SymbolType.Updated;
                String ColumnName = gridColumn.Key.ToString();
                switch (ColumnName)
                {
                    case "Select":
                        bool isChecked = bool.Parse(ColumnText);
                        e.Cell.Row.Selected = isChecked;
                        SetSelectedRowColors(e.Cell.Row);
                        break;

                    case "UseUDAFromUnderlyingOrRoot":
                        UseUDAFromUnderlyingOrRoot = bool.Parse(ColumnText);
                        if (UseUDAFromUnderlyingOrRoot)
                        {
                            e.Cell.Row.Cells["UDAAssetClassID"].Activation = Activation.Disabled;
                            e.Cell.Row.Cells["UDASectorID"].Activation = Activation.Disabled;
                            e.Cell.Row.Cells["UDASubSectorID"].Activation = Activation.Disabled;
                            e.Cell.Row.Cells["UDACountryID"].Activation = Activation.Disabled;
                            e.Cell.Row.Cells["UDASecurityTypeID"].Activation = Activation.Disabled;
                            EnableDisableDynamicUDARows(false, e.Cell.Row);
                            SetUDAFromUnderlyingSymbol();
                        }
                        else
                        {
                            e.Cell.Row.Cells["UDAAssetClassID"].Activation = Activation.AllowEdit;
                            e.Cell.Row.Cells["UDASectorID"].Activation = Activation.AllowEdit;
                            e.Cell.Row.Cells["UDASubSectorID"].Activation = Activation.AllowEdit;
                            e.Cell.Row.Cells["UDACountryID"].Activation = Activation.AllowEdit;
                            e.Cell.Row.Cells["UDASecurityTypeID"].Activation = Activation.AllowEdit;
                            EnableDisableDynamicUDARows(true, e.Cell.Row);
                        }
                        break;

                    case "UDASectorID":
                    case "UDASubSectorID":
                    case "UDASecurityTypeID":
                    case "UDACountryID":

                        if (secMasterUiObj.UnderLyingSymbol.Equals(secMasterUiObj.TickerSymbol, StringComparison.OrdinalIgnoreCase))
                        {
                            SetSameUDAforDerivatives(secMasterUiObj.TickerSymbol, ColumnName, changedValue);
                        }
                        break;

                    case "AssetID":
                        // Update UDA User Asset automatically to Asset Class if it is undefined
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7970
                        if (_isAddedRow)
                            e.Cell.Row.Cells["UDAAssetClassID"].Value = GetValueFromText(e.Cell.Row.Cells["UDAAssetClassID"].ValueListResolved, ColumnText, grdData.ActiveRow.Cells["PutOrCall"].Text);
                        else if (e.Cell.Row.Cells["UDAAssetClassID"].Text == "Undefined")
                            e.Cell.Row.Cells["UDAAssetClassID"].Value = GetValueFromText(e.Cell.Row.Cells["UDAAssetClassID"].ValueListResolved, ColumnText, grdData.ActiveRow.Cells["PutOrCall"].Text);
                        break;
                    case "PutOrCall":
                        e.Cell.Row.Cells["UDAAssetClassID"].Value = GetValueFromText(e.Cell.Row.Cells["UDAAssetClassID"].ValueListResolved, grdData.ActiveRow.Cells["AssetID"].Text, grdData.ActiveRow.Cells["PutOrCall"].Text);
                        break;
                }

                //Updating dynamic UDA for derivatives
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9019
                //SerializableDictionary<string, DynamicUDA> _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                if (_dynamicUDACache.ContainsKey(ColumnName))
                {
                    if (secMasterUiObj.UnderLyingSymbol.Equals(secMasterUiObj.TickerSymbol, StringComparison.OrdinalIgnoreCase))
                    {
                        SetSameUDAforDerivatives(secMasterUiObj.TickerSymbol, ColumnName, changedValue);
                    }
                }
                //Added to set default value when roundlot is null, PRANA-11585
                if (e.Cell.Row.Cells["RoundLot"].Text.Equals("__________________.__________"))
                {
                    e.Cell.Row.Cells["RoundLot"].Value = 1;
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
        /// Set UDA of derivatives in grid
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="TickerSymbol"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        private void SetSameUDAforDerivatives(string TickerSymbol, string ColumnName, object ColumnValue)
        {
            try
            {
                //if there is multiple rows in grid then change UDA for other rows
                if (_secMasterUIobj.Count > 1)
                {
                    foreach (UltraGridRow row in grdData.Rows)
                    {
                        if (row.Cells["UnderLyingSymbol"].Value.ToString().Equals(TickerSymbol, StringComparison.OrdinalIgnoreCase))
                        {
                            row.Cells[ColumnName].Value = ColumnValue;
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

        public event EventHandler LaunchPricingInput;
        private void SetSameUDAforDerivatives(SecMasterUIObj secMasterUiObj)
        {
            try
            {
                if (_secMasterUIobj.Count > 1)
                {
                    foreach (SecMasterUIObj uiobj in _secMasterUIobj)
                    {
                        if (uiobj.UnderLyingSymbol.Equals(secMasterUiObj.TickerSymbol, StringComparison.OrdinalIgnoreCase))
                        {
                            uiobj.UDASectorID = secMasterUiObj.UDASectorID;
                            uiobj.UDASubSectorID = secMasterUiObj.UDASubSectorID;
                            uiobj.UDASecurityTypeID = secMasterUiObj.UDASecurityTypeID;
                            uiobj.UDACountryID = secMasterUiObj.UDACountryID;

                        }
                    }
                    grdData.Refresh();
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

        private void SetUDAFromUnderlyingSymbol()
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    SecMasterUIObj secMasterUiObj = (SecMasterUIObj)grdData.ActiveRow.ListObject;
                    if (secMasterUiObj.AssetID == (int)AssetCategory.Future || secMasterUiObj.AssetID == (int)AssetCategory.FutureOption)
                    {

                        SetUDAfromFuturRootSymbol(secMasterUiObj.TickerSymbol);
                    }
                    else if (!secMasterUiObj.UnderLyingSymbol.Equals(secMasterUiObj.TickerSymbol))
                    {
                        if (_securityMaster != null && _securityMaster.IsConnected && !string.IsNullOrEmpty(secMasterUiObj.UnderLyingSymbol))
                        {
                            SecMasterRequestObj reqObj = new SecMasterRequestObj();
                            reqObj.AddData(secMasterUiObj.UnderLyingSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                            reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                            reqObj.HashCode = this.GetHashCode();
                            _securityMaster.SendRequest(reqObj);
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = lblStatus.Text = "Please enter Underlying Symbol!!";

                        }
                    }
                    else
                    {

                        toolStripStatusLabel1.Text = lblStatus.Text = "UDA is same as underlying symbol.";

                    }

                }
                else
                {
                    toolStripStatusLabel1.Text = lblStatus.Text = "TradeService not connected";
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
        /// Add Symbol to Pricing Input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPricingInputToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {
                    SecMasterUIObj secMasterUiObj = (SecMasterUIObj)grdData.ActiveRow.ListObject;
                    List<string> list = new List<string>();
                    if (secMasterUiObj != null)
                    {

                        list.Add(secMasterUiObj.TickerSymbol.ToString());
                        list.Add(secMasterUiObj.UnderLyingSymbol.ToString());
                        list.Add(secMasterUiObj.AssetID.ToString());
                        list.Add(secMasterUiObj.BloombergSymbol.ToString());
                        list.Add(secMasterUiObj.OSIOptionSymbol.ToString());
                        list.Add(secMasterUiObj.IDCOOptionSymbol.ToString());
                        list.Add(secMasterUiObj.LongName.ToString());
                        list.Add(secMasterUiObj.ProxySymbol.ToString());

                    }
                    if (LaunchPricingInput != null && !string.IsNullOrEmpty(secMasterUiObj.TickerSymbol))
                    {
                        LaunchPricingInput(this, new LaunchFormEventArgs(list));
                        toolStripStatusLabel1.Text = "";
                    }

                    else
                    {
                        toolStripStatusLabel1.Text = "Please select a symbol to add to Pricing Input";

                    }

                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select a symbol to add to Pricing Input";
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

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                SecMasterbaseList lst = new SecMasterbaseList();
                lst.RequestID = System.Guid.NewGuid().ToString();
                int selectedRowCount = 0;
                foreach (UltraGridRow gridRow in grdData.Rows)
                {
                    SecMasterUIObj uiObj = (SecMasterUIObj)gridRow.ListObject;

                    Boolean isRowSelected = Boolean.Parse(gridRow.Cells["Select"].Value.ToString().ToLower());
                    if (isRowSelected)
                    {
                        selectedRowCount++;
                        if (!uiObj.IsSecApproved)
                        {
                            uiObj.ApprovalDate = DateTime.Now;
                            uiObj.ApprovedBy = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                            uiObj.IsSecApproved = true;

                            if (uiObj.AUECID == int.MinValue)
                            {
                                MessageBox.Show("This is not a valid AUEC, Please re enter the AUEC details", "Error", MessageBoxButtons.OK);
                                return;
                            }
                            if (!ValidateSymbolForSave(uiObj))
                            {
                                return;
                            }

                            SecMasterBaseObj secMasterBaseObj = GetSecMasterObjFromUIObject(uiObj);
                            if (secMasterBaseObj != null)
                            {
                                secMasterBaseObj.SymbolType = (int)BusinessObjects.AppConstants.SymbolType.Updated;
                                lst.Add(secMasterBaseObj);
                            }
                        }
                    }
                }

                toolStripStatusLabel1.Text = string.Empty;
                if (lst.Count > 0)
                {
                    _securityMaster.SaveNewSymbols(lst);
                    _isAddedRow = false;
                }
                else if (selectedRowCount > 0)
                {
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SecurityAlreadyApproved;
                }
                else
                {
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SelectSecToApprove;
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
        /// Trade selected symbol from TT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tradeSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {

                    SecMasterUIObj uiobj = (SecMasterUIObj)grdData.ActiveRow.ListObject;
                    if (uiobj != null)
                    {
                        string symbol;
                        switch (SymbologyHelper.DefaultSymbology)
                        {
                            case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                                symbol = uiobj.FactSetSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.ActivSymbol:
                                symbol = uiobj.ActivSymbol;
                                break;
                            case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                symbol = uiobj.BloombergSymbol;
                                break;
                            default:
                                symbol = uiobj.TickerSymbol;
                                break;
                        }
                        if (SymbolDoubleClicked != null && !string.IsNullOrEmpty(symbol))
                        {
                            SymbolDoubleClicked(symbol, null);
                        }
                    }
                    toolStripStatusLabel1.Text = "";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Please select a symbol to trade.";
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

        public void HandleOnLoadRequest(ListEventAargs requestArgs)
        {
            try
            {
                if (requestArgs != null)
                {
                    SymbolLookupRequestObject symbolLookupRequest = new SymbolLookupRequestObject();
                    Dictionary<String, String> argDict = requestArgs.argsObject as Dictionary<String, String>;

                    if (argDict != null && argDict.ContainsKey("Action"))
                    {
                        SecMasterConstants.SecurityActions symbolAction = (SecMasterConstants.SecurityActions)Enum.Parse(typeof(SecMasterConstants.SecurityActions), argDict["Action"]);
                        switch (symbolAction)
                        {
                            //handling of ADD security requested from out side. 
                            case SecMasterConstants.SecurityActions.ADD:

                                if (argDict.ContainsKey("SecMaster"))
                                {
                                    SecMasterUIObj smUIObj = binaryFormatter.DeSerialize(argDict["SecMaster"]) as SecMasterUIObj;
                                    if (smUIObj != null)
                                    {
                                        AddNewSymbolToSM(smUIObj);
                                    }
                                }
                                else if (argDict.ContainsKey("Symbol"))
                                {
                                    AddNewSymbolToSM(argDict["Symbol"]);
                                }
                                break;

                            //handling of Search/Approve security requested from out side. 
                            case SecMasterConstants.SecurityActions.APPROVE:
                            case SecMasterConstants.SecurityActions.SEARCH:
                            case SecMasterConstants.SecurityActions.UPDATE:

                                if (argDict.ContainsKey("SearchCriteria") && argDict.ContainsKey("Symbol"))
                                {
                                    String symbol = argDict["Symbol"];
                                    if (string.IsNullOrEmpty(symbol) && symbolAction == SecMasterConstants.SecurityActions.SEARCH)
                                    {
                                        toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;
                                        return;
                                    }
                                    else
                                    {
                                        SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), argDict["SearchCriteria"]);
                                        txtbxInput.Text = symbol;
                                        cmbbxSearchCriteria.Value = searchCriteria.ToString();
                                        cmbbxMatchOn.Text = SecMasterConstants.SearchMatchOn.Exact.ToString();
                                        rdBtnSearchSymbols.Checked = true;

                                        switch (searchCriteria)
                                        {
                                            case SecMasterConstants.SearchCriteria.Ticker:
                                                symbolLookupRequest.TickerSymbol = symbol;
                                                break;
                                            case SecMasterConstants.SearchCriteria.Bloomberg:
                                                symbolLookupRequest.BloombergSymbol = symbol;
                                                break;
                                            case SecMasterConstants.SearchCriteria.FactSetSymbol:
                                                symbolLookupRequest.FactSetSymbol = symbol;
                                                break;
                                            case SecMasterConstants.SearchCriteria.ActivSymbol:
                                                symbolLookupRequest.ActivSymbol = symbol;
                                                break;
                                        }
                                        // done changes get data for exact match when symbol Lookup UI is opened from TT, PRANA-13792
                                        //if ((searchCriteria != SecMasterConstants.SearchCriteria.CompanyName) && (searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol))
                                        //    SearchSecurityInCacheThenDB(searchCriteria);
                                    }
                                }
                                if (argDict.ContainsKey(ApplicationConstants.CONST_IS_SECURITY_APPROVED))
                                {
                                    symbolLookupRequest.IsSecApproved = bool.Parse(argDict[ApplicationConstants.CONST_IS_SECURITY_APPROVED]);
                                    txtbxInput.Text = "";
                                    chkbxUnApprovedSec.Checked = true;
                                }
                                else
                                {
                                    chkbxUnApprovedSec.Checked = false;
                                }

                                this.symbolLookupRequestObject = symbolLookupRequest;

                                RequestData();
                                _secMasterUIobjOld.Clear();
                                _secMasterUIobjOld.AddRange(_secMasterUIobj);
                                if (symbolAction == SecMasterConstants.SecurityActions.UPDATE)
                                {
                                    _isUpdateRequestFromTradingTicket = true;
                                    if (argDict.ContainsKey("SymbologyCode"))
                                    {
                                        _missingSymbologyCode = argDict["SymbologyCode"];
                                    }
                                }
                                break;
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

        private void mnuItmExit_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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

        private void mnuSymbolLookup_Opening(object sender, CancelEventArgs e)
        {
            if (!CachedDataManager.CheckPermissionAllAccountsAndTradingAccountsFromCache())
            {
                addPricingInputToolStripMenuItem.Enabled = false;
            }

        }

        private void mnuItemSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean IsClear = _isClearDataOnSearch;
                if (IsClear)
                {
                    _isClearDataOnSearch = false;
                }
                else
                {
                    _isClearDataOnSearch = true;
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
        private void ClearSavedLayout(object sender, EventArgs e)
        {
            try
            {
                String filePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SymbolLookUp.xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                //remove search param file
                String searchParamfilePath = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SMUISearchParams.xml";
                if (File.Exists(searchParamfilePath))
                {
                    File.Delete(searchParamfilePath);
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
        /// Open UDA UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUDAData_Click(object sender, EventArgs e)
        {
            try
            {
                if (udaUI != null)
                {
                    udaUI.Close();
                }

                udaUI = new UDAUIForm();
                // We have to show the form here to Create handle of UI.
                // Otherwise it will be updated on background thread before handle created, that may cause freeze scenarios.
                udaUI.Show();
                BringFormToFront(udaUI);
                udaUI.FormClosed += new FormClosedEventHandler(udaUI_FormClosed);
                udaUI.SetUp(_securityMaster);

                udaUI.StartPosition = FormStartPosition.Manual;
                udaUI.Show();
                BringFormToFront(udaUI);
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

        void udaUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (udaUI != null)
                {
                    udaUI = null;
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

        private void btnAUECMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (auecMappingUI == null)
                {
                    auecMappingUI = new AUECMappingUI();
                    // We have to show the form here to Create handle of UI.
                    // Otherwise it will be updated on background thread before handle created, that may cause freeze scenarios.
                    auecMappingUI.Show();
                    BringFormToFront(auecMappingUI);
                    auecMappingUI.FormClosed += new FormClosedEventHandler(auecMappingUI_FormClosed);
                    auecMappingUI.SetUp(_securityMaster);
                }
                auecMappingUI.StartPosition = FormStartPosition.Manual;
                auecMappingUI.Show();
                BringFormToFront(auecMappingUI);
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

        void auecMappingUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (auecMappingUI != null)
                {
                    auecMappingUI = null;
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

        private void rdBtnSearchSymbols_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cmbbxMatchOn.Enabled = false;
                cmbbxSearchCriteria.Enabled = false;
                chkbxUnApprovedSec.Enabled = false;
                txtbxInput.Enabled = false;
                txtbxInput.Text = string.Empty;

                if (rdBtnSearchSymbols.Checked)
                {
                    cmbbxMatchOn.Enabled = true;
                    cmbbxSearchCriteria.Enabled = true;
                    chkbxUnApprovedSec.Enabled = true;
                    txtbxInput.Enabled = true;
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SetSearchParams;
                }
                else if (rdBtnOpenSymbols.Checked)
                {
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SearchAllOpenSymbols;

                }
                else if (rdBtnHistSymbols.Checked)
                {
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_SearchAllHistTradedSymbols;
                }

                //Check any changes in Ui for save layout, modified by Omshiv, May 2014
                _isLayoutChanged = true;

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
        /// handle on view changed from UI as show all coulns or UDA only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxSMView_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Added null condition check, PRANA-12708
                if (cmbbxSMView.Value != null)
                {
                    _smViewName = cmbbxSMView.Value.ToString();
                }
                SetColumnsForSelectedSMView();
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
        private void SetColumnsForSelectedSMView()
        {
            try
            {
                //Added null condition check, PRANA-12708
                if (cmbbxSMView.Value != null && cmbbxSMView.Value.ToString().Equals(SecMasterConstants.SmViewType.CustomView.ToString()))
                {

                    //Modified by: sachin mishra Purpose: For deletion of old save layout file. Now gridData file rename into Symbollookup.xml CHMW-3221
                    String path = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + grdData.Name;
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    path = Application.StartupPath + "\\Prana Preferences\\" + _loggedInUserId + "\\" + "SymbolLookUp.xml";
                    if (File.Exists(path))
                    {
                        grdData.DisplayLayout.Load(path,
                        PropertyCategories.AppearanceCollection |
                        PropertyCategories.Bands |
                        PropertyCategories.ColScrollRegions |
                        PropertyCategories.ColumnFilters |
                        PropertyCategories.General |
                        PropertyCategories.Groups |
                        PropertyCategories.RowScrollRegions |
                        PropertyCategories.SortedColumns |
                        PropertyCategories.Summaries |
                        PropertyCategories.UnboundColumns

                        );
                    }

                    List<string> columnNames = new List<string>();
                    foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                    {
                        columnNames.Add(col.Key);
                    }

                    // add dynamic uda fields to grid in custom view after reloading saved layout
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-8984
                    SetDynamicUDA(_dynamicUDACache);
                    // set grid column properties for dynamic udas, PRANA-10440
                    SetGridUDAColumns(_dynamicUDACache);

                    ColumnEnumerator gridColumns = grdData.DisplayLayout.Bands[0].Columns.GetEnumerator();
                    while (gridColumns.MoveNext())
                    {
                        UltraGridColumn gridColum = gridColumns.Current;
                        if (!columnNames.Contains(gridColum.Key))
                            gridColum.Hidden = true;
                        if (gridColum.Header.Caption == OrderFields.CAPTION_BLOOMBERGSYMBOL)
                            gridColum.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL_WithCompositeCode;
                        if (gridColum.Header.Caption == OrderFields.PROPERTY_BLOOMBERGSYMBOLEXCODE)
                            gridColum.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL_WithExchangeCode;
                    }
                }
                else if (cmbbxSMView.Value != null && cmbbxSMView.Value.ToString().Equals(SecMasterConstants.SmViewType.UDAColumns.ToString()))
                {
                    ColumnEnumerator gridColumns = grdData.DisplayLayout.Bands[0].Columns.GetEnumerator();
                    while (gridColumns.MoveNext())
                    {
                        UltraGridColumn gridColum = gridColumns.Current;
                        gridColum.Hidden = true;
                    }

                    UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                    UltraGridColumn colTickerSymbol = gridBand.Columns["TickerSymbol"];
                    colTickerSymbol.Hidden = false;
                    UltraGridColumn colLongName = gridBand.Columns["LongName"];
                    colLongName.Hidden = false;

                    UltraGridColumn colUDAAsset = gridBand.Columns["UDAAssetClassID"];
                    colUDAAsset.Hidden = false;

                    UltraGridColumn colUDAUDASector = gridBand.Columns["UDASectorID"];
                    colUDAUDASector.Hidden = false;

                    UltraGridColumn colUDASubSector = gridBand.Columns["UDASubSectorID"];
                    colUDASubSector.Hidden = false;

                    UltraGridColumn colUDASecurityType = gridBand.Columns["UDASecurityTypeID"];
                    colUDASecurityType.Hidden = false;

                    UltraGridColumn colUDACountry = gridBand.Columns["UDACountryID"];
                    colUDACountry.Hidden = false;

                    if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                    {
                        foreach (string uda in _dynamicUDACache.Keys)
                        {
                            if (gridBand.Columns.Exists(uda))
                            {
                                UltraGridColumn gridUDAcolumn = gridBand.Columns[uda];
                                gridUDAcolumn.Hidden = false;
                            }
                        }
                    }
                }
                else
                {
                    ColumnEnumerator gridColumns = grdData.DisplayLayout.Bands[0].Columns.GetEnumerator();
                    while (gridColumns.MoveNext())
                    {
                        UltraGridColumn gridColum = gridColumns.Current;
                        if (!gridColum.IsChaptered)
                            gridColum.Hidden = false;

                    }
                }
                SetMaskingForGridColumns();
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
        /// focus on selected row near to mouse pointer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// on closing, check it already in process and promp to user for save layout or not.
        /// created by omshiv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SymbolLookUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_isClosingFromExtSource)
                {
                    if (_isSearchInProcess)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Your Search in process. Please wait and try again", "Prana Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (_isLayoutChanged)
                    {

                        DialogResult result = MessageBox.Show("Do you want to save search and grid layout?", "Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result.Equals(DialogResult.Yes))
                        {
                            saveLayoutToolStripMenuItem_Click(this, null);
                        }
                        else if (result.Equals(DialogResult.Cancel))
                        {
                            e.Cancel = true;
                        }
                        else
                        {
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

        public void SetClosingUIFrmExternally(bool isCLosingFromExt)
        {
            try
            {
                _isClosingFromExtSource = isCLosingFromExt;
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

        private void grdData_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                //Check any changes in Ui for save layout, modified by Omshiv, May 2014
                _isLayoutChanged = true;
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

        private void grdData_AfterPerformAction(object sender, AfterUltraGridPerformActionEventArgs e)
        {
            try
            {
                //Check any changes in Ui for save layout, modified by Omshiv, May 2014
                _isLayoutChanged = true;
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
        /// cmbbxSearchCriteria ValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxSearchCriteria_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Check any changes in Ui for save layout, modified by Omshiv, May 2014
                _isLayoutChanged = true;
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
        /// btnValidateFromLiveFeed_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidateFromLiveFeed_Click(object sender, EventArgs e)
        {
            try
            {
                if (liveFeedValidateUI == null)
                {
                    liveFeedValidateUI = new LiveFeedValidationForm();

                    liveFeedValidateUI.FormClosed += new FormClosedEventHandler(liveFeedValidateUI_FormClosed);
                    ((ILaunchForm)liveFeedValidateUI).LaunchForm += new EventHandler(liveFeedValidateUI_LaunchForm);

                    liveFeedValidateUI.SecurityMaster = _securityMaster;
                    if (!string.IsNullOrEmpty(txtbxInput.Text))
                    {
                        liveFeedValidateUI.SetUp(txtbxInput.Text.Trim(), cmbbxSearchCriteria.Value.ToString());
                    }
                    liveFeedValidateUI.ShowInTaskbar = false;
                    liveFeedValidateUI.SetUp(txtbxInput.Text.Trim(), cmbbxSearchCriteria.Value.ToString());

                }
                liveFeedValidateUI.StartPosition = FormStartPosition.Manual;
                liveFeedValidateUI.Show();
                BringFormToFront(liveFeedValidateUI);
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

        void liveFeedValidateUI_LaunchForm(object sender, EventArgs e)
        {
            try
            {
                if (LaunchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_PricingDataLookUp);
                    LaunchForm(this.FindForm(), args);
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
        /// btnAccountWiseUDA_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccountWiseUDA_Click(object sender, EventArgs e)
        {
            try
            {
                frmAccountWiseUDA = new Form();

                accountWiseUDA = new ctrlAccountWiseUDA();
                accountWiseUDA.SetUp(_securityMaster);
                UltraPanel ultraPanel1 = new UltraPanel();
                ultraPanel1.ClientArea.SuspendLayout();
                ultraPanel1.SuspendLayout();
                ultraPanel1.Dock = DockStyle.Fill;
                ultraPanel1.Name = "ultraPanel1";
                frmAccountWiseUDA.Controls.Add(ultraPanel1);
                accountWiseUDA.Dock = DockStyle.Fill;
                ultraPanel1.ClientArea.Controls.Add(accountWiseUDA);
                frmAccountWiseUDA.ShowIcon = false;
                frmAccountWiseUDA.Text = "Account Wise UDA";
                frmAccountWiseUDA.Size = new System.Drawing.Size(760, 510);
                frmAccountWiseUDA.StartPosition = FormStartPosition.CenterParent;
                frmAccountWiseUDA.MaximumSize = frmAccountWiseUDA.MinimumSize = new System.Drawing.Size(760, 510);
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(frmAccountWiseUDA);
                frmAccountWiseUDA.Load += new System.EventHandler(frmAccountWiseUDA_Load);
                frmAccountWiseUDA.ShowInTaskbar = false;
                ultraPanel1.ClientArea.ResumeLayout(false);
                ultraPanel1.ClientArea.PerformLayout();
                ultraPanel1.ResumeLayout(false);
                frmAccountWiseUDA.ShowDialog(this.Parent);
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
        /// Form close event for liveFeedValidateUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void liveFeedValidateUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (liveFeedValidateUI != null)
                {
                    liveFeedValidateUI = null;
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

        OTCTemplateView templateViewUI = null;
        InstrumentTypesFieldsView instrumentTypesViewUI = null;
        private void toolBarManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                String toolKey = e.Tool.Key;

                switch (toolKey)
                {
                    case CONST_ADVANCED_SEARCH:
                        btnAdvncdSearch_Click(this, null);
                        break;
                    case CONST_ADD_SECURITY:
                        addSymbolToolStripMenuItem_Click(this, null);
                        break;
                    case CONST_EXPORT_TO_EXCEL:
                        exportToExcelToolStripMenuItem_Click(this, null);
                        break;
                    case CONST_SAVE_LAYOUT:
                        saveLayoutToolStripMenuItem_Click(this, null);
                        break;
                    case CONST_CLEAR_LAYOUT:
                        ClearSavedLayout(this, null);
                        break;
                    case CONST_AUTO_CLEAR_DATA_ON_SEARCH:
                        StateButtonTool tool = toolBarManager.Tools[CONST_AUTO_CLEAR_DATA_ON_SEARCH] as StateButtonTool;
                        if (tool != null)
                        {
                            _isClearDataOnSearch = tool.Checked;
                        }
                        break;
                    case CONST_VALIDATE_SYMBOL_FROM_LIVE_FEED:
                        btnValidateFromLiveFeed_Click(this, null);
                        break;
                    case CONST_EDIT_UDA:
                        //Opening UDAUI linked to wrong function, changed to correct function, modified by Suraj Nataraj August 2014
                        menuUDAData_Click(this, null);
                        break;
                    case CONST_FUTURE_ROOT_DATA:
                        btnRootData_Click(this, null);
                        break;
                    case CONST_AUEC_Mappings:
                        btnAUECMapping_Click(this, null);
                        break;
                    case CONST_FUND_WISE_UDA:
                        btnAccountWiseUDA_Click(this, null);
                        break;
                    case CONST_DYNAMIC_UDA:
                        //Open the Dynamic UDA form when Dynamic UDA button will clicked
                        LoadDynamicUDA();
                        break;
                    case CONST_OTCTemplate:

                        if (templateViewUI == null)
                        {
                            templateViewUI = new OTCTemplateView();
                            // ElementHost.EnableModelessKeyboardInterop(templateViewUI);
                            templateViewUI.Closed += templateViewUI_Closed;
                            BringFormToFront(templateViewUI);
                        }
                        else
                        {
                            // ElementHost.EnableModelessKeyboardInterop(templateViewUI);
                            templateViewUI.WindowState = System.Windows.WindowState.Normal;
                            templateViewUI.Activate();

                        }
                        break;

                    case CONST_InstrumentFields:

                        if (instrumentTypesViewUI == null)
                        {
                            instrumentTypesViewUI = new InstrumentTypesFieldsView();
                            instrumentTypesViewUI.Closed += instrumentTypesViewUI_Closed;
                            BringFormToFront(instrumentTypesViewUI);
                            instrumentTypesViewUI.HideCheckBoxColumn(true);

                        }
                        else
                        {
                            instrumentTypesViewUI.WindowState = System.Windows.WindowState.Normal;
                            instrumentTypesViewUI.Activate();
                        }

                        break;
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

        private void BringFormToFront(System.Windows.Window window)
        {
            if (!window.IsVisible)
            {
                window.ShowInTaskbar = false;
                ElementHost.EnableModelessKeyboardInterop(window);
                new System.Windows.Interop.WindowInteropHelper(window) { Owner = Handle };
                window.Show();
                window.Activate();
            }
        }

        void instrumentTypesViewUI_Closed(object sender, EventArgs e)
        {
            if (instrumentTypesViewUI != null)
                instrumentTypesViewUI = null;
        }

        void templateViewUI_Closed(object sender, EventArgs e)
        {
            if (templateViewUI != null)
                templateViewUI = null;
        }


        /// <summary>
        /// LoadDynamicUDA() method to show Dynamic UDA form when Dynamic UDA button will clicked on Symbol Looup UI
        /// </summary>
        private void LoadDynamicUDA()
        {
            try
            {
                //SerializableDictionary<string, DynamicUDA> dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                // Send list of symbol lookup columns so that same name dynamic UDA cannot be added
                List<string> symbolLookupColumns = new List<string>();
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                {
                    if (!symbolLookupColumns.Contains(col.Key.ToLower().Trim()))
                        symbolLookupColumns.Add(col.Key.ToLower().Trim());
                    if (!symbolLookupColumns.Contains(col.Header.Caption.Replace(" ", String.Empty).ToLower().Trim()))
                        symbolLookupColumns.Add(col.Header.Caption.Replace(" ", String.Empty).ToLower().Trim());
                }

                _frmDynamicUDA = new DynamicUDAForm(_dynamicUDACache, symbolLookupColumns);
                _frmDynamicUDA.SaveDynamicUDA += frmDynamicUDA_SaveDynamicUDA;
                _frmDynamicUDA.CheckMasterValueAssigned += _frmDynamicUDA_CheckMasterValueAssigned;
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(_frmDynamicUDA);
                _frmDynamicUDA.ShowDialog(this.FindForm());
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
        /// To check master value is used or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _frmDynamicUDA_CheckMasterValueAssigned(object sender, EventArgs<string, string> e)
        {
            try
            {
                bool result = _secMasterSyncService.InnerChannel.CheckMasterValueAssigned(e.Value, e.Value2);
                _frmDynamicUDA.DeleteListViewMasterValue(result);
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
        /// Event to Save Dynamic UDA 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmDynamicUDA_SaveDynamicUDA(object sender, EventArgs<DynamicUDA, string> e)
        {
            try
            {
                SerializableDictionary<string, string> oldMasterValues = new SerializableDictionary<string, string>();
                if (_dynamicUDACache.ContainsKey(e.Value.Tag))
                    oldMasterValues = _dynamicUDACache[e.Value.Tag].MasterValues;

                bool saved = _secMasterSyncService.InnerChannel.SaveDynamicUDA(e.Value, e.Value2);
                if (saved)
                {
                    _frmDynamicUDA.UpdateDynamicUDAGrid(e.Value);
                    // Update dynamic UDA cache after saving or updating dynamic UDA
                    _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                    PublishDynamicUDA(e.Value);
                    if (!(string.IsNullOrWhiteSpace(e.Value2)))
                    {
                        string[] renamedKeys = e.Value2.Split(',');
                        for (int i = 0; i < renamedKeys.Length; i++)
                        {
                            RefreshGridColumnAfterRename(e.Value.Tag, oldMasterValues[renamedKeys[i]], e.Value.MasterValues[renamedKeys[i]]);
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

        public bool _isClearDataOnSearch { get; set; }

        /// <summary>
        /// handle on full scan or quick scan checkbox clicked
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBxIsFullSearch_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBxIsFullSearch.Checked)
                {
                    chkBxIsFullSearch.Text = "Full Scan";
                    toolStripStatusLabel1.Text = "\'Full Scan\' will be slower in comparison to \'Quick Scan\' ";
                }
                else
                {
                    chkBxIsFullSearch.Text = "Quick Scan";
                    toolStripStatusLabel1.Text = SecMasterConstants.MSG_FullQuickScan;
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

        private void frmAccountWiseUDA_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void toolBarManager_BeforeToolbarListDropdown(object sender, BeforeToolbarListDropdownEventArgs e)
        {
            try
            {
                e.ShowLockToolbarsMenuItem = false;
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
        /// Gets value of valuelist from the text
        /// </summary>
        /// <param name="valueList">The valuelist</param>
        /// <param name="text">Text String</param>
        /// <returns>Value for text</returns>
        private int GetValueFromText(IValueList valueList, string text, string putOrCall)
        {
            int newValue = int.MinValue;
            try
            {
                string assetName = string.Empty;
                if (putOrCall.Equals("NONE"))
                {
                    putOrCall = "PUT";
                }
                if (text.Equals("EquityOption") || text.Equals("FutureOption"))
                {
                    assetName = (putOrCall == "CALL") ? "Call" : "Put";
                    text = text.Replace("Option", " ");
                    assetName = text + assetName;
                    text = text + putOrCall;
                }
                for (int i = 0; i < valueList.ItemCount; i++)
                {
                    if (valueList.GetText(i).Equals(text) || valueList.GetText(i).Equals(assetName))
                        newValue = (int)valueList.GetValue(i);
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
            return newValue;
        }

        /// <summary>
        /// Add dynamic UDAs to grdData
        /// </summary>        
        private void SetDynamicUDA(SerializableDictionary<string, DynamicUDA> dynamicUDAcache)
        {
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (!grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                        {
                            UltraGridColumn gridUDAcolumn = grdData.DisplayLayout.Bands[0].Columns.Add(uda);
                            gridUDAcolumn.Header.Caption = dynamicUDAcache[uda].HeaderCaption.ToString();
                            gridUDAcolumn.Header.Column.Width = 100;

                            if (dynamicUDAcache[uda].MasterValues != null && dynamicUDAcache[uda].MasterValues.Count > 0)
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                                gridUDAcolumn.ValueList = GetValueList(dynamicUDAcache[uda].MasterValues);
                            }
                            else
                            {
                                gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;
                                gridUDAcolumn.CellMultiLine = DefaultableBoolean.False;
                            }
                            if (uda == "CustomUDA8" || uda == "CustomUDA9" || uda == "CustomUDA10" || uda == "CustomUDA11" || uda == "CustomUDA12")
                            {
                                gridUDAcolumn.Hidden = true;
                            }
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

        /// <summary>
        /// Gets list of dynamic UDA column names
        /// </summary>
        /// <returns>List of dynamic UDA column names</returns>
        private List<string> GetDynamicColumnList()
        {
            List<string> list = new List<string>();

            try
            {

                foreach (string key in _dynamicUDACache.Keys)
                {
                    list.Add(key);
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

            return list;
        }

        /// <summary>
        /// Sets the dynamic UDAs on fututre root UI
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event</param>
        private void futureRootSymUI_SetDynamicUDAEvent(object sender, EventArgs<bool> e)
        {
            try
            {
                futureRootSymUI.SetDynamicUDA(_dynamicUDACache);
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
        /// Bind value of dynamic UDAs to columns for each row
        /// </summary>
        /// <param name="dynamicUDAcache">The dynamic UDA cache</param>
        /// <param name="secMasterUIObj">The secMasterUIObj</param>
        /// <param name="ultraGridRow">The grid row</param>
        private void BindDynamicUDAValue(SerializableDictionary<string, DynamicUDA> dynamicUDAcache, SecMasterUIObj secMasterUIObj, UltraGridRow ultraGridRow)
        {
            grdData.AfterCellUpdate -= new CellEventHandler(this.grdData_AfterCellUpdate);
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                        {
                            if (secMasterUIObj.DynamicUDA.ContainsKey(uda) && secMasterUIObj.DynamicUDA[uda].ToString() != string.Empty)
                                ultraGridRow.Cells[uda].Value = secMasterUIObj.DynamicUDA[uda].ToString();
                            else
                                ultraGridRow.Cells[uda].Value = dynamicUDAcache[uda].DefaultValue;
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
            grdData.AfterCellUpdate += new CellEventHandler(this.grdData_AfterCellUpdate);
        }

        /// <summary>
        /// Bind value from ultragrid cell to dynamic UDA dictionary
        /// </summary>
        /// <param name="secMasterUIObj"></param>
        /// <param name="ultraGridRow"></param>
        private void BindDynamicUDAValueToSecMasterUIObj(ref SecMasterUIObj secMasterUIObj, UltraGridRow ultraGridRow)
        {
            try
            {
                if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                {
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                        {
                            if (ultraGridRow.Cells[uda].Value != null && ultraGridRow.Cells[uda].Value.ToString() != _dynamicUDACache[uda].DefaultValue && ultraGridRow.Cells[uda].Value.ToString() != string.Empty)
                            {
                                if (secMasterUIObj.DynamicUDA.ContainsKey(uda))
                                    secMasterUIObj.DynamicUDA[uda] = ultraGridRow.Cells[uda].Text;
                                else
                                    secMasterUIObj.DynamicUDA.Add(uda, ultraGridRow.Cells[uda].Text);
                            }
                            // Added check if current value is default, undefined or empty, then remove uda key from secMaster obj
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-9058
                            else if (ultraGridRow.Cells[uda].Value != null && ultraGridRow.Cells[uda].Value.ToString() == string.Empty)
                            {
                                if (secMasterUIObj.DynamicUDA.ContainsKey(uda))
                                    secMasterUIObj.DynamicUDA.Remove(uda);
                            }
                            else if (ultraGridRow.Cells[uda].Value != null && ultraGridRow.Cells[uda].Value.ToString() == _dynamicUDACache[uda].DefaultValue)
                            {
                                if (secMasterUIObj.DynamicUDA.ContainsKey(uda))
                                    secMasterUIObj.DynamicUDA[uda] = ultraGridRow.Cells[uda].Text;
                            }
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

        /// <summary>
        /// set dynamic UDA columns properties
        /// </summary>
        /// <param name="gridBand">The gridband</param>
        /// <param name="dynamicUDAcache">The dynamic UDAs cache</param>
        private void SetGridUDAColumns(SerializableDictionary<string, DynamicUDA> dynamicUDAcache)
        {
            try
            {
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];
                if (gridBand != null)
                {
                    if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                    {
                        foreach (string uda in dynamicUDAcache.Keys)
                        {
                            if (grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                            {
                                UltraGridColumn gridUDAcolumn = gridBand.Columns[uda];
                                gridUDAcolumn.Header.Caption = dynamicUDAcache[uda].HeaderCaption.ToString();
                                gridUDAcolumn.Header.Column.Width = 100;

                                if (dynamicUDAcache[uda].MasterValues != null && dynamicUDAcache[uda].MasterValues.Count > 0)
                                {
                                    gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                                    gridUDAcolumn.ValueList = GetValueList(dynamicUDAcache[uda].MasterValues);
                                }
                                else
                                {
                                    gridUDAcolumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                                    gridUDAcolumn.CellMultiLine = DefaultableBoolean.False;
                                }

                            }
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

        public delegate void PublishInvokeDelegate(DynamicUDA dynamicUDA);
        private void PublishDynamicUDA(DynamicUDA dynamicUDA)
        {
            try
            {
                if (UIValidation.GetInstance().validate(futureRootSymUI))
                {
                    if (futureRootSymUI.InvokeRequired)
                    {
                        PublishInvokeDelegate publishDelegate = PublishDynamicUDA;
                        (futureRootSymUI).BeginInvoke(publishDelegate, dynamicUDA);
                    }
                    else
                    {
                        SerializableDictionary<string, DynamicUDA> dynamicUDAcache = new SerializableDictionary<string, DynamicUDA>();
                        dynamicUDAcache.Add(dynamicUDA.Tag, dynamicUDA);

                        //publish dynamic UDAs to Symbol lookup UI
                        SetDynamicUDA(dynamicUDAcache);
                        SetGridUDAColumns(dynamicUDAcache);

                        //publish dynamic UDAs to future root UI
                        if (futureRootSymUI != null)
                        {
                            futureRootSymUI.AddDynamicUDA(dynamicUDAcache);
                            futureRootSymUI.SetDynamicUDA(dynamicUDAcache);
                            //dynamic uda values will not be changes when default value is changed, PRANA-10217
                            // refresh dynamic uda column on Future root UI, PRANA-9986
                            //futureRootSymUI.RefreshDynamicUDAColumn(dynamicUDA.Tag);
                        }

                        //publish dynamic UDA to Security Information popup                
                        //SerializableDictionary<string, DynamicUDA> _dynamicUDAs = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                        ctrlDynamicUDASymbol1.BindDynamicUDAs(_dynamicUDACache);
                        CustomThemeHelper.SetThemeProperties(ctrlDynamicUDASymbol1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);

                        //dynamic uda values will not be changes when default value is changed, PRANA-10217
                        //RefreshDynamicUDAColumn(dynamicUDA.Tag);
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


        ///// <summary>
        ///// Refreshes UDA data column on grid
        ///// </summary>
        ///// <param name="uda"></param>
        //private void RefreshDynamicUDAColumn(string uda)
        //{
        //    try
        //    {
        //        //SerializableDictionary<string, DynamicUDA> _dynamicUDAcache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
        //        SerializableDictionary<string, DynamicUDA> _dynamicUDA = new SerializableDictionary<string, DynamicUDA>();
        //        _dynamicUDA.Add(_dynamicUDACache[uda].Tag, _dynamicUDACache[uda]);

        //        foreach (UltraGridRow row in grdData.Rows)
        //        {
        //            SecMasterUIObj secMasterUIObj = (SecMasterUIObj)row.ListObject;
        //            BindDynamicUDAValue(_dynamicUDA, secMasterUIObj, row);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Refreshed grid column after rename of master values is saved
        /// </summary>
        /// <param name="uda">The dynamic UDA tag</param>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        private void RefreshGridColumnAfterRename(string uda, string oldValue, string newValue)
        {
            try
            {
                foreach (UltraGridRow row in grdData.Rows)
                {
                    if (grdData.DisplayLayout.Bands[0].Columns.Exists(uda))
                    {
                        if (row.Cells[uda].Value.ToString() == oldValue)
                            row.Cells[uda].Value = newValue;
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

        private int GetIndexFromValue(IValueList valueList, string text)
        {
            int index = 0;
            try
            {
                for (int i = 0; i < valueList.ItemCount; i++)
                {
                    if (valueList.GetValue(i).Equals(text))
                    {
                        index = i;
                        break;
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
            return index;
        }

        /// <summary>
        /// Update Dynamic Uda Rows on Security Information UI
        /// </summary>
        /// <param name="row"></param>
        /// <param name="_dynamicUDACache"></param>
        private void UpdateDynamicUDAFromUnderlying(UltraGridRow row, SerializableDictionary<string, object> secMasterObjDynamicUDA)
        {
            try
            {
                if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                {
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (row.Cells.Exists(uda))
                        {
                            if (secMasterObjDynamicUDA.ContainsKey(uda))
                                row.Cells[uda].Value = secMasterObjDynamicUDA[uda].ToString();
                            else
                                row.Cells[uda].Value = _dynamicUDACache[uda].DefaultValue;
                            //row.Cells[uda].Value = dynamicUDACache[uda].ToString();
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

        /// <summary>
        /// Enable/ Disable Dynamic UDA rows
        /// </summary>
        /// <param name="p"></param>
        private void EnableDisableDynamicUDARows(bool val, UltraGridRow row)
        {
            try
            {
                //SerializableDictionary<string, DynamicUDA> _dynamicUDAcache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                if (_dynamicUDACache != null && _dynamicUDACache.Count > 0)
                {
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (row.Cells.Exists(uda))
                        {
                            if (val)
                                row.Cells[uda].Activation = Activation.AllowEdit;
                            else
                                row.Cells[uda].Activation = Activation.Disabled;
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

        /// <summary>
        /// Update dynamic UDA cache on Future root UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void futureRootSymUI_UpdateDynamicUDACache(object sender, EventArgs<bool> e)
        {
            try
            {
                //_dynamicUDAcache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                futureRootSymUI.UpdateDynamicUDA(_dynamicUDACache);
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
        /// show tool tip on status bar mouse hover for dynamic UDAs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripStatusLabel1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (sender.ToString().Contains("special characters"))
                {
                    UltraToolTipInfo toolTipInfo = new UltraToolTipInfo("Allowed special characters are: &/:@*!^%-$_#~(=)`{}\"[];'><?|", Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
                    ultraToolTipManager1.SetUltraToolTip(lblStatus, toolTipInfo);
                    ultraToolTipManager1.ShowToolTip(lblStatus);
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
        /// Gets value of issuer from underlying
        /// </summary>
        /// <param name="secMasterUiObj">Security master Obj</param>
        /// <returns>Value of issuer</returns>
        private string GetIssuerValueFromUnderlying(SecMasterUIObj secMasterUiObj)
        {
            // set local search to false when sending request for underlying symbol, PRANA-9838
            _isLocalDBSearchInProcess = false;
            _isSetIssuer = true;
            string issuer = string.Empty;
            try
            {
                if (secMasterUiObj.TickerSymbol.ToUpper().Equals(secMasterUiObj.UnderLyingSymbol.ToUpper()))
                    issuer = secMasterUiObj.LongName;
                // set default issuer for FX and FX Forward symbols same as Underlying symbol, PRANA-10830
                else if (secMasterUiObj.AssetID.Equals((int)AssetCategory.FXForward) || secMasterUiObj.AssetID.Equals((int)AssetCategory.FX))
                    issuer = secMasterUiObj.UnderLyingSymbol;
                else
                {
                    List<string> underlyingSymbolList = new List<string>();
                    underlyingSymbolList.Add(secMasterUiObj.UnderLyingSymbol.ToString());
                    Dictionary<string, SecMasterBaseObj> underlyingSymbolBaseObjList = _secMasterSyncService.InnerChannel.GetSecMasterSymbolData(underlyingSymbolList, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    if (underlyingSymbolBaseObjList.ContainsKey(secMasterUiObj.UnderLyingSymbol.ToString()))
                    {
                        issuer = underlyingSymbolBaseObjList[secMasterUiObj.UnderLyingSymbol.ToString()].LongName;
                        _isSetIssuer = false;
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
            return issuer;
        }

        /// <summary>
        /// Set input masking for grid columns
        /// </summary>
        private void SetMaskingForGridColumns()
        {
            try
            {
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];
                if (gridBand.Columns.Exists("RoundLot"))
                {
                    gridBand.Columns["RoundLot"].Format = "##################0.#########";
                    gridBand.Columns["RoundLot"].MaskInput = "{double:18.10}";
                    gridBand.Columns["RoundLot"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                    gridBand.Columns["RoundLot"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                    gridBand.Columns["RoundLot"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
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

        /// <summary>
        /// To change the character casing for Activ Symbol from Upper to camelCase or vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbActivSymbolCamelCase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //Get the grid band to change its character casing
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                //Check whether checkbox is checked or not and change the character casing and activ symbol value accordingly
                if (cbActivSymbolCamelCase.Checked)
                {
                    gridBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Normal;
                }
                else
                {
                    if (grdData.ActiveRow.Cells["ActivSymbol"].Value != null)
                    {
                        //Get the current activ symbol value
                        string activSymbol = grdData.ActiveRow.Cells["ActivSymbol"].Value.ToString();
                        grdData.ActiveRow.Cells["ActivSymbol"].Value = activSymbol.ToUpper();
                    }
                    gridBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Upper;
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
        /// To set the Allow CamelCase CheckBox as per the Activ Symbol value
        /// </summary>
        /// <param name="activSymbol"></param>
        private void SetAllowCamelCaseCheckBox(string activSymbol)
        {
            try
            {
                //Get the grid band to change its character casing
                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                //set the checkbox as checked if activ symbol have any lowercase character
                if (activSymbol.Any(char.IsLower))
                {
                    cbActivSymbolCamelCase.Checked = true;
                    gridBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Normal;
                }
                else
                {
                    cbActivSymbolCamelCase.Checked = false;
                    gridBand.Columns["ActivSymbol"].CharacterCasing = CharacterCasing.Upper;
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
                exporter.Export(grdData, exportFilePath);
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
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (gridName == "grdData")
                ExportGridData(filePath);
            else if (gridName == "ultraGrid" && auecMappingUI != null)
                auecMappingUI.ExportGridData(filePath);
        }
    }
}