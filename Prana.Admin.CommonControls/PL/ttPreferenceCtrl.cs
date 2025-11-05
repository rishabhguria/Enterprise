using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientPreferences;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.ExtensionUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Prana.Admin.CommonControls.PL
{
    public partial class ttPreferenceCtrl : UserControl
    {
        public ttPreferenceCtrl()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode())
            {
                var numericUpDownEx = GetAll(this, typeof(PranaNumericUpDown));
                foreach (PranaNumericUpDown numericUpDownExEditor in numericUpDownEx)
                {
                    numericUpDownExEditor.AutoSelect = true;
                }
            }

        }

        private TradingTicketPreferenceType _ttPreferenceType;
        private int _userID = int.MinValue;
        private int _companyID = int.MinValue;
        private bool _isDataSaved = true;

        public bool IsDataSaved
        {
            get { return _isDataSaved; }
            set { _isDataSaved = value; }
        }

        /// <summary>
        ///is equity option manual validation
        /// </summary>
        bool _isEquityOptionManualValidation;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is equity option manual validation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is equity option manual validation; otherwise, <c>false</c>.
        /// </value>
        public bool IsEquityOptionManualValidation
        {
            get { return _isEquityOptionManualValidation; }
            set { _isEquityOptionManualValidation = value; }
        }

        public TradingTicketPreferenceType TTPreferenceType
        {
            get { return _ttPreferenceType; }
            set { _ttPreferenceType = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public bool GetIsEquityOptionManualValidation()
        {
            return IsEquityOptionManualValidation;
        }

        public void SetupControl(int userID, int companyID, bool isEquityOptionManualValidation)
        {
            _userID = userID;
            _companyID = companyID;
            this._isEquityOptionManualValidation = isEquityOptionManualValidation;
            ucbEquityOptionManualValidation.Checked = isEquityOptionManualValidation;
            TradingTicketPrefManager.GetInstance.Initialise(_ttPreferenceType, _userID, _companyID);
            if (_ttPreferenceType == TradingTicketPreferenceType.Company)
            {
                ugbxNumerics.Visible = false;
            }
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                ugbxShowSelections.Visible = false;
                ugbxFromControlToControl.Visible = false;


                tableLayoutPanel4.Controls.Remove(ulblVenue);
                tableLayoutPanel4.Controls.Remove(ulblOrderType);
                tableLayoutPanel4.Controls.Remove(ulblTIF);
                tableLayoutPanel4.Controls.Remove(ulblHandlingInstruction);
                tableLayoutPanel4.Controls.Remove(ucmbVenue);
                tableLayoutPanel4.Controls.Remove(ucmbOrderType);
                tableLayoutPanel4.Controls.Remove(ucmbTIF);
                tableLayoutPanel4.Controls.Remove(ucmbHandlingInst);

                tableLayoutPanel4.Controls.Remove(ucmbExecInst);
                tableLayoutPanel4.Controls.Remove(ulblExecutionInst);
                tableLayoutPanel4.Controls.Remove(ulblStrategy);
                tableLayoutPanel4.Controls.Remove(ulblAccount);
                tableLayoutPanel4.Controls.Remove(ulblSettlCurrency);
                tableLayoutPanel4.Controls.Remove(ucmbTradingAcc);
                tableLayoutPanel4.Controls.Remove(ucmbStrategy);
                tableLayoutPanel4.Controls.Remove(ulblTradingAccount);
                tableLayoutPanel4.Controls.Remove(ultraPanel2);

                this.tableLayoutPanel4.Controls.Add(this.ulblOrderType, 0, 1);
                this.tableLayoutPanel4.Controls.Add(this.ulblTIF, 0, 2);
                this.tableLayoutPanel4.Controls.Add(this.ulblHandlingInstruction, 0, 3);
                this.tableLayoutPanel4.Controls.Add(this.ulblExecutionInst, 0, 4);
                this.tableLayoutPanel4.Controls.Add(this.ucmbOrderType, 1, 1);
                this.tableLayoutPanel4.Controls.Add(this.ucmbTIF, 1, 2);
                this.tableLayoutPanel4.Controls.Add(this.ucmbHandlingInst, 1, 3);
                this.tableLayoutPanel4.Controls.Add(this.ucmbExecInst, 1, 4);


                this.tableLayoutPanel4.Controls.Add(this.ulblStrategy, 2, 0);
                this.tableLayoutPanel4.Controls.Add(this.ulblAccount, 2, 1);
                this.tableLayoutPanel4.Controls.Add(this.ulblSettlCurrency, 2, 2);
                this.tableLayoutPanel4.Controls.Add(this.ucmbTradingAcc, 3, 3);
                this.tableLayoutPanel4.Controls.Add(this.ucmbStrategy, 3, 0);
                this.tableLayoutPanel4.Controls.Add(this.ucmbAccount, 3, 1);
                this.tableLayoutPanel4.Controls.Add(this.ulblTradingAccount, 2, 3);
                this.tableLayoutPanel4.Controls.Add(this.ultraPanel2, 3, 2);
            }
            if (BLL.ModuleManager.CompanyModulesPermittedToUser == null)
            {
                BLL.ModuleManager.CompanyModulesPermittedToUser = BLL.ModuleManager.GetModulesForCompanyUser(int.MinValue);
            }
                TradingTicketPrefManager.GetInstance.GetPreferenceBindingData(
                                BLL.ModuleManager.CompanyModulesPermittedToUser.Cast<BLL.Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_LEVELING_MODULE)),
                                BLL.ModuleManager.CompanyModulesPermittedToUser.Cast<BLL.Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_PRORATA_NAV_MODULE)));

            SetDataSource(ucmbBroker, TradingTicketPrefManager.GetInstance.Brokers, "Name", "CounterPartyID");
            SetDataSource(ucmbOrderType, TradingTicketPrefManager.GetInstance.OrderTypes, "Type", "OrderTypesID");
            SetDataSource(ucmbTIF, TradingTicketPrefManager.GetInstance.TimeInForces, "Name", "TimeInForceID");
            SetDataSource(ucmbExecInst, TradingTicketPrefManager.GetInstance.ExecutionInstructions, "ExecutionInstructions", "ExecutionInstructionsID");
            SetDataSource(ucmbHandlingInst, TradingTicketPrefManager.GetInstance.HandlingInstructions, "Name", "HandlingInstructionID");
            SetDataSource(ucmbTradingAcc, TradingTicketPrefManager.GetInstance.TradingAccounts, "Name", "TradingAccountID");
            SetDataSource(ucmbAccount, TradingTicketPrefManager.GetInstance.Accounts, "Name", "AccountID");
            SetDataSource(ucmbStrategy, TradingTicketPrefManager.GetInstance.Strategies, "Name", "StrategyID");

            SetPreferencesOnUI();
        }
        DataTable dt = new DataTable();
        /// <summary>
        /// Bind DataSources TO Grid
        /// </summary>
        private void BindDataSourcesTOGrid()
        {
            try
            {
                dt.Clear();
                if (!dt.Columns.Contains("Select"))
                    dt.Columns.Add("Select");
                if (!dt.Columns.Contains("FromControl"))
                    dt.Columns.Add("FromControl");
                if (!dt.Columns.Contains("ToControl"))
                    dt.Columns.Add("ToControl");

                List<DefTTControlsMapping> list = new List<DefTTControlsMapping>();
                string controlsMapping = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.DefTTControlsMapping;
                list = !string.IsNullOrWhiteSpace(controlsMapping) ? JsonConvert.DeserializeObject<List<DefTTControlsMapping>>(controlsMapping) : new List<DefTTControlsMapping>();

                if (list != null)
                {
                    foreach (DefTTControlsMapping row in list)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FromControl"] = row.FromControl;
                        dr["ToControl"] = row.ToControl;
                        dr["Select"] = false;
                        dt.Rows.Add(dr);
                    }
                }
                grdFromControlToControl.DataSource = dt;
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
        /// Create ValueList From Order Properties
        /// </summary>
        /// <returns></returns>
        private ValueList CreateValueListFromOrderProperties()
        {
            ValueList vl1 = new ValueList();
            try
            {
                Order od = new Order();
                Type t = od.GetType();
                PropertyInfo[] props = t.GetProperties();
                if (props != null)
                {
                    foreach (PropertyInfo prp in props)
                    {
                        string propName = prp.Name;
                        if (propName.StartsWith(ApplicationConstants.CONST_TRADE_ATTRIBUTE) && int.TryParse(propName.Replace(ApplicationConstants.CONST_TRADE_ATTRIBUTE, ""), out int attrNum) && attrNum >= 7 && attrNum <= 45)
                        {
                            // Skip Additional Trade Attributes (TradeAttribute7 to TradeAttribute45)
                            continue;
                        }
                        vl1.ValueListItems.Add(propName, prp.Name);
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
            return vl1;
        }
        /// <summary>
        /// ultraGrid1 InitializeLayout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                grdFromControlToControl.DisplayLayout.Bands[0].Columns["FromControl"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdFromControlToControl.DisplayLayout.Bands[0].Columns["FromControl"].ValueList = CreateValueListFromOrderProperties();
                grdFromControlToControl.DisplayLayout.Bands[0].Columns["FromControl"].Header.Caption = "From Property";

                grdFromControlToControl.DisplayLayout.Bands[0].Columns["ToControl"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdFromControlToControl.DisplayLayout.Bands[0].Columns["ToControl"].ValueList = CreateValueListFromOrderProperties();
                grdFromControlToControl.DisplayLayout.Bands[0].Columns["ToControl"].Header.Caption = "To Property";

                grdFromControlToControl.DisplayLayout.Bands[0].Columns["Select"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
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
        /// btnAdd Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = dt.NewRow();
                dr["Select"] = "false";
                dr["FromControl"] = "-Select-";
                dr["ToControl"] = "-Select-";
                dt.Rows.Add(dr);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (Convert.ToBoolean(dr["Select"]) == true)
                        dr.Delete();
                }
                dt.AcceptChanges();
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
        private void SetPreferencesOnUI()
        {
            grdAssetSide.DataSource = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.DefAssetSides;
            BindDataSourcesTOGrid();
            SetBrokerPreference();
            SetOrderTypePreference();
            SetTimeInForcePreference();
            SetExecutionInstructionPreference();
            SetHandlingInstructionPreference();
            SettradingAccountPreference();
            SetAccountPreference();
            SetStrategyPreference();
            SetSettlementCurrencyPreference();
            SetShowTargetQTYPreference();
            decimal numQuantity = 0;
            decimal incQuantity = 0;
            decimal incLimit = 0;
            decimal incStop = 0;
            decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Quantity.ToString(), out numQuantity);
            decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IncrementOnQty.ToString(), out incQuantity);
            decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IncrementOnLimit.ToString(), out incLimit);
            decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IncrementOnStop.ToString(), out incStop);

            uNumQuantity.Value = numQuantity;
            uNumIncQty.Value = incQuantity;
            uNumIncLimit.Value = incLimit;
            uNumIncStop.Value = incStop;

            //Hide the panal of radio buttons of $Amount and Quantity from UI preference if Dollar Amount Permission is not given to TT from the admin  
            if (!Boolean.Parse(TradingTicketPrefManager.DollarAmountPermission.ToString()))
            {
                pnlDefaultAQuantity.Visible = false;
                setDefaultQuantityTo.Visible = false;
            }

            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.QuantityType == QuantityTypeOnTT.Amount)
            {
                rBtnAmount.Checked = true;
            }
            else
            {
                rBtnQuantity.Checked = true;
            }
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsUseRoundLots)
            {
                rbtnRoundLotYes.Checked = true;
                uNumIncQty.Enabled = false;
            }
            else
            {
                rBtnRoundLotNo.Checked = true;
                uNumIncQty.Enabled = true;
            }
        }

        private void SetSettlementCurrencyPreference()
        {
            try
            {
                bool? isSettlmentCurrencySetAsBase = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsSettlementCurrencyBase;
                if (isSettlmentCurrencySetAsBase != null)
                {
                    if (isSettlmentCurrencySetAsBase.HasValue)
                    {
                        if ((bool)isSettlmentCurrencySetAsBase)
                        {
                            rdbtnBase.Checked = true;
                        }
                        else
                        {
                            rdbtnLocal.Checked = true;
                        }
                    }
                }
                else
                {
                    if (_ttPreferenceType == TradingTicketPreferenceType.User)
                    {
                        bool? isSettlmentCurrencySetAsBaseFromAdmin = TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.IsSettlementCurrencyBase;
                        if (isSettlmentCurrencySetAsBaseFromAdmin != null && isSettlmentCurrencySetAsBaseFromAdmin.HasValue)
                        {
                            if ((bool)isSettlmentCurrencySetAsBaseFromAdmin)
                            {
                                rdbtnBase.Checked = true;
                            }
                            else
                            {
                                rdbtnLocal.Checked = true;
                            }
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
        /// Set Show Hide Target QTY Preference
        /// </summary>
        private void SetShowTargetQTYPreference()
        {
            try
            {
                bool? isSShowargetQTY = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.IsShowTargetQTY;
                if (isSShowargetQTY != null)
                {
                    if (isSShowargetQTY.HasValue)
                    {
                        if ((bool)isSShowargetQTY)
                        {
                            ucbTargetQTY.Checked = true;
                        }
                        else
                        {
                            ucbTargetQTY.Checked = false;
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
        private void SetStrategyPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Strategy != null)
            {
                ucmbStrategy.Value =
                    TradingTicketPrefManager.GetInstance.Strategies.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Strategy.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Strategy
                        : null;
            }
            else
            {
                ucmbStrategy.Value = null;
            }
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Strategy != null && TradingTicketPrefManager.GetInstance.Strategies.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Strategy.Value))
                    ucmbStrategy.NullText = TradingTicketPrefManager.GetInstance.Strategies.Cast<Strategy>().First(strategy => strategy.StrategyID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Strategy.Value).Name;
            }
        }

        private void SetAccountPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Account != null)
            {
                ucmbAccount.Value =
                    TradingTicketPrefManager.GetInstance.Accounts.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Account.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Account
                        : null;
            }
            else
            {
                ucmbAccount.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Account != null && TradingTicketPrefManager.GetInstance.Accounts.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Account.Value))
                    ucmbAccount.NullText = TradingTicketPrefManager.GetInstance.Accounts.Cast<Account>().First(account => account.AccountID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Account.Value).Name;
            }
        }

        private void SettradingAccountPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TradingAccount != null)
            {
                ucmbTradingAcc.Value =
                    TradingTicketPrefManager.GetInstance.TradingAccounts.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TradingAccount.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TradingAccount
                        : null;
            }
            else
            {
                ucmbTradingAcc.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TradingAccount != null && TradingTicketPrefManager.GetInstance.TradingAccounts.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TradingAccount.Value))
                    ucmbTradingAcc.NullText = TradingTicketPrefManager.GetInstance.TradingAccounts.Cast<TradingAccount>().First(tradingAccount => tradingAccount.TradingAccountID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TradingAccount.Value).Name;
            }
        }

        private void SetHandlingInstructionPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.HandlingInstruction != null)
            {
                ucmbHandlingInst.Value =
                    TradingTicketPrefManager.GetInstance.HandlingInstructions.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.HandlingInstruction.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.HandlingInstruction
                        : null;
            }
            else
            {
                ucmbHandlingInst.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.HandlingInstruction != null && TradingTicketPrefManager.GetInstance.HandlingInstructions.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.HandlingInstruction.Value))
                    ucmbHandlingInst.NullText = TradingTicketPrefManager.GetInstance.HandlingInstructions.Cast<HandlingInstruction>().First(handlingInstruction => handlingInstruction.HandlingInstructionID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.HandlingInstruction.Value).Name;
            }
        }

        private void SetExecutionInstructionPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.ExecutionInstruction != null)
            {
                ucmbExecInst.Value =
                    TradingTicketPrefManager.GetInstance.ExecutionInstructions.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.ExecutionInstruction.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.ExecutionInstruction
                        : null;
            }
            else
            {
                ucmbExecInst.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.ExecutionInstruction != null && TradingTicketPrefManager.GetInstance.ExecutionInstructions.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.ExecutionInstruction.Value))
                    ucmbExecInst.NullText = TradingTicketPrefManager.GetInstance.ExecutionInstructions.Cast<ExecutionInstruction>().First(executionInstruction => executionInstruction.ExecutionInstructionsID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.ExecutionInstruction.Value).ExecutionInstructions;
            }
        }

        private void SetTimeInForcePreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TimeInForce != null)
            {
                ucmbTIF.Value =
                    TradingTicketPrefManager.GetInstance.TimeInForces.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TimeInForce.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.TimeInForce
                        : null;
            }
            else
            {
                ucmbTIF.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TimeInForce != null && TradingTicketPrefManager.GetInstance.TimeInForces.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TimeInForce.Value))
                    ucmbTIF.NullText = TradingTicketPrefManager.GetInstance.TimeInForces.Cast<TimeInForce>().First(timeInForce => timeInForce.TimeInForceID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.TimeInForce.Value).Name;
            }
        }

        private void SetOrderTypePreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.OrderType != null)
            {
                ucmbOrderType.Value =
                    TradingTicketPrefManager.GetInstance.OrderTypes.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.OrderType.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.OrderType
                        : null;
            }
            else
            {
                ucmbOrderType.Value = null;
            }
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.OrderType != null && TradingTicketPrefManager.GetInstance.OrderTypes.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.OrderType.Value))
                    ucmbOrderType.NullText = TradingTicketPrefManager.GetInstance.OrderTypes.Cast<OrderType>().First(orderType => orderType.OrderTypesID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.OrderType.Value).Type;
            }
        }

        private void SetBrokerPreference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Broker != null)
            {
                ucmbBroker.Value =
                    TradingTicketPrefManager.GetInstance.Brokers.Contains(
                        TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Broker.Value)
                        ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Broker
                        : null;
            }
            else
            {
                ucmbBroker.Value = null;
            }
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Broker != null && TradingTicketPrefManager.GetInstance.Brokers.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Broker.Value))
                    ucmbBroker.NullText = TradingTicketPrefManager.GetInstance.Brokers.Cast<CounterParty>().First(broker => broker.CounterPartyID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Broker.Value).Name;
            }
        }

        /// <summary>
        /// Disable Broker field based on isEnabled parameter
        /// </summary>
        public void EnableDisableBroker(bool isEnabled)
        {
            try
            {
                ucmbBroker.Enabled = isEnabled;               
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
        /// Disable Broker field and set a valid value when field is disabled on Save
        /// </summary>
        public void SetBrokerFieldOnSave(bool isEnabled)
        {
            try
            {
                EnableDisableBroker(isEnabled);
                if (!isEnabled && ucmbBroker.Value != null && !ValueListUtilities.CheckIfValueExistsInValuelist(ucmbBroker.ValueList, ucmbBroker.Value.ToString()))
                {
                    int? broker = TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Broker;
                    ucmbBroker.Value = broker != null && TradingTicketPrefManager.GetInstance.Brokers.Contains(broker.Value) ? broker : null;
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

        private void SetVenuePrference()
        {
            if (TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Venue != null)
            {
                ucmbVenue.Value =
                TradingTicketPrefManager.GetInstance.Venues.Contains(
                 TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Venue.Value)
                 ? TradingTicketPrefManager.GetInstance.TradingTicketUiPrefs.Venue
                 : null;
            }
            else
            {
                ucmbVenue.Value = null;
            }

            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                if (TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Venue != null && TradingTicketPrefManager.GetInstance.Venues.Contains(TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Venue.Value))
                    ucmbVenue.NullText = TradingTicketPrefManager.GetInstance.Venues.Cast<Venue>().First(venue => venue.VenueID == TradingTicketPrefManager.GetInstance.CompanyTradingTicketUiPrefs.Venue.Value).Name;
            }
        }

        private void SetDataSource(UltraComboEditor ultraComboEditor, object dataSource, string displayMember, string valueMember)
        {
            ultraComboEditor.Value = null;
            ultraComboEditor.DataSource = dataSource;
            ultraComboEditor.DisplayMember = displayMember;
            ultraComboEditor.ValueMember = valueMember;
        }

        private void ultrComboEditor_ValueChanged(object sender, EventArgs e)
        {
            if (ucmbBroker.Value != null)
            {
                int brokerID;
                if (Int32.TryParse(ucmbBroker.Value.ToString(), out brokerID))
                {
                    TradingTicketPrefManager.GetInstance.GetVenuesBasedOnSelectedCounterparty(brokerID);
                    SetDataSource(ucmbVenue, TradingTicketPrefManager.GetInstance.Venues, "Name", "VenueID");
                    SetVenuePrference();
                }
            }
        }

        private void AmountRadiobtn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rBtnAmount.Checked)
                {
                    ulblQuantity.Text = ApplicationConstants.DOLLAR_NOTIONAL;
                    ulblIncQty.Text = ApplicationConstants.DOLLAR_INCREASE_ON_NOTIONAL;
                }
                else
                {
                    ulblQuantity.Text = ApplicationConstants.QUANTITY;
                    ulblIncQty.Text = ApplicationConstants.INCREASE_ON_QUANTITY;
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

        private void rbtnRoundLotYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnRoundLotYes.Checked)
                {
                    uNumIncQty.Enabled = false;
                }
                else
                {
                    uNumIncQty.Enabled = true;
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
        private void spinner_Validated(object sender, EventArgs e)
        {
            if (((PranaNumericUpDown)sender).Value == 0)
            {
                ((PranaNumericUpDown)sender).Value = 0.0001M;
            }
        }
        private void grdAssetSide_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraDropDown cmbAsset = new UltraDropDown
            {
                DataSource = TradingTicketPrefManager.GetInstance.AssetSides.Keys,
                DisplayMember = "Name",
                ValueMember = "AssetID"
            };

            grdAssetSide.DisplayLayout.Bands[0].Columns["Asset"].ValueList = cmbAsset;
            grdAssetSide.DisplayLayout.Bands[0].Columns["Asset"].Header.Caption = "Asset";
            grdAssetSide.DisplayLayout.Bands[0].Columns["Asset"].CellActivation = Activation.NoEdit;
            grdAssetSide.DisplayLayout.Bands[0].Columns["Asset"].Header.VisiblePosition = 0;
            grdAssetSide.DisplayLayout.Bands[0].Columns["Asset"].Header.Fixed = true;

            grdAssetSide.DisplayLayout.Bands[0].Columns["OrderSide"].Header.Caption = "Order Side";
            grdAssetSide.DisplayLayout.Bands[0].Columns["OrderSide"].CellActivation = Activation.AllowEdit;
            grdAssetSide.DisplayLayout.Bands[0].Columns["OrderSide"].Header.VisiblePosition = 1;
            grdAssetSide.DisplayLayout.Bands[0].Columns["OrderSide"].Header.Fixed = true;
            if (_ttPreferenceType == TradingTicketPreferenceType.User)
            {
                grdAssetSide.DisplayLayout.Bands[0].Columns["OrderSide"].NullText = "(Company Default)";
            }
        }

        private void grdAssetSide_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            Sides sides = GetSidesFromAssetID(Convert.ToInt32(e.Row.Cells["Asset"].Value));
            if (sides.Count > 0)
            {
                UltraDropDown cmbOrderSide = new UltraDropDown
                {
                    DataSource = sides,
                    DisplayMember = "Name",
                    ValueMember = "SideID"
                };
                cmbOrderSide.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbOrderSide.DisplayLayout.Bands[0].Columns["SideID"].Hidden = true;
                cmbOrderSide.DisplayLayout.Bands[0].Columns["TagValue"].Hidden = true;
                cmbOrderSide.DisplayLayout.Bands[0].Columns["AUECID"].Hidden = true;
                cmbOrderSide.DisplayLayout.Bands[0].Columns["CVAUECID"].Hidden = true;

                e.Row.Cells["OrderSide"].ValueList = cmbOrderSide;
                if (e.Row.Cells["OrderSide"].Value != null)
                {
                    if (!ValueListUtilities.CheckIfValueExistsInValuelist(e.Row.Cells["OrderSide"].ValueList, e.Row.Cells["OrderSide"].Value.ToString()))
                    {
                        e.Row.Cells["OrderSide"].Value = null;
                    }
                }
            }
        }

        private Sides GetSidesFromAssetID(int assetID)
        {
            Asset key = TradingTicketPrefManager.GetInstance.AssetSides.Keys.First(x => x.AssetID == assetID);
            return TradingTicketPrefManager.GetInstance.AssetSides[key];
        }

        public TradingTicketUIPrefs SaveTradePreferences()
        {
            TradingTicketUIPrefs preferences = new TradingTicketUIPrefs();
            if (ValidateControlsBeforeSave() && ValidateGridBeforeSave())
            {
                preferences.IncrementOnStop = (double?)ReturnValueAfterConversion(uNumIncStop.Value, typeof(Double));
                preferences.IncrementOnLimit = (double?)ReturnValueAfterConversion(uNumIncLimit.Value, typeof(Double));
                preferences.IncrementOnQty = (double?)ReturnValueAfterConversion(uNumIncQty.Value, typeof(Double));
                preferences.Quantity = (double?)ReturnValueAfterConversion(uNumQuantity.Value, typeof(Double));
                preferences.IsSettlementCurrencyBase = rdbtnBase.Checked ? true : false;
                preferences.Strategy = (int?)ReturnValueAfterConversion(ucmbStrategy.Value, typeof(Int32));
                preferences.Account = (int?)ReturnValueAfterConversion(ucmbAccount.Value, typeof(Int32));
                preferences.TradingAccount = (int?)ReturnValueAfterConversion(ucmbTradingAcc.Value, typeof(Int32));
                preferences.HandlingInstruction =
                    (int?)ReturnValueAfterConversion(ucmbHandlingInst.Value, typeof(Int32));
                preferences.ExecutionInstruction = (int?)ReturnValueAfterConversion(ucmbExecInst.Value, typeof(Int32));
                preferences.TimeInForce = (int?)ReturnValueAfterConversion(ucmbTIF.Value, typeof(Int32));
                preferences.OrderType = (int?)ReturnValueAfterConversion(ucmbOrderType.Value, typeof(Int32));
                preferences.Venue = _ttPreferenceType == TradingTicketPreferenceType.User ? null : (int?)ReturnValueAfterConversion(ucmbVenue.Value, typeof(Int32));
                preferences.Broker = (int?)ReturnValueAfterConversion(ucmbBroker.Value, typeof(Int32));
                preferences.QuantityType = rBtnQuantity.Checked ? QuantityTypeOnTT.Quantity : QuantityTypeOnTT.Amount;
                preferences.IsShowTargetQTY = ucbTargetQTY.Checked ? true : false;
                preferences.DefAssetSides = (List<DefAssetSide>)grdAssetSide.DataSource;
                preferences.IsUseRoundLots = rbtnRoundLotYes.Checked ? true : false;

                DataTable dt = new DataTable();
                dt = (DataTable)grdFromControlToControl.DataSource;
                List<DefTTControlsMapping> list = new List<DefTTControlsMapping>();


                list = (from rw in dt.AsEnumerable()
                        select new DefTTControlsMapping()
                        {
                            FromControl = rw["FromControl"].ToString(),
                            ToControl = (rw["ToControl"].ToString())
                        }).ToList();

                preferences.DefTTControlsMapping = JsonConvert.SerializeObject(list);
                TradingTicketPrefManager.GetInstance.SaveTTPreference(preferences);
                _isDataSaved = true;
                return preferences;
            }
            MessageBox.Show("Some values are incorrect, thus preferences cannot be saved");
            _isDataSaved = false;
            return preferences;
        }

        private static object ReturnValueAfterConversion(object controlValue, Type type)
        {
            object value = null;
            if (controlValue != null)
            {
                if (type == typeof(Int32))
                {
                    value = Convert.ToInt32(controlValue);
                }
                else if (type == typeof(Double))
                {
                    value = Convert.ToDouble(controlValue);
                }
                else if ((type == typeof(Double)))
                {
                    value = Convert.ToBoolean(controlValue);
                }
            }
            return value;
        }

        private bool ValidateGridBeforeSave()
        {
            bool isValidated = true;

            try
            {
                if (grdAssetSide.DisplayLayout.Rows.Any(row => row.DataErrorInfoResolved.HasErrors))
                {
                    isValidated = false;
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
            return isValidated;
        }


        private bool ValidateControlsBeforeSave()
        {
            bool isValidated = true;

            try
            {
                var ultraComobEditors = GetAll(this, typeof(UltraComboEditor));
                errorProvider.Clear();
                foreach (UltraComboEditor ucmbEditor in ultraComobEditors.Cast<UltraComboEditor>())
                {
                    if (ucmbEditor.Value == null)
                    {
                        continue;
                    }
                    if (!ValueListUtilities.CheckIfValueExistsInValuelist(ucmbEditor.ValueList, ucmbEditor.Value.ToString()))
                    {
                        isValidated = SetErrorProviderInformation(ucmbEditor);
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
            return isValidated;
        }

        private bool SetErrorProviderInformation(UltraComboEditor ucmbEditor)
        {
            errorProvider.SetIconPadding(ucmbEditor, -35);
            errorProvider.SetError(ucmbEditor, "Value for this field is incorrect");
            return false;
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            IEnumerable<Control> enumerable = controls as IList<Control> ?? controls.ToList();
            return enumerable.SelectMany(ctrl => GetAll(ctrl, type)).Concat(enumerable).Where(c => c.GetType() == type);
        }


        private void grdAssetSide_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = true;
                e.RaiseErrorEvent = false;
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

        private void grdAssetSide_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals("OrderSide"))
            {
                // If the cell is null or not activated return, no further action is needed
                //
                if ((!e.Cell.IsActiveCell))
                    return;
                if (e.Cell.Value == null)
                {
                    e.Cell.Row.DataErrorInfo.SetColumnError("OrderSide", String.Empty);
                }
                else
                {
                    if (!ValueListUtilities.CheckIfValueExistsInValuelist(e.Cell.ValueList, e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.DataErrorInfo.SetColumnError("OrderSide", "Please select a valid order side");
                        e.Cell.Row.DataErrorInfo.RowError = "The order side for the asset is invalid";
                    }
                    else
                    {
                        e.Cell.Row.DataErrorInfo.SetColumnError("OrderSide", String.Empty);
                    }
                }

                // Clean up the errors on the row if there are no column errors on that row.
                //
                if (e.Cell.Row.DataErrorInfoResolved.GetColumnsInError().Length == 0)
                    e.Cell.Row.DataErrorInfo.ClearErrors();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the ucbEquityOptionManualValidation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ucbEquityOptionManualValidation_CheckedChanged(object sender, System.EventArgs e)
        {
            _isEquityOptionManualValidation = ucbEquityOptionManualValidation.Checked;
        }

        /// <summary>
        /// To export data for automation
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
                exporter.Export(grdAssetSide, exportFilePath);
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
    }
}