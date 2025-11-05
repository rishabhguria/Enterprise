using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Allocation.BLL;
using Prana.BusinessObjects;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
namespace Prana.CommissionRules
{
    public partial class OrderForm : Form
    {
        
        public OrderForm()
        {
            InitializeComponent();

            ctrlCommissionFilters1.RefreshClick += new EventHandler(ctrlCommissionFilters1_RefreshClick);
            ctrlRecalculate1.RecalculateCommission += new EventHandler(ctrlRecalculate1_RecalculateCommission);
            //ctrlCommissionFilters1.SetUp(new CompanyUser());
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

        BasketFundAllocationManager _basketFundAllocationManager = BasketFundAllocationManager.GetInstance;
        void ctrlRecalculate1_RecalculateCommission(object sender, EventArgs e)
        {
            CommissionRule commissionRule = sender as CommissionRule;

            CommissionRuleEvents commissionRuleEvent = (CommissionRuleEvents)e;

            CommissionCalculator commissionCalculator = new CommissionCalculator();


            foreach (UltraGridRow existingRow in this.grdOrders.Rows.GetFilteredInNonGroupByRows())
            {
                AllocationOrder allocationOrder = (AllocationOrder)existingRow.ListObject;
                AllocationFund allocfund = new AllocationFund();
                allocationOrder.AllocatedQty= 0;
                foreach (UltraGridRow row in existingRow.ChildBands[0].Rows)
                {
                    allocfund = (AllocationFund)row.ListObject;
                    allocationOrder.AllocatedQty += allocfund.AllocatedQty;
                    allocfund.FundName = CachedDataManager.GetInstance.GetFundText(allocfund.FundID);

                }
                
                if (commissionRuleEvent.GroupWise == true)
                {
                    //BasketGroup btGroup = _basketFundAllocationManager.AllocatedBasketGroups.GetBasketGroup(allocationOrder.GroupID);
                    commissionCalculator.CalculateCommissionGroupwiseForBasket(commissionRule, allocationOrder);
                    commissionCalculator.CalculateFeesGroupwiseForBasket(commissionRule, allocationOrder);
                    //btGroup.Commission += allocationOrder.Commission;
                    //btGroup.Fees += allocationOrder.Fees;
                    foreach (UltraGridRow row in existingRow.ChildBands[0].Rows)
                    {
                        allocfund = (AllocationFund)row.ListObject;
                        allocfund.ParentBasketGroup = null;
                        allocfund.Commission = 0;
                        allocfund.Fees = 0;
                        allocfund.Commission = (allocationOrder.Commission * allocfund.AllocatedQty) / (allocationOrder.AllocatedQty);
                        allocfund.Fees = (allocationOrder.Fees * allocfund.AllocatedQty) / (allocationOrder.AllocatedQty);
                        allocfund.ParentBasketGroup = (AllocationOrder)existingRow.ListObject;
                    }

                }
                else if (commissionRuleEvent.GroupWise == false && _commissionCalculationTime==true )
                {
                    
                    allocationOrder.Commission = 0;
                    allocationOrder.Fees = 0;
                    foreach (UltraGridRow row in existingRow.ChildBands[0].Rows)
                    {
                     allocfund = (AllocationFund)row.ListObject;
                     allocfund.ParentBasketGroup = null;
                        commissionCalculator.CalculateCommissionFundWiseBasket(commissionRule, allocfund, allocationOrder);
                    commissionCalculator.CalculateFeesFundWiseBasket(commissionRule, allocfund, allocationOrder);
                    if (allocfund.ParentBasketGroup == null)
                    {
                        allocationOrder.Commission += allocfund.Commission;
                        allocationOrder.Fees += allocfund.Fees;
                    }
                     }
                     allocfund.ParentBasketGroup = (AllocationOrder)existingRow.ListObject;
                     
                }
                else if (commissionRuleEvent.GroupWise == false && _commissionCalculationTime == false)
                {

                    allocationOrder.Commission = 0;
                    allocationOrder.Fees = 0;
                    
                    foreach (UltraGridRow row in existingRow.ChildBands[0].Rows)
                    {
                        allocfund = (AllocationFund)row.ListObject;
                        allocfund.Commission = 0.0;
                        allocfund.Fees = 0.0;
                        commissionCalculator.CalculateCommissionFundWiseBasket(commissionRule, allocfund, allocationOrder);
                        commissionCalculator.CalculateFeesFundWiseBasket(commissionRule, allocfund, allocationOrder);
                       
                            allocationOrder.Commission += allocfund.Commission;
                            allocationOrder.Fees += allocfund.Fees;
                           
                       
                    }
                    //allocfund.ParentBasketGroup = (AllocationOrder)existingRow.ListObject;
                }

            }
            this.ModifiedAllocationOrderCollection = (AllocationOrderCollection)grdOrders.DataSource;
            grdOrders.Refresh();
        }
        AllocationOrderCollection _ordercollection = new AllocationOrderCollection();
        CachedDataManager _cachedDataManager = null;
        
        public void GetBasketOrders(AllocationOrderCollection ordercollection,string listIDs)
        {
            _cachedDataManager = CachedDataManager.GetInstance;
            foreach (AllocationOrder allocationOrder  in ordercollection)
            {
                //allocationOrder.AssetName = _cachedDataManager.GetAssetText(allocationOrder.AssetID);
                //allocationOrder.UnderlyingName = _cachedDataManager.GetUnderLyingText(allocationOrder.UnderlyingID);
                //allocationOrder.ExchangeID = _cachedDataManager.GetExchangeIdFromAUECId(allocationOrder.AUECID);
                //allocationOrder.ExchangeName = _cachedDataManager.GetExchangeText(allocationOrder.ExchangeID);
                string[] list = listIDs.Split(',');
                foreach (String s in list)
                {
                    if (allocationOrder.ListID == s)
                       _ordercollection.Add(allocationOrder);
                }
               
                
            }
            //_ordercollection = ordercollection;
        }

       

        private void OrderForm_Load(object sender, EventArgs e)
        {
           
            if (_ordercollection.Count > 0)
            {
                _gridBandOrders = grdOrders.DisplayLayout.Bands[0];
                
                //run time column has been added in band
                //_gridBandOrders.Columns.Add("Commission");
                //_gridBandOrders.Columns.Add("Fees");
                //grdOrders.DataSource = null;
                grdOrders.DataSource = _ordercollection;
                grdOrders.DataBind();
              
                

          
            }
        }
        UltraGridBand _gridBandOrders = null;
        UltraGridBand _gridBandFunds = null;

        private void grdOrders_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            _gridBandFunds = grdOrders.DisplayLayout.Bands[1];
            HideGridColumnProperties(_gridBandOrders);
            SetColumnFormats(_gridBandOrders);
            SetAllocationFundsColumns(_gridBandFunds);
            //grdOrders.DisplayLayout.Bands["AllocationOrderCollection"].Columns["Commission"].Formula = "sum([../../AllocationFunds/" + COL_AF_Commission + "] )";
            //grdOrders.DisplayLayout.Bands["AllocationOrderCollection"].Columns["Fees"].Formula = "sum([../AllocationFunds/" + COL_AF_Fees + "] )";
        
           
        }
        const string COL_AF_FundName = "FundName";
        const string COL_AF_FundID = "FundID";
        const string COL_AF_Percentage = "Percentage";
        const string COL_AF_AllocatedQty = "AllocatedQty";
        const string COL_AF_GroupID = "GroupID";
        const string COL_AF_Commission = "Commission";
        const string COL_AF_Fees = "Fees";
        const string COL_AF_Parent = "Parent";
        private bool  _commissionCalculationTime ;
        private const string FORMAT_COST = "{0:#,##,###.##}";

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
                ColCommission.CellActivation = Activation.AllowEdit;
                ColCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                
                //ColCommission.Format = string.Format(FORMAT_COST, ColCommission);
            

