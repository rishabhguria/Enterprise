
/*                                                                      
Author: Sandeep Singh                                                                      
Date: Dec 05,2012                                                                      
Desc: Customized EOD file for SS - AXYS                                                                      
EXEC:                                                                      
[P_GetAXYSEOD_SS_Strategy] 27,'1184,1183,1182,1186,1185','2013-09-27',5,'1,15,12',0,0,27                                                                     
*/                                                                      
                                                                      
CREATE Procedure [dbo].[P_GetAXYSEOD_SS_Strategy]                                                                      
(                                                                      
 @ThirdPartyID int,                                                                                                                                                                      
 @CompanyFundIDs varchar(max),                                                                                                                                                                      
 @InputDate datetime,                                                                                                                                                                  
 @CompanyID int,                                                                                                                                  
 @AuecIDs varchar(max),                                                                        
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                       
 @DateType int ,          
 @fileFormatID int                 
)                                                                        
As             
            
--Declare @ThirdPartyID int                                                                                                                                                                      
--Declare @CompanyFundIDs varchar(max)                                                
--Declare @InputDate datetime                                                
--Declare @CompanyID int                                                
--Declare @AuecIDs varchar(max)                                                
--Declare @TypeID int            
--Declare @DateType int             
--            
--Set @ThirdPartyID =  27            
--Set @CompanyFundIDs = '1184,1183,1182,1186,1185'            
--Set @InputDate = '2013-09-04'            
--Set @CompanyID = 5            
--Set @AuecIDs =   N'1,15,12'                                      
--Set @TypeID = 0             
--Set @DateType = 0            
            
Declare @ThirdPartyID_LocalVar int                                                                                                                                                                      
Declare @CompanyFundIDs_LocalVar varchar(max)                                                
Declare @InputDate_LocalVar datetime                                                
Declare @CompanyID_LocalVar int                                                
Declare @AuecIDs_LocalVar varchar(max)                                                
Declare @TypeID_LocalVar int            
Declare @DateType_LocalVar int            
            
Set @ThirdPartyID_LocalVar = @ThirdPartyID                                                                 
Set @CompanyFundIDs_LocalVar = @CompanyFundIDs                                                
Set @InputDate_LocalVar = @InputDate                                                
Set @CompanyID_LocalVar = @CompanyID                                                
Set @AuecIDs_LocalVar =@AuecIDs                                           
Set @TypeID_LocalVar = @TypeID             
Set @DateType_LocalVar = @DateType                                            
                
Declare @Fund Table                  
(                                            
FundID int                                         
)                                                                               
                                                                              
Declare @AUECID Table                                                                                                                                
(                                                                                                                                
AUECID int                                                                                                                            
)                                                               
                                                        
Insert into @Fund                                                                              
Select Cast(Items as int) from dbo.Split(@CompanyFundIDs_LocalVar,',')                                       
                                                                              
Insert into @AUECID                                                                              
Select Cast(Items as int) from dbo.Split(@AuecIDs_LocalVar,',')                                                   
                      
Declare @BaseCurrencyID int                                             
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                          
                                                                    
Create Table #FXConversionRates                                                    
(                                                                                                   
 FromCurrencyID int,                                                   
 ToCurrencyID int,                                                                       
 RateValue float,                                                                                                       
 ConversionMethod int,                                                                                                                    
 Date DateTime,                                                                                                
 eSignalSymbol varchar(max)                                                                                       
)                                                                       
                                                                    
-- get Security Master Data in a Temp Table                                                                                                                                                        
Create Table #SecMasterDataTempTable                
 (                                                                                                                                                                                                                                                           
  AUECID int,                                                                                               
  TickerSymbol Varchar(100),                                                         
  CompanyName  VarChar(500),                                                                                                                                                                         
  AssetID int,                                                                                                                                              
  AssetName Varchar(100),                                                        
  SecurityTypeName Varchar(200),                          
  SectorName Varchar(100),                                                                                                      
  SubSectorName Varchar(100),                                                  
  CountryName  Varchar(100),                                                  
  PutOrCall Varchar(5),                                                    
  Multiplier Float,                                                                                                          
  LeadCurrencyID int,                                                                                                                                   
  VsCurrencyID int,                                                                                                                                                    
  CurrencyID int,                                                                                                                                    
  UnderlyingSymbol Varchar(100),                                                                                                
  BloombergSymbol Varchar(200),                                                                                                
  SedolSymbol Varchar(100),                         
  ISINSymbol Varchar(100),                                                                                               
  CusipSymbol Varchar(100),                                            
  OSISymbol Varchar(100),                                                                                                
  IDCOSymbol Varchar(100)                                                                                                     
)                                                                                                                       
                                                     
Insert Into #SecMasterDataTempTable                                                                                                    
Select                                                                                       
 AUECID ,                                                                                                  
 TickerSymbol ,                                                                                      
 CompanyName  ,                                                                         
 AssetID,                                                                                                       
 AssetName,                                                                                 
 SecurityTypeName,                                                                                                                                       
 SectorName ,                                                                                            
 SubSectorName ,   
 CountryName ,                                                                                                                                                                                                 
 PutOrCall ,                                                                                              
 Multiplier ,                                                                                                                       
 LeadCurrencyID ,                                                                                                                                                                            
 VsCurrencyID,                                                                                                                                                    
 CurrencyID,             
 UnderlyingSymbol,                                                                                                
 BloombergSymbol,                                                                                                
 SedolSymbol,                                                                            
 ISINSymbol,                                                                                                
 CusipSymbol,                                                                            
 OSISymbol,                      
 IDCOSymbol                                                                        
 From V_SecMasterData             
            
--Insert data into temp table and then perform operations                          
--select required fields from t_group                          
Select                           
GroupID,                          
OrderSideTagValue,                          
AUECID,                          
AUECLocalDate                          
into #T_Group                          
from T_Group                          
                          
--select required fields from pm_taxlots                          
Select                           
TaxlotID,                          
TaxLotClosingId_Fk,                          
LotId,                          
GroupID,                          
FundID,                  
OrderSideTagValue,            
ExternalTransId                          
into #PM_Taxlots                          
from PM_Taxlots                          
                          
--select required fields from pm_taxlotclosing                          
Select                          
TaxLotClosingId,                          
PositionalTaxlotID,                          
ClosingTaxlotID,                          
ClosedQty,                          
AUECLocalDate                          
into #PM_TaxlotClosing                          
from PM_TaxlotClosing                          
                        
--select required fields from level2allocation                        
Select                          
TaxlotId,                        
LotId                         
into #T_Level2Allocation                          
from T_Level2Allocation             
            
Select *                          
into #V_Taxlots                       
from V_Taxlots             
            
/* Collect closing information            
   To update closing trade strategy on the basis of Opening trade            
*/            
CREATE TABLE #ClosingTradeInfoTable                                        
(                                        
TaxLotID varchar(50),                
Symbol varchar(50),                                      
Quantity float,                
StrategyID int,                
PositionalTaxlotID Varchar(50),                
ClosingTaxlotID Varchar(50)                                       
)               
            
