            
/***********************************************************************************************              
select * from t_companyfunds              
select * from t_asset              
              
[PMGetActivitySummaryReport_RJOCustomization]  '2014/08/19','2014/08/19','ProceeData','1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321','1,2,3,4,5,6,7,8,9,10,11'              
***********************************************************************************************/              
                            
                                                           
CREATE proc [dbo].[PMGetActivitySummaryReport_RJOCustomization]                                                                                                          
(                                                                                                        
 @startDate datetime,                                                                                                        
 @enddate datetime,                      
 @FetchDataOnBasisOf varchar(20),--ProceeData/TradeDate                        
 @Fund varchar(max),                          
 @Asset varchar(max)                                                                                                     
)                                                                                                        
as                                  
                              
Declare @BaseCurrencyID int                                                   
Set @BaseCurrencyID=(Select BaseCurrencyID from T_Company)                              
                            
Create Table #SecMasterDataTempTable                                                                                                                                                                                
(                                                                                                                                                                                 
  AUECID int,                                        
  TickerSymbol Varchar(100),                                                                                                                                                                                                                 
  CompanyName  VarChar(500),                                                                                                                                            
  AssetName Varchar(100),               
  AssetID int,                                                                                                                                                                                                               
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
  LeadCurrency Varchar(10),              
  BloombergSymbol Varchar(100),                                          
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
 AssetName,              
 AssetID,                                                                                                              
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
 LeadCurrency,              
 BloombergSymbol,                                          
 SedolSymbol,                                          
 ISINSymbol,                                          
 CusipSymbol,                                          
 OSISymbol,                                          
 IDCOSymbol                                                                                                                                             
     From V_SecMasterData                    
                  
Declare @T_FundIDs Table                                    
(                                    
 FundId int                                    
)                                    
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                           
                          
Declare @T_AssetIDs Table                                        
(                                        
 AssetId int                                        
)                                        
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')                              
                                       
 Select                                   
 T_Asset.*                               
 InTo #T_Asset                                     
 From T_Asset INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetId                            
                          
CREATE TABLE #T_CompanyFunds                                    
(                                    
 CompanyFundID int,                                    
 FundName varchar(50),                                    
 FundShortName varchar(50),                                    
 CompanyID int,                                    
 FundTypeID int,                        
 UIOrder int NULL                                   
)                           
                                 
Insert Into #T_CompanyFunds                           
Select               
 CompanyFundID,                                    
 FundName,                                    
 FundShortName,                                    
 CompanyID,                                    
 FundTypeID,                            
 UIOrder                      
 From T_CompanyFunds               
 INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                               
                                                                                    
                                                                                                        
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
ClearingFee float,    
StampDuty Float,            
ClearingBrokerFee float,            
MiscFees float,                                                                                             
SideMultiplier int,                                                                                                      
Side varchar(300),                                                                                       
DC int,                                                                                                  
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
CompanyName varchar(500),                                                                                                  
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
CounterParty varchar(10),                                                   
CurrencyID int,                                            
MasterStrategyName varchar(50),                                      
UnderlyingSymbol Varchar(100),                              
DividendLocal float,                              
DividendBase float,                              
CashValueBase float,              
BloombergSymbol Varchar(100),                                          
SedolSymbol Varchar(100),                               
ISINSymbol Varchar(100),                                          
CusipSymbol Varchar(100),                                          
OSISymbol Varchar(100),                                          
IDCOSymbol Varchar(100),    
CashFees float,  
IsCashFees bit                                                                                                                                                 
)                                                                                                        
               
-----------------------------------------------------------------------------------------------                          
If(@FetchDataOnBasisOf='ProcessDate')                      
Begin                                                                                                                                                         
 -- insert TaxLotID details                                                                                                        
 Insert InTo #TempReportTable                                            
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
ClearingFee,              
 StampDuty ,            
 ClearingBrokerFee ,            
 MiscFees ,                     
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
 CompanyName,                                                    
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
 DividendLocal,                              
 DividendBase,               
 CashValueBase,              
 BloombergSymbol,                                          
 SedolSymbol,                                          
 ISINSymbol,                                          
 CusipSymbol,                                          
 OSISymbol,                                          
 IDCOSymbol,    
 CashFees,  
 IsCashFees                                                                      
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
isnull(VTL.Commission,0)+isnull(VTL.SoftCommission,0) as Commission,                                                                  
isnull(VTL.OtherBrokerFees,0) as OtherBrokerFees,                                                                                        
isnull(VTL.StampDuty,0) + isnull(VTL.TransactionLevy,0) + isnull(VTL.ClearingFee,0) + isnull(VTL.TaxOnCommissions,0) + isnull(VTL.MiscFees,0) + isnull(VTL.SecFee,0) + isnull(VTL.OccFee,0) + isnull(VTL.OrfFee,0) as OtherFees,                              
  
    
      
        
isnull(VTL.ClearingFee,0)   as ClearingFee,    
            
