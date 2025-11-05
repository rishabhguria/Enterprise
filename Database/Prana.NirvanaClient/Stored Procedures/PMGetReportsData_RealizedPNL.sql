
/* ******************************************************************                                         
case                                           
when  datediff(d,#temp2.tradeDate,@startDate)<0                                          
then                                           
dbo.getformatteddatepart(#temp2.tradeDate)<=dbo.getformatteddatepart(#TempMP.date)                                            
else                                           
dbo.getformatteddatepart(@startDate)<=dbo.getformatteddatepart(#TempMP.date)                                              
exec PMGetReportsData_RealizedPNL 5,'02-01-2011','10-10-2012','1182,1183','1,2,3,4'                                        
Select * from T_Company          
          
Modified Date: 24-April-2012                                                
Modified By : Sandeep Singh                                                                                                                    
<Description: FX Spot, FX Forward and Fixed Income related changes>             
                            
Modified Date: 27-Sep-2012      
Modified By: Rahul Gupta      
Description: FX level implementation at taxlot level                
***********************************************************************/                                                                                                                   
                                                                                                                    
CREATE PROCEDURE [dbo].[PMGetReportsData_RealizedPNL]                                                                                                  
(                                                                                                                                                                          
  @CompanyID int,                                                                                                  
  @StartDate datetime,--in Historical All auec date remain same                                                                                                                      
  @EndDate datetime,      
  @Fund varchar(max),                            
  @Asset varchar(max)                                                                                                                                                          
)                                                                                                                                                                          
As                                                                                                                                                                              
                                                                                                                      
Begin                                                                                                                                                              
                                                                                                                                          
 declare @toAllAUECDatesString varchar(max)                                                                                                  
 declare @fromAllAUECDatesString varchar(max)                                                                                                  
 set @toAllAUECDatesString = dbo.GetAUECDateString(@endDate)                                                                                                  
 set @fromAllAUECDatesString = dbo.GetAUECDateString(@startDate)                                                                                                  
                                                                                                   
 Create Table #ToAUECDatesTable                            
 (                                                    
  AUECID int,                              
  CurrentAUECDate DateTime                                                    
 )                                                                                                                                                          
                                                       
 Insert Into #ToAUECDatesTable                                            
   Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)                                                       
                                                                                                               
 Create Table #FromAUECDatesTable                            
 (                                                    
   AUECID int,                                                    
   CurrentAUECDate DateTime                                                    
 )                             
                                                                                                 
Insert Into #FromAUECDatesTable                                                     
   Select * From dbo.GetAllAUECDatesFromString(@FromAllAUECDatesString)                                                   
                                                                                
                                          
DECLARE @startingDate DateTime                                       
Set @startingDate = DateAdd(yy, -20, @endDate)                                                                                                            
                                                                                                            
 Create Table #TEMPGetConversionFactorForGivenDateRange                                                     
 (                                                                                                           
  FCID int,                                                                            
  TCID int,                                         
  ConversionFactor float,                                                                  
  ConversionMethod int,                                                                                                                                          
  DateCC DateTime                                                                                                                                                               
 )                                                        
 INSERT INTO #TEMPGetConversionFactorForGivenDateRange                                                                                                                                      
 SELECT                                                                                                                        
  FromCurrencyID,                                                            
  ToCurrencyID,                                                                      
  RateValue,                                                          
  ConversionMethod,                                                                                                                             
  Date                                                                                             
 FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startingDate, @endDate)                                             
                                           
Update #TEMPGetConversionFactorForGivenDateRange                                                                          
Set ConversionFactor = 1.0/ConversionFactor                                                                                                                    
Where ConversionFactor <> 0 and ConversionMethod = 1                                                                                                                    
                                    
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
T_CompanyFunds.CompanyFundID, 
T_CompanyFunds.FundName,
T_CompanyFunds.FundShortName,
T_CompanyFunds.CompanyID, 
T_CompanyFunds.FundTypeID,
T_CompanyFunds.UIOrder                                                 
 From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                                   
 Where T_CompanyFunds.IsActive=1                                                                                                           
                                                                                                                                                          
                                        
                                        
-- get Security Master Data in a Temp Table                                              
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
  BloombergSymbol Varchar(100),                                                                                                 
  SedolSymbol Varchar(100),                      
  ISINSymbol Varchar(100),                      
  CusipSymbol Varchar(100),                      
  OSISymbol Varchar(100),                      
  IDCOSymbol Varchar(100),        
  AssetID int
