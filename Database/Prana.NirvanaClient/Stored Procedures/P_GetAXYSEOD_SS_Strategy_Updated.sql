/*                                                                                            
Author: Sandeep Singh                                                                                            
Date: Dec 05,2012                                                                                            
Desc: Customized EOD file for SS - AXYS                                                                                            
EXEC:                                                                                            
[P_GetAXYSEOD_SS_Strategy_Updated] 26,'2,6,3,1,5,4','2015-06-23',2,'77,63,43,59,18,61,74,1,15,62,73,80,81',0,0,23 

Modified By: Sandeep
Date: 06 July 2015
http://jira.nirvanasolutions.com:8080/browse/CI-1118                                                                                          
*/ 

CREATE Procedure [dbo].[P_GetAXYSEOD_SS_Strategy_Updated]                                                                                            
(                                                                                            
 @ThirdPartyID int,                                                                                                                                                                                            
 @CompanyFundIDs varchar(max),                                                                                                                                                                                            
 @InputDate datetime,                                                                                                                                                                                        
 @CompanyID int,                                                                                                                                                        
 @AuecIDs varchar(max),                                                                                              
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                                                             
 @DateType int ,                                
 @fileFormatID int                                       
) with Recompile                                                                                                            
As                                   
                                                    
--Declare @ThirdPartyID int                                                                                                                                                                                            
--Declare @CompanyFundIDs varchar(max)                                                                      
--Declare @InputDate datetime                                                                      
--Declare @CompanyID int                                                                      
--Declare @AuecIDs varchar(max)                                                                      
--Declare @TypeID int                                  
--Declare @DateType int                                   
--Declare @fileFormatID int                                                      
                                  
--Set @ThirdPartyID =  8                                                    
--Set @CompanyFundIDs = N'1,2,3,4,5,6'
--Set @InputDate = '2021-05-19'        
--Set @CompanyID = 2                                                  
--Set @AuecIDs =   '15,63,43,81,44,1,18,61,73,62,59,53,77,31,34,20,80,74'                                      
--Set @TypeID = 0                                   
--Set @DateType = 0                                  
--Set @fileFormatID = 23              
                                  
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
Set @BaseCurrencyID=(select Top 1 BaseCurrencyID from T_Company)                                                                
                                                                                          
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
  AssetName Varchar(50),                                                     
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
  SedolSymbol Varchar(50),                                               
  ISINSymbol Varchar(50),                                                                                                                     
  CusipSymbol Varchar(50),                                                                  
  OSISymbol Varchar(50),                                                                                                                      
  IDCOSymbol Varchar(50)                                                                                                                           
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
Create Table #T_Group
(
GroupTableId Int Identity(1,1) not null,
GroupID nvarchar(100),                                                
OrderSideTagValue nchar(20), 
AUECID Int,                                 
AUECLocalDate DateTime 
)
 
Insert InTo #T_Group                                             
Select                                                 
GroupID,                                                
OrderSideTagValue,                                                
AUECID,                                                
AUECLocalDate                                                
From T_Group with (nolock)  

