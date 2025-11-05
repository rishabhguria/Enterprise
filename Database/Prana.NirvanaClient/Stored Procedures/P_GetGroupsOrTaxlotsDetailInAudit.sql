CREATE Procedure [dbo].[P_GetGroupsOrTaxlotsDetailInAudit]
(    
@GroupId varchar(100),    
@TaxlotId varchar(100)   
--@AuditID int    
)    
as    
begin    
set @TaxlotId=NULLIF(@TaxlotId,'')    
    
IF @TaxlotId is null    
begin    
Create Table #Groups(    
 --[AuditId] [int] NULL,    
 [GroupID] [varchar](50) NULL,    
 [OrderSideTagValue] [varchar](3) NULL,    
 [Symbol] [varchar](100) NULL,    
 [OrderTypeTagValue] [varchar](3) NULL,    
 [CounterPartyID] [int] NULL,    
 [VenueID] [int] NULL,    
 [TradingAccountID] [int] NULL,    
 [AUECID] [int] NULL,    
 [CumQty] [float] NULL,    
 [AllocatedQty] [float] NULL,    
 [Quantity] [float] NULL,    
 [AvgPrice] [float] NULL,    
 [IsPreAllocated] [bit] NULL,    
 [ListID] [varchar](50) NULL,    
 [UserID] [int] NULL,    
 [ISProrataActive] [bit] NULL,    
 [AutoGrouped] [bit] NULL,    
 [StateID] [int] NULL,    
 [IsBasketGroup] [bit] NULL,    
 [BasketGroupID] [varchar](50) NULL,    
 [IsManualGroup] [bit] NOT NULL DEFAULT ((0)),    
 [AllocationDate] [datetime] NULL,    
 [SettlementDate] [datetime] NULL,    
 [AssetID] [int] NULL,    
 [UnderLyingID] [int] NULL,    
 [ExchangeID] [int] NULL,    
 [CurrencyID] [int] NULL,    
 [Description] [varchar](max) NULL,    
 [AUECLocalDate] [datetime] NULL,    
 [IsSwapped] [bit] NULL,    
 [FXRate] [float] NULL,    
 [FXConversionMethodOperator] [varchar](3) NULL,    
 [TaxlotClosingID_Fk] [uniqueidentifier] NULL,    
 [Commission] [float] NOT NULL DEFAULT ((0)),    
 [OtherBrokerFees] [float] NOT NULL DEFAULT ((0)),    
 [StampDuty] [float] NOT NULL DEFAULT ((0)),    
 [TransactionLevy] [float] NOT NULL DEFAULT ((0)),    
 [ClearingFee] [float] NOT NULL DEFAULT ((0)),    
 [TaxOnCommissions] [float] NOT NULL DEFAULT ((0)),    
 [MiscFees] [float] NOT NULL DEFAULT ((0)),    
 [AccruedInterest] [float] DEFAULT ((0)),    
 [ProcessDate] [datetime] NOT NULL DEFAULT (((1)/(1))/(1800)),    
 [OriginalPurchaseDate] [datetime] NOT NULL DEFAULT (((1)/(1))/(1800)),    
 [ModifiedBy] [int] NULL,    
 [ModifiedDate] [datetime] NULL,    
 [IsModified] [bit] NULL,    
 [AllocationSchemeID] [int] NOT NULL DEFAULT ((0)),    
 [CommissionSource] [int] NOT NULL DEFAULT ('1'),    
 [TradeAttribute1] [varchar](200) NULL,    
 [TradeAttribute2] [varchar](200) NULL,    
 [TradeAttribute3] [varchar](200) NULL,    
 [TradeAttribute4] [varchar](200) NULL,    
 [TradeAttribute5] [varchar](200) NULL,    
 [TradeAttribute6] [varchar](200) NULL,    
 [TaxlotIdsWithAttributes] [nvarchar](500) NULL,    
 [SecFee] [float] NOT NULL DEFAULT ((0)),    
 [OccFee] [float] NOT NULL DEFAULT ((0)),    
 [OrfFee] [float] NOT NULL DEFAULT ((0)),    
 [ClearingBrokerFee] [float] NOT NULL DEFAULT ((0)),    
 [SoftCommission] [float] NOT NULL DEFAULT ((0)),
 [TransactionType] [varchar](200) NULL, 
 [InternalComments] [varchar](500) Null,    
 [OptionPremiumAdjustment] [float] NOT NULL DEFAULT ((0)),
 [AdditionalTradeAttributes] [varchar](MAX) NULL
)    
    
insert into #Groups select GroupID, OrderSideTagValue, Symbol, OrderTypeTagValue, CounterPartyID, VenueID, TradingAccountID, AUECID,    
CumQty,AllocatedQty, Quantity,AvgPrice,IsPreAllocated,ListID,UserID,ISProrataActive,AutoGrouped,StateID,IsBasketGroup,BasketGroupID,IsManualGroup,AllocationDate,    
SettlementDate,AssetID,UnderLyingID,ExchangeID,CurrencyID, Description, AUECLocalDate, IsSwapped, FXRate, FXConversionMethodOperator,    
 TaxlotClosingID_Fk,Commission,OtherBrokerFees,StampDuty,TransactionLevy,ClearingFee,TaxOnCommissions,MiscFees,AccruedInterest,    