--UDAAssetName varchar(100),                                            
--SecurityTypeName varchar(100),                                            
--SectorName varchar(100),                                            
--SubSectorName varchar(100),                                            
--CountryName varchar(100)                                                                
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
 BloombergSymbol,                                                                                           
 SedolSymbol,                      
 ISINSymbol,                      
 CusipSymbol,                      
 OSISymbol,                      
 IDCOSymbol,        
 AssetID
--AssetName,                                            
--SecurityTypeName,                                            
--SectorName,                                            
--SubSectorName,                                            
--CountryName                                                                                 
     From V_SecMasterData                                             
                                            
--for international future                                             
Declare @BaseCurrencyID int                                                                                                                                                                
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company Where CompanyID=@CompanyID )                                                
                          
--Create Table #RealizedPNLDataTable                          
--(                          
--PositionalTaxlotID Varchar(50),                          
--ClosingTaxlotID Varchar(50),                          
--Symbol Varchar(100),                          
--PositionSideID Varchar(5),                          
--ClosingSideID Varchar(5),                          
--PositionTradeDate DateTime,                          
--ClosingTradeDate DateTime,                          
--OpenPrice float,                          
--ClosingPrice float,                          
--FundID int,                 
--StrategyID int,                          
--AssetID int,                          
--UnderLyingID int,                          
--ExchangeID int,                          
--CurrencyID int,                          
--PositionalTaxlotCommission float,                          
--ClosingTaxlotCommission float,                          
--ClosingMode int,                          
--Multiplier float,                          
--OpeiningPositionTag int,                          
--ClosingPositionTag int,                          
--ClosedQty float,                          
--PositionNotionalValue float,                          
--PositionBenchMarkRate float,                          
--PositionDifferential float,                          
--PositionOrigCostBasis float,                          
--PositionDayCount float,                          
--PositionFirstResetDate DateTime,                          
--PositionOrigTransDate DateTime,                          
--NotionalValue float,                          
--BenchMarkRate float,                          
--Differential float,                          
--OrigCostBasis float,                          
--DayCount float,                          
--FirstResetDate DateTime,                          
--OrigTransDate DateTime,                          
--IsOpeningSwapped bit,                          
--IsClosingSwapped bit,                          
--FundName varchar(100),                          
--MasterFundName Varchar(100),                          
--OpeningSide Varchar(50),                          
--ClosingSide Varchar(50),                          
--StrategyName Varchar(100),                          
--UDATickerSymbol Varchar(100),                          
--UDAAssetName Varchar(100),                          
--UDASecurityTypeName Varchar(100),                          
--UDASectorName Varchar(100),                          
--UDASubSectorName Varchar(100),                          
--UDACountryName Varchar(100),                          
--PrimeBrokerName Varchar(100),                          
--AssetName Varchar(100),                          
--OTConvFactor float,                          
--OTConvMethod Varchar(5),                          
--OTFXConvFactor float,                          
--OTFXConvMethod Varchar(5),                          
--CTConvFactor float,                          
--CTConvMethod Varchar(5),                          
--CTFXConvFactor float,                          
--CTFXConvMethod Varchar(5),                          
--CompanyName Varchar(100),                          
--PutOrCall Varchar(5),                          
--PositionSide Varchar(5),                          
--MasterStrategyName Varchar(100),                          
--UnderlyingSymbol Varchar(100)                          
--)                                
--                          
--Insert into #RealizedPNLDataTable          
Select                                                                                                       
 PTC.PositionalTaxlotID,                                                                                          
 PTC.ClosingTaxlotID,                                                                                      
 PT.Symbol as Symbol,                                                                                                 
 G.OrderSideTagValue as PositionSideID,                                                                                      
 G1.OrderSideTagValue as ClosingSideID,                                                                                                      
 G.ProcessDate as PositionTradeDate,                                        
 --G1.CreationDate as ClosingTradeDate, --now closing taxlot Trade date is cloisng date                                                                                                   
 PTC.AUECLocalDate as ClosingTradeDate, --now closing taxlot Trade date is cloisng date         
