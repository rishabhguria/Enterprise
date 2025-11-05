sp_helptext P_GetThirdparty_GroupBYCustodian_Condire_EOD

--sp_helptext P_GetThirdparty_GroupBYCustodian_Condire_EOD  
  
    
/*           
EXEC [P_FFGetThirdPartyFundsDetails_ExecBrokers_CondirAutomailer]  '05-22-2024'    
Modified By: Prabhat    
Modified Date: 7/31/2019    
Desc: Settlement currency, Net Notional Value and some fee fields added    
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-9255     
*/    
                  
CREATE PROCEDURE [dbo].[P_GetThirdparty_GroupBYCustodian_Condire_EOD]                                                                                           
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
as            
         
 --Declare @inputDate datetime     
 --Set @inputDate = '05-22-2024'    
    
Create Table #Temp_Taxlots    
(    
--AccountName Varchar(200),    
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
 When CF.CompanyFundID In (1,2,3,4,5,6,7,9,10,11,13,14,15,18,20,21,22,24,25,26,27,36,37,38,39,40,41,45,46,47,48,49,50,51,53,55,57,58,59,60,61,62,63,64)       
 Then 'MS'       
 When CF.CompanyFundID In (8,28,32,35,42,43,52,84,85)       
 Then 'Jefferies'       
 When CF.CompanyFundID In (65,66,67,68,69,70,71,72,73,74,75,82,83)      
Then 'CITI'        
Else ''    
 End As Custodian,    
 T_Side.Side as Side,                                                                                                     
 VT.Symbol,       
 T_CounterParty.ShortName as CounterParty ,    
 VT.AvgPrice As AveragePrice,    
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
Replace(Cast((convert(varchar, max(TradeDate), 10)  + Cast(Max(Symbol_PK) As Varchar(100))) As Varchar(200)),'-','') As TradeRefID,    
Custodian,    
Side,    
Symbol,                                                                                                                                                                                                      
max(CounterParty) As CounterParty,    
AveragePrice As AveragePrice ,              
Max(Asset) AS Asset ,                                                 
Max(Exchange) AS Exchange,                                               
Max(CurrencySymbol) As CurrencySymbol ,    
Max(SettlementCurrency) As SettlementCurrency ,    
convert(char(10), MAX(TradeDate), 101) as TradeDate,    
convert(char(10), MAX(SettlementDate), 101) as SettlementDate,    
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
Max(StrikePrice) As StrikePrice,            
convert(char(10), Max(ExpirationDate), 101) as ExpirationDate    
InTo #Temp_Grouped_Taxlots    
From #Temp_Taxlots    
Group By Symbol,Side,Custodian,AveragePrice    
    
--select * from  #Temp_Grouped_Taxlots    
    
--Update T    
--Set T.AveragePrice =     
--Case    
--When T.AllocatedQty <> 0    
--Then (T.AveragePrice/T.AllocatedQty)    
--Else 0    
--End    
--From #Temp_Grouped_Taxlots T    
    
    
--select * from #Temp_Grouped_Taxlots    
Update T    
Set T.AveragePrice =     
Round(AveragePrice,4)    
From #Temp_Grouped_Taxlots T    
    
Select *    
From #Temp_Grouped_Taxlots    
--Where Symbol= 'WEX'    
Order By Symbol    
    
    
--FOR XML Path('ThirdPartyFlatFileDetail'), root('ThirdPartyFlatFileDetailCollection')    
    
Drop Table #Temp_Taxlots,#Temp_Grouped_Taxlots