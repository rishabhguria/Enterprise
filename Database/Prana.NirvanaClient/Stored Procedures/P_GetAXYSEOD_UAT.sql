/*                                                    
Author: Sandeep Singh                                                    
Date: Dec 05,2012                                                    
Desc: Customized EOD file for SS - AXYS                                                    
EXEC:                                                    
[P_GetAXYSEOD_UAT] 27,'1184,1183,1182,1186,1185','2013-04-04',5,'20,69,65,30,28,27,21,1,15,19',0                                                   
*/                                                    
                                                    
CREATE Procedure [dbo].[P_GetAXYSEOD_UAT]                                                    
(                                                    
@thirdPartyID int,                                                                                                                                                    
@companyFundIDs varchar(max),                                                                                                                                                    
@inputDate datetime,                                                                                                                                                
@companyID int,                                                                                                                
@auecIDs varchar(max),                                                      
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                         
@dateType int  -- 0 for process date and 1 for AUEClocalDate i.e. Fetch data based on process date (0) or AUEClocalDate (1)     
)                                                      
As                                          
--                                                 
--Declare @thirdPartyID int                                                                                                                                                    
--Declare @companyFundIDs varchar(max)                              
--Declare @inputDate datetime                              
--Declare @companyID int                              
--Declare @auecIDs varchar(max)                              
--Declare @TypeID int                              
--Set @thirdPartyID = 27                                               
--Set @companyFundIDs = '1184,1183,1182,1186,1185'                              
--Set @inputDate = '2013-04-01'                              
--Set @companyID = 5                              
--Set @auecIDs = N'1'                              
--Set @TypeID = 0                              
--                              
--                                
Declare @Fund Table                                                                                                              
(                                                                                                              
FundID int                                                                                                          
)                                                             
                                                            
Declare @AUECID Table                                                                                                              
(                                                                                                              
AUECID int                                                                                                          
)                                             
                                      
Insert into @Fund                                                            
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')                                                               
                                                            
Insert into @AUECID                                                            
Select Cast(Items as int) from dbo.Split(@auecIDs,',')                                 
                                                  
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
            
-------------------------------------Added by Narendra-----------------------------        
--Insert data into temp table and then perform operations        
--select required fields from t_group        
select         
GroupID,        
OrderSideTagValue,        
AUECID,        
AUECLocalDate        
into #T_Group        
from T_Group        
        
        
--select required fields from pm_taxlots        
select         
TaxlotID,        
TaxLotClosingId_Fk,        
LotId,        
GroupID,        
FundID,        
OrderSideTagValue        
into #PM_Taxlots        
from PM_Taxlots        
        
--select required fields from pm_taxlotclosing        
select        
TaxLotClosingId,        
PositionalTaxlotID,        
ClosingTaxlotID,        
ClosedQty,        
AUECLocalDate        
into #PM_TaxlotClosing        
from PM_TaxlotClosing        
      
--select required fields from level2allocation      
select        
TaxlotId,      
LotId       
into #T_Level2Allocation        
from T_Level2Allocation              
--select all fields from v_taxlots        
select *        
into #V_Taxlots        
from V_Taxlots        
---------------------------------------------------------------------------------        
                                              
                             
Create Table #TempTransaction                                                    
(                               
TaxlotID Varchar(200),                                                                         
ClosingTaxlotID Varchar(200),                                                    
PortfolioCode Varchar(200),             
TranCode Varchar(20),                                                    
Comment Varchar(2000),                                                    
SecType Varchar(20),                                                 
SecuritySymbol Varchar(200),                                                    
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
SoftCommission Varchar(200)
)                            
                                                    
Create Table #TempTransaction_Final                                                          
(      
TableID [bigint] IDENTITY(1,1) NOT NULL,                             
TaxlotID Varchar(200),                                                                               
ClosingTaxlotID Varchar(200),                                                          
PortfolioCode Varchar(200),                   
TranCode Varchar(20),                                                          
Comment Varchar(2000),                                                          
SecType Varchar(20),                                                       
SecuritySymbol Varchar(200),                                                          
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
SoftCommission Varchar(200)                                                           
)                                   
                                                         
                                                   
                                                    
