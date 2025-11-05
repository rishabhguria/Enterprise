using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;

namespace Prana.AllocationNew
{
    public class AllocationConstants
    {
        /// <summary>
        /// Enum for storing allocation grid name
        /// </summary>
        public enum AllocationGrid
        {
            grdAllocated,
            grdUnallocated
        }

        /// <summary>
        /// Type of filters available
        /// </summary>
        public enum FilterScope
        {
            All,
            Allocated,
            UnAllocated,
            Split,
            None
        }

        /// <summary>
        /// Enum for the actions which are passed to SaveDataAsync so that the save can be handled on a different thread everytime
        /// </summary>
        public enum ActionAfterSavingData
        {
            DoNothing,
            GetData,
            ClearData,
            CancelEditChanges,
            CloseAllocation,
            ChangeTab
        }

        /// <summary>
        /// While doing unallocation, either it is successfully completed or there could be issues. Issues could be due to...
        /// 1) Problem in unallocating the trades
        /// 2) Problem in writing the file. Generally before unallocation, we find out what trades are already closed/CA applied 
        ///     and we don't unallocate unless these are reverted. We just write that info in the file for further viewing.
        /// </summary>
        public enum UnAllocationCompletionStatus
        {
            /// <summary>
            /// All of the groupids unallocated successfully.
            /// </summary>
            Success,
            /// <summary>
            /// Some error occured while unallocating trades
            /// </summary>
            UnallocationError,
            /// <summary>
            /// Some/All of the groupids were not unallocated due to closed/ca applied etc
            /// </summary>
            FileWriteError
        }

        #region Allocation Columns

        public static List<string> UnAllocatedDisplayColumns
        {
            get
            {
                List<string> _unAllocatedDisplayColumns = new List<string>();
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_AUECLOCALDATE);
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_ORDER_SIDE);
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_EXECUTED_QTY);
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_SYMBOL);
                //_unAllocatedDisplayColumns.Add(OrderFields.CAPTION_AVGPRICE);
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_COUNTERPARTY_NAME);
                    _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_USER);
                _unAllocatedDisplayColumns.Add(OrderFields.CAPTION_ASSETCATEGORY);


                return _unAllocatedDisplayColumns;
            }
        }
        public static List<string> AllocatedDisplayColumns
        {
            get
            {
                List<string> _allocatedDisplayColumns = new List<string>();
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_AUECLOCALDATE);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_ORDER_SIDE);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_SYMBOL);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_EXECUTED_QTY);
               // _allocatedDisplayColumns.Add(OrderFields.CAPTION_AVGPRICE);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_COUNTERPARTY_NAME);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_TOTALCOMMISSIONANDFEES);
                    _allocatedDisplayColumns.Add(OrderFields.CAPTION_USER);
                _allocatedDisplayColumns.Add(OrderFields.CAPTION_ASSETCATEGORY);

                   
                return _allocatedDisplayColumns;
            }
        }
        public static List<string> AllocatedReportColumns
        {
            get
            {
                List<string>  _allocationReportColumns = new List<string>();
                    _allocationReportColumns = new List<string>();
                    _allocationReportColumns.Add(OrderFields.CAPTION_ORDER_SIDE);
                    _allocationReportColumns.Add(OrderFields.CAPTION_SYMBOL);
                    _allocationReportColumns.Add(OrderFields.CAPTION_COUNTERPARTY_NAME);
                    _allocationReportColumns.Add(OrderFields.CAPTION_VENUE);
                    _allocationReportColumns.Add(OrderFields.CAPTION_Level2Name);
                    _allocationReportColumns.Add(OrderFields.CAPTION_Level1Name);
                    _allocationReportColumns.Add(OrderFields.CAPTION_EXECUTED_QTY);
                    _allocationReportColumns.Add(OrderFields.CAPTION_AVGPRICE);
                    _allocationReportColumns.Add(OrderFields.CAPTION_TOTALCOMMISSIONANDFEES);
                    _allocationReportColumns.Add(OrderFields.CAPTION_COMPANYNAME);
                    _allocationReportColumns.Add(OrderFields.CAPTION_ASSET_NAME);
                    _allocationReportColumns.Add(OrderFields.CAPTION_AUECLOCALDATE);
                    _allocationReportColumns.Add(OrderFields.CAPTION_QUANTITY);
                       
                return _allocationReportColumns;
            }
        }
        #endregion

        #region Grouped_Order_Columns

        public const string CAPTION_OrderSideTagValue = "OrderSideTagValue";
        public const string COL_OrderSide = "OrderSide";
        public const string CAPTION_Side = "Side";
        public const string COL_OrderType = "OrderType";
        public const string CAPTION_OrderTypeTagValue = "OrderTypeTagValue";
        public const string CAPTION_Symbol = "Symbol";
        public const string COL_Venue = "Venue";
        public const string CAPTION_Quantity = "Quantity";
        public const string CAPTION_Asset = "Asset";
        public const string COL_AvgPrice = "AvgPrice";
        public const string CAPTION_AssetID = "AssetID";
        public const string CAPTION_AssetName = "AssetName";
        public const string CAPTION_UnderlyingID = "UnderlyingID";
        public const string COL_UnderlyingName = "UnderlyingName";
        public const string CAPTION_UnderLying = "Underlying";

        public const string CAPTION_ExchangeID = "ExchangeID";
        public const string CAPTION_ExchangeName = "ExchangeName";
        public const string CAPTION_CurrencyID = "CurrencyID";
        public const string CAPTION_CurrencyName = "CurrencyName";
        public const string CAPTION_AUECID = "AUECID";
        public const string CAPTION_TradingAccountID = "TradingAccountID";
        public const string CAPTION_TradingAccountName = "TradingAccountName";
        public const string CAPTION_CompanyUserName = "CompanyUserName";
        public const string CAPTION_CounterPartyID = "CounterPartyID";
        public const string COL_CounterPartyName = "CounterPartyName";
        public const string CAPTION_COUNTERPARTY = "Broker";
        public const string CAPTION_VenueID = "VenueID";
        public const string COL_CumQty = "CumQty";
        public const string COL_AllocatedQty = "AllocatedQty";
        public const string COL_COMPANYNAME = "CompanyName";

        public const string CAPTION_TradeAttribute1 = "Trade Attribute 1";
        public const string CAPTION_TradeAttribute2 = "Trade Attribute 2";
        public const string CAPTION_TradeAttribute3 = "Trade Attribute 3";
        public const string CAPTION_TradeAttribute4 = "Trade Attribute 4";
        public const string CAPTION_TradeAttribute5 = "Trade Attribute 5";
        public const string CAPTION_TradeAttribute6 = "Trade Attribute 6";

        public const string COL_TradeAttribute1 = "TradeAttribute1";
        public const string COL_TradeAttribute2 = "TradeAttribute2";
        public const string COL_TradeAttribute3 = "TradeAttribute3";
        public const string COL_TradeAttribute4 = "TradeAttribute4";
        public const string COL_TradeAttribute5 = "TradeAttribute5";
        public const string COL_TradeAttribute6 = "TradeAttribute6";

        public const string CAPTION_Updated = "Updated";
        public const string CAPTION_NotAllExecuted = "NotAllExecuted";
        public const string CAPTION_GroupID = "GroupID";

        public const string CAPTION_OtherFees = "OtherFees";
        public const string COL_Level1Name = "Level1Name";
        public const string CAPTION_COMMISSIONSOURCE = "Commission Source";
        public const string COL_COMMISSIONSOURCE = "CommissionSource";
        public const string CAPTION_SOFTCOMMISSIONSOURCE = "Soft Commission Source";
        public const string COL_SOFTCOMMISSIONSOURCE = "SoftCommissionSource";

        public const string CAPTION_AutoGrouped = "AutoGrouped";
        public const string CAPTION_AllocatedEqualTotalQty = "AllocatedEqualTotalQty";
        public const string CAPTION_StateID = "StateID";
        public const string CAPTION_AllocationLevelList = "AllocationLevelList";
        public const string CAPTION_IsCommissionCalculated = "IsCommissionCalculated";
        public const string CAPTION_ProcessDate = "Process Date";
        public const string CAPTION_OriginalPurchaseDate = "Original Purchase Date";
        public const string CAPTION_AUECLocalDate = "Trade Date";

        public const string COl_AUECLocalDate = "AUECLocalDate";
        public const string COl_ProcessDate = "ProcessDate";
        public const string COl_OriginalPurchaseDate = "OriginalPurchaseDate";
        public const string CAPTION_AF_TaxLotID = "TaxLotID";
        public const string CAPTION_Allocations = "Allocations";
        public const string CAPTION_TaxlotQty = "TaxLotQty";
        public const string CAPTION_Percentage = "Percentage";
        public const string CAPTION_CreationDate = "CreationDate";
        public const string COL_SettlementDate = "SettlementDate";
        public const string CAPTION_ExpirationDate = "ExpirationDate";
        public const string CAPTION_CommissionText = "CommissionText";
        public const string CAPTION_OrderID = "ClOrderID";
        public const string COL_FXRate = "FXRate";
        public const string COL_FXConversionMethodOperator = "FXConversionMethodOperator";
        public const string CAPTION_EXECUTED_QTY = "Executed Qty";
        public const string CAPTION_AVERAGEPRICE = "Average Price";
        public const string COL_ExternalTransId = "ExternalTransId";
        public const string COL_LotID = "LotId";
        public const string CAPTION_ExternalTransId = "External Transaction Id";
        //Added by Rahul on 20120207
        public const string COL_TOTALCOMMISSIONANDFEES = "TotalCommissionandFees";
        public const string COL_DESCRIPTION = "Description";
        public const string COL_DELTA = "Delta";
        public const string COL_M2MPROFITLOSS = "M2MProfitLoss";
        public const string COL_ACCRUEDINTEREST = "AccruedInterest";
        public const string COL_SWAPPARAMETERS = "SwapParameters";
        public const string COL_PRANAMSGTYPE = "PranaMsgType";
        public const string CAP_PARENT = "Parent";

        //Added by Rahul on 6,Sep'2012
        public const string COL_SEDOLSYMBOL = "SedolSymbol";
        public const string COL_BLOOMBERGSYMBOL = "BloombergSymbol";
        public const string COL_CUSIPSYMBOL = "CusipSymbol";
        public const string COL_ISINSYMBOL = "IsinSymbol";
        public const string COL_IDCOSYMBOL = "IDCOSymbol";
        public const string COL_OSISYMBOL = "OSISymbol";

        public const string CAP_SEDOLSYMBOL = "Sedol Symbol";
        public const string CAP_BLOOMBERGSYMBOL = "Bloomberg Symbol";
        public const string CAP_CUSIPSYMBOL = "Cusip Symbol";
        public const string CAP_ISINSYMBOL = "ISIN Symbol";
        public const string CAP_IDCOSYMBOL = "IDCO Symbol";
        public const string CAP_OSISYMBOL = "OSI Symbol";

        //change comment added for audit trail other columns already present new constants created
        public const string COL_CHANGECOMMENT = "ChangeComment";
        public const string COL_UNDERLYINGDELTA = "UnderlyingDelta";
        public const string COL_COMMISSIONAMOUNT = "CommissionAmt";
        public const string COL_COMMISSIONRATE = "CommissionRate";

        public const string COL_SOFTCOMMISSIONAMOUNT = "SoftCommissionAmt";
        public const string COL_SOFTCOMMISSIONRATE = "SoftCommissionRate";
        public const string COL_TransactionType = "TransactionType";

        public const string CAPTION_ClosingStatus = "Closing Status";
        public const string CAPTION_TransactionType = "Transaction Type";

        public const string COL_ClosingStatus = "ClosingStatus";

        public const string COL_COMMISSIONPERSHARE = "CommissionPerShare";
        public const string CAP_COMMISSIONPERSHARE = "Commission/Share";

        public const string COL_INTERNALCOMMENTS = "InternalComments";
        public const string CAP_INTERNALCOMMENTS = "Internal Comments";

        public List<string> TradeStringFields = new List<string>();

        #endregion
    }
}
