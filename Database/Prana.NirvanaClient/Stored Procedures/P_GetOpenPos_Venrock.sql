CREATE Procedure [dbo].[P_GetOpenPos_Venrock]                            
(                                     
@ThirdPartyID int,                                                
@CompanyFundIDs varchar(max),                                                                                                                                                                              
@InputDate datetime,                                                                                                                                                                          
@CompanyID int,                                                                                                                                          
@AUECIDs varchar(max),                                                                                
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                
@FileFormatID int                                      
)                                      
AS    
  
--Declare @ThirdPartyID int,                                                
--@CompanyFundIDs varchar(max),                                                                                                                                                                              
--@InputDate datetime,                                                                                                                                                                          
--@CompanyID int,                                                                                                                                          
--@AUECIDs varchar(max),                                                                                
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                
--@FileFormatID int  
     
--set @thirdPartyID=122  
--set @companyFundIDs=N'4,10,1,2,3,5,6,7,8,9,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,'  
--set @inputDate='2025-07-03 08:45:25'  
--set @companyID=7  
--set @auecIDs=N'44,43,18,1,15,62,73,12,80,88,81,'  
--set @TypeID=1  
--set @dateType=1  
--set @fileFormatID=80  
    
Declare @Fund Table                                                               
(                    
FundID int                          
)      
    
Insert into @Fund                                                                                                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')     
    
-- get Mark Price for End Date                            
CREATE TABLE #MarkPriceForEndDate       
(        
  Finalmarkprice FLOAT        
 ,Symbol VARCHAR(max)        
 --,FundID INT        
 )       
      
INSERT INTO #MarkPriceForEndDate       
(        
 FinalMarkPrice        
 ,Symbol        
 --,FundID        
 )        
SELECT       
case     
 when DMP.FinalMarkPrice like '%E%' then 0    
else DMP.FinalMarkPrice end,       
 DMP.Symbol        
 --,DMP.FundID        
FROM PM_DayMarkPrice DMP    
--Inner Join @Fund F On F.FundID = DMP.FundID      
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0  and DMP.FundID = 0     
    
-- get forex rates for 2 date ranges              
CREATE TABLE #FXConversionRates (    
 FromCurrencyID INT    
 ,ToCurrencyID INT    
 ,RateValue FLOAT    
 ,ConversionMethod INT    
 ,DATE DATETIME    
 ,eSignalSymbol VARCHAR(max)    
 ,FundID INT    
 )    
    
INSERT INTO #FXConversionRates    
EXEC P_GetAllFXConversionRatesForGivenDateRange @InputDate    
 ,@InputDate    
    
UPDATE #FXConversionRates    
SET RateValue = 1.0 / RateValue    
WHERE RateValue <> 0    
 AND ConversionMethod = 1    
    
-- Delete records for FundID = 0, Contellation maintained Fundwise FX Rate    
    
--Select * From #FXConversionRates    
--Where FundID = 1257    
    
    
SELECT PT.Taxlot_PK        
InTo #TempTaxlotPK             
FROM PM_Taxlots PT With (NoLock)             
Inner Join @Fund Fund on Fund.FundID = PT.FundID                  
Where PT.Taxlot_PK in                                           
(                                                                                                    
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                     
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                          
 group by TaxlotId                                                                          
)                                                                                              
And PT.TaxLotOpenQty > 0           
        
Select                   
CF.FundName As AccountName,              
PT.Symbol,                  
Case                          
 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                          
 Then 'Long'                          
 Else 'Short'                          
End as PositionIndicator,      
PT.TaxlotOpenQty As OpenPositions,    
SM.BloombergSymbol,    
Curr.CurrencySymbol As LocalCurrency,    
SM.Multiplier As AssetMultiplier,     
SM.CompanyName As SecurityDescription,     
Case     
 When G.IsSwapped  = 1    
 Then 'EquitySwap'    
 Else A.AssetName     
