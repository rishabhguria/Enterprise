-- =============================================      
-- Modified By: Sumit Kakra
-- Modification Date: July 8 2013
-- Description: Nirvana Data access webservice proedure for webmethod SODPositions
-- EXEC [P_DA_SODPositions] '24-FEB-2015','',0
-- =============================================      
CREATE Procedure [dbo].[P_DA_SODPositions]
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

Select  @BaseCurrencyID = BaseCurrencyID from T_Company
Select  @DefaultAUECID = DefaultAUECID from T_Company
Select @YesterDaydate = dbo.AdjustBusinessDays(@date,-1,@DefaultAUECID)

Create Table #taxlotpks
(
  taxlot_PK int
)

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
  from dbo.[GetFXConversionRatesForDate]( @YesterDaydate,0) Where FromCurrencyID <> @BaseCurrencyID And RateValue <> 0 and FundID = 0

Create Table #SODPosition
(
  FundID int,
  Level2ID int,
  Symbol varchar(max),
  TaxLotOpenQty float,
  AvgPrice float
)

Insert into #taxlotpks
  Select max(taxlot_PK) from PM_Taxlots
where Datediff(d,PM_Taxlots.AUECModifiedDate,@date) >= 0                
  group by taxlotid        


Insert Into #SODPosition
 Select                                                                                           
 FundID,
 Level2ID,                                                                                                    
 PT.Symbol as Symbol,
 Sum(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.TaxLotOpenQty ELSE PT.TaxLotOpenQty * -1 End) as OpenQty,
 CASE 
	When Sum(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.TaxLotOpenQty ELSE PT.TaxLotOpenQty * -1 End) > 0
		THEN SUM(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.AvgPrice*PT.TaxLotOpenQty ELSE 0 End)
			 /SUM(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.TaxLotOpenQty ELSE PT.TaxLotOpenQty * -1 End)
	ELSE
		SUM(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then 0 ELSE PT.AvgPrice*PT.TaxLotOpenQty End)
			 /(SUM(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.TaxLotOpenQty ELSE PT.TaxLotOpenQty * -1 End)*-1)
 End As AvgPrice                                                                     
 FROM PM_Taxlots PT                                 
 INNER JOIN #taxlotpks ON PT.taxlot_PK = #taxlotpks.taxlot_PK  
Group By FundID, Level2ID, Symbol
Having SUM(PT.TaxLotOpenQty) <> 0 
	  And Sum(CASE  When OrderSideTagValue in ('1','3','A','B','E') Then PT.TaxLotOpenQty ELSE PT.TaxLotOpenQty * -1 End) <> 0

Select                                                                                           
 T_CompanyFunds.FundName,                                                                                                     
 T_CompanyStrategy.StrategyName,                                                                                        
 PT.Symbol as Symbol,
 isnull(V_SecMasterData.CompanyName,'') as SecurityName,                                                                              
 PT.TaxLotOpenQty as OpenQty,
 PT.AvgPrice as AvgPrice ,                                                                             
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
	MarkPrice.FinalMarkPrice As ClosingMark,
 Case When PT.TaxLotOpenQty > 0 
		Then 'Long'
	  When PT.TaxLotOpenQty <0
		Then 'Short'
 Else 'None'
 End As PositionSide,
	T_Currency.CurrencySymbol AS TradeCurrency,
FXRates.FXRate as FXRate 
 FROM #SODPosition PT                                 
 INNER JOIN	T_CompanyFunds ON PT.FundID = T_CompanyFunds.CompanyFundID
 INNER JOIN T_CompanyStrategy ON PT.Level2ID = T_CompanyStrategy.CompanyStrategyID 
 INNER JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol
 INNER JOIN T_Currency ON T_Currency.CurrencyID = V_SecMasterData.CurrencyID
 INNER JOIN dbo.T_Asset ON dbo.T_Asset.AssetID = V_SecmasterData.AssetID 
 LEFT OUTER JOIN dbo.T_CompanyThirdPartyMappingDetails CTPMD ON PT.FundID = CTPMD.InternalFundNameID_FK 
 LEFT OUTER JOIN dbo.T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID
 LEFT OUTER JOIN dbo.T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID 
 LEFT OUTER JOIN #FXRates FXRates  ON V_SecmasterData.CurrencyID = FXRates.FromCurrencyID 
										And FXRates.ToCurrencyID = @BaseCurrencyID
 LEFT OUTER JOIN dbo.[GetMarkPriceForBusinessDay](@date) MarkPrice  ON PT.Symbol = MarkPrice.Symbol

Union All

   Select
 CF.FundName,                                                                                                     
 '' As StrategyName,                                                                                        
 CLocal.CurrencySymbol as Symbol,
 isnull(CLocal.CurrencySymbol,'') as SecurityName,                                                                              
 CFCC.CashValueLocal as OpenQty,
 1 as AvgPrice ,                                                                             
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
	1 As ClosingMark,
 Case When CFCC.CashValueBase > 0 
		Then 'Long'
	  When CFCC.CashValueBase <0
		Then 'Short'
 Else 'None'
 End As PositionSide,
	CLocal.CurrencySymbol AS TradeCurrency,
FXRates.FXRate as FXRate 
   From PM_CompanyFundCashCurrencyValue  CFCC  
	INNER JOIN T_CompanyFunds CF ON CFCC.FundID = CF.CompanyFundID                                                                        
	INNER JOIN T_Currency CLocal ON LocalCurrencyID = CLocal.CurrencyID
	LEFT OUTER JOIN dbo.T_CompanyThirdPartyMappingDetails CTPMD ON CF.CompanyFundID = CTPMD.InternalFundNameID_FK 
	LEFT OUTER JOIN dbo.T_CompanyThirdParty CTP ON CTPMD.CompanyThirdPartyID_FK = CTP.CompanyThirdPartyID
	LEFT OUTER JOIN dbo.T_ThirdParty TP ON CTP.ThirdPartyID = TP.ThirdPartyID                                                   LEFT OUTER JOIN #FXRates FXRates  ON CFCC.LocalCurrencyID = FXRates.FromCurrencyID 
										And FXRates.ToCurrencyID = @BaseCurrencyID
WHERE dbo.GetFormattedDatePart(CFCC.Date) = dbo.GetFormattedDatePart(DateAdd(d,-1,@date))


drop table #taxlotpks,#SODPosition,#FXRates
END TRY                                      
BEGIN CATCH                              
                                        
 SET @ErrorMessage = ERROR_MESSAGE();                                      
 SET @ErrorNumber = Error_number();          
                                       
END CATCH;            