CREATE NONCLUSTERED INDEX [index_nonclustered_GroupID] ON [dbo].[#T_Group]
(
	[GroupID] ASC
)
INCLUDE ([AUECID], [OrderSideTagValue],	[AUECLocalDate]) 
  
  --Select * from #T_Group                                              
--select required fields from pm_taxlots
Create Table #PM_Taxlots
(
TaxlotTableId Int Identity(1, 1) not null,
TaxlotID Varchar(50),                                                
TaxLotClosingId_Fk uniqueidentifier,                                                
LotId Varchar(200),                                               
GroupID nvarchar(100),                                                
FundID Int,                                        
OrderSideTagValue nchar(20),                                  
ExternalTransId Varchar(200) 
)
   
Insert InTo #PM_Taxlots                                             
Select                                                 
TaxlotID,                                                
TaxLotClosingId_Fk,                                                
LotId,                                                
GroupID,                                                
FundID,                                        
OrderSideTagValue,                                  
ExternalTransId                                                
from PM_Taxlots with (nolock)   

CREATE NONCLUSTERED INDEX [index_nonclustered_PMTaxGroupID] ON [dbo].[#PM_Taxlots]
(
	[GroupID] ASC,
	[TaxlotID] Asc	
)                 
                                          
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
from T_Level2Allocation with (nolock)                                                  
                                  
Select *                                              
into #V_Taxlots                                             
from V_Taxlots with (nolock)                                                  
                                  
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
Select                                   
	b.Items as SplitValue                                  
	,TaxlotID                                  
	,OrderSideTagValue                                  
                                  
From CTE a                                  
Cross apply dbo.Split(a.ExternalTransID,',') b                                   
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
                         
Select *                   
Into #Temp_ExternalTransIDSplitted_OriginalTaxlotID                
from #Temp_ExternalTransIDSplittedTable                        
                           
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
Exchange Varchar(100),                 
ExchangeFee Float,                                                                                            
commission Float,                                                                                            
Broker Varchar(100),                                                                                            
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
Level1AllocationID Varchar(100),                                                                      
TaxLotStateID Int,                                                                    
FromDeleted Varchar(50),                                              
InternalTranCode varchar(20) Null,                                  
FundID Int,                                  
ExchangeID Int,                                  
PutOrCall Varchar(10)                                  
)  

Create Table #TempTransaction_BeforeStrategy_Mediatory                                                                                            
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
Exchange Varchar(100),                 
ExchangeFee Float,                                                                                            
commission Float,                                                                                            
Broker Varchar(100),                                                                                            
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
Level1AllocationID Varchar(100),                                                                      
TaxLotStateID Int,                                                                    
FromDeleted Varchar(50),                                              
InternalTranCode varchar(20) Null,                                  
FundID Int,                                  
ExchangeID Int,                                  
PutOrCall Varchar(10),
Open_CloseTag Varchar(5), 
OpeningDate DateTime,
ClosingDate DateTime, 
OpeningTaxlotState Int, 
ClosingTaxlotState Int,
OpeningTaxlotFormatID Int, 
ClosingTaxlotFormatID Int                                  
)                                 
                                  
Create Table #TempTransaction                                                                                            
(                                                              
TaxlotID Varchar(50),                                                                                                                 
ClosingTaxlotID Varchar(50),                                    
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
Level1AllocationID Varchar(50),                                                                      
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
TaxlotID Varchar(50),                                                                                                                 
ClosingTaxlotID Varchar(50),                                                           
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
Level1AllocationID Varchar(50),                                                                      
TaxLotStateID Int,                                                                    
FromDeleted Varchar(50),                                    
InternalTranCode varchar(20) Null,                                  
StrategyID Int,                                  
FundID Int,                                  
ExchangeID Int,                                  
PutOrCall Varchar(10),                     
InternalTranID varchar(50),                                
Ranking int,                                
OriginalTaxlotID Varchar(50)                                       
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
Case
	When VT.CurrencyID = 7
	Then ((Round(AvgPrice,7) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * SideMultiplier))
	Else ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * SideMultiplier))
End As TradeAmount,               
              
NULL As OriginalCost,                                                                                            
NULL As Comment1,--COL20                                                                                            
NULL As WithholdingTax,                                                               
Null As Exchange,                                                                                            
StampDuty As ExchangeFee,                                      
VT.Commission As commission,                                   
Lower(T_CounterParty.ShortName) As Broker,                                                                                            
'n' As ImpliedComm,                                         
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
VT.Level1AllocationID As Level1AllocationID,                                                                    
0 As TaxLotStateID,                                                                   
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
Inner Join T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                                                                                            
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                                                                                          
Inner Join @Fund F On F.FundID = VT.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID                                                                                             
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = VT.Symbol                                                                                           
Left Outer Join T_CounterParty On VT.CounterPartyID = T_CounterParty.CounterPartyID                                                                                            
WHERE DateDiff(DAY, (
			CASE 
				WHEN @DateType_LocalVar = 1
				THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
			END
			), @InputDate_LocalVar) >= 0 
			--And DateDiff(DAY, VT.AUECLocalDate,'11-15-2017') <> 0 
			
  
UPDATE T
SET T.TaxLotStateID = PB.TaxlotState
FROM #TempTransaction_BeforeStrategy T
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = T.TaxlotID
WHERE PB.TaxlotState <> 0
	AND PB.FileFormatID = @fileFormatID
	      