-- collect all opening and closing trade information            
Insert Into #ClosingTradeInfoTable              
Select              
VTL.TaxLotID,              
VTL.Symbol AS Symbol,              
IsNull(#PM_TaxlotClosing.ClosedQty,VTL.TaxLotQty) as Quantity,              
VTL.Level2ID as StrategyID,              
#PM_TaxlotClosing.PositionalTaxlotID,              
#PM_TaxlotClosing.ClosingTaxlotID              
From #V_Taxlots VTL              
Left Outer Join #PM_TaxlotClosing On #PM_TaxlotClosing.ClosingTaxLotID = VTL.TaxLotID                        
            
-- Update Closing trade strategy from Opening trade            
Update ClosingTrade                
Set                 
ClosingTrade.StrategyID = OpeningTrade.StrategyID                          
From #ClosingTradeInfoTable ClosingTrade              
Inner Join #ClosingTradeInfoTable OpeningTrade On OpeningTrade.TaxLotID = ClosingTrade.PositionalTaxLotID             
            
/* Grouping of same taxlot if closed with more than one trade of same strategy            
   Example: One sell of 1000 qty is closed with two buy trades of 500 each and same strategy, so sell trade will have only one row             
*/            
Select                 
TaxlotID,                
Max(Symbol) As Symbol,                
Sum(Quantity) As Quantity,                
StrategyID                
Into #TaxlotIDAndStrategyIDGroupingTable                
From #ClosingTradeInfoTable                
Group By TaxlotID, StrategyID             
            
            
--Select *          
--Into #T_TaxlotAndExternalTransactionIDMapping            
--From T_TaxlotAndExternalTransactionIDMapping            
            
/* Split External Transaction ID */            
Create Table #Temp_TaxlotAndExternalTransIDTable            
(            
TaxlotID Varchar(50),            
ExternalTransID Varchar(200),            
OrderSideTagValue Varchar(10)            
)            
            
/* Insert from PM_Taxlots */            
Insert InTo #Temp_TaxlotAndExternalTransIDTable            
Select             
Distinct            
TaxlotID,            
ExternalTransID,            
OrderSideTagValue            
From #PM_Taxlots            
            
/* Insert from T_DeletedTaxlots */            
Insert InTo #Temp_TaxlotAndExternalTransIDTable            
Select             
Distinct            
TaxlotID,            
ExternalTransID,            
OrderSideTagValue            
From T_DeletedTaxlots            
Where TaxlotState = 3  And FileFormatID = @fileFormatID          
            
Create Table #Temp_SplittedWithComma            
(            
SplitValue varchar(500),            
TaxlotID varchar(50),            
OrderSideTagValue Varchar(10)            
)             
            
;WITH CTE(TaxlotID,ExternalTransID,OrderSideTagValue) AS             
(            
   Select TaxlotID,ExternalTransID,OrderSideTagValue from #Temp_TaxlotAndExternalTransIDTable            
)            
Insert Into #Temp_SplittedWithComma            
select             
  b.items as SplitValue            
, TaxlotID            
,OrderSideTagValue            
            
from CTE a            
cross apply dbo.Split(a.ExternalTransID,',') b             
Order by TaxlotID            
            
Select            
TaxlotID,            
Case             
 When Len(SplitValue) > 0 And CharIndex(':', SplitValue) > 0          
 Then Substring(SplitValue,(CharIndex(':', SplitValue) + 1),Len(SplitValue))           
 When Len(SplitValue) > 0  And CharIndex(':', SplitValue) <= 0          
 Then SplitValue             
 Else ''             
End As ExternalTransID,            
Case             
 When Len(SplitValue) > 0 And CharIndex(':', SplitValue) > 0          
 Then Substring(SplitValue,1,(CharIndex(':', SplitValue)-1))           
 When Len(SplitValue) > 0 And CharIndex(':', SplitValue) <= 0          
 Then 0               
 Else 0             
End As StrategyID,           
Side            
Into #Temp_ExternalTransIDSplittedTable            
 From #Temp_SplittedWithComma            
 Inner Join T_Side On #Temp_SplittedWithComma.OrderSideTagValue = T_Side.SideTagValue            
            
SELECT                    
TaxlotID As OriginalTaxlotID,           
ExternalTransID,          
StrategyID,          
Side,                   
Ranking = DENSE_RANK() OVER(PARTITION BY TaxlotID,Side ORDER BY TaxlotID, StrategyID ASC)            
InTo #Temp_ExternalTransIDSplittedTable_Ranking                  
FROM #Temp_ExternalTransIDSplittedTable          
Order by TaxlotID, StrategyID          
          
/* Split External Transaction ID */            
            
-- Temp Tables used here to do all the operations            
Create Table #TempTransaction_BeforeStrategy                                                                      
(                                        
TaxlotID Varchar(200),                                                                                           
ClosingTaxlotID Varchar(200),                                                                      
PortfolioCode Varchar(200),--COL1                               
TranCode Varchar(20),                                                                      
Comment Varchar(2000),                                                                      
AssetName Varchar(20),                                                                 
TickerSymbol Varchar(200),                                                                      
TradeDate Varchar(20),                                                                      
SettleDate Varchar(20),                                                                      
OriginalCostDate Varchar(20),                                                                      
Quantity Float,           
CloseMath Varchar(10),--10                                                                      
VersusDate Varchar(20),                                                                      
SourceType Varchar(20),                                                                      
SourceSymbol Varchar(20),                                                                      
TradeDateFXRate Varchar(20),                                                                      
SettleDateFXRate Varchar(20),                                                                      
OriginalFXRate Varchar(20),                                                                      
MarkToMarket Varchar(10),                                                                      
TradeAmount Float,                                                                      
OriginalCost Varchar(200),                                                                      
Comment1 Varchar(10),--COL20                                        
WithholdingTax Varchar(10),                                                                      
Exchange Varchar(200),                                                                      
ExchangeFee Float,                                                                      
commission Float,                                                                      
Broker Varchar(200),                                                                      
ImpliedComm Varchar(10),                              
OtherFees Float,                                                                      
CommPurpose Varchar(10),                                                                      
Pledge Varchar(10),--29                                                                      
LotLocation Varchar(10),--COL30                                                                      
DestPledge Varchar(10),                                               
DestLotLocation Varchar(10),                                                                      
OriginalFace Varchar(10),                                                                      
YieldOnCost Varchar(10),                                                                      
DurationOnCost Varchar(10),--COL35                                                            
UserDef1 Varchar(10),                                                                      
UserDef2 Varchar(10),                                                                      
UserDef3 Varchar(10),                                        
TranID Varchar(100),                                                                      
IPCounter Varchar(10),                               
Repl Varchar(10),                                                             
Source Varchar(10),                                                                      
Comment2 Varchar(10),                                   
OmniAcct Varchar(10),                                                                      
Recon Varchar(10),                                                                      
Post Varchar(10),                                                                      
LabelName Varchar(10),                                                                      
LabelDefinition Varchar(10),   
LabelDefinition_Date Varchar(10),                                                                      
LabelDefinition_String Varchar(10),--COL50                                                                      
Comment3 Varchar(10),                                                                      
RecordDate Varchar(10),                                                                    
ReclaimAmount Varchar(10),                                
Strategy Varchar(10),                                                                      
Comment4 Varchar(10),                                                                      
IncomeAccount Varchar(10),                                                                  
AccrualAccount Varchar(10),                                                                      
DivAccrualMethod Varchar(10),                                                                      
PerfContributionOrWithdrawal Varchar(10),--59                                                                      
--System internal use only                                              
CustomOrderBy Int,                                                
Level1AllocationID Varchar(200),                                                
TaxLotStateID Int,                                              
FromDeleted Varchar(50),                        
InternalTranCode varchar(20) Null,            
FundID Int,            
ExchangeID Int,            
PutOrCall Varchar(10)            
)            
            
Create Table #TempTransaction                                                                      
(                                        
TaxlotID Varchar(200),                                                                                           
ClosingTaxlotID Varchar(200),              
PortfolioCode Varchar(200),                               
TranCode Varchar(20),                                                                      
Comment Varchar(2000),                                                                      
AssetName Varchar(20),                                                                   
TickerSymbol Varchar(200),                                                                      
TradeDate Varchar(20),                                                                      
SettleDate Varchar(20),                                                                      
OriginalCostDate Varchar(20),                                                                      
Quantity Float,                                                                      
CloseMath Varchar(10),--10                                                                      
VersusDate Varchar(20),                                                                      
SourceType Varchar(20),                                                                      
SourceSymbol Varchar(20),                                                                      
TradeDateFXRate Varchar(20),                                                                      
SettleDateFXRate Varchar(20),                                                                      
OriginalFXRate Varchar(20),                                                                      
MarkToMarket Varchar(10),                                                                      
TradeAmount Varchar(500),                                                                      
OriginalCost Varchar(200),                                                                      
Comment1 Varchar(10),--COL20                               
WithholdingTax Varchar(10),                                                                      
Exchange Varchar(200),                                                                      
ExchangeFee Varchar(200),                                                                      
commission Varchar(200),            
Broker Varchar(200),                                                                      
ImpliedComm Varchar(10),                                                                      
OtherFees Varchar(200),                                                                      
CommPurpose Varchar(10),                                                                      
Pledge Varchar(10),--29                                                                      
LotLocation Varchar(10),--COL30                                                                      
DestPledge Varchar(10),                                               
DestLotLocation Varchar(10),                                                                      
OriginalFace Varchar(10),                                                                      
YieldOnCost Varchar(10),                                                                      
DurationOnCost Varchar(10),--COL35                                                            
UserDef1 Varchar(10),                                                                      
UserDef2 Varchar(10),                                                                      
UserDef3 Varchar(10),                                                       
TranID Varchar(100),                                                                      
IPCounter Varchar(10),                               
Repl Varchar(10),                                                             
Source Varchar(10),                                       
Comment2 Varchar(10),                                   
OmniAcct Varchar(10),                                                                      
Recon Varchar(10),                                                                      
Post Varchar(10),                                                                      
LabelName Varchar(10),                                                                      
LabelDefinition Varchar(10),           
LabelDefinition_Date Varchar(10),                                                                      
LabelDefinition_String Varchar(10),--COL50                                                                      
Comment3 Varchar(10),                                                                      
RecordDate Varchar(10),                                                                    
ReclaimAmount Varchar(10),                                
Strategy Varchar(10),                                                                      
Comment4 Varchar(10),                                                                      
IncomeAccount Varchar(10),                                                                      
AccrualAccount Varchar(10),                                                                      
DivAccrualMethod Varchar(10),                                                                      
PerfContributionOrWithdrawal Varchar(10),--59                                                                      
--System internal use only                                              
CustomOrderBy Int,                                                
Level1AllocationID Varchar(200),                                                
TaxLotStateID Int,                                              
FromDeleted Varchar(50),                                            
InternalTranCode varchar(20) Null,            
StrategyID Int,            
FundID Int,            
ExchangeID Int,            
PutOrCall Varchar(10)                                                                      
)                 
                
Create Table #Temptransaction_Final                                                                      
(                  
TableID [bigint] IDENTITY(1,1) NOT NULL,                                         
TaxlotID Varchar(200),                                                                                           
ClosingTaxlotID Varchar(200),                                                                      
PortfolioCode Varchar(200),--COL1                               
TranCode Varchar(20),                                                                      
Comment Varchar(2000),                                                                      
AssetName Varchar(20),                                                                   
TickerSymbol Varchar(200),                                                                      
TradeDate Varchar(20),                                                                      
SettleDate Varchar(20),                                                                      
OriginalCostDate Varchar(20),                                                                      
Quantity Float,      
CloseMath Varchar(10),--10                                                                      
VersusDate Varchar(20),                                                                      
SourceType Varchar(20),                                                                      
SourceSymbol Varchar(20),                                                                      
TradeDateFXRate Varchar(20),                                                                      
SettleDateFXRate Varchar(20),                                                                      
OriginalFXRate Varchar(20),                                                                      
MarkToMarket Varchar(10),                                                                      
TradeAmount Varchar(500),                                                                      
OriginalCost Varchar(200),                                                                      
Comment1 Varchar(10),--COL20                                                 
WithholdingTax Varchar(10),                                                                      
Exchange Varchar(200),                                                                      
ExchangeFee Varchar(200),                                                                      
commission Varchar(200),                                                                      
Broker Varchar(200),                                                       
ImpliedComm Varchar(10),                                                                      
OtherFees Varchar(200),                                                                      
CommPurpose Varchar(10),                                                                      
Pledge Varchar(10),--29                                                                      
LotLocation Varchar(10),--COL30                                                                      
DestPledge Varchar(10),                                               
DestLotLocation Varchar(10),                                                                      
OriginalFace Varchar(10),                                                                      
YieldOnCost Varchar(10),                                   
DurationOnCost Varchar(10),--COL35                                                                      
UserDef1 Varchar(10),                                                                      
UserDef2 Varchar(10),                                                                      
UserDef3 Varchar(10),                                                       
TranID Varchar(100),                                                                      
IPCounter Varchar(10),                                                                      
Repl Varchar(10),                                                             
Source Varchar(10),                                                                      
Comment2 Varchar(10),                                   
OmniAcct Varchar(10),                                                                      
Recon Varchar(10),                                            
Post Varchar(10),                                                                      
LabelName Varchar(10),                                                                      
LabelDefinition Varchar(10),                                                                      
LabelDefinition_Date Varchar(10),                                                                      
LabelDefinition_String Varchar(10),--COL50                                                                      
Comment3 Varchar(10),                                                                      
RecordDate Varchar(10),                                                                    
ReclaimAmount Varchar(10),                                
Strategy Varchar(10),                                                                      
Comment4 Varchar(10),                                                          
IncomeAccount Varchar(10),                                                                      
AccrualAccount Varchar(10),                                                                      
DivAccrualMethod Varchar(10),                                                                      
PerfContributionOrWithdrawal Varchar(10),--59                                                                      
--System internal use only                                              
CustomOrderBy Int,                                                
Level1AllocationID Varchar(200),                                                
TaxLotStateID Int,                                              
FromDeleted Varchar(50),                                            
InternalTranCode varchar(20) Null,            
StrategyID Int,            
FundID Int,            
ExchangeID Int,            
PutOrCall Varchar(10),          
InternalTranID varchar(200),          
Ranking int,          
OriginalTaxlotID Varchar(200)                 
)              
            
-- All Opeining and Closing Trades of Current date            
Insert Into #TempTransaction_BeforeStrategy                                                                     
Select                                                                       
VT.TaxlotID As TaxlotID,                                                 
VT.TaxlotID As ClosingTaxlotID,                                                                      
Null PortfolioCode,                                                                        
Side As TranCode,                                              
NULL As Comment,                                                                      
T_Asset.AssetName As AssetName,                                                                    
VT.Symbol As TickerSymbol,                                                                   
--format mmddyyyy                                                                      
REPLACE(CONVERT(VARCHAR(10), AUECLocalDate, 101), '/', '') As TradeDate,                                                                      
REPLACE(CONVERT(VARCHAR(10), SettlementDate, 101), '/', '') As SettleDate,                                                                      
NULL As OriginalCostDate,                                                                      
TaxlotQty As Quantity,                         
--LTRIM(RTRIM(STR(TaxlotQty,LEN(TaxlotQty),8))) As Quantity,                                                
NULL as CloseMath,--10                                                        
NULL As VersusDate,                                                                
Null As SourceType,             
Null As SourceSymbol,                                                                
NULL As TradeDateFXRate,                                    
NULL As SettleDateFXRate,                                                                      
NULL As OriginalFXRate,                                       
NULL As MarkToMarket,                                                         
--Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees) * SideMultiplier)) As Decimal(32,2)) As Varchar(500)) As TradeAmount,                

          
                               
