-- =============================================      
-- Modified By: Sumit Kakra
-- Modification Date: July 8 2013
-- Description: Nirvana Data access webservice proedure for webmethod TaxlotSnapshot
-- Usage: EXEC [P_DA_TaxlotSnapshot] '2021-07-1','',0
-- =============================================      
Create Procedure [dbo].[P_DA_TaxlotSnapshot]
(
@date DateTime,
@errorMessage varchar(max) output,                                      
@errorNumber int output                               
)                            
As  
SET @ErrorMessage = 'Success'                                      
SET @ErrorNumber = 0                               
                            
BEGIN TRY                        
      -- To ensure no locking, it allows dirty reads, so check for blank symbols and Qty>0
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 

Declare @BaseCurrencyID int
Declare @YesterDaydate datetime
Declare @DefaultAUECID int
Declare @DateForCashValues datetime    
    
Set @DateForCashValues = @date    

Select  @BaseCurrencyID = BaseCurrencyID from T_Company
Select  @DefaultAUECID = DefaultAUECID from T_Company
Select @YesterDaydate = dbo.AdjustBusinessDays(@date,-1,@DefaultAUECID)

Create Table #taxlotpks
(
  taxlot_PK int
)

Insert into #taxlotpks
  Select max(taxlot_PK) from PM_Taxlots                                                                                           
  where Datediff(d,PM_Taxlots.AUECModifiedDate,@date) >= 0                
  group by taxlotid        


Create Table #FXRates
  (FromCurrencyID int,
   ToCurrencyID int,         
   FXRate float) 

Insert Into #FXRates
Select FromCurrencyID, ToCurrencyID, 
Case When ConversionMethod = 1
	Then 1.0/RateValue
Else
	RateValue
End As FxRate
  from dbo.[GetFXConversionRatesForDate]( @date,0) Where FromCurrencyID <> @BaseCurrencyID And RateValue <> 0 and FundID = 0

----Day End Accruals    
  SELECT FundID ,cast(cast(SUM(CloseDrBal - CloseCrBal) AS float) as DECIMAL(28, 12)) AS Cash,    
  cast(cast(SUM(CloseDrBalBase - CloseCrBalBase) AS float) as DECIMAL(28, 12)) AS BaseCash ,  
  T_SubAccountBalances.CurrencyID as CurrencyID   
  into #StartOfDayAccountWiseAccruals FROM T_SubAccountBalances        
  INNER JOIN T_SubAccounts ON T_SubAccountBalances.SubAccountID = T_SubAccounts.SubAccountID        
  INNER JOIN T_TransactionType ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId        
  WHERE T_TransactionType.TransactionType = 'Accrued Balance'        
  AND DATEDIFF(dd, T_SubAccountBalances.TransactionDate, DATEADD(dd, -1, @DateForCashValues)) = 0        
  GROUP BY FundID,CurrencyID     
      
----Today's Accruals      
  SELECT T_Journal.FundID, cast(cast(SUM(DR-CR) AS float) as DECIMAL(28, 12)) AS Cash,    
  cast(cast(SUM((DR-CR) * FxRate) AS float) as DECIMAL(28, 12)) AS BaseCash ,  
  T_Journal.CurrencyID as CurrencyID      
  into #DayAccountWiseAccruals FROM T_Journal       
  INNER JOIN T_SubAccounts ON T_Journal.SubAccountID = T_SubAccounts.SubAccountID        
  INNER JOIN T_TransactionType ON T_TransactionType.TransactionTypeId = T_SubAccounts.TransactionTypeId        
  WHERE T_TransactionType.TransactionType = 'Accrued Balance' AND DATEDIFF(dd, T_Journal.TransactionDate, @DateForCashValues) = 0        
  GROUP BY T_Journal.FundID,T_Journal.CurrencyID

Create Table #TodaysCash
  (FundID int,
   LocalCurrencyID int,
   BaseCurrencyID int,
   Cash float,
   BaseCash float,
   Date datetime) 

Insert Into #TodaysCash
SELECT     FundID,CurrencyID,NULL,SUM(DR*1 + -1*CR) As Cash,
		   SUM(DR * isnull(FXRates.FXRate,1) + -1*CR*isnull(FXRates.FXRate,1) ) As BaseCash, @date As Date
