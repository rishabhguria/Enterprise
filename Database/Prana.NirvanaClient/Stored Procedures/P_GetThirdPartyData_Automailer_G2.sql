
/*       
EXEC [P_GetThirdPartyData_Automailer_G2]  '03-31-2021'
Created By: Kuldeep kumar
Desc: Grouping the data on the basis of Side, Symbol, CounterParty, AvgPrice and PB.
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-2314  
*/
              
CREATE PROCEDURE [dbo].[P_GetThirdPartyData_Automailer_G2]                                                                                       
(                                                                                                      
 @inputDate datetime                                                                                                  
)                                                                                                         
AS          
     
     
 --Declare @inputDate datetime 
 --Set @inputDate = '04-31-2021'

 CREATE TABLE #VT (
    AccountName VARCHAR(100),
	TaxLotID VARCHAR(50)
	,FundID INT
	,OrderTypeTagValue VARCHAR(3)
	,SideID VARCHAR(3)
	,Symbol VARCHAR(100)
	,CounterPartyID INT
	,SideMultiplier INT
	,OrderQty FLOAT
	,AvgPrice FLOAT
	,CumQty FLOAT
	,Quantity FLOAT
	,AUECID INT
	,AssetID INT
	,UnderlyingID INT
	,ExchangeID INT
	,CurrencyID INT
	,Level1AllocationID VARCHAR(50)
	,Level2Percentage FLOAT
	,TaxLotQty FLOAT
	,IsBasketGroup VARCHAR(20)
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,GroupRefID INT
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,Level2ID INT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,FromDeleted VARCHAR(5)
	,ProcessDate DATETIME
	,OriginalPurchaseDate DATETIME
	,AccruedInterest FLOAT
	,BenchMarkRate FLOAT
	,Differential FLOAT
	,SwapDescription VARCHAR(500)
	,DayCount INT
	,FirstResetDate DATETIME
	--,IsSwapped BIT
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(3)	
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,TransactionType VARCHAR(200)
	,SettlCurrency INT
	,TotalExpenses FLOAT
	,PBName VARCHAR(200)
	)

INSERT INTO #VT
SELECT
    CF.FundName AS AccountName,
	 VT.TaxLotID AS TaxlotID
	,ISNULL(VT.FundID, 0) AS FundID
	,VT.OrderTypeTagValue
	,VT.OrderSideTagValue AS SideID
	,VT.Symbol
	,VT.CounterPartyID	
	,VT.SideMultiplier	
	,(VT.TaxLotQty) AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.AUECID
	,VT.AssetID
	,VT.UnderlyingID
	,VT.ExchangeID
	,VT.CurrencyID
	,VT.Level1AllocationID AS Level1AllocationID
	,(VT.Level2Percentage)
	,(VT.TaxLotQty)
	,'' AS IsBasketGroup
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees
	,VT.GroupRefID
	,ISNULL(VT.StampDuty, 0) AS StampDuty
	,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy
	,ISNULL(ClearingFee, 0) AS ClearingFee
	,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions
	,ISNULL(MiscFees, 0) AS MiscFees
	,VT.AUECLocalDate
	,0 AS Level2ID
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,'No' AS FromDeleted
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.AccruedInterest
	,VT.BenchMarkRate
	,VT.Differential
	,VT.SwapDescription
	,VT.DayCount
	,VT.FirstResetDate
	--,VT.IsSwapped
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot	
	,ISNULL(VT.SecFee, 0) AS SecFee
	,ISNULL(VT.OccFee, 0) AS OccFee
	,ISNULL(VT.OrfFee, 0) AS OrfFee
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,VT.TransactionType
	,VT.SettlCurrency_Taxlot AS SettlCurrency
	,VT.TotalExpenses
	,'' AS PBName
From V_TaxLots  VT  
Inner Join T_CompanyFunds CF on CF.CompanyFundID=VT.FundID 
Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 

 UPDATE #VT
SET PBName = CASE 
	
		WHEN CharIndex('G2 Investment Partners LP', AccountName) <> 0
			THEN 'GS'
		WHEN CharIndex('G2 Investment Partners QP LP', AccountName) <> 0
			THEN 'GS'
		WHEN CharIndex('Quantum Partners LP', AccountName) <> 0
			THEN 'MS'
		WHEN CharIndex('MS Investment Partners LP', AccountName) <> 0
			THEN 'MS'
		WHEN CharIndex('MS Investment Partners QP', AccountName) <> 0
			THEN 'MS'
		WHEN CharIndex('G2 Long Only Fund QP LP', AccountName) <> 0
			THEN 'MS'
		ELSE PBName
		END