Delete From T
From #TempTransaction_BeforeStrategy T
Inner Join #V_Taxlots VT On VT.TaxLotID = T.TaxlotID
Where (
		(DateDiff(Day,VT.AUECLocalDate,@InputDate_LocalVar) <> 0 And (TaxLotStateID In (1,4)))
		Or (DateDiff(DAY,VT.AUECLocalDate,@InputDate_LocalVar) < 0 And TaxLotStateID in (0,2,3))   
		)  

ALTER TABLE #TempTransaction_BeforeStrategy
Add Open_CloseTag Varchar(5), OpeningDate DateTime, ClosingDate DateTime, OpeningTaxlotState Int, ClosingTaxlotState Int,
OpeningTaxlotFormatID Int, ClosingTaxlotFormatID Int
                                   
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
'' As PutOrCall,
'C' As Open_CloseTag,
G.AUECLocalDate OpeningDate,
G1.AUECLocalDate As ClosingDate,
0 As OpeningTaxlotState,
0 As ClosingTaxlotState,
Null As OpeningTaxlotFormatID,
Null As ClosingTaxlotFormatID                                                                   

From #PM_TaxlotClosing  PTC                                                                                  
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                                                                      
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk) 
Inner Join @Fund F On F.FundID = PT.FundID                                                                                              
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                                                                                         
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                                                                            
Inner Join @AUECID AUEC On AUEC.AUECID = G.AUECID
Inner Join T_Side on T_Side.SideTagValue=G.OrderSideTagValue


UPDATE T
SET T.OpeningTaxlotState = PB.TaxlotState,
T.OpeningTaxlotFormatID = @fileFormatID
FROM #TempTransaction_BeforeStrategy T
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = T.TaxlotID
WHERE PB.TaxlotState <> 0
	AND PB.FileFormatID = @fileFormatID 
	And Open_CloseTag = 'C'                                               
         
UPDATE T
SET T.ClosingTaxlotState = PB.TaxlotState,
T.ClosingTaxlotFormatID = @fileFormatID
FROM #TempTransaction_BeforeStrategy T
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = T.ClosingTaxlotID
WHERE PB.TaxlotState <> 0
	AND PB.FileFormatID = @fileFormatID 
	And Open_CloseTag = 'C' 

/*
Get Closing Data based on below criteria and keep in a temp table #TempTransaction_BeforeStrategy_Mediatory
1)  If Selected date from UI i.e. @InputDate_LocalVar is equals to Closing Date i.e. ClosingDate and 
    And Opening and Closing Taxlots have different trade date i.e. both open on different date
	And Opening Taxlot has Taxlot State = 1 i.e already generated

2)   If Selected date from UI i.e. @InputDate_LocalVar is equals to Closing Date i.e. ClosingDate and 
    And Opening and Closing Taxlots have different trade date i.e. both open on different date
	And Opening Taxlot has Taxlot State is 0 (not generated) or 2 (amended)
*/

Insert InTo #TempTransaction_BeforeStrategy_Mediatory
Select *
From #TempTransaction_BeforeStrategy
Where 
Open_CloseTag = 'C' 
And 
	(
		OpeningTaxlotFormatID = @fileFormatID And ClosingTaxlotFormatID = @fileFormatID
		And (  
				OpeningTaxlotState = 1
				And (DateDiff(d,@InputDate_LocalVar,ClosingDate) = 0 And (DateDiff(d,OpeningDate,ClosingDate) <> 0))
			)
	)
Or
	( 
		OpeningTaxlotFormatID = @fileFormatID 
		And ((ClosingTaxlotFormatID Is Null And ClosingTaxlotState = 0) Or (ClosingTaxlotFormatID = @fileFormatID And ClosingTaxlotState = 2))
		And (  
				ClosingTaxlotState In (0,2) And (DateDiff(d,ClosingDate,@InputDate_LocalVar) >= 0 And (DateDiff(d,OpeningDate,ClosingDate) <> 0))
			) 
	)                                                                       
 
