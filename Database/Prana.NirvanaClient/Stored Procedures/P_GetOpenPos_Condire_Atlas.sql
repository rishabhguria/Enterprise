/*
exec [P_GetOpenPos_Condire_Atlas] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

CREATE Procedure [dbo].[P_GetOpenPos_Condire_Atlas]                        
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

SELECT * INTO #ZeroFundFxRate              
FROM #FXConversionRates              
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
--IsNull((PT.TaxlotOpenQty * IsNull((MP_FundWise.FinalMarkPrice), 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue, 
IsNull((PT.TaxlotOpenQty * MP_FundWise.Finalmarkprice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,
IsNull((PT.TaxlotOpenQty * MP_FundWise.Finalmarkprice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) * ISNULL(FXRatesForEndDate.Val,0) AS MarketValueBase, 
Case 
	When SM.CurrencyID = CF.LocalCurrency 
	Then 1
--Else IsNull(FXDayRatesForEndDate.RateValue,0) 
Else IsNull(FXRatesForEndDate.Val,0) 
End As FXRate,
SM.LeadCurrency,
SM.VsCurrency,
SM.CUSIPSymbol,
SM.ExpirationDate,
SM.PutOrCall,
SM.UnderLyingSymbol,
PT.AvgPrice,
SM.StrikePrice,
Ex.DisplayName,
SM.ReutersSymbol
Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
Inner join T_Exchange Ex ON Ex.ExchangeID = G.ExchangeID
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID) 
--LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON 
--		(FXDayRatesForEndDate.FromCurrencyID = SM.CurrencyID
--		AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency
--		AND DateDiff(d, FXDayRatesForEndDate.DATE,@InputDate) = 0
--		AND FXDayRatesForEndDate.FundID = PT.FundID )  
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                
   FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                
AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
   AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0                
   AND FXDayRatesForEndDate.FundID = PT.FundID              
    ) 
	 LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (     
   ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID               
   AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency              
   AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0    
   AND ZeroFundFxRateEndDate.FundID = 0              
   )   
  		
CROSS APPLY (              
  SELECT
  CASE
  WHEN SM.CurrencyID = CF.LocalCurrency          
   THEN 1
   ELSE 
	CASE               
	WHEN FXDayRatesForEndDate.RateValue IS NULL              
	THEN 
		CASE               
			WHEN ZeroFundFxRateEndDate.RateValue IS NULL              
			THEN 0        
		ELSE ZeroFundFxRateEndDate.RateValue  
		END              
	ELSE FXDayRatesForEndDate.RateValue              
	END   
	END           
  ) AS FXRatesForEndDate(Val) 

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
Max(ReutersSymbol) As ReutersSymbol,
Max(MarkPrice) As MarkPrice,

Sum(MarketValue) AS MarketValue,
Sum(MarketValueBase) AS MarketValueBase,
Max(FXRate) As FXRate,
Max(AvgPrice) As AvgPrice,
Max(DisplayName) As Exchange,
Max(PutOrCall) As PutOrCall,
Max(StrikePrice) As StrikePrice,
Max(UnderLyingSymbol) As UnderLyingSymbol,
Max(OSISymbol) As OSISymbol,
Temp.ExpirationDate  
Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator,Temp.AssetClass,temp.ExpirationDate

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
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundFxRate