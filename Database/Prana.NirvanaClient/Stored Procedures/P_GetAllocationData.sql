Create Proc P_GetAllocationData                
(                      
@FromDate datetime,                      
@ToDate Datetime,    
@IsGetUnallocatedGroups bit,    
@FilterString nvarchar(max),
@SymbolFilterQuery nvarchar(max),
@IsSkipStateIDFilter BIT = 0
)                   
AS                  
 
if(@FilterString = '' AND @SymbolFilterQuery = '')  
BEGIN 

Select * into #T_Group     
From T_group     
where (DATEDIFF(d,AllocationDate,@FromDate) <=0           
and DATEDIFF(d,AllocationDate,@ToDate)>=0                  
and CumQty>0 AND @IsGetUnallocatedGroups = 0) OR (DATEDIFF(d,AllocationDate,@ToDate)>=0                  
and CumQty>0 and StateID = 1 AND @IsGetUnallocatedGroups = 1)      
               
Select          
G.GroupID,          
G.OrderSideTagValue,           
G.Symbol,           
G.OrderTypeTagValue,          
G.CounterPartyID,          
G.TradingAccountID,          
G.VenueID,          
G.AUECID,          
G.CumQty,          
G.AllocatedQty ,          
G.Quantity,          
G.AvgPrice,          
G.IsPreAllocated,          
G.ListID,          
G.UserID,          
G.ISProrataActive,          
G.AutoGrouped,          
G.IsManualGroup,          
G.StateID,          
G.AllocationDate,          
G.SettlementDate,          
G.AssetID,          
G.underlyingID,          
G.ExchangeID,          
G.CurrencyID,          
G.Description,          
G.AUECLocalDate,          
G.IsSwapped,          
G.FXRate,          
G.FXConversionMethodOperator,          
G.TaxlotClosingID_Fk,          
G.Commission ,          
G.SoftCommission ,          
G.OtherBrokerFees ,          
G.ClearingBrokerFee ,          
G.StampDuty ,          
G.TransactionLevy ,          
G.ClearingFee ,          
G.TaxOnCommissions ,          
G.MiscFees ,          
G.SecFee ,          
G.OccFee ,          
G.OrfFee ,          
G.AccruedInterest ,          
G.ProcessDate ,          
G.OriginalPurchaseDate ,          
G.CommissionSource ,          
G.IsModified ,          
G.AllocationSchemeID,          
G.TaxLotIdsWithAttributes,          
G.TradeAttribute1 ,           
G.TradeAttribute2,          
G.TradeAttribute3,          
G.TradeAttribute4,          
G.TradeAttribute5,          
G.TradeAttribute6,          
G.TransactionType,          
G.NirvanaProcessDate,          
G.InternalComments,          
G.SettlCurrency,          
G.OptionPremiumAdjustment,          
G.ChangeType,          
G.IsCommissionChanged,          
G.IsSoftCommissionChanged,        
G.TransactionSource,          
G.[OriginalAllocationPreferenceID],
G.BorrowerID,
G.BorrowBroker,
G.ShortRebate, 
          
L1.AllocatedQty AS L1AllocatedQty,           
L1.FundID AS L1FundID,          
L1.Percentage AS L1Percentage,          
L1.AllocationId AS L1AllocationId,          
          
L2.TaxLotQty,          
L2.Level2ID,          
L2.Level2Percentage,          
L2.Level1AllocationID,          
L2.TaxLotID,          
L2.Commission as L2Commission,          
L2.SoftCommission as L2SoftCommission,          
L2.OtherBrokerFees AS L2OtherBrokerFees,          
L2.ClearingBrokerFee AS L2ClearingBrokerFee,          
L2.StampDuty AS L2StampDuty,          
L2.TransactionLevy AS L2TransactionLevy,          
L2.ClearingFee AS L2ClearingFee,          
L2.TaxOnCommissions AS L2TaxOnCommissions,          
L2.MiscFees AS L2MiscFees,          
L2.SecFee AS L2SecFee,          
L2.OccFee AS L2OccFee,          
L2.OrfFee AS L2OrfFee,          
L2.TaxLotState AS L2TaxLotState,          
L2.AccruedInterest AS L2AccruedInterest,          
L2.FXRate AS L2FXRate,          
L2.FXConversionMethodOperator AS L2FXConversionMethodOperator,          
ExternalTransId,          
LotId,          
L2.TradeAttribute1 AS L2TradeAttribute1,          
L2.TradeAttribute2 AS L2TradeAttribute2,          
L2.TradeAttribute3 AS L2TradeAttribute3,          
L2.TradeAttribute4 AS L2TradeAttribute4,          
L2.TradeAttribute5 AS L2TradeAttribute5,          
L2.TradeAttribute6 AS L2TradeAttribute6,          
          
