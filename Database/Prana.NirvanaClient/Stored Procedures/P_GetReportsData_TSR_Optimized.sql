-- -Main Script - Different                        
                                    
/****************************************************                                          
Created By : Sandeep Singh     
Created Date : May 31,2012                                         
Purpose : Get Trasanctions between 2 date ranges                                          
Module Name: PortfolioReports/Trasanction Summary Report                                             
Parameters: CompanyID Int,                                          
   StartDate DateTime,                                          
   EndDate DateTime                                                                                                                                                                                               
                                            
EXEC [P_GetReportsData_TSR_Optimized] 6, '05-22-2012', '05-23-2012' ,'TradeDate','2,7,23','SedolSymbol','B5VJH76','2,3,4'          
*****************************************************/                                                         
CREATE proc [dbo].[P_GetReportsData_TSR_Optimized]                                                                                        
(                                                                                      
 @companyID int,                                                                                      
 @startDate datetime,                                                                                      
 @enddate datetime,              
 @FetchDataOnBasisOf varchar(20),--ProceeData/TradeDate                                                                                     
 @Fund varchar(max),     
 @Asset varchar(max),       
 @SearchBy varchar(20),        
 @SearchText Varchar(200)    
                
)                                                                                      
as     
    
--Declare @companyID int    
--Declare  @startDate datetime    
-- Declare @enddate datetime              
-- Declare @FetchDataOnBasisOf varchar(20)--ProceeData/TradeDate                                                                                     
-- Declare @Fund varchar(max)    
-- Declare @Asset varchar(max)    
-- Declare @SearchBy varchar(20)    
-- Declare @SearchText Varchar(200)    
--    
--Set @companyID=6    
--Set @startDate = '05-22-2012'    
--Set @enddate ='05-23-2012'    
--SEt @FetchDataOnBasisOf = 'TradeDate'    
--Set @Fund= '2,7,23'    
--Set @Asset = '1'    
--Set @SearchBy = 'SedolSymbol'    
--Set @SearchText = 'B5VJH76'    
      
    
Declare @T_FundIDs Table              
(              
 FundId int              
)              
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')              
              
CREATE TABLE #T_CompanyFunds              
(              
 CompanyFundID int,              
 FundName varchar(50),              
 FundShortName varchar(50),              
 CompanyID int,              
 FundTypeID int              
)              
Insert Into #T_CompanyFunds              
 Select         
 T_CompanyFunds.CompanyFundID,        
 T_CompanyFunds.FundName,        
 T_CompanyFunds.FundShortName,        
 T_CompanyFunds.CompanyID,        
 T_CompanyFunds.FundTypeID              
  From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID    
                                                                                 
    
Declare @T_AssetIDs Table              
(              
 AssetId int              
)              
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')              
              
             
 Select         
 T_Asset.*     
 InTo    
 #T_Asset           
  From T_Asset INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetId        
             
              
 Select V_Taxlots.*     
Into #Temp_Taxlots              
 From V_Taxlots     
 INNER JOIN #T_CompanyFunds FundIDs ON V_Taxlots.FundID = FundIDs.CompanyFundID     
 INNER JOIN #T_Asset AssetIDs ON V_Taxlots.AssetID = AssetIDs.AssetID             
 Where DateDiff(day,@StartDate,V_Taxlots.AUECLocalDate) >=0                                                         
  and  DateDiff(day,V_Taxlots.AUECLocalDate,@EndDate)>=0    
                                                                         
CREATE TABLE #TEMPGetNonZeroConversionFactorForDate                                                                                                                                                                                
(                                                                                                                                                     
  FCID int,                                                                                                                                                            
  TCID int,                                                                                                                                                                                     
  ConversionFactor float,                                                                                                                                
  ConversionMethod int,                                                                                                                                
  DateCC DateTime                                                                                                                                                   
)                                                                                                                                                    
INSERT INTO #TEMPGetNonZeroConversionFactorForDate                                                                         
SELECT                                                                                                                                               
 FromCurrencyID,                                                                                                                                                  
 ToCurrencyID,                                                                                
 RateValue,                                                                                                                                
 ConversionMethod,                                                           
 Date                                     
FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startDate, @enddate)                                                                                        
                                                                                 
--Create temp table for report                                                                 
Create Table #TempReportTable                                                                        
(                                             
TradeDate datetime,                                  
SettlementDate datetime,                                                                                      
--2 relates to opening taxlots if it is closed else null                                          
Symbol varchar(50),                                                                                      
AssetID int,                                                                                 
UnderLyingID int,                                                    
ExchangeID int,                                                                                      
AUECID int,                                                                          
Multiplier float,                                                                                      
NotionalValue float,                                                                                      
TaxLotID varchar(50),                                                                                      
Price float,                                                     
Quantity float,                                                                                      
I1 float,                                                               
Commission float,                                                                                      
OtherBrokerFees float,                                                                                      
OtherFees float,                                                                              
SideMultiplier int,                                                                                    
Side varchar(50),                                                                     
DC int,                                                                                  
--DayDiff int,                                                                  
ConversionRate float,                                                                                      
ConversionMethod int,                                                                                
FxConversionRate float,                                                                                      
FxConversionMethod int,                                                                                      
OpeningTaxLotSideTagValue varchar(1),                                                                                      
OpeningTaxLot varchar(50),                                                                                      
OpeningTaxLotPrice float,                                                                                      
Q2 float,                                                                                      
I2 float,                                                                                      
OpeningTaxLotCommission float,               
OpeningTaxLotFees float,                                                                             
OpeningTaxLotTradeDate datetime,                                                        
ClosingMode int,                                                                                  
ClosingDate datetime,                                                                  
ClosingQty float,                                                                        
FundName varchar(50),                                                                                
MasterFundName varchar(50),                                                                                
GroupParameter1 varchar(50),                              
GroupParameter2 varchar(50),                                                                                
GroupParameter3 varchar(50),                                                                                
GroupParameter4 varchar(50),                                                                                
TickerSymbol varchar(50),                                                                                
AssetName varchar(50),                                                                                
SecurityTypeName varchar(50),                                                                                
SectorName varchar(50),                                                                                
SubSectorName varchar(50),                                                                          
CountryName varchar(50),                                                                                
CurrencySymbol varchar(20),                                                                                
LanguageName varchar(50),                                                                      
NotionalValueOpening float,                                                          
StrategyID int,                                                                                
StrategyName varchar(100),                                    
PrimeBrokerName varchar(50),                                                     
InternalAssetName varchar(50),                                                      
PutOrCall varchar(10) ,             
CounterParty varchar(100),                                    
CurrencyID int,                          
MasterStrategyName varchar(50),                      
UnderlyingSymbol Varchar(100),              
  BloombergSymbol Varchar(100),              
  SedolSymbol Varchar(100),              
  ISINSymbol Varchar(100),              
  CusipSymbol Varchar(100),              
  OSISymbol Varchar(100),              
  IDCOSymbol Varchar(100),    
TradeCurrency Varchar(5)                                                               
)                                                                                      
                
--------------------------------------------------------------------------------------------------             
If(@FetchDataOnBasisOf='ProcessDate')              
Begin                                                                                                                                          
-- insert TaxLotID details                                                                                      
insert into #TempReportTable                          
(                                     
TradeDate,                                                                                
SettlementDate,                                                                                
Symbol,                                                                                    
AssetID,                                                                                
UnderLyingID,                                                                                
ExchangeID,                                                                                
AUECID,                                                                           
Multiplier,                                                                                
NotionalValue,                    
TaxLotID,                                                   
Price,                      
Quantity,                                                                                
SideMultiplier,                                                                                
Side,                                                                                
I1,                                                                         
DC,                                                                                
Commission,                                                   
OtherBrokerFees,                                                                              
OtherFees,                                                                                    
ConversionRate,                                                                                
ConversionMethod,                                                                                
FxConversionRate,                                                                                
FxConversionMethod,                                                                                
--DayDiff,                                                                                  
OpeningTaxLot,                  
OpeningTaxLotPrice ,                                                                                
Q2,                                                                                
I2,                                                                                
OpeningTaxLotCommission,                                                                                
OpeningTaxLotFees,                                  
OpeningTaxLotTradeDate,                                                                          
FundName,                                                                                
MasterFundName,                                                                      
GroupParameter1,                                                                                
GroupParameter2,                                                                                
GroupParameter3,                                             
GroupParameter4,                                                                                
TickerSymbol,                                                                                
AssetName,                                                                                
SecurityTypeName,                                              
SectorName,                                                                                
SubSectorName,                                                                                
CountryName,                                                                                
CurrencySymbol,                                                                                
LanguageName,                                                        
StrategyID,                                                          
StrategyName,                                               
PrimeBrokerName,                                                        
InternalAssetName,                                                      
PutOrCall ,                                    
CounterParty,                                    
CurrencyID,                          
MasterStrategyName,                      
UnderlyingSymbol,            
BloombergSymbol,              
SedolSymbol,              
ISINSymbol,              
CusipSymbol,              
OSISymbol,              
IDCOSymbol,    
TradeCurrency                                                    
)                                                                                     
                                                                                     
Select                                            
VTL.ProcessDate,                                                                                
VTL.SettlementDate,                                                                            
VTL.Symbol AS Symbol,                                                                                    
T_AUEC.AssetID,                                                                            
T_AUEC.UnderLyingID,                                                                                
T_AUEC.ExchangeID,                                                                        
T_AUEC.AUECID,                                                                       
ISNULL(SM.Multiplier, 0) AS Multiplier,                                                                                
Case VTL.Quantity                         
 When 0                         
 Then 0                        
 Else isnull((isnull(VTL.NotionalValue, 0) * isnull(VTL.TaxlotQty, 0) / ISNULL(VTL.Quantity, 1)), 0)                          
