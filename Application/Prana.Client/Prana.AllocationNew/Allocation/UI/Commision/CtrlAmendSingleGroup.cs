using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.MiscUtilities;
using Infragistics.Win;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using Prana.BusinessLogic;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Utilities.UIUtilities;
using System.Text.RegularExpressions;

//AYG 05282013: This User Control has been added to make changes on Edit Trade Control of a single Group

namespace Prana.AllocationNew
{

    public partial class ctrlAmendSingleGroup : UserControl
    {

        // this event is called when user click on cancel button, it unpins the editsingleGroup control        
        //public event EventHandler UnpinEditSingleTradeControl;

        internal event EventHandler CloseEditSingleGroup;

        private ValueList _sides = new ValueList();

        public ctrlAmendSingleGroup()
        {
            InitializeComponent();
        }

        private int _userID = 0;

        ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        ProxyBase<ICashManagementService> _cashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            set { _cashManagementServices = value; }

        }

        internal ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        /// <summary>
        /// Binds the default values to the combo-boxes
        /// </summary>
        internal void InitControl(int userID)
        {
            try
            {
                _userID = userID;
                BindSides();
                BindCounterParties();
                BindFXOperator();
                BindVenue();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Binds Sides on the SideComboBox
        /// </summary>

        private void BindSides()
        {
            try
            {
                DataTable dtSides = new DataTable();
                DataColumn colSideID = new DataColumn("SideID");
                colSideID.DataType = typeof(string);

                DataColumn colSideName = new DataColumn("SideName");
                colSideName.DataType = typeof(string);

                dtSides.Columns.Add(colSideID);
                dtSides.Columns.Add(colSideName);

                DataRow row = dtSides.NewRow();
                row[colSideID] = int.MinValue;
                row[colSideName] = ApplicationConstants.C_COMBO_SELECT;

                dtSides.Rows.Add(row);
                dtSides.Rows.Add(FIXConstants.SIDE_Buy, "Buy");
                dtSides.Rows.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
                dtSides.Rows.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
                dtSides.Rows.Add(FIXConstants.SIDE_Sell, "Sell");
                dtSides.Rows.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
                dtSides.Rows.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
                dtSides.Rows.Add(FIXConstants.SIDE_SellShort, "Sell short");
                BindCombo(cmbSide, dtSides, "SideName", "SideID");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        ///  Binds Venues on the CounterPartyVenueComboBox
        /// </summary>
        private void BindVenue()
        {
            try
            {
                DataTable dtVenues = new DataTable();
                DataColumn colVenueID = new DataColumn("VenueID");
                colVenueID.DataType = typeof(int);

                DataColumn colVenueName = new DataColumn("VenueName");
                colVenueName.DataType = typeof(string);

                dtVenues.Columns.Add(colVenueID);
                dtVenues.Columns.Add(colVenueName);

                DataRow row = dtVenues.NewRow();
                row[colVenueID] = int.MinValue;
                row[colVenueName] = ApplicationConstants.C_COMBO_SELECT;

                dtVenues.Rows.Add(row);
                Dictionary<int, string> venues = Prana.CommonDataCache.CachedData.GetInstance().Venues;
                //Dictionary<int, string> venues = Prana.CommonDataCache.KeyValueDataManager.GetInstance().GetVenues();
                foreach (KeyValuePair<int, string> kvp in venues)
                {
                    DataRow dRow = dtVenues.NewRow();
                    dRow[colVenueID] = kvp.Key;
                    dRow[colVenueName] = kvp.Value;
                    dtVenues.Rows.Add(dRow);
                }

                BindCombo(cmbVenue, dtVenues, "VenueName", "VenueID");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Binds FXOperators on the FXOperatorComboBox
        /// </summary>

        private void BindFXOperator()
        {
            try
            {
                DataTable dtFXOperators = new DataTable();
                DataColumn colOperatorID = new DataColumn("OperatorID");
                colOperatorID.DataType = typeof(int);

                DataColumn colOperatorName = new DataColumn("OperatorName");
                colOperatorName.DataType = typeof(string);

                dtFXOperators.Columns.Add(colOperatorID);
                dtFXOperators.Columns.Add(colOperatorName);

                //DataRow row = dtFXOperators.NewRow();
                //row[colOperatorID] = int.MinValue;
                //row[colOperatorName] = "-";

                //dtFXOperators.Rows.Add(row);

                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {

                        DataRow dRow = dtFXOperators.NewRow();
                        dRow[colOperatorID] = var.Value;
                        dRow[colOperatorName] = var.DisplayText;
                        dtFXOperators.Rows.Add(dRow);
                    }
                }
                //Kuldeep A.: Changed this as this was causing FxConversionOperator to be saved as '0' or '1' rather than 'M' or 'D'.
                BindCombo(cmbFXConvOperator, dtFXOperators, "OperatorName", "OperatorName");
                BindCombo(cmbSettlFXConvOperator, dtFXOperators, "OperatorName", "OperatorName");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Binds CounterParties on the CounterPartyComboBox
        /// </summary>

        private void BindCounterParties()
        {
            try
            {
                DataTable dtCounterParties = new DataTable();
                DataColumn colCounterPartyID = new DataColumn("CounterPartyID");
                colCounterPartyID.DataType = typeof(int);

                DataColumn colCounterPartyName = new DataColumn("CounterPartyName");
                colCounterPartyName.DataType = typeof(string);

                dtCounterParties.Columns.Add(colCounterPartyID);
                dtCounterParties.Columns.Add(colCounterPartyName);

                DataRow row = dtCounterParties.NewRow();
                row[colCounterPartyID] = int.MinValue;
                row[colCounterPartyName] = ApplicationConstants.C_COMBO_SELECT;

                dtCounterParties.Rows.Add(row);

                Dictionary<int, string> userCounterParties = Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserCounterParties();
                foreach (KeyValuePair<int, string> kvp in userCounterParties)
                {
                    DataRow dRow = dtCounterParties.NewRow();
                    dRow[colCounterPartyID] = kvp.Key;
                    dRow[colCounterPartyName] = kvp.Value;
                    dtCounterParties.Rows.Add(dRow);
                }

                BindCombo(cmbCounterParty, dtCounterParties, "CounterPartyName", "CounterPartyID");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///  Binds data on a ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="dt"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>

        private void BindCombo(UltraCombo comboBox, DataTable dt, string displayMember, string valueMember)
        {
            try
            {
                comboBox.DataSource = null;
                comboBox.DataSource = dt;

                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;

                foreach (UltraGridColumn column in comboBox.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                    if (column.Key.Equals(displayMember))
                    {
                        column.Hidden = false;
                    }
                }
                comboBox.Text = ApplicationConstants.C_COMBO_SELECT;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        AllocationGroup _group = null;
        AllocationGroup _groupClone = null;

        /// <summary>
        ///  It binds the data from the group to the UI components 
        /// </summary>
        /// <param name="group"></param>
        internal void EditGroup(AllocationGroup group, AllocationGroup originalGroup, ValueList[] attribLists)
        {
            try
            {
                ClearDataBindings();

                _group = group;
                if (_group != null)
                {
                    BindData(_group);
                    _groupClone = originalGroup;
                }

                if (attribLists != null)
                {
                    txtAttribute1.ValueList = attribLists[0];
                    txtAttribute2.ValueList = attribLists[1];
                    txtAttribute3.ValueList = attribLists[2];
                    txtAttribute4.ValueList = attribLists[3];
                    txtAttribute5.ValueList = attribLists[4];
                    txtAttribute6.ValueList = attribLists[5];
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private double CalculateAccruedInterest(AllocationGroup group)
        {
            List<PranaBasicMessage> lstAllocationGroup = new List<PranaBasicMessage>();
            double accruedInterest = 0.0;
            try
            {
                lstAllocationGroup.Add(group);
                lstAllocationGroup = _cashManagementServices.InnerChannel.CalculateAccruedInterest(lstAllocationGroup);
                foreach (PranaBasicMessage obj in lstAllocationGroup)
                {
                    accruedInterest = obj.AccruedInterest;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accruedInterest;
        }

        /// <summary>
        /// Binds the data to all the controls on EditTrade Control
        /// </summary>
        /// <param name="group"></param>

        private void BindData(AllocationGroup group)
        {
            try
            {
                lblSymbol.DataBindings.Add("Text", group, "Symbol", true, DataSourceUpdateMode.OnPropertyChanged);
                lblAsset.DataBindings.Add("Text", group, "AssetName", true, DataSourceUpdateMode.OnPropertyChanged);
                txtExecutedQty.DataBindings.Add("Text", group, "CumQty", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAvgPrice.DataBindings.Add("Text", group, "AvgPrice", true, DataSourceUpdateMode.OnPropertyChanged);
                cmbSide.DataBindings.Add("Text", group, "OrderSide", true, DataSourceUpdateMode.OnPropertyChanged);
                dtpTradeDate.DataBindings.Add("DateTime", group, "AUECLocalDate", true, DataSourceUpdateMode.OnPropertyChanged);
                dtpProcessDate.DataBindings.Add("DateTime", group, "ProcessDate", true, DataSourceUpdateMode.OnPropertyChanged);
                dtpOriginalPurchaseDate.DataBindings.Add("DateTime", group, "OriginalPurchaseDate", true, DataSourceUpdateMode.OnPropertyChanged);
                dtpSettleDate.DataBindings.Add("DateTime", group, "SettlementDate", true, DataSourceUpdateMode.OnPropertyChanged);

                cmbCounterParty.DataBindings.Add("Text", group, "CounterPartyName", true, DataSourceUpdateMode.OnPropertyChanged);
                cmbVenue.DataBindings.Add("Text", group, "Venue", true, DataSourceUpdateMode.OnPropertyChanged);
                lblTotalComm.DataBindings.Add("Text", group, "TotalCommissionandFees", true, DataSourceUpdateMode.OnPropertyChanged);
                txtCommission.DataBindings.Add("Text", group, "Commission", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSoftCommission.DataBindings.Add("Text", group, "SoftCommission", true, DataSourceUpdateMode.OnPropertyChanged);
                txtOtherBrokerFees.DataBindings.Add("Text", group, "OtherBrokerFees", true, DataSourceUpdateMode.OnPropertyChanged);
                txtClearingBrokerFee.DataBindings.Add("Text", group, "ClearingBrokerFee", true, DataSourceUpdateMode.OnPropertyChanged);
                txtMiscFees.DataBindings.Add("Text", group, "MiscFees", true, DataSourceUpdateMode.OnPropertyChanged);
                txtClearingFee.DataBindings.Add("Text", group, "ClearingFee", true, DataSourceUpdateMode.OnPropertyChanged);
                txtTransactionLevy.DataBindings.Add("Text", group, "TransactionLevy", true, DataSourceUpdateMode.OnPropertyChanged);
                txtStampDuty.DataBindings.Add("Text", group, "StampDuty", true, DataSourceUpdateMode.OnPropertyChanged);
                txtTaxOnCommissions.DataBindings.Add("Text", group, "TaxOnCommissions", true, DataSourceUpdateMode.OnPropertyChanged);
                txtDescription.DataBindings.Add("Text", group, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
                txtInternalComments.DataBindings.Add("Text", group, "InternalComments", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSecFee.DataBindings.Add("Text", group, "SecFee", true, DataSourceUpdateMode.OnPropertyChanged);
                txtOccFee.DataBindings.Add("Text", group, "OccFee", true, DataSourceUpdateMode.OnPropertyChanged);
                txtOrfFee.DataBindings.Add("Text", group, "OrfFee", true, DataSourceUpdateMode.OnPropertyChanged);
                txtFxRate.DataBindings.Add("Text", group, "FXRate", true, DataSourceUpdateMode.OnPropertyChanged);
                txtSettlCurrFxRate.DataBindings.Add("Text", group, OrderFields.PROPERTY_SettCurrFXRate, true, DataSourceUpdateMode.OnPropertyChanged);
                txtSettlCurrAmt.DataBindings.Add("Text", group, OrderFields.PROPERTY_SETTLEMENTCURRENCYAMOUNT, true, DataSourceUpdateMode.OnPropertyChanged);
                cmbFXConvOperator.DataBindings.Add("Text", group, "FXConversionMethodOperator", true, DataSourceUpdateMode.OnPropertyChanged);
                cmbSettlFXConvOperator.DataBindings.Add("Text", group, OrderFields.PROPERTY_SettCurrFXRateCalc, true, DataSourceUpdateMode.OnPropertyChanged);
                txtAccruedInterest.DataBindings.Add("Text", group, "AccruedInterest", true, DataSourceUpdateMode.OnPropertyChanged);

                txtAttribute1.DataBindings.Add("Text", group, "TradeAttribute1", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAttribute2.DataBindings.Add("Text", group, "TradeAttribute2", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAttribute3.DataBindings.Add("Text", group, "TradeAttribute3", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAttribute4.DataBindings.Add("Text", group, "TradeAttribute4", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAttribute5.DataBindings.Add("Text", group, "TradeAttribute5", true, DataSourceUpdateMode.OnPropertyChanged);
                txtAttribute6.DataBindings.Add("Text", group, "TradeAttribute6", true, DataSourceUpdateMode.OnPropertyChanged);

                if (group.SettlementCurrencyID != group.CurrencyID)
                {
                    //Average Price is to be auto calculated
                    //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                    if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                    {
                        txtAvgPrice.Enabled = false;
                    }
                    else if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementFXRate)
                    {
                        txtSettlCurrFxRate.Enabled = false;
                        cmbSettlFXConvOperator.Enabled = false;
                    }
                    else if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementPrice)
                    {
                        txtSettlCurrAmt.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clears databinding of all the controls on EditTrade
        /// </summary>
        internal void ClearDataBindings()
        {
            try
            {
                statusProvider.Clear();
                lblSymbol.DataBindings.Clear();
                lblAsset.DataBindings.Clear();
                txtExecutedQty.DataBindings.Clear();

                txtAvgPrice.DataBindings.Clear();
                cmbSide.DataBindings.Clear();
                dtpTradeDate.DataBindings.Clear();
                dtpProcessDate.DataBindings.Clear();
                dtpOriginalPurchaseDate.DataBindings.Clear();
                dtpSettleDate.DataBindings.Clear();

                cmbCounterParty.DataBindings.Clear();
                cmbVenue.DataBindings.Clear();

                lblTotalComm.DataBindings.Clear();
                txtCommission.DataBindings.Clear();
                txtSoftCommission.DataBindings.Clear();
                txtOtherBrokerFees.DataBindings.Clear();
                txtClearingBrokerFee.DataBindings.Clear();
                txtMiscFees.DataBindings.Clear();
                txtClearingFee.DataBindings.Clear();
                txtTransactionLevy.DataBindings.Clear();
                txtStampDuty.DataBindings.Clear();
                txtTaxOnCommissions.DataBindings.Clear();
                txtSecFee.DataBindings.Clear();
                txtOccFee.DataBindings.Clear();
                txtOrfFee.DataBindings.Clear();
                txtDescription.DataBindings.Clear();
                txtInternalComments.DataBindings.Clear();
                txtFxRate.DataBindings.Clear();
                txtSettlCurrFxRate.DataBindings.Clear();
                txtSettlCurrAmt.DataBindings.Clear();
                cmbFXConvOperator.DataBindings.Clear();
                cmbSettlFXConvOperator.DataBindings.Clear();
                txtAccruedInterest.DataBindings.Clear();
                txtAttribute1.DataBindings.Clear();
                txtAttribute2.DataBindings.Clear();
                txtAttribute3.DataBindings.Clear();
                txtAttribute4.DataBindings.Clear();
                txtAttribute5.DataBindings.Clear();
                txtAttribute6.DataBindings.Clear();
                _group = null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Changes Enabled status of some controls regarding its AllocationState
        /// </summary>
        /// <param name="readOnlyStatus"></param>
        internal void ChangeEnableStatus(bool readOnlyStatus)
        {
            try
            {
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() != PranaReleaseViewType.CHMiddleWare)
                {
                    txtExecutedQty.ReadOnly = readOnlyStatus;
                }
                cmbSide.ReadOnly = readOnlyStatus;
                dtpTradeDate.ReadOnly = readOnlyStatus;
                dtpOriginalPurchaseDate.ReadOnly = readOnlyStatus;
                dtpProcessDate.ReadOnly = readOnlyStatus;
                dtpSettleDate.ReadOnly = readOnlyStatus;
                cmbVenue.ReadOnly = readOnlyStatus;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Change Enable Status For Exercise
        /// </summary>
        /// <param name="group"></param>
        //Added By amit
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3495
        internal void ChangeEnableStatusForExercise(AllocationGroup group)
        {
            try
            {
                if (group.GroupStatus == PostTradeEnums.Status.Exercise)
                {
                    txtClearingBrokerFee.ReadOnly = true;
                    txtTaxOnCommissions.ReadOnly = true;
                    txtTransactionLevy.ReadOnly = true;
                    txtStampDuty.ReadOnly = true;
                    txtSoftCommission.ReadOnly = true;
                    txtSettlCurrFxRate.ReadOnly = true;
                    txtSettlCurrAmt.ReadOnly = true;
                    txtSecFee.ReadOnly = true;
                    txtOtherBrokerFees.ReadOnly = true;
                    txtOrfFee.ReadOnly = true;
                    txtOccFee.ReadOnly = true;
                    txtMiscFees.ReadOnly = true;
                    txtInternalComments.ReadOnly = true;
                    txtFxRate.ReadOnly = true;
                    txtDescription.ReadOnly = true;
                    txtCommission.ReadOnly = true;
                    txtClearingFee.ReadOnly = true;
                    txtAccruedInterest.ReadOnly = true;
                    txtAttribute1.ReadOnly = true;
                    txtAttribute2.ReadOnly = true;
                    txtAttribute3.ReadOnly = true;
                    txtAttribute4.ReadOnly = true;
                    txtAttribute5.ReadOnly = true;
                    txtAttribute6.ReadOnly = true;
                    txtAvgPrice.ReadOnly = true;
                    txtExecutedQty.ReadOnly = true;
                    cmbSide.ReadOnly = true;
                    cmbCounterParty.ReadOnly = true;
                    cmbFXConvOperator.ReadOnly = true;
                    cmbSettlFXConvOperator.ReadOnly = true;
                    cmbVenue.ReadOnly = true;
                    dtpTradeDate.ReadOnly = true;
                    dtpOriginalPurchaseDate.ReadOnly = true;
                    dtpProcessDate.ReadOnly = true;
                    dtpSettleDate.ReadOnly = true;
                }
                else
                {
                    txtClearingBrokerFee.ReadOnly = false;
                    txtTaxOnCommissions.ReadOnly = false;
                    txtTransactionLevy.ReadOnly = false;
                    txtStampDuty.ReadOnly = false;
                    txtSoftCommission.ReadOnly = false;
                    txtSettlCurrFxRate.ReadOnly = false;
                    txtSettlCurrAmt.ReadOnly = false;
                    txtSecFee.ReadOnly = false;
                    txtOtherBrokerFees.ReadOnly = false;
                    txtOrfFee.ReadOnly = false;
                    txtOccFee.ReadOnly = false;
                    txtMiscFees.ReadOnly = false;
                    txtInternalComments.ReadOnly = false;
                    txtFxRate.ReadOnly = false;
                    txtDescription.ReadOnly = false;
                    txtCommission.ReadOnly = false;
                    txtClearingFee.ReadOnly = false;
                    txtAccruedInterest.ReadOnly = false;
                    txtAttribute1.ReadOnly = false;
                    txtAttribute2.ReadOnly = false;
                    txtAttribute3.ReadOnly = false;
                    txtAttribute4.ReadOnly = false;
                    txtAttribute5.ReadOnly = false;
                    txtAttribute6.ReadOnly = false;
                    txtAvgPrice.ReadOnly = false;
                    cmbCounterParty.ReadOnly = false;
                    cmbFXConvOperator.ReadOnly = false;
                    cmbSettlFXConvOperator.ReadOnly = false;
                    cmbVenue.ReadOnly = false;
                    if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                        txtExecutedQty.ReadOnly = false;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Change Enable Status For Closed And Partial Closed Trades
        /// </summary>
        /// <param name="group"></param>
        // Added By : Manvendra Prajapati
        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-10162
        internal void ChangeEnableStatusForClosedAndPartialClosed(AllocationGroup gParent)
        {
            try
            {
                if (gParent.ClosingStatus == ClosingStatus.Closed || gParent.ClosingStatus == ClosingStatus.PartiallyClosed)
                {
                    cmbSettlFXConvOperator.ReadOnly = true;
                    txtSettlCurrFxRate.ReadOnly = true;
                    txtSettlCurrAmt.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Applies the data to the grid and closes the SingleEditTradeUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                CloseEditSingleGroup.Invoke(this, null);
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

        /// <summary>
        ///  Allows only numeric values in a textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '.')
                    {
                        e.Handled = true;
                    }

                    // Allows only one decimal point
                    if (e.KeyChar == '.' && (((Infragistics.Win.UltraWinEditors.TextEditorControlBase)(sender)).Text).IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    _group.UpdateGroupPersistenceStatus();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DateTime _settleDate;

        private void dtpSettleDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && dtpSettleDate.DateTime.Date < dtpProcessDate.DateTime.Date && _group.AllocatedQty == 0)
                {
                    MessageBox.Show("Settlement Date can not be less than Process Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                    dtpSettleDate.DateTime = _settleDate;
                }
                else
                {
                    if (_group != null && dtpSettleDate.IsInEditMode == true)
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                    }
                }
                _settleDate = dtpSettleDate.DateTime.Date;
                if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                {

                    //_group.AccruedInterest = CalculateAccruedInterest(_group);                   
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtpSettleDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    //PRANA-4907:Date needs to be validated in case of changing it manually and not by date chooser. 
                    dtpSettleDate_AfterCloseUp(sender, e);
                    if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                    {
                        SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                        secMasterReqobj.AddData(_group.Symbol, ApplicationConstants.PranaSymbology);
                        secMasterReqobj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(secMasterReqobj);
                        //_group.AccruedInterest = CalculateAccruedInterest(_group);
                    }
                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DateTime _originalPurchaseDate;
        private void dtpOriginalPurchaseDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && dtpOriginalPurchaseDate.DateTime.Date > dtpProcessDate.DateTime.Date && _group.AllocatedQty == 0)
                {
                    MessageBox.Show("OriginalPurchase Date cannot be greater than Process Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                    dtpOriginalPurchaseDate.DateTime = _originalPurchaseDate;
                }

                if (_group != null && dtpOriginalPurchaseDate.IsInEditMode == true)
                {
                    _group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                }
                _originalPurchaseDate = dtpOriginalPurchaseDate.DateTime.Date;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtpProcessDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && dtpProcessDate.DateTime.Date < _group.AUECLocalDate.Date && _group.AllocatedQty == 0)
                {
                    MessageBox.Show("Process Date cannot be less than Trade Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                    dtpProcessDate.DateTime = _group.ProcessDate.Date;
                }
                if (_group != null && dtpProcessDate.DateTime.Date > _group.SettlementDate.Date && _group.AllocatedQty == 0)
                {
                    MessageBox.Show("Process Date cannot be greater than Settlement Date, it will be reverted to Last Valid Value", "Warning!", MessageBoxButtons.OK);
                    dtpProcessDate.DateTime = _group.ProcessDate.Date;
                }
                if (_group != null && dtpProcessDate.IsInEditMode == true)
                {
                    _group.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                    _group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtExecutedQty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtExecutedQty.Text))
                {
                    if (double.Parse(txtExecutedQty.Text) > _group.Quantity)
                    {
                        //modified by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3384
                        MessageBox.Show("Executed Quantity should be less than or equal to the Quantity!", "Warning");
                        if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                        {
                            txtExecutedQty.Text = _group.Quantity.ToString();
                        }
                    }

					//set percision digits to 8, PRANA-12296
                    txtExecutedQty.Text = string.Format("{0:#,##0.########}", double.Parse(txtExecutedQty.Text));

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;

                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);

                    UpdateUnAllocatedGroupTaxlotState();

                    if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                    {
                        SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                        secMasterReqobj.AddData(_group.Symbol, ApplicationConstants.PranaSymbology);
                        secMasterReqobj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(secMasterReqobj);
                        //_group.AccruedInterest = CalculateAccruedInterest(_group);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtCommission_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtCommission.Text))
                {
                    txtCommission.Text = string.Format("{0:#,##0.########}", double.Parse(txtCommission.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;

                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOtherBrokerFees_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtOtherBrokerFees.Text))
                {
                    txtOtherBrokerFees.Text = string.Format("{0:#,##0.########}", double.Parse(txtOtherBrokerFees.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtClearingBrokerFee_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtClearingBrokerFee.Text))
                {
                    txtClearingBrokerFee.Text = string.Format("{0:#,##0.########}", double.Parse(txtClearingBrokerFee.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtStampDuty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtStampDuty.Text))
                {
                    txtStampDuty.Text = string.Format("{0:#,##0.########}", double.Parse(txtStampDuty.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtTransactionLevy_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtTransactionLevy.Text))
                {
                    txtTransactionLevy.Text = string.Format("{0:#,##0.########}", double.Parse(txtTransactionLevy.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtClearingFee_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtClearingFee.Text))
                {
                    txtClearingFee.Text = string.Format("{0:#,##0.########}", double.Parse(txtClearingFee.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtTaxOnCommissions_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtTaxOnCommissions.Text))
                {
                    txtTaxOnCommissions.Text = string.Format("{0:#,##0.########}", double.Parse(txtTaxOnCommissions.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtMiscFees_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtMiscFees.Text))
                {
                    txtMiscFees.Text = string.Format("{0:#,##0.########}", double.Parse(txtMiscFees.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtSecFee_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtSecFee.Text))
                {
                    txtSecFee.Text = string.Format("{0:#,##0.########}", double.Parse(txtSecFee.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOccFee_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtOccFee.Text))
                {
                    txtOccFee.Text = string.Format("{0:#,##0.########}", double.Parse(txtOccFee.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOrfFee_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtOrfFee.Text))
                {
                    txtOrfFee.Text = string.Format("{0:#,##0.########}", double.Parse(txtOrfFee.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAvgPrice_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtAvgPrice.Text) && double.Parse(txtAvgPrice.Text) > 0)
                {

					//set percision digits to 8, PRANA-12296
                    txtAvgPrice.Text = string.Format("{0:#,##0.########}", double.Parse(txtAvgPrice.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.AvgPrice = double.Parse(txtAvgPrice.Text);
                    _group.UpdateTaxlotAvgPrice();

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;
                    UpdateUnAllocatedGroupTaxlotState();
                    //UpdateSettlCurrAmt();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtCommission_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedCommission = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtCommission.Text))
                {
                    updatedCommission = double.Parse(txtCommission.Text);

                    _group.Commission = updatedCommission;

                    commFields = new CommissionFields();
                    commFields.Commission = updatedCommission;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtCommission.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.Commission_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOtherBrokerFees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedOtherBrokerFees = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtOtherBrokerFees.Text))
                {
                    updatedOtherBrokerFees = double.Parse(txtOtherBrokerFees.Text);
                    _group.OtherBrokerFees = updatedOtherBrokerFees;

                    commFields = new CommissionFields();
                    commFields.OtherBrokerFees = updatedOtherBrokerFees;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtOtherBrokerFees.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.OtherBrokerFees_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtClearingBrokerFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedClearingBrokerFee = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtClearingBrokerFee.Text))
                {
                    updatedClearingBrokerFee = double.Parse(txtClearingBrokerFee.Text);
                    _group.ClearingBrokerFee = updatedClearingBrokerFee;

                    commFields = new CommissionFields();
                    commFields.ClearingBrokerFee = updatedClearingBrokerFee;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtClearingBrokerFee.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.ClearingBrokerFee_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtStampDuty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedStampDuty = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtStampDuty.Text))
                {
                    updatedStampDuty = double.Parse(txtStampDuty.Text);
                    _group.StampDuty = updatedStampDuty;
                    commFields = new CommissionFields();
                    commFields.StampDuty = updatedStampDuty;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtStampDuty.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.StampDuty_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtTransactionLevy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedTransactionLevy = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtTransactionLevy.Text))
                {
                    updatedTransactionLevy = double.Parse(txtTransactionLevy.Text);

                    _group.TransactionLevy = updatedTransactionLevy;
                    commFields = new CommissionFields();
                    commFields.TransactionLevy = updatedTransactionLevy;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtTransactionLevy.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TransactionLevy_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtClearingFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedClearingFee = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtClearingFee.Text))
                {
                    updatedClearingFee = double.Parse(txtClearingFee.Text);

                    _group.ClearingFee = updatedClearingFee;
                    commFields = new CommissionFields();
                    commFields.ClearingFee = updatedClearingFee;
                    if (txtClearingFee.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.ClearingFee_Changed);
                    }
                    _group.UpdateTaxlotCommissionAndFees(commFields);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtTaxOnCommissions_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedTaxOnCommissions = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtTaxOnCommissions.Text))
                {
                    updatedTaxOnCommissions = double.Parse(txtTaxOnCommissions.Text);

                    _group.TaxOnCommissions = updatedTaxOnCommissions;
                    commFields = new CommissionFields();
                    commFields.TaxOnCommissions = updatedTaxOnCommissions;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtTaxOnCommissions.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TaxOnCommission_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtMiscFees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedMiscFees = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtMiscFees.Text))
                {
                    updatedMiscFees = double.Parse(txtMiscFees.Text);

                    _group.MiscFees = updatedMiscFees;
                    commFields = new CommissionFields();
                    commFields.MiscFees = updatedMiscFees;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtMiscFees.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.MiscFees_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtSecFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedSecFee = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtSecFee.Text))
                {
                    updatedSecFee = double.Parse(txtSecFee.Text);

                    _group.SecFee = updatedSecFee;
                    commFields = new CommissionFields();
                    commFields.SecFee = updatedSecFee;
                    if (txtSecFee.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.SecFee_Changed);
                    }
                    _group.UpdateTaxlotCommissionAndFees(commFields);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOccFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedOccFee = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtOccFee.Text))
                {
                    updatedOccFee = double.Parse(txtOccFee.Text);

                    _group.OccFee = updatedOccFee;
                    commFields = new CommissionFields();
                    commFields.OccFee = updatedOccFee;
                    if (txtOccFee.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.OccFee_Changed);
                    }
                    _group.UpdateTaxlotCommissionAndFees(commFields);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtOrfFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedOrfFee = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtOrfFee.Text))
                {
                    updatedOrfFee = double.Parse(txtOrfFee.Text);

                    _group.OrfFee = updatedOrfFee;
                    commFields = new CommissionFields();
                    commFields.OrfFee = updatedOrfFee;
                    if (txtOrfFee.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.OrfFee_Changed);
                    }
                    _group.UpdateTaxlotCommissionAndFees(commFields);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CancelEditing();
                //if (this.UnpinEditSingleTradeControl != null)
                //    this.UnpinEditSingleTradeControl(sender, e);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Cancels changes to the EditTrade Control
        /// </summary>
        internal void CancelEditing()
        {
            try
            {
                if (_group != null)
                {
                    if (AllocationManager.GetInstance().AnythingChanged())
                    {
                        AllocationManager.GetInstance().Remove(_group.GroupID);
                        AllocationManager.GetInstance().RemoveAllocated(_group);
                        AllocationManager.GetInstance().RemoveUnAllocated(_group);
                        AllocationManager.GetInstance().ClearDictionaryUnsaved(_group.GroupID);
                        ClearDataBindings();
                        _group = (AllocationGroup)_groupClone.Clone();
                        AllocationManager.GetInstance().AddGroup(_group);
                        BindData(_group);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets default values to the EditTrade Control
        /// </summary>
        const string DefaultNumericValueToText = "0";
        internal void SetDefaultValuetoControl()
        {
            try
            {
                lblSymbol.Text = string.Empty;
                lblAsset.Text = string.Empty;
                txtExecutedQty.Text = DefaultNumericValueToText;
                txtAvgPrice.Text = DefaultNumericValueToText;
                cmbSide.Text = ApplicationConstants.C_COMBO_SELECT;
                dtpTradeDate.DateTime = DateTime.Now.Date;
                dtpProcessDate.DateTime = DateTime.Now.Date;
                dtpOriginalPurchaseDate.DateTime = DateTime.Now.Date;

                dtpSettleDate.DateTime = DateTime.Now.Date;

                cmbCounterParty.Text = ApplicationConstants.C_COMBO_SELECT;
                cmbVenue.Text = ApplicationConstants.C_COMBO_SELECT;
                lblTotalComm.Text = DefaultNumericValueToText;
                txtCommission.Text = DefaultNumericValueToText;
                txtSoftCommission.Text = DefaultNumericValueToText;
                txtOtherBrokerFees.Text = DefaultNumericValueToText;
                txtClearingBrokerFee.Text = DefaultNumericValueToText;
                txtMiscFees.Text = DefaultNumericValueToText;
                txtClearingFee.Text = DefaultNumericValueToText;
                txtTransactionLevy.Text = DefaultNumericValueToText;
                txtStampDuty.Text = DefaultNumericValueToText;
                txtTaxOnCommissions.Text = DefaultNumericValueToText;
                txtSecFee.Text = DefaultNumericValueToText;
                txtOccFee.Text = DefaultNumericValueToText;
                txtOrfFee.Text = DefaultNumericValueToText;
                txtDescription.Text = string.Empty;
                txtInternalComments.Text = string.Empty;
                txtFxRate.Text = DefaultNumericValueToText;
                txtSettlCurrFxRate.Text = DefaultNumericValueToText;
                txtSettlCurrAmt.Text = DefaultNumericValueToText;
                cmbFXConvOperator.Text = string.Empty;
                cmbSettlFXConvOperator.Text = string.Empty;
                txtAccruedInterest.Text = DefaultNumericValueToText;

                txtAttribute1.Text = string.Empty;
                txtAttribute2.Text = string.Empty;
                txtAttribute3.Text = string.Empty;
                txtAttribute4.Text = string.Empty;
                txtAttribute5.Text = string.Empty;
                txtAttribute6.Text = string.Empty;

                _originalPurchaseDate = DateTime.Now.Date;
                _settleDate = DateTime.Now.Date;
                _group = null;
                _groupClone = null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method reverts group'value on cancel button
        /// </summary>
        /// <param name="group"></param>
        private void RetrieveGroupValues(AllocationGroup group)
        {
            try
            {
                txtExecutedQty.Text = group.CumQty.ToString();
                txtAvgPrice.Text = group.AvgPrice.ToString();
                cmbSide.Text = group.OrderSide.ToString();
                dtpTradeDate.DateTime = group.AUECLocalDate;
                dtpProcessDate.DateTime = group.ProcessDate;
                dtpOriginalPurchaseDate.DateTime = group.OriginalPurchaseDate;
                dtpSettleDate.DateTime = group.SettlementDate;

                cmbCounterParty.Text = group.CounterPartyName;
                cmbVenue.Text = group.Venue;
                lblTotalComm.Text = group.TotalCommissionandFees.ToString();
                txtCommission.Text = group.Commission.ToString();
                txtSoftCommission.Text = group.SoftCommission.ToString();
                txtOtherBrokerFees.Text = group.OtherBrokerFees.ToString();
                txtClearingBrokerFee.Text = group.ClearingBrokerFee.ToString();
                txtMiscFees.Text = group.MiscFees.ToString();
                txtClearingFee.Text = group.ClearingFee.ToString();
                txtTransactionLevy.Text = group.TransactionLevy.ToString();
                txtStampDuty.Text = group.StampDuty.ToString();
                txtTaxOnCommissions.Text = group.TaxOnCommissions.ToString();
                txtSecFee.Text = group.SecFee.ToString();
                txtOccFee.Text = group.OccFee.ToString();
                txtOrfFee.Text = group.OrfFee.ToString();
                txtDescription.Text = group.Description;
                txtInternalComments.Text = group.InternalComments;
                txtFxRate.Text = group.FXRate.ToString();
                txtSettlCurrFxRate.Text = group.SettlCurrFxRate.ToString();
                txtSettlCurrAmt.Text = group.SettlCurrAmt.ToString();
                cmbFXConvOperator.Text = group.FXConversionMethodOperator;
                cmbSettlFXConvOperator.Text = group.FXConversionMethodOperator;
                txtAccruedInterest.Text = group.AccruedInterest.ToString();

                txtAttribute1.Text = group.TradeAttribute1;
                txtAttribute2.Text = group.TradeAttribute2;
                txtAttribute3.Text = group.TradeAttribute3;
                txtAttribute4.Text = group.TradeAttribute4;
                txtAttribute5.Text = group.TradeAttribute5;
                txtAttribute6.Text = group.TradeAttribute6;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Updates Settlement data of a group based on its Trade Date
        /// </summary>
        /// <param name="group"></param>
        private void GetSettlementDate(AllocationGroup group)
        {
            try
            {
                int auecID = Convert.ToInt32(group.AUECID);
                string sideText = group.OrderSide.ToString();
                if (sideText != "0")
                {
                    string sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);
                    int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                    DateTime tradeDate = Convert.ToDateTime(group.AUECLocalDate.ToString());
                    if (auecSettlementPeriod == 0)
                    {
                        group.SettlementDate = tradeDate;
                    }
                    else
                    {
                        group.SettlementDate = Prana.Utilities.DateTimeUtilities.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbSide_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbSide != null && cmbSide.Rows.Count > 0 && cmbSide.Text != ApplicationConstants.C_COMBO_SELECT)
                {
                    if (cmbSide.Value != null)
                    {
                        _group.OrderSideTagValue = cmbSide.Value.ToString();
                        //Added to change Transaction Type, PRANA-12418
                        _group.AddTradeAction(TradeAuditActionType.ActionType.TransactionType_Changed);
                        _group.TransactionType = Regex.Replace(cmbSide.Text, @"\s+", "");
                    }
                    // update taxlots value and state to publish
                    _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtpOriginalPurchaseDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    
                    //PRANA-4907:Date needs to be validated in case of changing it manually and not by date chooser.
                    dtpOriginalPurchaseDate_AfterCloseUp(sender, e);
                    if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                    {
                        SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                        secMasterReqobj.AddData(_group.Symbol, ApplicationConstants.PranaSymbology);
                        secMasterReqobj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(secMasterReqobj);
                        //_group.AccruedInterest = CalculateAccruedInterest(_group);
                    }
                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtpProcessDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    //PRANA-4907:Date needs to be validated in case of changing it manually and not by date chooser.
                    dtpProcessDate_AfterCloseUp(sender, e);
                    _group.OriginalPurchaseDate = dtpProcessDate.DateTime;
                    if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                    {
                        SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                        secMasterReqobj.AddData(_group.Symbol, ApplicationConstants.PranaSymbology);
                        secMasterReqobj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(secMasterReqobj);
                        //_group.AccruedInterest = CalculateAccruedInterest(_group);
                    }
                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);

                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void dtpTradeDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.AUECLocalDate = dtpTradeDate.DateTime;
                    _group.ProcessDate = dtpTradeDate.DateTime;
                    _group.OriginalPurchaseDate = dtpTradeDate.DateTime;
                    GetSettlementDate(_group);

                    if (_group != null && _group.PersistenceStatus != ApplicationConstants.PersistenceStatus.NotChanged && (_group.AssetName == AssetCategory.FixedIncome.ToString() || _group.AssetName == AssetCategory.ConvertibleBond.ToString()))
                    {
                        SecMasterRequestObj secMasterReqobj = new SecMasterRequestObj();
                        secMasterReqobj.AddData(_group.Symbol, ApplicationConstants.PranaSymbology);
                        secMasterReqobj.HashCode = this.GetHashCode();
                        _securityMaster.SendRequest(secMasterReqobj);
                        //_group.AccruedInterest = CalculateAccruedInterest(_group);
                    }
                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbCounterParty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbCounterParty.Value != null)
                {
                    _group.CounterPartyID = (int)cmbCounterParty.Value;

                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbVenue_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbVenue.Value != null)
                {
                    _group.VenueID = (int)cmbVenue.Value;
                    // update taxlots value and state to publish
                    //_group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.IsAnotherTaxlotAttributesUpdated = true;

                    _group.Description = txtDescription.Text;

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        // update taxlots value and state to publish
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtFxRate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.IsAnotherTaxlotAttributesUpdated = true;

                    _group.FXRate = double.Parse(txtFxRate.Text);
                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void txtSettlFxRate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.SettlCurrFxRate = double.Parse(txtSettlCurrFxRate.Text);
                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() == _group.SettlementCurrencyID)
                    {
                        txtFxRate.Text = txtSettlCurrFxRate.Text;
                        _group.UpdateFXRateAndFXOperator();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void txtSettlCurrAmt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.SettlCurrAmt = double.Parse(txtSettlCurrAmt.Text);
                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    if (double.Parse(txtAvgPrice.Value.ToString()) == 0)
                    {
                        txtSettlCurrAmt.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbFXConvOperator_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbFXConvOperator != null)
                {

                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED && cmbFXConvOperator.Value != null)
                    {
                        _group.UpdateGroupTaxlots("FXConversionMethodOperator", cmbFXConvOperator.Value.ToString());
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void cmbSettlFXConvOperator_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbSettlFXConvOperator != null)
                {

                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(OrderFields.PROPERTY_SettCurrFXRateCalc, cmbSettlFXConvOperator.Value.ToString());
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    if (CachedDataManager.GetInstance.GetCompanyBaseCurrencyID() == _group.SettlementCurrencyID)
                    {
                        cmbFXConvOperator.Value = cmbSettlFXConvOperator.Value;
                        _group.UpdateFXRateAndFXOperator();
                    }
                    // UpdateSettlCurrAmt();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }




        private void txtAccruedInterest_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.IsAnotherTaxlotAttributesUpdated = true;

                    // update taxlots value and state to publish
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //PKE: Commented since not in use, to be removed
        //private void txtProxySymbol_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_group != null)
        //        {
        //            _group.IsAnotherTaxlotAttributesUpdated = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        private void txtAttribute1_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute1 = txtAttribute1.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute1 = txtAttribute1.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }

                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute1 = txtAttribute1.Text;
                        }
                    }

                    updateAttribList(txtAttribute1);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAttribute2_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute2 = txtAttribute2.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute2 = txtAttribute2.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute2 = txtAttribute2.Text;
                        }
                    }
                    updateAttribList(txtAttribute2);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAttribute3_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute3 = txtAttribute3.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute3 = txtAttribute3.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute3 = txtAttribute3.Text;
                        }
                    }
                    updateAttribList(txtAttribute3);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAttribute4_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute4 = txtAttribute4.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute4 = txtAttribute4.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute4 = txtAttribute4.Text;
                        }
                    }
                    updateAttribList(txtAttribute4);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAttribute5_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute5 = txtAttribute5.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute5 = txtAttribute5.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute5 = txtAttribute5.Text;
                        }
                    }
                    updateAttribList(txtAttribute5);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAttribute6_Leave(object sender, EventArgs e)
        {
            try
            {
                TradeAttributes tradeAttr = null;
                if (_group != null)
                {
                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        tradeAttr = UpdateDataforTradeAttributes(_group, string.Empty);
                        tradeAttr.TradeAttribute6 = txtAttribute6.Text;
                        _group.UpdateTaxlotTradeAttributes(tradeAttr);
                    }
                    else
                    {
                        _group.TradeAttribute6 = txtAttribute6.Text;
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                    //Updating trade attributes to order as well so that it can be maintained while grouping and ungrouping
                    if (_group.State != PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Updated;
                    else
                        _group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;

                    if (_group.Orders.Count == 1)
                    {
                        foreach (AllocationOrder ord in _group.Orders)
                        {
                            ord.TradeAttribute6 = txtAttribute6.Text;
                        }
                    }
                    updateAttribList(txtAttribute6);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates TradeAttributes of a group to its taxlots
        /// </summary>
        /// <param name="group"></param>
        /// <param name="taxLotID"></param>
        /// <returns></returns>
        private TradeAttributes UpdateDataforTradeAttributes(AllocationGroup group, string taxLotID)
        {
            TradeAttributes tradeAttr = new TradeAttributes();
            try
            {
                if (taxLotID == string.Empty)
                {
                    tradeAttr.TradeAttribute1 = group.TradeAttribute1;
                    tradeAttr.TradeAttribute2 = group.TradeAttribute2;
                    tradeAttr.TradeAttribute3 = group.TradeAttribute3;
                    tradeAttr.TradeAttribute4 = group.TradeAttribute4;
                    tradeAttr.TradeAttribute5 = group.TradeAttribute5;
                    tradeAttr.TradeAttribute6 = group.TradeAttribute6;
                }
                else
                {
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (taxlot.TaxLotID == taxLotID)
                        {
                            tradeAttr.TradeAttribute1 = taxlot.TradeAttribute1;
                            tradeAttr.TradeAttribute2 = taxlot.TradeAttribute2;
                            tradeAttr.TradeAttribute3 = taxlot.TradeAttribute3;
                            tradeAttr.TradeAttribute4 = taxlot.TradeAttribute4;
                            tradeAttr.TradeAttribute5 = taxlot.TradeAttribute5;
                            tradeAttr.TradeAttribute6 = taxlot.TradeAttribute6;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return tradeAttr;
        }

        /// <summary>
        /// Updates data in dictionary for publishing
        /// </summary>
        private void UpdateUnAllocatedGroupTaxlotState()
        {
            try
            {
                if (_group != null)
                {
                    if (_group.PersistenceStatus != ApplicationConstants.PersistenceStatus.New)
                    {
                        if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            TaxLot updatedTaxlot = _allocationServices.InnerChannel.CreateUnAllocatedTaxLot((PranaBasicMessage)_group, _group.GroupID);
                            _group.UpdateTaxlotState(updatedTaxlot);
                            _group.IsModified = true;
                            _group.CompanyUserID = _userID;
                            if (_group.Orders.Count == 1)
                            {
                                _group.Orders[0].IsModified = true;
                                _group.Orders[0].AvgPrice = _group.AvgPrice;
                                _group.Orders[0].CumQty = _group.CumQty;
                                _group.Orders[0].Description = _group.Description;
                                _group.Orders[0].InternalComments = _group.InternalComments;
                                _group.Orders[0].AUECLocalDate = _group.AUECLocalDate;
                                _group.Orders[0].OriginalPurchaseDate = _group.OriginalPurchaseDate;
                                _group.Orders[0].ProcessDate = _group.ProcessDate;
                                _group.Orders[0].SettlementDate = _group.SettlementDate;
                                _group.Orders[0].Venue = _group.Venue;
                                _group.Orders[0].VenueID = _group.VenueID;
                                _group.Orders[0].CounterPartyID = _group.CounterPartyID;
                                _group.Orders[0].CounterPartyName = _group.CounterPartyName;
                                _group.Orders[0].OrderSideTagValue = _group.OrderSideTagValue;
                                _group.Orders[0].OrderSide = _group.OrderSide;
                                _group.Orders[0].FXRate = _group.FXRate;
                                _group.Orders[0].FXConversionMethodOperator = _group.FXConversionMethodOperator;
                                _group.Orders[0].SettlCurrFxRate = _group.SettlCurrFxRate;
                                _group.Orders[0].SettlCurrFxRateCalc = _group.SettlCurrFxRateCalc;
                                _group.Orders[0].SettlCurrAmt = _group.SettlCurrAmt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void dtpTradeDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    if (dtpTradeDate.IsInEditMode == true)
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.TradeDate_Changed);
                        _group.AddTradeAction(TradeAuditActionType.ActionType.ProcessDate_Changed);
                        _group.AddTradeAction(TradeAuditActionType.ActionType.OriginalPurchaseDate_Changed);
                        _group.AddTradeAction(TradeAuditActionType.ActionType.SettlementDate_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                _group.UpdateGroupPersistenceStatus();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }

        private void cmb_KeyDown(object sender, KeyEventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }

        private void cmb_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }

        private void dtp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }

        private void dtp_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }

        private void dtp_KeyDown(object sender, KeyEventArgs e)
        {
            if (_group != null)
            {
                _group.UpdateGroupPersistenceStatus();
            }
        }
        internal void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        Prana.AllocationNew.CtrlAmendmend.SecMasterObjHandler secObjhandler = new Prana.AllocationNew.CtrlAmendmend.SecMasterObjHandler(UpdateValue);
                        this.Invoke(secObjhandler, new object[] { sender, e });
                    }
                    else
                    {
                        UpdateValue(sender, e);
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

        private void UpdateValue(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (e.Value == null)
                {
                    //Logger.Write(@"SecMasterObj is null in UpdateValue function in \Prana_CA\Prana.PM\Prana.PM.Client.UI\Controls\CtrlCreateAndImportPosition.cs class.");
                    return;
                }
                else
                {
                    if (_group != null)
                    {
                        SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)e.Value;
                        AllocationGroup group = null;

                        foreach (AllocationGroup allgroup in AllocationManager.GetInstance().UnAllocatedGroups)
                        {
                            if (allgroup.GroupID.Equals(_group.GroupID))
                            {
                                group = allgroup;
                                break;
                            }
                        }

                        if (group != null)
                        {
                            group.CouponRate = fixedIncomeObj.Coupon;
                            group.MaturityDate = fixedIncomeObj.MaturityDate;
                            group.IssueDate = fixedIncomeObj.IssueDate;
                            group.FirstCouponDate = fixedIncomeObj.FirstCouponDate;
                            group.AccrualBasis = fixedIncomeObj.AccrualBasis;
                            group.BondType = fixedIncomeObj.BondType;
                            group.Freq = fixedIncomeObj.Frequency;
                            group.IsZero = fixedIncomeObj.IsZero;
                            group.ExpirationDate = fixedIncomeObj.MaturityDate;
                            double accruedTemp = group.AccruedInterest;
                            group.AccruedInterest = CalculateAccruedInterest(group);
                            if (accruedTemp != group.AccruedInterest)
                                group.AddTradeAction(TradeAuditActionType.ActionType.AccruedInterest_Changed);
                            group.UpdateTaxlotAccruedInterest();
                        }
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

        private void txtExecutedQty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtExecutedQty.Text))
                {
                    if (txtExecutedQty.IsInEditMode && _groupClone.CumQty != double.Parse(txtExecutedQty.Text))
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.ExecutedQuantity_Changed);
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

        private void txtAvgPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtAvgPrice.Text) && double.Parse(txtAvgPrice.Text) > 0)
                {
                    if (txtAvgPrice.IsInEditMode && _groupClone.AvgPrice != double.Parse(txtAvgPrice.Text))
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.AvgPrice_Changed);
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

        private void cmbSide_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbSide != null && cmbSide.Rows.Count > 0 && cmbSide.Text != ApplicationConstants.C_COMBO_SELECT)
                {
                    if (cmbSide.IsDroppedDown && _groupClone.OrderSideTagValue != cmbSide.Value.ToString())
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.OrderSide_Changed);
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

        private void cmbCounterParty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbCounterParty.Value != null)
                {
                    if (cmbCounterParty.IsDroppedDown && _groupClone.CounterPartyID != (int)cmbCounterParty.Value)
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.Counterparty_Changed);
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

        private void cmbVenue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbVenue.Value != null)
                {
                    if (cmbVenue.IsDroppedDown && _groupClone.VenueID != (int)cmbVenue.Value)
                    {
                        _group.AddTradeAction(TradeAuditActionType.ActionType.Venue_Changed);
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

        private void txtDescription_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtDescription.IsInEditMode && _groupClone.Description != txtDescription.Text)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.Description_Changed);
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

        private void txtFxRate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFxRate.Text != null && txtFxRate.Text == "")
                {
                    txtFxRate.Text = DefaultNumericValueToText;
                }
                if (_group != null && txtFxRate.IsInEditMode && _groupClone.FXRate != double.Parse(txtFxRate.Text))
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.FxRate_Changed);
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
        private void txtSettlFxRate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSettlCurrFxRate.Text != null && txtSettlCurrFxRate.Text == "")
                {
                    txtSettlCurrFxRate.Text = DefaultNumericValueToText;
                }
                if (_group != null && txtSettlCurrFxRate.IsInEditMode && _groupClone.SettlCurrFxRate != double.Parse(txtSettlCurrFxRate.Text))
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.SettlCurrFxRate_Changed);
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


        /// <summary>
        ///  Update SettlCurrAmt  on change of dependent parameters
        /// </summary>
        //internal void UpdateSettlCurrAmt()
        //{
        //    try
        //    {
        //        double value = 0;

        //        double settlCurrAmt = Convert.ToDouble(txtSettlCurrAmt.Value);
        //        double avgPrice = Convert.ToDouble(txtAvgPrice.Value);
        //        double settlCurrFxRate = Convert.ToDouble(txtSettlCurrFxRate.Value);
        //        if (settlCurrFxRate == 0 || avgPrice == 0 || cmbSettlFXConvOperator.Value == null)
        //        {
        //            value = 0;
        //        }
        //        else if (cmbSettlFXConvOperator.Value.ToString().Equals(Operator.M.ToString()))
        //        {
        //            value = avgPrice * settlCurrFxRate;
        //        }

        //        else if (cmbSettlFXConvOperator.Value.ToString().Equals(Operator.D.ToString()))
        //        {
        //            value = avgPrice / settlCurrFxRate;
        //        }
        //        value = Math.Round(value, 4);
        //        if (settlCurrAmt != value)
        //        {
        //            this.txtSettlCurrAmt.ValueChanged -= txtSettlCurrAmt_ValueChanged;
        //            txtSettlCurrAmt.Value = value;
        //            this.txtSettlCurrAmt.ValueChanged += txtSettlCurrAmt_ValueChanged;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        private void txtSettlCurrAmt_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSettlCurrAmt.Text != null && txtSettlCurrAmt.Text == "")
                {
                    txtSettlCurrAmt.Text = DefaultNumericValueToText;
                }
                if (_group != null && txtSettlCurrAmt.IsInEditMode && _groupClone.SettlCurrAmt != double.Parse(txtSettlCurrAmt.Text))
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.SettlCurrAmt_Changed);
                }
                if (double.Parse(txtAvgPrice.Value.ToString()) != 0)
                {
                    //UpdateSettlCurrFxRate();
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


        /// <summary>
        ///  Update SettlCurrFxRate  on change of dependent parameters
        /// </summary>
        //internal void UpdateSettlCurrFxRate()
        //{
        //    try
        //    {
        //        double value = 0;
        //        double settlCurrAmt = Convert.ToDouble(txtSettlCurrAmt.Value);
        //        double avgPrice = Convert.ToDouble(txtAvgPrice.Value);
        //        double settlCurrFxRate = Convert.ToDouble(txtSettlCurrFxRate.Value);
        //        if (settlCurrAmt == 0 || avgPrice == 0 || cmbSettlFXConvOperator.Value == null)
        //        {
        //            value = 0;
        //        }
        //        else if (cmbSettlFXConvOperator.Value.ToString().Equals(Operator.M.ToString()))
        //        {
        //            value = settlCurrAmt / avgPrice;
        //        }
        //        else if (cmbSettlFXConvOperator.Value.ToString().Equals(Operator.D.ToString()))
        //        {
        //            value = avgPrice / settlCurrAmt;
        //        }
        //        value = Math.Round(value, 4);
        //        if (settlCurrFxRate != value)
        //        {
        //            this.txtSettlCurrFxRate.ValueChanged -= txtSettlFxRate_ValueChanged;
        //            txtSettlCurrFxRate.Value = value;
        //            this.txtSettlCurrFxRate.ValueChanged += txtSettlFxRate_ValueChanged;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}



        private void cmbFXConvOperator_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbFXConvOperator != null)
                {
                    if (cmbFXConvOperator.IsDroppedDown && _groupClone.FXConversionMethodOperator != cmbFXConvOperator.Value.ToString())
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.FxConversionMethodOperator_Changed);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSettlFXConvOperator_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && cmbSettlFXConvOperator != null)
                {
                    if (cmbSettlFXConvOperator.IsDroppedDown && _groupClone.SettlCurrFxRateCalc != cmbSettlFXConvOperator.Value.ToString())
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.SettlCurrFxRateCalc_Changed);
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
        private void txtAccruedInterest_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAccruedInterest.IsInEditMode && _groupClone.AccruedInterest != _group.AccruedInterest)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.AccruedInterest_Changed);
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

        private void txtAttribute1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute1.IsInEditMode && _groupClone.TradeAttribute1 != _group.TradeAttribute1)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute1_Changed);
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

        private void txtAttribute2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute2.IsInEditMode && _groupClone.TradeAttribute2 != _group.TradeAttribute2)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute2_Changed);
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

        private void txtAttribute3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute3.IsInEditMode && _groupClone.TradeAttribute3 != _group.TradeAttribute3)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute3_Changed);
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

        private void txtAttribute4_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute4.IsInEditMode && _groupClone.TradeAttribute4 != _group.TradeAttribute4)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute4_Changed);
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

        private void txtAttribute5_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute5.IsInEditMode && _groupClone.TradeAttribute5 != _group.TradeAttribute5)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute5_Changed);
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

        private void txtAttribute6_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtAttribute6.IsInEditMode && _groupClone.TradeAttribute6 != _group.TradeAttribute6)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.TradeAttribute6_Changed);
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

        private void txtSoftCommission_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && !string.IsNullOrEmpty(txtSoftCommission.Text))
                {
                    txtSoftCommission.Text = string.Format("{0:#,##0.########}", double.Parse(txtSoftCommission.Text));
                    _group.IsAnotherTaxlotAttributesUpdated = true;
                    _group.CommSource = CommisionSource.Manual;
                    _group.SoftCommSource = CommisionSource.Manual;

                    _group.CompanyUserID = _userID;
                    _group.IsModified = true;

                    UpdateUnAllocatedGroupTaxlotState();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtSoftCommission_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionFields commFields = null;
                double updatedCommission = 0;
                if ((_group != null) && !string.IsNullOrEmpty(txtSoftCommission.Text))
                {
                    updatedCommission = double.Parse(txtSoftCommission.Text);

                    _group.SoftCommission = updatedCommission;

                    commFields = new CommissionFields();
                    commFields.SoftCommission = updatedCommission;

                    _group.UpdateTaxlotCommissionAndFees(commFields);
                    if (txtSoftCommission.IsInEditMode)
                    {
                        AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.SoftCommission_Changed);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// added by: Bharat Raturi, 18 jun 2014
        /// make the controls holding the data read only
        /// </summary>
        internal void MakeControlsReadOnly(Control parCtrl, bool isReadOnly)
        {
            try
            {
                if (isReadOnly)
                {
                    foreach (Control ctrl in parCtrl.Controls)
                    {
                        MakeControlsReadOnly(ctrl, isReadOnly);
                        if (ctrl is Infragistics.Win.UltraWinEditors.UltraTextEditor || ctrl is UltraCombo || ctrl is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor || ctrl is Button || ctrl is Infragistics.Win.UltraWinEditors.UltraComboEditor)
                        {
                            ctrl.Enabled = false;
                        }
                    }
                }
                else
                {
                    foreach (Control ctrl in parCtrl.Controls)
                    {
                        MakeControlsReadOnly(ctrl, isReadOnly);
                        if (ctrl is Infragistics.Win.UltraWinEditors.UltraTextEditor || ctrl is UltraCombo || ctrl is Infragistics.Win.UltraWinEditors.UltraDateTimeEditor || ctrl is Button || ctrl is Infragistics.Win.UltraWinEditors.UltraComboEditor)
                        {
                            ctrl.Enabled = true;
                        }
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

        private void updateAttribList(Infragistics.Win.UltraWinEditors.UltraComboEditor ctl)
        {
            if (ctl.ValueList != null && ctl.Text != null && ctl.Text.Length > 0)
            {
                BindableValueList bvl = (BindableValueList)ctl.ValueList;
                BindingSource bs = (BindingSource)bvl.DataSource;
                DataView dw = (DataView)bs.DataSource;
                if (dw.Find(ctl.Text) < 0)
                {
                    DataRowView drw = dw.AddNew();
                    drw.BeginEdit();
                    drw[0] = ctl.Text;
                    drw.EndEdit();
                }
            }
        }

        private void txtInternalComments_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null && txtInternalComments.IsInEditMode && _groupClone.InternalComments != txtInternalComments.Text)
                {
                    AuditManager.Instance.AddActionToAllGroupAndTaxlots(_group, TradeAuditActionType.ActionType.InternalComments_Changed);
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

        private void txtInternalComments_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    _group.IsAnotherTaxlotAttributesUpdated = true;

                    _group.InternalComments = txtInternalComments.Text;

                    if (_group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED)
                    {
                        // update taxlots value and state to publish
                        _group.UpdateGroupTaxlots(string.Empty, string.Empty);
                    }
                    else
                    {
                        UpdateUnAllocatedGroupTaxlotState();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtAvgPrice_EnabledChanged(object sender, EventArgs e)
        {
            try
            {  
                if (_group != null)
                {
                    if (_group.SettlementCurrencyID != _group.CurrencyID)
                    {
                        if (txtAvgPrice.Enabled == true)
                        {
                            //Average Price is to be auto calculated
                            //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                            {
                                txtAvgPrice.Enabled = false;
                            }
                        }
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

        private void txtSettlCurrFxRate_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    if (_group.SettlementCurrencyID != _group.CurrencyID)
                    {
                        if (txtSettlCurrFxRate.Enabled == true)
                        {
                            //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementFXRate)
                            {
                                txtSettlCurrFxRate.Enabled = false;
                            }
                        }
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
        void cmbSettlFXConvOperator_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    if (_group.SettlementCurrencyID != _group.CurrencyID)
                    {
                        if (cmbSettlFXConvOperator.Enabled == true)
                        {
                            //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementFXRate)
                            {
                                cmbSettlFXConvOperator.Enabled = false;
                            }
                        }
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
        private void txtSettlCurrAmt_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                if (_group != null)
                {
                    if (_group.SettlementCurrencyID != _group.CurrencyID)
                    {
                        if (txtSettlCurrAmt.Enabled == true)
                        {
                            //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.SettlementPrice)
                            {
                                txtSettlCurrAmt.Enabled = false;
                            }
                        }
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
    }
}