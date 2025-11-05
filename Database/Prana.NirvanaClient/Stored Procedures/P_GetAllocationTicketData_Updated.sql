/*********************************************                              
                          
AUTHOR: Rahul Gupta                        
Creation Date: 2012-12-12                        
Decsription: Backend Sp for Allocation Ticket Report                 
          
Modified By : Ankit          
Modification Date: 2013-1-16           
Decsription: [log001] For Custom Fund Name requirement by SS i.e Fund name before ":"             
          
Usage :             
P_GetAllocationTicketData_Updated '2013-07-11','2013-07-23'                              
                              
*********************************************/                                     
CREATE Proc [dbo].[P_GetAllocationTicketData_Updated]                                                                                                                        
(                                                                                                                                                                                                            
 @startDate datetime,                                                                                                                      
 @enddate datetime                                                                                                                  
)                                                                                                                      
as                                 
                              
--DECLARE @startDate datetime                              
--SET @startDate = '2013-07-11'                              
--                                                                                                                  
--DECLARE @enddate datetime                              
--SET @enddate = '2013-07-11'                                
SET NOCOUNT ON;                              
                              
DECLARE @BaseCurrencyID int                              
SET @BaseCurrencyID = (Select BaseCurrencyID from T_Company)                              
                              
CREATE TABLE #FXConversionData                                                                                                                                                                                                                
(                                                                                                                                                                                     
  FCID int,                                                                                                                                                                                            
  TCID int,                                                                                                                                                                                                                     
  RateValue float,                                        
  ConversionMethod int,                                                                                   
  DateCC DateTime                                                                                                                                                                           
)                                                                                                                                                                                    
INSERT INTO #FXConversionData                                                                                                         
SELECT                                                                                                                                                                               
 FromCurrencyID,                                                                                                                                                                                  
 ToCurrencyID,                                      
 RateValue,                 
 ConversionMethod,           
 Date                                                                     
FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startDate, @enddate)                                  
            
----------------------------------------------------------------------            
--[log001]          
CREATE TABLE #CustomFundName                                                                                                                                                                                                               
(                                                                                                                                                                                     
  CompanyFundID int,                                                                                                                                                                                            
  FundName  Varchar(100)                                                                                                                                                                         
)                                                                                                                                                                                    
INSERT INTO #CustomFundName                                                                                                         
SELECT                                                                                                                                                                               
 CompanyFundID,                                                                                                                                                                                            
  FundName                                                                     
FROM T_CompanyFunds Where IsActive=1                            
            
update #CustomFundName           
set FundName = Substring( FundName, 1, CharIndex( ':', FundName ) - 1)           
where  CharIndex( ':', FundName )>1            
--Select * from #CustomFundName              
            
-------------------------------------------------------------------------            
                      
Create Table #SecMasterDataTempTable                                 
(                             
  AUECID int,                                       
  TickerSymbol Varchar(100),                                                                        
  CompanyName  VarChar(500),                               
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
  AssetID int,                                                            
  CUSIPSymbol varchar(100),                                                            
  SEDOLSymbol varchar(100),                                                            
  ISINSymbol varchar(100),                                                            
  BloombergSYmbol varchar(100),                                                            
  OSISYmbol varchar(100),                                                      
  IDCOSymbol varchar(100)                                                                                                                   
)                                                                                                
                                             
Insert Into #SecMasterDataTempTable                                                                            
Select                                                                                                               
 AUECID ,                                                                                                            
 TickerSymbol ,                                                                          
 CompanyName  ,                                                                   
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
 AssetID,                                                    
 CUSIPSymbol,                                                            
 SEDOLSymbol,                                                            
 ISINSymbol,                             
 BloombergSYmbol,                                                      
 OSISYmbol,                                                      
 IDCOSymbol                                        
     From V_SecMasterData                               
                              
Select *       
Into #V_Taxlots_Temp      
From V_Taxlots VTL      
Where datediff(d,VTL.ProcessDate,@startDate) <= 0                                                                   
and Datediff(d,VTL.ProcessDate,@endDate) >=0       
      
--Select PM_TaxLotClosing.*       
--Into #PM_TaxLotClosing_Temp      
--From PM_TaxLotClosing      
--Inner Join #V_Taxlots_Temp VTL On PM_TaxLotClosing.ClosingTaxLotID = VTL.TaxLotID        
      