End as NotionalValue,                                                   
TaxLotID,                                                                                  
VTL.AvgPrice,                                                     
TaxLotQty,                                                                                
dbo.GetSideMultiplier(VTL.OrderSideTagValue),                                                                                
T_Side.Side,                                                                                
isnull(BenchMarkRate+Differential,0) As I1,                                                                                
isnull(DayCount,0) as DC,                                                                                
isnull(VTL.Commission,0) as Commission,                                                  
isnull(VTL.OtherBrokerFees,0) as OtherBrokerFees,                                                                                  
isnull(VTL.StampDuty + VTL.TransactionLevy + VTL.ClearingFee + VTL.TaxOnCommissions + VTL.MiscFees,0) as OtherFees,                                                                                    
CASE C.BaseCurrencyID                                                                
  WHEN VTL.CurrencyID                                                                                
  THEN 1                                                                    
     ELSE                                 
      CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                   
  WHEN 0                                                                 
  THEN ISNULL(TGNZCF.ConversionFactor, 0)                                                                              
    ELSE ISNULL(VTL.FXRate_Taxlot,G.FXRate)                                             
   END                                                                 
 END AS ConversionRate,                                                 
                                        
 CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                 
     WHEN 0                                                                 
     THEN ISNULL(TGNZCF.ConversionMethod, 0)                                                                
     ELSE                                                                
    CASE ISNULL(ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator), 'M')                                                                  
    WHEN 'M'                                                                 
    THEN 0                                                                
    ELSE 1                                                          
    END                                                              
  END AS ConversionMethod,                                                              
                                                                              
CASE C.BaseCurrencyID                                                                
 WHEN SM.VsCurrencyID                                                                                
    THEN 1                                                                              
    ELSE                                           
     CASE                                                                 
       WHEN ISNULL(VTL.FXRate_Taxlot,G.FXRate) > 0       
       THEN ISNULL(VTL.FXRate_Taxlot,G.FXRate)                                                                           
       ELSE ISNULL(FXConversionData.ConversionFactor, 0)                                                                
     END                                                                 
    END AS FXConversionRate,                                                              
                                                             
 CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                  
     WHEN 0                                                                 
     THEN ISNULL(FXConversionData.ConversionMethod, 0)                                                                
     ELSE                                                                
        CASE                                                                 
    WHEN ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) = 'M'       
    THEN 0                      
    WHEN ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) ='D'       
 THEN 1                                                   
    ELSE ISNULL(FXConversionData.ConversionMethod, 0)                                                                
       END                                                              
   END AS FXConversionMethod,                                                                               
0 As OpeningTaxlot,                
0 As OpeningTaxlotPrice,                                                                          
0 As Q2,                                                                                
0 As I2,                                                                                
0 OpeningTaxlotCommission,                                                                                
0 OpeningTaxlotFees,                                                                          
getDate(),    --Column Count 30                                           
FundName,                                                                                
IsNull(CMF.MasterFundName,'Unassigned') as  MasterFundName,                                                         
'' As GroupParameter1,                                                                                
'' As GroupParameter2,                                                                                
'' As GroupParameter3,                                                                                
'' As GroupParameter4,                                                                                
ISNULL(SM.TickerSymbol, 'Undefined'),                                        
ISNULL(SM.AssetName, 'Undefined'),                                                                         
ISNULL(SM.SecurityTypeName, 'Undefined'),                                                                                
ISNULL(SM.SectorName, 'Undefined'),                                                            
ISNULL(SM.SubSectorName, 'Undefined'),                                                                                
ISNULL(SM.CountryName, 'Undefined'),                                                                                
                                                                               
SM.LeadCurrency AS CurrencySymbol,                                             
 '' AS LanguageName,                                                          
 Level2ID,                                                          
 CS.StrategyName AS StrategyName,                                                        
 TP.ThirdPartyName AS PrimeBrokerName,                                                        
 A.AssetName AS AssetName,                                                      
 ISNULL(SM.PutOrCall,'-1') AS PutOrCall,                                      
ISNULL(CP.ShortName,'Undefined') as CounterParty,                                    
Case                                  
 When VTL.AssetID=5 Or VTL.AssetID=11                                  
 Then SM.VsCurrencyID                                  
 Else VTL.CurrencyID                                  
End as CurrencyID,                          
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,            
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,            
IsNull(SM.BloombergSymbol,'') as BloombergSymbol,              
 IsNull(SM.SedolSymbol,'') as SedolSymbol,              
 IsNull(SM.ISINSymbol,'') as ISINSymbol,              
 IsNull(SM.CusipSymbol,'') as CusipSymbol,              
 IsNull(SM.OSISymbol,'') as OSISymbol,              
 IsNull(SM.IDCOSymbol,'') as IDCOSymbol,    
Currency.CurrencySymbol As TradeCurrency                               
                                                                                  
 from #Temp_Taxlots VTL join T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                    
    INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID                                                              
                                                                                      
join T_AUEC on VTL.AUECID=T_AUEC.AUECID                                                                                      
INNER JOIN T_Company C ON C.companyID = @companyID                                        
INNER JOIN #T_Asset A ON T_AUEC.AssetID = A.AssetID                              
Left Outer JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                                                             
LEFT OUTER JOIN T_Currency Currency on VTL.CurrencyID = Currency.CurrencyID                                                                                 
--LEFT OUTER JOIN T_Currency Curr on VTL.CurrencyID = Curr.CurrencyID                                                                                 
--LEFT OUTER JOIN T_Currency FXCurr on VTL.TradedCurrencyID = FXCurr.CurrencyID                           
                                                          
