USE [PrevattCapital]
GO

/****** Object:  StoredProcedure [dbo].[P_FFGetThirdPartyFundsDetails_ExecBrokers_COWN]    Script Date: 12/3/2024 1:26:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*       
EXEC [P_FFGetThirdPartyFundsDetails_ExecBrokers]  '07-30-2019'
Modified By: Prabhat
Modified Date: 7/31/2019
Desc: Settlement currency, Net Notional Value and some fee fields added
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-248  
*/
              
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_ExecBrokers_COWN]                                                                                       
(                                                                                                      
 @inputDate datetime                                                                                                  
)                                                                                                         
AS          
     
 --Declare @inputDate datetime 
 --Set @inputDate = '07-30-2019'


Select                 

	VT.TaxlotID as TradeRefID,
	VT.FundID as FundID,
	T_CompanyFunds.FundName As AccountName,
	ISNULL(T_OrderType.OrderTypesID,0)as OrderTypesID,                                                                                                      
	ISNULL(T_OrderType.OrderTypes,'Multiple') as OrderTypes,                                                                                                      
	VT.OrderSideTagValue as SideID,                                                                                                        
	T_Side.Side as Side,                                                                                                 
	VT.Symbol,                                                                                                       
	VT.CounterPartyID,               
	T_CounterParty.ShortName as CounterParty ,
	VT.VenueID,                                                                                
	CONVERT(Decimal(38,9),Sum(VT.TaxLotQty)) as OrderQty,
	CASE 
    WHEN T_CounterParty.ShortName IN ('COWN', 'ELEV', 'KATZ', 'RDBN') 
   THEN CONVERT(NUMERIC(32,16), VT.AvgPrice)
    ELSE ROUND(CONVERT(decimal(18,8), VT.AvgPrice), 4)
    END AS AveragePrice,
	CONVERT(Decimal(38,9),VT.CumQty) as CumQty,  
	CONVERT(Decimal(38,9), VT.Quantity) as Quantity,
	VT.AUECID,                                                                                                      
	VT.AssetID,             
	T_Asset.AssetName,
	T_Asset.AssetName as Asset,                                                                                                
	VT.UnderlyingID,               
	T_Underlying.UnderlyingName,
	VT.ExchangeID,              
	T_Exchange.DisplayName as Exchange,                                                                                                   
	Currency.CurrencyID,                                                                                                      
	Currency.CurrencyName,              
	Currency.CurrencySymbol,               
	VT.Level1AllocationID as Level1AllocationID,                                                  
	CONVERT(Decimal(38,9),Sum(VT.Level2Percentage)) as Level2Percentage,
	CONVERT(Decimal(38,9),Sum(VT.TaxLotQty)) as AllocatedQty,     
	'' as IsBasketGroup,                                                                                                      
	SM.PutOrCall,                 
	convert(money, SM.StrikePrice) as StrikePrice,                                                   
	convert(char(10), SM.ExpirationDate, 101) as ExpirationDate,
	convert(char(10), VT.SettlementDate, 101) as SettlementDate,
	convert(money, Sum(VT.Commission)) as CommissionCharged, 
	convert(money, Sum(VT.OtherBrokerFees)) as OtherBrokerFees,
	convert(money, Sum(IsNull(VT.SecFee,0))) As Secfee,                    
	ISNULL(T_CounterPartyVenue.DisplayName,'') as CounterPartyVenue,
	VT.GroupRefID,                                                                            
	convert(money, Sum(ISNULL(VT.StampDuty,0))) as StampDuty,
	convert(money, Sum(ISNULL(VT.TransactionLevy,0))) as TransactionLevy, 
	convert(money, Sum(ISNULL(ClearingFee,0))) as ClearingFee,
	convert(money, Sum(ISNULL(TaxOnCommissions,0))) as TaxOnCommissions,
	convert(money, Sum(ISNULL(MiscFees,0))) as MiscFees , 
	convert(char(10),VT.AUECLocalDate, 101) as TradeDate,
	SM.Multiplier,                                              
	SM.ISINSymbol,                                          
	SM.CUSIPSymbol,                                          
	SM.SEDOLSymbol,                                          
	SM.ReutersSymbol,                                          
	SM.BloombergSymbol,                                          
	SM.CompanyName,                                          
	SM.UnderlyingSymbol,                              
	SM.LeadCurrencyID,                              
	SM.LeadCurrency,                               
	SM.VsCurrencyID,                              
	SM.VsCurrency,              
	SM.OSISymbol,              
	SM.OpraSymbol,              
	SM.IDCOSymbol,
	convert(money, Sum(ISNull(VT.AccruedInterest,0))) as AccruedInterest,
	Convert(money,CASE             
	WHEN VT.CurrencyID <> T_CompanyFunds.LocalCurrency            
	THEN CASE    
	WHEN IsNull(VT.FXRate, 0) <> 0            
	THEN CASE             
	WHEN VT.FXConversionMethodOperator = 'M'            
	THEN sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0) )           
	WHEN VT.FXConversionMethodOperator = 'D'            
	AND VT.FXRate > 0            
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
	SettleCurr.CurrencySymbol As SettlementCurrency,
	CONVERT(Decimal(38,9),Round(Sum(ISNull(VT.FXRate_Taxlot,0)),8)) As TradeFXRate,
	ISNull(VT.IsSwapped,0) AS IsSwapped
                     