FROM       T_Journal Inner Join  T_SubAccounts ON T_Journal.SubAccountID = T_SubAccounts.SubAccountID INNER JOIN
                      T_SubCategory ON T_SubAccounts.SubCategoryID = T_SubCategory.SubCategoryID INNER JOIN
                      T_MasterCategory ON T_MasterCategory.MasterCategoryID = T_SubCategory.MasterCategoryID Left OUTER JOIN
			#FXRates FXRates ON T_Journal.CurrencyID = FXRates.FromCurrencyID
WHERE     (T_MasterCategory.MasterCategoryName = 'Cash') 
			And T_Journal.TransactionDate >= dbo.GetFormattedDatePart(@date) 
			And T_Journal.TransactionDate <= DateAdd(d,1,dbo.GetFormattedDatePart(@date))
Group By FundID,CurrencyID

Insert Into #TodaysCash
Select
	FundID,
	LocalCurrencyID,
	BaseCurrencyID,
	CFCC.CashValueLocal as Cash,
	CFCC.CashValueBase as BaseCash,
	DateAdd(d,-1,dbo.GetFormattedDatePart(@date)) As Date
From PM_CompanyFundCashCurrencyValue CFCC                                                                        
WHERE dbo.GetFormattedDatePart(CFCC.Date) = DateAdd(d,-1,dbo.GetFormattedDatePart(@date))

Update #TodaysCash
Set BaseCurrencyID = @BaseCurrencyID

update TC    
set TC.Cash= TC.Cash + SOD.Cash,   
TC.BaseCash= TC.BaseCash + SOD.BaseCash  
from #TodaysCash TC    
inner join #StartOfDayAccountWiseAccruals SOD ON SOD.FundID = TC.FundID   
and SOD.CurrencyID = TC.LocalCurrencyID  
and datediff(d,dateadd(d,-1,@DateForCashValues),TC.Date)=0  
  
  
update TC    
set TC.Cash= TC.Cash + DA.Cash,     
TC.BaseCash= TC.BaseCash + DA.BaseCash    
from #TodaysCash TC    
inner join #DayAccountWiseAccruals DA ON DA.FundID = TC.FundID    
and DA.CurrencyID = TC.LocalCurrencyID  
and datediff(d,@DateForCashValues,TC.Date)=0  
   
      
 SELECT * INTO #MarkPrices FROM dbo.[GetMarkPriceForBusinessDay](@date)

 Select                                                                                           
 PT.GroupID as GroupID,
 PT.TaxLotID as TaxLotID,
 T_CompanyFunds.FundName,                                                                                                     
 T_CompanyStrategy.StrategyName,                                                                                        
 PT.Symbol as Symbol,
 isnull(V_SecMasterData.CompanyName,'') as SecurityName,  
 T_Side.Side As Side,      
 PT.TaxLotOpenQty as OpenQty,
 PT.AvgPrice as AvgPrice ,                                                                             
 G.AUECLocaldate as TaxlotDate,                                                                                      
 PT.OpenTotalCommissionandFees,                                                            
 isnull(V_SecMasterData.UnderlyingSymbol,'') as UnderlyingSymbol,                                            
 IsNull(V_SecMasterData.PutOrCall,'') as PutOrCall,
 IsNull(V_SecMasterData.StrikePrice,0) as StrikePrice,              
 isnull(V_SecMasterData.Multiplier,1) as Multiplier,  
 T_Asset.AssetName,
 isnull(V_SecMasterData.ExpirationDate,'1/1/1800') as ExpirationDate,
	TP.ShortName as PrimeBrokerCode,
	TP.ThirdPartyName AS PrimeBrokerName,
	V_SecmasterData.AssetName AS UDAAssetName,
	V_SecmasterData.SecurityTypeName AS UDASecurityTypeName,
	V_SecmasterData.SectorName AS UDASectorName,
	V_SecmasterData.SubSectorName AS UDASubSectorName,
	V_SecmasterData.CountryName AS UDACountryName,
	V_SecmasterData.BloombergSymbol AS BloombergSymbol,
	V_SecmasterData.OSISymbol AS OSISymbol,
	V_SecmasterData.SEDOLSymbol AS SEDOLSymbol,
	V_SecMasterData.CUSIPSymbol AS CUSIPSymbol,	
	V_SecMasterData.ISINSymbol AS ISINSymbol,	 
	MarkPrice.FinalMarkPrice As ClosingMark,