----Insert InTo #TempTransaction_BeforeStrategy_Mediatory0
----Select *
----From #TempTransaction_BeforeStrategy
----Where Open_CloseTag = 'C' 
----And OpeningTaxlotFormatID = @fileFormatID And 
----((ClosingTaxlotFormatID Is Null And ClosingTaxlotState = 0) Or (ClosingTaxlotFormatID = @fileFormatID And ClosingTaxlotState = 2))
----And (  
----ClosingTaxlotState In (0,2)
----And (DateDiff(d,ClosingDate,@InputDate_LocalVar) >= 0 
----And (DateDiff(d,OpeningDate,ClosingDate) <> 0)                                                                                        
---- )
----) 

Delete From #TempTransaction_BeforeStrategy
Where Open_CloseTag = 'C' 

ALTER TABLE #TempTransaction_BeforeStrategy
DROP COLUMN Open_CloseTag, OpeningDate, ClosingDate, OpeningTaxlotState,ClosingTaxlotState, OpeningTaxlotFormatID,ClosingTaxlotFormatID

ALTER TABLE #TempTransaction_BeforeStrategy_Mediatory
DROP COLUMN Open_CloseTag, OpeningDate, ClosingDate, OpeningTaxlotState,ClosingTaxlotState, OpeningTaxlotFormatID,ClosingTaxlotFormatID

Insert InTo #TempTransaction_BeforeStrategy
Select Distinct * From #TempTransaction_BeforeStrategy_Mediatory

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

Case
	When DT.CurrencyID = 7
	Then                           
CASE                                                                       
WHEN (OrderSideTagValue IN ('A', 'B', '1'))                                                             
	THEN ((Round(AvgPrice,7) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))                                     
	WHEN (OrderSideTagValue IN ('2', '5', '6', 'C', 'D'))                                                              
	Then ((Round(AvgPrice,7) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees  + SecFee + OccFee + OrfFee + ClearingBrokerFee) * -1))                                    
	ELSE ((Round(AvgPrice,7) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))                                      
END  
Else
CASE                                                                                 
	WHEN (OrderSideTagValue IN ('A', 'B', '1'))                                                                                 
	THEN ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))                                    
	WHEN (OrderSideTagValue IN ('2', '5', '6', 'C', 'D'))                                                                       
	Then ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees  + SecFee + OccFee + OrfFee + ClearingBrokerFee) * -1))                                  
	ELSE ((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1))                                  
END			
End As TradeAmount,                                                                                
NULL As OriginalCost,                                                                          
NULL As Comment1,--COL20                                                                                            
NULL As WithholdingTax,                                                                                            
Null As Exchange,                                              
DT.StampDuty As ExchangeFee,                                          
DT.Commission As commission,                                                                                            
Lower(T_CounterParty.ShortName) As Broker,                                                                        
'n' As ImpliedComm,                                                                               
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
From T_DeletedTaxlots DT  With (NoLock)                                                            
Inner Join T_Side on T_Side.SideTagValue=DT.OrderSideTagValue                                                                                            
Inner Join T_Asset on T_Asset.AssetID=DT.AssetID                                                                                              
Inner Join @Fund F On F.FundID = DT.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = DT.AUECID                                                                                                               
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = DT.Symbol                                                                                           
Left Outer Join T_CounterParty On DT.CounterPartyID = T_CounterParty.CounterPartyID                                                                                            
--FX Rate for Trade Date from Daily Valuation                                                                                      
Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                          
On (FXDayRatesForTradeDate.FromCurrencyID = DT.CurrencyID                                             
And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID          
And DateDiff(d,DT.AUECLocalDate,FXDayRatesForTradeDate.Date)=0)                                                       
Where DateDiff(d,DT.AUECLocalDate,@InputDate_LocalVar) >= 0 
And DT.TaxlotState = 3 and DT.FileFormatID = @fileFormatID  
---- Update TaxlotState and Level1 ID that is used for system internal use only   

                                                                 
Update #TempTransaction_BeforeStrategy                                                         
Set Level1AllocationID = VT.Level1AllocationID,                                                                      
TaxLotStateID = T_PBWiseTaxlotState.TaxLotState                                                                      
From #TempTransaction_BeforeStrategy                                                                       
Inner Join #V_Taxlots VT On VT.TaxlotID = #TempTransaction_BeforeStrategy.TaxlotID                                                                      
Inner Join T_PBWiseTaxlotState With (Nolock) on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID                                    
Where T_PBWiseTaxlotState.FileFormatID = @fileFormatID           
                                  
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
                                  
