
/*       
EXEC [P_GetThirdPartyDetails_Automailer_Chimera]  '03-08-2022'
DESC: Customized SP for Chimera Automailer
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-3657
*/
              
CREATE PROCEDURE [dbo].[P_GetThirdPartyDetails_Automailer_Chimera]                                                                                       
(                                                                                                      
 @inputDate datetime                                                                                                  
)                                                                                                         
AS          
     
 --Declare @inputDate datetime 
 --Set @inputDate = '03-08-2022'

Create Table #Temp_Taxlots
(
TradeRefID Varchar(50),
AccountName Varchar(200),
Side Varchar(50),
Symbol Varchar(200),
CounterParty Varchar(50),
AveragePrice Float,
Asset Varchar(50),
UnderlyingName Varchar(100),
Exchange Varchar(50),
CurrencySymbol Varchar(10),
AllocatedQty Float,
PutOrCall Varchar(20),
StrikePrice Float,
ExpirationDate DateTime,
SettlementDate DateTime,
CommissionCharged Float,
OtherBrokerFees Float,
Secfee Float,
StampDuty Float,
TransactionLevy Float,
ClearingFee Float,
TaxOnCommissions Float,
MiscFees Float,
TradeDate DateTime,
Multiplier Float,
ISINSymbol Varchar(50),
CUSIPSymbol Varchar(50),
SEDOLSymbol Varchar(50),
BloombergSymbol Varchar(50),
CompanyName Varchar(500),
UnderlyingSymbol Varchar(50),
OSISymbol Varchar(50),
IDCOSymbol Varchar(50),
NetNotionalValueBase Float,
NetNotionalValue Float,
OccFee Float,
ORFFees Float,
ClearingBrokerFee Float,
SoftCommission Float,
SettlementCurrency Varchar(10),
TradeFXRate Float
)

InSert InTo #Temp_Taxlots
Select                 

	VT.TaxlotID as TradeRefID,
	Case
		When T_CounterParty.ShortName = 'MS'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '020A39HI5'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '020A3MJ52'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '020A3X7Z5'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'BARC'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '93698606'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '95713487'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '93602872'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'COWN'
		Then
			Case
			    When T_CompanyFunds.FundName = 'Atom'
				Then '1BRK02001272300045'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then 'CHIMCWF C01272300067'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then 'CHIMGSCO'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'WEDB'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '1WC15605'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '1WC15340'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '1WC15341'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'JEFC'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '490-17572'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '2ZD14019'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '390-99874'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'RAJA'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '6906L350'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '668U0548'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '868R0300'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'CS'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '25NYM0'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '2400H0'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '255320'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'CHLM'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then 'D6Y017885'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then 'D6Y000374'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then 'D6Y017319'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'KEYB'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then 'RF3258652'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then 'RF3206818'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then 'RF3232145'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'TELS'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '55TC6930'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '55TC1058'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discre
tionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '55TC6497'
				
			Else T_CompanyFunds.FundName
			End
		When T_CounterParty.ShortName = 'RAJA_RJ'
		Then
			Case
				When T_CompanyFunds.FundName = 'Atom'
				Then '6906L350'
				When T_CompanyFunds.FundName In ('Booth Bay SMA', 'Booth Bay DA')
				Then '668U0548'
				When T_CompanyFunds.FundName In ('Chimera Capital','Chimera Systematic','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore','MIO Onshore','Walleye WMO','Walleye WMO Systematic','Walleye Concentrated','WOF Consumer TMT Discret
ionary','WOF Consumer TMT Systematic', 'Walleye WOF','Walleye WOF Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
				Then '868X3835'
				
			Else T_CompanyFunds.FundName
			End
		Else T_CompanyFunds.FundName
	End As AccountName,
	T_Side.Side as Side,                                                                                                 
	VT.Symbol,                                                         
	T_CounterParty.ShortName as CounterParty ,
	VT.AvgPrice as AveragePrice,
	T_Asset.AssetName as Asset,                                                                                                
	T_Underlying.UnderlyingName,
	T_Exchange.DisplayName as Exchange,                                                                                                   
	Currency.CurrencySymbol,               
	VT.TaxLotQty as AllocatedQty,     
	SM.PutOrCall As PutOrCall,                 
	SM.StrikePrice As StrikePrice,                                                   
	SM.ExpirationDate as ExpirationDate,
	VT.SettlementDate as SettlementDate,
	VT.Commission as CommissionCharged, 
	VT.OtherBrokerFees as OtherBrokerFees,
	VT.SecFee As Secfee,                    
	VT.StampDuty as StampDuty,
	VT.TransactionLevy as TransactionLevy, 
	ClearingFee as ClearingFee,
	TaxOnCommissions as TaxOnCommissions,
	 MiscFees as MiscFees , 
	VT.AUECLocalDate as TradeDate,
	SM.Multiplier As Multiplier,                                              
	SM.ISINSymbol As ISINSymbol,                                          
	SM.CUSIPSymbol As CUSIPSymbol,                                          
	SM.SEDOLSymbol As SEDOLSymbol,                                          
	SM.BloombergSymbol As BloombergSymbol,                                          
	SM.CompanyName As CompanyName,                                          
	SM.UnderlyingSymbol As UnderlyingSymbol,                              
	SM.OSISymbol As OSISymbol,              
	SM.IDCOSymbol As IDCOSymbol,
		CASE             
		WHEN VT.CurrencyID <> T_CompanyFunds.LocalCurrency            
		THEN 
			CASE    
			WHEN IsNull(VT.FXRate, 0) <> 0            
			THEN 
				CASE             
					WHEN VT.FXConversionMethodOperator = 'M'            
					THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0)            
					WHEN VT.FXConversionMethodOperator = 'D' AND VT.FXRate > 0            
					THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / NULLIF(VT.FXRate,0)         
				END 
			ELSE 0          
			END            
		ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)
	END  AS NetNotionalValueBase,
	 
	(VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) AS NetNotionalValue ,
	 VT.OccFee as OccFee,
	VT.OrfFee as ORFFees,
	VT.ClearingBrokerFee as ClearingBrokerFee,
	VT.SoftCommission as SoftCommission,
	SettleCurr.CurrencySymbol As SettlementCurrency,
	VT.FXRate_Taxlot As TradeFXRate
                   
From V_TaxLots  VT               
Inner Join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                            
Inner Join T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
Inner Join V_SecMasterData as SM On SM.TickerSymbol = VT.Symbol            
Inner Join T_Asset on T_Asset.AssetID = VT.AssetID                 
Inner Join T_Underlying on T_Underlying.UnderlyingID=VT.UnderlyingID      
Inner Join T_CompanyFunds on T_CompanyFunds.CompanyFundID=VT.FundID
Inner Join T_CounterParty on T_CounterParty.CounterPartyID = VT.CounterPartyID                             
Inner Join T_Exchange on T_Exchange.ExchangeID = VT.ExchangeID  
Left Outer Join T_Currency as SettleCurr on SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot                                                                                                      

Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType Not IN ('Assignment', 'Exercise','Expire')
 --And T_CounterParty.ShortName ='BARC'
 --And FundName In ('Chimera Capital','Booth Bay SMA', 'Booth Bay DA','Diamond Growth Fund','Diamond Neutral Fund','Stevens Capital','MIO Offshore',
 --'MIO Onshore','Walleye WIF', 'Walleye WOF', 'Walleye Concentrated')
 ----And T_CounterParty.ShortName = 'BARC'

--FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')