O.ParentClOrderID,          
O.ClorderID,          
O.CumQty AS OCumQty,          
O.Quantity AS OQuantity,          
O.AvgPrice AS OAvgPrice,          
O.NirvanaMsgType,           
O.FXRate OFXRate,          
O.FundID AS OFundID,          
O.StrategyID AS OStrategyID,          
O.OriginalPurchaseDate AS OOriginalPurchaseDate,          
O.ProcessDate AS OProcessDate,          
O.AUECLocalDate AS OAUECLocalDate,          
O.IsModified AS OIsModified,          
O.OrderSideTagValue AS OOrderSideTagValue,          
O.VenueID AS OVenueID,          
O.CounterPartyID AS OCounterPartyID,          
O.TradingAccountID AS OTradingAccountID,          
O.SettlementDate AS OSettlementDate,          
O.FXConversionMethodOperator AS OFXConversionMethodOperator,          
O.MultiTradeName,          
ISNULL(O.ImportFileID,0) AS ImportFileID,          
O.UserID AS OUserID,          
O.TradeAttribute1 AS OTradeAttribute1,          
O.TradeAttribute2 AS OTradeAttribute2,          
O.TradeAttribute3 AS OTradeAttribute3,          
O.TradeAttribute4 AS OTradeAttribute4,          
O.TradeAttribute5 AS OTradeAttribute5,          
O.TradeAttribute6 AS OTradeAttribute6,          
O.InternalComments AS OInternalComments,          
ISNULL(I.ImportFileName,'') AS ImportFileName,          
O.Text,          
O.SettlCurrency AS OSettlCurrency,          
O.TransactionSource AS OTransactionSource,        
ISNULL(O.[OriginalAllocationPreferenceID],0) as OOriginalAllocationPreferenceID,      
         
-- Swaps Related Fields          
S.NotionalValue ,          
S.DayCount ,          
S.BenchMarkRate ,          
S.Differential ,          
S.FirstResetDate ,          
S.ResetFrequency ,          
S.OrigTransDate ,          
S.OrigCostBasis ,          
S.SwapDescription ,          
S.ClosingPrice,          
S.ClosingDate,          
S.TransDate ,          
S.SwapPK , -- possibly not using          
CASE
    WHEN U.GroupID IS NULL THEN 0
    ELSE 1
END AS IsManuallyModified,
G.AdditionalTradeAttributes,
L2.AdditionalTradeAttributes AS L2AdditionalTradeAttributes,    
O.AdditionalTradeAttributes AS OAdditionalTradeAttributes 
Into #AllocationGroups          
from #T_Group G  left outer join T_FundAllocation L1 on G.GroupID=L1.GroupID          
left outer join T_Level2Allocation L2 on L1.AllocationId=L2.Level1AllocationID           
left outer join T_TradedOrders O on G.GroupID=O.GroupID          
left outer join T_ImportFileLog I on O.ImportFileID=I.ImportFileID          
left outer join T_SwapParameters S on G.GroupID=S.GroupID       
left outer join T_UngroupedAllocationGroups U on G.GroupID=U.GroupID       
          
SELECT GroupID, Count(*) as Count           
Into #GroupCounts          
From #AllocationGroups          
Group By GroupID          
          
SELECT * from #AllocationGroups AG          
Inner JOIn #GroupCounts GC          
ON AG.GroupID = GC.GroupID          
Where GC.Count = 1          
          
SELECT * from #AllocationGroups AG          
Inner JOIn #GroupCounts GC          
ON AG.GroupID = GC.GroupID          
Where GC.Count > 1     
    
Drop Table #T_Group

END
ELSE
BEGIN
EXEC P_GetAllocationData_filters @FromDate,@ToDate,@IsGetUnallocatedGroups,@FilterString, @SymbolFilterQuery, @IsSkipStateIDFilter
END