((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * SideMultiplier)) As TradeAmount,            
NULL As OriginalCost,                                                                      
NULL As Comment1,--COL20                                                                      
NULL As WithholdingTax,                                         
Null As Exchange,                                                                      
--Case                                                                       
-- When StampDuty > 0                                                                      
-- Then Cast(Cast(StampDuty As Decimal(32,2)) As Varchar(200))                                                                      
-- Else  Cast(StampDuty As Varchar(200))                                                    
--End As ExchangeFee,              
StampDuty As ExchangeFee,                                                                    
--Case                                                                      
-- When VT.Commission > 0                                                                      
-- Then Cast(Cast(VT.Commission As Decimal(32,2)) As Varchar(200))                      
-- Else  Cast(VT.Commission As Varchar(200))                                                                      
--End As commission,                                                                      
VT.Commission As commission,             
Lower(T_CounterParty.ShortName) As Broker,                                                                      
'n' As ImpliedComm,                                                              
--Case                                                                      
-- When (TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) > 0                                                                      
-- Then Cast(Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) As Decimal(32,2)) As Varchar(200))                                                                   
--Else  Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) As Varchar(200))                                                                      
--End As OtherFees,            
(TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) As OtherFees,                                                                      
NULL As CommPurpose,                                                                      
'n' As Pledge,--29                                                                      
'253' As LotLocation,--COL30                                                                      
NULL As DestPledge,                                                                      
NULL As DestLotLocation,                                                                      
NULL As OriginalFace,                          
NULL As YieldOnCost,               
NULL As DurationOnCost,--COL35                                                                      
NULL As UserDef1,                                                                      
--UserDef2 value added as strategy code, as per the client request sent by Korey on Feb 7, 2013                                                
Null UserDef2,                              
NULL As UserDef3,                                                                      
--Case                                               
--When ExternalTransID Is Not Null And ExternalTransID <> ''                                              
--Then ExternalTransID                          
--Else Null                                              
--End As TranID,             
Null As TranID,                                                                     
NULL As IPCounter,                                             
NULL As Repl,                                                                      
NULL As Source,                                                                      
NULL As Comment2,                                                                      
NULL As OmniAcct,                                    
NULL As Recon,                                                                      
'y' As Post,                                                                      
NULL As LabelName,                                                                      
NULL As LabelDefinition,                                                                      
NULL As LabelDefinition_Date,                                            
NULL As LabelDefinition_String ,--COL50                    
NULL As Comment3,                                                                      
NULL As RecordDate,                                                      
NULL As ReclaimAmount,                                                        
Null Strategy,                                                                      
NULL As Comment4,                                      
NULL As IncomeAccount,                                                                      
NULL As AccrualAccount,                                                                      
NULL As DivAccrualMethod,                                             
NULL As PerfContributionOrWithdrawal,--59                                          
Case                                                                      
 When (Side='Buy' or Side='Buy to Open' Or Side='Sell short' or Side='Sell to Open')                                                                      
 Then 1                                                                      
 When (Side='Buy to Close' or Side='Buy to Cover' Or Side='Sell' or Side='Sell to Close')                                                                      
 Then 0                                                                      
 Else 1                                                                 
End As CustomOrderBy,                                                
Null As Level1AllocationID,                                                
T_PBWiseTaxlotState.TaxLotState As TaxLotStateID,                                              
'No' As FromDeleted,                                            
Case                                                                      
 When (Side='Buy' or Side='Buy to Open')                                                                      
 Then 'by'                                                                      
 When (Side='Buy to Close' or Side='Buy to Cover')                                                      
 Then 'cs'                                                                      
 When (Side='Sell' or Side='Sell to Close')                                          
 Then 'sl'                                                                      
 When (Side='Sell short' or Side='Sell to Open')                                                         
 Then 'ss'                                                                      
 Else NULL                                                                 
End As InternalTranCode,            
VT.FundID,            
VT.ExchangeID,            
SM.PutOrCall             
                                                                      
From #V_Taxlots VT                                                
Inner Join T_PBWiseTaxlotState on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID                                                                     
Inner Join T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                                                                      
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                                                                    
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = VT.Symbol                                                                     
Left Outer Join T_CounterParty On VT.CounterPartyID = T_CounterParty.CounterPartyID                                                                      
Where VT.AUECID in (select AUECID from @AUECID)                                                   
And VT.FundID in (select FundID from @Fund) And T_PBWiseTaxlotState.PBID = @ThirdPartyID_LocalVar and T_PBWiseTaxlotState.FileFormatID = @fileFormatID                                                
And ((DateDiff(d,VT.AUECLocalDate,@InputDate_LocalVar) = 0 And (T_PBWiseTaxlotState.TaxLotState in (1,4)))                                           
 Or ((DateDiff(d,VT.AUECLocalDate,@InputDate_LocalVar)>=0 And T_PBWiseTaxlotState.TaxLotState in (0,2,3))))                                           
                       
-- Get Closed transactions                                              
Insert Into #TempTransaction_BeforeStrategy                                                                           
Select                                                                       
PT.TaxlotID As TaxlotID,                                                                      
PT1.TaxlotID As ClosingTaxlotID,             
Null PortfolioCode,                                                                      
                                                                      
';' As TranCode,                                                                      
'LOT:QTY ' + IsNull(PT.LotId,'') + ':' + Cast(Cast(PTC.ClosedQty As Decimal(32,8)) As Varchar(2000)) As Comment,                                 
NULL As AssetName,                                                                      
NULL As TickerSymbol,                                                                      
--format mmddyyyy                                                                      
REPLACE(CONVERT(VARCHAR(10), G1.AUECLocalDate, 101), '/', '') As TradeDate,                                                                      
NULL As SettleDate,                                                         
NULL As OriginalCostDate,                                                                      
0 As Quantity,                                                                      
NULL as CloseMath,--10                                                                      
NULL As VersusDate,                                                                      
NULL As SourceType,                                                                      
NULL As SourceSymbol,                                                                      
NULL As TradeDateFXRate,                                                                      
NULL As SettleDateFXRate,                                                                      
NULL As OriginalFXRate,                                       
NULL As MarkToMarket,                   
NULL As TradeAmount,                                                                      
NULL As OriginalCost,                                                                      
NULL As Comment1,--COL20                                                                      
NULL As WithholdingTax,                                                                     
NULL As Exchange,                                                                      
NULL As ExchangeFee,                                                                      
NULL As commission,                                                                      
NULL As Broker,         
NULL As ImpliedComm,                                                                      
NULL As OtherFees,                                                                      
NULL As CommPurpose,                                  
NULL As Pledge,--29                                                                      
NULL As LotLocation,--COL30                                                                      
NULL As DestPledge,                                                                      
NULL As DestLotLocation,                                                                      
NULL As OriginalFace,                                                                      
NULL As YieldOnCost,                                                                      
NULL As DurationOnCost,--COL35                                                                      
NULL As UserDef1,                                                      
NULL As UserDef2,                                                                      
NULL As UserDef3,                                                                      
NULL As TranID,                                                                 
NULL As IPCounter,                                                         
NULL As Repl,                                                                      
NULL As Source,                                                                      
NULL As Comment2,                                                                      
NULL As OmniAcct,                                                       
NULL As Recon,                                                                      
'y' As Post,                                                                      
NULL As LabelName,                                                                      
NULL As LabelDefinition,                                                                      
NULL As LabelDefinition_Date,                                                                      
NULL As LabelDefinition_String ,--COL50                                     
NULL As Comment3,                                                                      
NULL As RecordDate,                                                                      
NULL As ReclaimAmount,                                 
NULL As Strategy,                                                                      
NULL As Comment4,                                                                     
NULL As IncomeAccount,                                                   
NULL As AccrualAccount,                                                                      
NULL As DivAccrualMethod,                                                                      
NULL As PerfContributionOrWithdrawal,--59                                           
--System internal tags                                                                   
1 As CustomOrderBy,                                                
Null As Level1AllocationID,                                                
Null As TaxLotStateID,                                              
'No' As FromDeleted,                                            
Case                                                      
 When (Side='Buy' or Side='Buy to Open')                                                                   
 Then 'by'                                                                      
 When (Side='Buy to Close' or Side='Buy to Cover')                                                                      
 Then 'cs'                                                                      
 When (Side='Sell' or Side='Sell to Close')                                                                      
 Then 'sl'                                                                      
 When (Side='Sell short' or Side='Sell to Open')         
 Then 'ss'                                                                      
 Else NULL                                                                 
End As InternalTranCode,            
PT.FundID,            
0 As ExchangeID,            
'' As PutOrCall                                                                 
                                                                      