End As AssetClass,    
SM.ISINSymbol,    
SM.SEDOLSymbol,    
SM.OSISymbol,    
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,     
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,    
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,    
Case     
 When SM.CurrencyID = CF.LocalCurrency     
 Then 1    
Else IsNull(FXDayRatesForEndDate.RateValue,0)     
End As FXRate,    
SM.LeadCurrency,    
SM.VsCurrency,    
SM.CUSIPSymbol,    
UDA.AssetName as UDAAsset,    
Case     
 When G.AssetID='5'     
 Then SettlementDate    
Else SM.ExpirationDate    
End As ExpirationDate,    
IsNull((PT.TaxlotOpenQty * MP_FundWise.Finalmarkprice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,  
SM.UnderLyingSymbol,  
IsNull(MP_FundWise_Underlying.Finalmarkprice,0) As UnderlyingSymbol_MarkPrice   
    
--SM.ExpirationDate    
Into #TempOpenPositionsTable             
From PM_Taxlots PT With (NoLock)  
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol    
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID      
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = PT.FundID    
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID    
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID    
Inner join VenrockSM.dbo.T_SMSymbolLookUpTable SMT With (NoLock) on SMT.TickerSymbol=PT.Symbol    
inner join VenrockSM.dbo.T_UDAAssetClass UDA With (NoLock) on UDA.AssetID = SMT.UDAAssetClassID    
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol)   
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise_Underlying ON (SM.UnderLyingSymbol = MP_FundWise_Underlying.Symbol)    
    
--LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (                
--   MP.Symbol = PT.Symbol                
--   AND DateDiff(d, MP.DATE, @InputDate) = 0                
--   AND MP.FundID = PT.FundID                
--   )                
-- LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (                
--   MP1.Symbol = PT.Symbol                
--   AND DateDiff(d, MP1.DATE, @InputDate) = 0                
--   AND MP1.FundID = 0                
--   )      
    
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON     
  (FXDayRatesForEndDate.FromCurrencyID = SM.CurrencyID    
  AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency    
  AND DateDiff(d, FXDayRatesForEndDate.DATE,@InputDate) = 0)    
  --AND FXDayRatesForEndDate.FundID = PT.FundID       
Where PT.TaxlotOpenQty > 0    
    
    
Select                   
Temp.AccountName As AccountName,                  
Temp.Symbol,                  
Temp.PositionIndicator As PositionIndicator,       
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions,     
Max(BloombergSymbol) As BloombergSymbol,     
Max(LocalCurrency) As LocalCurrency,     
Max(Temp.AssetMultiplier) As AssetMultiplier,          
Max(SecurityDescription) As SecurityDescription,    
Sum(Temp.TotalCost_Local) As TotalCost_Local,     
AssetClass As AssetClass,    
Max(ISINSymbol) As ISINSymbol,    
Max(SEDOLSymbol) As SEDOLSymbol,    
Max(CUSIPSymbol) As CUSIPSymbol,    
UDAAsset,    
Max(MarkPrice) As MarkPrice,    
Max(FXRate) As FXRate,    
Max(OSISymbol) As OSISymbol,    
Temp.ExpirationDate,    
Sum(MarketValue) AS MarketValue,  
Max(UnderLyingSymbol) As UnderLyingSymbol,  
Max(UnderlyingSymbol_MarkPrice) As UnderlyingSymbol_MarkPrice  
  
Into #TempTable                  
From #TempOpenPositionsTable Temp      
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator,Temp.AssetClass,temp.ExpirationDate,temp.UDAAsset    
    
Alter Table #TempTable          
Add UnitCost Float Null          
          
UPdate #TempTable          
Set UnitCost = 0.0          
          
UPdate #TempTable          
Set UnitCost =           
Case            
 When OpenPositions <> 0 And AssetMultiplier <> 0            
 Then (TotalCost_Local/OpenPositions) /AssetMultiplier            
 Else 0            
End            
From #TempTable            
          
Select * from #TempTable     
Order By AccountName,Symbol,PositionIndicator      
    
    
Drop Table #TempOpenPositionsTable, #TempTable    
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates 