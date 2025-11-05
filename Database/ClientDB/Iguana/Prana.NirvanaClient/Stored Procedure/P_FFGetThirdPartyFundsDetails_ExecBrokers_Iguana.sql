/****** Object:  StoredProcedure [dbo].[P_FFGetThirdPartyFundsDetails_alan]    Script Date: 02/18/2010 14:11:53 *****                                                                        
-- 11/7/2011 -- special modification applied for Jasinkiewicz Capital, where fundname Jasinkiewicz Partners L.P. is replaced to J.P. Morgan, by request of Sandeep                                                                        
                                                                        
P_FFGetThirdPartyFundsDetails_alan 23,'1184,1183,1182,1186,1185','1/25/2010 3:51:24 PM',6,'1,20,21,18,1,15,12,16,33'                                                                        
[P_FFGetThirdPartyFundsDetails_ExecBrokers_test] '12/22/2010'                                                                        
[P_FFGetThirdPartyFundsDetails_ExecBrokers] '2/15/2013'                                                                         
Modified by: Sandeep Singh                                                                      
Date: Feb 19, 2013                                                                      
Description: For Two Funds Compass and Compass Offshore, taxlot qty is summed up                                                                      
P_FFGetThirdPartyFundsDetails_ExecBrokers_Iguana '03-08-2016'                                                                      
*/
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_ExecBrokers_Iguana] (@inputDate DATETIME)
AS
SELECT
	-- 1 as Tag , --ashish                                                                        
	-- null as parent,    --ashish                                                                                                                                                     
	--                                                                                                                                                           
	VT.TaxlotID AS TradeRefID
	,-- TaxlotID, changed by alan                                                                                                                                                                
	ISNULL(VT.FundID, 0) AS AccountID
	,--Sandeep Singh                                                                            
	CASE 
		WHEN T_CompanyFunds.FundName = 'Iguana Healthcare-Lazard'
			THEN 'Iguana@Pershing'
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
	convert(DECIMAL(18, 4), VT.AvgPrice) AS AveragePrice
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
	AND T_CompanyFunds.FundName IN ('Iguana@Pershing')
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

UNION ALL