Insert Into #TempTransaction                                                    
                                                    
Select                                                     
VT.TaxlotID As TaxlotID,                               
VT.TaxlotID As ClosingTaxlotID,                                                    
Case                                                    
 When FundName = 'OFFSHORE'                                                    
 Then 'ssfqp'                                                    
 When FundName = 'LP C/O'                                                    
 Then 'cay'                                                    
 When FundName = 'Fund1'                                                    
 Then 'sspe'                                                    
 When FundName = 'Fund2'                                                    
 Then 'techn'                                                    
 When FundName = 'Fund With Long Name'               
 Then 'tech2'                                                    
 When FundName = 'reet'                                                    
 Then 'ls'                                                    
 Else NULL                                         
End As PortfolioCode,                                                    
                                                    
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
End As TranCode,                                                    
NULL As Comment,                                                    
                                                    
-- there is some mapping in the XSLT, please check after phase 1                                                    
Case                                                  
 When T_Asset.AssetName = 'PrivateEquity'                                                    
 Then 'cbus'                                                    
 When T_Asset.AssetName = 'Equity'                                                    
 Then 'csus'                                                    
 When (T_Asset.AssetName='EquityOption' and PutOrCall = 'CALL')                                                    
 Then 'clus'                                                    
 When (T_Asset.AssetName='EquityOption' and PutOrCall = 'PUT')                                                    
 Then 'ptus'                                                
 When T_Asset.AssetName = 'FixedIncome'                                                    
 Then 'wtus'                                                   
 Else NULL                                  
End As SecType,                                                    
                                           
Case                                                    
 When CHARINDEX('_',VT.Symbol) > 0                                                 
 Then LOWER(SUBSTRING(VT.Symbol,1,CHARINDEX('_',VT.Symbol)-1))                                          
 When (CHARINDEX('-TC',VT.Symbol) > 0 Or CHARINDEX('-VC',VT.Symbol) > 0)                                                 
 Then LOWER(SUBSTRING(VT.Symbol,1,CHARINDEX('-',VT.Symbol)-1) + '.CN')                                            
 When (CHARINDEX('-TAI',VT.Symbol) > 0 Or CHARINDEX('-GTS',VT.Symbol) > 0)                                                 
 Then LOWER(SUBSTRING(VT.Symbol,1,CHARINDEX('-',VT.Symbol)-1) + '.TT')                               
 Else LOWER(VT.Symbol)                                                    
End As SecuritySymbol,                                            
                                                  
--format mmddyyyy                                                    
REPLACE(CONVERT(VARCHAR(10), AUECLocalDate, 101), '/', '') As TradeDate,                                                    
REPLACE(CONVERT(VARCHAR(10), SettlementDate, 101), '/', '') As SettleDate,                                                    
NULL As OriginalCostDate,                                                    
TaxlotQty As Quantity,       
--LTRIM(RTRIM(STR(TaxlotQty,LEN(TaxlotQty),8))) As Quantity,                              
NULL as CloseMath,--10                                                    
NULL As VersusDate,        
--25 Jan 2013: Request by Korey: short sells (trans code â€œssâ€), please make SourceType (column 12) â€œawusâ€ (instead of â€œcausâ€),                                                    
Case                                                    
 When (Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover' or Side='Sell' or Side='Sell to Close')                                                    
 Then 'caus'                                                    
 Else 'awus'                                                    
End As SourceType,                                                  
-- it should be 'cash' as per the spec document but as the file has already approved, so I have not change this code                                                     
Case                                                     
 When (Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover' or Side='Sell' or Side='Sell to Close')                                                    
 Then 'capfund'                                                    
 Else 'none'                                                    
End As SourceSymbol,                                              
NULL As TradeDateFXRate,                  
NULL As SettleDateFXRate,                                                    
NULL As OriginalFXRate,                                                    
NULL As MarkToMarket,                                                    
Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + SoftCommission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * SideMultiplier)) As Decimal(32,2)) As Varchar(500)) As TradeAmount,                    
    
      
        
          
            
               
               
                  
                    
                      
NULL As OriginalCost,                                                    
NULL As Comment1,--COL20                                                    
NULL As WithholdingTax,                       
-- Exchange Mapping                                                   
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
End As Exchange,                                                    
Case                                                     
 When StampDuty > 0                                                    
 Then Cast(Cast(StampDuty As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(StampDuty As Varchar(200))                                  
End As ExchangeFee,                                                    
Case                                                    
 When VT.Commission > 0                                                    
 Then Cast(Cast(VT.Commission As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(VT.Commission As Varchar(200))                                                    
End As commission,                                                    
Lower(T_CounterParty.ShortName) As Broker,                                                    
'n' As ImpliedComm,                                                    
Case                                                    
 When (TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) > 0                                                    
 Then Cast(Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) As Decimal(32,2)) As Varchar(200))                                                 
 Else  Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) As Varchar(200))                                                    