from #PM_TaxlotClosing  PTC                                                                                                                                                   
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                                                
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                                               
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                                                                   
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                                                      
Inner Join T_Side on T_Side.SideTagValue=G.OrderSideTagValue                                                                                                                                                
Inner Join T_PBWiseTaxlotState OpeningTaxlotState on OpeningTaxlotState.TaxlotID=PT.TaxlotID                                    
Inner Join T_PBWiseTaxlotState ClosingTaxlotState on ClosingTaxlotState.TaxlotID=PT1.TaxlotID                                                                                  
Where G.AUECID in (select AUECID from @AUECID)                                                   
And PT.FundID in (select FundID from @Fund)                                            
And (OpeningTaxlotState.PBID = @ThirdPartyID_LocalVar and OpeningTaxlotState.FileFormatID = @fileFormatID and ClosingTaxlotState.FileFormatID = @fileFormatID)                                          
And                                             
(                                        
(DateDiff(d,@InputDate_LocalVar,PTC.AUECLocalDate) = 0 and (DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) <> DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate))                                                   
And OpeningTaxlotState.TaxlotState In (1))                                        
Or                                            
(DateDiff(d,PTC.AUECLocalDate,@InputDate_LocalVar) >= 0 And ClosingTaxlotState.TaxlotState In (0,2) And (DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) <> DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)))                        
)                              
                              
--Get Deleted taxlots if any                                        
Insert Into #TempTransaction_BeforeStrategy                                                                  
                                                                      
Select                                                          
DT.TaxlotID As TaxlotID,                                                 
DT.TaxlotID As ClosingTaxlotID,                                                                      
Null PortfolioCode,                                                                     
Side As TranCode,                                                                      
NULL As Comment,                       
T_Asset.AssetName As AssetName,                      
DT.Symbol As TickerSymbol,                                                                    
--format mmddyyyy                                                                      
REPLACE(CONVERT(VARCHAR(10), AUECLocalDate, 101), '/', '') As TradeDate,                                                                      
REPLACE(CONVERT(VARCHAR(10), SettlementDate, 101), '/', '') As SettleDate,                                    
NULL As OriginalCostDate,                                                                      
TaxlotQty As Quantity,                                                                      
NULL as CloseMath,--10                                                                     
NULL As VersusDate,                                               
Null As SourceType,                                                                 
Null As SourceSymbol,                                                                
NULL As TradeDateFXRate,                                                                      
NULL As SettleDateFXRate,                                                                    
NULL As OriginalFXRate,                                                                      
NULL As MarkToMarket,                                                 
CASE                                                 
 WHEN (OrderSideTagValue IN ('A', 'B', '1'))                                                 
 --THEN Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees) * 1)) As Decimal(32,2)) As Varchar(500))             
 THEN ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))                                                    
 WHEN (OrderSideTagValue IN ('2', '5', '6', 'C', 'D'))                                                 
-- THEN  Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees) * -1)) As Decimal(32,2)) As Varchar(500))                                     
 Then ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees  + SecFee + OccFee + OrfFee + ClearingBrokerFee) * -1))            
 ELSE ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))            
--Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees) * 1)) As Decimal(32,2)) As Varchar(500))                                              
END As TradeAmount,                                                
NULL As OriginalCost,                                                                      
NULL As Comment1,--COL20                                                                      
NULL As WithholdingTax,                                                                      
Null As Exchange,                        
--Case                                                                       
-- When StampDuty > 0                                                                      
-- Then Cast(Cast(StampDuty As Decimal(32,2)) As Varchar(200))                                     
-- Else Cast(StampDuty As Varchar(200))                                                                      
--End As ExchangeFee,             
DT.StampDuty As ExchangeFee,                                                                      
--Case                                                                      
-- When DT.Commission > 0                                                                      
-- Then Cast(Cast(DT.Commission As Decimal(32,2)) As Varchar(200))                                                                      
-- Else  Cast(DT.Commission As Varchar(200))                                                   
--End As commission,            
DT.Commission As commission,                                                                      
Lower(T_CounterParty.ShortName) As Broker,                                                  
'n' As ImpliedComm,                                                         
--Case                                                                      
-- When (TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) > 0                                                        
-- Then Cast(Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) As Decimal(32,2)) As Varchar(200))                                                                   
-- Else  Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees) As Varchar(200))                                                                      
--End As OtherFees,              
(TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees  + SecFee + OccFee + DT.OrfFee + ClearingBrokerFee) As OtherFees,                                                                    
NULL As CommPurpose,                                                                      
'n' As Pledge,--29                                                                      
'253' As LotLocation,--COL30                                                                      
NULL As DestPledge,                                                                      
NULL As DestLotLocation,                                                                      
NULL As OriginalFace,                                                                      
NULL As YieldOnCost,                   
NULL As DurationOnCost,--COL35                                                                      
NULL As UserDef1,                        
                                          
Null UserDef2,                                                                      
NULL As UserDef3,                                               
--Case                                              
--When DT.ExternalTransId Is Not Null And ExternalTransID <> ''                                              
--Then DT.ExternalTransId                                              
--Else NULL                                               
--End As TranID,             
Null As TranID,                                                                     
NULL As IPCounter,                                                                      
NULL As Repl,                                                                      
NULL As Source,                                                                      
NULL As Comment2,                                                                      
NULL As OmniAcct,                                                                      
NULL As Recon,                                                                      
'y' As Post,                                                                      
NULL As LabelName,                                                 
NULL As LabelDefinition,                                                                      
NULL As LabelDefinition_Date,                                   
NULL As LabelDefinition_String ,--COL50                                                                      
NULL As Comment3,                                                                      
NULL As RecordDate,                                                            
NULL As ReclaimAmount,                                       
Null Strategy,                                              
NULL As Comment4,                                                                      
NULL As IncomeAccount,                                                                      
NULL As AccrualAccount,                                                                      
NULL As DivAccrualMethod,                                                                      
NULL As PerfContributionOrWithdrawal,--59                                                                      
0 As CustomOrderBy,       
DT.Level1AllocationID,                                                
DT.TaxLotState,                                              
'Yes' As FromDeleted,                                            
Case                                       
 When (Side='Buy' or Side='Buy to Open')                                                               
 Then 'by'                                                                      
 When (Side='Buy to Close' or Side='Buy to Cover')                                                               
 Then 'cs'                                      
 When (Side='Sell' or Side='Sell to Close')                                                                      
 Then 'sl'                                                                      
 When (Side='Sell short' or Side='Sell to Open')                                                                      
 Then 'ss'                                                                      
 Else NULL                                                                 
End As InternalTranCode,            
DT.FundID,            
DT.ExchangeID,            
SM.PutOrCall                                                                                                                
From T_DeletedTaxlots DT                                                                         
Inner Join T_Side on T_Side.SideTagValue=DT.OrderSideTagValue                                                                      
Inner Join T_Asset on T_Asset.AssetID=DT.AssetID                                                                        
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = DT.Symbol                                                                     
Left Outer Join T_CounterParty On DT.CounterPartyID = T_CounterParty.CounterPartyID                                                                      
--FX Rate for Trade Date from Daily Valuation                                                                    
Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                    
On (FXDayRatesForTradeDate.FromCurrencyID = DT.CurrencyID                                                                                                                                             
And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                   
And DateDiff(d,DT.AUECLocalDate,FXDayRatesForTradeDate.Date)=0)                                                                      
Where DateDiff(d,DT.AUECLocalDate,@InputDate_LocalVar) >= 0 And                                                   
DT.AUECID in (select AUECID from @AUECID)                                                   
And DT.FundID in (select FundID from @Fund)                                                 
And DT.TaxlotState = 3 And DT.PBID = @ThirdPartyID_LocalVar and DT.FileFormatID = @fileFormatID           
            
-- Update TaxlotState and Level1 ID that is used for system internal use only                                              
Update #TempTransaction_BeforeStrategy                                   
Set Level1AllocationID = VT.Level1AllocationID,                                                
TaxLotStateID = T_PBWiseTaxlotState.TaxLotState                                                
From #TempTransaction_BeforeStrategy                                                 
Inner Join #V_Taxlots VT On VT.TaxlotID = #TempTransaction_BeforeStrategy.TaxlotID                                                
Inner Join T_PBWiseTaxlotState on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID                                   
Where T_PBWiseTaxlotState.PBID = @ThirdPartyID_LocalVar and T_PBWiseTaxlotState.FileFormatID = @fileFormatID            
            
--Symbol Mapping                                                      
Update #TempTransaction_BeforeStrategy                                                      
Set TickerSymbol = Lower(Mapping.AXYSSymbol)                              
From #TempTransaction_BeforeStrategy                                                       
Inner Join T_PranaAXYSSymbolMapping  Mapping On Mapping.PranaSymbol = #TempTransaction_BeforeStrategy.TickerSymbol              
            
Insert InTo #TempTransaction            
Select             
Temp_BeforeStrategy.TaxlotID,                                                                                           
Temp_BeforeStrategy.ClosingTaxlotID,                                                                      
Temp_BeforeStrategy.PortfolioCode,                               
Temp_BeforeStrategy.TranCode,                                                                      
Temp_BeforeStrategy.Comment,                                                                      
Temp_BeforeStrategy.AssetName,                                                                   
Temp_BeforeStrategy.TickerSymbol,                                                                      
Temp_BeforeStrategy.TradeDate ,                                                                      
Temp_BeforeStrategy.SettleDate ,                                                                      
Temp_BeforeStrategy.OriginalCostDate ,            
Case            
 When Temp_BeforeStrategy.Quantity > 0            
 Then StrategyTable.Quantity             
 Else Temp_BeforeStrategy.Quantity            
End As Quantity,            
Temp_BeforeStrategy.CloseMath ,--10                                                                      
Temp_BeforeStrategy.VersusDate,                
Temp_BeforeStrategy.SourceType ,                                                                      
Temp_BeforeStrategy.SourceSymbol,                                                                      
Temp_BeforeStrategy.TradeDateFXRate ,                                                                      
Temp_BeforeStrategy.SettleDateFXRate ,                                                                      
Temp_BeforeStrategy.OriginalFXRate ,                                                                      
Temp_BeforeStrategy.MarkToMarket ,             
Case            
 When Temp_BeforeStrategy.Quantity > 0            
 Then Cast(Cast(((Temp_BeforeStrategy.TradeAmount / Temp_BeforeStrategy.Quantity) * StrategyTable.Quantity) As Decimal(32,2)) As Varchar(200))            
 Else Null            