-- this union will be executed for 2 funds i.e. Compass and Compass Offshore. Here we have to do sum of taxlot qunatity                                                                      
-- TradeRefID field was giving the TaxlotID, now we are combining the Taxlot Qty, so it is replaced with GroupID                       
SELECT
	-- Group by GroupRefID, so there is no use of TaxlotID                                                                  
	-- As there is a unique GroupRefID for each group, so group information will remains same if take max or min                                                                      
	Max(VT.GroupID) AS TradeRefID
	,Max(VT.FundID) AS AccountID
	,'Iguana@JPM' AS AccountName
	,Max(ISNULL(T_OrderType.OrderTypesID, 0)) AS OrderTypesID
	,Max(ISNULL(T_OrderType.OrderTypes, 'Multiple')) AS OrderTypes
	,Max(VT.OrderSideTagValue) AS SideID
	,-- there is a unique GroupRefID for each group, so Side Tag value will give the correct value                                                                                                                           
	Max(T_Side.Side) AS Side
	,Max(VT.Symbol) AS Symbol
	,Max(VT.CounterPartyID) AS CounterPartyID
	,Max(T_CounterParty.ShortName) AS CounterParty
	,Max(VT.VenueID) AS VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                                                                                      
	Max(Convert(DECIMAL(18, 4), VT.AvgPrice)) AS AveragePrice
	,--added as AveragePrice,added convert to money by alan                                                                                                         
	Max(VT.CumQty) AS CumQty
	,Max(VT.Quantity) AS Quantity
	,Max(VT.AUECID) AS AUECID
	,Max(VT.AssetID) AS AssetID
	,Max(T_Asset.AssetName) AS AssetName
	,Max(T_Asset.AssetName) AS Asset
	,Max(VT.UnderlyingID) AS UnderlyingID
	,Max(T_Underlying.UnderlyingName) AS UnderlyingName
	,Max(VT.ExchangeID) AS ExchangeID
	,Max(T_Exchange.DisplayName) AS Exchange
	,Max(Currency.CurrencyID) AS CurrencyID
	,Max(Currency.CurrencyName) AS CurrencyName
	,Max(Currency.CurrencySymbol) AS CurrencySymbol
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,--Percentage,                                                                                            
	Convert(INT, Sum(VT.TaxLotQty)) AS AllocatedQty
	,-- orignally nothing after Sum(VT.TaxLotQty), added convert to int changed by alan                                                                                                                          
	'' AS IsBasketGroup
	,Max(SM.PutOrCall) AS PutOrCall
	,Max(convert(MONEY, SM.StrikePrice)) AS StrikePrice
	,Max(convert(CHAR(10), SM.ExpirationDate, 101)) AS ExpirationDate
	,-- added convert date, alan                                                                                              
	Max(convert(CHAR(10), VT.SettlementDate, 101)) AS SettlementDate
	,---- added convert date, alan                                                                                                                           
	Convert(MONEY, Sum(VT.Commission)) AS CommissionCharged
	,--added as CommissionCharged, added convert to money by alan                                                                                                                                 
	Convert(MONEY, Sum(VT.OtherBrokerFees)) AS OtherBrokerFees
	,--added as OtherBrokerFees, added convert to money by alan                                                                                                           
	0 AS SecFee
	,Max(ISNULL(T_CounterPartyVenue.DisplayName, '')) AS CounterPartyVenue
	,-- originally ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName, changed by alan                                                                            
	VT.GroupRefID
	,Convert(MONEY, Sum(ISNULL(VT.StampDuty, 0))) AS StampDuty
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(VT.TransactionLevy, 0))) AS TransactionLevy
	,--added convert to money                                                                 
	Convert(MONEY, Sum(ISNULL(ClearingFee, 0))) AS ClearingFee
	,--added convert to money                                                       
	Convert(MONEY, Sum(ISNULL(TaxOnCommissions, 0))) AS TaxOnCommissions
	,--added convert to money                                                                                                                     
	Convert(MONEY, Sum(ISNULL(MiscFees, 0))) AS MiscFees
	,--added convert to money                                                           
	Max(convert(CHAR(10), VT.AUECLocalDate, 101)) AS TradeDate
	,-- added as TradeDate, added convert, changed by alan                                                                                                           
	Max(SM.Multiplier) AS Multiplier
	,Max(SM.ISINSymbol) AS ISINSymbol
	,Max(SM.CUSIPSymbol) AS CUSIPSymbol
	,Max(SM.SEDOLSymbol) AS SEDOLSymbol
	,Max(SM.ReutersSymbol) AS ReutersSymbol
	,Max(SM.BloombergSymbol) AS BloombergSymbol
	,Max(SM.CompanyName) AS CompanyName
	,Max(SM.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(SM.LeadCurrencyID) AS LeadCurrencyID
	,Max(SM.LeadCurrency) AS LeadCurrency
	,Max(SM.VsCurrencyID) AS VsCurrencyID
	,Max(SM.VsCurrency) AS VsCurrency
	,Max(SM.OSISymbol) AS OSISymbol
	,Max(SM.OpraSymbol) AS OpraSymbol
	,Max(SM.IDCOSymbol) AS IDCOSymbol
FROM V_TaxLots VT
LEFT OUTER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT OUTER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT OUTER JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT OUTER JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = VT.UnderlyingID
LEFT OUTER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = VT.ExchangeID
WHERE DateDiff(d, VT.AUECLocalDate, @inputDate) = 0
	AND T_CompanyFunds.FundName IN (
		'Iguana Healthcare-JPM'
		,'New Issues-JPM'
		,'Whitney Capital-JPM'
		,'Iguana IPO'
		)
--where AUECLocalDate > '2016-10-11 23:59:00.000' and AUECLocalDate < '2016-10-13 00:00:00.000' And T_CompanyFunds.FundName In ('Iguana Healthcare-JPM', 'Iguana IPO')                                                                     
--and symbol In ('REGN')                                                                    
--and symbol In ('CLLS','JUNO','O:IWM 16S113.00D8')                                 
GROUP BY VT.GroupRefID

UNION ALL

-- this union will be executed for 2 funds i.e. Compass and Compass Offshore. Here we have to do sum of taxlot qunatity                                                                      
-- TradeRefID field was giving the TaxlotID, now we are combining the Taxlot Qty, so it is replaced with GroupID                                                                      
SELECT
	-- Group by GroupRefID, so there is no use of TaxlotID                                                                      
	-- As there is a unique GroupRefID for each group, so group information will remains same if take max or min                                             
	Max(VT.GroupID) AS TradeRefID
	,Max(VT.FundID) AS AccountID
	,'Iguana@BNP' AS AccountName
	,Max(ISNULL(T_OrderType.OrderTypesID, 0)) AS OrderTypesID
	,Max(ISNULL(T_OrderType.OrderTypes, 'Multiple')) AS OrderTypes
	,Max(VT.OrderSideTagValue) AS SideID
	,-- there is a unique GroupRefID for each group, so Side Tag value will give the correct value                                                                                                                           
	Max(T_Side.Side) AS Side
	,Max(VT.Symbol) AS Symbol
	,Max(VT.CounterPartyID) AS CounterPartyID
	,Max(T_CounterParty.ShortName) AS CounterParty
	,Max(VT.VenueID) AS VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                             
	Max(Convert(DECIMAL(18, 4), VT.AvgPrice)) AS AveragePrice
	,--added as AveragePrice,added convert to money by alan                                                                                                                         
	Max(VT.CumQty) AS CumQty
	,Max(VT.Quantity) AS Quantity
	,Max(VT.AUECID) AS AUECID
	,Max(VT.AssetID) AS AssetID
	,Max(T_Asset.AssetName) AS AssetName
	,Max(T_Asset.AssetName) AS Asset
	,Max(VT.UnderlyingID) AS UnderlyingID
	,Max(T_Underlying.UnderlyingName) AS UnderlyingName
	,Max(VT.ExchangeID) AS ExchangeID
	,Max(T_Exchange.DisplayName) AS Exchange
	,Max(Currency.CurrencyID) AS CurrencyID
	,Max(Currency.CurrencyName) AS CurrencyName
	,Max(Currency.CurrencySymbol) AS CurrencySymbol
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,--Percentage,                                                               
	Convert(INT, Sum(VT.TaxLotQty)) AS AllocatedQty
	,-- orignally nothing after Sum(VT.TaxLotQty), added convert to int changed by alan                                                       
	'' AS IsBasketGroup
	,Max(SM.PutOrCall) AS PutOrCall
	,Max(convert(MONEY, SM.StrikePrice)) AS StrikePrice
	,Max(convert(CHAR(10), SM.ExpirationDate, 101)) AS ExpirationDate
	,-- added convert date, alan                                                                              
	Max(convert(CHAR(10), VT.SettlementDate, 101)) AS SettlementDate
	,---- added convert date, alan                                                                                                                         
	Convert(MONEY, Sum(VT.Commission)) AS CommissionCharged
	,--added as CommissionCharged, added convert to money by alan                                                                                  
	Convert(MONEY, Sum(VT.OtherBrokerFees)) AS OtherBrokerFees
	,--added as OtherBrokerFees, added convert to money by alan                                                                                                           
	0 AS SecFee
	,Max(ISNULL(T_CounterPartyVenue.DisplayName, '')) AS CounterPartyVenue
	,-- originally ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName, changed by alan                                                                      
	VT.GroupRefID
	,Convert(MONEY, Sum(ISNULL(VT.StampDuty, 0))) AS StampDuty
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(VT.TransactionLevy, 0))) AS TransactionLevy
	,--added convert to money                                                                                
	Convert(MONEY, Sum(ISNULL(ClearingFee, 0))) AS ClearingFee
	,--added convert to money                                                                             
	Convert(MONEY, Sum(ISNULL(TaxOnCommissions, 0))) AS TaxOnCommissions
	,--added convert to money                                                                                                                            
	Convert(MONEY, Sum(ISNULL(MiscFees, 0))) AS MiscFees
	,--added convert to money                                                                                                        
	Max(convert(CHAR(10), VT.AUECLocalDate, 101)) AS TradeDate
	,-- added as TradeDate, added convert, changed by alan                                                                                                                 
	Max(SM.Multiplier) AS Multiplier
	,Max(SM.ISINSymbol) AS ISINSymbol
	,Max(SM.CUSIPSymbol) AS CUSIPSymbol
	,Max(SM.SEDOLSymbol) AS SEDOLSymbol
	,Max(SM.ReutersSymbol) AS ReutersSymbol
	,Max(SM.BloombergSymbol) AS BloombergSymbol
	,Max(SM.CompanyName) AS CompanyName
	,Max(SM.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(SM.LeadCurrencyID) AS LeadCurrencyID
	,Max(SM.LeadCurrency) AS LeadCurrency
	,Max(SM.VsCurrencyID) AS VsCurrencyID
	,Max(SM.VsCurrency) AS VsCurrency
	,Max(SM.OSISymbol) AS OSISymbol
	,Max(SM.OpraSymbol) AS OpraSymbol
	,Max(SM.IDCOSymbol) AS IDCOSymbol
FROM V_TaxLots VT
LEFT OUTER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT OUTER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT OUTER JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT OUTER JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = VT.UnderlyingID
LEFT OUTER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = VT.ExchangeID
WHERE DateDiff(d, VT.AUECLocalDate, @inputDate) = 0
	AND T_CompanyFunds.FundName IN (
		'East River'
		,'East River IPO'
		)
--where AUECLocalDate > '2016-10-11 23:59:00.000' and AUECLocalDate < '2016-10-13 00:00:00.000'                                                                     
--And T_CompanyFunds.FundName In ('East River','East River IPO')                                                                    
--and symbol In ('KITE')                                                                    
--and symbol In ('CLLS','JUNO','O:IWM 16S113.00D8')                                                                    
GROUP BY VT.GroupRefID

UNION ALL

-- this union will be executed for 2 funds i.e. Compass and Compass Offshore. Here we have to do sum of taxlot qunatity                                                                      
-- TradeRefID field was giving the TaxlotID, now we are combining the Taxlot Qty, so it is replaced with GroupID                                                                      
SELECT
	-- Group by GroupRefID, so there is no use of TaxlotID                     
	-- As there is a unique GroupRefID for each group, so group information will remains same if take max or min                                                                      
	Max(VT.GroupID) AS TradeRefID
	,Max(VT.FundID) AS AccountID
	,'Iguana@JEFF' AS AccountName
	,Max(ISNULL(T_OrderType.OrderTypesID, 0)) AS OrderTypesID
	,Max(ISNULL(T_OrderType.OrderTypes, 'Multiple')) AS OrderTypes
	,Max(VT.OrderSideTagValue) AS SideID
	,-- there is a unique GroupRefID for each group, so Side Tag value will give the correct value                                                                                                                           
	Max(T_Side.Side) AS Side
	,Max(VT.Symbol) AS Symbol
	,Max(VT.CounterPartyID) AS CounterPartyID
	,Max(T_CounterParty.ShortName) AS CounterParty
	,Max(VT.VenueID) AS VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                                                                                      
	Max(Convert(DECIMAL(18, 4), VT.AvgPrice)) AS AveragePrice
	,--added as AveragePrice,added convert to money by alan                                                                                                                                             
	Max(VT.CumQty) AS CumQty
	,Max(VT.Quantity) AS Quantity
	,Max(VT.AUECID) AS AUECID
	,Max(VT.AssetID) AS AssetID
	,Max(T_Asset.AssetName) AS AssetName
	,Max(T_Asset.AssetName) AS Asset
	,Max(VT.UnderlyingID) AS UnderlyingID
	,Max(T_Underlying.UnderlyingName) AS UnderlyingName
	,Max(VT.ExchangeID) AS ExchangeID
	,Max(T_Exchange.DisplayName) AS Exchange
	,Max(Currency.CurrencyID) AS CurrencyID
	,Max(Currency.CurrencyName) AS CurrencyName
	,Max(Currency.CurrencySymbol) AS CurrencySymbol
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,--Percentage,                                                                                            
	Convert(INT, Sum(VT.TaxLotQty)) AS AllocatedQty
	,-- orignally nothing after Sum(VT.TaxLotQty), added convert to int changed by alan                                                       
	'' AS IsBasketGroup
	,Max(SM.PutOrCall) AS PutOrCall
	,Max(convert(MONEY, SM.StrikePrice)) AS StrikePrice
	,Max(convert(CHAR(10), SM.ExpirationDate, 101)) AS ExpirationDate
	,-- added convert date, alan                                                                                              
	Max(convert(CHAR(10), VT.SettlementDate, 101)) AS SettlementDate
	,---- added convert date, alan                                                         
	Convert(MONEY, Sum(VT.Commission)) AS CommissionCharged
	,--added as CommissionCharged, added convert to money by alan                                                                                                                                 
	Convert(MONEY, Sum(VT.OtherBrokerFees)) AS OtherBrokerFees
	,--added as OtherBrokerFees, added convert to money by alan                                  
	0 AS SecFee
	,Max(ISNULL(T_CounterPartyVenue.DisplayName, '')) AS CounterPartyVenue
	,-- originally ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName, changed by alan                                         
	VT.GroupRefID
	,Convert(MONEY, Sum(ISNULL(VT.StampDuty, 0))) AS StampDuty
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(VT.TransactionLevy, 0))) AS TransactionLevy
	,--added convert to money                                                                                 
	Convert(MONEY, Sum(ISNULL(ClearingFee, 0))) AS ClearingFee
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(TaxOnCommissions, 0))) AS TaxOnCommissions
	,--added convert to money                                                                                                                            
	Convert(MONEY, Sum(ISNULL(MiscFees, 0))) AS MiscFees
	,--added convert to money                                                           
	Max(convert(CHAR(10), VT.AUECLocalDate, 101)) AS TradeDate
	,-- added as TradeDate, added convert, changed by alan                                                            
	Max(SM.Multiplier) AS Multiplier
	,Max(SM.ISINSymbol) AS ISINSymbol
	,Max(SM.CUSIPSymbol) AS CUSIPSymbol
	,Max(SM.SEDOLSymbol) AS SEDOLSymbol
	,Max(SM.ReutersSymbol) AS ReutersSymbol
	,Max(SM.BloombergSymbol) AS BloombergSymbol
	,Max(SM.CompanyName) AS CompanyName
	,Max(SM.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(SM.LeadCurrencyID) AS LeadCurrencyID
	,Max(SM.LeadCurrency) AS LeadCurrency
	,Max(SM.VsCurrencyID) AS VsCurrencyID
	,Max(SM.VsCurrency) AS VsCurrency
	,Max(SM.OSISymbol) AS OSISymbol
	,Max(SM.OpraSymbol) AS OpraSymbol
	,Max(SM.IDCOSymbol) AS IDCOSymbol
FROM V_TaxLots VT
LEFT OUTER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT OUTER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT OUTER JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT OUTER JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = VT.UnderlyingID
LEFT OUTER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = VT.ExchangeID
WHERE DateDiff(d, VT.AUECLocalDate, @inputDate) = 0
	AND T_CompanyFunds.FundName IN ('Iguana Healthcare-Jefferies')
--where AUECLocalDate > '2016-10-11 23:59:00.000' and AUECLocalDate < '2016-10-13 00:00:00.000' And T_CompanyFunds.FundName In ('Iguana Healthcare-JPM', 'Iguana IPO')                                                                     
--and symbol In ('REGN')                                     
--and symbol In ('CLLS','JUNO','O:IWM 16S113.00D8')                                                                    
GROUP BY VT.GroupRefID

UNION ALL

-- this union will be executed for 2 funds i.e. Compass and Compass Offshore. Here we have to do sum of taxlot qunatity                                                                      
-- TradeRefID field was giving the TaxlotID, now we are combining the Taxlot Qty, so it is replaced with GroupID                                                                      
SELECT
	-- Group by GroupRefID, so there is no use of TaxlotID                                                  
	-- As there is a unique GroupRefID for each group, so group information will remains same if take max or min                                                                      
	Max(VT.GroupID) AS TradeRefID
	,Max(VT.FundID) AS AccountID
	,'OCA@JEFF' AS AccountName
	,Max(ISNULL(T_OrderType.OrderTypesID, 0)) AS OrderTypesID
	,Max(ISNULL(T_OrderType.OrderTypes, 'Multiple')) AS OrderTypes
	,Max(VT.OrderSideTagValue) AS SideID
	,-- there is a unique GroupRefID for each group, so Side Tag value will give the correct value                                                                                                                            
	Max(T_Side.Side) AS Side
	,Max(VT.Symbol) AS Symbol
	,Max(VT.CounterPartyID) AS CounterPartyID
	,Max(T_CounterParty.ShortName) AS CounterParty
	,Max(VT.VenueID) AS VenueID
	,Sum(VT.TaxLotQty) AS OrderQty
	,--AllocatedQty                                                                                                              
	Max(Convert(DECIMAL(18, 4), VT.AvgPrice)) AS AveragePrice
	,--added as AveragePrice,added convert to money by alan                                                                                                                                             
	Max(VT.CumQty) AS CumQty
	,Max(VT.Quantity) AS Quantity
	,Max(VT.AUECID) AS AUECID
	,Max(VT.AssetID) AS AssetID
	,Max(T_Asset.AssetName) AS AssetName
	,Max(T_Asset.AssetName) AS Asset
	,Max(VT.UnderlyingID) AS UnderlyingID
	,Max(T_Underlying.UnderlyingName) AS UnderlyingName
	,Max(VT.ExchangeID) AS ExchangeID
	,Max(T_Exchange.DisplayName) AS Exchange
	,Max(Currency.CurrencyID) AS CurrencyID
	,Max(Currency.CurrencyName) AS CurrencyName
	,Max(Currency.CurrencySymbol) AS CurrencySymbol
	,Max(VT.Level1AllocationID) AS Level1AllocationID
	,Sum(VT.Level2Percentage) AS Level2Percentage
	,--Percentage,                                 
	Convert(INT, Sum(VT.TaxLotQty)) AS AllocatedQty
	,-- orignally nothing after Sum(VT.TaxLotQty), added convert to int changed by alan                                                                                                                          
	'' AS IsBasketGroup
	,Max(SM.PutOrCall) AS PutOrCall
	,Max(convert(MONEY, SM.StrikePrice)) AS StrikePrice
	,Max(convert(CHAR(10), SM.ExpirationDate, 101)) AS ExpirationDate
	,-- added convert date, alan                                                                               
	Max(convert(CHAR(10), VT.SettlementDate, 101)) AS SettlementDate
	,---- added convert date, alan                                                         
	Convert(MONEY, Sum(VT.Commission)) AS CommissionCharged
	,--added as CommissionCharged, added convert to money by alan                                                                                                                                 
	Convert(MONEY, Sum(VT.OtherBrokerFees)) AS OtherBrokerFees
	,--added as OtherBrokerFees, added convert to money by alan                                                                                                           
	0 AS SecFee
	,Max(ISNULL(T_CounterPartyVenue.DisplayName, '')) AS CounterPartyVenue
	,-- originally ISNULL(T_CounterPartyVenue.DisplayName,'')as CVName, changed by alan                                         
	VT.GroupRefID
	,Convert(MONEY, Sum(ISNULL(VT.StampDuty, 0))) AS StampDuty
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(VT.TransactionLevy, 0))) AS TransactionLevy
	,--added convert to money                                                                                 
	Convert(MONEY, Sum(ISNULL(ClearingFee, 0))) AS ClearingFee
	,--added convert to money                                                                                                                 
	Convert(MONEY, Sum(ISNULL(TaxOnCommissions, 0))) AS TaxOnCommissions
	,--added convert to money                                                                                                                            
	Convert(MONEY, Sum(ISNULL(MiscFees, 0))) AS MiscFees
	,--added convert to money                                                           
	Max(convert(CHAR(10), VT.AUECLocalDate, 101)) AS TradeDate
	,-- added as TradeDate, added convert, changed by alan                                                            
	Max(SM.Multiplier) AS Multiplier
	,Max(SM.ISINSymbol) AS ISINSymbol
	,Max(SM.CUSIPSymbol) AS CUSIPSymbol
	,Max(SM.SEDOLSymbol) AS SEDOLSymbol
	,Max(SM.ReutersSymbol) AS ReutersSymbol
	,Max(SM.BloombergSymbol) AS BloombergSymbol
	,Max(SM.CompanyName) AS CompanyName
	,Max(SM.UnderlyingSymbol) AS UnderlyingSymbol
	,Max(SM.LeadCurrencyID) AS LeadCurrencyID
	,Max(SM.LeadCurrency) AS LeadCurrency
	,Max(SM.VsCurrencyID) AS VsCurrencyID
	,Max(SM.VsCurrency) AS VsCurrency
	,Max(SM.OSISymbol) AS OSISymbol
	,Max(SM.OpraSymbol) AS OpraSymbol
	,Max(SM.IDCOSymbol) AS IDCOSymbol