--G.AvgPrice /dbo.[GetSplitFactorForReports](G.AUECLocalDate,PTC.AUECLocalDate,PT.TaxLotID) As OpenPrice,                                                                                                 
 PT.AvgPrice as OpenPrice, --/ IsNull(SplitTab.SplitFactor,1) as OpenPrice ,                                                                                                      
 PT1.AvgPrice as ClosingPrice ,                                                                                                      
 PT.FundID as FundID,                                                                                                    
 PT.Level2ID as StrategyID,                                                                                                    
 G.AssetID,                                                                                                    
 G.UnderLyingID,                                                                               
 G.ExchangeID,                                                            
 G.CurrencyID,         
 PT.ClosedTotalCommissionandFees as PositionalTaxlotCommission,                                                                                                      
 PT1.ClosedTotalCommissionandFees as ClosingTaxlotCommission,                                                                                                      
 PTC.ClosingMode as ClosingMode,                                                    
 ISNULL(SM.Multiplier, 0) AS Multiplier,                              
Case dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue)                                                                                
 When 1                                                                                                                        
  Then 0                                                                                                
 When -1                                                                                                                                   
  Then 1                                                                                                                                                              
 Else 0                                                                                       
End as OpeiningPositionTag,                               
 isnull(PT1.PositionTag, 0) as ClosingPositionTag,                                                                                                      
 PTC.ClosedQty  ,                                       
 isnull(SW.NotionalValue,0) as PositionNotionalValue,                                                                                                                
 isnull(SW.BenchMarkRate,0) as PositionBenchMarkRate,                                    
 isnull(SW.Differential,0) as PositionDifferential,                                                                                                                
 isnull(SW.OrigCostBasis,0) as PositionOrigCostBasis,                                                                                                                      
 isnull(SW.DayCount,0) as PositionDayCount,                        
 SW.FirstResetDate as PositionFirstResetDate,                                                                                                                
 SW.OrigTransDate as PositionOrigTransDate,                                                                                                       
 isnull(SW1.NotionalValue,0) as NotionalValue,                                                    
 isnull(SW1.BenchMarkRate,0) as BenchMarkRate,                                                                                                                
 isnull(SW1.Differential,0) as Differential,                                                              
isnull(SW1.OrigCostBasis,0) as OrigCostBasis,                                                                                                                     
 isnull(SW1.DayCount,0) as DayCount,                                           
 SW1.FirstResetDate as FirstResetDate,                                                                                                      
 SW1.OrigTransDate as OrigTransDate ,                                                                                                    
 isnull(G.IsSwapped, 0) AS IsOpeningSwapped,                                                                                                  
 isnull(G1.IsSwapped, 0) AS IsClosingSwapped,                                                             
 FundName,                   
 IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                                                                                              
 TS.Side AS OpeningSide,                                                             
 TS1.Side AS ClosingSide,                                                                      
 CS.StrategyName AS StrategyName,    

                                                                                                  
 ISNULL(SM.TickerSymbol, 'Undefined') AS UDATickerSymbol,                                                                      
 ISNULL(SM.AssetName, 'Undefined') AS UDAAssetName,                                                                                                   
 ISNULL(SM.SecurityTypeName, 'Undefined') AS UDASecurityTypeName,                                                                   
 ISNULL(SM.SectorName, 'Undefined') AS UDASectorName,                                         
 ISNULL(SM.SubSectorName, 'Undefined') AS UDASubSectorName,                                                                                                          
 ISNULL(SM.CountryName, 'Undefined') AS UDACountryName,                                                                                  
 
TP.ThirdPartyName AS PrimeBrokerName,                                 
 A.AssetName AS AssetName,                                                                                              
                                                                                
CASE Comp.BaseCurrencyID                                                                                                   
 WHEN G.CurrencyID                                                                                                         
 THEN 1                                                                               
 ELSE                                                     
  CASE ISNULL(IsNull(PT.FXRate,G.FXRate), 0)                                                                                      
   WHEN 0                                                                                       
   THEN ISNULL(OTConvFactor.ConversionFactor, 0)                                                                            
   ELSE IsNull(PT.FXRate,G.FXRate)                             
  END                                                                                       
END AS OTConvFactor,                                                    
                                                    
