using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.TradingTicket.TTView;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.TradingTicket.TTPresenter
{
    /// <summary>
    /// http://www.dreamincode.net/forums/topic/342849-introducing-mvp-model-view-presenter-pattern-winforms/ (We have used this in MTT)
    /// https://www.codeproject.com/Articles/31210/Model-View-Presenter
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Prana.Interfaces.ILiveFeedCallback" />
    internal class MTTPresenter : IDisposable, ILiveFeedCallback
    {
        public EventHandler PriceForComplianceNotAvailable;
        /// <summary>
        /// Used for getting live-feed data
        /// </summary>
        public DuplexProxyBase<IPricingService> _pricingServicesProxy;
        private readonly IMultiTradingTicketView iMultiTradingTicketView;
        private ProxyBase<IAllocationManager> _allocationProxy;
        private Dictionary<int, string> customSchemes = null;
        private Dictionary<int, string> prefs = null;
        private Dictionary<Tuple<int, int>, ValueList> auecAccountWiseCP = new Dictionary<Tuple<int, int>, ValueList>();
        private Dictionary<int, string> allocationPrefs = null;
        private System.Threading.Timer timer = null;

        private TradingTicketType _tradingTicketType = TradingTicketType.Manual;
        private Boolean _isPricingAvailable = false;

        /// <summary>
        /// This will be a unique id generated when each time the user clicks on the trade button of the multi-trade form
        /// </summary>
        private String MultiTradeId = "";

        public ProxyBase<IAllocationManager> AllocationProxy
        {
            get { return _allocationProxy; }
        }

        public MTTPresenter(IMultiTradingTicketView mttView)
        {
            this.iMultiTradingTicketView = mttView;
            Initialize();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void LiveFeedConnected()
        {
        }

        public void LiveFeedDisConnected()
        {
        }

        /// <summary>
        /// Snapshots the response.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data != null && data.LastPrice == 0 && data.Ask == 0 && data.Bid == 0)
                    _isPricingAvailable = false;
                else
                    _isPricingAvailable = true;

                if (timer != null)
                    timer.Change(3000, Timeout.Infinite);
                iMultiTradingTicketView.SnapshotResponse(data);
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

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_allocationProxy != null)
                    {
                        _allocationProxy.Dispose();
                    }
                    if (_pricingServicesProxy != null)
                    {
                        _pricingServicesProxy.Dispose();
                    }
                    if(timer != null)
                    {
                        timer.Dispose();
                    }
                    if (_vls != null)
                    {
                        foreach (BindableValueList v in _vls)
                        {
                            if (v != null)
                            {
                                v.DataSource = null;
                                v.Dispose();
                            }
                        }
                        _vls = null;
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
        /// Binds all ComboBox on MTT.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BindAllComboBox(object sender, EventArgs e)
        {
            try
            {
                Parallel.Invoke(
                    () => BindAccountBrokerVenueComboBoxes(),
                    () => BindMiscComboBoxes()
                    );
                iMultiTradingTicketView.SetStatusBarMessage("4.Applying theme.");
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

        private void BindAccountBrokerVenueComboBoxes()
        {
            iMultiTradingTicketView.SetStatusBarMessage("1.Mapping Accounts.");
            BindDataInAccountColumn();
            iMultiTradingTicketView.SetStatusBarMessage("2.Mapping Brokers.");
            BindDataInBrokerColumn();
            iMultiTradingTicketView.SetStatusBarMessage("3.Mapping Venues.");
            BindDataInVenueColumn();
            iMultiTradingTicketView.SetStatusBarMessage("4.Applying theme.");
        }
        private void BindMiscComboBoxes()
        {
            BindDataInOrderTypeColumn();
            BindDataInStrategyColumn();
            BindDataInTIFColumn();
            SetTradeAttributeCaption();
            BindTradeAttribute();
            BindDataInSettlementCurrency();
            BindDataInExecutionInstructions();
            BindDataInHandlingInstructions();
            BindDataInTradingAccountCombo();
            BindDataInSettlementFxOperator();
            BindCommissionBasis();
            BindBrokerBulkCombo();
            SetIncrementValues();
            BindDataInOrderSideColumn();
        }

        private void BindCommissionBasis()
        {
            try
            {
                IList list = EnumHelper.ToList(typeof(CalculationBasis));
                //Remove the items from list that are not required on TT.
                for (int i = 0; i < 4; i++)
                {
                    list.RemoveAt(2);
                }
                ValueList valueList = new ValueList();
                for (int i = 0; i < list.Count; i++)
                {
                    valueList.ValueListItems.Add(((KeyValuePair<Enum, string>)list[i]).Key, ((KeyValuePair<Enum, string>)list[i]).Value);
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_CALCBASIS);
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_SOFTCOMMISSIONCALCBASIS);
                iMultiTradingTicketView.FillBulkCommissionBasis(list);
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

        private void BindDataInAccountColumn()
        {
            try
            {
                OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;
                if (customSchemes == null)
                    customSchemes = _allocationProxy.InnerChannel.GetAllocationSchemesBySource(FixedPreferenceCreationSource.StagedOrderImport);
                if (prefs == null)
                    prefs = _allocationProxy.InnerChannel.GetInvisibleAllocationPreferences();
                DataTable accountsAndAllocationDefaults = GetAccountAndAllocationPrefTable();

                object[] items = accountsAndAllocationDefaults.Rows[0].ItemArray;
                accountsAndAllocationDefaults.Rows.RemoveAt(0);

                DataView dv = accountsAndAllocationDefaults.DefaultView;
                dv.Sort = OrderFields.PROPERTY_LEVEL1NAME;
                accountsAndAllocationDefaults = dv.ToTable();
                DataRow firstRow = accountsAndAllocationDefaults.NewRow();
                firstRow.ItemArray = items;
                accountsAndAllocationDefaults.Rows.InsertAt(firstRow, 0);

                DataTable dt = ListOfAccounts(String.Empty, accountsAndAllocationDefaults);


                ValueList valueList = new ValueList();
                foreach (DataRow row in dt.Rows)
                {
                    valueList.ValueListItems.Add(Convert.ToInt32(row[OrderFields.PROPERTY_LEVEL1ID]), row[OrderFields.PROPERTY_LEVEL1NAME].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_ACCOUNT);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].OriginalAllocationPreferenceID > 0 && list[i].Level1ID > 0 && valueList != null)
                    {
                        bool isDropdownChanged = false;
                        ValueList vl = null;
                        if (customSchemes.ContainsKey(list[i].Level1ID))
                        {
                            vl = valueList.Clone();
                            vl.ValueListItems.Add(list[i].Level1ID, TradingTicketConstants.CUSTOM_FIXED);
                            isDropdownChanged = true;
                        }
                        else if (prefs.ContainsKey(list[i].Level1ID))
                        {
                            vl = valueList.Clone();
                            if (list[i].TransactionSource == TransactionSource.PST)
                            {
                                vl.ValueListItems.Add(list[i].Level1ID, TradingTicketConstants.PTT);
                            }
                            else if (list[i].TransactionSource == TransactionSource.Rebalancer)
                            {
                                vl.ValueListItems.Add(list[i].Level1ID, TradingTicketConstants.REBAL);
                            }
                            else
                            {
                                vl.ValueListItems.Add(list[i].Level1ID, TradingTicketConstants.LIT_CUSTOM);
                            }
                            isDropdownChanged = true;
                        }
                        if (isDropdownChanged)
                        {
                            iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(vl, i, OrderFields.PROPERTY_ACCOUNT);
                        }
                    }
                }
                iMultiTradingTicketView.BindBulkCombo(dt, OrderFields.PROPERTY_ACCOUNT);
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

        private void BindDataInBrokerColumn()
        {
            try
            {
                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;
                    for (int i = 0; i < list.Count; i++)
                    {
                        BindDataInBrokerBasedOnAccountID(i);
                    }
                }
                else
                {
                    Dictionary<int, string> cpDict = MTTHelperManager.GetInstance().GetCounterparties();
                    ValueList cpList = new ValueList();
                    foreach (KeyValuePair<int, string> counterParty in cpDict)
                    {
                        cpList.ValueListItems.Add(counterParty.Key, counterParty.Value);
                    }
                    iMultiTradingTicketView.AddDropDownForGivenColumn(cpList, OrderFields.PROPERTY_COUNTERPARTY_NAME);

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
        /// Binds the broker in first row.
        /// Logic : Broker will be enabled only if the assestID, CounterPartyId, UnderLyingId are same of all the trades in grid.
        /// </summary>
        private void BindBrokerBulkCombo()
        {
            try
            {
                Dictionary<int, string> cpList1 = MTTHelperManager.GetInstance().GetCounterparties();
                ValueList cpListBulk = new ValueList();
                foreach (KeyValuePair<int, string> counterParty in cpList1)
                {
                    cpListBulk.ValueListItems.Add(counterParty.Key, counterParty.Value);
                }
                if (cpListBulk != null)
                {
                    cpListBulk.SortStyle = ValueListSortStyle.Ascending;
                    iMultiTradingTicketView.BindBulkCombo(cpListBulk, OrderFields.PROPERTY_COUNTERPARTY_NAME);
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

        private void BindDataInExecutionInstructions()
        {
            try
            {
                DataTable executionInstructions = CachedDataManager.GetInstance.GetExecutionInstruction().Copy();
                ValueList valueList = new ValueList();
                foreach (DataRow row in executionInstructions.Rows)
                {
                    valueList.ValueListItems.Add(row[OrderFields.PROPERTY_EXECUTION_INSTID], row[OrderFields.CAPTION_EXECUTION_INST].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_EXECUTION_INST_TagValue);
                iMultiTradingTicketView.BindBulkCombo(executionInstructions, OrderFields.PROPERTY_EXECUTION_INST_TagValue);
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

        private void BindDataInHandlingInstructions()
        {
            try
            {
                DataTable handlingInstructions = CachedDataManager.GetInstance.GetHandlingInstruction().Copy();
                handlingInstructions.Columns[0].ColumnName = OrderFields.PROPERTY_HANDLING_INSTID;
                handlingInstructions.Columns[1].ColumnName = OrderFields.PROPERTY_HANDLING_INST;
                ValueList valueList = new ValueList();
                foreach (DataRow row in handlingInstructions.Rows)
                {
                    valueList.ValueListItems.Add(row[OrderFields.PROPERTY_HANDLING_INSTID], row[OrderFields.PROPERTY_HANDLING_INST].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_HANDLING_INST_TagValue);
                iMultiTradingTicketView.BindBulkCombo(handlingInstructions, OrderFields.PROPERTY_HANDLING_INST_TagValue);
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

        private void BindDataInOrderSideColumn()
        {
            try
            {
                DataTable orderSide = CachedDataManager.GetInstance.GetOrderSides();
                OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;
                Dictionary<int, ValueList> assetWiseSides = new Dictionary<int, ValueList>();
                for (int i = 0; i < list.Count; i++)
                {
                    ValueList valueList;
                    if (assetWiseSides.ContainsKey(list[i].AssetID))
                        valueList = assetWiseSides[list[i].AssetID];
                    else
                    {
                        valueList = new ValueList();
                        DataTable sides = CachedDataManager.GetInstance.GetOrderSides(list[i].AssetID);
                        foreach (DataRow row in sides.Rows)
                        {
                            valueList.ValueListItems.Add(row[OrderFields.PROPERTY_ORDER_SIDEID], row[OrderFields.PROPERTY_ORDER_SIDE].ToString());
                        }
                        assetWiseSides.Add(list[i].AssetID, valueList);
                    }
                    iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(valueList, i, OrderFields.PROPERTY_ORDER_SIDE);
                }
                iMultiTradingTicketView.BindBulkCombo(orderSide, OrderFields.PROPERTY_ORDER_SIDE);
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

        private void BindDataInOrderTypeColumn()
        {
            try
            {
                DataTable types = CachedDataManager.GetInstance.GetOrderTypes().Copy();
                ValueList valueList = new ValueList();
                foreach (DataRow row in types.Rows)
                {
                    valueList.ValueListItems.Add(row[OrderFields.PROPERTY_ORDER_TYPE_ID], row[OrderFields.PROPERTY_ORDER_TYPE].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_ORDER_TYPE);
                iMultiTradingTicketView.BindBulkCombo(types, OrderFields.PROPERTY_ORDER_TYPE);
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

        private void BindDataInSettlementCurrency()
        {
            try
            {
                int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;

                ValueList vl = new ValueList();
                vl.ValueListItems.Add(companyBaseCurrencyID, dictCurrencies[companyBaseCurrencyID]);
                iMultiTradingTicketView.AddDropDownForGivenColumn(vl, TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].CurrencyID != companyBaseCurrencyID && dictCurrencies.ContainsKey(list[i].CurrencyID))
                    {
                        ValueList valueList = vl.Clone();
                        valueList.ValueListItems.Add(list[i].CurrencyID, dictCurrencies[list[i].CurrencyID]);
                        iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(valueList, i, TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME);
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

        private void BindDataInSettlementFxOperator()
        {
            try
            {
                ValueList settlementFxOperator = new ValueList();
                List<EnumerationValue> SettlementFxOperatorEnumList = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Operator));
                foreach (EnumerationValue var in SettlementFxOperatorEnumList.Where(var => !var.Value.Equals((int)Operator.Multiple)))
                {
                    settlementFxOperator.ValueListItems.Add(var.DisplayText, var.DisplayText);
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(settlementFxOperator, OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR);
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

        private void BindDataInStrategyColumn()
        {
            try
            {
                StrategyCollection strategies = CachedDataManager.GetInstance.GetUserStrategies();
                if (strategies.Contains(int.MinValue))
                {
                    strategies.RemoveAt(strategies.IndexOf(int.MinValue));
                }
                ValueList valueList = new ValueList();
                for (int i = 0; i < strategies.Count; i++)
                {
                    valueList.ValueListItems.Add(((Strategy)strategies[i]).StrategyID, ((Strategy)strategies[i]).Name);
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_STRATEGY);
                iMultiTradingTicketView.BindBulkCombo(strategies, OrderFields.PROPERTY_STRATEGY);
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

        private void BindDataInTIFColumn()
        {
            try
            {
                DataTable tifs = CachedDataManager.GetInstance.GetTIFS().Copy();
                ValueList valueList = new ValueList();
                foreach (DataRow row in tifs.Rows)
                {
                    valueList.ValueListItems.Add(row[OrderFields.PROPERTY_TIFID], row[OrderFields.PROPERTY_TIF].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_TIF_TAGVALUE);
                iMultiTradingTicketView.BindBulkCombo(tifs, OrderFields.PROPERTY_TIF_TAGVALUE);
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

        private void BindDataInTradingAccountCombo()
        {
            try
            {
                TradingAccountCollection tradingAccounts = CachedDataManager.GetInstance.GetUserTradingAccounts();
                DataTable dt = new DataTable();
                dt.Columns.Add(TradingTicketConstants.LIT_VALUE);
                dt.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                foreach (TradingAccount tradingAccount in tradingAccounts)
                {
                    dt.Rows.Add(tradingAccount.TradingAccountID, tradingAccount.Name);
                }
                DataView dv = dt.DefaultView;
                dv.Sort = TradingTicketConstants.LIT_DISPLAY;
                DataTable newDt = dv.ToTable();
                ValueList valueList = new ValueList();
                foreach (DataRow row in newDt.Rows)
                {
                    valueList.ValueListItems.Add(row[TradingTicketConstants.LIT_VALUE], row[TradingTicketConstants.LIT_DISPLAY].ToString());
                }
                iMultiTradingTicketView.AddDropDownForGivenColumn(valueList, OrderFields.PROPERTY_TRADING_ACCOUNT);
                iMultiTradingTicketView.BindBulkCombo(newDt, OrderFields.PROPERTY_TRADING_ACCOUNT);
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
        /// Binds data for venue combo when broker value is changed on bulk update UI
        /// </summary>
        /// <param name="brokerName"></param>
        private void BindDataInVenueBasedOnBrokerIDBulk(string brokerName)
        {
            try
            {
                ValueList venueList = MTTHelperManager.GetInstance().GetVenuesByCounterPartyID(CachedDataManager.GetInstance.GetCounterPartyID(brokerName));
                iMultiTradingTicketView.BindBulkCombo(venueList, OrderFields.PROPERTY_VENUE);
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
        ///<summary>
        ///Binds the data in Broker based on Account when MasterFund preference is ON
        ///This is called when Account is changed
        ///</summary>
        /// <param name="index">The index.</param>
        private void BindDataInBrokerBasedOnAccountID(int index)
        {
            try
            {
                OrderSingle rowObject = (OrderSingle)iMultiTradingTicketView.GetOrderSingleBasedOnIndex(index).ListObject;
                if (rowObject != null && rowObject.AUECID > 0)
                {
                    ValueList cpList;
                    Tuple<int, int> tuple = new Tuple<int, int>(rowObject.AUECID, rowObject.Level1ID);
                    if (auecAccountWiseCP.ContainsKey(tuple))
                    {
                        cpList = auecAccountWiseCP[tuple];
                        iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(cpList, index, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    }
                    else
                    {
                        cpList = TTHelperManager.GetInstance().GetCounterparties(rowObject.AUECID);
                        if (iMultiTradingTicketView.BulkAccountQty != null && iMultiTradingTicketView.BulkAccountQty.AllocationOperationPreference != null
                            && iMultiTradingTicketView.BulkAccountQty.AllocationOperationPreference.OperationPreferenceId == rowObject.Level1ID)
                        {
                            List<int> accountIds = iMultiTradingTicketView.BulkAccountQty.AllocationOperationPreference.GetSelectedAccountsList();
                            cpList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
                        }
                        else if (rowObject.Level1ID > 0)
                        {
                            if ((rowObject.OriginalAllocationPreferenceID > 0 && rowObject.Level1ID == rowObject.OriginalAllocationPreferenceID) ||
                                (prefs != null && prefs.ContainsKey(rowObject.Level1ID)) || (allocationPrefs != null && allocationPrefs.ContainsKey(rowObject.Level1ID)))
                            {
                                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, rowObject.Level1ID);

                                if (operationPreference != null)
                                {
                                    List<int> accountIds = operationPreference.GetSelectedAccountsList();
                                    cpList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountIds, cpList);
                                }
                            }
                            else
                            {
                                List<int> accountsList = new List<int> { rowObject.Level1ID };
                                cpList = TTHelperManager.GetInstance().GetCounterpartiesFilterByAccount(accountsList, cpList);
                            }
                        }
                        if (cpList != null)
                        {
                            cpList.SortStyle = ValueListSortStyle.Ascending;
                            auecAccountWiseCP.Add(tuple, cpList);
                            iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(cpList, index, OrderFields.PROPERTY_COUNTERPARTY_NAME);
                        }
                    }

                    if (TradingTktPrefs.UserTradingTicketUiPrefs.Broker.HasValue && TradingTktPrefs.UserTradingTicketUiPrefs.Broker != int.MinValue)
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(cpList, CachedDataManager.GetInstance.GetCounterPartyText(TradingTktPrefs.UserTradingTicketUiPrefs.Broker.Value)))
                        {
                            rowObject.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(TradingTktPrefs.UserTradingTicketUiPrefs.Broker.Value);
                            rowObject.CounterPartyID = TradingTktPrefs.UserTradingTicketUiPrefs.Broker.Value;
                            iMultiTradingTicketView.UpgradeGridAfterBrokerChange(index, rowObject.CounterPartyID);
                        }
                    }
                    else if (TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.HasValue && TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker != int.MinValue)
                    {
                        if (ValueListUtilities.CheckIfValueExistsInValuelistFromMTT(cpList, CachedDataManager.GetInstance.GetCounterPartyText(TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.Value)))
                        {
                            rowObject.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.Value);
                            rowObject.CounterPartyID = TradingTktPrefs.CompanyTradingTicketUiPrefs.Broker.Value;
                            iMultiTradingTicketView.UpgradeGridAfterBrokerChange(index, rowObject.CounterPartyID);
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

        /// <summary>
        /// Binds the data in venue based on broker identifier.
        /// This is called when we change broker, and dataSource of venue ultracell needs to be changed.
        /// Also, called to change the datasource of first trade's venue(self made).
        /// </summary>
        /// <param name="index">The index.</param>
        private void BindDataInVenueBasedOnBrokerID(int index)
        {
            try
            {
                IsMTTSettingUP = iMultiTradingTicketView.IsMTTSettingUp;
                UltraGridRow ulroGridRow = iMultiTradingTicketView.GetOrderSingleBasedOnIndex(index);
                if (ulroGridRow != null)
                {
                    OrderSingle orderSingle = ulroGridRow.ListObject as OrderSingle;
                    if (orderSingle != null)
                    {
                        var brokerId = CachedDataManager.GetInstance.GetCounterPartyID(ulroGridRow.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Text);
                        ValueList venueList;
                        if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                            venueList = MTTHelperManager.GetInstance().GetVenuesByCounterPartyID(brokerId, orderSingle.AUECID);
                        else
                            venueList = MTTHelperManager.GetInstance().GetVenuesByCounterPartyID(brokerId);

                        UpdateDataInVenueColumn(venueList, orderSingle, brokerId);
                        iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(venueList, index, OrderFields.PROPERTY_VENUE);
                        iMultiTradingTicketView.UpdateAlgoType(ulroGridRow);
                        iMultiTradingTicketView.UpgradeGridAfterVenueChange(index, orderSingle.VenueID);
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

        private void BindDataInVenueColumn()
        {
            try
            {
                IsMTTSettingUP = iMultiTradingTicketView.IsMTTSettingUp;
                OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;
                ValueList venueListBulk = MTTHelperManager.GetInstance().GetVenues();
                for (int i = 0; i < list.Count; i++)
                {
                    ValueList venueList;
                    if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                        venueList = MTTHelperManager.GetInstance().GetVenuesByCounterPartyID(list[i].CounterPartyID, list[i].AUECID);
                    else
                        venueList = MTTHelperManager.GetInstance().GetVenuesByCounterPartyID(list[i].CounterPartyID);
                    iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(venueList, i, OrderFields.PROPERTY_VENUE);
                }
                iMultiTradingTicketView.BindBulkCombo(venueListBulk, OrderFields.PROPERTY_VENUE);
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

        private static bool IsMTTSettingUP;

        private static void UpdateDataInVenueColumn(ValueList venueList, OrderSingle orderSingle, int brokerId)
        {
            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue.ContainsKey(Convert.ToInt32(brokerId)) && ValueListUtilities.CheckIfValueExistsInValuelist(venueList, TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(brokerId)].ToString()) && !IsMTTSettingUP)
            {
                orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(brokerId)]);
                orderSingle.VenueID = TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionVenue[Convert.ToInt32(brokerId)];
            }
            else if (TradingTktPrefs.CompanyTradingTicketUiPrefs.Venue.HasValue && ValueListUtilities.CheckIfValueExistsInValuelist(venueList, TradingTktPrefs.CompanyTradingTicketUiPrefs.Venue.ToString()) && !IsMTTSettingUP)
            {
                orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText((int)TradingTktPrefs.CompanyTradingTicketUiPrefs.Venue);
                orderSingle.VenueID = (int)TradingTktPrefs.CompanyTradingTicketUiPrefs.Venue;
            }
            else if (venueList != null && venueList.ValueListItems.Count > 0 && !IsMTTSettingUP)
            {
                orderSingle.Venue = venueList.ValueListItems[0].DisplayText;
                orderSingle.VenueID = (int)venueList.ValueListItems[0].DataValue;
            }
        }

        BindableValueList[] _vls;

        private void BindTradeAttribute()
        {
            try
            {
                List<string>[] attribLists = _allocationProxy.InnerChannel.GetTradeAttributes();
                TradeAttributesCache.updateCache(attribLists, true);
                _vls = TradeAttributesCache.getValueList((Form)iMultiTradingTicketView);
                SetTradeAttributeValueList(_vls);
                iMultiTradingTicketView.SetBulkTradeAttributeValueList(_vls);
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

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>(TradingTicketConstants.PRICING_SERVICE_ADDRESS, this);
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
        /// Gets the account and allocation preference table from Cached Data Manager.
        /// </summary>
        /// <returns></returns>
        private DataTable GetAccountAndAllocationPrefTable()
        {
            try
            {
                DataTable accountsAndAllocationRules = new DataTable();
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1ID);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_LEVEL1NAME);
                accountsAndAllocationRules.Columns.Add(OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE);

                AccountCollection _userAccounts = CachedDataManager.GetInstance.GetUserAccounts();

                if (_userAccounts != null)
                {
                    foreach (Account userAccount in _userAccounts)
                    {
                        DataRow accountRow = accountsAndAllocationRules.NewRow();
                        accountRow[OrderFields.PROPERTY_LEVEL1ID] = userAccount.AccountID;
                        accountRow[OrderFields.PROPERTY_LEVEL1NAME] = userAccount.Name;
                        if (userAccount.AccountID != int.MinValue)
                        {
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = false;
                        }
                        else
                        {
                            accountRow[OrderFields.PROPERTY_LEVEL1NAME] = ApplicationConstants.C_LIT_UNALLOCATED;
                            accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                        }
                        accountsAndAllocationRules.Rows.Add(accountRow);
                    }

                    // Prana-9688: If trade server hang at the very same moment when TT is opened, then below pref is not able to be fetched via proxy and gives error.
                    // which interrupts further flow and accounts are not binded which led to Object reference error while accessing value of binded accounts.
                    // So applied handling here not to bother the further executions.
                    try
                    {
                        //This method is returning preferences which are created from Edit Allocation preferences UI, PRANA-23524
                        Dictionary<int, string> preferences = _allocationProxy.InnerChannel.GetAllocationPreferences(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID,
                            AllocationSubModulePermission.IsLevelingPermitted,
                            AllocationSubModulePermission.IsProrataByNavPermitted);
                        if (preferences != null)
                        {
                            foreach (int prefId in preferences.Keys)
                            {
                                DataRow accountRow = accountsAndAllocationRules.NewRow();
                                accountRow[OrderFields.PROPERTY_LEVEL1ID] = prefId;
                                accountRow[OrderFields.PROPERTY_LEVEL1NAME] = preferences[prefId];
                                accountRow[OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE] = true;
                                accountsAndAllocationRules.Rows.Add(accountRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                return accountsAndAllocationRules;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// returns a list of symbols for rows which are checked, if no rows is checked then all symbols are returned
        /// </summary>
        /// <param name="CheckboxColumn"></param>
        /// <returns></returns>
        private List<string> GetAllCheckedSymbols()
        {
            List<string> listCheckedSymbols = new List<string>();
            try
            {
                listCheckedSymbols.AddRange(iMultiTradingTicketView.CheckedUltraGridRows.Select(row => row.Cells[OrderFields.PROPERTY_SYMBOL].Value.ToString()));
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
            if (listCheckedSymbols.Count > 0)
                return listCheckedSymbols;
            return new List<string>(iMultiTradingTicketView.DictOrdersBindedSymbolwise.Keys);
        }


        /// <summary>
        /// Gets the checked rows for trade.
        /// Dictionary Key : Ordersingle, value : marketPrice
        /// </summary>
        /// <returns></returns>
        private List<Tuple<int, OrderSingle, double>> GetCheckedRowsForTrade(out bool isMultiDayManual)
        {
            List<Tuple<int, OrderSingle, double>> orderSinglesAndMarkPrice = new List<Tuple<int, OrderSingle, double>>();
            isMultiDayManual = false;
            try
            {
                List<UltraGridRow> rows = iMultiTradingTicketView.CheckedUltraGridRows;
                bool IsRowErrorInGrid = false;
                for (int i = rows.Count - 1; i >= 0; i--)
                {
                    UltraGridRow ordeRow = rows[i];
                    if (ordeRow.DataErrorInfo != null && !string.IsNullOrEmpty(ordeRow.DataErrorInfo.RowError))
                    {
                        IsRowErrorInGrid = true;
                    }
                    else if (_tradingTicketType == TradingTicketType.Manual
                        && ordeRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value != null
                        && ((ordeRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("Good Till Date")
                        || (ordeRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals(FIXConstants.TIF_GTD)))
                        || (ordeRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals("GTC")
                        || (ordeRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.Equals(FIXConstants.TIF_GTC)))))
                    {
                        isMultiDayManual = true;
                    }
                    else
                    {
                        OrderSingle order = DeepCopyHelper.Clone(ordeRow.ListObject as OrderSingle);
                        StringBuilder result;
                        if (!ValidateOrderSingle(ordeRow, order, out result))
                        {
                            MessageBox.Show(result.ToString());
                            return new List<Tuple<int, OrderSingle, double>>();
                        }
                        orderSinglesAndMarkPrice.Add(Tuple.Create(ordeRow.Index, order, Convert.ToDouble(ordeRow.Cells[TradingTicketConstants.COLUMN_MARKETPRICE].Value)));
                    }
                }
                if (isMultiDayManual)
                {
                    iMultiTradingTicketView.SetStatusBarMessage(TradingTicketConstants.MSG_CANNOT_BOOK_DONEAWAY_TRADES_FOR_MULTIDAY);
                }
                if (IsRowErrorInGrid)
                {
                    if (!(MessageBox.Show("Some orders have incomplete or incorrect information. Do you wish to send the remaining orders?", "Multi Trading Ticket", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        return new List<Tuple<int, OrderSingle, double>>();
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
            return orderSinglesAndMarkPrice;
        }

        /// <summary>
        /// Gets the error string.
        /// </summary>
        /// <returns></returns>
        private string GetErrorString()
        {
            string errorString = TradingTicketConstants.MSG_DATA_MANAGER_NOT_CONNECTED;
            try
            {
                if (iMultiTradingTicketView.PriceSymbolSettings.ValidateSymbolCheck)
                {
                    errorString += TradingTicketConstants.CAPTION_SYMBOL + " ";
                }
                if (iMultiTradingTicketView.PriceSymbolSettings.RiskCtrlCheck)
                {
                    errorString += TradingTicketConstants.CAPTION_PRICE + " ";
                }
                errorString += TradingTicketConstants.MSG_COULD_NOT_VALIDATED_PROCEED_ANAWAYS;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return errorString;
        }

        /// <summary>
        /// Gets the name of multi trade.
        /// </summary>
        /// <returns></returns>
        private string GetMultiTradeName()
        {
            try
            {
                DateTime dateTimeValue = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                return dateTimeValue.ToString("MM-dd-yyyy") + "-" + dateTimeValue.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Handles the GetPrice event of the MTTView control to send snapShot request.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MTTView_GetPrice(object sender, EventArgs e)
        {
            List<string> ListOfSymbol = GetAllCheckedSymbols();
            try
            {
                if (IsLiveFeedConnected())
                {
                    _pricingServicesProxy.InnerChannel.RequestSnapshot(ListOfSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol, false);
                    if (!iMultiTradingTicketView.IsMTTSettingUp)
                    {
                        iMultiTradingTicketView.SetStatusBarMessage(TradingTicketConstants.MSG_PRICES_FETCHING);
                        timer = new System.Threading.Timer(timerRefresh_Tick, null, 3000, Timeout.Infinite);
                        iMultiTradingTicketView.EnableOrDisableMTT(false);
                    }
                }
                else
                {
                    MessageBox.Show(TradingTicketConstants.MSG_DATA_MANAGER_NOT_CONNECTED_CANNOT_FETCH_LIVE_PRICES, TradingTicketConstants.MSG_DATA_MANAGER_NOT_CONNECTED, MessageBoxButtons.OK);
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

        private void timerRefresh_Tick(object state)
        {
            try
            {
                timer.Dispose();
                timer = null;
                iMultiTradingTicketView.SetStatusBarMessage(TradingTicketConstants.MSG_PRICES_FETCHED);
                iMultiTradingTicketView.EnableOrDisableMTT(true);
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
        /// Wire methods to events and create allocation proxy.
        /// </summary>
        private void Initialize()
        {
            try
            {
                iMultiTradingTicketView.PriceSymbolSettings = TradingTktPrefs.PriceSymbolValidationData;
                iMultiTradingTicketView.BindAllDropDowns += BindAllComboBox;
                iMultiTradingTicketView.TagDatabaseManagerWork += MTTView_TagDatabaseManagerWork;
                iMultiTradingTicketView.TradeClick += MTTView_TradeClick;
                iMultiTradingTicketView.UpdateVenueOnBrokerChange += MTTView_UpdateVenueOnBrokerChange;
                iMultiTradingTicketView.UpdateVenueOnBrokerChangeBulk += MTTView_UpdateVenueOnBrokerChangeBulk;
                iMultiTradingTicketView.UpdateVenueForFirstRowOnBrokerChange += iMultiTradingTicketView_UpdateVenueForFirstRowOnBrokerChange;
                iMultiTradingTicketView.GetPrice += MTTView_GetPrice;
                iMultiTradingTicketView.UnwireEventsInPresenter += MTTView_UnwireEventsInPresenter;
                iMultiTradingTicketView.UpdateBrokerOnAccountChange += MTTView_UpdateBrokerOnAccountChange;
                CreatePricingServiceProxy();
                _allocationProxy = new ProxyBase<IAllocationManager>(TradingTicketConstants.LIT_ALLOCATION_END_POINT_ADDRESS_NEW);
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

        private void MTTView_UpdateBrokerOnAccountChange(object sender, EventArgs<int> e)
        {
            try
            {
                BindDataInBrokerBasedOnAccountID(e.Value);
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
        /// Handles the UnwireEventsInPresenter event of the MTTView control.
        /// Unwires the event and called when before MTT is reopened only.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MTTView_UnwireEventsInPresenter(object sender, EventArgs e)
        {
            try
            {
                iMultiTradingTicketView.BindAllDropDowns -= BindAllComboBox;
                iMultiTradingTicketView.TagDatabaseManagerWork -= MTTView_TagDatabaseManagerWork;
                iMultiTradingTicketView.TradeClick -= MTTView_TradeClick;
                iMultiTradingTicketView.UpdateVenueOnBrokerChange -= MTTView_UpdateVenueOnBrokerChange;
                iMultiTradingTicketView.GetPrice -= MTTView_GetPrice;
                iMultiTradingTicketView.UnwireEventsInPresenter -= MTTView_UnwireEventsInPresenter;
                iMultiTradingTicketView.UpdateVenueForFirstRowOnBrokerChange -= iMultiTradingTicketView_UpdateVenueForFirstRowOnBrokerChange;
                iMultiTradingTicketView.UpdateBrokerOnAccountChange -= MTTView_UpdateBrokerOnAccountChange;
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

        void iMultiTradingTicketView_UpdateVenueForFirstRowOnBrokerChange(object sender, EventArgs<int> e)
        {
            try
            {
                UltraGridRow ulroGridRow = iMultiTradingTicketView.GetOrderSingleBasedOnIndex(e.Value);
                UltraGridRow urowForAUECid = iMultiTradingTicketView.GetOrderSingleBasedOnIndex(e.Value + 1);
                if (ulroGridRow != null)
                {
                    string brokerName = ulroGridRow.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Text;
                    string venueName = ulroGridRow.Cells[OrderFields.PROPERTY_VENUE].Text;
                    OrderSingle orderSingle = urowForAUECid.ListObject as OrderSingle;
                    if (orderSingle != null)
                    {
                        bool needToUpdateSelectedVenue = true;
                        ValueList venueList = TTHelperManager.GetInstance().GetVenuesByCounterPartyID(CachedDataManager.GetInstance.GetCounterPartyID(brokerName)
                            , orderSingle.AssetID, orderSingle.UnderlyingID, orderSingle.AUECID);
                        DataTable table = new DataTable();
                        table.Columns.Add(TradingTicketConstants.LIT_VALUE);
                        table.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                        foreach (ValueListItem t in venueList.ValueListItems)
                        {
                            table.Rows.Add(t.DataValue, t.DisplayText);
                            if (t.DisplayText == venueName)
                            {
                                needToUpdateSelectedVenue = false;
                            }
                        }
                        if (needToUpdateSelectedVenue)
                        {
                            ulroGridRow.Cells[OrderFields.PROPERTY_VENUE].Value = String.Empty;
                            ulroGridRow.Update();
                        }
                        iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(venueList, e.Value, OrderFields.PROPERTY_VENUE);
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
        /// Determines whether [is live feed connected].
        /// </summary>
        /// <returns></returns>
        private bool IsLiveFeedConnected()
        {
            try
            {
                return _pricingServicesProxy.InnerChannel.IsLiveFeedActive;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Lists the of accounts.
        /// </summary>
        /// <returns></returns>
        private DataTable ListOfAccounts(string masterFund, DataTable accountsAndAllocationDefaults)
        {
            try
            {
                DataTable accountsAndAllocationDefaultsCloned;
                // If master fund is present
                if (!string.IsNullOrEmpty(masterFund))
                {
                    accountsAndAllocationDefaultsCloned = accountsAndAllocationDefaults.Clone();

                    //for select entry in drop down
                    DataRow r = accountsAndAllocationDefaultsCloned.NewRow();
                    r[0] = accountsAndAllocationDefaults.Rows[0][0];
                    r[1] = accountsAndAllocationDefaults.Rows[0][1];
                    r[2] = accountsAndAllocationDefaults.Rows[0][2];
                    accountsAndAllocationDefaultsCloned.Rows.Add(r);

                    int fundId = CachedDataManager.GetInstance.GetMasterFundID(masterFund);
                    for (int i = 1; i < accountsAndAllocationDefaults.Rows.Count; i++)
                    {
                        var row = accountsAndAllocationDefaults.Rows[i];
                        var fundIdForAccount = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(Convert.ToInt32(row.ItemArray[0]));
                        if (Convert.ToInt32(row.ItemArray[0]) > 0 && fundId == fundIdForAccount)
                        {
                            r = accountsAndAllocationDefaultsCloned.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            accountsAndAllocationDefaultsCloned.Rows.Add(r);
                        }
                    }
                }
                else
                {
                    accountsAndAllocationDefaultsCloned = accountsAndAllocationDefaults.Copy();
                }
                if (accountsAndAllocationDefaultsCloned.Rows[0].ItemArray[0].ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                    accountsAndAllocationDefaultsCloned.Rows[0].ItemArray[0] = TradingTicketConstants.LIT_UNALLOCATED;
                return accountsAndAllocationDefaultsCloned;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Maps the identifier to tag value.
        /// </summary>
        private void MapIdToTagValue()
        {
            try
            {
                OrderBindingList list = iMultiTradingTicketView.ListOrdersBinded;

                DataTable dt = TagDatabase.GetInstance().TIF;
                Dictionary<string, string> tifs = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {
                    tifs.Add(row.Field<string>(OrderFields.PROPERTY_TIF_TAGVALUE), row.Field<string>(OrderFields.PROPERTY_TIFID));
                }

                dt = TagDatabase.GetInstance().OrderType;
                Dictionary<string, string> ordertype = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {
                    ordertype.Add(row.Field<string>(OrderFields.PROPERTY_ORDER_TYPETAGVALUE), row.Field<string>(OrderFields.PROPERTY_ORDER_TYPE_ID));
                }

                dt = TagDatabase.GetInstance().ExecutionInstruction;
                Dictionary<string, string> executionInstruction = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {
                    executionInstruction[row.Field<string>(OrderFields.PROPERTY_EXECUTION_INST_TagValue)] = row.Field<string>(OrderFields.PROPERTY_EXECUTION_INSTID);
                }

                dt = TagDatabase.GetInstance().HandlingInstruction;
                Dictionary<string, string> handlingInstruction = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {
                    handlingInstruction[row.Field<string>(OrderFields.PROPERTY_HANDLING_INST_TagValue)] = row.Field<string>(OrderFields.PROPERTY_HANDLING_INSTID);
                }

                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                Dictionary<int, string> counterParties = TTHelperManager.GetInstance().GetCounterparties();
                Dictionary<int, string> Venues = CachedDataManager.GetInstance.GetAllVenues();
                Dictionary<int, string> TradingAccounts = CachedDataManager.GetInstance.GetAllTradingAccount();
                allocationPrefs = _allocationProxy.InnerChannel.GetAllocationPreferences(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, true, true);
                for (int i = 0; i < list.Count; i++)
                {
                    //Using column's value list  for validation
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_STRATEGY, list[i].Strategy);
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_ORDER_SIDE, list[i].OrderSide);
                    if ((allocationPrefs != null && allocationPrefs.ContainsKey(list[i].Level1ID)) || list[i].OriginalAllocationPreferenceID > 0)
                    {
                        iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_ACCOUNT, list[i].Level1ID.ToString());
                    }

                    string result = tifs.ContainsKey(list[i].TIF) ? tifs[list[i].TIF] : null;
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_TIF_TAGVALUE, result);

                    result = ordertype.ContainsKey(list[i].OrderTypeTagValue) ? ordertype[list[i].OrderTypeTagValue] : null;
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_ORDER_TYPE, result);

                    result = executionInstruction.ContainsKey(list[i].ExecutionInstruction) ? executionInstruction[list[i].ExecutionInstruction] : null;
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_EXECUTION_INST_TagValue, result);

                    result = handlingInstruction.ContainsKey(list[i].HandlingInstruction) ? handlingInstruction[list[i].HandlingInstruction] : null;
                    iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_HANDLING_INST_TagValue, result);

                    if (!counterParties.ContainsKey(list[i].CounterPartyID))
                        iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_COUNTERPARTY_NAME, null);
                    else
                        iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_COUNTERPARTY_NAME, counterParties[list[i].CounterPartyID]);

                    if (!Venues.ContainsKey(list[i].VenueID))
                        iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_VENUE, null);

                    if (!TradingAccounts.ContainsKey(list[i].TradingAccountID))
                        iMultiTradingTicketView.UpdateCell(i, OrderFields.PROPERTY_TRADING_ACCOUNT, null);

                    result = dictCurrencies.ContainsKey(list[i].SettlementCurrencyID) ? dictCurrencies[list[i].SettlementCurrencyID] : null;
                    iMultiTradingTicketView.UpdateCell(i, TradingTicketConstants.COLUMN_SETTLEMENT_CURRENCYNAME, result);

                    if (TradingTktPrefs.TTGeneralPrefs.IsUseCustodianAsExecutingBroker)
                    {
                        SetCustodianBrokerPreference(list[i], i);
                    }
                    else
                    {
                        list[i].IsUseCustodianBroker = false;
                        list[i].AccountBrokerMapping = string.Empty;
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
        /// Sets order properties and updates UI for custodian orders
        /// </summary>
        /// <returns></returns>
        private void SetCustodianBrokerPreference(OrderSingle order, int index)
        {
            try
            {
                bool isSetCustodianBroker = false;
                AllocationOperationPreference operationPreference = null;
                if (order.IsUseCustodianBroker)
                {
                    isSetCustodianBroker = true;
                }
                else if (order.TransactionSource != TransactionSource.PST && String.IsNullOrEmpty(order.ClOrderID))
                {
                    if(!String.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(order.Level1ID)))
                    {
                        isSetCustodianBroker = true;
                    }
                    else
                    {
                        operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, order.Level1ID);
                        if (TradeManager.TradeManager.GetInstance().IsAllocationPrefValidForCustodianBroker(order, operationPreference))
                        {
                            isSetCustodianBroker = true;
                        }
                    }
                    order.IsUseCustodianBroker = isSetCustodianBroker;
                }
                if (isSetCustodianBroker)
                {
                    order.CounterPartyID = int.MinValue;

                    if(String.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(order.Level1ID)) && operationPreference==null)
                    {
                        operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, order.Level1ID);
                    }
                    ValueList cpList = iMultiTradingTicketView.GetCounterParties(index);

                    Dictionary<int, int> accountBrokerMapping = TradeManager.TradeManager.GetInstance().GetAccountBrokerMappingForSelectedFund(order.Level1ID, cpList, operationPreference, order);
                    order.AccountBrokerMapping = JsonHelper.SerializeObject(accountBrokerMapping);

                    iMultiTradingTicketView.UpdateCell(index, OrderFields.PROPERTY_COUNTERPARTY_NAME, "Default Broker(s)");
                    ValueList venueList = new ValueList();
                    venueList.ValueListItems.Add(1, "Drops");
                    iMultiTradingTicketView.AddDropDownToGivenIndexForColumn(venueList, index, OrderFields.PROPERTY_VENUE);
                    iMultiTradingTicketView.UpdateCell(index, OrderFields.PROPERTY_VENUE, "Drops");
                    iMultiTradingTicketView.SetUnmappedBrokerError(index, accountBrokerMapping);
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
        /// Update Expiry Date and set expiry time to EndClearanceTimes 
        /// </summary>
        /// <param name="or"></param>
        void UpdateExpiryDate(OrderSingle or)
        {
            try
            {
                if (or.TIF.Equals(FIXConstants.TIF_GTD))
                {
                    DateTime Dt;
                    if (DateTime.TryParse(or.ExpireTime, out Dt))
                    {
                        DateTime TimeStamp = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(or.AUECID);
                        string convertedAUECExpireDate = new DateTime(Dt.Year, Dt.Month, Dt.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                        or.ExpireTime = convertedAUECExpireDate;
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
        /// Sets the default values for live order when done from PTT.
        /// </summary>
        /// <param name="orderSingle">The or.</param>
        void SetDefaultValuesForLiveOrder(OrderSingle orderSingle, string orderStatusText, string orderStatusTagValue)
        {
            try
            {
                if (orderSingle.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && orderSingle.MsgType != FIXConstants.MSGExecutionReport && orderSingle.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX &&
                    orderSingle.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    orderSingle.MsgType = FIXConstants.MSGOrder;
                }
                orderSingle.PegDifference = double.Epsilon;
                orderSingle.PNP = "0";
                orderSingle.DiscretionInst = string.Empty;
                orderSingle.DiscretionOffset = double.Epsilon;
                orderSingle.DisplayQuantity = 0;
                orderSingle.LocateReqd = false;
                orderSingle.AvgPrice = 0;
                orderSingle.CumQty = 0;
                orderSingle.OrderStatus = orderStatusText;
                orderSingle.OrderStatusTagValue = orderStatusTagValue;
                UpdateExpiryDate(orderSingle);
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
        /// Creates the order for stage and live trade.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void CreateLiveOrder(List<Tuple<int, OrderSingle, double>> listCheckedOrders)
        {
            try
            {
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));
                int sucessfullTradeCounter = 0;
                if (CompliancePriceCheckRequired)
                {
                foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                {
                    OrderSingle orderSingle = tuple.Item2;
                    if (orderSingle.AvgPrice == Convert.ToDouble(0) && ComplianceCacheManager.GetPreTradeCheck(iMultiTradingTicketView.LoginUser.CompanyUserID))
                    {
                        if (this.PriceForComplianceNotAvailable != null)
                        {
                            this.PriceForComplianceNotAvailable(this, new EventArgs());
                        }
                        return;
                    }
                }
                }

                string orderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew);
                string orderStatusTagValue = TagDatabaseManager.GetInstance.GetOrderStatusValue(orderStatus);
                ShowFixDisconnectedPopup(listCheckedOrders);
                System.Threading.Tasks.Task.Run(() =>
                {
                    iMultiTradingTicketView.EnableOrDisableMTT(false);
                    foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                    {
                        OrderSingle or = tuple.Item2;
                        if (or != null)
                        {
                            double avgPrice = or.AvgPrice;
                            or.CumQtyForSubOrder = or.CumQty;
                            or.AvgPriceForCompliance = or.AvgPrice;
                            SetDefaultValuesForLiveOrder(or, orderStatus, orderStatusTagValue);

                            if (iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.PTT || iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.WatchList
       || iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.OptionChain)
                            {
                                or.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                                or.CumQty = 0;
                                or.IsStageRequired = true;
                                UpdateAllocationForPTTOrder(or);
                            }
                            or.TradeApplicationSource = Convert.ToInt32(TradeApplicationSource.Enterprise);
                            Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(or.AccountBrokerMapping);
                            if ((TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(or.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED
                                || (or.IsUseCustodianBroker && TradeManagerExtension.GetInstance().CheckAllFixConnectionsStatus(or, accountBrokerMapping).Count == 0))
                                && TradingRulesValidator.ValidateCompanyTradingRules(or, avgPrice, false))
                            {
                                SendTradeToBlotter(tuple, ref sucessfullTradeCounter);
                            }
                        }
                    }
                    iMultiTradingTicketView.DeleteRowsFromGrid();
                    TradeManagerExtension.GetInstance().SendMultiTradeDetails(MultiTradeId, sucessfullTradeCounter);
                    iMultiTradingTicketView.EnableOrDisableMTT(true);
                });
            }
            catch (Exception ex)
            {
                _isPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Shows the message box with number of disconnected FIX connections
        /// </summary>
        private static void ShowFixDisconnectedPopup(List<Tuple<int, OrderSingle, double>> listCheckedOrders)
        {
            try
            {
                bool isCustodianOrderPresent = false;
                HashSet<int> disconnectedFix = new HashSet<int>();
                foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                {
                    OrderSingle or = tuple.Item2;
                    Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(or.AccountBrokerMapping);
                    if (or.IsUseCustodianBroker)
                    {
                        isCustodianOrderPresent = true;
                        disconnectedFix.UnionWith(TradeManagerExtension.GetInstance().CheckAllFixConnectionsStatus(or, accountBrokerMapping));
                    }
                    else
                    {
                        if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(or.CounterPartyID) != PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            disconnectedFix.Add(or.CounterPartyID);
                        }
                    }
                }
                if (isCustodianOrderPresent && disconnectedFix.Count > 0)
                {
                    MessageBox.Show("FIX Connection for " + disconnectedFix.Count + " Broker(s) is down. Please resend your order", "Fix Disconnection Notice", MessageBoxButtons.OK);
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
        /// Updates the allocation for PTT order.
        /// </summary>
        /// <param name="or">The or.</param>
        private void UpdateAllocationForPTTOrder(OrderSingle or)
        {
            try
            {
                if (or != null && or.TransactionSource == TransactionSource.PST)
                {
                    AllocationOperationPreference aop = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, or.Level1ID);
                    if (aop != null)
                    {
                        bool isInGeneralRule = false;
                        //or.PSTAllocationPreferenceID = or.Level1ID;
                        if (aop.CheckListWisePreference != null && aop.CheckListWisePreference.Count == 1)
                        {
                            CheckListWisePreference clwp = aop.CheckListWisePreference.First().Value;

                            if (clwp.OrderSideList != null && clwp.OrderSideList.Count == 1 && or.OrderSideTagValue.Equals(clwp.OrderSideList[0]))
                            {
                                isInGeneralRule = true;
                                if (clwp.TargetPercentage.Count == 1)
                                {
                                    or.Level1ID = clwp.TargetPercentage.Keys.First();
                                }
                            }
                        }
                        if (!isInGeneralRule && aop.TargetPercentage.Count == 1)
                        {
                            or.Level1ID = aop.TargetPercentage.Keys.First();
                        }
                        or.IsPricingAvailable = _isPricingAvailable;
                    }
                }
            }
            catch (Exception ex)
            {
                _isPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends the trade to blotter.
        /// and updates the value of successfulTradeCounter which is used by TradeManager.GetInstance().SendMultiTradeDetails Method
        /// </summary>
        /// <param name="orderSingle">The order single.</param>
        /// <param name="sucessfullTradeCounter">The sucessfull trade counter.</param>
        /// <param name="marketPrice">The market price.</param>
        private void SendTradeToBlotter(Tuple<int, OrderSingle, double> tuple, ref int sucessfullTradeCounter, bool isOrderValidated=false)
        {
            try
            {
                bool isTradeSuccessful;
                if (tuple.Item2.IsUseCustodianBroker)
                {
                    tuple.Item2.CounterPartyName = String.Empty;
                    AllocationOperationPreference allocPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), CachedDataManager.GetInstance.LoggedInUser.CompanyUserID, tuple.Item2.Level1ID);
                    Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(tuple.Item2.AccountBrokerMapping);
                    isTradeSuccessful = TradeManager.TradeManager.GetInstance().SendBlotterMultipleTrades(tuple.Item2, accountBrokerMapping, allocPreference, _tradingTicketType == TradingTicketType.Stage, isOrderValidated: isOrderValidated);
                }
                else
                {
                    isTradeSuccessful = TradeManager.TradeManager.GetInstance().SendBlotterTrades(tuple.Item2, tuple.Item3, isOrderValidated: isOrderValidated);
                }

                if (isTradeSuccessful)
                {
                    sucessfullTradeCounter++;
                    iMultiTradingTicketView.MarkTradeForDeletion(tuple.Item1);
                    BlotterOrderCollections.GetInstance().UpdateOrderTabCollectionFromMTT(tuple.Item2);
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
        /// Handles the TradeClick event of the MTTView control.
        /// Called when DoneAway or Send Button is clicked
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{TradingTicketType}"/> instance containing the event data.</param>
        private void MTTView_TradeClick(object sender, EventArgs<TradingTicketType> e)
        {
            try
            {
                // generating a unique ID every time the multi button is clicked
                MultiTradeId = IDGenerator.GenerateMultiTradeId();
                _tradingTicketType = e.Value;
                bool isMultiDayManual;
                List<Tuple<int, OrderSingle, double>> listCheckedOrders = GetCheckedRowsForTrade(out isMultiDayManual);

                if (listCheckedOrders != null && listCheckedOrders.Count > 0)
                {
                    if (_tradingTicketType == TradingTicketType.Live)
                    {
                        CreateLiveOrder(listCheckedOrders);
                    }
                    else if (_tradingTicketType == TradingTicketType.Manual)
                    {
                        CreateManualOrder(listCheckedOrders);
                    }
                    else
                    {
                        CreateStageOrders(listCheckedOrders);
                    }
                    // iMultiTradingTicketView.SetStatusBarMessage(string.Empty);
                }
                else if(!isMultiDayManual)
                {
                    iMultiTradingTicketView.SetStatusBarMessage(TradingTicketConstants.MSG_PLEASE_CHECK_A_ROW_TO_TRADE);
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
        /// Creates the stage orders.
        /// </summary>
        private void CreateStageOrders(List<Tuple<int, OrderSingle, double>> listCheckedOrders)
        {
            try
            {
                iMultiTradingTicketView.EnableOrDisableMTT(false);
                bool CompliancePriceCheckRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CompliancePriceCheckRequired"));
                int sucessfullTradeCounter = 0;
                bool isOrderValidated = false;
                if (ComplianceCacheManager.GetPreTradeCheckStaging(iMultiTradingTicketView.LoginUser.CompanyUserID))
                {
                    if (CompliancePriceCheckRequired && listCheckedOrders.Any(tuple => tuple.Item2.AvgPrice == 0.0d))
                    {
                        if (PriceForComplianceNotAvailable != null)
                        {
                            PriceForComplianceNotAvailable(this, new EventArgs());
                        }
                        return;
                    }
                    List<Tuple<int, OrderSingle, double>> listValidatedOrders = new List<Tuple<int, OrderSingle, double>>();
                    foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                    {
                        OrderSingle or = tuple.Item2;
                        if (ValidationManager.ValidateOrder(or, iMultiTradingTicketView.LoginUser.CompanyUserID))
                        {
                            listValidatedOrders.Add(tuple);
                        }
                    }
                    listCheckedOrders = listValidatedOrders;
                    if (listCheckedOrders.Count == 0)
                    {
                        return;
                    }
                    isOrderValidated = true;
                    ComplianceCommon.UpdateMultipleReplaceOrderAlerts(listCheckedOrders.Select(pair => pair.Item2).ToList());
                    if (!iMultiTradingTicketView.IsEditOrders && !ComplianceCommon.ValidateOrderInCompliance_New(listCheckedOrders.Select(pair => pair.Item2).ToList(), (Form)iMultiTradingTicketView, iMultiTradingTicketView.LoginUser.CompanyUserID, true))
                    {
                        return;
                    }
                }
                string orderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew);
                if (iMultiTradingTicketView.IsEditOrders)
                {
                    orderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingReplace);
                }
                string orderStatusTagValue = TagDatabaseManager.GetInstance.GetOrderStatusValue(orderStatus);
                foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                {
                    OrderSingle or = tuple.Item2;
                    if (or != null)
                    {
                        SetDefaultValuesForLiveOrder(or, orderStatus, orderStatusTagValue);
                        or.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                        or.AvgPriceForCompliance = or.AvgPrice;
                        or.AvgPrice = 0;
                        or.CumQty = 0;
                        or.IsStageRequired = false;
                        or.TradeApplicationSource = Convert.ToInt32(TradeApplicationSource.Enterprise);
                        SendTradeToBlotter(tuple, ref sucessfullTradeCounter, isOrderValidated);
                    }
                }
                iMultiTradingTicketView.DeleteRowsFromGrid();
                TradeManagerExtension.GetInstance().SendMultiTradeDetails(MultiTradeId, sucessfullTradeCounter);
            }
            catch (Exception ex)
            {
                _isPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                iMultiTradingTicketView.EnableOrDisableMTT(true);
            }
        }

        /// <summary>
        /// Creates the order for non stage trade.
        /// Called when trades are not opened from PTT.
        /// In both the cases, live and done away
        /// </summary>
        private void CreateManualOrder(List<Tuple<int, OrderSingle, double>> listCheckedOrders)
        {
            try
            {
                foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                {
                    OrderSingle orderSingle = tuple.Item2;
                    if (orderSingle != null)
                    {
                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(orderSingle.AUECLocalDate))
                        {
                            MessageBox.Show("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                + "). Please reach out to your Support Team for further assistance", "NAV Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                iMultiTradingTicketView.EnableOrDisableMTT(false);
                int sucessfullTradeCounter = 0;
                string orderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew);
                foreach (Tuple<int, OrderSingle, double> tuple in listCheckedOrders)
                {
                    OrderSingle orderSingle = tuple.Item2;
                    if (orderSingle != null)
                    {
                        // the following variable is to check whether trade manager has forwarded the trade or not
                        orderSingle.OrderStatus = orderStatus;
                        orderSingle.DiscretionInst = "0";
                        orderSingle.CumQtyForSubOrder = orderSingle.CumQty;
                        orderSingle.IsManualOrder = true;

                        if (!TradingRulesValidator.ValidateCompanyTradingRules(orderSingle, orderSingle.AvgPrice, false))
                        {
                            continue;
                        }
                        if (iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.PTT || iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.WatchList
                             || iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.OptionChain)
                        {
                            orderSingle.IsStageRequired = true;
                            orderSingle.CumQty = 0;
                            orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                        }

                        if (orderSingle.TransactionSource == TransactionSource.None || orderSingle.TransactionSource == TransactionSource.FIX)
                        {
                            orderSingle.TransactionSource = TransactionSource.TradingTicket;
                            orderSingle.TransactionSourceTag = (int)TransactionSource.TradingTicket;
                        }

                        orderSingle.IsPricingAvailable = _isPricingAvailable;
                        orderSingle.TradeApplicationSource = Convert.ToInt32(TradeApplicationSource.Enterprise);
                        SendTradeToBlotter(tuple, ref sucessfullTradeCounter);
                    }
                }
                iMultiTradingTicketView.DeleteRowsFromGrid();
                // send the multi trade details to the trade server
                TradeManagerExtension.GetInstance().SendMultiTradeDetails(MultiTradeId, sucessfullTradeCounter);
            }
            catch (Exception ex)
            {
                _isPricingAvailable = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                iMultiTradingTicketView.EnableOrDisableMTT(true);
            }
        }

        /// <summary>
        /// A function is called for binding data for venue combo when broker value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTTView_UpdateVenueOnBrokerChangeBulk(object sender, EventArgs<string> e)
        {
            try
            {
                BindDataInVenueBasedOnBrokerIDBulk(e.Value);
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
        /// Handles the UpdateVenueOnBrokerChange event of the MTTView control.
        /// Called when any broker is changed
        /// Updates the datasource of venue utlragridcell
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{System.Int32}"/> instance containing the event data.</param>
        private void MTTView_UpdateVenueOnBrokerChange(object sender, EventArgs<int> e)
        {
            try
            {
                BindDataInVenueBasedOnBrokerID(e.Value);
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
        /// Sets the order values from row.
        /// Sets the default values of few tags, calls before validating the ordersingle
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="orderRow">The order row.</param>
        private void SetOrderValuesFromRow(ref OrderSingle order, ref UltraGridRow orderRow)
        {
            try
            {
                if (_tradingTicketType == TradingTicketType.Manual)
                {
                    double targetQuantity = 0.0;
                    if (order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild)
                    {
                        order.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDManualSub;
                    }
                    else
                    {
                        order.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDManual;
                    }
                    if (orderRow.Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value != null && Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value) > 0)
                    {
                        targetQuantity = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_EXECUTED_QTY].Value);
                        if (orderRow.Cells[OrderFields.PROPERTY_AVGPRICE].Value != null && Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_AVGPRICE].Value) > 0)
                        {
                            order.AvgPrice = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_AVGPRICE].Value);
                            order.TransactionTime = Convert.ToDateTime(orderRow.Cells[OrderFields.PROPERTY_AUECLOCALDATE].Value).Date.AddHours(12);
                        }
                        if (iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.PTT || iMultiTradingTicketView.TradingTicketParent == TradingTicketParent.WatchList)
                        {
                            order.CumQtyForSubOrder = targetQuantity;
                        }
                        else
                        {
                            order.CumQty = targetQuantity;
                        }
                    }
                    else
                    {
                        order.CumQty = 0;
                        order.AvgPrice = 0;
                    }
                }
                else
                {
                    if (orderRow.Cells[OrderFields.PROPERTY_QUANTITY].Value != null && Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_QUANTITY].Value) > 0)
                    {
                        order.Quantity = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_QUANTITY].Value);
                    }

                    if (orderRow.Cells[OrderFields.PROPERTY_PRICE].Value != null && Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_PRICE].Value) > 0)
                    {
                        order.Price = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_PRICE].Value);
                    }
                }
                order.Quantity = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_QUANTITY].Value);
                order.MultiTradeName = GetMultiTradeName();
                order.MultiTradeId = MultiTradeId;
                order.CompanyUserID = iMultiTradingTicketView.LoginUser.CompanyUserID;
                order.ModifiedUserId = iMultiTradingTicketView.LoginUser.CompanyUserID;
                order.ActualCompanyUserID = iMultiTradingTicketView.LoginUser.CompanyUserID;
                order.CompanyUserName = iMultiTradingTicketView.LoginUser.ShortName;
                order.ClientTime = DateTime.Now.ToLongTimeString();

                if (orderRow.Cells[OrderFields.PROPERTY_STOP_PRICE].Value != null)
                {
                    if (Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_STOP_PRICE].Value.ToString()) == double.Epsilon)
                    {
                        order.StopPrice = 0.0;
                    }
                    else
                        order.StopPrice = Convert.ToDouble(orderRow.Cells[OrderFields.PROPERTY_STOP_PRICE].Value);
                }
                else
                    order.StopPrice = 0.0;


                if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Market || order.OrderTypeTagValue == FIXConstants.ORDTYPE_Pegged || order.OrderTypeTagValue == FIXConstants.ORDTYPE_MarketOnClose ||
                order.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop)
                {
                    order.Price = 0.0;
                }
                if (order.OrderTypeTagValue != FIXConstants.ORDTYPE_Stop && order.OrderTypeTagValue != FIXConstants.ORDTYPE_Stoplimit)
                {
                    order.StopPrice = 0.0;
                }
                order.TransactionTime = DateTime.Now.ToUniversalTime();
                order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(orderRow.Cells[OrderFields.PROPERTY_HANDLING_INST_TagValue].Value.ToString());
                order.OrderSideTagValue = orderRow.Cells[OrderFields.PROPERTY_ORDER_SIDE].Value == null ? String.Empty : TagDatabaseManager.GetInstance.GetOrderSideValue(orderRow.Cells[OrderFields.PROPERTY_ORDER_SIDE].Value.ToString());
                order.TIF = orderRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value == null ? String.Empty : TagDatabaseManager.GetInstance.GetTIFTagValueBasedOnText(orderRow.Cells[OrderFields.PROPERTY_TIF_TAGVALUE].Value.ToString());
                order.ExecutionInstruction = orderRow.Cells[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Value == null ? String.Empty : TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(orderRow.Cells[OrderFields.PROPERTY_EXECUTION_INST_TagValue].Value.ToString());
                string strategy = Convert.ToString(orderRow.Cells[OrderFields.PROPERTY_STRATEGY].Value);
                int strategyValue = 0;
                if (int.TryParse(strategy, out strategyValue))
                    order.Level2ID = strategyValue;
                string brokerName = Convert.ToString(orderRow.Cells[OrderFields.PROPERTY_COUNTERPARTY_NAME].Value);
                order.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(brokerName);
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
        /// Sets the trade attribute caption.
        /// Updating the caption of tradeAttributes
        /// </summary>
        private void SetTradeAttributeCaption()
        {
            try
            {
                string lblTA1 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute1);
                string lblTA2 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute2);
                string lblTA3 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute3);
                string lblTA4 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute4);
                string lblTA5 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute5);
                string lblTA6 = CachedDataManager.GetInstance.GetAttributeNameForValue(TradingTicketConstants.CAPTION_TradeAttribute6);
                iMultiTradingTicketView.SetTradeAttributeCaptions(lblTA1, lblTA2, lblTA3, lblTA4, lblTA5, lblTA6);
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
        /// Get the limit and Stop Increment
        /// values from TT prefernces,if any.
        /// </summary>
        private void SetIncrementValues()
        {
            try
            {
                decimal limitIncrement = decimal.MinValue;
                if (Decimal.TryParse(TradingTktPrefs.UserTradingTicketUiPrefs.IncrementOnLimit.ToString(), out limitIncrement))
                {
                    iMultiTradingTicketView.SetLimitIncrement(limitIncrement);
                }
                else if (Decimal.TryParse(TradingTktPrefs.CompanyTradingTicketUiPrefs.IncrementOnLimit.ToString(), out limitIncrement))
                {
                    iMultiTradingTicketView.SetLimitIncrement(limitIncrement);
                }
                decimal stopIncrement = decimal.MinValue;
                if (Decimal.TryParse(TradingTktPrefs.UserTradingTicketUiPrefs.IncrementOnStop.ToString(), out stopIncrement))
                {
                    iMultiTradingTicketView.SetStopIncrement(stopIncrement);
                }
                else if (Decimal.TryParse(TradingTktPrefs.CompanyTradingTicketUiPrefs.IncrementOnStop.ToString(), out stopIncrement))
                {
                    iMultiTradingTicketView.SetStopIncrement(stopIncrement);
                }
                decimal quantityIncrement = decimal.MinValue;
                if (Decimal.TryParse(TradingTktPrefs.UserTradingTicketUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                {
                    iMultiTradingTicketView.SetQuantityIncrement(quantityIncrement);
                }
                else if (Decimal.TryParse(TradingTktPrefs.CompanyTradingTicketUiPrefs.IncrementOnQty.ToString(), out quantityIncrement))
                {
                    iMultiTradingTicketView.SetQuantityIncrement(quantityIncrement);
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
        /// Set the Trade Attributes
        /// </summary>
        /// <param name="vls">The VLS.</param>
        /// <param name="columnName">Name of the column.</param>
        private void TradeAttributeValue(ValueListItemsCollection vls, string columnName)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add(TradingTicketConstants.LIT_VALUE);
                table.Columns.Add(TradingTicketConstants.LIT_DISPLAY);
                if (vls != null)
                {
                    foreach (ValueListItem t in vls)
                    {
                        table.Rows.Add(t.DataValue, t.DisplayText);
                    }
                    iMultiTradingTicketView.AddDropDownForGivenColumn(vls.ValueList, columnName);
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
        /// Sets the trade attribute value list.
        /// </summary>
        /// <param name="vls">The VLS.</param>
        private void SetTradeAttributeValueList(BindableValueList[] vls)
        {
            try
            {
                TradeAttributeValue(vls[0].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE1);
                TradeAttributeValue(vls[1].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE2);
                TradeAttributeValue(vls[2].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE3);
                TradeAttributeValue(vls[3].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE4);
                TradeAttributeValue(vls[4].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE5);
                TradeAttributeValue(vls[5].ValueListItems, OrderFields.PROPERTY_TRADEATTRIBUTE6);
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
        /// Handles the TagDatabaseManagerWork event of the MTTView control.
        /// Mapping Id to DisplayName
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MTTView_TagDatabaseManagerWork(object sender, EventArgs e)
        {
            try
            {
                MapIdToTagValue();
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
        /// Validates the order single before trade to see if any row contains incorrect value on MTT grid.
        /// </summary>
        /// <param name="orderRow">The order row.</param>
        /// <param name="order">The order.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private bool ValidateOrderSingle(UltraGridRow orderRow, OrderSingle order, out StringBuilder result)
        {
            result = new StringBuilder();
            try
            {
                SetOrderValuesFromRow(ref order, ref orderRow);

                if (iMultiTradingTicketView.PriceSymbolSettings.RiskCtrlCheck || iMultiTradingTicketView.PriceSymbolSettings.ValidateSymbolCheck)
                {
                    if (IsLiveFeedConnected())
                    {
                        if (iMultiTradingTicketView.PriceSymbolSettings.RiskCtrlCheck)
                        {
                            if (order.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit || order.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOnClose ||
                            order.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitOrBetter || order.OrderTypeTagValue == FIXConstants.ORDTYPE_LimitWithOrWithout)
                            {
                                if (!TradeManagerCore.GetInstance().IsWithinLimits(order, Convert.ToDouble(orderRow.Cells[TradingTicketConstants.COLUMN_MARKETPRICE].Value)))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(GetErrorString(), String.Empty, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }

                if (order.MsgType != FIXConstants.MSGOrderCancelReplaceRequest && order.MsgType != FIXConstants.MSGExecutionReport &&
                order.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX && order.MsgType != CustomFIXConstants.MSGAlgoSyntheticReplaceOrder)
                {
                    order.MsgType = FIXConstants.MSGOrder;
                }

                if (order.AssetID == (int)AssetCategory.FX || order.AssetID == (int)AssetCategory.FXForward)
                {
                    if (!FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(order))
                    {
                        result = result.Append(TradingTicketConstants.MSG_PLEASE_ENTER_VALID_SYMBOL);
                        return false;
                    }
                }
                if (order.Quantity == 0.0)
                {
                    result = result.Append(TradingTicketConstants.MSG_VALID_QUANTITY);
                    return false;
                }

                if (String.IsNullOrEmpty(order.UnderlyingSymbol) || order.UnderlyingSymbol == TradingTicketConstants.LIT_NOT_AVAILABLE)
                {
                    order.UnderlyingSymbol = string.Empty;
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            result = result.Append("");
            return false;
        }
    }
}