ProcessDate,OriginalPurchaseDate,ModifiedUserID,ModifiedDate,IsModified,AllocationSchemeID,CommissionSource,TradeAttribute1,TradeAttribute2,    
TradeAttribute3,TradeAttribute4,TradeAttribute5,TradeAttribute6,TaxlotIdsWithAttributes,SecFee,OccFee,OrfFee,ClearingBrokerFee,SoftCommission,
TransactionType, InternalComments,OptionPremiumAdjustment, AdditionalTradeAttributes from T_Group where GroupID =@GroupId    
    
insert into #Groups select GroupID, OrderSideTagValue, Symbol, OrderTypeTagValue, CounterPartyID, VenueID, TradingAccountID, AUECID,    
CumQty,AllocatedQty, Quantity,AvgPrice,IsPreAllocated,ListID,UserID,ISProrataActive,AutoGrouped,StateID,IsBasketGroup,BasketGroupID,IsManualGroup,AllocationDate,    
SettlementDate,AssetID,UnderLyingID,ExchangeID,CurrencyID, Description, AUECLocalDate, IsSwapped, FXRate, FXConversionMethodOperator,    
 TaxlotClosingID_Fk,Commission,OtherBrokerFees,StampDuty,TransactionLevy,ClearingFee,TaxOnCommissions,MiscFees,AccruedInterest,    
ProcessDate,OriginalPurchaseDate,ModifiedBy,ModifiedDate,IsModified,AllocationSchemeID,CommissionSource,TradeAttribute1,TradeAttribute2,    
TradeAttribute3,TradeAttribute4,TradeAttribute5,TradeAttribute6,TaxlotIdsWithAttributes,SecFee,OccFee,OrfFee,ClearingBrokerFee,SoftCommission,
TransactionType, InternalComments,OptionPremiumAdjustment, AdditionalTradeAttributes from T_Group_DeletedAudit where GroupID =@GroupId
    
Select * from #Groups    
Drop table #Groups    
end    
else    
begin    
create table #Taxlots(    
 --[AuditId] [int] NULL,    
 [TaxLotID] [varchar](50) NULL,    
 [Symbol] [varchar](100) NULL,    
 [TaxLotOpenQty] [float] NULL,    
 [AvgPrice] [float] NULL,    
 [TimeOfSaveUTC] [datetime] NULL,    
 [GroupID] [nvarchar](50) NULL,    
 [AUECModifiedDate] [datetime] NULL,    
 [FundID] [int] NULL,    
 [Level2ID] [int] NULL,    
 [OpenTotalCommissionandFees] [float] NULL,    
 [ClosedTotalCommissionandFees] [float] NULL,    
 [PositionTag] [int] NULL,    
 [OrderSideTagValue] [nchar](10) NULL,    
 [TaxLotClosingId] [uniqueidentifier] NULL,    
 [ParentRow_Pk] [bigint] NULL,    
 [AccruedInterest] [float] NULL DEFAULT ((0)),    
 [FXRate] [float] NULL,    
 [FXConversionMethodOperator] [varchar](3) NULL,    
 [ExternalTransId] [varchar](100) NULL,    
 [TradeAttribute1] [varchar](200) NULL,    
 [TradeAttribute2] [varchar](200) NULL,    
 [TradeAttribute3] [varchar](200) NULL,    
 [TradeAttribute4] [varchar](200) NULL,    
 [TradeAttribute5] [varchar](200) NULL,    
 [TradeAttribute6] [varchar](200) NULL,    
 [LotId] [varchar](200) NULL,
 [AdditionalTradeAttributes] [varchar](max) NULL
)    
    
insert into #Taxlots select TaxLotID,Symbol, TaxLotOpenQty, AvgPrice, TimeOfSaveUTC, GroupID,    
AUECModifiedDate,FundID,Level2ID,OpenTotalCommissionandFees,ClosedTotalCommissionandFees,PositionTag,    
OrderSideTagValue,TaxLotClosingId_Fk,ParentRow_Pk,AccruedInterest,FXRate,FXConversionMethodOperator,    
ExternalTransId,TradeAttribute1,TradeAttribute2,TradeAttribute3,TradeAttribute4,TradeAttribute5,    
TradeAttribute6,LotId, AdditionalTradeAttributes from PM_Taxlots where TaxLot_PK in     
(SELECT Max(Taxlot_Pk) from PM_Taxlots where PM_Taxlots.GroupID = @GroupId and PM_Taxlots.TaxLotID=@TaxlotId)    
    
insert into #Taxlots select TaxLotID,Symbol, TaxLotOpenQty, AvgPrice, TimeOfSaveUTC, GroupID,    
AUECModifiedDate,FundID,Level2ID,OpenTotalCommissionandFees,ClosedTotalCommissionandFees,PositionTag,    
OrderSideTagValue,TaxLotClosingId,ParentRow_Pk,AccruedInterest,FXRate,FXConversionMethodOperator,    
ExternalTransId,TradeAttribute1,TradeAttribute2,TradeAttribute3,TradeAttribute4,TradeAttribute5,    
TradeAttribute6,LotId, AdditionalTradeAttributes from PM_Taxlots_DeletedAudit where PM_Taxlots_DeletedAudit.GroupID=@GroupId    
and PM_Taxlots_DeletedAudit.TaxLotID=@TaxlotId   
    
select * from #Taxlots    
    
drop table #Taxlots    
    
end    
end    