CASE ISNULL(IsNull(PT.FXRate,G.FXRate), 0)                                                                                     
 WHEN 0                                                     
 THEN 0 --THEN ISNULL(OTConvFactor.ConversionMethod, 0)                                                                             
 ELSE                                                                             
  CASE ISNULL(ISNULL(PT.FXConversionMethodOperator,G.FXConversionMethodOperator), 'M')                                                                                      
   WHEN 'M'                                                     
   THEN  0                                         
   ELSE  1                                                                    
  END        
END AS OTConvMethod,                                                    
        
-- Previously we were handling FX rate for Normal trades and FX spot & Forward trades in different ways,     
-- now FX Rate for both types will be from the same destination                                                                          
CASE Comp.BaseCurrencyID                                                                                                   
 WHEN SM.VsCurrencyID                                                                                        
 THEN 1                                                                                                        
 ELSE                                                                            
  CASE                                
   WHEN IsNull(PT.FXRate,G.FXRate) > 0                                                                                       
   THEN IsNull(PT.FXRate,G.FXRate)                                        
 ELSE ISNULL(OTConvFactor.ConversionFactor, 0)                             
  END                                                                                    
END AS OTFXConvFactor,                                                    
                                                    
CASE ISNULL(IsNull(PT.FXRate,G.FXRate), 0)                                                  
 WHEN 0                                                     
 THEN 0                                                                             
 ELSE                                                                                      
  CASE                                                                                       
   WHEN ISNULL(ISNULL(PT.FXConversionMethodOperator,G.FXConversionMethodOperator), 'M')='M'                                                            
   THEN 0                                                   
   WHEN ISNULL(ISNULL(PT.FXConversionMethodOperator,G.FXConversionMethodOperator), 'D')='D'                                                          
   THEN 1                                                                                       
   ELSE ISNULL(OTConvFactor.ConversionMethod, 0)                                             
  END                                                             
END AS OTFXConvMethod,                                                                            
                                                                            
CASE Comp.BaseCurrencyID                                                                                                   
 WHEN G1.CurrencyID                                                                              
 THEN 1                                                                                                        
 ELSE                                                     
  CASE ISNULL(IsNull(PT1.FXRate,G1.FXRate), 0)                                                                         
   WHEN 0                                                                               
   THEN ISNULL(CTConvFactor.ConversionFactor, 0)                                                                      
   ELSE IsNull(PT1.FXRate,G1.FXRate)                                       
  END                                                                                       
END AS CTConvFactor,                                                        
                                                                  
CASE ISNULL(IsNull(PT1.FXRate,G1.FXRate), 0)                                                                                      
 WHEN 0                                                     
 THEN 0    
 ELSE                                                                                      
  CASE ISNULL(ISNULL(PT1.FXConversionMethodOperator,G1.FXConversionMethodOperator), 'M')                                                                                      
   WHEN 'M'                         
   THEN 0                         
   ELSE 1                                                    
  END                                                           
END AS CTConvMethod ,                                                                            
                          
CASE Comp.BaseCurrencyID                                                                                                   
 WHEN SM.VsCurrencyID                                                                                                         
 THEN 1                                                                      
 ELSE                                                                     
  CASE                                                   
   WHEN IsNull(PT1.FXRate,G1.FXRate) > 0                                                                                                    
   THEN IsNull(PT1.FXRate,G1.FXRate)                                                     
   ELSE ISNULL(CTConvFactor.ConversionFactor, 0)                                              
  END                                                                          
END AS CTFXConvFactor,                                          
                                                    
CASE ISNULL(IsNull(PT1.FXRate,G1.FXRate), 0)                                                                                      
 WHEN 0                                                     
 THEN 0                                                                            
 ELSE                                                                                      
  CASE                                                               
   WHEN ISNULL(ISNULL(PT1.FXConversionMethodOperator,G1.FXConversionMethodOperator), 'M')='M'                                                            
   THEN 0                                                   
   WHEN ISNULL(ISNULL(PT1.FXConversionMethodOperator,G1.FXConversionMethodOperator), 'D')='D'                                                        
   THEN 1                                            
   ELSE ISNULL(CTConvFactor.ConversionMethod, 0)                                                                                      
  END                                                                                    
END AS CTFXConvMethod ,                                                                            
                                                    
 ISNULL(SM.CompanyName, '') AS CompanyName,                                                                        
 ISNULL(SM.PutOrCall, ' ') AS PutOrCall,                                                                
                                                                
 CASE dbo.GetSideMultiplierForClosing( G.OrderSideTagValue, G1.OrderSideTagValue)                                                                                                                          
  WHEN 1                                                                                                                   
  THEN 'Long'                                                  
  WHEN -1                                                                 
  THEN 'Short'                                                                                            
  ELSE ''                                                                
 END AS PositionSide,                                          
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                            
 IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,            