LEFT JOIN T_CompanyStrategy CS ON VTL.Level2ID = CS.CompanyStrategyID                                                        
                                                        
LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON VTL.FundID = CTPMD.InternalFundNameID_FK                                                        
LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                        
LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                      
                                                                
--left join SecurityMasterDB.dbo.V_GetSymbolUDAData as SM on VTL.Symbol=SM.TickerSymbol                                                                                                                                                
left join V_SecMasterData as SM on VTL.Symbol=SM.TickerSymbol                                                      
                                                              
LEFT JOIN #TEMPGetNonZeroConversionFactorForDate TGNZCF on VTL.CurrencyId = TGNZCF.FCID                                         
 AND C.BaseCurrencyID = TGNZCF.TCID                                                               
 AND DATEDIFF(d, TGNZCF.DateCC, VTL.ProcessDate) = 0                                                
                                                                                                            
--LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData on                                                                                                             
-- (FXConversionData.FCID = SM.LeadCurrencyID And FXConversionData.TCID = C.BaseCurrencyID)                                                           
-- and (DATEDIFF(d, FXConversionData.DateCC, VTL.AUECLocalDate) = 0)                                            
                                          
LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData                                          
On (FXConversionData.FCID = SM.VsCurrencyID                                           
And FXConversionData.TCID = C.BaseCurrencyID)              
And (DATEDIFF(d, FXConversionData.DateCC, VTL.ProcessDate) = 0)                                                                                         
                                                                                
LEFT OUTER JOIN #T_CompanyFunds CF ON VTL.FundID = CF.CompanyFundID                                                                                
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                    
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                       
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                    
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                            
--LEFT OUTER JOIN T_FutureMultipliers FM ON SUBSTRING(VTL.Symbol, 0, CHARINDEX(' ', VTL.Symbol)) = FM.Symbol                                                                              
Where datediff(d,VTL.ProcessDate,@startDate) <= 0                                                                                       
and Datediff(d,VTL.ProcessDate,@endDate) >=0                       
                                                                                     
