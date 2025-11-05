-- Resonate EOD fro Realized Gain and Loss Report
-- Jira: ONB-2394


CREATE Procedure [dbo].[P_GetRealizedPNLGainNLoss_Resonate]                        
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
--	@CompanyFundIDs varchar(max),                                                                                                                                                                          
--	@InputDate datetime,                                                                                                                                                                      
--	@CompanyID int,                                                                                                                                      
--	@AUECIDs varchar(max),                                                                            
--	@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--	@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--	@FileFormatID int,
--	@MinTradeDate datetime

--set @thirdPartyID=86
--set @companyFundIDs=N''
--set @inputDate=''
--set @companyID=1
--set @auecIDs=N''
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=167
Declare @MinTradeDate datetime
Declare @AUECIDForOneDayBack Int

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select Cast(Items As Int) from dbo.Split(@companyFundIDs,',') 

-- Get forex rates for 2 date ranges          

---------------  Create Temp data table -------------------------------------------------------------------------
CREATE TABLE #TempTaxlotInfo
(
	PositionalTaxlotId VARCHAR(200),
	ClosingTaxlotId VARCHAR(200),
	PositionType VARCHAR(15),
	ClosedQty FLOAT,
	OpenPrice FLOAT,
	ClosePrice FLOAT,
	Symbol VARCHAR(200),
	OpenTradeFXRate float,
	OpenTradeFXOperator VARCHAR(2),
	PTOpenTotalCommissionandFees FLOAT,
	PTClosedTotalCommissionandFees FLOAT,
	FundID INT,
	OpenTradeLevel2ID INT,
	CloseTradeFXRate FLOAT,
	CloseTradeFXOperator VARCHAR(2), 
	PT1OpenTotalCommissionandFees FLOAT,
	PT1ClosedTotalCommissionandFees FLOAT,
	CloseTradeLevel2ID INT,
	OpenTradeDate DATETIME,
	TradeCurrencyID Int,
	OriginalPurchaseDate DATETIME,
	ClosingTradeDate DATETIME,
	SideMultiplier FLOAT,
	AuecID INT
)
INSERT INTO #TempTaxlotInfo 
SELECT 
--DATEADD(hh, DATEDIFF(hh, getutcdate(), getdate()), PTC.TimeOfSaveUTC) as ActionDate,
--PTC.TimeofSaveUTC,
PTC.PositionalTaxlotId,
PTC.ClosingTaxlotId,
PTC.PositionSide As PositionType,
PTC.ClosedQty As ClosedQty,
PTC.OpenPrice As OpenPrice,
PTC.ClosePrice As ClosePrice,
PT.Symbol As Symbol,
PT.FXRate As OpenTradeFXRate,
PT.FXConversionMethodOperator As OpenTradeFXOperator,
PT.OpenTotalCommissionandFees AS PTOpenTotalCommissionandFees,
PT.ClosedTotalCommissionandFees AS PTClosedTotalCommissionandFees,
PT.FundID As FundID,
PT.Level2ID As OpenTradeLevel2ID,
PT1.FXRate As CloseTradeFXRate,
PT1.FXConversionMethodOperator As CloseTradeFXOperator,
PT1.OpenTotalCommissionandFees AS PT1OpenTotalCommissionandFees,
PT1.ClosedTotalCommissionandFees AS PT1ClosedTotalCommissionandFees,
PT1.Level2ID As CloseTradeLevel2ID,
G.AUECLocalDate As OpenTradeDate,
G.CurrencyID As TradeCurrencyID,
G.OriginalPurchaseDate As OriginalPurchaseDate,
G1.AUECLocalDate As ClosingTradeDate,
DBO.GetSideMultiplierForClosing(G.OrderSideTagValue, G1.OrderSideTagValue) AS SideMultiplier,
G.AUECID As Auecid

FROM PM_TaxlotClosing PTC
INNER JOIN PM_Taxlots PT ON (PTC.PositionalTaxlotID = PT.TaxlotID AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)
INNER JOIN PM_Taxlots PT1 ON (PTC.ClosingTaxlotID = PT1.TaxlotID AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk)
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
INNER JOIN T_Group G1 ON G1.GroupID = PT1.GroupID
Where Datediff(Day,(DATEADD(hh, DATEDIFF(hh, getutcdate(), getdate()), PTC.TimeOfSaveUTC)),@inputDate) = 0