CREATE TABLE #ClosingTradeInfoTable                              
(                              
TaxLotID varchar(50),      
Symbol varchar(50),                            
Quantity float,      
StrategyID int,      
PositionalTaxlotID Varchar(50),      
ClosingTaxlotID Varchar(50)                             
)      
    
Select    
PT.TaxLotID,    
PT.Symbol AS Symbol,    
PT.TaxLotOpenQty,    
PT.Level2ID,    
PT.OrderSideTagValue    
InTo #OpenTaxlots    
From PM_Taxlots PT    
Where Taxlot_PK in    
(    
 Select Max(Taxlot_PK) from PM_Taxlots    
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@Enddate) >= 0    
 group by TaxlotId    
)    
and TaxLotOpenQty<>0    
    
--Select * from #OpenTaxlots    
    
Insert Into #ClosingTradeInfoTable    
Select    
VTL.TaxLotID,    
VTL.Symbol AS Symbol,    
IsNull(PM_TaxLotClosing.ClosedQty,VTL.TaxLotQty) as Quantity,    
VTL.Level2ID as StrategyID,    
PM_TaxLotClosing.PositionalTaxlotID,    
PM_TaxLotClosing.ClosingTaxlotID    
From #V_Taxlots_Temp VTL    
Left Outer Join PM_TaxLotClosing On PM_TaxLotClosing.ClosingTaxLotID = VTL.TaxLotID    
    
Insert Into #ClosingTradeInfoTable    
Select    
TaxLotID,    
Symbol,    
TaxLotOpenQty,    
Level2ID,    
Null,    
Null    
From #OpenTaxlots Where TaxLotID In (Select ClosingTaxlotID From #ClosingTradeInfoTable) --OrderSideTagValue In ( '2')    
      
Update ClosingTrade      
Set       
ClosingTrade.StrategyID = OpeningTrade.StrategyID                
From #ClosingTradeInfoTable ClosingTrade    
Inner Join #ClosingTradeInfoTable OpeningTrade On OpeningTrade.TaxLotID = ClosingTrade.PositionalTaxLotID      
      
Select       
TaxlotID,      
Max(Symbol) As Symbol,      
Sum(Quantity) As Quantity,      
StrategyID      
Into #TaxlotIDAndStrategyIDGroupingTable      
From #ClosingTradeInfoTable      
Group By TaxlotID, StrategyID      
      
--select * from #TaxlotIDAndStrategyIDGroupingTable      
                                  
                                  
-------------------------------------------------------------------------------------------------------                                
CREATE TABLE #T_Transactions                              
(                              
GroupID varchar(100),                              
TaxLotID varchar(50),                                
TradeDate datetime,                                
ProcessDate datetime,                                                                
SettlementDate datetime,                                                                                                                                                       
Symbol varchar(50),                              
SecurityName varchar(200),                                
AUECID int,                                                                                                                     
Asset varchar(50),                                                                  
UnderlyingName varchar(50),                                                                 
ExchangeName varchar(50),                               
AccountID int,                                         
FundName varchar(50),              
FundAccountNo varchar(50),                              
Quantity float,                                
AvgPrice float,                                
Side varchar(50),                                                               
Multiplier float,                              
SideMultiplier int,                       
PutOrCall varchar(10) ,                               
I1 float,                                 
DC int,                          
AllocationScheme varchar(100),                                 
CommissionLocal float,                                
CommissionBase float,                                                                                                                        
OtherBrokerFeesLocal float,                              
OtherBrokerFeesBase float,                                                                                                                       
OtherFeesLocal float,                                 
OtherFeesBase float,                                                 
NotionalValue float,                             
FXRate_TradeDate float,                                                                                                                                                                                         
MasterFundName varchar(50),                      
UDAAssetName varchar(50),                                                                                                                
UDASecurityTypeName varchar(50),                                                                                                                
UDASectorName varchar(50),                                                                                                                
UDASubSectorName varchar(50),                                                                                                          
UDACountryName varchar(50),                                                                                                                
CurrencySymbol varchar(20),                                                                                                                     
StrategyID int,                                                                                                                
StrategyName varchar(100),                              
PrimeBrokerCode varchar(50),                                                    
PrimeBrokerName varchar(50),                                                                                                                    
CounterParty varchar(100),                      
BrokerName varchar(100),                         
MasterStrategyName varchar(50),                                                      
UnderlyingSymbol Varchar(100),                                              
BloombergSymbol Varchar(100),                                              
SedolSymbol Varchar(100),                                              
ISINSymbol Varchar(100),                                              
CusipSymbol Varchar(100),                                              
OSISymbol Varchar(100),                                              
IDCOSymbol Varchar(100),                              
TotalCommissionAndFeesLocal float,                              
TotalCommissionAndFeesBase float,                              
CommissionPerShare float,                            
AllocationPercent float,                              
PrincipalAmountLocal float,                              
PrincipalAmountBase float,                              
NetAmountLocal float,                              
NetAmountBase float,                                                                                                                        
ClearingBrokerFeesLocal float,                              
ClearingBrokerFeesBase float,                                       
SoftCommissionLocal float,                                      
SoftCommissionBase float                                                                    
)                              
                               