T_CompanyFunds.FundShortName,
@Date as AsOfDate,
FXRates.FXRate as FXClosingMark,
T_Currency.CurrencySymbol AS TradeCurrency
 FROM PM_Taxlots PT                                 
 INNER JOIN T_Group G ON G.GroupID = PT.GroupID
 INNER JOIN T_Side ON PT.OrderSideTagValue = dbo.T_Side.SideTagValue
 INNER JOIN	T_CompanyFunds ON PT.FundID = T_CompanyFunds.CompanyFundID
 INNER JOIN T_CompanyStrategy ON PT.Level2ID = T_CompanyStrategy.CompanyStrategyID 
 INNER JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol
 INNER JOIN T_Currency ON T_Currency.CurrencyID = V_SecMasterData.CurrencyID
 INNER JOIN dbo.T_Asset ON dbo.T_Asset.AssetID = V_SecmasterData.AssetID LEFT OUTER JOIN
		dbo.T_CompanyThirdPartyMappingDetails CTPMD ON PT.FundID = CTPMD.InternalFundNameID_FK LEFT OUTER JOIN
		dbo.T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID LEFT OUTER JOIN
		dbo.T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID   
 INNER JOIN #taxlotpks ON PT.taxlot_PK = #taxlotpks.taxlot_PK 
 LEFT OUTER JOIN #MarkPrices MarkPrice  ON PT.Symbol = MarkPrice.Symbol
 LEFT OUTER JOIN #FXRates FXRates  ON V_SecmasterData.CurrencyID = FXRates.FromCurrencyID 
										And FXRates.ToCurrencyID = @BaseCurrencyID
 Where TaxLotOpenQty<>0   
And Datediff(d,PT.AUECModifiedDate,@Date) >= 0 
And PT.Symbol is Not Null 
And TaxLotOpenQty<>0 
And TaxLotOpenQty is not NULL

Union All

   Select
Convert(varchar(max),NewID()) As GroupID,
Convert(varchar(max),NewID()) As TaxLotID,

 CF.FundName,                                                                                                     
 '' As StrategyName,                                                                                        
 CLocal.CurrencySymbol as Symbol,
 isnull(CLocal.CurrencySymbol,'') as SecurityName,
 Case When CFCC.Cash > 0 
		Then 'Buy'
	  When CFCC.Cash <0
		Then 'Sell'
 Else 'None'
 End As Side,                                                                              
 CFCC.Cash as OpenQty,
 1 as AvgPrice , 
 CFCC.Date as TaxlotDate, 
0 As OpenTotalCommissionandFees,                                                                            
 isnull(CLocal.CurrencySymbol,'') as UnderlyingSymbol,                                            
 '' as PutOrCall,
 0 as StrikePrice,              
 1 as Multiplier,  
 'Cash' As AssetName,
 '1/1/1800' as ExpirationDate,
	TP.ShortName as PrimeBrokerCode,
	TP.ThirdPartyName AS PrimeBrokerName,
	'Cash' AS UDAAssetName,
	'Cash'  AS UDASecurityTypeName,
	'Cash'  AS UDASectorName,
	'Cash'  AS UDASubSectorName,
	'Cash'  AS UDACountryName,
	CLocal.CurrencySymbol + ' Curncy' AS BloombergSymbol,
	'' AS OSISymbol,
	 '' AS SEDOLSymbol,
	 '' AS CUSIPSymbol,
	'' AS ISINSymbol,	 
	1 As ClosingMark,
	CF.FundShortName,
	@Date As AsOfDate,
	FXRates.FXRate as FXClosingMark,
	CLocal.CurrencySymbol
   From #TodaysCash  CFCC  
	INNER JOIN T_CompanyFunds CF ON CFCC.FundID = CF.CompanyFundID                                                                        
	INNER JOIN T_Currency CLocal ON LocalCurrencyID = CLocal.CurrencyID
	LEFT OUTER JOIN dbo.T_CompanyThirdPartyMappingDetails CTPMD ON CF.CompanyFundID = CTPMD.InternalFundNameID_FK 
	LEFT OUTER JOIN dbo.T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID
	LEFT OUTER JOIN dbo.T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                        
	LEFT OUTER JOIN #FXRates FXRates  ON LocalCurrencyID = FXRates.FromCurrencyID 
										And FXRates.ToCurrencyID = @BaseCurrencyID

drop table #taxlotpks, #FXRates, #TodaysCash, #MarkPrices, #StartOfDayAccountWiseAccruals, #DayAccountWiseAccruals 

END TRY                                      
BEGIN CATCH                              
                                        
 SET @ErrorMessage = ERROR_MESSAGE();                                      
 SET @ErrorNumber = Error_number();          
                                       
END CATCH;