End As TradeAmount,                                                                     
Temp_BeforeStrategy.OriginalCost ,                                                                      
Temp_BeforeStrategy.Comment1 ,--COL20                                                 
Temp_BeforeStrategy.WithholdingTax ,                                                                      
Temp_BeforeStrategy.Exchange ,             
Case            
 When Temp_BeforeStrategy.ExchangeFee > 0 And Temp_BeforeStrategy.Quantity > 0                                                                     
 Then Cast(Cast((Temp_BeforeStrategy.ExchangeFee * (StrategyTable.Quantity / Temp_BeforeStrategy.Quantity)) As Decimal(32,2)) As Varchar(200))            
 When Temp_BeforeStrategy.ExchangeFee = 0            
 Then Cast(Temp_BeforeStrategy.ExchangeFee As Varchar(200))             
Else Null            
End as ExchangeFee ,            
Case            
 When (Temp_BeforeStrategy.Quantity > 0 And Temp_BeforeStrategy.commission > 0)                                                                  
 Then Cast(Cast((Temp_BeforeStrategy.commission * (StrategyTable.Quantity / Temp_BeforeStrategy.Quantity)) As Decimal(32,2)) As Varchar(200))            
 When Temp_BeforeStrategy.commission = 0            
 Then Cast(Temp_BeforeStrategy.commission As Varchar(200))            
Else Null            
End commission,                                                                      
Temp_BeforeStrategy.Broker ,                                                                      
Temp_BeforeStrategy.ImpliedComm ,             
Case             
 When Temp_BeforeStrategy.Quantity > 0 And Temp_BeforeStrategy.OtherFees > 0            
 Then Cast(Cast((Temp_BeforeStrategy.OtherFees * (StrategyTable.Quantity / Temp_BeforeStrategy.Quantity)) As Decimal(32,2)) As Varchar(200))            
 When Temp_BeforeStrategy.OtherFees  = 0            
 Then Cast(Temp_BeforeStrategy.OtherFees As Varchar(200))            
Else Null            
End As OtherFees,                  
Temp_BeforeStrategy.CommPurpose ,                                                                      
Temp_BeforeStrategy.Pledge ,--29                                                 
Temp_BeforeStrategy.LotLocation ,--COL30                                                                      
Temp_BeforeStrategy.DestPledge ,                                               
Temp_BeforeStrategy.DestLotLocation ,                                                                      
Temp_BeforeStrategy.OriginalFace ,                                                                      
Temp_BeforeStrategy.YieldOnCost ,                                                                      
Temp_BeforeStrategy.DurationOnCost ,--COL35                                                            
Temp_BeforeStrategy.UserDef1 ,                                                                      
Temp_BeforeStrategy.UserDef2 ,                                                                      
Temp_BeforeStrategy.UserDef3 ,                                                       
Temp_BeforeStrategy.TranID ,                                                                      
Temp_BeforeStrategy.IPCounter ,      
Temp_BeforeStrategy.Repl ,                                                             
Temp_BeforeStrategy.Source ,                                                                      
Temp_BeforeStrategy.Comment2 ,                                   
Temp_BeforeStrategy.OmniAcct ,                                                                      
Temp_BeforeStrategy.Recon ,                                                                      
Temp_BeforeStrategy.Post ,                                                                      
Temp_BeforeStrategy.LabelName ,                                                                      
Temp_BeforeStrategy.LabelDefinition ,                                                                      
Temp_BeforeStrategy.LabelDefinition_Date ,                                                                      
Temp_BeforeStrategy.LabelDefinition_String ,--COL50                                                                      
Temp_BeforeStrategy.Comment3 ,                                                                      
Temp_BeforeStrategy.RecordDate ,                                                                    
Temp_BeforeStrategy.ReclaimAmount ,                                
Null Strategy,                                                                  
Temp_BeforeStrategy.Comment4 ,                                                                      
Temp_BeforeStrategy.IncomeAccount ,                                                                      
Temp_BeforeStrategy.AccrualAccount,                                                                      
Temp_BeforeStrategy.DivAccrualMethod ,                                                                      
Temp_BeforeStrategy.PerfContributionOrWithdrawal ,--59                                                               
--System internal use only                                              
Temp_BeforeStrategy.CustomOrderBy ,                                                
Temp_BeforeStrategy.Level1AllocationID ,                                                
Temp_BeforeStrategy.TaxLotStateID ,                                              
Temp_BeforeStrategy.FromDeleted ,                                            
Temp_BeforeStrategy.InternalTranCode,            
StrategyTable.StrategyID,            
Temp_BeforeStrategy.FundID,            
Temp_BeforeStrategy.ExchangeID,            
Temp_BeforeStrategy.PutOrCall            
From #TaxlotIDAndStrategyIDGroupingTable StrategyTable            
Inner Join  #TempTransaction_BeforeStrategy Temp_BeforeStrategy On StrategyTable.TaxlotID = Temp_BeforeStrategy.TaxlotID             
LEFT OUTER JOIN T_CompanyStrategy CS ON StrategyTable.StrategyID = CS.CompanyStrategyID                
            
--Deleted taxlots            
Insert InTo #TempTransaction            
Select             
Temp_BeforeStrategy.TaxlotID,                                                                                           
Temp_BeforeStrategy.ClosingTaxlotID,                                                                      
Temp_BeforeStrategy.PortfolioCode,             
Temp_BeforeStrategy.TranCode,                                                   
Temp_BeforeStrategy.Comment,                                                                      
Temp_BeforeStrategy.AssetName,                                                                   
Temp_BeforeStrategy.TickerSymbol,                                                                      
Temp_BeforeStrategy.TradeDate ,                                                                      
Temp_BeforeStrategy.SettleDate ,                                                                      
Temp_BeforeStrategy.OriginalCostDate ,            
Temp_BeforeStrategy.Quantity,            
--Case            
-- When Temp_BeforeStrategy.Quantity > 0            
-- Then StrategyTable.Quantity             
-- Else Temp_BeforeStrategy.Quantity            
--End As Quantity,            
Temp_BeforeStrategy.CloseMath ,--10                   
Temp_BeforeStrategy.VersusDate,                                                                      
Temp_BeforeStrategy.SourceType ,                                                                      
Temp_BeforeStrategy.SourceSymbol,                                                                      
Temp_BeforeStrategy.TradeDateFXRate ,                                                                      
Temp_BeforeStrategy.SettleDateFXRate ,                                                                      
Temp_BeforeStrategy.OriginalFXRate ,                                                                      
Temp_BeforeStrategy.MarkToMarket ,            
Temp_BeforeStrategy.TradeAmount,            
--Case            
-- When Temp_BeforeStrategy.Quantity > 0            
-- Then Cast(Cast(((Temp_BeforeStrategy.TradeAmount / Temp_BeforeStrategy.Quantity) * StrategyTable.Quantity) As Decimal(32,2)) As Varchar(200))            
-- Else Null            
--End As TradeAmount,                                                                    
Temp_BeforeStrategy.OriginalCost ,                                                                      
Temp_BeforeStrategy.Comment1 ,--COL20                                                 
Temp_BeforeStrategy.WithholdingTax ,                                                                      
Temp_BeforeStrategy.Exchange ,             
Temp_BeforeStrategy.ExchangeFee,            
Temp_BeforeStrategy.commission,                                                                      
Temp_BeforeStrategy.Broker ,                                                                      
Temp_BeforeStrategy.ImpliedComm ,             
Temp_BeforeStrategy.OtherFees,                                                                      
Temp_BeforeStrategy.CommPurpose ,                                                                      
Temp_BeforeStrategy.Pledge ,--29                                                                      
Temp_BeforeStrategy.LotLocation ,--COL30                                                                      
Temp_BeforeStrategy.DestPledge ,                                               
Temp_BeforeStrategy.DestLotLocation ,              
Temp_BeforeStrategy.OriginalFace ,                                                                      
Temp_BeforeStrategy.YieldOnCost ,                                                                      
Temp_BeforeStrategy.DurationOnCost ,--COL35                                                            
Temp_BeforeStrategy.UserDef1 ,                                                                      
Temp_BeforeStrategy.UserDef2 ,                                                                      
Temp_BeforeStrategy.UserDef3 ,                                                       
Temp_BeforeStrategy.TranID ,                                                                      
Temp_BeforeStrategy.IPCounter ,                               
Temp_BeforeStrategy.Repl ,                                                             
Temp_BeforeStrategy.Source ,                                                                      
Temp_BeforeStrategy.Comment2 ,                                   
Temp_BeforeStrategy.OmniAcct ,                 
Temp_BeforeStrategy.Recon ,                                                                      
Temp_BeforeStrategy.Post ,                                                                      
Temp_BeforeStrategy.LabelName ,                       
Temp_BeforeStrategy.LabelDefinition ,                                                                      
Temp_BeforeStrategy.LabelDefinition_Date ,                                                                      
Temp_BeforeStrategy.LabelDefinition_String ,--COL50                                                                      
Temp_BeforeStrategy.Comment3 ,                                                                      
Temp_BeforeStrategy.RecordDate ,                                                 
Temp_BeforeStrategy.ReclaimAmount ,                                
Temp_BeforeStrategy.Strategy ,             
Temp_BeforeStrategy.Comment4 ,                                                                      
Temp_BeforeStrategy.IncomeAccount ,                                                                      
Temp_BeforeStrategy.AccrualAccount,                                                                      
Temp_BeforeStrategy.DivAccrualMethod ,                                                                      
Temp_BeforeStrategy.PerfContributionOrWithdrawal ,--59                                                                      
--System internal use only                                              
Temp_BeforeStrategy.CustomOrderBy ,                                                
Temp_BeforeStrategy.Level1AllocationID ,                                                
Temp_BeforeStrategy.TaxLotStateID ,                                              
Temp_BeforeStrategy.FromDeleted ,                                            
Temp_BeforeStrategy.InternalTranCode,            
StrategyTable.StrategyID As StrategyID,            
Temp_BeforeStrategy.FundID,            
Temp_BeforeStrategy.ExchangeID,            
Temp_BeforeStrategy.PutOrCall            
From  #TempTransaction_BeforeStrategy Temp_BeforeStrategy            
----Inner Join #T_TaxlotAndExternalTransactionIDMapping StrategyTable On StrategyTable.TaxlotID = Temp_BeforeStrategy.TaxlotID            
--Inner Join #Temp_ExternalTransIDSplittedTable StrategyTable On StrategyTable.TaxlotID = Temp_BeforeStrategy.TaxlotID            
Left Outer Join #Temp_ExternalTransIDSplittedTable StrategyTable On StrategyTable.TaxlotID = Temp_BeforeStrategy.TaxlotID            
            
