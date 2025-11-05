
-- Usage : P_GetFillInfo '2010-07-01' 
CREATE PROCEDURE P_GetFillInfo      
(      
	@lowerdate datetime
)      
             
as    
   
select
F.ClOrderID,      
convert(datetime,substring(F.TransactTime,0, 9),103) as TradeDate,
SettlementDate,      
FundShortName as Mnkr,      
senderSubID as Account,      
Case 
	When Sec.AssetId = 2 
		Then  Sec.PutOrCall 
				+ ': ' + Substring(Sec.TickerSymbol,3, CharIndex(' ', Sec.TickerSymbol) - 3)
				+ ': ' + Convert(varchar, Sec.ExpirationDate,1)
				+ ': ' + Cast(Cast(Sec.StrikePrice AS DECIMAL(10,2)) as varchar)
				+ ': ' + IsNull(Sec_Underlying.CompanyName, '')
	Else Sec.TickerSymbol
End As Symbol,      
Side.Side,      
F.LastPx as Price,      
F.LastShares * [dbo].[GetSideMultiplier](Side.SideTagValue)  as ExecutedQty,      
F.CumQty as ExecutedQtyonOrder,      
F.LastPx * F.LastShares *[dbo].[GetSideMultiplier](Side.SideTagValue) as Amount,      
CP.ShortName as Broker,
F.TransactTime as ExecutionTime
      
from T_Order O join T_Sub S on S.ParentClOrderID = O.ParentClOrderID join T_Fills F on F.ClOrderID = S.ClOrderID       
left outer join T_CompanyFunds on S.FundID = T_CompanyFunds.CompanyFundID      
left outer join T_Side Side on F.SideID = Side.SideTagValue      
left outer join T_CounterParty CP on S.CounterPartyID = CP.CounterPartyID     
left outer join V_SecMasterData_WithUnderlying Sec on dbo.Trim(O.Symbol) = Sec.TickerSymbol  
left outer join V_SecMasterData_WithUnderlying Sec_Underlying on Sec.UnderlyingSymbol = Sec_Underlying.TickerSymbol
         
where              
            
S.NirvanaMsgType != 3 -- Exclude Staged Orders              
and datediff(d,convert(datetime,substring(F.TransactTime,0, 9),112),@lowerdate) = 0     
and  F.LastShares!=0       