                UltraGridColumn colFees = gridBand.Columns[COL_AF_Fees];
                //colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                colFees.Width = 50;
                colFees.Header.Caption = "Fees";
                colFees.Header.VisiblePosition = 5;
                colFees.CellActivation = Activation.AllowEdit;
                colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
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
        private void HideGridColumnProperties(UltraGridBand band)
        {
            try
            {
                  for (int i = 0; i < band.Columns.Count; i++)
                    {
                        band.Columns[i].Hidden = true;
                    }
                }
                 
                           
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
       
        private void SetColumnFormats(UltraGridBand band)
        {

          
            band.Columns["Quantity"].Header.VisiblePosition=4;
            band.Columns["Quantity"].Hidden = false;
            band.Columns["AVGPRICE"].Header.VisiblePosition = 3;
            band.Columns["AVGPRICE"].Hidden = false;
            band.Columns["AVGPRICE"].Header.Caption = "AvgPX";
            band.Columns["AssetName"].Header.VisiblePosition = 7;
            band.Columns["AssetName"].Hidden = false;
            band.Columns["AssetName"].Header.Caption = "Asset";
            band.Columns["CounterPartyName"].Header.VisiblePosition = 6;
            band.Columns["CounterPartyName"].Hidden = false;
            band.Columns["ExchangeName"].Header.VisiblePosition = 9;
            band.Columns["ExchangeName"].Hidden = false;
            //band.Columns["FUND"].Header.VisiblePosition = 5;
            //band.Columns["FUND"].Hidden = false;
            band.Columns["OrderSide"].Header.VisiblePosition = 2;
            band.Columns["OrderSide"].Hidden = false;
            band.Columns["OrderSide"].Header.Caption = "Side";
            band.Columns["SYMBOL"].Header.VisiblePosition = 1;
            band.Columns["SYMBOL"].Hidden = false;
            band.Columns["UnderlyingName"].Header.VisiblePosition = 8;
            band.Columns["UnderlyingName"].Hidden = false;
            band.Columns["UnderlyingName"].Header.Caption = "UnderLying";
            band.Columns["VENUE"].Header.VisiblePosition = 10;
            band.Columns["VENUE"].Hidden = false;
            band.Columns["Commission"].Hidden = false;
            band.Columns["Commission"].Header.VisiblePosition = 11;
            band.Columns["Fees"].Header.VisiblePosition = 12;
            band.Columns["Fees"].Hidden = false;

            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn col in band.Columns.All)
            {

                col.Width = 65;
                col.AutoSizeMode = ColumnAutoSizeMode.Default;
                col.CellAppearance.TextHAlign = HAlign.Right;
                col.CellActivation = Activation.NoEdit;
            }
            if (_commissionCalculationTime.Equals(true))
            {
                band.Columns["Commission"].CellActivation = Activation.NoEdit;
                band.Columns["Fees"].CellActivation = Activation.NoEdit;
            }
            else 
            {
                band.Columns["Commission"].CellActivation = Activation.AllowEdit;
                band.Columns["Fees"].CellActivation = Activation.AllowEdit;
            }
           
          
       
           
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            AllocationOrderCollection allocationOrderCollection = (AllocationOrderCollection)grdOrders.DataSource;
            AllocationOrderCollection allocationOrderModified = new AllocationOrderCollection();
            foreach (AllocationOrder allocationOrder in allocationOrderCollection)
            {
                if (allocationOrder.IsCommissionCalculated == false)
                {
                    allocationOrderModified.Add(allocationOrder);
                }
            }
            if (allocationOrderModified.Count > 0)
            {
                BasketAllocationDBManager.SaveAndUpdateCommissionandFeesForBasket(allocationOrderModified);
                OrderAllocationDBManager.SaveCommissionAndFeesForstrategy();
                MessageBox.Show("Commission and Fees Saved Successfully for Baskets");
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to close ?", "Confirm Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ctrlCommissionFilters1.RefreshClick -= new EventHandler(ctrlCommissionFilters1_RefreshClick);
                this.FindForm().Close();
            }
        }

