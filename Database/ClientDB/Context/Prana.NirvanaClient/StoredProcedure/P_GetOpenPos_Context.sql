/*    
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-464
Created BY: Kuldeep  
Date: 10 Oct, 2019  
Desc: Get Open Positions for Context as of a date. This stored proc is going to be used for Third Party.  
Update the Sp from middleware to realtime.  

EXEC [P_GetOpenPos_Context] 1,'2,1','10-11-2019',1,'1',1,1,31  
*/    
    
    
    
Create Procedure [dbo].[P_GetOpenPos_Context]                            
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
        
--Declare @InputDate DateTime        
--Set @InputDate = '07-22-2017'    
    
--Declare @CompanyFundIDs varchar(max)     
--Set @companyFundIDs = '1257'       
    
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
 ,FundID INT        
 )       
      
INSERT INTO #MarkPriceForEndDate       
(        
 FinalMarkPrice        
 ,Symbol        
 ,FundID        
 )        
SELECT       
  DMP.FinalMarkPrice        
 ,DMP.Symbol        
 ,DMP.FundID        
FROM PM_DayMarkPrice DMP    
Inner Join @Fund F On F.FundID = DMP.FundID      
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0      
    
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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @InputDate    
 ,@InputDate    
    
UPDATE #FXConversionRates    
SET RateValue = 1.0 / RateValue    
WHERE RateValue <> 0    
 AND ConversionMethod = 1    
    
-- Delete records for FundID = 0, Contellation maintained Fundwise FX Rate    
Delete #FXConversionRates    
WHERE fundID = 0    
    
--Select * From #FXConversionRates    
--Where FundID = 1257    
    
    
SELECT PT.Taxlot_PK        
InTo #TempTaxlotPK             
FROM PM_Taxlots PT              
Inner Join @Fund Fund on Fund.FundID = PT.FundID                  
Where PT.Taxlot_PK in                                           
(                                                                                                    
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                       
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                          
 group by TaxlotId                             
)                                                                                              
And PT.TaxLotOpenQty > 0           
        
Select                   
CF.FundName As AccountName,    
PT.Symbol,      
SM.BloombergSymbol,                       
Curr.CurrencySymbol As LocalCurrency,    
PT.TaxlotOpenQty As OpenPositions,    
SM.Multiplier As AssetMultiplier,    
CONVERT(VARCHAR(10), PT.AUECModifiedDate, 101) AS TradeDate ,     
Case     
 When G.IsSwapped  = 1    
 Then 'EquitySwap'    
 Else A.AssetName     
End As AssetClass,    
    
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,     
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,    
IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) AS MarkPrice,    
     
IsNull((PT.TaxlotOpenQty * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue    
    
Into #TempOpenPositionsTable             
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol    
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID      
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID    
Inner Join T_Asset A On A.AssetID = SM.AssetID    
INNER JOIN T_Group G ON G.GroupID = PT.GroupID    
--LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID)     
  
 LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (      
   MP.Symbol = PT.Symbol      
   AND DateDiff(d, MP.DATE, @InputDate) = 0      
   AND MP.FundID = PT.FundID      
   )      
 LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (      
   MP1.Symbol = PT.Symbol      
   AND DateDiff(d, MP1.DATE, @InputDate) = 0      
   AND MP1.FundID = 0      
   )      
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON     
  (FXDayRatesForEndDate.FromCurrencyID = SM.CurrencyID    
  AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency    
  AND DateDiff(d, FXDayRatesForEndDate.DATE,@InputDate) = 0    
  AND FXDayRatesForEndDate.FundID = PT.FundID )      
Where PT.TaxlotOpenQty > 0    
    
    
Select                   
Temp.AccountName As AccountName,   
Temp.Symbol as Symbol,       
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions,     
Max(BloombergSymbol) As BloombergSymbol,     
Max(LocalCurrency) As LocalCurrency,       
Sum(Temp.TotalCost_Local) As TotalCost_Local,     
AssetClass As AssetClass,    
Sum(MarkPrice) As MarkPrice,    
Sum(MarketValue) As MarketValue,    
Max(TradeDate) As TradeDate    
      
Into #TempTable                  
From #TempOpenPositionsTable Temp      
Group By Temp.AccountName,Temp.AssetClass,Temp.symbol  
    
Alter Table #TempTable          
Add UnitCostLocal Float Null          
          
UPdate #TempTable          
Set UnitCostLocal = 0.0          
    
    
Update #TempTable      
Set UnitCostLocal  = Abs(TotalCost_Local/OpenPositions)      
Where OpenPositions <> 0           
      
  
Insert into #TempTable  
select  
CF.FundName as AccountName,                           
'' as Symbol,                           
'0' as OpenPositions,  
CurrencyLocal.CurrencySymbol as BloombergSymbol,  
'' as LocalCurrency,  
'0' As TotalCost_Local,   
'Cash' As AssetClass,  
'0' As MarkPrice,  
CompanyFundCashCurrencyValue.CashValueLocal As MarketValue,
CONVERT(VARCHAR(10), CompanyFundCashCurrencyValue.Date, 101) AS TradeDate,     
'0' AS UnitCostLocal                        
from PM_CompanyFundCashCurrencyValue CompanyFundCashCurrencyValue                 
inner join @Fund Fund   on Fund.FundId = CompanyFundCashCurrencyValue.FundID  
               
inner join T_CompanyFunds CF  on CF.CompanyFundId = CompanyFundCashCurrencyValue.FundID                          
left join T_Currency CurrencyLocal   on CurrencyLocal.CurrencyId = CompanyFundCashCurrencyValue.LocalCurrencyID                          
left join T_Currency CurrencyBase    on CurrencyBase.CurrencyId = CompanyFundCashCurrencyValue.BaseCurrencyID                          
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = CompanyFundCashCurrencyValue.FundID     
LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID   
where datediff(dd, CompanyFundCashCurrencyValue.Date, @inputDate)=0                          
      
        
Select * from #TempTable   
Order By AccountName,Symbol,AssetClass       
 
Drop Table #TempOpenPositionsTable, #TempTable    
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates