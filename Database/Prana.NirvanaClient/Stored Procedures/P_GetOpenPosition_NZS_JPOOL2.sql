
CREATE PROCEDURE [dbo].[P_GetOpenPosition_NZS_JPOOL2] (
	@CompanyFundIDs VARCHAR(max)
	,@InputDate DATETIME
	)
AS

--Declare @CompanyFundIDs VARCHAR(max) ,@InputDate DATETIME
--SET @CompanyFundIDs = ''
--SET @InputDate = '2023-08-10'

SET NOCOUNT ON                                                       
BEGIN
	IF (@InputDate = '')
	BEGIN
		SET @InputDate = GetDate()
	END
	    
	Set @companyFundIDs = '8' 
	                        
	DECLARE @Fund TABLE (FundID INT)
	IF (
			@CompanyFundIDs IS NULL
			OR @CompanyFundIDs = ''
			)
		INSERT INTO @Fund
		SELECT CompanyFundID AS FundID
		FROM T_CompanyFunds Where IsActive=1
	ELSE
		INSERT INTO @Fund
		SELECT Cast(Items AS INT)
		FROM dbo.Split(@companyFundIDs, ',')


		------------temp table from th file------------

select  *
Into #TempCurrencyFXRateForFile
from openrowset('MSDASQL'
,'Driver={Microsoft Access Text Driver (*.txt, *.csv)};
DBQ=C:\Nirvana\NZSCapital\NZSCapital\Jpool EOD Custom Batch\'
,'select * from "FXRateBloomberg.csv"') 

--select * from #TempCurrencyFXRateForFile

-- get Mark Price for End Date                        
CREATE TABLE #MarkPriceForEndDate   
(    
  Finalmarkprice FLOAT    
 ,Symbol VARCHAR(200)    
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
FROM PM_DayMarkPrice DMP With (NoLock)
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0 

--select * from #ZeroFundMarkPriceEndDate 

SELECT PT.Taxlot_PK    
InTo #TempTaxlotPK         
FROM PM_Taxlots PT          
Inner Join @Fund Fund on Fund.FundID = PT.FundID              
Where PT.Taxlot_PK In                                       
	(                                                                                                
		 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                  
		 Where DateDiff(DAY, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
		 group by TaxlotId                                                      
	)                                                                                          
And PT.TaxLotOpenQty > 0        
    
Select               
CF.FundName As AccountName,              
PT.Symbol,        
  
PT.TaxlotOpenQty As OpenPositions,
Curr.CurrencySymbol As LocalCurrency,
SM.Multiplier As AssetMultiplier,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
 
SM.CompanyName As SecurityDescription, 
Case 
	When G.IsSwapped  = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AssetClass,
SM.SEDOLSymbol,
SM.ISINSymbol,
PT.AvgPrice AS AvgPrice,

IsNull(MPEndDate.Val, 0) As MarkPrice,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue_Local,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValueBase,
--AU.DisplayName As Exchange
UDA.CustomUDA9 AS ExchangMIC


Into #TempOpenPositionsTable         
From PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
INNER JOIN T_Group G  With (NoLock) ON G.GroupID = PT.GroupID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF  With (NoLock) On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
--INNER JOIN T_AUEC AU With (NoLock) ON AU.AUECID = SM.AUECID 
INNER JOIN  V_UDA_DynamicUDA UDA With (NoLock) ON UDA.Symbol_PK=SM.Symbol_PK

LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
		MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )
LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
		PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)
CROSS APPLY (
	SELECT CASE 
			WHEN MPE.FinalMarkPrice IS NULL
				THEN CASE 
						WHEN MPZeroEndDate.FinalMarkPrice IS NULL
						THEN 0
						ELSE MPZeroEndDate.FinalMarkPrice
					END
			ELSE MPE.Finalmarkprice
			END
	) AS MPEndDate(Val)
--LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID) 
Where PT.TaxlotOpenQty > 0 and SM.AssetId<>5

UPdate #TempOpenPositionsTable      
Set MarketValueBase =
(
CASE
   WHEN (FFTemp.Currency ='EUR' or FFTemp.Currency ='GBP' or FFTemp.Currency ='NZD'  or FFTemp.Currency ='USD')
THEN (MarketValue_Local *  FFTemp.FXRate)
  WHEN (FFTemp.Currency ='AUD' )
THEN (MarketValue_Local)
   ELSE ((MarketValue_Local/FFTemp.FXRate))
END
)
from #TempOpenPositionsTable PSTemp
inner join #TempCurrencyFXRateForFile FFTemp on FFTemp.Currency = PSTemp.LocalCurrency


Select               
PT.AccountName As AccountName,              
PT.Symbol,            
  
Sum(PT.OpenPositions * PT.SideMultiplier) As OpenPositions, 
Max(LocalCurrency) As LocalCurrency, 
Max(PT.AssetMultiplier) As AssetMultiplier,
Max(PT.SideMultiplier)  AS SideMultiplier,     
Max(SecurityDescription)As SecurityDescription,
AssetClass As AssetClass,
Sum(PT.AvgPrice) As AvgPrice, 
Max(SEDOLSymbol) As SEDOLSymbol,
Max(ISINSymbol) As ISINSymbol,
Max(MarkPrice) As MarkPrice,
Sum(MarketValue_Local) AS MarketValue,
Sum(MarketValueBase) AS MarketValueBase,
Max(ExchangMIC) As ExchangMIC  
Into #TempGroupedOpenPosTable              
From #TempOpenPositionsTable PT 
Group By PT.AccountName,PT.AssetClass,PT.Symbol


Select               
CF.FundName As AccountName,              
CC.CurrencySymbol As Symbol ,   
EODCash.CashValueLocal As OpenPositions,
CC.CurrencySymbol As LocalCurrency, 
'' AS AssetMultiplier,
'' AS SideMultiplier,      
CC.CurrencySymbol As SecurityDescription,
'Cash' As AssetClass,
'' As AvgPrice, 

'' As SEDOLSymbol,
'' AS ISINSymbol,
1 As MarkPrice,
EODCash.CashValueLocal AS MarketValue,
1 AS MarketValueBase,
'' AS  ExchangMIC

Into #TempEODCash             
From PM_CompanyFundCashCurrencyValue EODCash
Inner Join @Fund Fund On Fund.FundID = EODCash.FundID
inner join T_CompanyFunds CF on EODCash.FundID = CF.CompanyFundID
inner join T_Currency CC With (NoLock) on EODCash.LocalCurrencyID=CC.CurrencyID
 Where datediff(d,EODCash.Date,@Inputdate)=0 
 

 --select * from #TempEODCash

 UPdate #TempEODCash      
Set MarketValueBase =
(
CASE
   WHEN (FFTemp.Currency ='EUR' or FFTemp.Currency ='GBP' or FFTemp.Currency ='NZD'  or FFTemp.Currency ='USD')
     THEN (CashTemp.MarketValue *  FFTemp.FXRate)
   WHEN (FFTemp.Currency ='AUD')
       THEN (CashTemp.MarketValue)
    ELSE ((CashTemp.MarketValue/FFTemp.FXRate))
END
)
from #TempEODCash CashTemp
inner join #TempCurrencyFXRateForFile FFTemp on FFTemp.Currency = CashTemp.LocalCurrency



  ------------------------------------------------
 -- Select * from #TempEODCash
Insert InTo #TempGroupedOpenPosTable    
Select               
EODCash.AccountName As AccountName,              
EODCash.Symbol,   
Sum(EODCash.OpenPositions) As OpenPositions,
Max(LocalCurrency) As LocalCurrency, 
Max(EODCash.AssetMultiplier) As AssetMultiplier,
Max(EODCash.SideMultiplier) As SideMultiplier,    
Max(SecurityDescription) As SecurityDescription,
Min(EODCash.AssetClass) As AssetClass,
Min(EODCash.AvgPrice) As AvgPrice, 
Max(SEDOLSymbol) As SEDOLSymbol,
Max(ISINSymbol) As ISINSymbol,
Max(MarkPrice) As MarkPrice,
Sum(MarketValue) AS MarketValue,
Sum(MarketValueBase) AS MarketValueBase,
Max(ExchangMIC)  AS ExchangMIC
From #TempEODCash EODCash  
Group By EODCash.AccountName,EODCash.Symbol

--select * from #TempGroupedOpenPosTable 

Select
'JPOOL2' AS Portfolio, 
SecurityDescription,
CASE
   WHEN AssetClass='Equity'
     THEN 'EQUITY'
   WHEN AssetClass='FX'
     THEN 'CASH'
WHEN AssetClass='Cash'
    THEN 'CASH'
ELSE AssetClass
END 
AS UnderlyingSecurityGroup,

ISINSymbol AS ISIN,
SEDOLSymbol AS SEDOL,
ExchangMIC AS ExchangMIC,
LocalCurrency AS Currency,
OpenPositions AS Quantity,
case
when AssetClass='Equity'
then MarkPrice
else ''
end 
AS PriceLocal,
MarketValue As MVLocal,
MarketValueBase AS MVAUD

From #TempGroupedOpenPosTable
Order By AccountName,Symbol  

Drop Table #TempOpenPositionsTable,#TempEODCash,#TempCurrencyFXRateForFile,#MarkPriceForEndDate,#TempTaxlotPK
Drop Table #TempGroupedOpenPosTable,#ZeroFundMarkPriceEndDate
END