Select                 

	MAX(VT.TaxlotID) as TradeRefID,
	Max(VT.FundID) as FundID,
	Max(T_CompanyFunds.FundName) As AccountName,
	Max(ISNULL(T_OrderType.OrderTypesID,0))as OrderTypesID,                                                                                                      
	Max(ISNULL(T_OrderType.OrderTypes,'Multiple')) as OrderTypes,                                                                                                  
	T_Side.Side as Side,                                                                                                 
	VT.Symbol,                                                                                                       
	Max(VT.CounterPartyID),               
	T_CounterParty.ShortName as CounterParty ,                                                                               
	CONVERT(Decimal(38,9),Sum(VT.TaxLotQty)) as OrderQty,
	Round(convert(decimal(38,9), VT.AvgPrice),4) as AveragePrice,
	Sum(CONVERT(Decimal(38,9),VT.CumQty)) as CumQty,  
	Sum(CONVERT(Decimal(38,9), VT.Quantity)) as Quantity,
	Max(T_Asset.AssetName) as Asset,               
	Max(T_Underlying.UnderlyingName),              
	Max(T_Exchange.DisplayName) as Exchange,                                                                                                        
	Max(Currency.CurrencyName),      
	Max(Currency.CurrencySymbol) as CurrencySymbol,               
	Max(VT.Level1AllocationID) as Level1AllocationID,                                                  
	CONVERT(Decimal(38,9),Sum(VT.Level2Percentage)) as Level2Percentage,
    CONVERT(Decimal(38,9),Sum(VT.TaxLotQty)) as AllocatedQty,     
	'' as IsBasketGroup,                                                                                                      
	Max(SM.PutOrCall) as PutOrCall,                 
	Max(convert(money, SM.StrikePrice)) as StrikePrice,                                                   
	Max(convert(char(10), SM.ExpirationDate, 101)) as ExpirationDate,
	Max(convert(char(10), VT.SettlementDate, 101)) as SettlementDate,
	convert(money, Sum(VT.Commission)) as CommissionCharged, 
	convert(money, Sum(VT.OtherBrokerFees)) as OtherBrokerFees,
	convert(money, Sum(IsNull(VT.SecFee,0))) As Secfee, 
	Max(VT.GroupRefID),                                                                            
	convert(money, Sum(ISNULL(VT.StampDuty,0))) as StampDuty,
	convert(money, Sum(ISNULL(VT.TransactionLevy,0))) as TransactionLevy, 
	convert(money, Sum(ISNULL(ClearingFee,0))) as ClearingFee,
	convert(money, Sum(ISNULL(TaxOnCommissions,0))) as TaxOnCommissions,
	convert(money, Sum(ISNULL(MiscFees,0))) as MiscFees , 
	Max(convert(char(10),VT.AUECLocalDate, 101)) as TradeDate,
	Max(SM.Multiplier),                                              
	Max(SM.ISINSymbol) as ISINSymbol,                                          
	Max(SM.CUSIPSymbol) as CUSIPSymbol,          
	Max(SM.SEDOLSymbol) as SEDOLSymbol,                               
	Max(SM.ReutersSymbol) as ReutersSymbol,                                          
	Max(SM.BloombergSymbol) as BloombergSymbol,                                          
	Max(SM.CompanyName),                                          
	Max(SM.UnderlyingSymbol) as UnderlyingSymbol,                              
	Max(SM.LeadCurrencyID),                              
	Max(SM.LeadCurrency),                               
	Max(SM.VsCurrencyID),                              
	Max(SM.VsCurrency),              
	Max(SM.OSISymbol),              
	Max(SM.OpraSymbol),              
	Max(SM.IDCOSymbol),
	convert(money, Sum(ISNull(VT.AccruedInterest,0))) as AccruedInterest,
	Convert(money,CASE             
	WHEN Max(VT.CurrencyID) <> Max(T_CompanyFunds.LocalCurrency)            
	THEN CASE    
	WHEN Max(IsNull(VT.FXRate, 0)) <> 0            
	THEN CASE             
	WHEN Max(VT.FXConversionMethodOperator) = 'M'            
	THEN sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0) )           
	WHEN Max(VT.FXConversionMethodOperator) = 'D'            
	AND Max(VT.FXRate) > 0            
	THEN sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / NULLIF(VT.FXRate,0))         
	END 
	ELSE 0          
	END            
	ELSE sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses))
	END)  AS NetNotionalValueBase,
	 
	Convert(money, sum(VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)) AS NetNotionalValue ,
	convert(money, Sum(ISNull(VT.OccFee,0))) as OccFee,
	convert(money, Sum(ISNull(VT.OrfFee,0))) as ORFFees,
	convert(money, Sum(ISNull(VT.ClearingBrokerFee,0))) as ClearingBrokerFee,
	convert(money, Sum(ISNull(VT.SoftCommission,0))) as SoftCommission,
	Max(SettleCurr.CurrencySymbol) As SettlementCurrency,
	CONVERT(Decimal(38,9),Round(Sum(ISNull(VT.FXRate_Taxlot,0)),8)) As TradeFXRate

From #VT VT               
Inner Join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                            
Inner Join T_Side ON dbo.T_Side.SideTagValue = VT.SideID
Inner Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol            
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                 
Inner Join T_Underlying on T_Underlying.UnderlyingID=VT.UnderlyingID               
Inner Join T_CompanyFunds on T_CompanyFunds.CompanyFundID=VT.FundID 
Left Outer Join T_Currency as SettleCurr on SettleCurr.CurrencyID = VT.SettlCurrency                                                                                                      
Left Outer Join T_CounterParty on T_CounterParty.CounterPartyID=VT.CounterPartyID                             
Left Outer Join T_Exchange on T_Exchange.ExchangeID=VT.ExchangeID 
left JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue                                      
--LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID And T_CounterPartyVenue.VenueID=VT.VenueID                                                                                                  

Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 
Group by                                      
 T_Side.Side,                                                                      
 VT.Symbol,                                                                                                       
 T_CounterParty.ShortName,                                                           
 VT.AvgPrice,
 VT.PBName

FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')