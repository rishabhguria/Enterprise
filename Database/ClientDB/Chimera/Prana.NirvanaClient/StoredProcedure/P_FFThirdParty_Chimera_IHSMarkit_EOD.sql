

CREATE PROCEDURE [dbo].[P_FFThirdParty_Chimera_IHSMarkit_EOD] 
 (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS

--Declare @InputDate DateTime        
--Set @InputDate = '06-07-2022'    
    
--Declare @CompanyFundIDs varchar(max)     
--Set @companyFundIDs = '1' 

Declare @Fund Table                                                               
(                    
FundID int
)  
    
Insert into @Fund                                                                                                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')

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
Case
 When CF.FundName In ('WOF Consumer TMT Discretionary','WOF Consumer TMT Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')    
 Then 'Chimera Capital Management'
 Else CF.FundName
End As AccountName,    
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
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier

Into #TempOpenPositionsTable             
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol    
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID      
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID   
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
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
And CF.FundName In ('WOF Consumer TMT Discretionary','WOF Consumer TMT Systematic','WMO Consumer TMT Discretionary','WMO Consumer TMT Systematic')
And A.AssetID=1

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
Max(LongShort) As LongShort

InTo #Temp_GroupedData
From #TempOpenPositionsTable as Temp
Group By Temp.AccountName,Temp.Symbol

select * from #Temp_GroupedData

Drop Table #TempOpenPositionsTable,#Temp_GroupedData,#TempTaxlotPK


