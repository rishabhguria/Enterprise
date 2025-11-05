
/*       
EXEC [P_FFGetThirdPartyFundsDetails_ExecBrokers_ACK_AUTOMAILER]  '11-06-2023'
Modified By: Prabhat
Modified Date: 7/31/2019
Desc: Settlement currency, Net Notional Value and some fee fields added
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-248  
*/
              
CREATE PROCEDURE P_FFGetThirdPartyFundsDetails_ExecBrokers_ACK                                                                                    
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
     
 --Declare @inputDate datetime 
 --Set @inputDate = '26-03-2024'

Create Table #Temp_Taxlots
(
AccountName Varchar(200),
Custodian Varchar(100),
Side Varchar(50),                                                                                                 
Symbol Varchar(200),                                                                                                       
CounterParty Varchar(50) ,
AveragePrice Decimal(38,9),
Asset Varchar(50),                                                                                               
Exchange Varchar(50),                                                                                                   
CurrencySymbol Varchar(10),               
AllocatedQty Decimal(38,9),	                
CommissionCharged Float, 
OtherBrokerFees Float,
Secfee Float,                    
StampDuty Float,
TransactionLevy Float, 
ClearingFee Float,
TaxOnCommissions Float,
MiscFees Float, 
TradeDate Date,
SettlementDate Date, 
OccFee Float,
ORFFees  Float,
ClearingBrokerFee  Float,
SoftCommission  Float,
SettlementCurrency Varchar(10),
PutOrCall Varchar(10) , 
Multiplier Decimal(38,9),                                              
ISINSymbol Varchar(50),                                          
CUSIPSymbol Varchar(50),                                          
SEDOLSymbol Varchar(50),                                          
BloombergSymbol Varchar(200),                                          
CompanyName Varchar(500),                                          
UnderlyingSymbol Varchar(50),                              
OSISymbol Varchar(50),              
IDCOSymbol Varchar(50),
ReutersSymbol Varchar(50),
StrikePrice Decimal(38,9),
ExpirationDate Date,
Symbol_PK Varchar(200)
)

Insert InTo #Temp_Taxlots
Select
	Case 
    When CF.CompanyFundID In (3,4)
    Then 'Wells ACK'
    When CF.CompanyFundID In (1,2,9,10)
    Then 'GS ACK'    
    Else CF.FundName
    End As AccountName,
	Case
	When CF.CompanyFundID In (3,4)
	Then 'WFPB'
	When CF.CompanyFundID In (1,2,9,10)
	Then 'GSCO'
	Else ''
	End As Custodian,
	T_Side.Side as Side,                                                                                                 
	VT.Symbol,                                                                                                       
	T_CounterParty.ShortName as CounterParty ,
	(VT.AvgPrice * VT.TaxLotQty) As AveragePrice,
	T_Asset.AssetName as Asset,                                                                                                
	T_Exchange.DisplayName as Exchange,                                                                                                   
	Currency.CurrencySymbol,               
	VT.TaxLotQty as AllocatedQty,	                
	VT.Commission as CommissionCharged, 
	VT.OtherBrokerFees as OtherBrokerFees,
	VT.SecFee As Secfee,                    
	VT.StampDuty as StampDuty,
	VT.TransactionLevy as TransactionLevy, 
	VT.ClearingFee as ClearingFee,
	VT.TaxOnCommissions as TaxOnCommissions,
	VT.MiscFees as MiscFees , 
	VT.AUECLocalDate as TradeDate,
	VT.SettlementDate As SettlementDate, 
	VT.OccFee as OccFee,
	VT.OrfFee as ORFFees,
	VT.ClearingBrokerFee as ClearingBrokerFee,
	VT.SoftCommission As SoftCommission,
	SettleCurr.CurrencySymbol As SettlementCurrency,
	SM.PutOrCall, 
	SM.Multiplier,                                              
	SM.ISINSymbol,                                          
	SM.CUSIPSymbol,                                          
	SM.SEDOLSymbol,                                          
	SM.BloombergSymbol,                                          
	SM.CompanyName,                                          
	SM.UnderlyingSymbol,                              
	SM.OSISymbol,              
	SM.IDCOSymbol,
	SM.ReutersSymbol,
	SM.StrikePrice,
	SM.ExpirationDate,
	SM.Symbol_PK

	
