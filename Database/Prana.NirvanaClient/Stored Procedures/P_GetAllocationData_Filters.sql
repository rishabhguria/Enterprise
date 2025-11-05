Create Proc P_GetAllocationData_Filters                
(                      
@FromDate datetime,                      
@ToDate Datetime,    
@IsGetUnallocatedGroups bit,    
@FilterString nvarchar(max),
@SymbolFilterQuery nvarchar(max),
@IsSkipStateIDFilter BIT = 0                 
)                   
AS                  
 
Create Table #filters_Splited 
(
FilterName nvarchar(100),
FilterValue nvarchar(MAX)
)
Create Table #FundIDs (CompanyFundID int)
Create Table #StrategyIDs (CompanyStrategyID int)
Create Table #OrderSideTagValues (SideID varchar(5))
Create Table #CounterPartyIDs (CPID int)
Create Table #TradingAccountIDs (TAID int)
Create Table #VenueIDs (VID int)
Create Table #CurrencyIDs (CID int)
Create Table #ExchangeIDs (EID int)
Create Table #AssetIDs (AID int)
Create Table #UnderlyingIDs (UID int)
Create Table #IsPreAllocated (PreAllocationValue int)
Create Table #IsManualGroup (ManualGroup int)
Create Table #GroupIDs (GrpId nvarchar(max))

SELECT * into #filters from dbo.split(@FilterString,'~')

IF(@FilterString <> '')
BEGIN
Insert into #filters_Splited
SELECT SUBSTRING(items,1,CHARINDEX(':',items)-1) as FilterName,
SUBSTRING(items,CHARINDEX(':',items)+1, len(items)) as FilterValue 
from #filters
END

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='GroupID') <>'')    
begin    
Insert into #GroupIDs     
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='GroupID'),',')    
end     

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='AccountID') <>'')
begin
Insert into #FundIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='AccountID'),',')
end
ELSE
begin
INSERT INTO #FundIDs
SELECT CompanyFundID from T_CompanyFunds Where IsActive=1
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='StrategyID') <>'')
begin
Insert into #StrategyIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='StrategyID'),',')
end
ELSE
begin
INSERT INTO #StrategyIDs
SELECT CompanyStrategyID from T_CompanyStrategy
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='OrderSideTagValue') <>'')
begin
Insert into #OrderSideTagValues 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='OrderSideTagValue'),',')
end
ELSE
begin
INSERT INTO #OrderSideTagValues
SELECT SideTagValue from T_Side
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='BrokerID') <>'')
begin
Insert into #CounterPartyIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='BrokerID'),',')
end
ELSE
begin
INSERT INTO #CounterPartyIDs
SELECT CounterPartyID from T_CounterParty
union ALL
select 0
union ALL
SELECT '-2147483648'
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='TradingAccountID') <>'')
begin
Insert into #TradingAccountIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='TradingAccountID'),',')
end
ELSE
begin
INSERT INTO #TradingAccountIDs
SELECT CompanyTradingAccountsID from T_CompanyTradingAccounts
union ALL
select 0
union ALL
SELECT '-2147483648'
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='VenueID') <>'')
begin
Insert into #VenueIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='VenueID'),',')
end
ELSE
begin
INSERT INTO #VenueIDs
SELECT VenueID from T_Venue
union ALL
select 0
union ALL
SELECT '-2147483648'
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='CurrencyID') <>'')
begin
Insert into #CurrencyIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='CurrencyID'),',')
end
ELSE
begin
INSERT INTO #CurrencyIDs
SELECT CurrencyID from T_Currency
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='ExchangeID') <>'')
begin
Insert into #ExchangeIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='ExchangeID'),',')
end
ELSE
begin
INSERT INTO #ExchangeIDs
SELECT ExchangeID from T_Exchange
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='AssetID') <>'')
begin
Insert into #AssetIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='AssetID'),',')
end
ELSE
begin
INSERT INTO #AssetIDs
SELECT AssetID from T_Asset
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='UnderlyingID') <>'')
begin
Insert into #UnderlyingIDs 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='UnderlyingID'),',')
end
ELSE
begin
INSERT INTO #UnderlyingIDs
SELECT UnderLyingID from T_UnderLying
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='IsPreAllocated') <>'')
begin
Insert into #IsPreAllocated 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='IsPreAllocated'),',')
end
ELSE
begin
INSERT INTO #IsPreAllocated
SELECT 0
union all 
SELECT 1
end