----Deleted taxlots                       
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
Temp_BeforeStrategy.CloseMath ,--10                                         
Temp_BeforeStrategy.VersusDate,                                                                                            
Temp_BeforeStrategy.SourceType ,                                                                                            
Temp_BeforeStrategy.SourceSymbol,                                                                                            
Temp_BeforeStrategy.TradeDateFXRate ,                                                                                            
Temp_BeforeStrategy.SettleDateFXRate ,                             
Temp_BeforeStrategy.OriginalFXRate ,                                                                                            
Temp_BeforeStrategy.MarkToMarket ,                                  
Temp_BeforeStrategy.TradeAmount,                                  
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
Left Outer Join #Temp_ExternalTransIDSplittedTable StrategyTable On StrategyTable.TaxlotID = Temp_BeforeStrategy.TaxlotID                                  
Where Temp_BeforeStrategy.TaxlotStateID = 3                                  
                                  
----Select * from #TempTransaction_BeforeStrategy                                   
                                  
Update #TempTransaction                                  
Set                                  
PortfolioCode =                                   
 Case                                                                                            
  When FundName = 'SSFQP MS'                                                    
 Then 'ssfqp'                                                                    
 When FundName = 'Cayman MS'                                                    
 Then 'cay'                          
 When FundName = 'PE MS'                                                    
 Then 'sspe'                                                                    
 When FundName = 'Tech MS'       
 Then 'techn'                                                                    
 When FundName = 'Tech2 MS'                                                    
 Then 'tech2'          
 When FundName = 'LS MS'                                                    
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
  When (AssetName = 'PrivateEquity' And CharIndex('_W',TickerSymbol) <> 0  Or     
  (AssetName = 'Equity' And TickerSymbol = 'EYEGW') Or (AssetName = 'PrivateEquity' And TickerSymbol In ('SRTSW','OPTTW','OPTTW2','VERBW','FLWRW.CN') ))                                                                 
  Then 'wtus'                                             
  --Units as it is PrivateEquity in our application                                            
  When (AssetName = 'PrivateEquity' And CharIndex('_U',TickerSymbol) <> 0) Or (AssetName = 'Equity' And TickerSymbol In ('NVEEU','SRTSU'))                                                                            
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
  When (AssetName = 'Equity' And TickerSymbol = 'XBI')                                                                                                     
  Then 'efus'                                                      
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
 When (CHARINDEX('-XET',TickerSymbol) > 0 )                                                                                                      
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('-',TickerSymbol)-1) + '.GR')                                   
 When (CHARINDEX('-LON',TickerSymbol) > 0 )                                                                                                          
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('-',TickerSymbol)-1) + '.LN')        
 When (CHARINDEX('-EEB',TickerSymbol) > 0 )                                                                                                          
 Then LOWER(SUBSTRING(TickerSymbol,1,CHARINDEX('-',TickerSymbol)-1) + '.FP')                                      
 Else LOWER(TickerSymbol)                                                                                            
End,                                    
                                  
--TranCode has side value                                  
--25 Jan 2013: Request by Korey: short sells (trans code �ss�), please make SourceType (column 12) �awus� (instead of �caus�),                                                   
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
  When CS.StrategyName = '(Rest/Reg) Lot 1'                                                      
 Then '1'                                                      
 When CS.StrategyName = '(Rest/Reg) Lot 2'                                                      
 Then '2'                                                      
 When CS.StrategyName = '(Rest/Reg) Lot 3'                                                      
 Then '3'                                                  
 When CS.StrategyName = '(Rest/Reg) Lot 4'                           
 Then '4'                                                      
 When CS.StrategyName = '(Rest/Reg) Lot 5'                       
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
 When CS.StrategyName = 'CB (R/R) Lt 1'                                              
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
  When CS.StrategyName = '(Rest/Reg) Lot 1'                                                      
 Then '1'                                                      
 When CS.StrategyName = '(Rest/Reg) Lot 2'        
 Then '2'                                         
 When CS.StrategyName = '(Rest/Reg) Lot 3'                                                      
 Then '3'                       
 When CS.StrategyName = '(Rest/Reg) Lot 4'                           
 Then '4'                                                      
 When CS.StrategyName = '(Rest/Reg) Lot 5'                                                      
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
 When CS.StrategyName = 'CB (R/R) Lt 1'                                              
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
                                  