Insert into #T_Transactions                                                                   
Select                              
VTL.GroupID,                                                                                 
VTL.TaxLotID,                      
VTL.AuecLocalDate as TradeDate,                                                                           
VTL.ProcessDate,                                                    
VTL.SettlementDate,                            
VTL.Symbol AS Symbol,                              
IsNull(SM.CompanyName,'') as SecurityName,                              
T_AUEC.AUECID as AUECID,                                                                                                                      
Asset.AssetName as Asset,                                                                                                          
Underlying.UnderlyingName as UnderlyingName,                                                                                                               
Exchange.FullName as ExchangeName,                                
VTL.FundID as AccountID,                
--[log001]              
CF.FundName,                
IsNull(Substring(CTPMD.FundAccntNo,1,LEN(CTPMD.FundAccntNo)-1),'') as FundAccountNo,                                                                                   
Temp.Quantity,--TaxLotQty as Quantity,                                
VTL.AvgPrice as AvgPrice,                               
T_Side.Side as Side,                              
ISNULL(SM.Multiplier, 0) AS Multiplier,                                                                                                               
dbo.GetSideMultiplier(VTL.OrderSideTagValue) as SideMultiplier,                                                                                                                
ISNULL(SM.PutOrCall,'') AS PutOrCall,                                                                                                         
ISNULL(BenchMarkRate + [Differential],0) As I1,                                                                                                                
ISNULL(DayCount,0) as DC,                                
ISNULL(AScheme.AllocationSchemeName,'Manual') as AllocationScheme,     
Case    
 When G.CumQty = 0    
 Then 0    
 Else ISNULL(VTL.Commission,0) * (Temp.Quantity / G.CumQty )     
End as CommissionLocal,                                 
0 as CommissionBase,      
Case    
 When G.CumQty = 0    
 Then 0    
 Else ISNULL(VTL.OtherBrokerFees,0) * (Temp.Quantity / G.CumQty )    
End as OtherBrokerFeesLocal,                               
0 as OtherBrokerFeesBase,       
Case    
 When G.CumQty = 0    
 Then 0    
 Else ISNULL(VTL.StampDuty + VTL.TransactionLevy + VTL.ClearingFee + VTL.TaxOnCommissions + VTL.MiscFees + VTL.SecFee + VTL.OccFee + VTL.OrfFee,0) * (Temp.Quantity / G.CumQty )    
End as OtherFeesLocal,                              
0 as OtherFeesBase,                                                                                                              
                              
Case VTL.Quantity                                                         
 When 0                                                         
 Then 0                                                        
 Else ISNULL((ISNULL(VTL.NotionalValue, 0) * ISNULL(VTL.TaxlotQty, 0) / ISNULL(VTL.Quantity, 1)), 0)                                                          
End as NotionalValue,                              
                               
Case                                                           
When VTL.CurrencyID <> @BaseCurrencyID                               
Then                                                      
 CASE                                                      
  WHEN IsNull(VTL.FXRate_Taxlot,IsNull(G.FXRate,0)) > 0                                                       
  THEN                               
 CASE ISNULL(VTL.FXConversionMethodOperator_Taxlot,ISNULL(G.FXConversionMethodOperator, 'M'))                              
  WHEN 'M'                              
  THEN IsNull(VTL.FXRate_Taxlot,IsNull(G.FXRate,0))                        
  WHEN 'D'                              
  THEN 1/IsNull(VTL.FXRate_Taxlot,IsNull(G.FXRate,0))                                                   
 END                                                  
  ELSE IsNull(FXConversionData.RateValue,0)                                                       
 END                                                      