Where Temp_BeforeStrategy.TaxlotStateID = 3            
            
--Select * from #TempTransaction_BeforeStrategy             
            
Update #TempTransaction            
Set            
PortfolioCode =             
 Case                                                                      
  When FundName = 'OFFSHORE'                                              
  Then 'ssfqp'                                   
  When FundName = 'LP C/O'                                              
  Then 'cay'                            
  When FundName = 'Private Equity:038C3894'                                              
  Then 'sspe'                                                                      
  When FundName = 'Technology:038C3895'                                              
  Then 'techn'                                                                      
  When FundName = 'Technology II:038C3896'                                              
  Then 'tech2'                                                                      
  When FundName = 'Life Sciences:038C3897'                                              
  Then 'ls'                                                                      
  Else NULL                                                           
 End,            
TranCode =            
Case            
 When (Comment is null Or Comment = '')            
 Then            
  Case                                                               
   When (TranCode='Buy' or TranCode='Buy to Open')                                                                      
   Then 'by'                                                                      
   When (TranCode='Buy to Close' or TranCode='Buy to Cover')                                                                      
   Then 'cs'                                                                      
   When (TranCode='Sell' or TranCode='Sell to Close')                                                                      
   Then 'sl'                                                                      
   When (TranCode='Sell short' or TranCode='Sell to Open')                                                                      
   Then 'ss'                                                                      
  Else NULL                                                                 
  End            
 Else TranCode            
End,            
AssetName =             
 Case                                                                    
  --Warrants as it is PrivateEquity in our application                                            
  When AssetName = 'PrivateEquity' And CharIndex('_W',TickerSymbol) <> 0                                              
  Then 'wtus'                       
  --Units as it is PrivateEquity in our application                      
  When (AssetName = 'PrivateEquity' And CharIndex('_U',TickerSymbol) <> 0) Or (AssetName = 'Equity' And TickerSymbol = 'NVEEU')                                              
  Then 'utus'                      
  --Rights as it is PrivateEquity in our application                      
  When AssetName = 'PrivateEquity' And CharIndex('_R',TickerSymbol) <> 0                                              
  Then 'rtus'                       
  --Corporate Bonds as it is FixedIncome in our application                                          
  When AssetName = 'FixedIncome' And CharIndex('_B',TickerSymbol) <> 0                                           
  Then 'cbus'                                                                      
  --Equity symbol with _PVT as it is Equity in our application                      
  When AssetName = 'Equity' And CharIndex('_PVT',TickerSymbol) <> 0                      
  Then 'csus'                          
  --Preferred stock as it is Equity in our application                                          
  When AssetName = 'Equity' And CharIndex('_P',TickerSymbol) <> 0                                            
  Then 'psus'                      
  --Equity                        
  When AssetName = 'Equity'                                                                      
  Then 'csus'                                                                      
  --Call Option                                    
  When (AssetName='EquityOption' and PutOrCall = 'CALL')                                                     
  Then 'clus'                                                                      
  --Put Option                                              
  When (AssetName='EquityOption' and PutOrCall = 'PUT')                                                                      
  Then 'ptus'                                                                  
Else NULL                                                    
 End,             
TickerSymbol =             
Case                                          
 When CHARINDEX('_',TickerSymbol) > 0                                                                   
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('_',TickerSymbol)-1))                                                            
 When (CHARINDEX('-TC',TickerSymbol) > 0 Or CHARINDEX('-VC',TickerSymbol) > 0)                                                                   
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('-',TickerSymbol)-1) + '.CN')                                                              
 When (CHARINDEX('-TAI',TickerSymbol) > 0 Or CHARINDEX('-GTS',TickerSymbol) > 0)                                                                   
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('-',TickerSymbol)-1) + '.TT')                                                 
 Else LOWER(TickerSymbol)                                                                      
End,              
            
--TranCode has side value            
--25 Jan 2013: Request by Korey: short sells (trans code ï¿½ssï¿½), please make SourceType (column 12) ï¿½awusï¿½ (instead of ï¿½causï¿½),             
SourceType =             
Case            
 When (Comment is null Or Comment = '')            
 Then            
  Case                                                                      
   When (TranCode='Buy' or TranCode='Buy to Open' or TranCode='Buy to Close' or TranCode='Buy to Cover' or TranCode='Sell' or TranCode='Sell to Close')                                                                      
   Then 'caus'                                                                      
   Else 'awus'                                                                    
  End            
 Else SourceType            
End,            
--TranCode has side value            
SourceSymbol =             
Case            
 When (Comment is null Or Comment = '')            
 Then            
  Case                                                                       
   When (TranCode='Buy' or TranCode='Buy to Open' or TranCode='Buy to Close' or TranCode='Buy to Cover' or TranCode='Sell' or TranCode='Sell to Close')                                                          
   Then 'capfund'                                                                      
   Else 'none'            
  End                
 Else SourceSymbol                                                
End,            
UserDef2 =             
Case             
When (Comment is null Or Comment = '')            
Then            
 Case                                                        
  When CS.StrategyName = ' Strategy1'                                                        
  Then '1'                                                        
  When CS.StrategyName = ' Strategy2'                                                        
  Then '2'                                                        
  When CS.StrategyName = 'Long'                                                        
  Then '3'                                                        
  When CS.StrategyName = 'Short'                                                        
  Then '4'                                                        
  When CS.StrategyName = 'Rest/Reg Lot 5'                                           
  Then '5'                                                        
  When CS.StrategyName = 'Wts (R/R) Lt 1'                                                        
  Then '6'                            
  When CS.StrategyName = 'Wts (R/R) Lt 2'                                                        
  Then '7'                                                        
  When CS.StrategyName = 'Wts (R/R) Lt 3'                                                        
  Then '8'                                                        
  When CS.StrategyName = 'PS (R/R) Lt 1'                                         
  Then '9'                                                       
  When CS.StrategyName = 'PS (R/R) Lt 2'                                                        
  Then '10'                                                        
  When CS.StrategyName = 'PS (R/R) Lt 3'                                                        
  Then '11'                                                
  When CS.StrategyName = 'PS (R/R) Lt 4'                                                        
  Then '12'                                                        
  When CS.StrategyName = 'Rule 144'                                                        
  Then '13'                                                        
  When CS.StrategyName = 'Interest'                                                        
  Then '14'                                                        
  When CS.StrategyName = 'Sell in Canada only - Reg S'                                                        
  Then '15'                                                        
  When CS.StrategyName = 'Miscellaneous'                                                        
  Then '16'                                                
  When CS.StrategyName = 'Liquidated Damages'                                                        
  Then '17'                                                        
  When CS.StrategyName = 'CB (R/R) Lt. 1'                                                        
  Then '18'                           
  When CS.StrategyName = 'Rule 144 Lt 2'                                                        
  Then '19'                                                        
  When CS.StrategyName = 'Rule 144 Lt 3'                                                        
  Then '20'                     
  When CS.StrategyName = 'Wts (Sell in CND only) - Reg S'                                                        
  Then '21'                                                        
  When CS.StrategyName = 'Sell in Canada only Lt2 - Reg S'                                                        
  Then '22'                                                        
  When CS.StrategyName = 'CB (Sell in CND  only)'                                        
  Then '23'                                                        
  When CS.StrategyName = 'Sell in Canada only Lt3 - Reg S'                                                        
  Then '24'                                                        
  Else NULL             
 End            
Else Null                                                       
End,            
Strategy =             
Case             
When (Comment is null Or Comment = '')            
Then            
 Case                                                        
  When CS.StrategyName = ' Strategy1'                       
  Then '1'                                                        
  When CS.StrategyName = ' Strategy2'                                                        
  Then '2'                                                        
  When CS.StrategyName = 'Long'                                                        
  Then '3'                                                        
  When CS.StrategyName = 'Short'                                                        
  Then '4'                                                        
  When CS.StrategyName = 'Rest/Reg Lot 5'                                           
  Then '5'                                                        
  When CS.StrategyName = 'Wts (R/R) Lt 1'                                               
  Then '6'                                                        
  When CS.StrategyName = 'Wts (R/R) Lt 2'                                                        
  Then '7'                                                        
  When CS.StrategyName = 'Wts (R/R) Lt 3'                                                        
  Then '8'                                                        
  When CS.StrategyName = 'PS (R/R) Lt 1'                                                        
  Then '9'                                                        
  When CS.StrategyName = 'PS (R/R) Lt 2'                    
  Then '10'                                                        
  When CS.StrategyName = 'PS (R/R) Lt 3'              
  Then '11'                                                
  When CS.StrategyName = 'PS (R/R) Lt 4'                                                        
  Then '12'                                                        
  When CS.StrategyName = 'Rule 144'                                                        
  Then '13'                                                        
  When CS.StrategyName = 'Interest'                                                        
  Then '14'                                                        
  When CS.StrategyName = 'Sell in Canada only - Reg S'                                                        
  Then '15'                                                        
  When CS.StrategyName = 'Miscellaneous'                                                        
  Then '16'                                                
  When CS.StrategyName = 'Liquidated Damages'                                                        
  Then '17'                                                        
  When CS.StrategyName = 'CB (R/R) Lt. 1'                                                        
  Then '18'                           
  When CS.StrategyName = 'Rule 144 Lt 2'                                                        
  Then '19'                                                        
  When CS.StrategyName = 'Rule 144 Lt 3'                                                        
  Then '20'                                                        
  When CS.StrategyName = 'Wts (Sell in CND only) - Reg S'              
  Then '21'                                                        
  When CS.StrategyName = 'Sell in Canada only Lt2 - Reg S'                                                 
  Then '22'                                                        
  When CS.StrategyName = 'CB (Sell in CND  only)'                                        
  Then '23'                                                        
  When CS.StrategyName = 'Sell in Canada only Lt3 - Reg S'                                                        
  Then '24'                                                        
  Else NULL             
 End            