--------------------------------------------------------------------------------------------------------                                                                                    
--update OpeningTaxLot details if any -- closing details                                                                      
Update  #TempReportTable                                                                                      
set 
OpeningTaxLot=#Temp_Taxlots.TaxLotID,                                                                                 
OpeningTaxLotPrice=#Temp_Taxlots.AvgPrice,                                                                                      
Q2=#Temp_Taxlots.Quantity,                                                                                      
I2 =#Temp_Taxlots.BenchMarkRate+#Temp_Taxlots.Differential,                                                                                      
OpeningTaxLotCommission =#Temp_Taxlots.Commission,                        
OpeningTaxLotFees =#Temp_Taxlots.OtherBrokerFees,                                                                                 
OpeningTaxLotTradeDate=#Temp_Taxlots.ProcessDate,                                                                          
ClosingMode=PM_TaxLotClosing.ClosingMode,                                                                  
ClosingQty=PM_TaxLotClosing.ClosedQty,                                                                                
ClosingDate=PM_TaxLotClosing.AUECLocalDate,                                                                      
NotionalValueOpening = isnull((isnull(#Temp_Taxlots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When #Temp_Taxlots.Quantity=0 Then 1 Else ISNULL(#Temp_Taxlots.Quantity, 1) End ), 0)                                                    
  
          
from PM_TaxLotClosing ,#Temp_Taxlots                                                                                      
where PM_TaxLotClosing.ClosingTaxLotID=#TempReportTable.TaxLotID                                                                                      
and #Temp_Taxlots.TaxLotID=PM_TaxLotClosing.PositionalTaxLotID                                                                                      
and (PM_TaxLotClosing.ClosingMode !=5  and PM_TaxLotClosing.ClosingMode !=2 )                                                                  
                                                           
                                                                        
                                                             
Update  #TempReportTable                                                                                      
set 
OpeningTaxLot=#Temp_Taxlots.TaxLotID,                                                                                      
OpeningTaxLotPrice=#Temp_Taxlots.AvgPrice,                                                                              
Q2=#Temp_Taxlots.Quantity,                                                                                      
I2 =#Temp_Taxlots.BenchMarkRate+#Temp_Taxlots.Differential,                                                                  
OpeningTaxLotCommission =#Temp_Taxlots.Commission,                                                                                      
OpeningTaxLotFees =#Temp_Taxlots.OtherBrokerFees,                                                                                  
OpeningTaxLotTradeDate=#Temp_Taxlots.ProcessDate,                                                                          
ClosingMode=PM_TaxLotClosing.ClosingMode,                                                                  
ClosingQty=PM_TaxLotClosing.ClosedQty,                                                                             
ClosingDate=PM_TaxLotClosing.AUECLocalDate,                                                               
NotionalValueOpening = isnull((isnull(#Temp_Taxlots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When #Temp_Taxlots.Quantity=0 Then 1 Else ISNULL(#Temp_Taxlots.Quantity, 1) End ), 0)                                                    
   
             
                                                                  
from PM_TaxLotClosing ,#Temp_Taxlots                                                                                      
where PM_TaxLotClosing.PositionalTaxLotID=#TempReportTable.TaxLotID                                                      
and #Temp_Taxlots.TaxLotID=PM_TaxLotClosing.ClosingTaxLotID                                                               
and (PM_TaxLotClosing.ClosingMode =5   or PM_TaxLotClosing.ClosingMode=2)               
End                                                                 
           
-------------------------------------------------------------------------------------------------      
             
Else--if Data fetching on the basis of AUECLocalDate              
Begin              
 insert into #TempReportTable                          
 (                                     
 TradeDate,                                                                                
 SettlementDate,                                                                                
 Symbol,                                                                                    
 AssetID,                                                                                
 UnderLyingID,                                                                                
 ExchangeID,                                                                     
 AUECID,                                                                           
 Multiplier,                                                                                
 NotionalValue,                                                                                    
 TaxLotID,                                                   
 Price,                                                                                
 Quantity,                                                                                
 SideMultiplier,                                                                                
 Side,                                               
 I1,                                                                         
 DC,                                                                                
 Commission,                                                                                
 OtherBrokerFees,                                                                              
 OtherFees,                                                                                    
 ConversionRate,                                                                                
 ConversionMethod,                                                                    
 FxConversionRate,                                                                                
 FxConversionMethod,                                                                                
 --DayDiff,                          
 OpeningTaxLot,                                                                                
 OpeningTaxLotPrice ,                                                                                
 Q2,                                                                                
 I2,                                                                                
 OpeningTaxLotCommission,                                                                                
 OpeningTaxLotFees,                                  
 OpeningTaxLotTradeDate,                                                                          
 FundName,                                                                                
 MasterFundName,                                                                      
 GroupParameter1,                                                                                
 GroupParameter2,                                
 GroupParameter3,                                             
 GroupParameter4,                                                                           
 TickerSymbol,                                                                                
 AssetName,                                                                                
 SecurityTypeName,                                              
 SectorName,                                                                                
 SubSectorName,                                                                                
 CountryName,                                                                                
 CurrencySymbol,                                                                                
 LanguageName,                                                        
 StrategyID,                                                          
 StrategyName,                                               
 PrimeBrokerName,                                                        
 InternalAssetName,                                                      
 PutOrCall ,                                    
 CounterParty,                                    
 CurrencyID,                          
 MasterStrategyName,                      
 UnderlyingSymbol,            
 BloombergSymbol,              
 SedolSymbol,              
 ISINSymbol,              
 CusipSymbol,              
 OSISymbol,              
 IDCOSymbol,    
TradeCurrency                                                
 )                                                                                     
                                                                                      
Select                                            
 VTL.AUECLocalDate,                                                                                
 VTL.SettlementDate,                                                                            
 VTL.Symbol AS Symbol,                                                                                    
 T_AUEC.AssetID,                                                                         
 T_AUEC.UnderLyingID,                                                                                
 T_AUEC.ExchangeID,                                                                          
 T_AUEC.AUECID,                                                                       
 ISNULL(SM.Multiplier, 0) AS Multiplier,                                                                                
 Case VTL.Quantity                         
  When 0                         
  Then 0                        
  Else isnull((isnull(VTL.NotionalValue, 0) * isnull(VTL.TaxlotQty, 0) / ISNULL(VTL.Quantity, 1)), 0)                          
 End as NotionalValue,                                                   
 TaxLotID,                                                                                  
 VTL.AvgPrice,                                                    
 TaxLotQty,                                                                                
 dbo.GetSideMultiplier(VTL.OrderSideTagValue),                                                                                
 T_Side.Side,                                                                                
 isnull(BenchMarkRate+Differential,0) As I1,                                                                                
 isnull(DayCount,0) as DC,                                                                                
 isnull(VTL.Commission,0) as Commission,                                                                                
 isnull(VTL.OtherBrokerFees,0) as OtherBrokerFees,                                                                                  
 isnull(VTL.StampDuty + VTL.TransactionLevy + VTL.ClearingFee + VTL.TaxOnCommissions + VTL.MiscFees,0) as OtherFees,                                                                                    
 CASE C.BaseCurrencyID                              
   WHEN VTL.CurrencyID                                                                                
   THEN 1                                                                    
   ELSE                                                                              
    CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                    
   WHEN 0                                                                 
   THEN ISNULL(TGNZCF.ConversionFactor, 0)                                                                              
   ELSE ISNULL(VTL.FXRate_Taxlot,G.FXRate)                                     
    END                                                                 
  END AS ConversionRate,                                                 
                                         
  CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                   
   WHEN 0                                                                 
   THEN ISNULL(TGNZCF.ConversionMethod, 0)                                                                
   ELSE                                                                
  CASE ISNULL(ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator), 'M')                                                                  
  WHEN 'M'                                                                 
  THEN 0                                                                
  ELSE 1                                                          
  END                                                              
   END AS ConversionMethod,                                                              
                                                                               
 CASE C.BaseCurrencyID                                                                
  WHEN SM.VsCurrencyID                                                
  THEN 1                                                                              
  ELSE                                           
   CASE                                                                 
     WHEN ISNULL(VTL.FXRate_Taxlot,G.FXRate) > 0         
     THEN ISNULL(VTL.FXRate_Taxlot,G.FXRate)                                                                         
     ELSE ISNULL(FXConversionData.ConversionFactor, 0)                      
   END                                                                 
  END AS FXConversionRate,                                                              
                                                              
  CASE ISNULL(ISNULL(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                  
   WHEN 0                                                                 
   THEN ISNULL(FXConversionData.ConversionMethod, 0)                                                                
   ELSE                          
   CASE                                                                 
  WHEN ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) = 'M'       
  THEN 0                                        
  WHEN ISNULL(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) ='D'       
  THEN 1                                                   
  ELSE ISNULL(FXConversionData.ConversionMethod, 0)                                                                
     END                                                              
    END AS FXConversionMethod,                                                                               
 0 As OpeningTaxlot,                                                                                
 0 As OpeningTaxlotPrice,                                                                                
 0 As Q2,                                                                                
 0 As I2,                                                                                
 0 OpeningTaxlotCommission,                                                                                
 0 OpeningTaxlotFees,                                                                          
 getDate(),    --Column Count 30                                                      
 FundName,                                                                                
 IsNull(CMF.MasterFundName,'Unassigned') as  MasterFundName,                                                                              
 '' As GroupParameter1,                                                                                
 '' As GroupParameter2,                                                                                
 '' As GroupParameter3,                                                       
 '' As GroupParameter4,                                                                                
 ISNULL(SM.TickerSymbol, 'Undefined'),                                        
 ISNULL(SM.AssetName, 'Undefined'),                                                                         
 ISNULL(SM.SecurityTypeName, 'Undefined'),                                                                                
 ISNULL(SM.SectorName, 'Undefined'),                                                            
 ISNULL(SM.SubSectorName, 'Undefined'),                                                                                
 ISNULL(SM.CountryName, 'Undefined'),                                                                                
                                                                                
 SM.LeadCurrency AS CurrencySymbol,                                             
  '' AS LanguageName,                                                          
  Level2ID,                                                          
  CS.StrategyName AS StrategyName,                                                        
  TP.ThirdPartyName AS PrimeBrokerName,                                                        
  A.AssetName AS AssetName,                                                      
  ISNULL(SM.PutOrCall,'-1') AS PutOrCall,                                      
 ISNULL(CP.ShortName,'Undefined') as CounterParty,                                    
 Case                                  
  When VTL.AssetID=5 Or VTL.AssetID=11                                  
  Then SM.VsCurrencyID                                  
  Else VTL.CurrencyID                                  
 End as CurrencyID,                          
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                      
 IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,            
 IsNull(SM.BloombergSymbol,'') as BloombergSymbol,              
 IsNull(SM.SedolSymbol,'') as SedolSymbol,              
 IsNull(SM.ISINSymbol,'') as ISINSymbol,              
 IsNull(SM.CusipSymbol,'') as CusipSymbol,              
 IsNull(SM.OSISymbol,'') as OSISymbol,              
 IsNull(SM.IDCOSymbol,'') as IDCOSymbol,    
Currency.CurrencySymbol As TradeCurrency                               
                                                                                   
  from #Temp_Taxlots VTL join T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                    
  INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID                                                    
                                                                                       
 join T_AUEC on VTL.AUECID=T_AUEC.AUECID                                                                                      
 INNER JOIN T_Company C ON C.companyID = @companyID                                                                          
 INNER JOIN #T_Asset A ON T_AUEC.AssetID = A.AssetID                                       
 Left Outer JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                                                             
 LEFT OUTER JOIN T_Currency Currency on VTL.CurrencyID = Currency.CurrencyID                                                                                  
 --LEFT OUTER JOIN T_Currency Curr on VTL.CurrencyID = Curr.CurrencyID                     
 --LEFT OUTER JOIN T_Currency FXCurr on VTL.TradedCurrencyID = FXCurr.CurrencyID                           
                                                           
 LEFT JOIN T_CompanyStrategy CS ON VTL.Level2ID = CS.CompanyStrategyID                                                        
 LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON VTL.FundID = CTPMD.InternalFundNameID_FK                                                        
 LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                        
 LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                        
 --left join SecurityMasterDB.dbo.V_GetSymbolUDAData as SM on VTL.Symbol=SM.TickerSymbol                                                                  
 left join V_SecMasterData as SM on VTL.Symbol=SM.TickerSymbol                                                      
                                                               
LEFT JOIN #TEMPGetNonZeroConversionFactorForDate TGNZCF on VTL.CurrencyId = TGNZCF.FCID                                         
AND C.BaseCurrencyID = TGNZCF.TCID                                                               
AND DATEDIFF(d, TGNZCF.DateCC, VTL.AUECLocalDate) = 0                                                
                                                                                                             
 --LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData on                                                                                                             
 -- (FXConversionData.FCID = SM.LeadCurrencyID And FXConversionData.TCID = C.BaseCurrencyID)                                                                                                             
 -- and (DATEDIFF(d, FXConversionData.DateCC, VTL.AUECLocalDate) = 0)                                            
                    
 LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData                                          
 On (FXConversionData.FCID = SM.VsCurrencyID                                           
 And FXConversionData.TCID = C.BaseCurrencyID)                             
 And (DATEDIFF(d, FXConversionData.DateCC, VTL.AUECLocalDate) = 0)                                                                                         
                                                                                 
 LEFT OUTER JOIN #T_CompanyFunds CF ON VTL.FundID = CF.CompanyFundID                                                                                
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                    
 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                       
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                  
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                            
 --LEFT OUTER JOIN T_FutureMultipliers FM ON SUBSTRING(VTL.Symbol, 0, CHARINDEX(' ', VTL.Symbol)) = FM.Symbol                                                                              
                                                                          
 where datediff(d,VTL.AUECLocalDate,@startDate) <= 0                                                                                       
 and Datediff(d,VTL.AUECLocalDate,@endDate) >=0                                    
                                                                                      
                                                                                     
 --update OpeningTaxLot details if any -- closing details                                                                      
update  #TempReportTable                                                                                      
set 
OpeningTaxLot=#Temp_Taxlots.TaxLotID,                                                                                 
OpeningTaxLotPrice=#Temp_Taxlots.AvgPrice,                                                                                      
Q2=#Temp_Taxlots.Quantity,                                                                                      
I2 =#Temp_Taxlots.BenchMarkRate+#Temp_Taxlots.Differential,                                                                                      
OpeningTaxLotCommission =#Temp_Taxlots.Commission,                                                                                      
OpeningTaxLotFees =#Temp_Taxlots.OtherBrokerFees,                                                                        
OpeningTaxLotTradeDate=#Temp_Taxlots.AUECLocalDate,                                                                          
ClosingMode=PM_TaxLotClosing.ClosingMode,                                                                  
ClosingQty=PM_TaxLotClosing.ClosedQty,                                                                                
ClosingDate=PM_TaxLotClosing.AUECLocalDate,                                                                      
NotionalValueOpening = isnull((isnull(#Temp_Taxlots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When #Temp_Taxlots.Quantity=0 Then 1 Else ISNULL(#Temp_Taxlots.Quantity, 1) End ), 0)                                                   

           
from PM_TaxLotClosing ,#Temp_Taxlots                                                                                      
where PM_TaxLotClosing.ClosingTaxLotID=#TempReportTable.TaxLotID                                                                                      
and #Temp_Taxlots.TaxLotID=PM_TaxLotClosing.PositionalTaxLotID                  
and (PM_TaxLotClosing.ClosingMode !=5  and PM_TaxLotClosing.ClosingMode !=2 )                                                                  
                                                            
                                       
                                                                         
update  #TempReportTable                                                                                      
set 
OpeningTaxLot=#Temp_Taxlots.TaxLotID,                                                                                      
OpeningTaxLotPrice=#Temp_Taxlots.AvgPrice,                                                                              
Q2=#Temp_Taxlots.Quantity,                                                                                      
I2 =#Temp_Taxlots.BenchMarkRate+#Temp_Taxlots.Differential,                               
OpeningTaxLotCommission =#Temp_Taxlots.Commission,                                                                                      
OpeningTaxLotFees =#Temp_Taxlots.OtherBrokerFees,                                                                                  
OpeningTaxLotTradeDate=#Temp_Taxlots.AUECLocalDate,                               
ClosingMode=PM_TaxLotClosing.ClosingMode,                                                                  
ClosingQty=PM_TaxLotClosing.ClosedQty,                                 
ClosingDate=PM_TaxLotClosing.AUECLocalDate,                                                                  
NotionalValueOpening = isnull((isnull(#Temp_Taxlots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When #Temp_Taxlots.Quantity=0 Then 1 Else ISNULL(#Temp_Taxlots.Quantity, 1) End ), 0)                                                   

                                                                   
from PM_TaxLotClosing ,#Temp_Taxlots                                                                                      
where PM_TaxLotClosing.PositionalTaxLotID=#TempReportTable.TaxLotID                                                      
and #Temp_Taxlots.TaxLotID=PM_TaxLotClosing.ClosingTaxLotID                                                               
and (PM_TaxLotClosing.ClosingMode =5   or PM_TaxLotClosing.ClosingMode=2)              
              
End              
       
    
Declare @SearchString varchar(max)        
      
set @SearchString  =replace(@SearchText,',',''',''')        
      
If(@SearchString <> '')        
 Begin        
  Exec      
('select                                          
 TradeDate,                                                                                      
 SettlementDate,                                                                                      
 --2 relates to opening taxlots if it is closed else null                                                                                      
 Symbol,                                                     
 AssetID,                                                                     
 UnderLyingID,                                                                                      
 ExchangeID,                                                                                      
 AUECID,                                                                                      
 Multiplier,                                                                                      
 NotionalValue,                                                                                      
 TaxLotID,                                                                                      
 Price,                                                 
 Quantity,                                                                                      
 I1,                                                                                      
 Commission,          
 OtherBrokerFees,                                                                                      
 OtherFees,                                                                              
 SideMultiplier,                                                                                    
 Side,                                                                                       
 DC,                                                    
 --DayDiff int,                                                                     
 ConversionRate,                                                   
 ConversionMethod,                                                                                      
 FxConversionRate,                                                                                      
 FxConversionMethod,                                                                                      
 OpeningTaxLotSideTagValue,                                                                                      
 OpeningTaxLot,                                     
 OpeningTaxLotPrice,                                                          
 Q2,                                  
 I2,                                                                                      
 OpeningTaxLotCommission,                                                                                      
 OpeningTaxLotFees,                                                    
 ISNULL(ClosingMode, 0) AS ClosingMode,                                                                            
 ISNULL(ClosingDate, getdate()) AS ClosingDate,                                                    
 FundName,                                                                                
 MasterFundName,                                                                                
 GroupParameter1,                                                                                
 GroupParameter2,                                                          
 GroupParameter3,                                                                                
 GroupParameter4,                                                                     
 TickerSymbol,                                                                                
 AssetName,-- UDAAssetName,                                                                                  
 SecurityTypeName,-- UDASecurityTypeName,                                                                                  
 SectorName ,--UDASectorName,                                                                                  
 SubSectorName,-- UDASubSectorName,                                                                                  
 CountryName,-- UDACountryName,                                                                                  
 CurrencySymbol,                                                                                
 LanguageName,                         
 OpeningTaxLotTradeDate,                                                                      
 NotionalValueOpening,                                                                  
 ClosingQty,                                                          
 StrategyID,                                                          
 StrategyName,                           
 PrimeBrokerName,                                                        
 InternalAssetName,                                                      
 PutOrCall ,                                    
 CounterParty,                                    
 CurrencyID,                          
 MasterStrategyName,                      
 UnderlyingSymbol,            
 BloombergSymbol,              
 SedolSymbol,              
 ISINSymbol,              
 CusipSymbol,              
 OSISymbol,              
 IDCOSymbol,    
 TradeCurrency                           
              
  from #TempReportTable Where ' + @SearchBy  + ' in ' + '(' + '''' + @SearchString +  '''' + ')' )         
 End        
Else        
 Begin        
-- return values                                                                            
select                                          
 TradeDate,                                                                                      
 SettlementDate,                                                                                      
 --2 relates to opening taxlots if it is closed else null                                                                                      
 Symbol,                                                     
 AssetID,                                                                     
 UnderLyingID,                                                                                      
 ExchangeID,                                                                                      
 AUECID,                                                                                      
 Multiplier,                                                                                      
 NotionalValue,                                                                                      
 TaxLotID,                                                                                      
 Price,                                                 
 Quantity,                                                                                      
 I1,                                                                                      
 Commission,            
 OtherBrokerFees,                                                                                      
 OtherFees,                                                                              
 SideMultiplier,                                                                                    
 Side,                                                                                       
 DC,                                                    
 --DayDiff int,                                                                     
 ConversionRate,                                                   
 ConversionMethod,                                                                                      
 FxConversionRate,                                                                                      
 FxConversionMethod,                                                                                      
 OpeningTaxLotSideTagValue,                                                                                      
 OpeningTaxLot,                                     
 OpeningTaxLotPrice,                                                          
 Q2,                                  
 I2,                                                                                      
 OpeningTaxLotCommission,                                                                                      
 OpeningTaxLotFees,                                                    
 ISNULL(ClosingMode, 0) AS ClosingMode,                                                                                  
 ISNULL(ClosingDate, getdate()) AS ClosingDate,                                                    
 FundName,                                                                                
 MasterFundName,                                                                                
 GroupParameter1,                                                                                
 GroupParameter2,                                                          
 GroupParameter3,                                                                                
 GroupParameter4,                                                                     
 TickerSymbol,                                                                                
 AssetName,-- UDAAssetName,                                                         
 SecurityTypeName,-- UDASecurityTypeName,                                                                                  
 SectorName ,--UDASectorName,                                                                                  
 SubSectorName,-- UDASubSectorName,                                                                                  
 CountryName,-- UDACountryName,                                                                                  
 CurrencySymbol,                                                                                
 LanguageName,                         
 OpeningTaxLotTradeDate,                                                                      
 NotionalValueOpening,                                                                  
 ClosingQty,                                                          
 StrategyID,                                                          
 StrategyName,                           
 PrimeBrokerName,                                                        
 InternalAssetName,                                                      
 PutOrCall ,                                    
 CounterParty,                                    
 CurrencyID,                          
 MasterStrategyName,                      
 UnderlyingSymbol,            
 BloombergSymbol,              
 SedolSymbol,              
 ISINSymbol,              
 CusipSymbol,              
 OSISymbol,              
 IDCOSymbol,    
 TradeCurrency                           
                                                                               
  from #TempReportTable     
    
 End                                   
-- drop the table         
    
                                          
                                          
drop table #TEMPGetNonZeroConversionFactorForDate ,#TempReportTable     
Drop Table #T_CompanyFunds,#Temp_Taxlots,#T_Asset