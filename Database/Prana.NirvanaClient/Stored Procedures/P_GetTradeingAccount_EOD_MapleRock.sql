/*
Author: Kuldeep Kumar
Creation Date: 26 Aug 2021
Description: Get Tradeing Account Name in EOD for Maple Rock
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-2954
exec [P_GetTradeingAccount_EOD_MapleRock]

*/
  
CREATE PROCEDURE [dbo].[P_GetTradeingAccount_EOD_MapleRock] 
(  
 @thirdPartyID INT  
 ,@companyFundIDs VARCHAR(max)  
 ,@inputDate DATETIME  
 ,@companyID INT  
 ,@auecIDs VARCHAR(max)  
 ,@TypeID INT  
 ,@dateType INT         
 ,@fileFormatID INT  
 )  
AS

DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
                                                                         
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  

Declare @PreviousBusinessDate DateTime
Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@inputDate,-1,1)

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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @PreviousBusinessDate
	,@inputDate

UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE fundID = 0

SELECT
 CF.FundShortName AS AccountName
 ,VT.Symbol As Symbol  
 ,T_Side.Side AS Side  
 ,Convert(varchar, VT.AUECLocalDate, 101) As TradeDate
 ,Convert(varchar, VT.ProcessDate, 101) As ProcessDate 
 ,Convert(varchar, VT.SettlementDate, 101) As SettlementDate  
 ,SM.CompanyName As [SecurityDescription]
 ,VT.AllocatedQty As [AllocatedQty]
 ,SM.CUSIPSymbol AS CUSIP
 ,SM.BloombergSymbol As BBGSymbol
 ,Currency.CurrencySymbol AS CurrencySymbol
 ,COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency
  ,CP.Shortname AS CounterParty
 ,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
 ,VT.AvgPrice As AvgPrice
 ,VT.FXRate_Taxlot
  ,VT.FXConversionMethodOperator_Taxlot  
 , VT.CumQty AS ExecutedQty
 ,ISNULL(VT.Commission,0) AS CommissionCharged
 ,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(VT.ClearingFee, 0) AS ClearingFee
	,ISNULL(VT.TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(VT.MiscFees, 0) AS MiscFees
	,ISNULL(VT.SecFee, 0) AS SecFee
	,ISNULL(VT.OccFee, 0) AS OccFee
	,ISNULL(VT.OrfFee, 0) AS OrfFee
	,ISNULL(VT.OtherBrokerFees, 0) AS OtherBrokerFees
	,ISNULL(VT.ClearingBrokerFee,0) AS ClearingBrokerFee
	,ISNULL(VT.AccruedInterest,0) AS AccruedInterest
	,ISNULL(VT.SoftCommission,0) AS SoftCommissionCharged
	,ISNULL(FXRatesForTradeDate.val,0) AS ForexRate
  ,IsNull(CTA.TradingAccountName,'') As TradingAccount
  ,IsNull(VT.TradeAttribute1,'') AS BorrowBroker
  ,IsNull(VT.TradeAttribute2,0)  AS BorrowID
  ,IsNull(VT.TradeAttribute3,0) AS BorrowQty
  ,IsNull(VT.TradeAttribute4,0) AS BorrowRate
  ,Case 
	  When G.IsSwapped  = 1
	   Then 'EquitySwap'
	  Else A.AssetName 
   End AS AssetClass
  
FROM V_Taxlots VT 
Inner join T_Asset A ON A.AssetID = VT.AssetID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner Join T_CompanyTradingAccounts AS CTA on CTA.CompanyTradingAccountsID =VT.TradingAccountID
Inner JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue  
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
Inner Join T_Group G ON G.GroupID = VT.GroupID
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = VT.FundID
		)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
		ZeroFundFxRateTradeDate.FromCurrencyID = VT.CurrencyID
		AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, VT.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
		AND ZeroFundFxRateTradeDate.FundID = 0
		)
CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForTradeDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
							THEN 0
						ELSE ZeroFundFxRateTradeDate.RateValue
						END
			ELSE FXDayRatesForTradeDate.RateValue
			END
	) AS FXRatesForTradeDate(Val)

WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.AUECLocalDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0  
 
 
ORDER BY TaxlotId