isnull(VTL.StampDuty,0) as StampDuty,            
isnull(VTL.ClearingBrokerFee,0) as ClearingBrokerFee,            
isnull(VTL.MiscFees ,0) as MiscFees  ,                                                                                                     
 CASE @BaseCurrencyID                                                                                  
   WHEN VTL.CurrencyID                                                                                                  
   THEN 1                                                                   
   ELSE                                                                                                
    CASE ISNULL(Isnull(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                                
   WHEN 0                                                                                   
   THEN ISNULL(TGNZCF.ConversionFactor, 0)                                                                                                
   ELSE Isnull(VTL.FXRate_Taxlot,G.FXRate)                                                              
    END               
  END AS ConversionRate,                                                            
                                                           
  CASE ISNULL(Isnull(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                                 
   WHEN 0                                                       
   THEN ISNULL(TGNZCF.ConversionMethod, 0)                                     
   ELSE                                                                                  
  CASE ISNULL(Isnull(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator), 'M')                                                                                  
  WHEN 'M'                                                                                   
  THEN 0                                                                                  
  ELSE 1                                                                            
  END                                                            
   END AS ConversionMethod,                                                                                
                                                                                                   
 CASE @BaseCurrencyID                                                                                  
  WHEN SM.VsCurrencyID                                                                                                  
  THEN 1                                                                                                
  ELSE                                                
   CASE                                                                                   
     WHEN Isnull(VTL.FXRate_Taxlot,G.FXRate) > 0             
     THEN Isnull(VTL.FXRate_Taxlot,G.FXRate)                                                                                               
     ELSE ISNULL(FXConversionData.ConversionFactor, 0)                                                                                  
   END                                                                                   
  END AS FXConversionRate,                                                                            
                                                                                
  CASE ISNULL(Isnull(VTL.FXRate_Taxlot,G.FXRate), 0)                                                                                  
   WHEN 0                                             
   THEN ISNULL(FXConversionData.ConversionMethod, 0)                                
   ELSE                                                                                  
   CASE                                                                    
  WHEN Isnull(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) = 'M'             
  THEN 0                                            
  WHEN Isnull(VTL.FXConversionMethodOperator_Taxlot,G.FXConversionMethodOperator) ='D'             
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
 ISNULL(SM.CompanyName, 'Undefined'),                                                                                                                          
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
  A.AssetName AS InternalAssetName,                                                                        
  ISNULL(SM.PutOrCall,'-1') AS PutOrCall,                                                    
 ISNULL(CP.ShortName,'Undefined') as CounterParty,                                                      
 Case                                                    
  When VTL.AssetID=5 Or VTL.AssetID=11                                                    
  Then SM.VsCurrencyID                                
  Else VTL.CurrencyID                                                    
 End as CurrencyID,                  
                                            
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                      
 IsNull(SM.UnderlyingSymbol,'Unassigned') as UnderlyingSymbol,                              
 0 as DividendLocal,                              
 0 as DividendBase,                              
 0 as CashValueBase,              
 ISNULL(SM.BloombergSymbol,'Undefined')as BloombergSymbol,                                            
 ISNULL(SM.SedolSymbol,'Undefined') as SedolSymbol,                                            
 ISNULL(SM.ISINSymbol,'Undefined') as ISINSymbol,                                            
 ISNULL(SM.CusipSymbol,'Undefined') as CusipSymbol,                                            
 ISNULL(SM.OSISymbol, 'Undefined') as OSISymbol,                                           
 ISNULL(SM.IDCOSymbol,'Undefined') as IDCOSymbol,    
 0 as CashFees,  
 0 as IsCashFees                                                                               
                                           
from V_TaxLots VTL              
INNER JOIN #T_CompanyFunds CF ON VTL.FundID = CF.CompanyFundID                                                        
INNER JOIN T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                                      
INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID      
INNER JOIN T_AUEC on VTL.AUECID=T_AUEC.AUECID                                                                                                                                         
INNER JOIN #T_Asset A ON T_AUEC.AssetID = A.AssetID                
LEFT OUTER JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                                                                                                                               
LEFT OUTER JOIN T_CompanyStrategy CS ON VTL.Level2ID = CS.CompanyStrategyID                                                                     
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails CTPMD ON VTL.FundID = CTPMD.InternalFundNameID_FK                                                                          
LEFT OUTER JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                          
LEFT OUTER JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                                          
LEFT OUTER JOIN #SecMasterDataTempTable as SM on VTL.Symbol=SM.TickerSymbol                  
                
LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate TGNZCF on VTL.CurrencyId = TGNZCF.FCID                                                    
AND @BaseCurrencyID = TGNZCF.TCID AND DATEDIFF(d, TGNZCF.DateCC, VTL.ProcessDate) = 0                   
              
LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData                                                            
On (FXConversionData.FCID = SM.VsCurrencyID                                                             
And FXConversionData.TCID = @BaseCurrencyID)                                                                                                                               
And (DATEDIFF(d, FXConversionData.DateCC, VTL.ProcessDate) = 0)                                                                                                           
                             
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                      
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                         
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                      
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                            
                                                                 
Where datediff(d,VTL.ProcessDate,@startDate) <= 0                                                                                                         
and Datediff(d,VTL.ProcessDate,@endDate) >=0                                                      
                                                                                                         
------------------------------------------------------------------------------------------------------                  
 --update OpeningTaxLot details if any -- closing details                                  
Update  #TempReportTable                                                                                                        
Set               
OpeningTaxLot = V_TaxLots.TaxLotID,                                                                                                   
OpeningTaxLotPrice = V_TaxLots.AvgPrice,                                                                                                        
Q2 = V_TaxLots.Quantity,                                                                                                        
I2 = V_TaxLots.BenchMarkRate+V_TaxLots.Differential,                                                                                                        
OpeningTaxLotCommission = V_TaxLots.Commission,                                                                                                        
OpeningTaxLotFees = V_TaxLots.OtherBrokerFees,                                                                                                   
OpeningTaxLotTradeDate = V_TaxLots.ProcessDate,                                                                              
ClosingMode = PM_TaxLotClosing.ClosingMode,                                                                                    
ClosingQty = PM_TaxLotClosing.ClosedQty,                                                                               
ClosingDate = PM_TaxLotClosing.AUECLocalDate,                                                                                        
NotionalValueOpening = isnull((isnull(V_TaxLots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When V_TaxLots.Quantity=0 Then 1 Else ISNULL(V_TaxLots.Quantity, 1) End ), 0)                                                                
  
    
      
        
          
            
                  
From PM_TaxLotClosing ,V_TaxLots                                                                                                        
Where PM_TaxLotClosing.ClosingTaxLotID = #TempReportTable.TaxLotID                                                                                                  
And V_TaxLots.TaxLotID = PM_TaxLotClosing.PositionalTaxLotID                                                                                                       
And (PM_TaxLotClosing.ClosingMode !=5  and PM_TaxLotClosing.ClosingMode !=2 )                                                                                    
                                                       
                                                                                           
                                                                                           
Update  #TempReportTable                                                                                                        
Set               
OpeningTaxLot = V_TaxLots.TaxLotID,                                                                                
OpeningTaxLotPrice = V_TaxLots.AvgPrice,                                                                                                
Q2 = V_TaxLots.Quantity,                                                                                                        
I2 = V_TaxLots.BenchMarkRate + V_TaxLots.Differential,                                                                                                        
OpeningTaxLotCommission = V_TaxLots.Commission,                        
OpeningTaxLotFees = V_TaxLots.OtherBrokerFees,                                                                                                    
OpeningTaxLotTradeDate = V_TaxLots.ProcessDate,                                                                                            
ClosingMode = PM_TaxLotClosing.ClosingMode,                                                                   
ClosingQty = PM_TaxLotClosing.ClosedQty,                                                                                         
ClosingDate = PM_TaxLotClosing.AUECLocalDate,                                          
NotionalValueOpening = isnull((isnull(V_TaxLots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When V_TaxLots.Quantity=0 Then 1 Else ISNULL(V_TaxLots.Quantity, 1) End ), 0)                                                                 
  
    
     
        
           
           
              
From PM_TaxLotClosing ,V_TaxLots                                                                                                   
Where PM_TaxLotClosing.PositionalTaxLotID=#TempReportTable.TaxLotID                                                                        
And V_TaxLots.TaxLotID = PM_TaxLotClosing.ClosingTaxLotID                                                                                                        
And (PM_TaxLotClosing.ClosingMode = 5  Or PM_TaxLotClosing.ClosingMode = 2)               
                      
End                  
-----------------------------------------------------------------------------------------------------------              
                      
Else -- If fetching data on the basis of AUECLocalDate                      
Begin                      
-- insert TaxLotID details                                                                                                        
 Insert InTo #TempReportTable                                            
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
 ClearingFee,               
 StampDuty ,            
ClearingBrokerFee ,            
MiscFees ,                                                                                                   
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
 CompanyName,                                                    
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
 DividendLocal,                              
 DividendBase,                              
 CashValueBase,              
 BloombergSymbol,                                          
 SedolSymbol,                                          
 ISINSymbol,                                          
 CusipSymbol,                                          
 OSISymbol,                                          
 IDCOSymbol,    
CashFees ,  
IsCashFees                                                                    
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
isnull(VTL.Commission,0)+isnull(VTL.SoftCommission,0) as Commission,                                                                  
isnull(VTL.OtherBrokerFees,0) as OtherBrokerFees,                                                                                        
isnull(VTL.StampDuty,0) + isnull(VTL.TransactionLevy,0) + isnull(VTL.ClearingFee,0) + isnull(VTL.TaxOnCommissions,0) + isnull(VTL.MiscFees,0) + isnull(VTL.SecFee,0) + isnull(VTL.OccFee,0) + isnull(VTL.OrfFee,0) as OtherFees,                              
  
    
      
        
          
isnull(VTL.ClearingFee,0) as ClearingFee,            
isnull(VTL.StampDuty,0) as StampDuty,            
isnull(VTL.ClearingBrokerFee,0) as ClearingBrokerFee,            
isnull(VTL.MiscFees ,0) as MiscFees  ,                                                                                                      
 CASE @BaseCurrencyID                                                                                  
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
                                                  
 CASE @BaseCurrencyID                                                                                  
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
 ISNULL(SM.CompanyName, 'Undefined'),                                                                                                                            
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
  A.AssetName AS InternalAssetName,                                                                        
  ISNULL(SM.PutOrCall,'-1') AS PutOrCall,                                                    
 ISNULL(CP.ShortName,'Undefined') as CounterParty,                                                      
 Case                                                    
  When VTL.AssetID=5 Or VTL.AssetID=11                                                    
  Then SM.VsCurrencyID                             
  Else VTL.CurrencyID                                                    
 End as CurrencyID,                                            
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                      
 IsNull(SM.UnderlyingSymbol,'Unassigned') as UnderlyingSymbol,                              
 0 as DividendLocal,                              
 0 as DividendBase,                              
 0 as CashValueBase,              
 ISNULL(SM.BloombergSymbol,'Undefined')as BloombergSymbol,                                            
 ISNULL(SM.SedolSymbol,'Undefined') as SedolSymbol,                                            
 ISNULL(SM.ISINSymbol,'Undefined') as ISINSymbol,                                            
 ISNULL(SM.CusipSymbol,'Undefined') as CusipSymbol,                                            
 ISNULL(SM.OSISymbol, 'Undefined') as OSISymbol,                                           
 ISNULL(SM.IDCOSymbol,'Undefined') as IDCOSymbol,                                                                              
 0 as CashFees     ,  
0 as IsCashFees                                                                                                
  from V_TaxLots VTL join T_Side on T_Side.SidetagValue=VTL.OrderSidetagValue                                                                                                      
 INNER JOIN T_GROUP G ON VTL.GroupRefID = G.GroupRefID          
 INNER JOIN T_AUEC on VTL.AUECID=T_AUEC.AUECID                                                                                                        
 INNER JOIN #T_Asset A ON T_AUEC.AssetID = A.AssetID               
 INNER JOIN #T_CompanyFunds CF ON VTL.FundID = CF.CompanyFundID                                                          
 Left Outer JOIN  T_CounterParty CP ON CP.CounterPartyID= VTL.CounterPartyID                        
 LEFT JOIN T_CompanyStrategy CS ON VTL.Level2ID = CS.CompanyStrategyID                              
 LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON VTL.FundID = CTPMD.InternalFundNameID_FK                                                                          
 LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                          
 LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                                                                                                    
 left join #SecMasterDataTempTable as SM on VTL.Symbol=SM.TickerSymbol                
                                                                      
 LEFT JOIN #TEMPGetNonZeroConversionFactorForDate TGNZCF on VTL.CurrencyId = TGNZCF.FCID                                                                                        
 AND @BaseCurrencyID = TGNZCF.TCID                                                                                 
 AND DATEDIFF(d, TGNZCF.DateCC, VTL.AUECLocalDate) = 0                 
                                                               
 LEFT OUTER JOIN #TEMPGetNonZeroConversionFactorForDate FXConversionData                                                            
 On (FXConversionData.FCID = SM.VsCurrencyID                                            
 And FXConversionData.TCID = @BaseCurrencyID)                                                                    
 And (DATEDIFF(d, FXConversionData.DateCC, VTL.AUECLocalDate) = 0)                                                                                                           
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                      
 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                         
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                      
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                            
                                                         
 Where datediff(d,VTL.AUECLocalDate,@startDate) <= 0                                                                                                         
 and Datediff(d,VTL.AUECLocalDate,@endDate) >=0                                                      
                                                                                                         
-----------------------------------------------------------------------------------------------------------                                                                                                       
 --update OpeningTaxLot details if any -- closing details                               
Update  #TempReportTable                                                                                                        
Set               
OpeningTaxLot = V_TaxLots.TaxLotID,                                                                                                   
OpeningTaxLotPrice = V_TaxLots.AvgPrice,                                                                                                        
Q2 = V_TaxLots.Quantity,                      
I2 = V_TaxLots.BenchMarkRate + V_TaxLots.Differential,                                                                                                        
OpeningTaxLotCommission = V_TaxLots.Commission,                                                                                                        
OpeningTaxLotFees = V_TaxLots.OtherBrokerFees,                                                                                                   
OpeningTaxLotTradeDate = V_TaxLots.AUECLocalDate,                                                                              
ClosingMode = PM_TaxLotClosing.ClosingMode,                                                                                    
ClosingQty = PM_TaxLotClosing.ClosedQty,                                                                                       
ClosingDate = PM_TaxLotClosing.AUECLocalDate,                                                                                        
NotionalValueOpening = isnull((isnull(V_TaxLots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When V_TaxLots.Quantity=0 Then 1 Else ISNULL(V_TaxLots.Quantity, 1) End ), 0)                                                                
  
    
      
        
          
            
              
From PM_TaxLotClosing ,V_TaxLots                                                                                                        
Where PM_TaxLotClosing.ClosingTaxLotID=#TempReportTable.TaxLotID                                                                                                        
And V_TaxLots.TaxLotID = PM_TaxLotClosing.PositionalTaxLotID                                                                                                        
And (PM_TaxLotClosing.ClosingMode !=5  and PM_TaxLotClosing.ClosingMode !=2 )                        
                                                       
                                                                           
                                                                                           
Update  #TempReportTable                                                                                                        
Set               
OpeningTaxLot = V_TaxLots.TaxLotID,                                                                                                        
OpeningTaxLotPrice = V_TaxLots.AvgPrice,                                                                                                
Q2 = V_TaxLots.Quantity,                                                                                                        
I2 = V_TaxLots.BenchMarkRate+V_TaxLots.Differential,                                                                                                        
OpeningTaxLotCommission = V_TaxLots.Commission,                                                                                                        
OpeningTaxLotFees = V_TaxLots.OtherBrokerFees,                                                                                                    
OpeningTaxLotTradeDate = V_TaxLots.AUECLocalDate,          
ClosingMode = PM_TaxLotClosing.ClosingMode,                                                                                    
ClosingQty = PM_TaxLotClosing.ClosedQty,                                                                                               
ClosingDate = PM_TaxLotClosing.AUECLocalDate,                                          
NotionalValueOpening = isnull((isnull(V_TaxLots.NotionalValue, 0) * isnull(PM_TaxLotClosing.ClosedQty, 0) / Case When V_TaxLots.Quantity=0 Then 1 Else ISNULL(V_TaxLots.Quantity, 1) End ), 0)                                                                 
  
   
       
       
           
           
             
                 
From PM_TaxLotClosing ,V_TaxLots                                                                                     
Where PM_TaxLotClosing.PositionalTaxLotID = #TempReportTable.TaxLotID                                                                        
And V_TaxLots.TaxLotID = PM_TaxLotClosing.ClosingTaxLotID                                                                                           
And (PM_TaxLotClosing.ClosingMode = 5  Or PM_TaxLotClosing.ClosingMode = 2)                       
                      
End                                                                                   
                                                                                                                          
--------------------------------------------------------------------------------------------------------------                                      
                              
Update #TEMPGetNonZeroConversionFactorForDate                              
 Set ConversionFactor = 1.0/ConversionFactor                                                                                                 
 Where ConversionFactor <> 0 and ConversionMethod = 1                                                                                                                                                                                
                                                                                                                           
                                
--Union All -- Cash Activities                                
Insert InTo #TempReportTable                               
Select                                
                                                            
SubAccount.TraderDate as TradeDate,                                                                                                      
SubAccount.PayoutDate as SettlementDate,                                                                                                        
SubAccount.PBDesc as Symbol,                        
6 as AssetID,                                                       
0 as UnderLyingID,                                                                                                        
0 as ExchangeID,                                                                                                        
0 as AUECID,                                                                                                        
1 as Multiplier,                                                                                                        
0 as NotionalValue,                                                                                                        
'' as TaxLotID,                                                                                                        
0 as Price,                    
SubAccount.CashValue as Quantity,                                                                                                        
0 as I1,                                                                         
0 as Commission,                                                                                                        
0 as OtherBrokerFees,                                
0 as OtherFees,     
0 as ClearingFee,                  
0 as StampDuty ,            
0 as ClearingBrokerFee,           
0 as MiscFees,                        
Case                               
When SubAccount.CashValue >= 0                              
Then  1                              
Else -1                                                                                              
End as SideMultiplier,                                                                 
IsNull(SubAccount.PBDesc,'Undefined') as Side,                                                                                                         
0 as DC,                                                                                                    
0 as ConversionRate,                                                                                                     
0 as ConversionMethod,                                                                                                        
0 as FxConversionRate,                                                                                                        
0 as FxConversionMethod,                                             
0 as OpeningTaxLotSideTagValue,                                                                                                        
'' as OpeningTaxLot,                                                                                                        
0 as OpeningTaxLotPrice,                                                                                                        
0 as Q2,                                                    
0 as I2,                                                    
0 as OpeningTaxLotCommission,                                               
0 as OpeningTaxLotFees,                             
'' as OpeningTaxLotTradeDate,                                                                                        
0 AS ClosingMode,                                                                                                    
GetDate() AS ClosingDate,                             
0 As ClosingQty,                                                                                                  
CF.FundName as FundName,         
'Undefined' as CompanyName,                                                                                                 
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                         
'' as GroupParameter1,                                                                                                  
'' as GroupParameter2,                                                                            
'' as GroupParameter3,                                                                                                  
'' as GroupParameter4,                                                                                                  
'Undefined' as TickerSymbol,                                                                                                  
'Undefined' as AssetName,-- UDAAssetName,                                                                       
'Undefined' as SecurityTypeName,-- UDASecurityTypeName,                                                                                                    
'Undefined' as SectorName ,--UDASectorName,                                     
'Undefined' as SubSectorName,-- UDASubSectorName,                                                                                                    
'Undefined' as CountryName,-- UDACountryName,                                                                                                    
'' as CurrencySymbol,                                                                                                  
'' as LanguageName,                                                                                            
0  as NotionalValueOpening,                                                                                    
0 As StrategyID,                                     
'Undefined' as StrategyName,                                                                          
IsNull(TP.ThirdPartyName,'Undefined') AS PrimeBrokerName,                                                                          
'Cash' as InternalAssetName,                                                                        
'' as PutOrCall ,                                                      
'Undefined' as CounterParty,                                                      
SubAccount.CurrencyID as CurrencyID,                                            
'Undefined' as MasterStrategyName,                                      
'Undefined' as UnderlyingSymbol,                              
0 as DividendLocal,                              
0 as DividendBase,                              
Case                                                                                                                                    
  When SubAccount.CurrencyID =  @BaseCurrencyID                                                                                                                                                       
  Then SubAccount.CashValue                                                      
  Else IsNull(FXDayRatesForSettleDate.ConversionFactor,0) * SubAccount.CashValue                                       
End as CashValueBase,              
'Undefined' as BloombergSymbol,                                            
'Undefined' as SedolSymbol,                                            
'Undefined' as ISINSymbol,                      
'Undefined' as CusipSymbol,                                            
'Undefined' as OSISymbol,                                           
'Undefined' as IDCOSymbol,    
 SubAccount.CashValue as CashFees    ,  
 1 as IsCashFees                                      
                                                                                                
From       
 (      
  SELECT DISTINCT           
  TOP (100) PERCENT Journal.TaxLotID,           
  Journal.FundID,           
  Journal.SubAccountID,          
  Journal.Symbol,           
  Journal.PBDesc,           
  Journal.TransactionDate AS PayOutDate,           
  Journal.TransactionID AS CashID,           
  Journal.DR - Journal.CR AS CashValue,           
  CurrencyConversionRate.ConversionRate AS FXRate,           
  0 AS IsAutomatic,           
  Journal.TransactionDate AS AccrueDate,           
  Journal.TransactionDate AS TraderDate,           
  SubAccounts.TransactionTypeID,           
  TransactionType.TransactionType,           
  CurrencyConversionRate.ConversionRateID,          
  Journal.CurrencyID           
  FROM           
  dbo.T_Journal AS Journal         
  INNER JOIN dbo.T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID         
  INNER JOIN dbo.T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID           
  INNER JOIN dbo.T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID         
  LEFT OUTER JOIN dbo.T_CurrencyStandardPairs AS CurrencyStandardPairs         
  ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID           
  AND CurrencyStandardPairs.ToCurrencyID = (SELECT TOP (1) BaseCurrencyID FROM  dbo.T_Company)         
  LEFT OUTER JOIN  dbo.T_CurrencyConversionRate AS CurrencyConversionRate         
  ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID         
  AND CurrencyConversionRate.Date = Journal.TransactionDate          
  WHERE ((Journal.TaxLotID IS NULL) OR (Journal.TaxLotID = ''))         
  AND (TransactionType.TransactionType = 'Cash')          
  and Journal.TransactionSource = 2    
  and transactionid not in(select transactionid from t_journal where transactionsource=2 and subaccountid in (41,92,93))         
 ) SubAccount             
INNER JOIN #T_CompanyFunds CF ON SubAccount.FundID = CF.CompanyFundID                              
INNER JOIN T_Currency CLocal ON SubAccount.CurrencyID = CLocal.CurrencyID                                 
LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON SubAccount.FundID = CTPMD.InternalFundNameID_FK                                                                          
LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                          
LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                                            
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                           
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                               
LEFT JOIN #TEMPGetNonZeroConversionFactorForDate FXDayRatesForSettleDate on FXDayRatesForSettleDate.FCID=SubAccount.CurrencyID                                                                                        
AND FXDayRatesForSettleDate.TCID = @BaseCurrencyID AND DateDiff(d,SubAccount.TraderDate,FXDayRatesForSettleDate.DateCC)=0                                   
Where DateDiff(d,@StartDate,SubAccount.TraderDate) >=0                                                                                                             
And DateDiff(d,SubAccount.TraderDate,@EndDate) >=0           
                          
                              
--Union All -- Dividend                              Insert InTo #TempReportTable                                 
Select                                
                                                            
CashDiv.ExDate as TradeDate,                                                                                                      
CashDiv.PayoutDate as SettlementDate,                                                                                                        
CashDiv.Symbol as Symbol,                                                                       
Isnull(SM.AssetID,0) as AssetID,                                                                                                
0 as UnderLyingID,                                                                                                        
0 as ExchangeID,                                                                                                        
Isnull(SM.AUECID,0) as AUECID,                                                    
1 as Multiplier,                                       
0 as NotionalValue,                                                                                                        
'' as TaxLotID,                                                                                                    
0 as Price,                     
0 as Quantity,                                                                                                        
0 as I1,                                                                                                        
0 as Commission,                                                                                                        
0 as OtherBrokerFees,                                                                                                        
0 as OtherFees,    
0 as ClearingFee,             
0 as StampDuty ,            
0 as ClearingBrokerFee ,            
0 as MiscFees ,                             
Case                               
When CashDiv.Dividend >= 0                              
Then 1                              
Else -1                              
End As SideMultiplier,                                                                                                      
'Dividends' as Side,                                                                                                         
0 as DC,                                                                                                    
0 as ConversionRate,                                                                                                     
0 as ConversionMethod,                                                                        
0 as FxConversionRate,                                                                                                        
0 as FxConversionMethod,                                                                                                        
0 as OpeningTaxLotSideTagValue,                                                                                                        
0 as OpeningTaxLot,                            
0 as OpeningTaxLotPrice,                                                                                                        
0 as Q2,                                                    
0 as I2,                                                                                                        
0 as OpeningTaxLotCommission,                                                          
0 as OpeningTaxLotFees,                            
'' as OpeningTaxLotTradeDate,                                                                     
0 AS ClosingMode,                                                                                                    
getdate() AS ClosingDate,                            
0 as ClosingQty,                                                                            
#T_CompanyFunds.FundName as FundName,          
'Undefined' as CompanyName,      
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                                                                                                  
'' as GroupParameter1,                                                                                                  
'' as GroupParameter2,                                                                            
'' as GroupParameter3,                                                                                                  
'' as GroupParameter4,                                                                                                  
ISNULL(SM.TickerSymbol, 'Undefined') as TickerSymbol,                                                                                                  
IsNull(SM.AssetName,'Undefined') as AssetName,-- UDAAssetName,                                                                                                    
IsNull(SM.SecurityTypeName,'Undefined') as SecurityTypeName,-- UDASecurityTypeName,                                                                                                    
IsNull(SM.SectorName,'Undefined') as SectorName ,--UDASectorName,                                                                                                    
IsNull(SM.SubSectorName,'Undefined') as SubSectorName,-- UDASubSectorName,                                                                                
IsNull(SM.CountryName,'Undefined') as CountryName,-- UDACountryName,                                                                                                    
IsNull(CLocal.CurrencySymbol,'') as CurrencySymbol,                                
'' as LanguageName,                                                                     
'' as NotionalValueOpening,                                              
0 as StrategyID,                                                                            
'Undefined' as StrategyName,                                                                          
IsNull(TP.ThirdPartyName,'Undefined') as PrimeBrokerName,                                                                          
'Cash' as InternalAssetName,                                                                        
'' as PutOrCall ,                                                      
'Undefined' as CounterParty,                                                      
IsNull(SM.CurrencyID,0) as CurrencyID,                                            
'Undefined' as MasterStrategyName,                                      
IsNull(SM.UnderlyingSymbol,'Undefined') as UnderlyingSymbol,                              
IsNull(CashDiv.Dividend,0) as DividendLocal,                               
Case                                                                                                                                    
  When SM.CurrencyID =  @BaseCurrencyID                                                                                                                                                       
  Then CashDiv.Dividend                                                      
  Else IsNull(FXDayRatesForDiviDate.ConversionFactor,0) * CashDiv.Dividend                           
End as DividendBase,                              
0 as CashValueBase,              
 ISNULL(SM.BloombergSymbol,'Undefined')as BloombergSymbol,                                            
 ISNULL(SM.SedolSymbol,'Undefined') as SedolSymbol,                                            
 ISNULL(SM.ISINSymbol,'Undefined') as ISINSymbol,                                            
 ISNULL(SM.CusipSymbol,'Undefined') as CusipSymbol,                                            
 ISNULL(SM.OSISymbol, 'Undefined') as OSISymbol,                                           
 ISNULL(SM.IDCOSymbol,'Undefined') as IDCOSymbol,    
 0 as CashFees ,  
 0 as IsCashFees                                                                          
                                           
from T_TaxlotCashDividends CashDiv               
Inner Join #T_CompanyFunds ON  CashDiv.FundID= #T_CompanyFunds.CompanyFundID                
LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = CashDiv.Symbol              
--Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                                                                                                     
Inner Join #T_Asset On #T_Asset.AssetID=SM.AssetId                                                                                                                      
LEFT OUTER JOIN T_Currency CLocal ON SM.CurrencyID = CLocal.CurrencyID                                                                                                                                                                                        
  
LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON CashDiv.FundID = CTPMD.InternalFundNameID_FK                                                                          
LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                          
LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                                                                                             
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID               LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                      
 
    
      
        
         
LEFT JOIN #TEMPGetNonZeroConversionFactorForDate FXDayRatesForDiviDate on FXDayRatesForDiviDate.FCID=SM.CurrencyID                                                                                   
AND FXDayRatesForDiviDate.TCID = @BaseCurrencyID AND DateDiff(d,CashDiv.ExDate,FXDayRatesForDiviDate.DateCC)=0                                                                                                                             
Where DateDiff(d,@StartDate,CashDiv.ExDate) >=0                                                                                                                
and DateDiff(d,CashDiv.ExDate,@EndDate)>=0                             
                            
-- return values                                                                                              
Select                                                            
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
ClearingFee,                
StampDuty ,            
ClearingBrokerFee ,            
MiscFees ,                                                                                            
SideMultiplier,                   
Side,                                                                                                         
DC,                                    
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
CompanyName,                         
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
DividendLocal,                              
DividendBase,                              
CashValueBase,        
BloombergSymbol,    
CashFees,     
IsCashFees                                       
                            
From #TempReportTable                                  
--       order by Symbol                                             
-- drop the table                           
                                                            
drop table #TEMPGetNonZeroConversionFactorForDate                                                                                                   
drop table #TempReportTable,#SecMasterDataTempTable            
drop table #T_CompanyFunds, #T_Asset 