        private void ctrlCommissionFilters1_RefreshClick(object sender, EventArgs e)
        {
            //((Prana.CommissionRules.CtrlCommissionFilters)sender).CmbAsset.Value;
            UltraGridBand band = grdOrders.DisplayLayout.Bands[0];
            RowsCollection rows = grdOrders.DisplayLayout.Rows;
            //Clear all filters
            foreach (UltraGridBand var in grdOrders.DisplayLayout.Bands)
            {
                band.ColumnFilters.ClearAllFilters();

            }

            if (ctrlCommissionFilters1.CmbAsset.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbAsset.Value.ToString()) != int.MinValue)
                {
                    grdOrders.DisplayLayout.Bands[0].ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbAsset.Value);
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
                        band.ColumnFilters["OrderSide"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbSide.Text);
                        break;
                    default:

                        break;
                }
            }
            //if (ctrlCommissionFilters1.CmbSide.Value != null)
            //{

            //    if (int.Parse(ctrlCommissionFilters1.CmbSide.Value.ToString()) != int.MinValue)
            //    {
            //        grdOrders.DisplayLayout.Bands[0].ColumnFilters["OrderSideTagValue"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbSide.Value);
            //    }
            //}
            if (ctrlCommissionFilters1.CmbUnderLying.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbUnderLying.Value.ToString()) != int.MinValue)
                {
                    grdOrders.DisplayLayout.Bands[0].ColumnFilters["UnderlyingID"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbUnderLying.Value);
                }
            }
            if (ctrlCommissionFilters1.CmbVenue.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbVenue.Value.ToString()) != int.MinValue)
                {
                    grdOrders.DisplayLayout.Bands[0].ColumnFilters["VENUE"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbVenue.Value);
                }
            }
            if (ctrlCommissionFilters1.CmbCounterParty.Value != null)
            {
                if (int.Parse(ctrlCommissionFilters1.CmbCounterParty.Value.ToString()) != int.MinValue)
                {
                    grdOrders.DisplayLayout.Bands[0].ColumnFilters["CounterPartyName"].FilterConditions.Add(FilterComparisionOperator.Equals, ctrlCommissionFilters1.CmbCounterParty.Value);
                }
            }



            if (ctrlCommissionFilters1.TextBoxSymbol.Text != string.Empty)
            {
                string[] txtSymbol = ctrlCommissionFilters1.TextBoxSymbol.Text.Split(',');

                grdOrders.DisplayLayout.Bands[0].ColumnFilters["SYMBOL"].LogicalOperator = FilterLogicalOperator.Or;
                foreach (string s in txtSymbol)
                {
                    grdOrders.DisplayLayout.Bands[0].ColumnFilters["SYMBOL"].FilterConditions.Add(FilterComparisionOperator.Equals, s);
                }
            }

        }

        private AllocationOrderCollection _allocationOrderCollection = new AllocationOrderCollection();
        public AllocationOrderCollection ModifiedAllocationOrderCollection
        {
            get
            {
                return _allocationOrderCollection;
            }
            set
            {
                _allocationOrderCollection=value ;
            }
        }
        public bool CommissionCalculationTime
        {
            get
            {
                return _commissionCalculationTime;
            }
            set
            {
                _commissionCalculationTime=value;
            }
        }

    }
}