Else Null                                                       
End,            
Exchange =             
Case             
 When (Comment is null Or Comment = '')            
 Then            
  Case                                                             
   When (T_Exchange.DisplayName = 'NYSE' Or T_Exchange.DisplayName = 'BB' Or T_Exchange.DisplayName = 'OTC' Or T_Exchange.DisplayName = 'PS')                                       
   Then '1'                                                                      
   When T_Exchange.DisplayName = 'NASDAQ'                                                                      
   Then '3'                                                                      
   When T_Exchange.DisplayName = 'AMEX'                                                   
   Then '2'                                                                      
   When (T_Exchange.DisplayName = 'TAI' Or  T_Exchange.DisplayName = 'TAI (OTC)')                                                                      
  Then '5'                                                                      
   When (T_Exchange.DisplayName = 'TSX' Or  T_Exchange.DisplayName = 'CDNX')                                                                      
   Then '4'                                                                      
   Else NULL                                                                      
  End            
 Else Exchange            
End            
From #TempTransaction Temp_Transaction            
Inner Join T_CompanyFunds CF On CF.CompanyFundID = Temp_Transaction.FundID            
Left Outer Join T_CompanyStrategy CS On CS.CompanyStrategyID = Temp_Transaction.StrategyID             
Left Outer Join T_Exchange On T_Exchange.ExchangeID = Temp_Transaction.ExchangeID                 
            
            
/*             
Case: Update Closing  Methodology i.e. CloseMath ='s'             
when a trade opens and closes on same date                                     
*/            
                                          
Update #TempTransaction                                              
Set CloseMath = 's' ,                                        
CustomOrderBy = 1                                             
from #PM_TaxlotClosing  PTC                                                  
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                         
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                            
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                          
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                              
Inner Join #TempTransaction on #TempTransaction.ClosingTaxlotID = PT1.TaxlotID                                                
Where ((DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) = DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)) And PT1.OrderSideTagValue in ('2','D','B'))                            
And (#TempTransaction.Comment is null Or #TempTransaction.Comment = '')                   
            
/*            
 Case: Update Closing  Methodology i.e. CloseMath ='b'             
 When Current date sell trade (Closing) closes historical buy trade (opening trade) i.e. buy and sell trades have different dates.            
 There are 2 columns, TaxlotID and ClosingTaxlotID in #TempTransaction,             
 for a opening trade TaxlotID column has its own TaxlotID and ClosingTaxlotID column has value with which it is closed i.e. sell trade taxlotID            
 for a closing trade i.e. sell trade both columns have same value i.e. its own TaxlotID                                                          
 Technical explaination:            
 ClosingTaxlotID -> when we fetch transactions from V_Taxlot view, it returns TaxlotID, I say it closingTaxlotID and add in table #TempTransaction                                                          
 When we fetch data from #PM_TaxlotClosing table, it has both positional and closing TaxlotIDs, I use only closingTaxlotID and collect in the #TempTransaction table                                                                    
 I need only closingTaxlotID. If a short transaction closes any long trade, then short trade will have more than one closingTaxlotID i.e.                                                                  
 one from #V_Taxlots and one or more from #PM_TaxlotClosing, so collect all the ClosingTaxlotID here                                                                     
*/            
----Select                                                                     
----ClosingTaxlotID                                                                  
----Into #TempClosingIDTable                                                                  
----From #TempTransaction                                 
----Group By ClosingTaxlotID             
----having Count(ClosingTaxlotID) > 1            
            
Select                                                                     
#TempTransaction.ClosingTaxlotID                                                             
Into #TempClosingIDTable                                                                    
from #PM_TaxlotClosing  PTC                                                
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                                              
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                          
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                        
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                            
Inner Join #TempTransaction on #TempTransaction.ClosingTaxlotID = PT1.TaxlotID                   
Where ((DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) <> DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)))            
Group By #TempTransaction.ClosingTaxlotID                                                                  
having Count(#TempTransaction.ClosingTaxlotID) > 1                                                                  
                                     
/*            
--we know that main closing trade has all the information like Qty, Price but child i.e. comment line (;) which one it is closing, has selected information                                                                  
--so I made a check on Qty field because Qty field is null in the comment line (;)            
*/                                                                 
Update #TempTransaction                                                                    
Set CloseMath = 'b'                                                   
From #TempTransaction                                                                    
Where ClosingTaxlotID in (Select ClosingTaxlotID From #TempClosingIDTable)                                                                    
And Quantity is not null and Quantity > 0 and CloseMath is null            
            
/*            
---------------------Added by Narendra------------------------------               
 Case: update CloseMath = 's' when a sell trade closes a historical buy trade and historical buy does not have LotId            
 Update closing methodology Closemath ='s'                                 
 if a taxlot closes buy of previous day but one of buy taxlot does not have lotid                              
*/                        
                            
-- collect all trades closingTaxlotID             
Select                                                                     
ClosingTaxlotID                                                                  
Into #TempClosingIDTable_WithoutLotIDTrades                                                                  
From #TempTransaction                                                                  
Group By ClosingTaxlotID             
having Count(ClosingTaxlotID) > 1            
              
-- Join with Level2Allocation table to check LotID                      
Select                                                                   
T1.ClosingTaxlotID                                                                  
Into #TempClosingIDTableToChangedMath                                                                    
from #TempTransaction T1                        
Inner Join #TempClosingIDTable_WithoutLotIDTrades TempClosingId On T1.ClosingTaxlotID = TempClosingId.ClosingTaxlotID                        
Inner Join #T_Level2Allocation L2 On T1.TaxLotId = L2.TaxLotId                          
Where T1.TaxlotId <> T1.ClosingTaxLotId And (L2.LotId is null Or L2.LotId = '')                          
                          
-- Update CloseMethodology to 's' of closing trade i.e. sell trade                       
Update T1                        
Set CloseMath='s'                          
from #TempTransaction T1                        
where ClosingTaxlotID                          
In (Select ClosingTaxlotID from #TempClosingIDTableToChangedMath)                        
And Taxlotid = ClosingTaxlotID            
                          
--Delete closed taxlots which have LotId null                                          
Delete TT1                          
from #TempTransaction TT1                          
Where TT1.ClosingTaxLotid                         
in (Select ClosingTaxlotID from #TempClosingIDTableToChangedMath)                           
and Taxlotid!=ClosingTaxlotID             
            
--------------------------------------------------------------------------                
/*                         
 If a previous date closed transaction i.e. buy transaction is closed with the currecnt date sell transaction and amended i.e. buy transaction is amended                                        
 then its handling is in XSLT, so we set its taxlot state to send                                           
*/            
Update #TempTransaction                                            
Set TaxLotStateID = 1                                            
From #TempTransaction                                            
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTransaction InnerTemp Where InnerTemp.InternalTranCode = #TempTransaction.InternalTranCode )               
And Comment is not null and TaxLotStateID = 2                                            
                                            
/* Collect taxlotid whose taxlot State is Amended and which is closing privious date buy taxlots i.e. CloseMath = 'b' */            
Select                                                 
ClosingTaxlotID,                      
TaxLotStateID                                                
Into #TempTaxlotState                                             
From #TempTransaction                                                
Where CloseMath = 'b'             
                      
/* Set taxlotstate to amend of previous date buy taxlots which is closed with today's sell taxlot but this sell is amended                                            
   so to bundle all these trades, we set taxlotstate to amend to the buy taxlots also.                      
*/            
                      
Update #TempTransaction                                            
Set TaxLotStateID = 2                                                
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTaxlotState Where TaxLotStateID = 2)                      
                        
/* if a sell trade closes historical buy trades then this buy trade should come under this sell trade i.e. come togather                      
 so to bundle all these trades, we set taxlotstate to allocated to the buy trades also.              
*/            
                                       
Update #TempTransaction                                                
Set TaxLotStateID = 0                                                
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTaxlotState Where TaxLotStateID = 0)                                        
                                       
/* this is to arrange same day open and close transactions i.e. buy and then sell or short and cover */            
                                       
Update #TempTransaction                                              
Set ClosingTaxlotID =PTC.ClosingTaxlotID,                                        
CustomOrderBy = 0                                              
from #PM_TaxlotClosing PTC                                                  
Inner Join #PM_Taxlots PT on (PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                        
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                          
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                          
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                     
Inner Join #TempTransaction on #TempTransaction.TaxlotID = PT.TaxlotID                                                
Where (DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) = DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)) And PT.OrderSideTagValue in ('1','A','5','C')                     
And #TempTransaction.Comment is Null                                            
                                
/*            
Special Case:            
If currecnt date sell trade closes one currecnt date and one historical buy (;), in the case, CloseMath will be 's' and comment line (;) has to be deleted            
            
PortfolioCode TranCode Comment      SecType SecuritySymbol TradeDate SettleDate OriginalCostDate Quantity CloseMath            
tech2   by   NULL      csus nptn   08092013 08142013 NULL    20650  NULL            
tech2   ;   LOT:QTY 65:2117.00000000 NULL NULL   08092013 NULL  NULL    0   NULL            
tech2   sl   NULL      csus nptn   08092013 08142013 NULL    22767  s            
            
Technical: collect TaxlotID in a temp table where Comment line (;) has some value i.e. not null and whose parent i.e. sell trade is also closed with .i.e. buy             
trade of same date            
*/            
            
