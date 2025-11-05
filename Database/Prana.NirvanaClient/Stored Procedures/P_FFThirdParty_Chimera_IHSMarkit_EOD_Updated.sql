
Create PROCEDURE [dbo].[P_FFThirdParty_Chimera_IHSMarkit_EOD_Updated] 
 (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS

--Declare @InputDate DateTime        
--Set @InputDate = '11-21-2022'    
    
--Declare @CompanyFundIDs varchar(max)     
--Set @companyFundIDs = '1' 

Declare @Fund Table                                                               
(                    
FundID int,
FundName Varchar(100)
)  

Insert Into @Fund  
Select CompanyFundID, FundName
From T_CompanyFunds 
Where FundName In ('WOF Consumer TMT Discretionary','WOF Consumer TMT Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic',
 'Swap - Walleye WOF TMT Discretionary','Swap - Walleye WMO TMT Discretionary')
 
Select 
SM.TickerSymbol,
SM.AssetId,
SM.CurrencyID,
SM.ISINSymbol As ISINSymbol,                                          
SM.CUSIPSymbol As CUSIPSymbol,                                          
SM.SEDOLSymbol As SEDOLSymbol,                                          
SM.BloombergSymbol As BloombergSymbol,
SM.ReutersSymbol,                                          
SM.CompanyName As CompanyName,
SM.Multiplier, 
UDA.Symbol_PK,
UDA.CustomUDA1,
UDA.CustomUDA2,
UDA.CustomUDA3,
UDA.CustomUDA4
InTo #Temp_SM
From  V_SecMasterData SM 
Inner Join V_UDA_DynamicUDA UDA On SM.Symbol_PK = UDA.Symbol_PK

SELECT PT.Taxlot_PK        
InTo #TempTaxlotPK             
FROM PM_Taxlots PT              
Inner Join @Fund Fund on Fund.FundID = PT.FundID                  
Where PT.Taxlot_PK in                                           
(                                                                                                    
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                       
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                          
 group by TaxlotId                             
)                                                                                              
And PT.TaxLotOpenQty > 0

Select
'Chimera Capital Management' As AccountName,    
PT.Symbol,
T_Side.Side as Side,
T_CounterParty.ShortName as CounterParty,     
SM.ISINSymbol As ISINSymbol,                                          
SM.CUSIPSymbol As CUSIPSymbol,                                          
SM.SEDOLSymbol As SEDOLSymbol,                                          
SM.BloombergSymbol As BloombergSymbol,
SM.ReutersSymbol As RIC,                                          
SM.CompanyName As CompanyName,                      
Curr.CurrencySymbol,    
PT.TaxlotOpenQty As OpenPosition,   
CONVERT(VARCHAR(10), PT.AUECModifiedDate, 101) AS TradeDate,
IsNull((PT.TaxlotOpenQty * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,   
Case
		When dbo.GetSideMultiplier(PT.OrderSideTagValue)=1
		Then 'Long'
		Else 'Short'
End As LongShort,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,

Case
	When G.IsSwapped = 1
	Then SM.CustomUDA1
Else '' 
End As CustomUDA1,
Case
	When G.IsSwapped = 1
	Then SM.CustomUDA2
Else ''
End As CustomUDA2,
Case
	When G.IsSwapped = 1
	Then SM.CustomUDA3
Else ''
End As CustomUDA3,
Case
	When G.IsSwapped = 1
	Then SM.CustomUDA4
Else ''
End As CustomUDA4,
TC.CurrencySymbol AS Settlcurrency,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,
Case
	When G.IsSwapped = 1
	Then 'EquitySwap'
Else A.AssetName
End As Asset

Into #TempOpenPositionsTable             
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join @Fund CF On CF.FundID = PT.FundID    
Inner Join #Temp_SM SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Group G ON G.GroupID = PT.GroupID
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID 
Inner JOIN T_Currency AS TC ON TC.CurrencyID = PT.SettlCurrency    
Inner Join T_CounterParty on T_CounterParty.CounterPartyID = G.CounterPartyID
Inner Join T_Side ON dbo.T_Side.SideTagValue = PT.OrderSideTagValue
Inner Join T_Asset A On A.AssetID = SM.AssetID
  
LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (      
   MP.Symbol = PT.Symbol      
   AND DateDiff(d, MP.[DATE], @InputDate) = 0      
   AND MP.FundID = PT.FundID      
   )      
 LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (      
   MP1.Symbol = PT.Symbol      
   AND DateDiff(d, MP1.[DATE], @InputDate) = 0      
   AND MP1.FundID = 0      
   )     
Where PT.TaxlotOpenQty > 0 
And A.AssetID=1

--Select * From #TempOpenPositionsTable

Select
AccountName ,
Max(Side) As Side ,
Max(CounterParty) As CounterParty ,
Symbol ,
Max(CurrencySymbol) As CurrencySymbol ,
Sum(OpenPosition * SideMultiplier) As OpenPosition ,
CONVERT(VARCHAR(10), @InputDate, 120) As TradeDate ,
Max(ISINSymbol) As ISINSymbol ,
Max(CUSIPSymbol) As CUSIPSymbol ,
Max(SEDOLSymbol) As SEDOLSymbol ,
Max(BloombergSymbol) As BloombergSymbol ,
Max(RIC) As RIC ,
Max(CompanyName) As CompanyName ,
Sum(MarketValue) As MarketValue ,
Max(LongShort) As LongShort,

Max(SideMultiplier) As SideMultiplier,
Max(CustomUDA1) As CustomUDA1,
Max(CustomUDA2) As CustomUDA2,
Max(CustomUDA3) As CustomUDA3,
Max(CustomUDA4) As CustomUDA4,
Max(Settlcurrency) As Settlcurrency,
Sum (TotalCost_Local) As NetAmount,
Temp.Asset

InTo #Temp_GroupedData
From #TempOpenPositionsTable as Temp
Group By Temp.Asset,Temp.Symbol, Temp.AccountName

select * from #Temp_GroupedData


Drop Table #TempOpenPositionsTable,#Temp_GroupedData,#TempTaxlotPK, #Temp_SM