End As OtherFees,                                           
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
Case                                      
 When CS.StrategyName = 'Rest/Reg Lot 1'                                      
 Then '1'                                      
 When CS.StrategyName = 'Rest/Reg Lot 2'                                      
 Then '2'                                      
 When CS.StrategyName = 'Rest/Reg Lot 3'                                      
 Then '3'                                      
 When CS.StrategyName = 'Rest/Reg Lot 4'                                      
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
End As UserDef2,                                                     
NULL As UserDef3,                                                    
Case                             
When ExternalTransID Is Not Null And ExternalTransID <> ''                            
Then ExternalTransID                            
Else Null                            
End As TranID,                                                    
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
Case                                      
 When CS.StrategyName = 'Rest/Reg Lot 1'                                      
 Then '1'                                      
 When CS.StrategyName = 'Rest/Reg Lot 2'                                      
 Then '2'                                      
 When CS.StrategyName = 'Rest/Reg Lot 3'                                      
 Then '3'                                      
 When CS.StrategyName = 'Rest/Reg Lot 4'                                      
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
ENd As Strategy,                                                    
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
Case                                                    
 When VT.SoftCommission > 0                                                    
 Then Cast(Cast(VT.SoftCommission As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(VT.SoftCommission As Varchar(200))                                                    
End As SoftCommission                            
                                                    
From #V_Taxlots VT                              
Inner Join T_PBWiseTaxlotState on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID                                                           
Inner Join T_Side on T_Side.SideTagValue=VT.OrderSideTagValue                                                    
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                                                
Inner Join T_CompanyFunds CF On CF.CompanyFundID=VT.FundID                                      
Left Outer Join T_CompanyStrategy CS On CS.CompanyStrategyID=VT.Level2ID                                                    
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = VT.Symbol                        
Left Outer Join T_Exchange On T_Exchange.ExchangeID = VT.ExchangeID                                                    
Left Outer Join T_CounterParty On VT.CounterPartyID = T_CounterParty.CounterPartyID                                                    
Where VT.AUECID in (select AUECID from @AUECID)                                 
And VT.FundID in (select FundID from @Fund) And T_PBWiseTaxlotState.PBID = @thirdPartyID                               
And ((DateDiff(d,VT.AUECLocalDate,@InputDate) = 0 And (T_PBWiseTaxlotState.TaxLotState in (1,4)))                         
 Or ((DateDiff(d,VT.AUECLocalDate,@InputDate)>=0 And T_PBWiseTaxlotState.TaxLotState in (0,2,3))))                         
          
--select * from #TempTransaction      
                          
-- Get Closed transactions                            
Insert Into #TempTransaction                                                         
Select                                                     
PT.TaxlotID As TaxlotID,                                                    
PT1.TaxlotID As ClosingTaxlotID,                                                    
Case                                                    
 When FundName = 'OFFSHORE'            
 Then 'ssfqp'                                                    
 When FundName = 'LP C/O'                                                    
 Then 'cay'                                                    
 When FundName = 'Fund1'                                                    
 Then 'sspe'                                                    
 When FundName = 'Fund2'                                                    
 Then 'techn'                                                    
 When FundName = 'Fund With Long Name'                        
 Then 'tech2'                                                    
 When FundName = 'reet'                                                    
 Then 'ls'                                                    
 Else NULL                                                    
End As PortfolioCode,                                                    
                                                    
';' As TranCode,                                                    
'LOT:QTY ' + IsNull(PT.LotId,'') + ':' + Cast(Cast(PTC.ClosedQty As Decimal(32,8)) As Varchar(2000)) As Comment,                                                    
NULL As SecType,                                     
                                                    
NULL As SecuritySymbol,                                                    
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
NULL As SoftCommission                                                   
                                                    
from #PM_TaxlotClosing  PTC                                                                                                                                 
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                              
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                             
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                                                                 
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                                                    
Inner Join T_Side on T_Side.SideTagValue=G.OrderSideTagValue                                                    
Inner Join T_CompanyFunds CF On CF.CompanyFundID=PT.FundID                                                                        
Inner Join T_PBWiseTaxlotState OpeningTaxlotState on OpeningTaxlotState.TaxlotID=PT.TaxlotID                            
Inner Join T_PBWiseTaxlotState ClosingTaxlotState on ClosingTaxlotState.TaxlotID=PT1.TaxlotID                                                                
Where G.AUECID in (select AUECID from @AUECID)                                 
And CF.CompanyFundID in (select FundID from @Fund)                          
And (OpeningTaxlotState.PBID = @ThirdPartyID)                          
And                           
(                          
(DateDiff(d,@InputDate,PTC.AUECLocalDate) =0 and (DateDiff(d,@InputDate,G.AUECLocalDate) <> DateDiff(d,@InputDate,G1.AUECLocalDate))                                 
And OpeningTaxlotState.TaxlotState In (1))                      
Or                          
(DateDiff(d,PTC.AUECLocalDate,@InputDate) >= 0 And ClosingTaxlotState.TaxlotState In (0,2) And (DateDiff(d,@InputDate,G.AUECLocalDate) <> DateDiff(d,@InputDate,G1.AUECLocalDate)))                      
)            
            
          
--select * from #TempTransaction                      
                               
                              
--GetDeleted taxlots if any                      
Insert Into #TempTransaction                                                    
                                                    
Select                                        
DT.TaxlotID As TaxlotID,                               
DT.TaxlotID As ClosingTaxlotID,                                                    
Case                                                    
 When FundName = 'OFFSHORE'                                                    
 Then 'ssfqp'                                                    
 When FundName = 'LP C/O'                                                    
 Then 'cay'                                                    
 When FundName = 'Fund1'                                
 Then 'sspe'                                                    
 When FundName = 'Fund2'                                                    
 Then 'techn'                                                    
 When FundName = 'Fund With Long Name'                                                    
 Then 'tech2'                                                    
 When FundName = 'reet'                                                    
 Then 'ls'                                                    
 Else NULL                                                    
End As PortfolioCode,                                                     
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
End As TranCode,                                                  
NULL As Comment,                                                     
Case                                                    
 When T_Asset.AssetName = 'PrivateEquity'                                                    
 Then 'cbus'                                                    
 When T_Asset.AssetName = 'Equity'                                                    
 Then 'csus'                                                    
 When (T_Asset.AssetName='EquityOption' and PutOrCall = 'CALL')                                                    
 Then 'clus'                                                    
 When (T_Asset.AssetName='EquityOption' and PutOrCall = 'PUT')                                                    
 Then 'ptus'                                                
 When T_Asset.AssetName = 'FixedIncome'                                                    
 Then 'wtus'                                                   
 Else NULL                                                    
End As SecType,                                                                          
Case                                                    
 When CHARINDEX('_',DT.Symbol) > 0                              
 Then LOWER(SUBSTRING(DT.Symbol,1,CHARINDEX('_',DT.Symbol)-1))                                          
 When (CHARINDEX('-TC',DT.Symbol) > 0 Or CHARINDEX('-VC',DT.Symbol) > 0)                                                 
 Then LOWER(SUBSTRING(DT.Symbol,1,CHARINDEX('-',DT.Symbol)-1) + '.CN')                                            
 When (CHARINDEX('-TAI',DT.Symbol) > 0 Or CHARINDEX('-GTS',DT.Symbol) > 0)                                            
 Then LOWER(SUBSTRING(DT.Symbol,1,CHARINDEX('-',DT.Symbol)-1) + '.TT')                                     
 Else LOWER(DT.Symbol)                                          
End As SecuritySymbol,                                            
                                                  
--format mmddyyyy                                                    
REPLACE(CONVERT(VARCHAR(10), AUECLocalDate, 101), '/', '') As TradeDate,                                                    
REPLACE(CONVERT(VARCHAR(10), SettlementDate, 101), '/', '') As SettleDate,                                                    
NULL As OriginalCostDate,                                                    
TaxlotQty As Quantity,                                                    
NULL as CloseMath,--10                                                    
NULL As VersusDate,                             
--25 Jan 2013: Request by Korey: short sells (trans code â€œssâ€), please make SourceType (column 12) â€œawusâ€ (instead of â€œcausâ€),                                             
Case                                                    
 When (Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover' or Side='Sell' or Side='Sell to Close')                                                    
 Then 'caus'                                                    
 Else 'awus'                                                    
End As SourceType,                                               
-- it should be 'cash' as per the spec document but as the file has already approved, so I have not change this code                                                     
Case                                                     
 When (Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover' or Side='Sell' or Side='Sell to Close')                                                    
 Then 'capfund'                                                    
 Else 'none'                            
End As SourceSymbol,                                              
NULL As TradeDateFXRate,                                                    
NULL As SettleDateFXRate,                                                    
NULL As OriginalFXRate,                 
NULL As MarkToMarket,                               
CASE                               
 WHEN (ORderSideTagValue IN ('A', 'B', '1'))                               
 THEN Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + SoftCommission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1)) As Decimal(32,2)) As Varchar(500))
    
      
        
         
 WHEN (ORderSideTagValue IN ('2', '5', '6', 'C', 'D'))                               
 THEN  Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + SoftCommission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * -1)) As Decimal(32,2)) As Varchar(500))                              
 ELSE Cast(Cast(((Round(AvgPrice,4) * TaxlotQty * SM.Multiplier) + ((Commission + SoftCommission + OtherBrokerFees + TransactionLevy + StampDuty + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) * 1)) As Decimal(32,2)) As Varchar(500))                               