Select                                                                   
#TempTransaction.TaxlotID                                                                  
Into #TempTaxlotIDTable_SpecialCase            
from #PM_TaxlotClosing  PTC                                                
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                  
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                          
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                        
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                     
Inner Join #TempTransaction on #TempTransaction.ClosingTaxlotID = PT1.TaxlotID                   
Where ((DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) = DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)))            
And (#TempTransaction.Comment is not null And #TempTransaction.Comment <> '')                                                                    
            
Delete #TempTransaction            
Where TaxlotID In (Select TaxlotID from #TempTaxlotIDTable_SpecialCase)            
And #TempTransaction.Comment is not null And #TempTransaction.Comment <> ''             
          
-- Add InternalTranID for system use           
Alter Table #TempTransaction          
Add InternalTranID varchar(200) Null          
          
-- #TempTransaction table has all data and could be same taxlotIDs, so assigned a unique ID          
SELECT           
    *,Ranking = DENSE_RANK() OVER(PARTITION BY TAXLOTID ORDER BY TaxlotID, StrategyID ASC),          
    TaxlotID As OriginalTaxlotID          
    Into #TempTransaction_WithRanking          
    FROM #TempTransaction           
    Order By TaxlotID, StrategyID           
          
/* Update Closing trade(s) taxlotID as strategy updated, so taxlotID should be */            
UPDATE #TempTransaction_WithRanking               
SET             
TaxlotID = Substring(TaxlotID, 1, (Len(TaxlotID) - 1)) + Cast (StrategyID As Varchar(10)),             
ClosingTaxlotID = Substring(ClosingTaxlotID, 1, (Len(ClosingTaxlotID) - 1)) + Cast (StrategyID As Varchar(10))                
Where StrategyID <> 0 and InternalTranCode In ('sl','cs')             
            
UPDATE #TempTransaction_WithRanking                                                                      
SET             
ClosingTaxlotID = Substring(ClosingTaxlotID, 1, (Len(ClosingTaxlotID) - 1)) + Cast (StrategyID As Varchar(10))                
Where StrategyID <> 0 and InternalTranCode In ('by','ss')             
            
--Collect Taxlots whose closing methodology is 's' then arrange by TaxlotID, closingID and customOrderBy                
Insert Into #Temptransaction_Final                   
Select * From #TempTransaction_WithRanking                   
Where ClosingTaxlotID in (Select ClosingTaxlotID From #TempTransaction_WithRanking Where CloseMath = 's')                   
Order by CustomOrderBy,TaxlotId,ClosingTaxlotID                
                  
--Collect Taxlots whose closing methodology is not 'S' then arrange by closingID and customOrderBy                  
Insert Into  #Temptransaction_Final                    
Select * From #TempTransaction_WithRanking Where ClosingTaxlotID Not In (Select ClosingTaxlotID From #TempTransaction_WithRanking Where CloseMath = 's')                   
Order by ClosingTaxlotID,CustomOrderBy            
            
/* Update External TransactionID i.e. TranID             
   Update Closing Trade TaxlotID as it is split dynamically             
*/             
            
--Select *          
--Into #Temp_ExternalTransIDSplittedTable_TEMP          
--From #Temp_ExternalTransIDSplittedTable_Ranking          
          
UPDATE #Temp_ExternalTransIDSplittedTable                                                                      
SET             
TaxlotID = Substring(TaxlotID, 1, (Len(TaxlotID) - 1)) + Cast (StrategyID As Varchar(10))                
Where StrategyID <> 0 and Side In ('Sell','Sell to Close','Buy to Close','Buy to Cover')             
            
-- Update TranID i.e. External Transaction ID in the table #Temptransaction_Final          
-- This table TaxlotID is upadted dynamically on the basis of strategy ID          
Update #Temptransaction_Final            
Set TranID = Temp_TranID.ExternalTransID,          
InternalTranID = Temp_TranID.ExternalTransID           
From #Temptransaction_Final            
Inner Join #Temp_ExternalTransIDSplittedTable Temp_TranID On Temp_TranID.TaxlotID = #Temptransaction_Final.TaxlotID            
Where (#Temptransaction_Final.Comment is Null Or #Temptransaction_Final.Comment = '')            
          
-- here we update InternalTranID in table #Temptransaction_Final, on the basis of Original taxlotID and Ranking of each record          
Update #Temptransaction_Final            
Set InternalTranID = Temp_TranID.ExternalTransID            
From #Temptransaction_Final            
Inner Join #Temp_ExternalTransIDSplittedTable_Ranking Temp_TranID On Temp_TranID.OriginalTaxlotID = #Temptransaction_Final.OriginalTaxlotID           
And  Temp_TranID.Ranking = #Temptransaction_Final.Ranking          
Where (#Temptransaction_Final.Comment is Null Or #Temptransaction_Final.Comment = '') And  TranID Is Null           
And InternalTranID Not In (Select InternalTranID From #Temptransaction_Final where InternalTranID is not null)          
          
--Select          
--Temp_MainTable.TaxlotID ,                                                                                           
--ClosingTaxlotID ,                                                                      
--PortfolioCode ,--COL1                               
--TranCode ,                                                                      
--Comment ,                                                                      
--AssetName ,                                                                   
--TickerSymbol ,                                                                      
--TradeDate ,                                                                      
--SettleDate ,                             
--OriginalCostDate ,                                                                      
--Quantity ,                                
--CloseMath ,--10                                                                      
--VersusDate ,                                                                      
--SourceType ,                                                                      
--SourceSymbol ,                                                                      
--TradeDateFXRate ,                                                                      
--SettleDateFXRate ,                                                                      
--OriginalFXRate ,                                                                      
--MarkToMarket ,                                                                      
--TradeAmount ,                                                                      
--OriginalCost ,                                                                      
--Comment1 ,--COL20                                                 
--WithholdingTax ,                                                                      
--Exchange ,                                                                      
--ExchangeFee ,                                                                      
--commission ,                                                                      
--Broker ,                                                                      
--ImpliedComm ,                                                                      
--OtherFees ,                                                                      
--CommPurpose ,                                                                      
--Pledge ,--29                                                                      
--LotLocation ,--COL30                                                                      
--DestPledge ,                                               
--DestLotLocation ,                                                                      
--OriginalFace ,                                                                      
--YieldOnCost ,                                                                      
--DurationOnCost ,--COL35                                                                      
--UserDef1 ,                                                                      
--UserDef2 ,                                                                      
--UserDef3 ,                                                       
--ChildTable.ExternalTransID,--TranID,                                                                      
--IPCounter ,                                                                      
--Repl ,                                                             
--Source ,                                                                      
--Comment2 ,                                   
--OmniAcct ,                                                                      
--Recon ,                                                                      
--Post ,                                                                      
--LabelName ,                                                                      
--LabelDefinition ,                                                                      
--LabelDefinition_Date ,                                                                      
--LabelDefinition_String,--COL50                                                                      
--Comment3 ,                                                                      
--RecordDate ,                                                                    
--ReclaimAmount ,                                
--Strategy ,                                                                      
--Comment4 ,                                                                      
--IncomeAccount ,                                                 
--AccrualAccount ,                                                                      
--DivAccrualMethod ,                                                                      
--PerfContributionOrWithdrawal ,--59                                                                      
----System internal use only                                              
--CustomOrderBy ,                                                
--Level1AllocationID ,                                                
--3 as TaxLotStateID ,                                              
--FromDeleted ,                                            
--InternalTranCode,            
--Temp_MainTable.StrategyID ,            
--FundID ,            
--ExchangeID ,            
--PutOrCall ,          
--ChildTable.ExternalTransID as InternalTranID ,          
--Temp_MainTable.Ranking ,          
--Temp_MainTable.OriginalTaxlotID           
--           
--Into #TempStrategyWind_UnwindRecords          
--from #Temptransaction_Final Temp_MainTable          
--Inner Join #Temp_ExternalTransIDSplittedTable_Ranking ChildTable On ChildTable.OriginalTaxlotID = Temp_MainTable.OriginalTaxlotID           
--And Temp_MainTable.InternalTranID <> ChildTable.ExternalTransID          
--Where Temp_MainTable.TaxLotStateID = 2 And Temp_MainTable.Comment Is null           
------and ChildTable.ExternalTransID Not In (Select InternalTranID From #Temptransaction_Final )           
------MainTable.InternalTranID Not In (Select ExternalTransID From #Temp_ExternalTransIDSplittedTable_Ranking )          
--          
--Insert Into #Temptransaction_Final          
--Select * from #TempStrategyWind_UnwindRecords          
----Where InternalTranID Not In (Select InternalTranID From #Temptransaction_Final where InternalTranID is not null)          
--          
--Delete #Temptransaction_Final          
--Where TaxlotStateID = 3 And InternalTranID In (Select TranID From #Temptransaction_Final Where TranID Is Not Null And TaxlotStateID = 2)          
            
Select            
CustomOrderBy,                                     
TaxlotID,                                              
ClosingTaxlotID,                                             
InternalTranCode,                                                                     
PortfolioCode, --1                                             
TranCode,                                                                      
Comment,                                                      
AssetName As SecType,                                                                      
TickerSymbol As SecuritySymbol,                                                                      
TradeDate ,                                                                      
SettleDate ,                                                                      
OriginalCostDate,                                                                      
Quantity ,                                                                      
CloseMath,--10               
VersusDate,                                                                      
SourceType ,                                                       
SourceSymbol ,                                                                   
TradeDateFXRate ,                                                                      
SettleDateFXRate ,                                                                      
OriginalFXRate ,                                                                      
MarkToMarket ,                                                                      
TradeAmount ,                                                                      
OriginalCost ,                                                          
Comment1 ,--COL20                                                                      
WithholdingTax ,                                                                      
Exchange ,     
ExchangeFee ,                                                                      
commission ,                                                               
Broker ,                                                                      
ImpliedComm ,                                                                      
OtherFees ,                                                   
CommPurpose ,                                                                      
Pledge ,--29                                                                      
LotLocation ,--COL30                                                                      
DestPledge ,                                                                      
DestLotLocation ,                                                                      
OriginalFace ,                                                                      
YieldOnCost ,                                               
DurationOnCost ,--COL35                                                                      
UserDef1 ,                                                                      
UserDef2 ,                                                                      
UserDef3 ,                                                  
TranID ,                                                                      
IPCounter ,                                                                      
Repl ,                                                                      
Source ,                                                                      
Comment2 ,                                       
OmniAcct ,                                                                      
Recon ,                                                                      
Post ,                                                                      
LabelName ,                                                                      
LabelDefinition ,                                                                
LabelDefinition_Date ,                                                              
LabelDefinition_String ,--COL50                                                                      
Comment3 ,                                                                      
RecordDate ,                                                                      
ReclaimAmount ,                                                                      
Strategy ,                          
Comment4 ,                                                                      
IncomeAccount ,                                                                      
AccrualAccount ,                                                                      
DivAccrualMethod ,                                                                      
PerfContributionOrWithdrawal,--59                                
Level1AllocationID,                                      
TaxLotStateID,                                              
FromDeleted  ,          
InternalTranID                                                                    
From #Temptransaction_Final            
Order by TableID                               
                                                                      
Drop Table #TempTransaction,#TempClosingIDTable,#Temptransaction_Final,#TempClosingIDTable_WithoutLotIDTrades                                                                  
Drop Table #FXConversionRates,#SecMasterDataTempTable,#TempTaxlotState,#TempClosingIDTableToChangedMath             
Drop Table #PM_Taxlots,#V_Taxlots,#PM_TaxlotClosing,#T_Group,#T_Level2Allocation,#TempTaxlotIDTable_SpecialCase            
Drop Table #ClosingTradeInfoTable,#TaxlotIDAndStrategyIDGroupingTable,#TempTransaction_BeforeStrategy            
--Drop Table #TempStrategyWind_UnwindRecords            
Drop Table #Temp_TaxlotAndExternalTransIDTable,#Temp_SplittedWithComma,#Temp_ExternalTransIDSplittedTable          
Drop Table #Temp_ExternalTransIDSplittedTable_Ranking,#TempTransaction_WithRanking


