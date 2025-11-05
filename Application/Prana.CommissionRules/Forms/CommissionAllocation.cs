using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Allocation.BLL;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.CommonDataCache;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.CommissionRules
{
    public partial class CommissionAllocation : Form, ICommissionCalculation
    {
        public CommissionAllocation()
        {
            InitializeComponent();
            ctrlCommissionFilters1.RefreshClick += new EventHandler(ctrlCommissionFilters1_RefreshClick);
            ctrlRecalculate1.RecalculateCommission += new EventHandler(ctrlRecalculate1_RecalculateCommission);
           
            //ctrlRecalculate1_RecalculateCommission += new EventHandler(ctrlRecalculate1_ViewRule);
        }
        //void ctrlRecalculate1_ViewRule(object sender, EventArgs e)
        //{
        //    CommissionRule commissionRule = sender as CommissionRule;
            
        //}

        void ctrlRecalculate1_RecalculateCommission(object sender, EventArgs e)
        {
            try
            {
                    btn_Save.Enabled = false;
                {
                    CommissionRule commissionRule = sender as CommissionRule;

                    CommissionRuleEvents commissionRuleEvent = (CommissionRuleEvents)e;

                    CommissionCalculator commissionCalculator = new CommissionCalculator();


                    foreach (UltraGridRow existingRow in this.grdGroupedOrders.Rows.GetFilteredInNonGroupByRows())
                    {
                        AllocationGroup allocatedGroup = (AllocationGroup)existingRow.ListObject;
                        AllocationFund allocfund = new AllocationFund();

                        if (commissionRuleEvent.GroupWise == true)
                        {
                            //BasketGroup btGroup = _basketFundAllocationManager.AllocatedBasketGroups.GetBasketGroup(allocationOrder.GroupID);
                            if (commissionRule.CommissionRate != float.MinValue)
                            {
                                commissionCalculator.CalculateCommissionGroupwise(commissionRule, allocatedGroup);
                            }
                            if (commissionRule.ClearingFeeRate != float.MinValue)
                            {
                                commissionCalculator.CalculateFeesGroupwise(commissionRule, allocatedGroup);
                            }
                            //btGroup.Commission += allocationOrder.Commission;
                            //btGroup.Fees += allocationOrder.Fees;
                            double tempCommission = allocatedGroup.Commission;
                            double tempFees = allocatedGroup.Fees;


                            foreach (UltraGridRow row in existingRow.ChildBands[1].Rows)
                            {
                                allocfund = (AllocationFund)row.ListObject;
                                if (_commissionCalculationTime.Equals(false)) // _commissionCalculationTime==true is Post Allocation. _commissionCalculationTime==true is Pre Allocation. 
                                {
                                    allocfund.Parent = null;
                                }
                                allocfund.Commission = 0;
                                allocfund.Fees = 0;
                                allocfund.Commission = (tempCommission * allocfund.AllocatedQty) / (allocatedGroup.AllocatedQty);
                                allocfund.Fees = (tempFees * allocfund.AllocatedQty) / (allocatedGroup.AllocatedQty);
                                allocfund.Parent = (AllocationGroup)existingRow.ListObject;
                            }
                            grdGroupedOrders.Refresh();
                        }
                        else if (commissionRuleEvent.GroupWise == false && _commissionCalculationTime == true)
                        {


                            allocatedGroup.Commission = 0;
                            allocatedGroup.Fees = 0;


                            foreach (UltraGridRow row in existingRow.ChildBands[1].Rows)
                            {
                                allocfund.Parent = null;
                                allocfund = (AllocationFund)row.ListObject;
                                if (commissionRule.CommissionRate != float.MinValue)
                                {
                                    commissionCalculator.CalculateCommissionFundWise(commissionRule, allocfund, allocatedGroup);

                                }
                                if (commissionRule.ClearingFeeRate != float.MinValue)
                                {
                                    commissionCalculator.CalculateFeesFundWise(commissionRule, allocfund, allocatedGroup);
                                }
                                if (allocfund.Parent == null)
                                {
                                    allocatedGroup.Commission += allocfund.Commission;
                                    allocatedGroup.Fees += allocfund.Fees;
                                }
                                allocfund.Parent = (AllocationGroup)existingRow.ListObject;
                            }

                            this.ModifiedallocationGroups = (AllocationGroups)grdGroupedOrders.DataSource;
                            grdGroupedOrders.Refresh();
                        }
                        else if (commissionRuleEvent.GroupWise == false && _commissionCalculationTime == false)
                        {
                            allocatedGroup.Commission = 0;
                            allocatedGroup.Fees = 0;


                            foreach (UltraGridRow row in existingRow.ChildBands[1].Rows)
                            {

                                allocfund = (AllocationFund)row.ListObject;
                                if (commissionRule.CommissionRate != float.MinValue)
                                {
                                    commissionCalculator.CalculateCommissionFundWise(commissionRule, allocfund, allocatedGroup);
                                }
                                if (commissionRule.ClearingFeeRate != float.MinValue)
                                {
                                    commissionCalculator.CalculateFeesFundWise(commissionRule, allocfund, allocatedGroup);
                                }


                                allocatedGroup.Commission += allocfund.Commission;
                                allocatedGroup.Fees += allocfund.Fees;

                            }

                            this.ModifiedallocationGroups = (AllocationGroups)grdGroupedOrders.DataSource;
                            grdGroupedOrders.Refresh();
                        }
                        //commissionCalculator.CalculateCommissionGroupwise(commissionRule,grdGroupedOrders.Capture);


                    }
                }
            }
            catch(Exception ex )
            {
                  bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }

            }
            finally
                {
                    btn_Save.Enabled = true;
                }
            
        }
            
        private AllocationGroups _allocationGroups = new AllocationGroups();
        public AllocationGroups ModifiedallocationGroups
        {
            get
            {
                return _allocationGroups;
            }
            set
            {
                value = _allocationGroups;
            }
        }

        
        void ctrlCommissionFilters1_RefreshClick(object sender, EventArgs e)
        {
            //((Prana.CommissionRules.CtrlCommissionFilters)sender).CmbAsset.Value;
            UltraGridBand band = grdGroupedOrders.DisplayLayout.Bands[0];
            RowsCollection rows = grdGroupedOrders.DisplayLayout.Rows;
            //Clear all filters
            foreach (UltraGridBand var in grdGroupedOrders.DisplayLayout.Bands)
            {
                band.ColumnFilters.ClearAllFilters();

            }

            if (ctrlCommissionFilters1.CmbAsset.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbAsset.Value.ToString()) != int.MinValue)
                {
                    grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_AssetID].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbAsset.Value);
                }
            }
            if (ctrlCommissionFilters1.CmbSide.Value != null)
            {

                switch (ctrlCommissionFilters1.CmbSide.Value.ToString())
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_BuyMinus:
                    case FIXConstants.SIDE_Cross:
                    case FIXConstants.SIDE_CrossShort:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_SellPlus:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_SellShortExempt:
                        // TODO: find fix tagvalues for the following sides and add the cases
                        //case FIXConstants.SIDE_CrossShortExempt:
                        //case FIXConstants.SIDE_Opposite:
                        band.ColumnFilters[COL_OrderSide].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbSide.Text);
                        break;
                    default:

                        break;
                }
            }
            //if (ctrlCommissionFilters1.CmbSide.Value != null)
            //{

            //    if (int.Parse(ctrlCommissionFilters1.CmbSide.Value.ToString()) != int.MinValue)
            //    {
            //        grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_OrderSide].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbSide.Value);
            //    }
            //}
            if (ctrlCommissionFilters1.CmbUnderLying.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbUnderLying.Value.ToString()) != int.MinValue)
                {
                    grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_UnderlyingID].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbUnderLying.Value);
                }
            }
            if (ctrlCommissionFilters1.CmbVenue.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbVenue.Value.ToString()) != int.MinValue)
                {
                    grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_VenueID].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbVenue.Value);
                }
            }
            if (ctrlCommissionFilters1.CmbCounterParty.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbCounterParty.Value.ToString()) != int.MinValue)
                {
                    grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_CounterPartyID].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbCounterParty.Value);
                }
            }



            if (ctrlCommissionFilters1.TextBoxSymbol.Text != string.Empty)
            {
                string[] txtSymbol = ctrlCommissionFilters1.TextBoxSymbol.Text.Split(',');

                grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters[COL_Symbol].LogicalOperator = FilterLogicalOperator.Or;
                foreach (string s in txtSymbol)
                {
                    grdGroupedOrders.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.Add(FilterComparisionOperator.StartsWith, s);
                }
            }

        }
        

        private CompanyUser _currentUser;

        public CompanyUser CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                ctrlCommissionFilters1.SetUp(_currentUser);
            }
        }
        public Form Reference()
        {
            return this;
        }

        public event EventHandler CommissionAllocationClosed;
      

        #region Grouped_Order_Columns

        const string COL_OrderSideTagValue = "OrderSideTagValue";
        const string COL_OrderSide = "OrderSide";
        const string COL_OrderType = "OrderType";
        const string COL_OrderTypeTagValue = "OrderTypeTagValue";
        const string COL_Symbol = "Symbol";
        const string COL_Venue = "Venue";
        const string COL_Quantity = "Quantity";
        //const string COL_ClOrderID = "ClOrderID";
        const string COL_AvgPrice = "AvgPrice";
        const string COL_AssetID = "AssetID";
        const string COL_AssetName = "AssetName";
        const string COL_UnderlyingID = "UnderlyingID";
        const string COL_UnderlyingName = "UnderlyingName";
        const string COL_UnderLying = "Underlying";
        const string COL_BasketName = "BasketName";
        const string COL_ExeValue = "ExeValue";

        const string COL_ExchangeID = "ExchangeID";
        const string COL_ExchangeName = "ExchangeName";
        const string COL_CurrencyID = "CurrencyID";
        const string COL_CurrencyName = "CurrencyName";
        const string COL_AUECID = "AUECID";
        const string COL_TradingAccountID = "TradingAccountID";
        const string COL_TradingAccountName = "TradingAccountName";
        const string COL_TradingAccount = "TradingAccount";
        const string COL_UserID = "UserID";
        const string COL_CounterPartyID = "CounterPartyID";
        const string COL_CounterPartyName = "CounterPartyName";
        const string COL_VenueID = "VenueID";
        const string COL_CumQty = "CumQty";
        const string COL_AllocatedQty = "AllocatedQty";
       
        const string COL_Updated = "Updated";
        const string COL_NotAllExecuted = "NotAllExecuted";
        const string COL_ListID = "ListID";
        const string COL_GroupID = "GroupID";
        const string COL_FundID = "FundID";
        const string COL_StrategyID = "StrategyID";//const string COL_AllocationTypeID = "AllocationTypeID";

        const string COL_OpenClose = "OpenClose";
        const string COL_OrigClOrderID = "OrigClOrderID";
        const string COL_Commission = "Commission";
        const string COL_Fees = "Fees";



        const string COL_AutoGrouped = "AutoGrouped";
        const string COL_IsProrataActive = "IsProrataActive";
        const string COL_IsPreAllocated = "IsPreAllocated";
        const string COL_AllocatedEqualTotalQty = "AllocatedEqualTotalQty";
        const string COL_State = "State";
        const string COL_AllocationType = "AllocationType";
        const string COL_SingleOrderAllocation = "SingleOrderAllocation";
        const string COL_BasketGroupID = "BasketGroupID";
        const string COL_IsBasketGroup = "IsBasketGroup";
        const string COL_IsCommissionCalculated = "IsCommissionCalculated";
        const string COL_GroupState = "GroupState";
        const string COL_BaseCurrencyID = "BaseCurrencyID";
        const string COl_IsManualGroup = "IsManualGroup";
        const string COl_AUECLocalDate = "AUECLocalDate";
        //const string COL_Fees = "SingleOrderAllocation";
        //const string COL_Fees = "SingleOrderAllocation";
        


        const string COL_AF_FundName = "FundName";
        const string COL_AF_FundID = "FundID";
        const string COL_AF_Percentage = "Percentage";
        const string COL_AF_AllocatedQty = "AllocatedQty";
        const string COL_AF_GroupID = "GroupID";
        const string COL_AF_Commission = "Commission";
        const string COL_AF_Fees = "Fees";
        const string COL_AF_Parent = "Parent";

        #endregion

        #region Order_columns_Hidden

        //const string COL_OrderSideTagValue = "OrderSideTagValue";
        //const string COL_OrderSide = "OrderSide";
        //const string COL_OrderType = "OrderType";
        //const string COL_OrderTypeTagValue = "OrderTypeTagValue";
        //const string COL_Symbol = "Symbol";
        //const string COL_Venue = "Venue";
        //const string COL_Quantity = "Quantity";
        //const string COL_ClOrderID = "ClOrderID";
        //const string COL_AvgPrice = "AvgPrice";
        //const string COL_AssetID = "AssetID";
        //const string COL_AssetName = "AssetName";
        //const string COL_UnderlyingID = "UnderlyingID";
        //const string COL_UnderlyingName = "UnderlyingName";

        //const string COL_ExchangeID = "ExchangeID";
        //const string COL_ExchangeName = "ExchangeName";
        //const string COL_CurrencyID = "CurrencyID";
        //const string COL_CurrencyName = "CurrencyName";
        //const string COL_AUECID = "AUECID";
        //const string COL_TradingAccountID = "TradingAccountID";
        //const string COL_TradingAccountName = "TradingAccountName";
        //const string COL_UserID = "UserID";
        //const string COL_CounterPartyID = "CounterPartyID";
        //const string COL_CounterPartyName = "CounterPartyName";
        //const string COL_VenueID = "VenueID";
        //const string COL_CumQty = "CumQty";
        //const string COL_AllocatedQty = "AllocatedQty";
        //const string COL_Updated = "Updated";
        //const string COL_NotAllExecuted = "NotAllExecuted";
        //const string COL_ListID = "ListID";
        //const string COL_GroupID = "GroupID";
        //const string COL_FundID = "FundID";
        //const string COL_StrategyID = "StrategyID";
        //const string COL_AllocationTypeID = "AllocationTypeID";

        //const string COL_OpenClose = "OpenClose";
        //const string COL_OrigClOrderID = "OrigClOrderID";
        //const string COL_Commission = "Commission";
        //const string COL_Fees = "Fees";

        #endregion

        //OrderFundAllocationManager _orderFundAllocationManager = null;
        OrderFundAllocationManager _orderFundAllocationManager = OrderFundAllocationManager.GetInstance;
        BasketFundAllocationManager _basketFundAllocationManager = BasketFundAllocationManager.GetInstance;

        private void GetAllAlloctedOrders()
        {
            // _orderFundAllocationManager = OrderFundAllocationManager.GetInstance;          
            AllocationGroups allocationGroups = _orderFundAllocationManager.AllocatedGroups;
            foreach (AllocationGroup allocationGroup in allocationGroups)
            {
                allocationGroup.AssetName = CachedDataManager.GetInstance.GetAssetText(allocationGroup.AssetID);
                allocationGroup.UnderLyingName = CachedDataManager.GetInstance.GetUnderLyingText(allocationGroup.UnderLyingID);
                allocationGroup.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(allocationGroup.ExchangeID);
               if (allocationGroup.CurrencyID != int.MinValue)
                {
                    allocationGroup.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(allocationGroup.CurrencyID);
                    
                }
                else
                {
                    allocationGroup.CurrencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(allocationGroup.AUECID);
                    allocationGroup.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(allocationGroup.CurrencyID);
                }
                foreach (AllocationFund allocationFund in allocationGroup.AllocationFunds)
                {
                    allocationFund.FundName = CachedDataManager.GetInstance.GetFundText(allocationFund.FundID);
                }
            }
            grdGroupedOrders.DataSource = null;
            grdGroupedOrders.DataSource = _orderFundAllocationManager.AllocatedGroups;
            grdGroupedOrders.DataBind();
           
            
           

        }
        private void GetAllBasketAllocatedGroups()
        {
            BasketGroupCollection basketGroupCollection = _basketFundAllocationManager.AllocatedBasketGroups;  
            //AllocationOrderCollection _allocationOrderCollection = new AllocationOrderCollection();
            grdBasketGroupedOrders.DataSource = null;
            grdBasketGroupedOrders.DataSource = _basketFundAllocationManager.AllocatedBasketGroups;
            grdBasketGroupedOrders.DataBind();

        }

        bool _commissionCalculationTime = false;
        AllocationOrderCollection _allocationOrderCollection = new AllocationOrderCollection();

        private void CommissionAllocation_Load(object sender, EventArgs e)
        {
            try
            {

                // _gridBandGroupedOrders.Columns.Add("Text");
                _orderFundAllocationManager = OrderFundAllocationManager.GetInstance;
                CommissionCalculator commissionCalculator = new CommissionCalculator();
                _basketFundAllocationManager = BasketFundAllocationManager.GetInstance;


                AllocationFunds allocationFundsFromDB = OrderAllocationDBManager.GetCommissionsAndFeesFromDB();

                if (allocationFundsFromDB.Count > 0)
                {
                    foreach (AllocationFund allocationFundDB in allocationFundsFromDB)
                    {
                        foreach (AllocationGroup allocationGroup in _orderFundAllocationManager.AllocatedGroups)
                        {
                            if (allocationFundDB.GroupID == allocationGroup.GroupID)
                            {
                                AllocationFunds allocationFunds = allocationGroup.AllocationFunds;
                                foreach (AllocationFund allocationFund in allocationFunds)
                                {
                                    if (allocationFund.FundID == allocationFundDB.FundID)
                                    {
                                        allocationFund.Commission = allocationFundDB.Commission;
                                        allocationFund.Fees = allocationFundDB.Fees;
                                        allocationGroup.IsCommissionCalculated = true;
                                        break;
                                    }

                                }
                                break;
                            }

                        }
                    }

                    foreach (AllocationGroup allocationGroup in _orderFundAllocationManager.AllocatedGroups)
                    {
                        double groupwiseComm = 0;
                        double groupwiseFee = 0;

                        allocationGroup.CommissionCalculationTime = true;

                        AllocationFunds allocationFunds = allocationGroup.AllocationFunds;
                        foreach (AllocationFund allocationFund in allocationFunds)
                        {
                            groupwiseComm = groupwiseComm + allocationFund.Commission;
                            groupwiseFee = groupwiseFee + allocationFund.Fees;
                        }
                        allocationGroup.Commission = groupwiseComm;
                        allocationGroup.Fees = groupwiseFee;
                    }
                }
                AllocationFunds allocationFundsfromDbBasket = BasketAllocationDBManager.GetCommissionsAndFeesFromDBForBasket();
                if (allocationFundsfromDbBasket.Count > 0)
                {
                    foreach (BasketGroup basketgroup in _basketFundAllocationManager.AllocatedBasketGroups)
                    {
                        basketgroup.Commission = 0.0;
                        basketgroup.Fees = 0.0;
                        OrderCollection ordercollection = null;
                        AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
                        ordercollection = basketgroup.AddedBaskets[0].BasketOrders;
                        AllocationFunds funds = null;
                        funds = basketgroup.AllocationFunds;
                        foreach (Order order in ordercollection)
                        {
                            AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
                            AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
                            allocationOrder.AllocationFunds = orderFunds;
                            allocationOrderCollection.Add(allocationOrder);
                            _allocationOrderCollection.Add(allocationOrder);
                        }
                        foreach (AllocationOrder allocationOrder in allocationOrderCollection)
                        {
                            bool IsCommissionCalculated = false;
                            allocationOrder.Commission = 0.0;
                            allocationOrder.Fees = 0.0;
                            AllocationFunds allocationFunds = allocationOrder.AllocationFunds;
                            if (allocationFunds != null)
                            {
                                foreach (AllocationFund allocationFund in allocationFunds)
                                {
                                    allocationFund.Commission = 0.0;
                                    allocationFund.Fees = 0.0;
                                    allocationFund.Commission = commissionCalculator.GetCommission(allocationOrder.ClOrderID, allocationFund.FundID, allocationFundsfromDbBasket, out IsCommissionCalculated);
                                    allocationFund.Fees = commissionCalculator.GetFees(allocationOrder.ClOrderID, allocationFund.FundID, allocationFundsfromDbBasket);
                                    allocationOrder.Commission += allocationFund.Commission;
                                    allocationOrder.Fees += allocationFund.Fees;
                                }
                            }
                            basketgroup.Commission += allocationOrder.Commission;
                            basketgroup.Fees += allocationOrder.Fees;
                            basketgroup.IsCommissionCalculated = IsCommissionCalculated;
                        }

                    }

                }

                #region Commented //if (allocationFundsfromDbBasket.Count > 0)
                //if (allocationFundsfromDbBasket.Count > 0)
                //{
                //    foreach (BasketGroup basketgroup in _basketFundAllocationManager.AllocatedBasketGroups)
                //    {
                //        basketgroup.Commission = 0.0;
                //        basketgroup.Fees = 0.0;
                //        OrderCollection ordercollection = null;
                //        AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
                //        ordercollection = basketgroup.AddedBaskets[0].BasketOrders;
                //        AllocationFunds funds = null;
                //        funds = basketgroup.AllocationFunds;
                //        foreach (Order order in ordercollection)
                //        {
                //            AllocationOrder allocationOrder= OrderAllocationManager.GetAllocationOrder(order);
                //            AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
                //            allocationOrder.AllocationFunds = orderFunds;
                //            allocationOrderCollection.Add(allocationOrder);
                //        }
                //        foreach (AllocationOrder allocationOrder in allocationOrderCollection)
                //        {
                //            double orderwiseComm = 0;
                //            double orderwiseFee = 0;
                //           // allocationOrder.CommissionCalculationTime = true;

                //            AllocationFunds allocationFunds = allocationOrder.AllocationFunds;
                //            if (allocationFunds != null)
                //            {
                //                foreach (AllocationFund allocationFund in allocationFunds)
                //                {
                //                    orderwiseComm = orderwiseComm + allocationFund.Commission;
                //                    orderwiseFee = orderwiseFee + allocationFund.Fees;
                //                }
                //                allocationOrder.Commission = orderwiseComm;
                //                allocationOrder.Fees = orderwiseFee;
                //            }
                //            basketgroup.Commission += allocationOrder.Commission;
                //            basketgroup.Fees += allocationOrder.Fees;
                //            basketgroup.IsCommissionCalculated = true;
                //        }

                //    }
                //}
                #endregion Commented 

                if (_orderFundAllocationManager.AllocatedGroups.Count > 0)
                {
                    // fill Saved Commission Rules Cache
                    //CommissionDBManager.GetAllSavedCommissionRules();
                    //get Commission Calculation Time PreAllcation or PostAllocation
                    _commissionCalculationTime = CommissionDBManager.GetCommissionCalculationTime();
                    if (_commissionCalculationTime.Equals(true))
                    {
                        this.Text = "Commission Calculation : Post Allocation";
                    }
                    else
                    {
                        this.Text = "Commission Calculation : Pre Allocation";
                    }
                    // set the Commission Calculation Time Property
                    CommissionRulesCacheManager.GetInstance().SetAllocatedCalculationProperty(_commissionCalculationTime);

                    // fill Saved CV-AUEC Commission Rules Cache           
                    CommissionDBManager.GetAllCommissionRulesForCVAUEC(_currentUser.CompanyID);
                    // Now start Commission Calculation

                    foreach (AllocationGroup allocationGroup in _orderFundAllocationManager.AllocatedGroups)
                    {
                        commissionCalculator.StartCalculation(allocationGroup); 
                    }

                    //commissionCalculator.StartCalculation(_orderFundAllocationManager.AllocatedGroups);

                }
                if (_basketFundAllocationManager.AllocatedBasketGroups.Count > 0)
                {
                    // fill Saved Commission Rules Cache
                    CommissionDBManager.GetAllSavedCommissionRules();
                    //get Commission Calculation Time PreAllcation or PostAllocation
                    _commissionCalculationTime = CommissionDBManager.GetCommissionCalculationTime();
                    if (_commissionCalculationTime.Equals(true))
                    {
                        this.Text = "Commission Calculation : Post Allocation";
                    }
                    else
                    {
                        this.Text = "Commission Calculation : Pre Allocation";
                    }
                    // set the Commission Calculation Time Property
                    CommissionRulesCacheManager.GetInstance().SetAllocatedCalculationProperty(_commissionCalculationTime);

                    // fill Saved CV-AUEC Commission Rules Cache           
                    CommissionDBManager.GetAllCommissionRulesForCVAUEC(_currentUser.CompanyID);
                    // Now start Commission Calculation
                    AllocationOrderCollection tempAllocationOrderCollection = new AllocationOrderCollection();

                    foreach (BasketGroup  basketGroup in _basketFundAllocationManager.AllocatedBasketGroups)
                    {                       
                       tempAllocationOrderCollection  = commissionCalculator.StartCalculationforBasket(basketGroup);
                       foreach (AllocationOrder allocationorder in tempAllocationOrderCollection)
                       {
                           _allocationOrderCollection.Add(allocationorder);
                       }
                    }
                   //_allocationOrderCollection = commissionCalculator.StartCalculationforBasket(_basketFundAllocationManager.AllocatedBasketGroups);
                    
                }
                GetAllAlloctedOrders();
                GetAllBasketAllocatedGroups();
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //string ab = grdGroupedOrders.ActiveRow.Cells[0].Value.ToString();
            AllocationGroups allocatedGroupsnew = new AllocationGroups();
            Infragistics.Win.UltraWinGrid.UltraGridRow[] col = grdGroupedOrders.Rows.GetFilteredInNonGroupByRows();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in col)
            {
                allocatedGroupsnew.Add((AllocationGroup)row.ListObject);
            }
            AllocationGroups allocatedGroups = (AllocationGroups)grdGroupedOrders.DataSource;
            BasketGroupCollection basketGroupCollection = (BasketGroupCollection)grdBasketGroupedOrders.DataSource;
            //AllocationOrderCollection orders = (AllocationOrderCollection)grdBasketGroupedOrders.DataSource;
           

            // collect all the modified groups
            AllocationGroups allocatedGroupsModified = new AllocationGroups();
            BasketGroupCollection basketGroupCollectionModified = new BasketGroupCollection();
            foreach (AllocationGroup allocationGroup in allocatedGroups)
            {
                if (allocationGroup.IsCommissionCalculated == false)
                {
                    allocatedGroupsModified.Add(allocationGroup);
                }                
            }
            foreach (BasketGroup basketGroup in basketGroupCollection)
            {
                if (basketGroup.IsCommissionCalculated == false)
                {
                    basketGroupCollectionModified.Add(basketGroup);
                }
            }

            //string xmlAllaoctedGroups = Utilities.XMLUtilities.SerializeToXML(allocatedGroups);
            //string xmlOrders = Utilities.XMLUtilities.SerializeToXML(orders);
            if (allocatedGroupsModified.Count > 0) 
            {
                OrderAllocationDBManager.SaveAndUpdateCommissionandFees(allocatedGroupsModified);
                OrderAllocationDBManager.SaveCommissionAndFeesForstrategy();
                MessageBox.Show("Commission and Fees saved successfully");
            }
            //if (basketGroupCollectionModified.Count > 0)
            //{
            //    BasketAllocationDBManager.SaveAndUpdateCommissionandFeesForBasket(basketGroupCollectionModified);
            //    OrderAllocationDBManager.SaveCommissionAndFeesForstrategy();
            //    MessageBox.Show("Commission and Fees Saved Successfully for Baskets");
            //}
                
        }

        UltraGridBand _gridBandGroupedOrders = null;
        UltraGridBand _gridBandAllocationFunds = null;
        private void grdGroupedOrders_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridLayout gridLayout = grdGroupedOrders.DisplayLayout;
            //SetGroupedGridAppearanceAndLayout(ref gridLayout);




            if (_orderFundAllocationManager.AllocatedGroups.Count > 0)
            {
                _gridBandGroupedOrders = grdGroupedOrders.DisplayLayout.Bands[0];
                SetGroupedOrdersColumns(_gridBandGroupedOrders);
               
                grdGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Commission].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Commission].CellAppearance.ForeColor = Color.Black;
                grdGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Fees].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Fees].CellAppearance.ForeColor = Color.Black;

                //grdGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Commission].CellAppearance.BackColor = Color.DeepSkyBlue;
                //grdGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Commission].CellAppearance.ForeColor = Color.Black;
                //grdGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Fees].CellAppearance.BackColor = Color.DeepSkyBlue;
                //grdGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Fees].CellAppearance.ForeColor = Color.Black;

                //_gridBandAllocationFunds = grdGroupedOrders.DisplayLayout.Bands[2];
                //SetAllocationFundsColumns(_gridBandAllocationFunds);

                grdGroupedOrders.DisplayLayout.Bands[1].Hidden = true;

                _gridBandAllocationFunds = grdGroupedOrders.DisplayLayout.Bands[3];
                if (_gridBandAllocationFunds != null)
                {
                    SetAllocationFundsColumns(_gridBandAllocationFunds);
                }
            }
        }

        #region PostAllocated_Tab
        /// <summary>
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="grid">The grid.</param>
        private void SetGroupedGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {
            gridLayout.Appearance.BackColor = Color.Black;
            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
            gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
            gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
            gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
            gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
            gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

            gridLayout.AutoFitStyle = AutoFitStyle.None;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            //gridLayout.Override.AllowAddNew = AllowAddNew.Yes;
            gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;
        }

        /// <summary>
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetGroupedOrdersColumns(UltraGridBand gridBand)
        {

            UltraGridColumn colSymbol = gridBand.Columns[COL_Symbol];
            colSymbol.Width = 50;
            colSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colSymbol.Header.VisiblePosition = 1;
            colSymbol.CellActivation = Activation.NoEdit;
            //colSymbol.Hidden = true;

            UltraGridColumn ColAssetName = gridBand.Columns[COL_AssetName];
            ColAssetName.Width = 65;
            ColAssetName.Header.Caption = "Asset";
            ColAssetName.Header.VisiblePosition = 2;
            ColAssetName.CellActivation = Activation.NoEdit;
            ColAssetName.Hidden = false;

            UltraGridColumn colOrderSide = gridBand.Columns[COL_OrderSide];
            colOrderSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colOrderSide.Header.VisiblePosition = 3;
            colOrderSide.CellActivation = Activation.NoEdit;
            colOrderSide.Header.Caption = "Side";
            //colOrderSide.Hidden = true;

            UltraGridColumn colAllocatedQty = gridBand.Columns[COL_AllocatedQty];
            colAllocatedQty.Width = 45;
            colAllocatedQty.Header.Caption = "Allocated";
            colAllocatedQty.Header.VisiblePosition = 4;
            colAllocatedQty.CellActivation = Activation.NoEdit;

           

            if (_commissionCalculationTime.Equals(true))
            {
                UltraGridColumn colCommission = gridBand.Columns[COL_Commission];
                //colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colCommission.Width = 110;
                colCommission.Header.Caption = "Commission";
                colCommission.Header.VisiblePosition = 5;
                colCommission.CellActivation = Activation.NoEdit;
                //colCommission.CellClickAction = CellClickAction.Edit;

                UltraGridColumn colFees = gridBand.Columns[COL_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 60;
                colFees.Header.VisiblePosition = 6;
                colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colFees.CellActivation = Activation.NoEdit;
            }
            else
            {
                UltraGridColumn colCommission = gridBand.Columns[COL_Commission];
                //colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colCommission.Width = 110;
                colCommission.Header.Caption = "Commission";
                colCommission.Header.VisiblePosition = 5;
                //colCommission.CellActivation = Activation.AllowEdit;
                //colCommission.CellClickAction = CellClickAction.Edit;

                UltraGridColumn colFees = gridBand.Columns[COL_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 60;
                colFees.Header.VisiblePosition = 6;
                colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            UltraGridColumn colAvgPrice = gridBand.Columns[COL_AvgPrice];
            colAvgPrice.Width = 80;
            colAvgPrice.Header.Caption = "AvgPX";
            colAvgPrice.Header.VisiblePosition = 7;
            colAvgPrice.CellActivation = Activation.NoEdit;
            colAvgPrice.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;    

            UltraGridColumn colCounterPartyName = gridBand.Columns[COL_CounterPartyName];
            colCounterPartyName.Width = 80;
            colCounterPartyName.Header.Caption = "Counter Party";
            colCounterPartyName.Header.VisiblePosition = 8;
            colCounterPartyName.CellActivation = Activation.NoEdit;

            UltraGridColumn colVenue = gridBand.Columns[COL_Venue];
            colVenue.Width = 55;
            colVenue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colVenue.Header.VisiblePosition = 9;
            colVenue.Header.Caption = "Venue";
            colVenue.CellActivation = Activation.NoEdit;
            //colVenue.Hidden = true;

            UltraGridColumn colAssetID = gridBand.Columns[COL_AssetID];
            colAssetID.Hidden = true;

            UltraGridColumn colUnderlyingName = gridBand.Columns[COL_UnderlyingName];
            colUnderlyingName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUnderlyingName.Header.VisiblePosition = 10;
            colUnderlyingName.Header.Caption = "Underlying";
            colUnderlyingName.CellActivation = Activation.NoEdit;
            //colUnderlyingName.Hidden = true;

            UltraGridColumn colExchangeName = gridBand.Columns[COL_ExchangeName];
            colExchangeName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colExchangeName.Header.VisiblePosition = 11;
            colExchangeName.Header.Caption = "Exchange";
            colExchangeName.CellActivation = Activation.NoEdit;

            UltraGridColumn colCurrencyName = gridBand.Columns[COL_CurrencyName];
            colCurrencyName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colCurrencyName.Hidden = true;
            colCurrencyName.Header.VisiblePosition = 12;
            colCurrencyName.Header.Caption = "Currency";
            colCurrencyName.CellActivation = Activation.NoEdit;

            //UltraGridColumn colAllocationTypeID = gridBand.Columns[COL_AllocationTypeID];
            //colAllocationTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTime;
            //colAllocationTypeID.Width = 140;
            //colAllocationTypeID.Header.Caption = "AllocationTypeID";
            //colAllocationTypeID.Header.VisiblePosition = 2;

            

            

            UltraGridColumn colAUECID = gridBand.Columns[COL_AUECID];
            colAUECID.Width = 65;
            colAUECID.Header.Caption = "AUECID";
            colAUECID.Header.VisiblePosition = 4;
            colAUECID.CellActivation = Activation.NoEdit;
            colAUECID.Hidden = true;

            

            //UltraGridColumn colClOrderID = gridBand.Columns[COL_ClOrderID];
            //colClOrderID.Width = 50;
            //colClOrderID.Header.Caption = "ClOrderID";
            //colClOrderID.Header.VisiblePosition = 7;

            

            UltraGridColumn colCounterPartyID = gridBand.Columns[COL_CounterPartyID];
            colCounterPartyID.Width = 60;
            colCounterPartyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
            colCounterPartyID.Header.Caption = "CounterPartyID";
            colCounterPartyID.Header.VisiblePosition = 8;
            colCounterPartyID.Hidden = true;

            

            //UltraGridColumn colCumQty = gridBand.Columns[COL_CumQty];
            //colCumQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colCumQty.Hidden = true;
            

            UltraGridColumn colCurrencyID = gridBand.Columns[COL_CurrencyID];
            colCurrencyID.Width = 80;
            colCurrencyID.Header.Caption = "Currency ID";
            colCurrencyID.Header.VisiblePosition = 10;
            colCurrencyID.CellActivation = Activation.NoEdit;
            colCurrencyID.Hidden = true;

            


            UltraGridColumn ColExchangeID = gridBand.Columns[COL_ExchangeID];
            ColExchangeID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            ColExchangeID.Hidden = true;
            

            

        
            
            //colExchangeName.Hidden = true;



            //UltraGridColumn colFundID = gridBand.Columns[COL_FundID];
            //colFundID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colFundID.Hidden = true;

            UltraGridColumn colGroupID = gridBand.Columns[COL_GroupID];
            colGroupID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colGroupID.Hidden = true;

            UltraGridColumn colListID = gridBand.Columns[COL_ListID];
            colListID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colListID.Hidden = true;

            UltraGridColumn colNotAllExecuted = gridBand.Columns[COL_NotAllExecuted];
            colNotAllExecuted.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colNotAllExecuted.Hidden = true;

            //UltraGridColumn colOpenClose = gridBand.Columns[COL_OpenClose];
            //colOpenClose.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colOpenClose.Hidden = true;



            

            UltraGridColumn colOrderSideTagValue = gridBand.Columns[COL_OrderSideTagValue];
            colOrderSideTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colOrderSideTagValue.Hidden = true;

            UltraGridColumn colOrderType = gridBand.Columns[COL_OrderType];
            colOrderType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colOrderType.Hidden = true;

            UltraGridColumn colOrderTypeTagValue = gridBand.Columns[COL_OrderTypeTagValue];
            colOrderTypeTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colOrderTypeTagValue.Hidden = true;

            //UltraGridColumn colOrigClOrderID = gridBand.Columns[COL_OrigClOrderID];
            //colOrigClOrderID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colOrigClOrderID.Hidden = true;

            UltraGridColumn colQuantity = gridBand.Columns[COL_CumQty];
            colQuantity.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colQuantity.Header.Caption = "Executed";
            colQuantity.Header.VisiblePosition = 13;
            colQuantity.CellActivation = Activation.NoEdit;
            //colQuantity.Hidden = true;

            //UltraGridColumn colStrategyID = gridBand.Columns[COL_StrategyID];
            //colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colStrategyID.Hidden = true;

            

            UltraGridColumn colTradingAccountID = gridBand.Columns[COL_TradingAccountID];
            colTradingAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colTradingAccountID.Hidden = true;

            UltraGridColumn colTradingAccountName = gridBand.Columns[COL_TradingAccountName];
            colTradingAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colTradingAccountName.Hidden = true;

            UltraGridColumn colUnderlyingID = gridBand.Columns[COL_UnderlyingID];
            colUnderlyingID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUnderlyingID.Hidden = true;

            

            UltraGridColumn colUpdated = gridBand.Columns[COL_Updated];
            colUpdated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUpdated.Hidden = true;

            UltraGridColumn colUserID = gridBand.Columns[COL_UserID];
            colUserID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUserID.Hidden = true;

            

            UltraGridColumn colVenueID = gridBand.Columns[COL_VenueID];
            colVenueID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colVenueID.Hidden = true;

            UltraGridColumn colAutoGrouped = gridBand.Columns[COL_AutoGrouped];
            colAutoGrouped.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAutoGrouped.Hidden = true;

            UltraGridColumn colIsProrataActive = gridBand.Columns[COL_IsProrataActive];
            colIsProrataActive.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colIsProrataActive.Hidden = true;

            UltraGridColumn colIsPreAllocated = gridBand.Columns[COL_IsPreAllocated];
            colIsPreAllocated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colIsPreAllocated.Hidden = true;

            UltraGridColumn colAllocatedEqualTotalQty = gridBand.Columns[COL_AllocatedEqualTotalQty];
            colAllocatedEqualTotalQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAllocatedEqualTotalQty.Hidden = true;

            UltraGridColumn colState = gridBand.Columns[COL_State];
            colState.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colState.Hidden = true;

            UltraGridColumn colAllocationType = gridBand.Columns[COL_AllocationType];
            colAllocationType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAllocationType.Hidden = true;

            UltraGridColumn colBasketGroupID = gridBand.Columns[COL_BasketGroupID];
            colBasketGroupID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colBasketGroupID.Hidden = true;

            UltraGridColumn colIsBasketGroup = gridBand.Columns[COL_IsBasketGroup];
            colIsBasketGroup.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colIsBasketGroup.Hidden = true;

            UltraGridColumn colSingleOrderAllocation = gridBand.Columns[COL_SingleOrderAllocation];
            colSingleOrderAllocation.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colSingleOrderAllocation.Hidden = true;

            UltraGridColumn colIsCommissionCalculated = gridBand.Columns[COL_IsCommissionCalculated];
            colIsCommissionCalculated.Header.Caption = "Parent";
            colIsCommissionCalculated.Hidden = true;

            UltraGridColumn colIsManualGroup = gridBand.Columns[COl_IsManualGroup];
            colIsManualGroup.Hidden = true;

            UltraGridColumn colAUECLocalDate = gridBand.Columns[COl_AUECLocalDate];
            colAUECLocalDate.Hidden = true;
        }
        /// <summary>
        /// Sets the Basket grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetBasketGroupedOrdersColumns(UltraGridBand gridBand)
        {

            UltraGridColumn ColBasketName = gridBand.Columns[COL_BasketName];
            ColBasketName.Width = 110;
            ColBasketName.Header.Caption = "BasketName";
            ColBasketName.Header.VisiblePosition = 1;
            ColBasketName.CellActivation = Activation.NoEdit;
           

            UltraGridColumn ColAssetID = gridBand.Columns[COL_AssetID];
            ColAssetID.Width = 65;
            ColAssetID.Header.Caption = "Asset";
            ColAssetID.Header.VisiblePosition = 2;
            ColAssetID.CellActivation = Activation.NoEdit;
            ColAssetID.Hidden = true ;
         

            UltraGridColumn  ColCumQty = gridBand.Columns[COL_CumQty];
            ColCumQty.Width = 65;
            ColCumQty.Header.Caption = "Cum Qty";
            ColCumQty.Header.VisiblePosition = 3;
            ColCumQty.CellActivation = Activation.NoEdit;

            UltraGridColumn ColExeValue = gridBand.Columns[COL_ExeValue];
            ColExeValue.Width   = 65 ;
            ColExeValue.Header.Caption = "Notional Value";
            ColExeValue.Header.VisiblePosition = 4;
            ColExeValue.CellActivation = Activation.NoEdit;
            //colOrderSide.Hidden = true;

           
           


            if (_commissionCalculationTime.Equals(true))
            {
                UltraGridColumn colCommission = gridBand.Columns[COL_Commission];
                //colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colCommission.Width = 110;
                colCommission.Header.Caption = "Commission";
                colCommission.Header.VisiblePosition = 5;
                colCommission.CellActivation = Activation.NoEdit;
                //colCommission.CellClickAction = CellClickAction.Edit;

                UltraGridColumn colFees = gridBand.Columns[COL_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 60;
                colFees.Header.VisiblePosition = 6;
                colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colFees.CellActivation = Activation.NoEdit;
            }
            else
            {
                UltraGridColumn colCommission = gridBand.Columns[COL_Commission];
                //colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colCommission.Width = 110;
                colCommission.Header.Caption = "Commission";
                colCommission.Header.VisiblePosition = 5;
                //colCommission.CellActivation = Activation.AllowEdit;
                //colCommission.CellClickAction = CellClickAction.Edit;

                UltraGridColumn colFees = gridBand.Columns[COL_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 60;
                colFees.Header.VisiblePosition = 6;
                colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            UltraGridColumn colVenueID = gridBand.Columns[COL_VenueID];
            colVenueID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colVenueID.Hidden = true;

            UltraGridColumn colTradingAccount = gridBand.Columns[COL_TradingAccount];
            colTradingAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colTradingAccount.Hidden = true;

            UltraGridColumn colTradingAccountID = gridBand.Columns[COL_TradingAccountID];
            colTradingAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colTradingAccountID.Hidden = true;

            UltraGridColumn colGroupState = gridBand.Columns[COL_GroupState];
            colGroupState.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colGroupState.Hidden = true;
     

            UltraGridColumn colUnderlyingID = gridBand.Columns[COL_UnderlyingID];
            colUnderlyingID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUnderlyingID.Hidden = true;


            UltraGridColumn colUnderlying = gridBand.Columns[COL_UnderLying];
            colUnderlying.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUnderlying.Hidden = true;


            UltraGridColumn colUpdated = gridBand.Columns[COL_Updated];
            colUpdated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUpdated.Hidden = true;


            UltraGridColumn colQuantity = gridBand.Columns[COL_Quantity];
            colQuantity.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colQuantity.Hidden = true;


            UltraGridColumn colUserID= gridBand.Columns[COL_UserID];
            colUserID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colUserID.Hidden = true;


            UltraGridColumn colCounterPartyID = gridBand.Columns[COL_CounterPartyID];
            colCounterPartyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colCounterPartyID.Hidden = true;

            UltraGridColumn colAuecID = gridBand.Columns[COL_AUECID];
            colAuecID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAuecID.Hidden = true;


            UltraGridColumn colAllocationType = gridBand.Columns[COL_AllocationType];
            colAllocationType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAllocationType.Hidden = true;

            UltraGridColumn colIsCommissionCalculated = gridBand.Columns[COL_IsCommissionCalculated];
            colIsCommissionCalculated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colIsCommissionCalculated.Hidden = true;

            UltraGridColumn colAllocatedQty = gridBand.Columns[COL_AllocatedQty];
            colAllocatedQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAllocatedQty.Hidden = true;

            UltraGridColumn colBasketGroupID = gridBand.Columns[COL_BasketGroupID];
            colBasketGroupID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colBasketGroupID.Hidden = true;

            UltraGridColumn colListID = gridBand.Columns[COL_ListID];
            colListID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colListID.Hidden = true;

            UltraGridColumn colBaseCurrencyID = gridBand.Columns[COL_BaseCurrencyID];
            colBaseCurrencyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colBaseCurrencyID.Hidden = true;

            //UltraGridColumn colAllocationType = gridBand.Columns[];
            //colUnderlyingID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colUnderlyingID.Hidden = true;



            //UltraGridColumn colCounterPartyName = gridBand.Columns[COL_CounterPartyName];
            //colCounterPartyName.Width = 80;
            //colCounterPartyName.Header.Caption = "Counter Party";
            //colCounterPartyName.Header.VisiblePosition = 7;
            //colCounterPartyName.CellActivation = Activation.NoEdit;

            //UltraGridColumn colVenue = gridBand.Columns[COL_Venue];
            //colVenue.Width = 55;
            //colVenue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //colVenue.Header.VisiblePosition = 7;
            //colVenue.Header.Caption = "Venue";
            //colVenue.CellActivation = Activation.NoEdit;
            ////colVenue.Hidden = true;

            
        }
        /// <summary>
        /// Sets the grid columns.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        private void SetAllocationFundsColumns(UltraGridBand gridBand)
        {
            UltraGridColumn colFundName = gridBand.Columns[COL_AF_FundName];
            colFundName.Width = 45;
            colFundName.Header.Caption = "Fund";
            colFundName.Header.VisiblePosition = 1;
            colFundName.CellActivation = Activation.NoEdit;

            UltraGridColumn colPercentage = gridBand.Columns[COL_AF_Percentage];
            colPercentage.Width = 45;
            colPercentage.Header.Caption = "Percentage";
            colPercentage.Header.VisiblePosition = 2;
            colPercentage.CellActivation = Activation.NoEdit;
            
            UltraGridColumn colAllocatedQty = gridBand.Columns[COL_AF_AllocatedQty];
            colAllocatedQty.Width = 45;
            colAllocatedQty.Header.Caption = "Allocated Qty";
            colAllocatedQty.Header.VisiblePosition = 3;
            colAllocatedQty.CellActivation = Activation.NoEdit;

            if (_commissionCalculationTime.Equals(true))
            {
                UltraGridColumn ColCommission = gridBand.Columns[COL_AF_Commission];
                //ColCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                ColCommission.Width = 110;
                ColCommission.Header.Caption = "Commission";
                ColCommission.Header.VisiblePosition = 4;
                //ColCommission.CellActivation = Activation.AllowEdit;

                UltraGridColumn colFees = gridBand.Columns[COL_AF_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 50;
                colFees.Header.Caption = "Fees";
                colFees.Header.VisiblePosition = 5;
                //colFees.CellActivation = Activation.AllowEdit;
            }
            else
            {
                UltraGridColumn ColCommission = gridBand.Columns[COL_AF_Commission];
                ColCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                ColCommission.Width = 110;
                ColCommission.Header.Caption = "Commission";
                ColCommission.Header.VisiblePosition = 4;
                ColCommission.CellActivation = Activation.NoEdit;

                UltraGridColumn colFees = gridBand.Columns[COL_AF_Fees];
                colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 70;
                colFees.Header.Caption = "Fees";
                colFees.Header.VisiblePosition = 5;
                colFees.CellActivation = Activation.NoEdit;
            }

            

            UltraGridColumn colGroupID = gridBand.Columns[COL_AF_GroupID];
            colGroupID.Header.Caption = "Group ID";
            colGroupID.Hidden = true;

            UltraGridColumn colFundID = gridBand.Columns[COL_AF_FundID];
            colFundID.Header.Caption = "Fund ID";
            colFundID.Hidden = true;

            //UltraGridColumn colParent = gridBand.Columns[COL_AF_Parent];
            //colParent.Header.Caption = "Parent";
            //colParent.Hidden = true;

            
        }
        #endregion


        #region PreAllocated_Tab_Hidden now
        UltraGridBand _gridBandBasketGroupedOrders = null;
        private void grdBasketGroupedOrders_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
           
            //grdBasketGroupedOrders.DisplayLayout.ViewStyleBand = ViewStyleBand.Horizontal;
            //    UltraGridLayout gridLayout = grdBasketGroupedOrders.DisplayLayout;
            //    SetGridAppearanceAndLayout(ref gridLayout);

            //    _gridBandUnGroupedOrders = grdBasketGroupedOrders.DisplayLayout.Bands[0];
            //    SetUnGroupedOrdersColumns(_gridBandUnGroupedOrders);
            if (_basketFundAllocationManager.AllocatedBasketGroups.Count > 0)
            {
                _gridBandBasketGroupedOrders = grdBasketGroupedOrders.DisplayLayout.Bands[0];
                SetBasketGroupedOrdersColumns(_gridBandBasketGroupedOrders);
                grdBasketGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Commission].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdBasketGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Commission].CellAppearance.ForeColor = Color.Black;
                grdBasketGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Fees].CellAppearance.BackColor = Color.DeepSkyBlue;
                grdBasketGroupedOrders.DisplayLayout.Bands[0].Columns[COL_Fees].CellAppearance.ForeColor = Color.Black;

                //grdBasketGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Commission].CellAppearance.BackColor = Color.DeepSkyBlue;
                //grdBasketGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Commission].CellAppearance.ForeColor = Color.Black;
                //grdGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Fees].CellAppearance.BackColor = Color.DeepSkyBlue;
                //grdBasketGroupedOrders.DisplayLayout.Bands[2].Columns[COL_AF_Fees].CellAppearance.ForeColor = Color.Black;

                //_gridBandAllocationFundsBasket = grdBasketGroupedOrders.DisplayLayout.Bands[2];
                //SetAllocationFundsColumns(_gridBandAllocationFundsBasket);
               //grdBasketGroupedOrders.DisplayLayout.Bands["BasketGroupCollection"].Columns[COL_Commission].Formula = "Sum( [" + COL_AF_Commission + "(*)])";
                // this formula has been added for getting sum in parent band from child Band :AM
                //grdBasketGroupedOrders.DisplayLayout.Bands["BasketGroupCollection"].Columns[COL_Commission].Formula = "sum([../AllocationFunds/" + COL_AF_Commission + "] )";
                //grdBasketGroupedOrders.DisplayLayout.Bands["BasketGroupCollection"].Columns[COL_Fees].Formula = "sum([../AllocationFunds/" + COL_AF_Fees + "] )";

                grdBasketGroupedOrders.DisplayLayout.Bands[1].Hidden = true;
                grdBasketGroupedOrders.DisplayLayout.Bands[7].Hidden = true;
                
            }
        }

        private void CommissionAllocation_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CommissionAllocationClosed != null)
                CommissionAllocationClosed(this, EventArgs.Empty);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to close ?", "Confirm Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ctrlCommissionFilters1.RefreshClick -= new EventHandler(ctrlCommissionFilters1_RefreshClick);
                this.FindForm().Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void grdBasketGroupedOrders_DoubleClick(object sender, EventArgs e)
        {
             OrderForm orderForm  = new OrderForm();
             orderForm.CommissionCalculationTime = _commissionCalculationTime;
             orderForm.CurrentUser = _currentUser;
             string BasketGroupID = grdBasketGroupedOrders.ActiveRow.Cells["BasketGroupID"].Value.ToString();
             string ListIDs = "";
            BasketGroup btGroup = _basketFundAllocationManager.AllocatedBasketGroups.GetBasketGroup(BasketGroupID);
             foreach (BasketDetail btDetail in btGroup.AddedBaskets)
             {
                 ListIDs += "," + btDetail.TradedBasketID;
             }
             
             //BasketGroup btGroup = _basketFundAllocationManager.AllocatedBasketGroups.GetBasketGroup(BasketGroupID);
             AllocationOrderCollection orderCollection = _allocationOrderCollection;// getBasketOrders(listID);
          
            if (orderCollection != null)
           {
               orderForm.GetBasketOrders(orderCollection,ListIDs.Substring(1));
               orderForm.ShowDialog();
           }
         
           // AllocationOrderCollection allocationOrderCollection = orderForm.ModifiedAllocationOrderCollection;
           //BasketGroupCollection basketGroupCollection = (BasketGroupCollection)this.grdBasketGroupedOrders.DataSource;
           btGroup.Commission = 0;
           btGroup.Fees = 0;
           foreach (AllocationOrder order in orderCollection )
           {
               if (order.ListID == btGroup.ListID)
               {
                   btGroup.Commission += order.Commission;
                   btGroup.Fees += order.Fees;
               }

           }
           grdBasketGroupedOrders.Refresh();
   
        }
        //OBSOLETE: func:getBasketOrders
        private AllocationOrderCollection getBasketOrders(string listID)
        {
            //AllocationOrderCollection allocationOrderCollection = new AllocationOrderCollection();
            OrderCollection  orderCollection = null;
            BasketGroupCollection basketGroupCollection = _basketFundAllocationManager.AllocatedBasketGroups;
            foreach (BasketGroup basketgroup in basketGroupCollection)
            {
                if (basketgroup.ListID == listID)
                {
                    orderCollection = basketgroup.AddedBaskets[0].BasketOrders;
                    foreach (Order order in orderCollection)
                    {
                        AllocationOrder allocationOrder = OrderAllocationManager.GetAllocationOrder(order);
                        _allocationOrderCollection.Add(allocationOrder);
                        //AllocationFunds orderFunds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, order.CumQty);
                        //AllocateOrder(allocationOrder, allocationOrder.CumQty, orderFunds, false, basketGroup.BasketGroupID);
                    }
                }
            }
            return _allocationOrderCollection;

        }

        ///// <summary>
        ///// Sets the grid appearance and layout.
        ///// </summary>
        ///// <param name="grid">The grid.</param>
        //private void SetGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        //{
        //    gridLayout.Appearance.BackColor = Color.Black;
        //    gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
        //    gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
        //    gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
        //    gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
        //    gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
        //    gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
        //    gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
        //    gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
        //    gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
        //    gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

        //    gridLayout.AutoFitStyle = AutoFitStyle.None;
        //    gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
        //    gridLayout.Override.AllowAddNew = AllowAddNew.Yes;
        //    gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
        //    gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
        //    gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
        //    gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
        //    gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
        //    gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

        //    gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
        //    gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;
        //}

        ///// <summary>
        ///// Sets the grid columns.
        ///// </summary>
        ///// <param name="gridBand">The grid band.</param>
        ///// <param name="isFundData">if set to <c>true</c> [is fund data].</param>
        //private void SetUnGroupedOrdersColumns(UltraGridBand gridBand)
        //{

        //    UltraGridColumn colAllocatedQty = gridBand.Columns[COL_AllocatedQty];
        //    colAllocatedQty.Width = 45;
        //    colAllocatedQty.Header.Caption = "Allocated Qty";
        //    colAllocatedQty.Header.VisiblePosition = 1;

        //    UltraGridColumn colAllocationTypeID = gridBand.Columns[COL_AllocationTypeID];
        //    colAllocationTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTime;
        //    colAllocationTypeID.Width = 140;
        //    colAllocationTypeID.Header.Caption = "AllocationTypeID";
        //    colAllocationTypeID.Header.VisiblePosition = 2;

        //    UltraGridColumn ColAssetID = gridBand.Columns[COL_AssetID];
        //    ColAssetID.Width = 65;
        //    ColAssetID.Header.Caption = "Side";
        //    ColAssetID.Header.VisiblePosition = 3;

        //    UltraGridColumn colAssetName = gridBand.Columns[COL_AssetName];
        //    colAssetName.Width = 50;
        //    colAssetName.Header.Caption = "Asset Name";
        //    colAssetName.Header.VisiblePosition = 4;

        //    UltraGridColumn colAUECID = gridBand.Columns[COL_AUECID];
        //    colAUECID.Width = 65;
        //    colAUECID.Header.Caption = "AUECID";
        //    colAUECID.Header.VisiblePosition = 5;

        //    UltraGridColumn colAvgPrice = gridBand.Columns[COL_AvgPrice];
        //    colAvgPrice.Width = 80;
        //    colAvgPrice.Header.Caption = "Average Price";
        //    colAvgPrice.Header.VisiblePosition = 6;

        //    UltraGridColumn colClOrderID = gridBand.Columns[COL_ClOrderID];
        //    colClOrderID.Width = 50;
        //    colClOrderID.Header.Caption = "ClOrderID";
        //    colClOrderID.Header.VisiblePosition = 7;

        //    UltraGridColumn colCommission = gridBand.Columns[COL_Commission];
        //    colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
        //    colCommission.Width = 50;
        //    colCommission.Header.Caption = "Commission";
        //    colCommission.Header.VisiblePosition = 8;

        //    UltraGridColumn colCounterPartyID = gridBand.Columns[COL_CounterPartyID];
        //    colCounterPartyID.Width = 60;
        //    colCounterPartyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
        //    colCounterPartyID.Header.Caption = "CounterPartyID";
        //    colCounterPartyID.Header.VisiblePosition = 9;

        //    UltraGridColumn colCounterPartyName = gridBand.Columns[COL_CounterPartyName];
        //    colCounterPartyName.Width = 80;
        //    colCounterPartyName.Header.Caption = "CounterParty Name";
        //    colCounterPartyName.Header.VisiblePosition = 10;

        //    UltraGridColumn colCumQty = gridBand.Columns[COL_CumQty];
        //    colCumQty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colCumQty.Hidden = true;
        //    UltraGridColumn colCurrencyID = gridBand.Columns[COL_CurrencyID];
        //    colCurrencyID.Width = 80;
        //    colCurrencyID.Header.Caption = "Currency ID";
        //    colCurrencyID.Header.VisiblePosition = 10;

        //    UltraGridColumn colCurrencyName = gridBand.Columns[COL_CurrencyName];
        //    colCurrencyName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colCurrencyName.Hidden = true;


        //    UltraGridColumn ColExchangeID = gridBand.Columns[COL_ExchangeID];
        //    ColExchangeID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    ColExchangeID.Hidden = true;

        //    UltraGridColumn colExchangeName = gridBand.Columns[COL_ExchangeName];
        //    colExchangeName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colExchangeName.Hidden = true;

        //    UltraGridColumn colFees = gridBand.Columns[COL_Fees];
        //    colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colFees.Hidden = true;

        //    UltraGridColumn colFundID = gridBand.Columns[COL_FundID];
        //    colFundID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colFundID.Hidden = true;

        //    UltraGridColumn colGroupID = gridBand.Columns[COL_GroupID];
        //    colGroupID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colGroupID.Hidden = true;

        //    UltraGridColumn colListID = gridBand.Columns[COL_ListID];
        //    colListID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colListID.Hidden = true;

        //    UltraGridColumn colNotAllExecuted = gridBand.Columns[COL_NotAllExecuted];
        //    colNotAllExecuted.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colNotAllExecuted.Hidden = true;

        //    UltraGridColumn colOpenClose = gridBand.Columns[COL_OpenClose];
        //    colOpenClose.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOpenClose.Hidden = true;



        //    UltraGridColumn colOrderSide = gridBand.Columns[COL_OrderSide];
        //    colOrderSide.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOrderSide.Hidden = true;

        //    UltraGridColumn colOrderSideTagValue = gridBand.Columns[COL_OrderSideTagValue];
        //    colOrderSideTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOrderSideTagValue.Hidden = true;

        //    UltraGridColumn colOrderType = gridBand.Columns[COL_OrderType];
        //    colOrderType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOrderType.Hidden = true;

        //    UltraGridColumn colOrderTypeTagValue = gridBand.Columns[COL_OrderTypeTagValue];
        //    colOrderTypeTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOrderTypeTagValue.Hidden = true;

        //    UltraGridColumn colOrigClOrderID = gridBand.Columns[COL_OrigClOrderID];
        //    colOrigClOrderID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colOrigClOrderID.Hidden = true;

        //    UltraGridColumn colQuantity = gridBand.Columns[COL_Quantity];
        //    colQuantity.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colQuantity.Hidden = true;

        //    UltraGridColumn colStrategyID = gridBand.Columns[COL_StrategyID];
        //    colStrategyID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colStrategyID.Hidden = true;

        //    UltraGridColumn colSymbol = gridBand.Columns[COL_Symbol];
        //    colSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colSymbol.Hidden = true;

        //    UltraGridColumn colTradingAccountID = gridBand.Columns[COL_TradingAccountID];
        //    colTradingAccountID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colTradingAccountID.Hidden = true;

        //    UltraGridColumn colTradingAccountName = gridBand.Columns[COL_TradingAccountName];
        //    colTradingAccountName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colTradingAccountName.Hidden = true;

        //    UltraGridColumn colUnderlyingID = gridBand.Columns[COL_UnderlyingID];
        //    colUnderlyingID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colUnderlyingID.Hidden = true;

        //    UltraGridColumn colUnderlyingName = gridBand.Columns[COL_UnderlyingName];
        //    colUnderlyingName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colUnderlyingName.Hidden = true;

        //    UltraGridColumn colUpdated = gridBand.Columns[COL_Updated];
        //    colUpdated.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colUpdated.Hidden = true;

        //    UltraGridColumn colUserID = gridBand.Columns[COL_UserID];
        //    colUserID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colUserID.Hidden = true;

        //    UltraGridColumn colVenue = gridBand.Columns[COL_Venue];
        //    colVenue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colVenue.Hidden = true;

        //    UltraGridColumn colVenueID = gridBand.Columns[COL_VenueID];
        //    colVenueID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //    colVenueID.Hidden = true;

        //}
        #endregion

        void tabAllocatedOrders_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (this.tabAllocatedOrders.ActiveTab.ToString() == "[1] - BasketGrouped Orders")
            {
                ctrlRecalculate1.Enabled = false;
                ctrlCommissionFilters1.Enabled = false;
            }
            else
            {
                ctrlRecalculate1.Enabled = true;
                ctrlCommissionFilters1.Enabled = true;
            
            }
        }

        void grdGroupedOrders_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals(COL_AllocatedQty).Equals(0))
            {
                e.Cell.Row.Activation = Activation.NoEdit;

                foreach (UltraGridRow row in e.Cell.Row.ChildBands[1].Rows)
                {
                    row.Activation = Activation.NoEdit;
                }

            }
            
            //e.Cell.Row.ChildBands
            //UltraGridBand gridBand = new UltraGridBand();
            //UltraGridColumn col = gridBand.Columns[COL_AllocatedQty];
            //if( Int32.Parse(int.Parse(col.ToString())==0)
            //{
              
            //}
            
        }
    }
}