END As TradeAmount,                              
NULL As OriginalCost,                                                    
NULL As Comment1,--COL20                                                    
NULL As WithholdingTax,                                                    
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
End As Exchange,                                       
Case                                                     
 When StampDuty > 0                                                    
 Then Cast(Cast(StampDuty As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(StampDuty As Varchar(200))                                                    
End As ExchangeFee,                                                    
Case                                                    
 When DT.Commission > 0                                                    
 Then Cast(Cast(DT.Commission As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(DT.Commission As Varchar(200))                                 
End As commission,                                                    
Lower(T_CounterParty.ShortName) As Broker,                                                    
'n' As ImpliedComm,                                       
Case                                                    
 When (TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) > 0                                      
 Then Cast(Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) As Decimal(32,2)) As Varchar(200))                                                 
 Else  Cast((TransactionLevy + OtherBrokerFees + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee) As Varchar(200))                                                    
End As OtherFees,                                                    
NULL As CommPurpose,                                                    
'n' As Pledge,--29                                                    
'253' As LotLocation,--COL30                                                    
NULL As DestPledge,                                                    
NULL As DestLotLocation,                                                    
NULL As OriginalFace,                                                    
NULL As YieldOnCost,                                                    
NULL As DurationOnCost,--COL35                                                    
NULL As UserDef1,                          
Case                                      
 When CS.StrategyName = 'Rest/Reg Lot 1'                                      
 Then '1'                                      
 When CS.StrategyName = 'Rest/Reg Lot 2'                                      
 Then '2'                                      
 When CS.StrategyName = 'Rest/Reg Lot 3'                                      
 Then '3'                                      
 When CS.StrategyName = 'Rest/Reg Lot 4'                                      
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
ENd As UserDef2,                                                    
NULL As UserDef3,                             
Case                            
When DT.ExternalTransId Is Not Null And ExternalTransID <> ''                            
Then DT.ExternalTransId                            
Else NULL                             
End As TranID,                                                    
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
Case                                      
 When CS.StrategyName = 'Rest/Reg Lot 1'                                      
 Then '1'                                      
 When CS.StrategyName = 'Rest/Reg Lot 2'                                      
 Then '2'                                      
 When CS.StrategyName = 'Rest/Reg Lot 3'                                      
 Then '3'                                      
 When CS.StrategyName = 'Rest/Reg Lot 4'                                
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
ENd As Strategy,                            
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
Case                                                    
 When DT.SoftCommission > 0                                                    
 Then Cast(Cast(DT.SoftCommission As Decimal(32,2)) As Varchar(200))                                                    
 Else  Cast(DT.SoftCommission As Varchar(200))                                 
End As SoftCommission                                          
                                                    
From T_DeletedTaxlots DT                                                       
Inner Join T_Side on T_Side.SideTagValue=DT.OrderSideTagValue                                                    
Inner Join T_Asset on T_Asset.AssetID=DT.AssetID                                                     
Inner Join T_CompanyFunds CF On CF.CompanyFundID=DT.FundID                                      
Left Outer Join T_CompanyStrategy CS On CS.CompanyStrategyID=DT.Level2ID                                                    
Left Outer Join #SecMasterDataTempTable SM On SM.TickerSymbol = DT.Symbol                                                   
Left Outer Join T_Exchange On T_Exchange.ExchangeID = DT.ExchangeID                                                    
Left Outer Join T_CounterParty On DT.CounterPartyID = T_CounterParty.CounterPartyID                                                    
--FX Rate for Trade Date from Daily Valuation                                                  
Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                  
On (FXDayRatesForTradeDate.FromCurrencyID = DT.CurrencyID                                                                                                                           
And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                 
And DateDiff(d,DT.AUECLocalDate,FXDayRatesForTradeDate.Date)=0)                                                    
Where DateDiff(d,DT.AUECLocalDate,@InputDate) >= 0 And                                 
DT.AUECID in (select AUECID from @AUECID)                                 
And DT.FundID in (select FundID from @Fund)                               
And DT.TaxlotState = 3 And DT.PBID = @ThirdPartyID                            
                        
-- Update TaxlotState and Level1 ID that is used for system internal use only                            
Update #TempTransaction                              
Set Level1AllocationID = VT.Level1AllocationID,                              
TaxLotStateID = T_PBWiseTaxlotState.TaxLotState                              
From #TempTransaction                               
Inner Join #V_Taxlots VT On VT.TaxlotID = #TempTransaction.TaxlotID                              
Inner Join T_PBWiseTaxlotState on T_PBWiseTaxlotState.TaxlotID=VT.TaxlotID                 
Where T_PBWiseTaxlotState.PBID = @ThirdPartyID                             
                              
--Symbol Mapping                                    
Update #TempTransaction                                    
Set SecuritySymbol = Lower(Mapping.AXYSSymbol)                                    
From #TempTransaction                                     
Inner Join T_PranaAXYSSymbolMapping  Mapping On Mapping.PranaSymbol = #TempTransaction.SecuritySymbol                           
                          
-- Update Closing Methology                        
-- if a taxlot opens and close same day then, we set Closemath ='s'                          
Update #TempTransaction                            
Set CloseMath = 's' ,                      
CustomOrderBy = 1                           
from #PM_TaxlotClosing  PTC                                
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                              
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                          
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                        
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                            
Inner Join #TempTransaction on #TempTransaction.ClosingTaxlotID = PT1.TaxlotID                              
Where ((DateDiff(d,@inputDate,G.AUECLocalDate) = DateDiff(d,@inputDate,G1.AUECLocalDate)) And PT1.OrderSideTagValue in ('2','D','B'))          
                       
                                               
-- ClosingTaxlotID -> when we fetch transactions from V_Taxlot view, it returns TaxlotID, I simply say it closingTaxlotID                                           
-- When we fetch data from #PM_TaxlotClosing table, it has both positional and closing TaxlotIDs, I use only closingTaxlotID and collect in the #TempTransaction table                                             
-- I need only closingTaxlotID. If a short transaction close any long trade, then short trade will have more than one closingTaxlotID i.e.                                                  
-- one from #V_Taxlots and one or more from #PM_TaxlotClosing, so collect all the ClosingTaxlotID here                                                   
Select                                                   
ClosingTaxlotID                                                  
Into #TempClosingIDTable                                                  
From #TempTransaction                                                  
Group By ClosingTaxlotID                                                  
having Count(ClosingTaxlotID) > 1                                                  
                   
--select * from #TempClosingIDTable          
                                         
--we know that main closing trade has all the information like Qty, Price but child i.e. which one it is closing, has selected information                                                  
--so I made a check on Qty field because Qty field is null in the child row                                                  
Update #TempTransaction                                                  
Set CloseMath = 'b'                                 
From #TempTransaction                                                  
Where ClosingTaxlotID in (Select ClosingTaxlotID From #TempClosingIDTable)                                                  
And Quantity is not null and Quantity > 0 --CAST(Quantity As Decimal(32,2)) > 0                             
            
        
---------------------Added by Narendra------------------------------          
--Update closing methodology Closemath ='s'               
--if a taxlot closes buy of previous day but one of buy taxlot does not have lotid            
          
Select                                                   
T1.ClosingTaxlotID                                                      
Into #TempClosingIDTableToChangedMath                                                  
from #TempTransaction T1            
Inner Join #TempClosingIDTable TempClosingId            
on T1.ClosingTaxlotID = TempClosingId.ClosingTaxlotID            
Inner Join #T_Level2Allocation L2 on T1.TaxLotId = L2.TaxLotId              
Where T1.TaxlotId <> T1.ClosingTaxLotid            
and (L2.LotId is null or L2.LotId = '')              
        
        
Update t1        
Set CloseMath='s'        
from #TempTransaction t1        
where ClosingTaxlotID        
in (select ClosingTaxlotID from #TempClosingIDTableToChangedMath)        
and Taxlotid=ClosingTaxlotID        
        
        
          
--delete closed taxlots which have LotId null                          
delete tt1          
from #TempTransaction tt1          
where tt1.ClosingTaxLotid         
in (select ClosingTaxlotID from #TempClosingIDTableToChangedMath)           
and Taxlotid!=ClosingTaxlotID          
          
--------------------------------------------------------------------------           
          
          
                      
-- if a previous date closed transaction i.e. buy transaction is closed with the currecnt date sell transaction and amended i.e. buy transaction is amended                      
-- then its handling is in XSLT, so we set its taxlot state to send                         
Update #TempTransaction                          
Set TaxLotStateID = 1                          
From #TempTransaction                          
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTransaction InnerTemp Where InnerTemp.InternalTranCode = #TempTransaction.InternalTranCode )                            
And Comment is not null and TaxLotStateID = 2                          
                          
-- collect taxlotid whose taxlot State is Amended and which is closing privious date buy taxlots i.e. CloseMath = 'b'                       
--Select                               
--ClosingTaxlotID                              
--Into #TempTaxlotState                           
--From #TempTransaction                  
--Where TaxLotStateID = 2 and CloseMath = 'b'           
        
Select                           
ClosingTaxlotID,        
TaxLotStateID                                  
Into #TempTaxlotState                       
From #TempTransaction                          
Where CloseMath = 'b' --and TaxLotStateID = 2                           
                        
-- set taxlotstate to amend of previous date buy taxlots which is closed with today's sell taxlot but this sell is amended                      
-- so to bundle all these trades, we set taxlotstate to amend to the buy taxlots also.                      
--Update #TempTransaction                              
--Set TaxLotStateID = 2                              
--Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTaxlotState)                              
         
-- set taxlotstate to amend of previous date buy taxlots which is closed with today's sell taxlot but this sell is amended                              
-- so to bundle all these trades, we set taxlotstate to amend to the buy taxlots also.        
        
Update #TempTransaction                          
Set TaxLotStateID = 2                          
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTaxlotState Where TaxLotStateID = 2)        
                     
-- if a sell trade closes historical buy trades then this buy trade should come under this sell trade i.e. come togather        
-- so to bundle all these trades, we set taxlotstate to allocated to the buy trades also.                   
Update #TempTransaction                                  
Set TaxLotStateID = 0           
Where ClosingTaxlotID In (Select ClosingTaxlotID From #TempTaxlotState Where TaxLotStateID = 0)                         
                      
-- this is to arrange same day open and close transactions i.e. buy and then sell or short and cover                      
Update #TempTransaction                            
Set ClosingTaxlotID =PTC.ClosingTaxlotID,                      
CustomOrderBy = 0                            
from #PM_TaxlotClosing PTC                                
Inner Join #PM_Taxlots PT on (PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                              
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk)                        
Inner Join #T_Group G on G.GroupID = PT.GroupID                                                        
Inner Join #T_Group G1 on G1.GroupID = PT1.GroupID                   
Inner Join #TempTransaction on #TempTransaction.TaxlotID = PT.TaxlotID                              
Where (DateDiff(d,@inputDate,G.AUECLocalDate) = DateDiff(d,@inputDate,G1.AUECLocalDate)) And PT.OrderSideTagValue in ('1','A','5','C')        
And #TempTransaction.Comment is Null                              
                  
--Select * from #TempTransaction          
              
--Collect Taxlots whose closing methodology is 'S' then arrange by TaxlotID, closingID and customOrderBy      
Insert Into #TempTransaction_Final       
Select * From #TempTransaction       
Where ClosingTaxlotID in (Select ClosingTaxlotID From #TempTransaction Where CloseMath = 's')       
Order by CustomOrderBy,TaxlotId,ClosingTaxlotID       
      
          
----Collect Taxlots whose closing methodology is not 'S' then arrange by closingID and customOrderBy      
Insert Into  #TempTransaction_Final        
Select * From #TempTransaction Where ClosingTaxlotID Not In (Select ClosingTaxlotID From #TempTransaction Where CloseMath = 's')       
Order by ClosingTaxlotID,CustomOrderBy       
                                                  
Select                      
CustomOrderBy,                       
TaxlotID,                            
ClosingTaxlotID,                           
InternalTranCode,                                                   
PortfolioCode,                                                    
TranCode,                                                    
Comment,                               
SecType,                                                    
SecuritySymbol,                                                    
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
FromDeleted,
SoftCommission                            
                                                     
From #Temptransaction_Final    
Order by TableID                                                           
--Order by ClosingTaxlotID,CustomOrderBy                  
                                                
Drop Table #TempTransaction,#TempClosingIDTable,#Temptransaction_Final                                                        
Drop Table #FXConversionRates,#SecMasterDataTempTable,#TempTaxlotState,#TempClosingIDTableToChangedMath,#PM_Taxlots,#V_Taxlots,#PM_TaxlotClosing,#T_Group,#T_Level2Allocation    
  