if((Select top 1 (filtervalue) from #filters_Splited where filtername ='IsManualGroup') <>'')
begin
Insert into #IsManualGroup 
SELECT * from dbo.Split((Select (filtervalue) from #filters_Splited where filtername ='IsManualGroup'),',')
end
ELSE
begin
INSERT INTO #IsManualGroup
SELECT 0
union all 
SELECT 1
end

CREATE TABLE #T_Groups(
	[GroupRefID] [bigint] NOT NULL,
	[GroupID] [varchar](50) NOT NULL,
	[OrderSideTagValue] [varchar](3) NOT NULL,
	[Symbol] [varchar](100) NOT NULL,
	[OrderTypeTagValue] [varchar](3) NOT NULL,
	[CounterPartyID] [int] NOT NULL,
	[VenueID] [int] NOT NULL,
	[TradingAccountID] [int] NOT NULL,
	[AUECID] [int] NOT NULL,
	[CumQty] [float] NOT NULL,
	[AllocatedQty] [float] NOT NULL,
	[Quantity] [float] NOT NULL,
	[AvgPrice] [float] NOT NULL,
	[IsPreAllocated] [bit] NOT NULL,
	[ListID] [varchar](50) NOT NULL,
	[UserID] [int] NOT NULL,
	[ISProrataActive] [bit] NOT NULL,
	[AutoGrouped] [bit] NOT NULL,
	[StateID] [int] NOT NULL,
	[IsBasketGroup] [bit] NULL,
	[BasketGroupID] [varchar](50) NULL,
	[IsManualGroup] [bit] NOT NULL,
	[AllocationDate] [datetime] NOT NULL,
	[SettlementDate] [datetime] NULL,
	[AssetID] [int] NULL,
	[UnderLyingID] [int] NULL,
	[ExchangeID] [int] NULL,
	[CurrencyID] [int] NULL,
	[Description] [varchar](max) NULL,
	[AUECLocalDate] [datetime] NOT NULL,
	[IsSwapped] [bit] NULL,
	[FXRate] [float] NULL,
	[FXConversionMethodOperator] [varchar](3) NULL,
	[TaxlotClosingID_Fk] [uniqueidentifier] NULL,
	[Commission] [float] NOT NULL,
	[OtherBrokerFees] [float] NOT NULL,
	[StampDuty] [float] NOT NULL,
	[TransactionLevy] [float] NOT NULL,
	[ClearingFee] [float] NOT NULL,
	[TaxOnCommissions] [float] NOT NULL,
	[MiscFees] [float] NOT NULL,
	[AccruedInterest] [float] NULL,
	[ProcessDate] [datetime] NOT NULL,
	[OriginalPurchaseDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsModified] [bit] NULL,
	[AllocationSchemeID] [int] NOT NULL,
	[CommissionSource] [int] NOT NULL,
	[TradeAttribute1] [varchar](200) NULL,
	[TradeAttribute2] [varchar](200) NULL,
	[TradeAttribute3] [varchar](200) NULL,
	[TradeAttribute4] [varchar](200) NULL,
	[TradeAttribute5] [varchar](200) NULL,
	[TradeAttribute6] [varchar](200) NULL,
	[TaxlotIdsWithAttributes] [nvarchar](500) NULL,
	[TransactionType] [varchar](50) NULL,
	[SecFee] [float] NOT NULL,
	[OccFee] [float] NOT NULL,
	[OrfFee] [float] NOT NULL,
	[ClearingBrokerFee] [float] NOT NULL,
	[SoftCommission] [float] NOT NULL,
	[NotionalChange] [float] NOT NULL,
	[NirvanaProcessDate] [datetime] NULL,
	[TransactionSource] [int] NULL,
	[InternalComments] [varchar](max) NULL,
	[SettlCurrency] [int] NULL,	
	[OptionPremiumAdjustment] [float] NULL,
	[ChangeType] [int] NOT NULL,
	[IsCommissionChanged] [bit] NOT NULL,
	[IsSoftCommissionChanged] [bit] NOT NULL,
	[OriginalAllocationPreferenceID] [int] NULL,
	[BorrowerID] [varchar](50) NULL,
	[BorrowBroker] [varchar](50) NULL,
	[ShortRebate] Float Null, 
	[AdditionalTradeAttributes] [varchar](max) NULL,
	[SideID] varchar(5) NULL,
	[CPID] [int] NULL,
	[TAID] [int] NULL,
	[VID] [int] NULL,
	[CID] [int] NULL,
	[EID] [int] NULL,
	[AID] [int] NULL,
	[UID] [int] NULL,
	[PreAllocationValue] [int] NULL,
	[ManualGroup] [int] NULL,
) 

DECLARE @SelectQuery nvarchar(max)
DECLARE @ParmDefinition nvarchar(500);
 
SET @SelectQuery = ' 
insert into #T_Groups
select *
From T_group G   
Inner JOIN #OrderSideTagValues OS ON G.OrderSideTagValue = OS.SideID
Inner JOIN #CounterPartyIDs CP ON G.CounterPartyID = CP.CPID
INNER JOIN #TradingAccountIDs TA ON G.TradingAccountID = TA.TAID
INNER JOIN #VenueIDs V ON G.VenueID = V.VID
INNER JOIN #CurrencyIDs C ON G.CurrencyID = C.CID
INNER JOIN #ExchangeIDs E ON G.ExchangeID = E.EID
INNER JOIN #AssetIDs A ON G.AssetID = A.AID
INNER JOIN #UnderlyingIDs U ON G.UnderLyingID = U.UID
INNER JOIN #IsPreAllocated IPA ON G.IsPreAllocated = IPA.PreAllocationValue
INNER JOIN #IsManualGroup IMG ON G.IsManualGroup = IMG.ManualGroup'

if EXISTS(SELECT 1 from #GroupIDs)
BEGIN
SET @SelectQuery = @SelectQuery + ' INNER JOIN #GroupIDs GRP ON G.GroupID = GRP.GrpId'
ALTER TABLE #T_Groups
ADD GrpId varchar(100)
END

IF (@IsSkipStateIDFilter = 1)
    BEGIN
        SET @SelectQuery += N'
    WHERE
      (DATEDIFF(d, G.AllocationDate, @FromDateCopy) <= 0 AND
       DATEDIFF(d, G.AllocationDate, @ToDateCopy) >= 0 AND
       G.CumQty > 0 AND @IsGetUnallocatedGroupsCopy = 0)
    OR
      (DATEDIFF(d, G.AllocationDate, @ToDateCopy) >= 0 AND
       @IsGetUnallocatedGroupsCopy = 1)';
    END
    ELSE
    BEGIN
        SET @SelectQuery += N'
    WHERE
      (DATEDIFF(d, G.AllocationDate, @FromDateCopy) <= 0 AND
       DATEDIFF(d, G.AllocationDate, @ToDateCopy) >= 0 AND
       G.CumQty > 0 AND @IsGetUnallocatedGroupsCopy = 0)
    OR
      (DATEDIFF(d, G.AllocationDate, @ToDateCopy) >= 0 AND
       G.CumQty > 0 AND G.StateID = 1 AND @IsGetUnallocatedGroupsCopy = 1)';
    END

SET @ParmDefinition = N'@FromDateCopy DATETIME, @ToDateCopy DATETIME, @IsGetUnallocatedGroupsCopy BIT, @IsSkipStateIDFilter BIT';

EXECUTE sys.sp_executesql @SelectQuery,@ParmDefinition,@FromDateCopy = @FromDate,@ToDateCopy = @ToDate, @IsGetUnallocatedGroupsCopy = @IsGetUnallocatedGroups, @IsSkipStateIDFilter = @IsSkipStateIDFilter;
               
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
G.[BorrowerID],
G.[BorrowBroker],
G.[ShortRebate],  
          
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
S.SwapPK ,-- possibly not using          
CASE
    WHEN U.GroupID IS NULL THEN 0
    ELSE 1
END AS IsManuallyModified,
G.AdditionalTradeAttributes,
L2.AdditionalTradeAttributes AS L2AdditionalTradeAttributes,    
O.AdditionalTradeAttributes AS OAdditionalTradeAttributes 
Into #AllocationGroups          
from #T_Groups G  
left outer join T_FundAllocation L1 on G.GroupID=L1.GroupID          
left outer join T_Level2Allocation L2 on L1.AllocationId=L2.Level1AllocationID           
left outer join T_TradedOrders O on G.GroupID=O.GroupID          
left outer join T_ImportFileLog I on O.ImportFileID=I.ImportFileID          
left outer join T_SwapParameters S on G.GroupID=S.GroupID  
left outer join T_UngroupedAllocationGroups U on G.GroupID=U.GroupID     
Where  (L1.FundID in (SELECT CompanyFundID FROM #FundIDs) OR G.StateID =1 ) 
and (L2.Level2ID in (SELECT CompanyStrategyID FROM #StrategyIDs) OR G.StateID =1 )
          
SELECT GroupID, Count(*) as Count           
Into #GroupCounts          
From #AllocationGroups          
Group By GroupID          
 
IF(@SymbolFilterQuery <> '') 
BEGIN

declare @query nvarchar(MAX)  
Set @query = 'SELECT * from #AllocationGroups G Inner JOIn #GroupCounts GC ON G.GroupID = GC.GroupID Where GC.Count = 1 ' + @SymbolFilterQuery  
EXECUTE(@query) 

Set @query = 'SELECT * from #AllocationGroups G Inner JOIn #GroupCounts GC ON G.GroupID = GC.GroupID Where GC.Count > 1 ' + @SymbolFilterQuery  
EXECUTE(@query)

END
ELSE
BEGIN
   
SELECT * from #AllocationGroups AG          
Inner JOIn #GroupCounts GC          
ON AG.GroupID = GC.GroupID          
Where GC.Count = 1 

SELECT * from #AllocationGroups AG          
Inner JOIn #GroupCounts GC          
ON AG.GroupID = GC.GroupID          
Where GC.Count > 1  

END          
    
Drop Table #T_Groups  
Drop TABLE #GroupCounts 
Drop TABLE #AllocationGroups  
DROP TABLE #FundIDs 
DROP TABLE #StrategyIDs 
DROP TABLE #OrderSideTagValues 
DROP TABLE #CounterPartyIDs 
DROP TABLE #TradingAccountIDs 
DROP TABLE #VenueIDs 
DROP TABLE #CurrencyIDs 
DROP TABLE #ExchangeIDs 
DROP TABLE #AssetIDs 
DROP TABLE #UnderlyingIDs 
DROP TABLE #IsPreAllocated 
DROP TABLE #IsManualGroup 
DROP TABLE #GroupIDs