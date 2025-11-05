using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Newtonsoft.Json.Linq;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.AuditTrail
{
    /// <summary>
    /// Audit Trail UI Class accesses the Audit Manager to get data and display. Instance stored in Nirvanamain
    /// </summary>
    public partial class ctrlAuditTrail : UserControl
    {
        bool _isStartUp = true;
        bool _loading = false;
        bool _isAllocationAuditTrailRequiredForTrade = false;

        Prana.BusinessObjects.CompanyUser _loginUser;

        #region COLUMNS AND CAPTIONS FOR AUDIT GRID
        const string COL_FUNDNAME = "Account Name";
        const string COL_COMPANYUSER = "Company User";
        const string COL_ORDERSIDE = "OrderSide";
        const string CAP_ORDERSIDE = "Order Side";
        const string COL_FUNDID = "FundID";
        const string COL_COMPANYUSERID = "CompanyUserId";
        const string COL_ORDERSIDETAGVALUE = "OrderSideTagValue";
        const string COL_TAXLOTCLOSINGID = "TaxlotClosingId";
        //const string COL_AUDITID = "AuditId";
        const string COL_ACTION = "Action";
        const string COL_ACTIONDATE = "ActionDate";
        const string CAP_ACTIONDATE = "Action Time (UTC)";
        const string COL_GROUPID = "GroupID";
        const string COL_TAXLOTID = "TaxLotID";
        const string COL_ORIGINALVALUE = "OriginalValue";
        const string CAP_ORIGINALVALUE = "Original Value";

        const string COL_NEWVALUE = "NewValue";
        const string CAP_NEWVALUE = "New Value";

        //const string COL_ORIGINALDATE = "OriginalDate";
        const string COL_ActionTime = "ActionTime";
        const string CAP_ActionTime = "Action Time";
        const string COL_SYMBOL = "Symbol";
        const string COL_COMMENT = "Comment";
        const string COL_SOURCE = "Source";
        const string COL_SOURCEID = "SourceID";
        const string COL_CLORDERID = "OrderId";
        const string COL_PARENTCLORDERID = "ParentOrderID";

        //const string COL_
        #endregion
        #region columns and captions COMMON for Taxlots and Groups grid
        const string COL_TRADEATTRIBUTE1 = "TradeAttribute1";
        const string COL_TRADEATTRIBUTE2 = "TradeAttribute2";
        const string COL_TRADEATTRIBUTE3 = "TradeAttribute3";
        const string COL_TRADEATTRIBUTE4 = "TradeAttribute4";
        const string COL_TRADEATTRIBUTE5 = "TradeAttribute5";
        const string COL_TRADEATTRIBUTE6 = "TradeAttribute6";
        const string COL_FXRATE = "FXRate";
        const string COL_FXCONVERSIONMETHODOPERATOR = "FXConversionMethodOperator";
        const string COL_AVGPRICE = "AvgPrice";
        const string COL_ACCRUEDINTEREST = "AccruedInterest";
        const string CAP_TRADEATTRIBUTE1 = "Trade Attribute 1";
        const string CAP_TRADEATTRIBUTE2 = "Trade Attribute 2";
        const string CAP_TRADEATTRIBUTE3 = "Trade Attribute 3";
        const string CAP_TRADEATTRIBUTE4 = "Trade Attribute 4";
        const string CAP_TRADEATTRIBUTE5 = "Trade Attribute 5";
        const string CAP_TRADEATTRIBUTE6 = "Trade Attribute 6";
        const string CAP_FXRATE = "FX Rate";
        const string CAP_FXCONVERSIONMETHODOPERATOR = "Fx Conversion Operator";
        const string CAP_AVGPRICE = "Average Price";
        const string CAP_ACCRUEDINTEREST = "Accrued Interest";
        #endregion
        #region only for groups in lower grid
        const string COL_ORDERTYPE = "Order Type";
        const string COL_ORDERTYPETAGVALUE = "OrderTypeTagValue";
        const string COL_COUNTERPARTY = "Counterparty";
        const string COL_COUNTERPARTYID = "CounterPartyID";
        const string COL_VENUE = "Venue";
        const string COL_VENUEID = "VenueID";
        const string COL_TRADINGACCOUNT = "Trading Account";
        const string COL_TRADINGACCOUNTID = "TradingAccountID";
        const string COL_USER = "User";
        const string COL_USERID = "UserID";
        const string COL_ASSETID = "AssetID";
        const string COL_ASSETCATEGORY = "Asset Category";
        const string COL_UNDERLYINGID = "UnderLyingID";
        const string COL_UNDERLYING = "Underlying";
        const string COL_EXCHANGEID = "ExchangeID";
        const string COL_EXCHANGE = "Exchange";
        const string COL_CURRENCYID = "CurrencyID";
        const string COL_CURRENCY = "Currency";
        const string COL_PROCESSDATE = "ProcessDate";
        const string CAP_PROCESSDATE = "Process Date";
        const string COL_ORIGINALPURCHASEDATE = "OriginalPurchaseDate";
        const string CAP_ORIGINALPURCHASEDATE = "Original Purchase Date";
        const string COL_ALLOCATIONSCHEMEID = "AllocationSchemeID";
        const string COL_ADDITIONALTRADEATTRIBUTES = "AdditionalTradeAttributes";
        #endregion
        #region ONLY FOR TAXLOTS IN LOWER GRID
        const string COL_TAXLOTOPENQTY = "TaxLotOpenQty";
        const string CAP_TAXLOTOPENQTY = "TaxLot Open Quantity";
        const string COL_LEVEL2ID = "Level2ID";
        const string COL_STRATEGY = "Strategy";
        #endregion
        #region hiddencolumns for groups and taxlots
        const string COL_LISTID = "ListID";
        const string COL_ISPRORATAACTIVE = "ISProrataActive";
        const string COL_AUTOGROUPED = "AutoGrouped";
        const string COL_STATEID = "StateID";
        const string COL_ISBASKETGROUP = "IsBasketGroup";
        const string COL_BASKETGROUPID = "BasketGroupID";
        const string COL_ISMANUALGROUP = "IsManualGroup";
        const string COL_ALLOCATIONDATE = "AllocationDate";
        const string COL_ISSWAPPED = "IsSwapped";
        const string COL_TAXLOTCLOSINGID_FK = "TaxLotClosingId_Fk";
        const string COL_MODIFIEDBY = "ModifiedBy";
        const string COL_MODIFIEDDATE = "ModifiedDate";
        const string COL_ISMODIFIED = "IsModified";
        const string COL_COMMISSIONSOURCE = "CommissionSource";
        const string COL_TAXLOTIDSWITHATTRIBUTES = "TaxlotIdsWithAttributes";
        const string COL_LOTID = "LotId";
        const string COL_EXTERNALTRANSID = "ExternalTransId";
        //const string COL_TAXLOTCLOSINGID = "TaxLotClosingId";
        const string COL_PARENTROWPK = "ParentRow_Pk";
        const string COL_POSITIONTAG = "PositionTag";
        const string COL_TIMEOFSAVEUTC = "TimeOfSaveUTC";
        const string COL_AUECMODIFIEDDATE = "AUECModifiedDate";
        #endregion

        /// <summary>
        /// form closed event binded to nirvanamain instance
        /// </summary>
        //public event EventHandler FormClosed;

        string _ignoredUsers;

        #region constructors

        /// <summary>
        /// Constructor draws the default grid structure
        /// </summary>
        public ctrlAuditTrail()
        {
            try
            {
                InitializeComponent();
                // Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();
                _isAllocationAuditTrailRequiredForTrade = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsAllocationAuditTrailRequiredForTrade"));

                var dict = Enum.GetValues(typeof(Prana.BusinessObjects.TradeAuditActionType.ActionSource))
               .Cast<Prana.BusinessObjects.TradeAuditActionType.ActionSource>()
               .ToDictionary(t => (int)t, t => t.ToString());

                //_dictAccounts.Add(1, "Allocation");
                //_dictAccounts.Add(2, "Blotter");
                //_dictAccounts.Add(3, "Closing");

                _multiSelectDropDown1.AddItemsToTheCheckList(dict, CheckState.Checked);
                _multiSelectDropDown1.AdjustCheckListBoxWidth();
                _multiSelectDropDown1.Visible = true;
                _multiSelectDropDown1.TitleText = "Source";
                _multiSelectDropDown1.SetTextEditorText("All");
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
        public void drawsthedefaultgridstructure()
        {
            try
            {
                //if the control is loaded on startup then only the control is to be intialized
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1161
                if (!_isStartUp)
                {
                    return;
                }
                _isStartUp = false;
                //Added By Faisal Shah Dated 01/08/14
                // LoggedInUser was used without initialization. So initialized it                
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1160
                _loginUser = CachedDataManager.GetInstance.LoggedInUser;
                FillIgnoredUsers();
                //CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                GetAndBindAuditUIDataForGroupIds(new List<string>(new string[] { "1" }));
                //_grdTradeAudit.DataSource = new DataTable();
                SetUI();
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
        /// Gets List of ignored users for which the audit trail is not to be shown for the current user
        /// </summary>
        private void FillIgnoredUsers()
        {
            try
            {
                _ignoredUsers = AuditManager.Instance.GetIgnoredUserIds(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
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
        #region initialization
        /// <summary>
        /// Adds the cache to the filters on UI to show autocomplete suggestions
        /// </summary>
        private void AddCacheToFilters()
        {
            try
            {
                _tbAccount.Values = GetSuggestionValueFromCache("accounts");
                _tbSide.Values = GetSuggestionValueFromCache("side");
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
        /// Get data from Cache manager and tagdatabase manager for suggestion box
        /// </summary>
        /// <param name="p">string for selecting case</param>
        /// <returns></returns>
        private string[] GetSuggestionValueFromCache(string p)
        {
            List<String> liReturn = new List<string>();
            try
            {

                switch (p.ToLower())
                {
                    case "accounts":
                        foreach (Account s in CachedDataManager.GetInstance.GetUserAccounts())
                            liReturn.Add(s.Name);
                        break;
                    case "side":
                        foreach (String s in TagDatabaseManager.GetInstance.GetAllOrderSides().Values)
                            liReturn.Add(s);
                        break;
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
            if (liReturn.Count > 0)
                return liReturn.ToArray();

            return null;
        }

        /// <summary>
        /// Sets the ui setting other than the grid
        /// </summary>
        private void SetUI()
        {
            try
            {
                AddCacheToFilters();
                _uccFromDate.Value = DateTime.Now;
                _uccToDate.Value = DateTime.Now;
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

        #region getting and binding data
        /// <summary>
        /// Gets the audit list using audit manager and binds to the grid
        /// </summary>
        /// <param name="groups"></param>
        public void GetAndBindAuditUIDataForGroupIds(List<string> groups)
        {
            try
            {
                StringBuilder accountIds = new StringBuilder();
                if (CachedDataManager.GetInstance.GetUserAccounts().Count != CachedDataManager.GetInstance.GetAllAccountsCount() + 1)
                {
                    foreach (Account fun in CachedDataManager.GetInstance.GetUserAccounts())
                    {
                        accountIds.Append(fun.AccountID);
                        accountIds.Append(',');
                    }
                    accountIds.Append(int.MaxValue);
                }
                if (groups != null && groups.Count > 0)
                {
                    GetAndBindAuditUIDataForGroupIdsAsync(groups, _ignoredUsers, accountIds.ToString().Trim(','));
                }
                else
                {
                    if (_grdTradeAudit.DisplayLayout.Bands[0] == null)
                    {
                        GetAndBindAuditUIDataForGroupIdsAsync(groups, _ignoredUsers, accountIds.ToString().Trim(','));
                    }
                }
                _tbGroupID.Value = String.Join(",", groups.ToArray());
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

        public void GetAndBindAuditUIDataForGroupIdsAsync(List<string> groupIds, string ignoredUsers, string accountIds)
        {
            try
            {
                object[] getDataGroupIdParameters = new object[] { groupIds as object, ignoredUsers as object, accountIds as object };
                this.FindForm().Enabled = false;
                BackgroundWorker getDataAsyncByGroupIds = new BackgroundWorker();
                getDataAsyncByGroupIds.DoWork += new DoWorkEventHandler(getDataAsyncByGroupIds_DoWork);
                getDataAsyncByGroupIds.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getDataAsyncByGroupIds_RunWorkerCompleted);
                getDataAsyncByGroupIds.RunWorkerAsync(getDataGroupIdParameters as object);
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

        void getDataAsyncByGroupIds_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((e.Cancelled == true))
                {
                    MessageBox.Show("Cancelled!", "AuditTrail", MessageBoxButtons.OK);
                }

                else if (!(e.Error == null))
                {
                    MessageBox.Show("Error: " + e.Error.Message, "AuditTrail", MessageBoxButtons.OK);
                }
                else
                {
                    DataTable entries = e.Result as DataTable;
                    BindNewTableToTradeAudit(entries);
                }
                this.FindForm().Enabled = true;
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
            finally
            {
                this.FindForm().Enabled = true;
            }
        }

        void getDataAsyncByGroupIds_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] parameters = e.Argument as object[];
                List<string> groups = (List<string>)parameters[0];
                string ignoredUsers = (string)parameters[1];
                string accountIds = (string)parameters[2];
                DataTable dt = AuditManager.Instance.GetAuditDataByGroupIds(groups, ignoredUsers, accountIds);
                if (!_isAllocationAuditTrailRequiredForTrade)
                {
                    dt = RemoveAuditTrailForTrade(dt);
                }
                e.Result = dt as object;
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

        public DataTable RemoveAuditTrailForTrade(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Comment"].ToString() == EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupCreatedForTrade) || dr["Comment"].ToString() == EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedForTrade))
                        dr.Delete();
                }
                dt.AcceptChanges();
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
            return dt;
        }

        /// <summary>
        /// gets the audit data using audit manager and binds it the grid
        /// </summary>
        /// <param name="from">the from date</param>
        /// <param name="till">the till date</param>
        /// <param name="symbol">the symbol filters</param>
        /// <param name="accountId"></param>
        /// <param name="orderSideTagValue"></param>
        public void GetAndBindAuditUIDataForFilters(AuditTrailFilterParams auditTrailFilterParams)
        {
            try
            {
                auditTrailFilterParams.IgnoredUsers = _ignoredUsers;

                if (String.IsNullOrEmpty(auditTrailFilterParams.AccountIDs))
                {
                    if (CachedDataManager.GetInstance.GetUserAccounts().Count == CachedDataManager.GetInstance.GetAllAccountsCount() + 1 || _tbAccount.Value == null)
                    {
                        GetAndBindAuditUIDataForFiltersAsync(auditTrailFilterParams);
                    }
                    else
                    {
                        StringBuilder tempAccountIds = new StringBuilder();
                        foreach (Account fun in CachedDataManager.GetInstance.GetUserAccounts())
                        {
                            tempAccountIds.Append(fun.AccountID);
                            tempAccountIds.Append(',');
                        }
                        auditTrailFilterParams.AccountIDs = tempAccountIds.ToString().Trim(',');
                        GetAndBindAuditUIDataForFiltersAsync(auditTrailFilterParams);
                    }
                }
                else
                {
                    GetAndBindAuditUIDataForFiltersAsync(auditTrailFilterParams);
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


        void GetAndBindAuditUIDataForFiltersAsync(AuditTrailFilterParams auditTrailFilterParams)
        {
            try
            {
                //  Object[] parameterFetchByDate = new object[] { auditTrailFilterParams };
                this.FindForm().Enabled = false;
                BackgroundWorker getDataAsyncByDate = new BackgroundWorker();
                getDataAsyncByDate.DoWork += new DoWorkEventHandler(getDataAsyncByDate_DoWork);
                getDataAsyncByDate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getDataAsyncByDate_RunWorkerCompleted);
                getDataAsyncByDate.RunWorkerAsync(auditTrailFilterParams);
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

        void getDataAsyncByDate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((e.Cancelled == true))
                {
                    MessageBox.Show("Cancelled!", "AuditTrail", MessageBoxButtons.OK);
                }

                else if (!(e.Error == null))
                {
                    MessageBox.Show("Error: " + e.Error.Message, "AuditTrail", MessageBoxButtons.OK);
                }
                else
                {
                    DataTable entries = e.Result as DataTable;
                    BindNewTableToTradeAudit(entries);
                }
                this.FindForm().Enabled = true;
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
            finally
            {
                this.FindForm().Enabled = true;
            }
        }

        void getDataAsyncByDate_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var parametersFetchByDate = e.Argument as AuditTrailFilterParams;
                DataTable entries = AuditManager.Instance.GetAuditUIDataByDate(parametersFetchByDate);
                if (!_isAllocationAuditTrailRequiredForTrade)
                {
                    entries = RemoveAuditTrailForTrade(entries);
                }
                e.Result = (object)entries;
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

        private delegate void DataSourceUpdated(DataTable dt);

        private void SetDataSource(DataTable dt)
        {
            try
            {
                this._grdTradeAudit.DataSource = dt;
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
        /// Binds the new table to the grid and also formats all the columns and grid according to needs
        /// </summary>
        /// <param name="entries">datatable which contains entries for the audit</param>
        public void BindNewTableToTradeAudit(DataTable entries)
        {
            try
            {
                entries.Columns.Add(COL_FUNDNAME, typeof(string));
                Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetAccounts();
                entries.Columns.Add(COL_COMPANYUSER, typeof(string));
                Dictionary<int, string> dictUsers = CachedDataManager.GetInstance.GetAllUsersName();
                entries.Columns.Add(COL_ORDERSIDE, typeof(string));
                Dictionary<string, string> dictSides = TagDatabaseManager.GetInstance.GetAllOrderSides();
                if (!entries.Columns.Contains(COL_SOURCE))
                    entries.Columns.Add(COL_SOURCE, typeof(string));

                if (!entries.Columns.Contains(COL_ActionTime))
                    entries.Columns.Add(COL_ActionTime, typeof(string));
                foreach (DataRow dr in entries.Rows)
                {
                    if (dr[COL_FUNDID].Equals(int.MaxValue) && dr[COL_SOURCEID].Equals("1"))
                    {
                        dr[COL_FUNDNAME] = "Multiple";
                    }
                    else if (dr[COL_FUNDID] != DBNull.Value)
                    {
                        if (dictAccounts.ContainsKey(int.Parse(dr[COL_FUNDID].ToString().Trim())))
                        {
                            dr[COL_FUNDNAME] = dictAccounts[int.Parse(dr[COL_FUNDID].ToString().Trim())];
                        }
                    }
                    if (dr[COL_COMPANYUSERID] != DBNull.Value)
                    {
                        if (dictUsers.ContainsKey(int.Parse(dr[COL_COMPANYUSERID].ToString().Trim())))
                        {
                            dr[COL_COMPANYUSER] = dictUsers[int.Parse(dr[COL_COMPANYUSERID].ToString().Trim())];
                        }
                    }
                    if (dr[COL_ORDERSIDETAGVALUE] != DBNull.Value)
                    {
                        if (dictSides.ContainsKey(dr[COL_ORDERSIDETAGVALUE].ToString().Trim()))
                        {
                            dr[COL_ORDERSIDE] = dictSides[dr[COL_ORDERSIDETAGVALUE].ToString().Trim()];
                        }
                    }

                    Prana.BusinessObjects.TradeAuditActionType.ActionSource source = TradeAuditActionType.ActionSource.None;
                    if (dr[COL_SOURCEID] != DBNull.Value)
                    {
                        source = ((Prana.BusinessObjects.TradeAuditActionType.ActionSource)int.Parse(dr[COL_SOURCEID].ToString().Trim()));
                    }
                    dr[COL_SOURCE] = source.ToString();
                    dr[COL_ActionTime] = Convert.ToDateTime(dr[COL_ACTIONDATE].ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                }
                //_grdTradeAudit.DataSource = entries;


                //Marshaling 
                if (UIValidation.GetInstance().validate(_grdTradeAudit))
                {
                    if (_grdTradeAudit.InvokeRequired)
                    {
                        DataSourceUpdated mi = new DataSourceUpdated(SetDataSource);
                        _grdTradeAudit.BeginInvoke(mi, new Object[] { entries });
                    }
                    else
                    {
                        SetDataSource(entries);
                    }
                }

                _grdTradeAudit.ContextMenuStrip = _grdTradeAuditContextMenu;
                if (!LoadAuditLayout())
                {
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Ascending;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_GROUPID].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Ascending;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_TAXLOTID].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Ascending;


                    #region column ordering default
                    //ordering should be sequential from 0 onwards
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].Header.VisiblePosition = 0;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ActionTime].Header.VisiblePosition = 1;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_SYMBOL].Header.VisiblePosition = 1;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORDERSIDE].Header.VisiblePosition = 2;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_FUNDNAME].Header.VisiblePosition = 3;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTION].Header.VisiblePosition = 4;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORIGINALVALUE].Header.VisiblePosition = 5;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_NEWVALUE].Header.VisiblePosition = 6;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_COMMENT].Header.VisiblePosition = 7;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_COMPANYUSER].Header.VisiblePosition = 8;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_TAXLOTID].Header.VisiblePosition = 9;
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_GROUPID].Header.VisiblePosition = 10;
                    #endregion
                }
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORDERSIDETAGVALUE].Hidden = true;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORDERSIDETAGVALUE].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_SOURCEID].Hidden = true;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_SOURCEID].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_COMPANYUSERID].Hidden = true;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_COMPANYUSERID].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID].Hidden = true;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_FUNDID].Hidden = true;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_FUNDID].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                //  //if (_grdTradeAudit.DisplayLayout.Bands[0].Columns.Exists(COL_ACTION))
                //{
                //    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTION].Editor = new EditorWithText();
                //    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTION].Editor.DataFilter = new TradeAuditActionTypeDataFilter();
                //}
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].Format = "MM/dd/yyyy HH:mm:ss";
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].GroupByMode = GroupByMode.Date;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].Header.Caption = CAP_ACTIONDATE;
                _grdTradeAudit.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;
                _grdTradeAudit.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                _grdTradeAudit.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                _grdTradeAudit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORDERSIDE].Header.Caption = CAP_ORDERSIDE;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ORIGINALVALUE].Header.Caption = CAP_ORIGINALVALUE;
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_NEWVALUE].Header.Caption = CAP_NEWVALUE;

                if (_grdTradeAudit.DisplayLayout.Bands[0].Columns.Exists(COL_ActionTime))
                {
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ActionTime].Header.Caption = CAP_ActionTime;
                }
                if (_grdTradeAudit.DisplayLayout.Bands[0].Columns.Exists(COL_ACTION))
                {
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTION].Editor = new EditorWithText();
                    _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTION].Editor.DataFilter = new TradeAuditActionTypeDataFilter();
                }
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].Editor = new DateTimeEditor();
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_ACTIONDATE].Editor.DataFilter = new UtcToLocalConversionDataFilter();

                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_SOURCE].Editor = new EditorWithText();
                _grdTradeAudit.DisplayLayout.Bands[0].Columns[COL_SOURCE].Editor.DataFilter = new UtcToLocalConversionDataFilter();

                _grdGroupsTaxlots.DataSource = null;
                _grdGroupsTaxlots.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.None;
                //_grdGroupsTaxlots.ResetDisplayLayout();
                //_grdGroupsTaxlots.Layouts.Clear();
                _grdTradeAudit.Refresh();
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

        #region events
        #region buttonclicks
        /// <summary>
        /// Get Data button click handler gets the data and shows it on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tbGroupID.Value == null)
                {
                    DateTime from = (DateTime)_uccFromDate.Value;
                    DateTime till = (DateTime)_uccToDate.Value;
                    String symbol = null;
                    StringBuilder accountIds = new StringBuilder();
                    StringBuilder orderSideTagValues = new StringBuilder();
                    if (_tbSymbol.Value != null && !String.IsNullOrEmpty(_tbSymbol.Value.ToString()))
                    {
                        symbol = _tbSymbol.Value.ToString();
                    }
                    if (_tbAccount.Value != null && !String.IsNullOrEmpty(_tbAccount.Value.ToString()))
                    {
                        Dictionary<int, string> accountCollection = CachedDataManager.GetInstance.GetAccounts();
                        foreach (string stAccount in _tbAccount.Value.ToString().Split(','))
                        {
                            foreach (KeyValuePair<int, string> kvp in accountCollection)
                            {
                                if (String.Compare(kvp.Value, stAccount, true) == 0)
                                {
                                    accountIds.Append(kvp.Key);
                                    accountIds.Append(',');
                                    break;
                                }
                            }
                        }
                    }
                    else if (_tbAccount.Value != null)
                    {
                        AccountCollection accountColl = CachedDataManager.GetInstance.GetUserAccounts();
                        if (accountColl.Count != CachedDataManager.GetInstance.GetAllAccountsCount() + 1)
                        {
                            foreach (Account f in accountColl)
                            {
                                accountIds.Append(f.AccountID);
                                accountIds.Append(',');
                            }
                        }
                    }
                    if (_tbSide.Value != null && !String.IsNullOrEmpty(_tbSide.Value.ToString()))
                    {
                        Dictionary<string, string> orderSides = TagDatabaseManager.GetInstance.GetAllOrderSides();
                        foreach (string stOrderSide in _tbSide.Value.ToString().Split(','))
                        {
                            foreach (KeyValuePair<string, string> kvp in orderSides)
                            {
                                if (String.Compare(kvp.Value, stOrderSide, true) == 0)
                                {
                                    orderSideTagValues.Append(kvp.Key);
                                    orderSideTagValues.Append(',');
                                    break;
                                }
                            }
                        }
                    }

                    var selectedSourcesDict = _multiSelectDropDown1.GetSelectedItemsInDictionary();
                    var selectedSources = string.Join(",", selectedSourcesDict.Keys.ToArray());

                    string tempaccountids = "";
                    string tempOrderSides = "";
                    if (accountIds != null)
                    {
                        tempaccountids = accountIds.ToString().TrimEnd(',');
                    }
                    if (orderSideTagValues != null)
                    {
                        tempOrderSides = orderSideTagValues.ToString().TrimEnd(',');
                    }

                    var auditFilterParams = new AuditTrailFilterParams()
                    {
                        AccountIDs = tempaccountids,
                        FromDate = from,
                        ToDate = till,
                        Symbol = symbol,
                        OrderSides = tempOrderSides,
                        SourceIDs = selectedSources,
                        IgnoredUsers = _ignoredUsers
                    };
                    GetAndBindAuditUIDataForFilters(auditFilterParams);


                    //GetAuditClick(this, new LaunchFormEventArgs(new KeyValuePair<string, object[]>("filters",new object[]{ from, till, symbol, accountId, orderSideTagValue })));
                }
                else
                {
                    GetAndBindAuditUIDataForGroupIds(new List<string>(_tbGroupID.Value.ToString().Split(',')));
                    //GetAuditClick(this,new LaunchFormEventArgs(new KeyValuePair<string, object[]>("groupids",new object[] { new List<string>(new string[] { _tbGroupID.Value.ToString() }) })));
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
        /// Handling of export on click of export button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog svFile = new SaveFileDialog();
            try
            {
                if (_grdTradeAudit.Rows.Count == 0)
                {
                    MessageBox.Show("No rows to export");
                    return;
                }

                svFile.Filter = "Excel File|*.xls";
                svFile.Title = "Export Audit Trail to an Excel File";
                if (svFile.ShowDialog() == DialogResult.OK)
                {
                    if (svFile.FileName != "")
                    {
                        if (Path.GetExtension(svFile.FileName) == ".xls")
                        {
                            ultraGridExcelExporter1.Export(this._grdTradeAudit, svFile.FileName);
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
        /// Handling of screenshot press button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btScreenShot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this.ParentForm);
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
        #region otherevents
        /// <summary>
        /// event to erase the filter boxes when a groupid is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tbGroupID_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_loading)
                {
                    _loading = true;
                    if (_tbSide.Value != null)
                        _tbSide.Value = null;
                    if (_tbSymbol.Value != null)
                        _tbSymbol.Value = "";
                    if (_tbAccount.Value != null)
                        _tbAccount.Value = "";
                }
                else
                {
                    _loading = false;
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
        /// event to clear the groupId filter when any field in other filters is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filters_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_loading)
                {
                    _loading = true;
                    if (_tbGroupID.Value != null)
                        _tbGroupID.Value = null;
                }
                else
                {
                    _loading = false;
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
        /// form closing event calls the form closed event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void TradeAuditUI_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    try
        //    {
        //        FormClosed(this, null);
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
        #endregion

        private void _btClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                _grdTradeAudit.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                //_tbAccount.Value = null;
                //_tbGroupID.Value = null;
                //_tbSide.Value = null;
                //_tbSymbol.Value = null;
                //_uccFromDate.Value = DateTime.Now;
                //_uccToDate.Value = DateTime.Now;
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

        private void _tbSymbol_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.KeyChar = Char.ToUpper(e.KeyChar);
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

        private void _grdTradeAudit_AfterExitEditMode(object sender, EventArgs e)
        {
            try
            {
                _grdTradeAuditContextMenu.Items.Remove(_grdTradeAuditMenuItemCopy);
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

        private void _grdTradeAudit_BeforeEnterEditMode(object sender, CancelEventArgs e)
        {
            try
            {
                _grdTradeAuditContextMenu.Items.Add(_grdTradeAuditMenuItemCopy);
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

        private void _grdMenuItemCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_grdTradeAudit.ActiveCell.Text))
                {
                    Clipboard.SetText(_grdTradeAudit.ActiveCell.Text);
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
        /// handles the showing of correct context menu in the audit grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _grdTradeAudit_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    _grdTradeAuditContextMenu.Hide();
                    Point point = new Point(e.X, e.Y);
                    UIElement uiElement = null;
                    if (sender != null)
                    {
                        UltraGridBase ugBase = sender as UltraGridBase;
                        if (ugBase != null)
                            uiElement = ugBase.DisplayLayout.UIElement.ElementFromPoint(point);
                    }
                    UltraGridRow row = null;
                    if (uiElement != null)
                    {
                        row = (UltraGridRow)uiElement.GetContext(typeof(UltraGridRow));
                    }
                    _grdTradeAuditContextMenu.Items.Clear();
                    if (row != null)
                    {
                        _grdTradeAuditContextMenu.Items.Add(_grdTradeAuditDetailsToolStripMenuItem);
                        _grdTradeAuditContextMenu.Items.Add(_grdAuditSaveLayoutToolStripMenuItem);
                        _grdTradeAudit.ActiveRow = row;
                        _grdTradeAuditContextMenu.Show();
                    }
                    else
                    {
                        _grdTradeAuditContextMenu.Items.Add(_grdAuditSaveLayoutToolStripMenuItem);
                        _grdTradeAuditContextMenu.Show();
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
        /// gets the full deleted or current taxlot/group for the selected item and binds it to the lower grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow row = _grdTradeAudit.ActiveRow;
                string groupId = ((UltraGridRow)row).GetCellValue(COL_GROUPID).ToString();
                string taxlotId = ((UltraGridRow)row).GetCellValue(COL_TAXLOTID).ToString();
                string parentOrderID = ((UltraGridRow)row).GetCellValue(COL_PARENTCLORDERID).ToString();
                string clOrderId = ((UltraGridRow)row).GetCellValue(COL_CLORDERID).ToString();

                if (!String.IsNullOrEmpty(groupId) || !String.IsNullOrEmpty(taxlotId))
                {
                    //string auditId = ((UltraGridRow)row).GetCellValue(COL_AUDITID).ToString();
                    DataTable taxlotGroup = GetGroupTaxlotForIds(groupId, taxlotId);
                    BindNewTableToGrdGroupsTaxlot(taxlotGroup);
                }
                else if (!String.IsNullOrEmpty(parentOrderID) || !String.IsNullOrEmpty(clOrderId))
                {

                    DataTable data = GetOrderDetailsForIds(parentOrderID, clOrderId);
                    _grdGroupsTaxlots.DataSource = data;
                    SetupTradeAttributesInGrid(data);
                    _grdGroupsTaxlots.Refresh();
                    _grdGroupsTaxlots.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand);
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
        /// Hides the AdditionalTradeAttributes column, excludes it from the column chooser,
        /// adds missing TradeAttribute columns to the DataTable, and populates rows with values from JSON.
        /// </summary>
        /// <param name="grid">The UltraGrid displaying the data.</param>
        private void SetupTradeAttributesInGrid(DataTable data)
        {           
            // Hide the AdditionalTradeAttributes column and exclude it from column chooser
            _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ADDITIONALTRADEATTRIBUTES].Hidden = true;
            _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ADDITIONALTRADEATTRIBUTES].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            // Add TradeAttribute7 to TradeAttribute45 columns if missing
            for (int i = 7; i <= 45; i++)
            {
                string colName = $"TradeAttribute{i}";
                if (!data.Columns.Contains(colName))
                {
                    data.Columns.Add(new DataColumn(colName)
                    {
                        Caption = $"Trade Attribute {i}"
                    });
                }
            }

            // Populate each DataRow with values parsed from the JSON in the specified column
            foreach (DataRow dataRow in data.Rows)
            {
                string json = dataRow[COL_ADDITIONALTRADEATTRIBUTES]?.ToString();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    JArray array = JArray.Parse(json);
                    foreach (JObject obj in array)
                    {
                        string name = (string)obj["Name"];
                        if (!string.IsNullOrEmpty(name) && dataRow.Table.Columns.Contains(name))
                        {
                            dataRow[name] = (string)obj["Value"] ?? "";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Order Details For Ids
        /// </summary>
        /// <param name="parentOrderID"></param>
        /// <param name="clOrderId"></param>
        /// <returns></returns>
        private DataTable GetOrderDetailsForIds(string parentOrderID, string clOrderId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = AuditManager.Instance.GetOrderDetailsByIds(parentOrderID, clOrderId);

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
            return dt;
        }


        /// <summary>
        /// binds the datatable to the lower grid and formats the grid
        /// </summary>
        /// <param name="taxlotGroup"></param>
        private void BindNewTableToGrdGroupsTaxlot(DataTable taxlotGroup)
        {
            try
            {
                _grdGroupsTaxlots.DataSource = taxlotGroup;
                _grdGroupsTaxlots.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;
                _grdGroupsTaxlots.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                #region Common between taxlot and groups type
                #region columns with data transformation
                if (!taxlotGroup.Columns.Contains(COL_ORDERSIDE))
                    taxlotGroup.Columns.Add(COL_ORDERSIDE, typeof(string));
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ORDERSIDETAGVALUE].Hidden = true;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ORDERSIDETAGVALUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                #endregion
                #region hidden columns
                //_grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUDITID].Hidden = true;
                //_grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUDITID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                #endregion
                #region columns with different captions
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE1].Header.Caption = CAP_TRADEATTRIBUTE1;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE2].Header.Caption = CAP_TRADEATTRIBUTE2;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE3].Header.Caption = CAP_TRADEATTRIBUTE3;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE4].Header.Caption = CAP_TRADEATTRIBUTE4;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE5].Header.Caption = CAP_TRADEATTRIBUTE5;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADEATTRIBUTE6].Header.Caption = CAP_TRADEATTRIBUTE6;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_FXRATE].Header.Caption = CAP_FXRATE;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_FXCONVERSIONMETHODOPERATOR].Header.Caption = CAP_FXCONVERSIONMETHODOPERATOR;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AVGPRICE].Header.Caption = CAP_AVGPRICE;
                _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ACCRUEDINTEREST].Header.Caption = CAP_ACCRUEDINTEREST;
                #endregion
                #endregion
                SetupTradeAttributesInGrid(taxlotGroup);
                if (taxlotGroup.Columns[COL_TAXLOTID] == null)
                {
                    #region columns with data transformation
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ORDERTYPETAGVALUE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ORDERTYPETAGVALUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_ORDERTYPE))
                        taxlotGroup.Columns.Add(COL_ORDERTYPE, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_COUNTERPARTYID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_COUNTERPARTYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_COUNTERPARTY))
                        taxlotGroup.Columns.Add(COL_COUNTERPARTY, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_VENUEID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_VENUEID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_VENUE))
                        taxlotGroup.Columns.Add(COL_VENUE, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADINGACCOUNTID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TRADINGACCOUNTID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_TRADINGACCOUNT))
                        taxlotGroup.Columns.Add(COL_TRADINGACCOUNT, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_USERID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_USERID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_USER))
                        taxlotGroup.Columns.Add(COL_USER, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ASSETID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ASSETID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_ASSETCATEGORY))
                        taxlotGroup.Columns.Add(COL_ASSETCATEGORY, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_UNDERLYINGID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_UNDERLYINGID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_UNDERLYING))
                        taxlotGroup.Columns.Add(COL_UNDERLYING, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_EXCHANGEID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_EXCHANGEID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_EXCHANGE))
                        taxlotGroup.Columns.Add(COL_EXCHANGE, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_CURRENCYID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_CURRENCYID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_CURRENCY))
                        taxlotGroup.Columns.Add(COL_CURRENCY, typeof(string));
                    #endregion
                    #region "COLUMNS WITH DIFFERENT CAPTIONS"
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_OTHERBROKERFEES].Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE].Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_STAMPDUTY].Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TRANSACTIONLEVY].Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_CLEARINGFEE].Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS].Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_MISCFEES].Header.Caption = OrderFields.CAPTION_MISCFEES;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_SECFEE].Header.Caption = OrderFields.CAPTION_SECFEE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_OCCFEE].Header.Caption = OrderFields.CAPTION_OCCFEE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[OrderFields.PROPERTY_ORFFEE].Header.Caption = OrderFields.CAPTION_ORFFEE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_PROCESSDATE].Header.Caption = CAP_PROCESSDATE;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ORIGINALPURCHASEDATE].Header.Caption = CAP_ORIGINALPURCHASEDATE;
                    #endregion
                    #region HIDDENCOLUMNS
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LISTID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LISTID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISPRORATAACTIVE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISPRORATAACTIVE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUTOGROUPED].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUTOGROUPED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_STATEID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_STATEID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISBASKETGROUP].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISBASKETGROUP].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_BASKETGROUPID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_BASKETGROUPID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISMANUALGROUP].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISMANUALGROUP].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ALLOCATIONDATE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ALLOCATIONDATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISSWAPPED].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISSWAPPED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID_FK].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID_FK].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_MODIFIEDBY].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_MODIFIEDBY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_MODIFIEDDATE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_MODIFIEDDATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISMODIFIED].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ISMODIFIED].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_COMMISSIONSOURCE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_COMMISSIONSOURCE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTIDSWITHATTRIBUTES].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTIDSWITHATTRIBUTES].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ALLOCATIONSCHEMEID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_ALLOCATIONSCHEMEID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    #endregion
                    foreach (DataRow row in taxlotGroup.Rows)
                    {
                        row[COL_ORDERSIDE] = TagDatabaseManager.GetInstance.GetOrderSideText(row[COL_ORDERSIDETAGVALUE].ToString());
                        row[COL_ORDERTYPE] = TagDatabaseManager.GetInstance.GetOrderTypeText(row[COL_ORDERTYPETAGVALUE].ToString());
                        row[COL_COUNTERPARTY] = CachedDataManager.GetInstance.GetCounterPartyText((int)row[COL_COUNTERPARTYID]);
                        row[COL_VENUE] = CachedDataManager.GetInstance.GetVenueText((int)row[COL_VENUEID]);
                        row[COL_TRADINGACCOUNT] = CachedDataManager.GetInstance.GetTradingAccountText((int)row[COL_TRADINGACCOUNTID]);
                        row[COL_USER] = CachedDataManager.GetInstance.GetUserText((int)row[COL_USERID]);
                        row[COL_ASSETCATEGORY] = CachedDataManager.GetInstance.GetAssetText((int)row[COL_ASSETID]);
                        row[COL_UNDERLYING] = CachedDataManager.GetInstance.GetUnderLyingText((int)row[COL_UNDERLYINGID]);
                        row[COL_EXCHANGE] = CachedDataManager.GetInstance.GetExchangeText((int)row[COL_EXCHANGEID]);
                        row[COL_CURRENCY] = CachedDataManager.GetInstance.GetCurrencyText((int)row[COL_CURRENCYID]);
                    }
                    _grdGroupsTaxlots.DataSource = taxlotGroup;
                    _grdGroupsTaxlots.Refresh();
                    _grdGroupsTaxlots.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand);
                }
                else
                {
                    #region columns with data transformation
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_FUNDID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_FUNDID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_FUNDNAME))
                        taxlotGroup.Columns.Add(COL_FUNDNAME, typeof(string));
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LEVEL2ID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LEVEL2ID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    if (!taxlotGroup.Columns.Contains(COL_STRATEGY))
                        taxlotGroup.Columns.Add(COL_STRATEGY, typeof(string));
                    #endregion
                    #region columns different captions
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTOPENQTY].Header.Caption = CAP_TAXLOTOPENQTY;
                    #endregion
                    #region hidden columns
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LOTID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_LOTID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_EXTERNALTRANSID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_EXTERNALTRANSID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TAXLOTCLOSINGID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_PARENTROWPK].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_PARENTROWPK].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_POSITIONTAG].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_POSITIONTAG].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TIMEOFSAVEUTC].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_TIMEOFSAVEUTC].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUECMODIFIEDDATE].Hidden = true;
                    _grdGroupsTaxlots.DisplayLayout.Bands[0].Columns[COL_AUECMODIFIEDDATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    #endregion
                    foreach (DataRow row in taxlotGroup.Rows)
                    {
                        row[COL_ORDERSIDE] = TagDatabaseManager.GetInstance.GetOrderSideText(row[COL_ORDERSIDETAGVALUE].ToString());
                        row[COL_FUNDNAME] = CachedDataManager.GetInstance.GetAccountText((int)row[COL_FUNDID]);
                        row[COL_STRATEGY] = CachedDataManager.GetInstance.GetStrategyText((int)row[COL_LEVEL2ID]);
                    }
                    _grdGroupsTaxlots.DataSource = taxlotGroup;
                    _grdGroupsTaxlots.Refresh();
                    _grdGroupsTaxlots.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand);
                }
                //
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
        /// gets the data table which may contain group or taxlot according to the ids
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="taxlotId"></param>
        /// <param name="auditId"></param>
        /// <returns></returns>
        private DataTable GetGroupTaxlotForIds(string groupId, string taxlotId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = AuditManager.Instance.GetGroupTaxlotForIds(groupId, taxlotId);
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
            return dt;
        }

        /// <summary>
        /// colours the rows background according to the type whether deleted or latest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _grdGroupsTaxlots_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists(COL_GROUPID))
                {
                    if (String.IsNullOrEmpty(e.Row.GetCellValue(COL_GROUPID).ToString()))
                    {
                        e.Row.Appearance.BackColor = Color.DarkGreen;
                    }
                    else
                    {
                        e.Row.Appearance.BackColor = Color.DarkRed;
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

        /// <summary>
        /// adds the copy menu item when user enters readonly edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _grdGroupsTaxlots_BeforeEnterEditMode(object sender, CancelEventArgs e)
        {
            try
            {
                if (_grdGroupsTaxlots.ContextMenuStrip == null)
                    _grdGroupsTaxlots.ContextMenuStrip = _grdGroupsTaxlotsContextMenuStrip;
                _grdGroupsTaxlots.ContextMenuStrip.Items.Add(_grdTaxlotGroupCopyMenuItem);
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
        /// removes the copy option from the context menu when user exits readonly edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _grdGroupsTaxlots_AfterExitEditMode(object sender, EventArgs e)
        {
            try
            {
                if (_grdGroupsTaxlots.ContextMenuStrip == null)
                    _grdGroupsTaxlots.ContextMenuStrip = _grdGroupsTaxlotsContextMenuStrip;
                _grdGroupsTaxlots.ContextMenuStrip.Items.Clear();
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
        /// copies the current cell value to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _grdTaxlotGroupCopyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(_grdGroupsTaxlots.ActiveCell.Text);
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

        private void _grdAuditSaveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string auditPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();
                if (!Directory.Exists(auditPreferencesPath))
                {
                    Directory.CreateDirectory(auditPreferencesPath);
                }

                string auditPrefFile = auditPreferencesPath + "\\" + "AuditGridLayout.xml";
                this._grdTradeAudit.DisplayLayout.SaveAsXml(auditPrefFile, PropertyCategories.All);

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

        private bool LoadAuditLayout()
        {
            if (_loginUser != null)
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string auditPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();
                string auditPrefFile = auditPreferencesPath + "\\" + "AuditGridLayout.xml";
                if (File.Exists(auditPrefFile))
                {
                    UltraGridLayout lay = new UltraGridLayout();
                    try
                    {
                        lay.LoadFromXml(auditPrefFile, PropertyCategories.All);

                        _grdTradeAudit.DisplayLayout.Load(lay, PropertyCategories.All);
                    }
                    catch (Exception e)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        Exception ex = new Exception("Error while loading layout for the Grid. This error can generally be resolved by removing the preferences file OR by saving the layout again.", e);
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal void setLoginUser(CompanyUser loginUser)
        {
            try
            {
                _loginUser = loginUser;
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

        private void ctrlAuditTrail_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void SetButtonsColor()
        {

            try
            {
                _btExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                _btExport.ForeColor = System.Drawing.Color.White;
                _btExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _btExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                _btExport.UseAppStyling = false;
                _btExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                _btScreenShot.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                _btScreenShot.ForeColor = System.Drawing.Color.White;
                _btScreenShot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _btScreenShot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                _btScreenShot.UseAppStyling = false;
                _btScreenShot.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                _btClearFilters.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                _btClearFilters.ForeColor = System.Drawing.Color.White;
                _btClearFilters.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _btClearFilters.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                _btClearFilters.UseAppStyling = false;
                _btClearFilters.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                _btGetData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                _btGetData.ForeColor = System.Drawing.Color.White;
                _btGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _btGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                _btGetData.UseAppStyling = false;
                _btGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void _grdTradeAudit_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this._grdTradeAudit);
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


        private void _grdGroupsTaxlots_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this._grdGroupsTaxlots);
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

        private void _grdTradeAudit_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void _grdGroupsTaxlots_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }

    #region UtcToLocalConversionDataFilter class
    public sealed class UtcToLocalConversionDataFilter : IEditorDataFilter
    {
        #region IEditorDataFilter Members

        object IEditorDataFilter.Convert(EditorDataFilterConvertArgs conversionArgs)
        {
            ConversionDirection direction = conversionArgs.Direction;
            UltraGridCell cell = conversionArgs.Context as UltraGridCell;
            DateTime value = conversionArgs.IsValid && conversionArgs.Value is DateTime ? (DateTime)conversionArgs.Value : DateTime.MinValue;

            if (cell == null || value == DateTime.MinValue)
                return conversionArgs.Value;

            switch (direction)
            {
                case ConversionDirection.EditorToOwner:
                    {
                        //  When the value is going from the editor back to
                        //  the cell, convert it to universal time.
                        //System.Diagnostics.Debug.WriteLine("Converting " + value.ToLongTimeString() + " to " + value.ToUniversalTime().ToLongTimeString());
                        conversionArgs.Handled = true;
                        return value.ToLocalTime();
                    }
                case ConversionDirection.OwnerToEditor:
                    {
                        //  When the value is going from the cell to
                        //  the editor, convert it to local time.
                        //System.Diagnostics.Debug.WriteLine("Converting " + value.ToLongTimeString() + " to " + value.ToLocalTime().ToLongTimeString());
                        conversionArgs.Handled = true;
                        return value.ToUniversalTime();
                    }
            }
            return conversionArgs.Value;
        }

        #endregion
    }
    #endregion UtcToLocalConversionDataFilter class
}