ELSE 1                                              
END as FXRate_TradeDate,                               
                                                                                                                 
ISNULL(CMF.MasterFundName,'Unassigned') as  MasterFundName,                                                                                                                           
                     
                                   
ISNULL(SM.AssetName, 'Undefined') as UDAAssetName,                                                                                                         
ISNULL(SM.SecurityTypeName, 'Undefined') as UDASecurityTypeName,                                                                                                                
ISNULL(SM.SectorName, 'Undefined') as UDASectorName,                                                                                            
ISNULL(SM.SubSectorName, 'Undefined') as UDASubSectorName,                                                                  
ISNULL(SM.CountryName, 'Undefined') as UDACountryName,                                                                                                                
                                                                                                               
Currency.CurrencySymbol AS CurrencySymbol,                                                                                                                                         
Temp.StrategyID,--Level2ID as StrategyID,                                                                                          
CS.StrategyName AS StrategyName,                                
TP.ShortName as PrimeBrokerCode,                                                                                      
TP.ThirdPartyName AS PrimeBrokerName,                                                                                                                                                
                                                                      
ISNULL(CP.ShortName,'Undefined') as CounterParty,                       
ISNULL(BMD.BrokerFullName,ISNULL(CP.ShortName,'Undefined')) as BrokerName,                                                    
ISNULL(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                            
ISNULL(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                            
ISNULL(NULLIF(SM.BloombergSymbol,''),'N/A') as BloombergSymbol,                                              
ISNULL(SM.SedolSymbol,'') as SedolSymbol,                                              
ISNULL(SM.ISINSymbol,'') as ISINSymbol,                                             
ISNULL(NULLIF(SM.CusipSymbol,''),'N/A') as CusipSymbol,                                              
ISNULL(SM.OSISymbol,'') as OSISymbol,                                              
ISNULL(SM.IDCOSymbol,'') as IDCOSymbol,                              
0 as TotalCommissionAndFeesLocal,                  
0 as TotalCommissionAndFeesBase,                              
0 as CommissionPerShare,               
                           
--Case                            
--When G.CumQty <> 0                            
--Then (VTL.TaxLotQty/G.CumQty)                            
--Else 0                            
--End as AllocationPercent,       
Case                            
When G.CumQty <> 0                            
 Then (Temp.Quantity / G.CumQty)                            
Else 0                            
End as AllocationPercent,                    
               
--IsNull(Round(VTL.AvgPrice,4) * VTL.TaxlotQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(VTL.OrderSideTagValue),0)  as PrincipalAmountlocal,      
IsNull(Round(VTL.AvgPrice,4) * Temp.Quantity * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(VTL.OrderSideTagValue),0)  as PrincipalAmountlocal,                              
              
0 as PrincipalAmountBase,                              
0 as NetAmountLocal,                              
0 as NetAmountBase,      
Case    
 When G.CumQty = 0    
 Then 0    
 Else ISNULL(VTL.ClearingBrokerFee,0) * (Temp.Quantity / G.CumQty )    
End as ClearingBrokerFeesLocal,                               
0 as ClearingBrokerFeesBase,
Case    
 When G.CumQty = 0    
 Then 0    
 Else ISNULL(VTL.SoftCommission,0) * (Temp.Quantity / G.CumQty )     
End as SoftCommissionLocal,                                 
0 as SoftCommissionBase                              
                                                                
                                                            
From #TaxlotIDAndStrategyIDGroupingTable Temp      
Inner Join  #V_Taxlots_Temp VTL On VTL.TaxlotID = Temp.TaxlotID                                                                                  
INNER JOIN T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                   
INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID                                                                                              
INNER JOIN T_AUEC on VTL.AUECID=T_AUEC.AUECID                                                                                
--INNER JOIN T_Company C ON C.companyID = @companyID                                                                          
INNER JOIN T_Asset Asset ON T_AUEC.AssetID = Asset.AssetID      
INNER JOIN T_Underlying Underlying ON T_AUEC.UnderlyingID = Underlying.UnderlyingID                               
INNER JOIN T_Exchange Exchange ON T_AUEC.ExchangeID = Exchange.ExchangeID              
--[log001]                               
INNER JOIN #CustomFundName CF ON VTL.FundID = CF.CompanyFundID                               
--INNER JOIN T_CompanyFunds CF ON VTL.FundID = CF.CompanyFundID          
INNER JOIN T_Currency Currency on VTL.CurrencyID = Currency.CurrencyID                            
LEFT OUTER JOIN T_AllocationScheme AScheme ON AScheme.AllocationSchemeID = G.AllocationSchemeID                                                            
LEFT OUTER JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                          
LEFT OUTER JOIN T_BrokerMappingDetails BMD ON CP.ShortName = BMD.BrokerShortName                                                                                         
--LEFT OUTER JOIN T_CompanyStrategy CS ON VTL.Level2ID = CS.CompanyStrategyID                                                                                        
LEFT OUTER JOIN T_CompanyStrategy CS ON Temp.StrategyID = CS.CompanyStrategyID                                                                                        
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails CTPMD ON VTL.FundID = CTPMD.InternalFundNameID_FK                                                                                        
LEFT OUTER JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                                        
LEFT OUTER JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                                      
LEFT OUTER JOIN #SecMasterDataTempTable as SM on VTL.Symbol=SM.TickerSymbol           
                                                                                                                
LEFT OUTER JOIN #FXConversionData FXConversionData                               
On VTL.CurrencyId = FXConversionData.FCID                                                                         
AND @BaseCurrencyID = FXConversionData.TCID                                                                                       
AND DATEDIFF(d, FXConversionData.DateCC, VTL.ProcessDate) = 0                                                                                
                                                                                                          
                                                                                                               
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                           
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                  
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                                                                    
                                                                                                                      
--Where datediff(d,VTL.ProcessDate,@startDate) <= 0                                                                   
--and Datediff(d,VTL.ProcessDate,@endDate) >=0                              
------------------------------------------------------------------------------------------------                              
                              
Update #T_Transactions                              
Set                               
CommissionBase = CommissionLocal * FXRate_TradeDate,                                
OtherBrokerFeesBase = OtherBrokerFeesLocal * FXRate_TradeDate,                              
OtherFeesBase = OtherFeesLocal * FXRate_TradeDate,                              
PrincipalAmountBase = PrincipalAmountLocal * FXRate_TradeDate,                              
TotalCommissionAndFeesLocal = CommissionLocal + SoftCommissionLocal + OtherBrokerFeesLocal + ClearingBrokerFeesLocal + OtherFeesLocal,                                
ClearingBrokerFeesBase = ClearingBrokerFeesLocal * FXRate_TradeDate,
SoftCommissionBase = SoftCommissionLocal * FXRate_TradeDate                                    
                              
UPDATE #T_Transactions                                                        
SET                                           
PrincipalAmountBase = PrincipalAmountBase/100,                                          
PrincipalAmountLocal = PrincipalAmountLocal/100                                                       
Where Asset = 'FixedIncome'                                
                              
                                       
UPDATE #T_Transactions                                                        
SET                                  
TotalCommissionAndFeesBase = TotalCommissionAndFeesLocal * FXRate_TradeDate                              
                              
UPDATE #T_Transactions                                                        
SET                                            
CommissionPerShare =                           
CASE                           
WHEN Quantity <> 0                          
THEN CommissionBase/Quantity                          
ELSE 0                          
END,                               
NetAmountBase = PrincipalAmountBase + TotalCommissionAndFeesBase,                                                        
NetAmountLocal = PrincipalAmountLocal + TotalCommissionAndFeesLocal                                                        
                              
UPDATE #T_Transactions                                                            
SET TaxlotID = Substring(TaxlotID, 1, (Len(TaxlotID) - 1)) + Cast (StrategyID As Varchar(10))      
Where StrategyID <> 0      
              
Select * from #T_Transactions order by Symbol                          
                              
Drop Table #ClosingTradeInfoTable,#TaxlotIDAndStrategyIDGroupingTable,#T_Transactions      
DROP TABLE #FXConversionData                              
DROP TABLE #SecMasterDataTempTable                              
DROP TABLE #CustomFundName,#V_Taxlots_Temp,#OpenTaxlots