IsNull(SM.BloombergSymbol,'') as BloombergSymbol,           
IsNull(SM.SedolSymbol,'') as SedolSymbol,           
IsNull(SM.ISINSymbol,'') as ISINSymbol,           
IsNull(SM.CusipSymbol,'') as CusipSymbol,           
IsNull(SM.OSISymbol,'') as OSISymbol,              
IsNull(SM.IDCOSymbol,'') as IDCOSymbol,                                                                                                 
G.OriginalPurchaseDate                                               
                                                                  
FROM PM_TaxlotClosing  PTC                                                                                
INNER JOIN T_Company Comp ON Comp.CompanyID = @companyID                                                                                                        
Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                    
Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                     
Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                        
Inner Join T_Group G1 on G1.GroupID = PT1.GroupID   
INNER JOIN #T_CompanyFunds CF on CF.CompanyFundID = PT.FundID     
INNER JOIN #T_Asset A ON G.AssetID = A.AssetID -- AssetID join was with AUEC.AssetID, but AUEC join has no more, so Now with G.AssetID                                                                                         
LEFT JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                    
--Inner Join T_AUEC AUEC on AUEC.AUECID=G.AUECID --Now AUECID comes in V_SecMasterData View                                                                                                        
Inner Join #ToAUECDatesTable ToAUECDatesTable on ToAUECDatesTable.AUECID=SM.AUECID                        
Inner Join #FromAUECDatesTable FromAUECDatesTable on FromAUECDatesTable.AUECID=SM.AUECID                                                                       
LEFT JOIN T_CompanyStrategy CS ON PT.Level2ID = CS.CompanyStrategyID                                                                                    
LEFT JOIN T_CompanyThirdPartyMappingDetails CTPMD ON PT.FundID = CTPMD.InternalFundNameID_FK                                                                                  
LEFT JOIN T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID                                                                
LEFT JOIN T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID 

--modified by omshiv, remove  V_GetSymbolUDAData, as UDA details are now in V_SecMasterData                                                                                      
--left join V_GetSymbolUDAData as UDA on PT.Symbol=UDA.TickerSymbol                                                                                      
Left Outer Join T_SwapParameters SW on SW.GroupID=G.GroupID                                                                                                 
Left Outer Join T_SwapParameters SW1 on SW1.GroupID=G1.GroupID                                                                                                  
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                    
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                 
LEFT JOIN T_Side TS on TS.SidetagValue = G.OrderSideTagValue                                                                                              
LEFT JOIN T_Side TS1 on TS1.SidetagValue = G1.OrderSideTagValue   
                                                                                         
-- here OT means Open Trade                                                                                  
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange OTConvFactor                         
ON G.CurrencyID  = OTConvFactor.FCID                                                     
AND Comp.BaseCurrencyID = OTConvFactor.TCID                                                                                
AND DATEDIFF(d,OTConvFactor.DateCC, G.ProcessDate) = 0                                                                                                                         
                 
-- here CT means Closing Trade                                                                                  
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange CTConvFactor                                                                                           
ON G1.CurrencyID  = CTConvFactor.FCID                                                                                           
AND Comp.BaseCurrencyID = CTConvFactor.TCID                            
AND DATEDIFF(d,CTConvFactor.DateCC, PTC.AUECLocalDate) = 0     
                                          
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                    
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                         
                                     
Where                                                                           
DateDiff(d,FromAUECDatesTable.CurrentAUECDate,PTC.AUECLocalDate) >=0                                                                                             
and  DateDiff(d,PTC.AUECLocalDate,ToAUECDatesTable.CurrentAUECDate)>=0                                                                                  
and  PTC.ClosingMode<>7                                            
                                                                                                                                          
End                                
                          
                                                                                                             
---------------------------------------------------------------------------------------------                                                                                  
DROP TABLE #TEMPGetConversionFactorForGivenDateRange,#FromAUECDatesTable,#ToAUECDatesTable  
DROP TABLE #SecMasterDataTempTable,#T_CompanyFunds, #T_Asset 