Select 
Max(TradeRefID) As TradeRefID,
AccountName,
Side ,
Symbol ,
CounterParty ,
Sum(AveragePrice * AllocatedQty) As AveragePrice,
Asset ,
UnderlyingName ,
Exchange ,
Max(CurrencySymbol) As CurrencySymbol ,
Sum(AllocatedQty) As AllocatedQty ,
Max(PutOrCall) As PutOrCall ,
Max(StrikePrice) As StrikePrice ,
Max(ExpirationDate) As ExpirationDate ,
Max(SettlementDate) As SettlementDate ,
Sum(CommissionCharged) As CommissionCharged ,
Sum(OtherBrokerFees) As OtherBrokerFees ,
Sum(Secfee) As Secfee ,
Sum(StampDuty) As StampDuty ,
Sum(TransactionLevy) As TransactionLevy ,
Sum(ClearingFee) As ClearingFee ,
Sum(TaxOnCommissions) As TaxOnCommissions ,
Sum(MiscFees) As MiscFees ,
Max(TradeDate) As TradeDate ,
Max(Multiplier) As Multiplier ,
Max(ISINSymbol) As ISINSymbol ,
Max(CUSIPSymbol) As CUSIPSymbol ,
Max(SEDOLSymbol) As SEDOLSymbol ,
Max(BloombergSymbol) As BloombergSymbol ,
Max(CompanyName) As CompanyName ,
Max(UnderlyingSymbol) As UnderlyingSymbol ,
Max(OSISymbol) As OSISymbol ,
Max(IDCOSymbol) As IDCOSymbol ,
Sum(NetNotionalValueBase) As NetNotionalValueBase ,
Sum(NetNotionalValue) As NetNotionalValue ,
Sum(OccFee) As OccFee ,
Sum(ORFFees) As ORFFees ,
Sum(ClearingBrokerFee) As ClearingBrokerFee ,
Sum(SoftCommission) As SoftCommission,
Max(SettlementCurrency) As SettlementCurrency ,
Max(TradeFXRate) As TradeFXRate 

InTo #Temp_GroupedData
From #Temp_Taxlots
Group By AccountName,Asset, Symbol,Side,CounterParty,Exchange,UnderlyingName

Update #Temp_GroupedData
Set 
AveragePrice = 
Case
When AllocatedQty <> 0
Then AveragePrice/AllocatedQty
Else 0
End 

Select 
TradeRefID As TradeRefID,
AccountName,
Side ,
Symbol ,
CounterParty ,
Round(CONVERT(Decimal(18,8), AveragePrice),4) As AveragePrice,
Asset ,
UnderlyingName ,
Exchange ,
CurrencySymbol As CurrencySymbol ,
CONVERT(Decimal(38,9),AllocatedQty) as AllocatedQty,
PutOrCall,
CONVERT(money, StrikePrice) As StrikePrice, 
CONVERT(char(10), ExpirationDate, 101) as ExpirationDate,
CONVERT(char(10), SettlementDate, 101) as SettlementDate,
CONVERT(money, CommissionCharged) as CommissionCharged, 
CONVERT(money, OtherBrokerFees) as OtherBrokerFees,
CONVERT(money, Secfee) As Secfee,                    
CONVERT(money, StampDuty) as StampDuty,
CONVERT(money, TransactionLevy) as TransactionLevy, 
CONVERT(money, ClearingFee) as ClearingFee,
CONVERT(money, TaxOnCommissions) as TaxOnCommissions,
CONVERT(money, MiscFees) as MiscFees , 
CONVERT(char(10),TradeDate, 101) as TradeDate,
Multiplier As Multiplier ,
ISINSymbol As ISINSymbol ,
CUSIPSymbol As CUSIPSymbol ,
SEDOLSymbol As SEDOLSymbol ,
BloombergSymbol As BloombergSymbol ,
CompanyName As CompanyName ,
UnderlyingSymbol As UnderlyingSymbol ,
OSISymbol As OSISymbol ,
IDCOSymbol As IDCOSymbol ,
Convert(money,NetNotionalValueBase) As NetNotionalValueBase ,
Convert(money,NetNotionalValue) As NetNotionalValue ,
Convert(money,OccFee) As OccFee ,
Convert(money,ORFFees) As ORFFees ,
Convert(money,ClearingBrokerFee) As ClearingBrokerFee ,
Convert(money,SoftCommission) As SoftCommission,
SettlementCurrency As SettlementCurrency ,
CONVERT(Decimal(38,9),TradeFXRate) As TradeFXRate 

From #Temp_GroupedData
Order By AccountName,Symbol,Side
FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')

Drop Table #Temp_Taxlots,#Temp_GroupedData