--/*                                   
--Case: Update Closing  Methodology i.e. CloseMath ='s'                                   
--when a trade opens and closes on same date                                  
--*/                                  
                                                                
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

/* 
Added by Sandeep to optimize this sp
Insert grouped dada in a temp table #TempTesting and use in the join 
Also remove group by and having from the main query
*/
Create Table #TempTesting
(
TableId int identity(1, 1) not null,
ClosingTaxlotID Varchar(100)
)

Insert InTo #TempTesting
Select 
#TempTransaction.ClosingTaxlotID
From #TempTransaction
Group By #TempTransaction.ClosingTaxlotID                                                                                        
having Count(#TempTransaction.ClosingTaxlotID) > 1 

CREATE NONCLUSTERED INDEX [index_nonclustered_ClosingID] ON [dbo].[#TempTesting]
(
	[ClosingTaxlotID] ASC
)
              
Create Table #TempClosingIDTable
(
TableId int identity(1, 1) not null,
ClosingTaxlotID Varchar(100)
)
 
 Insert Into #TempClosingIDTable    
Select                                                                                           
Distinct
#TempTesting.ClosingTaxlotID                                                                                   
from #PM_TaxlotClosing  PTC                                                                      
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                                                                    
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk) 
Inner Join #TempTesting on #TempTesting.ClosingTaxlotID = PT1.TaxlotID                                                    
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                                              
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                           
Where ((DateDiff(d,@InputDate_LocalVar,G.AUECLocalDate) <> DateDiff(d,@InputDate_LocalVar,G1.AUECLocalDate)))   
                             
                                                         
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
and Taxlotid != ClosingTaxlotID                                   
                                  
----------------------------------------------------------------------------                
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
                                
Select              
TaxlotId,                
ExternalTransID,                 
TaxlotId + '|' + Cast(ExternalTransID as NVarchar) As TaxlotAndExternalTransID                 
INTO #Temp_TaxlotAndExternalTransID                 
From #Temp_ExternalTransIDSplitted_OriginalTaxlotID                 
Where Side Not In ('Buy','Buy to Open' ,'Sell short' ,'Sell to Open')                
                  
SELECT                 
OriginalTaxlotId,InternalTranId,                 
OriginalTaxlotId + '|' + Cast(InternalTranId as nvarchar) As TaxlotAndExternalTransID                 
INTO #Temp_TaxlotAndExternalTransID_2                 
From #Temptransaction_Final Where Comment Is Null                
                
Select              
TaxlotId,                
ExternalTransID                 
InTo #Temp1                 
FROM #Temp_TaxlotAndExternalTransID where TaxlotAndExternalTransID NOT in(SELECT TaxlotAndExternalTransID from #Temp_TaxlotAndExternalTransID_2 Where TaxlotAndExternalTransID Is Not Null)                
                  
Select Distinct                   
#Temp1.TaxlotID,                
#Temp1.ExternalTransID,                
PortfolioCode,                  
TranCode,                  
AssetName As SecType,                  
TickerSymbol As SecuritySymbol,                  
Level1AllocationID,                  
TableID                  
InTo #Temp2                    
from #Temp1                
Inner Join #Temptransaction_Final On #Temptransaction_Final.OriginalTaxlotID = #Temp1.TaxlotID                
                             
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
Into #Temp_Output                                                                                     
From #Temptransaction_Final                           
Order by TableID                    
                  
Insert Into #Temp_Output                
Select                                
0,                                                         
TaxlotID,                                                                  
Null As ClosingTaxlotID,                                                                 
TranCode As InternalTranCode,                                                                                         
PortfolioCode, --1                                                                 
TranCode,                                                                                          
Null As Comment,                                            
SecType,                                                                                          
SecuritySymbol,                                                                                          
Null As TradeDate ,                                                                              
Null As SettleDate ,                                                                                          
Null As OriginalCostDate,                                                                                          
0 As Quantity ,                                                                                          
Null As CloseMath,--10                                   
Null As VersusDate,                              
Null As  SourceType ,                                                                           
Null As SourceSymbol ,                                                                                       
Null As TradeDateFXRate ,                                                                                          
Null As SettleDateFXRate ,                                          
Null As OriginalFXRate ,                                                                                          
Null As MarkToMarket ,                                                                                          
Null As TradeAmount ,                                                                                          
Null As OriginalCost ,                                                                              
Null As Comment1 ,--COL20                                                                                          
Null As WithholdingTax ,                                                                                          
Null As Exchange ,                         
Null As ExchangeFee ,                       
Null As commission ,                                                                                   
Null As Broker ,                                                                                          
Null As ImpliedComm ,                                                                                          
Null As OtherFees ,                                                                       
Null As CommPurpose ,                                                                                          
Null As Pledge ,--29                                              
Null As LotLocation ,--COL30                   
Null As DestPledge ,                                                                                          
Null As DestLotLocation ,                                                                                          
Null As OriginalFace ,                 
Null As YieldOnCost ,                                                                   
Null As DurationOnCost ,--COL35                                                                                          
Null As UserDef1 ,                                    
Null As UserDef2 ,                                                                                          
Null As UserDef3 ,                                                                      
ExternalTransID As TranID ,                                                                                          
Null As IPCounter ,                                   
Null As Repl ,                                                                                          
Null As Source ,                                                                                          
Null As Comment2 ,                                                           
Null As OmniAcct ,                                                                                          
Null As Recon ,                                                                                          
'y' As Post ,           
Null As LabelName ,                                                                                          
Null As LabelDefinition ,         
Null As LabelDefinition_Date ,                                                                                  
Null As LabelDefinition_String ,--COL50                                                                                          
Null As Comment3 ,                                                                                          
Null As RecordDate ,                                                                                          
Null As ReclaimAmount ,                                                                                          
Null As Strategy ,                                              
Null As Comment4 ,                                                                                          
Null As IncomeAccount ,                                                                                    
Null As AccrualAccount ,                 
Null As DivAccrualMethod ,                                                                                          
Null As PerfContributionOrWithdrawal,--59                                                    
Level1AllocationID,                                                          
'3' As TaxLotStateID,                                                                  
'Yes' As FromDeleted  ,                              
ExternalTransID as InternalTranID                                                                                        
From #Temp2                
Where ExternalTransID <> '' and ExternalTransID Is Not Null             

Select * from #Temp_Output                                               
Order by
	Case 
	When ClosingTaxlotID in (Select ClosingTaxlotID From #TempTransaction_WithRanking Where CloseMath = 's') 
	Then Cast(CustomOrderBy As Varchar(5))+TaxlotId+ClosingTaxlotID 
	When ClosingTaxlotID Not In (Select ClosingTaxlotID From #TempTransaction_WithRanking Where CloseMath = 's')                       
	Then ClosingTaxlotID+Cast(CustomOrderBy As Varchar(5)) 
	End                                              
 
DROP INDEX [index_nonclustered_GroupID] ON [#T_Group] , [index_nonclustered_PMTaxGroupID] ON [#PM_Taxlots] ,[index_nonclustered_ClosingID] ON [dbo].[#TempTesting]
Drop Table #Temp_TaxlotAndExternalTransID_2,#Temp_TaxlotAndExternalTransID,#Temp_ExternalTransIDSplitted_OriginalTaxlotID,#Temp1,#Temp2,#Temp_Output                                                                                    
Drop Table #TempTransaction ,#TempClosingIDTable,#Temptransaction_Final,#TempClosingIDTable_WithoutLotIDTrades
Drop Table #FXConversionRates,#SecMasterDataTempTable,#TempClosingIDTableToChangedMath,#TempTaxlotState                                
Drop Table #PM_Taxlots,#V_Taxlots,#PM_TaxlotClosing,#T_Group,#T_Level2Allocation,#TempTaxlotIDTable_SpecialCase                                
Drop Table #ClosingTradeInfoTable,#TaxlotIDAndStrategyIDGroupingTable,#TempTransaction_BeforeStrategy                                
Drop Table #Temp_TaxlotAndExternalTransIDTable,#Temp_SplittedWithComma,#Temp_ExternalTransIDSplittedTable                              
Drop Table #Temp_ExternalTransIDSplittedTable_Ranking,#TempTransaction_WithRanking,#TempTesting, #TempTransaction_BeforeStrategy_Mediatory 