FROM V_TaxLots VT
LEFT OUTER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT OUTER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
LEFT OUTER JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue
LEFT OUTER JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID
	AND T_CounterPartyVenue.VenueID = VT.VenueID
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
LEFT OUTER JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
LEFT OUTER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
LEFT OUTER JOIN T_Underlying ON T_Underlying.UnderlyingID = VT.UnderlyingID
LEFT OUTER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
LEFT OUTER JOIN T_Exchange ON T_Exchange.ExchangeID = VT.ExchangeID
WHERE DateDiff(d, VT.AUECLocalDate, @inputDate) = 0
	AND T_CompanyFunds.FundName IN ('OCA Iguana')
--where AUECLocalDate > '2016-10-11 23:59:00.000' and AUECLocalDate < '2016-10-13 00:00:00.000' And T_CompanyFunds.FundName In ('Iguana Healthcare-JPM', 'Iguana IPO')                                                                     
--and symbol In ('REGN')                                                                    
--and symbol In ('CLLS','JUNO','O:IWM 16S113.00D8')                                                                    
GROUP BY VT.GroupRefID
ORDER BY GroupRefID
FOR XML Path('ThirdPartyFlatFileDetail')
	,root('ThirdPartyFlatFileDetailCollection')
	--FOR XML EXPLICIT 