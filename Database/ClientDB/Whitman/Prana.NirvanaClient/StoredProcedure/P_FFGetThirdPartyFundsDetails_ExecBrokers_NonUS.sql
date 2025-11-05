/****** Object:  StoredProcedure [dbo].[P_FFGetThirdPartyFundsDetails_alan]    Script Date: 02/18/2010 14:11:53 *****                
-- 11/7/2011 -- special modification applied for Jasinkiewicz Capital, where fundname Jasinkiewicz Partners L.P. is replaced to J.P. Morgan, by request of Sandeep                
                
P_FFGetThirdPartyFundsDetails_alan 23,'1184,1183,1182,1186,1185','1/25/2010 3:51:24 PM',6,'1,20,21,18,1,15,12,16,33'                
[P_FFGetThirdPartyFundsDetails_ExecBrokers_test] '12/22/2010'                
[P_FFGetThirdPartyFundsDetails_ExecBrokers] '3/4/2013'                 
Modified by: Sandeep Singh              
Date: Feb 19, 2013              
Description: For Two Funds Compass and Compass Offshore, taxlot qty is summed up              
*/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_ExecBrokers_NonUS] (@inputDate DATETIME)
AS
SELECT
	-- 1 as Tag , --ashish                
	-- null as parent,    --ashish                                                                                             
	--                                                                                                   
	VT.TaxlotID AS TradeRefID
	,-- TaxlotID, changed by alan                                                                                                        
	ISNULL(VT.FundID, 0) AS FundID
	,--Sandeep Singh                    
	CASE 
		WHEN T_CompanyFunds.FundName = 'MR'
			THEN 'MR'
		ELSE T_CompanyFunds.FundName
		END AS AccountName
	,--changed by alan                  
	-- T_CompanyFunds.FundName,                                    
	ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID
	,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes
	,VT.OrderSideTagValue AS SideID
	,T_Side.Side AS Side
	,VT.Symbol
	,VT.CounterPartyID
	,T_CounterParty.ShortName AS CounterParty
	,--Sandeep Singh                                                                                             
	VT.VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                              
	convert(DECIMAL(18, 8), VT.AvgPrice) AS AveragePrice
	,--added as AveragePrice,added convert to money by alan                  
	-- Convert(int, AveragePrice),                                                                                                     
	VT.CumQty
	,--ExecutedQty                                                                                                        
	VT.Quantity
	,--TotalQty                                                                                                        
	VT.AUECID
	,VT.AssetID
	,T_Asset.AssetName
	,--Sandeep Singh                  
	T_Asset.AssetName AS Asset
	,VT.UnderlyingID
	,T_Underlying.UnderlyingName
	,--Sandeep Singh                                                 
	VT.ExchangeID
	,T_Exchange.DisplayName AS Exchange
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	,
	--CTPM.FundAccntNo as FundAccountNo, --added as FundAccountNo, changed by alan                                                                                                        
	VT.Level1AllocationID AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,--Percentage,                                                                                    
	convert(INT, Sum(VT.TaxLotQty)) AS AllocatedQty
	,-- orignally nothing after Sum(VT.TaxLotQty), added convert to int changed by alan                                                                                                  
	'' AS IsBasketGroup
	,SM.PutOrCall
	,convert(MONEY, SM.StrikePrice) AS StrikePrice
	,convert(CHAR(10), SM.ExpirationDate, 101) AS ExpirationDate
	,-- added convert date, alan                                                                                
	convert(CHAR(10), VT.SettlementDate, 101) AS SettlementDate
	,---- added convert date, alan                                                                                                   
	convert(MONEY, Sum(VT.Commission)) AS CommissionCharged
	,--added as CommissionCharged, added convert to money by alan                                                                                                      
	convert(MONEY, Sum(VT.OtherBrokerFees)) AS OtherBrokerFees
	,--added as OtherBrokerFees, added convert to money by alan                                                   
	0 AS SecFee
	,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CounterPartyVenue
	,-- originally ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName, changed by alan                                                                                                    
	VT.GroupRefID
	,convert(MONEY, Sum(ISNULL(VT.StampDuty, 0))) AS StampDuty
	,--added convert to money                                                         
	convert(MONEY, Sum(ISNULL(VT.TransactionLevy, 0))) AS TransactionLevy
	,--added convert to money                                                                  
	convert(MONEY, Sum(ISNULL(ClearingFee, 0))) AS ClearingFee
	,--added convert to money                                                         
	convert(MONEY, Sum(ISNULL(TaxOnCommissions, 0))) AS TaxOnCommissions
	,--added convert to money                                                                    
	convert(MONEY, Sum(ISNULL(MiscFees, 0))) AS MiscFees
	,--added convert to money                                                
	convert(CHAR(10), VT.AUECLocalDate, 101) AS TradeDate
	,-- added as TradeDate, added convert, changed by alan                                                         
	SM.Multiplier
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.ReutersSymbol
	,SM.BloombergSymbol
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.LeadCurrencyID
	,SM.LeadCurrency
	,SM.VsCurrencyID
	,SM.VsCurrency
	,SM.OSISymbol
	,SM.OpraSymbol
	,SM.IDCOSymbol
FROM V_TaxLots VT
--inner join T_CompanyThirdPartyMappingDetails as CTPM on  CTPM.InternalFundNameID_FK = VT.FundID                        
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = VT.UnderlyingID
LEFT OUTER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = VT.ExchangeID
WHERE DateDiff(d, VT.AUECLocalDate, @inputDate) = 0
	AND T_asset.AssetID <> '4'
	AND (
		VT.Symbol LIKE '%-EUX'
		OR (
			VT.CurrencyID = 4
			AND VT.Symbol LIKE '%-ICL'
			)
		)
--And T_CompanyFunds.FundName like ('Maerisland%')                          
--where AUECLocalDate > '2015-02-12 23:59:00.000' and AUECLocalDate < '2015-02-14 00:00:00.000' --And T_CompanyFunds.FundName like ('Maerisland%')                          
GROUP BY VT.TaxlotID
	,VT.Level1AllocationID
	,VT.FundID
	,T_CompanyFunds.FundName
	,T_OrderType.OrderTypesID
	,T_OrderType.OrderTypes
	,VT.OrderSideTagValue
	,T_Side.Side
	,VT.Symbol
	,VT.CounterPartyID
	,VT.VenueID
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,T_Exchange.DisplayName
	,Currency.CurrencyID
	,Currency.CurrencyName
	,Currency.CurrencySymbol
	,
	--CTPM.FundAccntNo,                                                                                                          
	SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate
	,VT.SettlementDate
	,T_CounterPartyVenue.DisplayName
	,VT.GroupRefID
	,VT.AUECLocalDate
	,SM.Multiplier
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.ReutersSymbol
	,SM.BloombergSymbol
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.LeadCurrencyID
	,SM.LeadCurrency
	,SM.VsCurrencyID
	,SM.VsCurrency
	,T_CounterParty.ShortName
	,T_Asset.AssetName
	,T_CounterParty.ShortName
	,T_Underlying.UnderLyingName
	,SM.OSISymbol
	,SM.OpraSymbol
	,SM.IDCOSymbol
FOR XML Path('ThirdPartyFlatFileDetail')
	,root('ThirdPartyFlatFileDetailCollection')
	--FOR XML EXPLICIT 