From V_TaxLots  VT               
Inner Join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                            
Inner Join T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
Inner Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol            
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                 
Inner Join T_Underlying on T_Underlying.UnderlyingID=VT.UnderlyingID               
Inner Join T_CompanyFunds on T_CompanyFunds.CompanyFundID=VT.FundID 
Left Outer Join T_Currency as SettleCurr on SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot                                                                                                      
Left Outer Join T_CounterParty on T_CounterParty.CounterPartyID=VT.CounterPartyID                             
Left Outer Join T_Exchange on T_Exchange.ExchangeID=VT.ExchangeID 
left JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue                                      
LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID And T_CounterPartyVenue.VenueID=VT.VenueID                                                                                                  

Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 
Group by                                                             
 VT.TaxlotID,                                                  
 VT.Level1AllocationID,                                       
 VT.FundID,               
 T_CompanyFunds.FundName,                       
 T_OrderType.OrderTypesID,                                      
 T_OrderType.OrderTypes,                                                                      
 VT.OrderSideTagValue,                                                                     
 T_Side.Side,                                                                      
 VT.Symbol,                                                                                                       
 VT.CounterPartyID,                                                               
 VT.VenueID,                                                                      
 VT.AvgPrice,                                                                                                      
 VT.CumQty,                                                                                                   
 VT.Quantity,                                                                                        
 VT.AUECID,                                                                                                      
 VT.AssetID,                                         
 VT.UnderlyingID,                                                                                                 
 VT.ExchangeID,               
 T_Exchange.DisplayName,                                                                                                  
 Currency.CurrencyID,                                                                                                      
 Currency.CurrencyName,                                                                                                      
 Currency.CurrencySymbol,              
 SM.PutOrCall,                                                   
 SM.StrikePrice,                                                                      
 SM.ExpirationDate,                                                            
 VT.SettlementDate,                                                                            
 T_CounterPartyVenue.DisplayName ,                                           
 VT.GroupRefID,                                                                            
 VT.AUECLocalDate,                                                        
 SM.Multiplier,                                          
 SM.ISINSymbol,                                          
 SM.CUSIPSymbol,                                          
 SM.SEDOLSymbol,                                        
 SM.ReutersSymbol,                                          
 SM.BloombergSymbol,               
 SM.CompanyName,                                          
 SM.UnderlyingSymbol,                              
 SM.LeadCurrencyID,                              
 SM.LeadCurrency,                               
 SM.VsCurrencyID,                              
 SM.VsCurrency,              
T_CounterParty.ShortName,              
T_Asset.AssetName,              
T_CounterParty.ShortName,              
T_Underlying.UnderLyingName,              
 SM.OSISymbol,              
 SM.OpraSymbol,              
 SM.IDCOSymbol,
 VT.FXRate,
 VT.FXConversionMethodOperator,
 T_CompanyFunds.LocalCurrency,
 VT.CurrencyID,
 SettleCurr.CurrencySymbol,
 VT.IsSwapped 

FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')


GO