From V_TaxLots  VT               
Inner Join T_Currency as Currency on Currency.CurrencyID = VT.CurrencyID                            
Inner Join T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue
Inner Join V_SecMasterData as SM On SM.TickerSymbol=VT.Symbol            
Inner Join T_Asset on T_Asset.AssetID=VT.AssetID                 
Inner Join T_Underlying on T_Underlying.UnderlyingID=VT.UnderlyingID               
Inner Join T_CompanyFunds CF On CF.CompanyFundID=VT.FundID 
Left Outer Join T_Currency as SettleCurr on SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot                                                                                                      
Left Outer Join T_CounterParty on T_CounterParty.CounterPartyID=VT.CounterPartyID                             
Left Outer Join T_Exchange on T_Exchange.ExchangeID=VT.ExchangeID 

Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
 And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 

Select 
Replace(Cast((convert(varchar, TradeDate, 10)  + Cast(Max(Symbol_PK) As Varchar(100))) As Varchar(200)),'-','') As TradeRefID,
AccountName,
Custodian,
Side,
Symbol,                                                                                                                                                                                                  
CounterParty,
Sum(AveragePrice) As AveragePrice ,          
Asset ,                                             
Exchange,                                           
Max(CurrencySymbol) As CurrencySymbol ,
Max(SettlementCurrency) As SettlementCurrency ,
convert(char(10), TradeDate, 101) as TradeDate,
convert(char(10), SettlementDate, 101) as SettlementDate,
Sum(AllocatedQty) As AllocatedQty,  
convert(money, Sum(CommissionCharged)) As CommissionCharged,
convert(money, Sum(SoftCommission)) As SoftCommission,       
convert(money, Sum(OtherBrokerFees)) As OtherBrokerFees, 
convert(money, Sum(Secfee)) As Secfee,             
convert(money, Sum(StampDuty)) As StampDuty,  
convert(money, Sum(TransactionLevy)) As TransactionLevy,    
convert(money, Sum(ClearingFee)) As ClearingFee,  
convert(money, Sum(TaxOnCommissions)) As TaxOnCommissions,  
convert(money, Sum(MiscFees)) As MiscFees,  
convert(money, Sum(OccFee)) As OccFee,  
convert(money, Sum(ORFFees)) As ORFFees,  
convert(money, Sum(ClearingBrokerFee)) As ClearingBrokerFee,  
Max(Multiplier) As Multiplier  ,           
Max(ISINSymbol) As ISINSymbol,                                       
Max(CUSIPSymbol) As CUSIPSymbol,                                        
Max(SEDOLSymbol) As SEDOLSymbol ,                                       
Max(BloombergSymbol) As BloombergSymbol,
Max(CompanyName) As CompanyName,                                                                                                                                                                                                                               

                   
Max(UnderlyingSymbol) As UnderlyingSymbol,
Max(OSISymbol) As OSISymbol,                                       
Max(IDCOSymbol) As IDCOSymbol ,                                        
Max(ReutersSymbol) As ReutersSymbol ,  
Max(PutOrCall) As PutOrCall ,                                   
StrikePrice As StrikePrice,        
convert(char(10), Max(ExpirationDate), 101) as ExpirationDate
InTo #Temp_Grouped_Taxlots
From #Temp_Taxlots
Group By AccountName, Custodian,Symbol, TradeDate,SettlementDate,Side, Asset,CounterParty, Exchange,StrikePrice


Update T
Set T.AveragePrice = 
Case
When T.AllocatedQty <> 0
Then (T.AveragePrice/T.AllocatedQty)
Else 0
End
From #Temp_Grouped_Taxlots T

Update T
Set T.AveragePrice = 
Round(AveragePrice,4)
From #Temp_Grouped_Taxlots T

Select *
From #Temp_Grouped_Taxlots
--Where CounterParty= 'CJSC'
Order By AccountName,Symbol


Drop Table #Temp_Taxlots,#Temp_Grouped_Taxlots