--Select * from #TempTaxlotInfo
SET @MinTradeDate = (
		SELECT Min(OpenTradeDate)
		FROM #TempTaxlotInfo
		WHERE DateDiff(d, OpenTradeDate, '01/01/1800') <> 0
		)
Set @AUECIDForOneDayBack= (Select max(AuecID) from #TempTaxlotInfo where datediff(d,OpenTradeDate,@MinTradeDate)=0)
IF (@MinTradeDate IS NULL)
BEGIN
	SET @MinTradeDate = @inputDate
	Set @AUECIDForOneDayBack=1
END

SET @MinTradeDate = dbo.AdjustBusinessDays(@MinTradeDate, - 1, @AUECIDForOneDayBack)
--- Get Forex Rate -------------------------
CREATE TABLE #FXConversionRates 
(
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(200)
	,FundID INT
)

INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @MinTradeDate ,@InputDate

Update #FXConversionRates                    
Set RateValue = 1.0/RateValue                    
Where RateValue <> 0 and ConversionMethod = 1   
         
-- For Fund Zero              
SELECT * INTO #ZeroFundFxRate              
FROM #FXConversionRates              
WHERE fundID = 0

--select * from #FXConversionRates
SELECT 
CF.FundName As Account,
Temp.Symbol As Symbol,
SM.BloombergSymbol As BloombergSymbol,
SM.CompanyName As [Description],
C.CurrencySymbol As TradeCurrency,
CONVERT(VARCHAR(10),Temp.OpenTradeDate, 101)  As OpenTradeDate,
CONVERT(VARCHAR(10), Temp.OriginalPurchaseDate, 101)  As OriginalPurchaseDate,
CONVERT(VARCHAR(10), Temp.ClosingTradeDate, 101) As ClosingTradeDate,
Temp.SideMultiplier As PositionType,
Temp.ClosedQty As ClosedQty,
Temp.OpenPrice As OpenPriceLocal,
Temp.ClosePrice As ClosePriceLocal,
(Temp.ClosedQty * Temp.OpenPrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PTClosedTotalCommissionandFees) As TotalCost_Local,
Case 
	When Temp.TradeCurrencyID = CF.LocalCurrency 
	Then (Temp.ClosedQty * Temp.OpenPrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PTClosedTotalCommissionandFees)
	When  Temp.OpenTradeFXRate > 0 And Temp.OpenTradeFXOperator='M'
	Then ((Temp.ClosedQty * Temp.OpenPrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PTClosedTotalCommissionandFees))*Temp.OpenTradeFXRate
	When  Temp.OpenTradeFXRate > 0 And Temp.OpenTradeFXOperator='D'
	Then  ((Temp.ClosedQty * Temp.OpenPrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PTClosedTotalCommissionandFees)) * (1/Temp.OpenTradeFXRate)
	Else ((Temp.ClosedQty * Temp.OpenPrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PTClosedTotalCommissionandFees)) * IsNull(FXRatesForTradeDate.Val,0) 
	End As TotalCost_Base,
(Temp.ClosedQty *Temp.ClosePrice * SM.Multiplier * SideMultiplier) + (Temp.PT1OpenTotalCommissionandFees) As Proceeds_Local,
Case 
	When SM.CurrencyID = CF.LocalCurrency 
	Then (Temp.ClosedQty * Temp.ClosePrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PT1ClosedTotalCommissionandFees)
	When IsNull(Temp.CloseTradeFXRate,0) > 0 And Temp.CloseTradeFXOperator='M'
	Then ((Temp.ClosedQty * Temp.ClosePrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PT1ClosedTotalCommissionandFees))*Temp.CloseTradeFXRate
	When IsNull(Temp.CloseTradeFXRate,0) > 0 And Temp.CloseTradeFXOperator='D'
	Then ((Temp.ClosedQty * Temp.ClosePrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PT1ClosedTotalCommissionandFees)) * (1/CloseTradeFXRate)
	Else ((Temp.ClosedQty * Temp.ClosePrice * SM.Multiplier * Temp.SideMultiplier) + (Temp.PT1ClosedTotalCommissionandFees)) * IsNull(FXRatesForEndDate.Val,0) 
	END As Proceeds_Base,
--Temp.ClosedQty * (Temp.ClosePrice - Temp.OpenPrice) As RealizedPNL,
--Case 
--	When SM.CurrencyID = CF.LocalCurrency 
--	Then Temp.ClosedQty * (Temp.ClosePrice-Temp.OpenPrice)
--	When IsNull(Temp.CloseTradeFXRate,0) > 0 And Temp.CloseTradeFXOperator='M'
--	Then Temp.ClosedQty * (Temp.ClosePrice-Temp.OpenPrice)*Temp.CloseTradeFXRate
--	When IsNull(Temp.CloseTradeFXRate,0) > 0 And Temp.CloseTradeFXOperator='D'
--	Then Temp.ClosedQty * (Temp.ClosePrice-Temp.OpenPrice)*(1/CloseTradeFXRate)
--Else Temp.ClosedQty * (Temp.ClosePrice-Temp.OpenPrice)*IsNull(FXRatesForEndDate.Val,0) 
--End As  RealizedPNL_Base,
OS.StrategyShortName As OpenStrategy,
CS.StrategyShortName As CloseStrategy
From #TempTaxlotInfo Temp
INNER JOIN T_companyFunds CF On CF.CompanyFundID = Temp.fundId
INNER JOIN V_SecMasterData SM On SM.TickerSymbol = Temp.Symbol
INNER JOIN T_Currency C On C.CurrencyID = SM.CurrencyID
INNER JOIN T_CompanyStrategy OS On OS.CompanyStrategyID = Temp.OpenTradeLevel2ID 
INNER JOIN T_CompanyStrategy CS On CS.CompanyStrategyID = Temp.CloseTradeLevel2ID 
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (  
				 FXDayRatesForTradeDate.FromCurrencyID = Temp.TradeCurrencyID
				AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
				AND DateDiff(d, Temp.OpenTradeDate, FXDayRatesForTradeDate.DATE) = 0                
				AND FXDayRatesForTradeDate.FundID = Temp.FundID              
				 ) 
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (     
		   ZeroFundFxRateTradeDate.FromCurrencyID = Temp.TradeCurrencyID           
		   AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency              
		   AND DateDiff(d, Temp.OpenTradeDate, ZeroFundFxRateTradeDate.DATE) = 0    
		   AND ZeroFundFxRateTradeDate.FundID = 0              
			)
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (  
				 FXDayRatesForEndDate.FromCurrencyID = Temp.TradeCurrencyID            
				AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
				AND DateDiff(d, Temp.ClosingTradeDate, FXDayRatesForEndDate.DATE) = 0                
				AND FXDayRatesForEndDate.FundID = Temp.FundID              
				 ) 
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (     
		   ZeroFundFxRateEndDate.FromCurrencyID = Temp.TradeCurrencyID                
		   AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency              
		   AND DateDiff(d, Temp.ClosingTradeDate, ZeroFundFxRateEndDate.DATE) = 0    
		   AND ZeroFundFxRateEndDate.FundID = 0              
			)	
CROSS APPLY (              
  SELECT 
	CASE               
	WHEN FXDayRatesForTradeDate.RateValue IS NULL  
	THEN 
		CASE      
			WHEN ZeroFundFxRateTradeDate.RateValue IS NULL              
			THEN 0        
		ELSE ZeroFundFxRateTradeDate.RateValue  
		END              
	ELSE FXDayRatesForTradeDate.RateValue              
	END              
  ) AS FXRatesForTradeDate(Val) 
CROSS APPLY (              
  SELECT 
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
  ) AS FXRatesForEndDate(Val) 

  Where Temp.FundID  in (select FundID from @Fund) 
--- Drop Table ----
Drop Table #FXConversionRates,#ZeroFundFxRate,#TempTaxlotInfo

