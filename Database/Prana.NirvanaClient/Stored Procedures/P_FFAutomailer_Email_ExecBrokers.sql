
/*       
EXEC [P_FFAutomailer_Email_ExecBrokers]  '07-30-2019'
 
*/CREATE PROCEDURE [dbo].[P_FFAutomailer_Email_ExecBrokers]                                                                                       
(                                                                                                      
 @inputDate datetime, 
 @CounterPartyName nvarchar(100),
 @ToEmail nvarchar(500),
 @CCEmail nvarchar(500)
                                                                                                  
)                                                                                                         
AS          
     
 --Declare @inputDate datetime 
 --Set @inputDate = '05-15-2023'

 --Declare @CounterPartyName nvarchar(max) 
 --Set @CounterPartyName = 'ZCITI'


Select
	T_CompanyFunds.FundName As AccountName,
	 CASE
	   WHEN T_Side.Side = 'Buy' or T_Side.Side ='Buy to Open'
				THEN 'B'
			WHEN T_Side.Side = 'Buy to Cover' or T_Side.Side ='Buy to Close'
				THEN 'CB'
			WHEN T_Side.Side = 'Sell' or T_Side.Side ='Sell to Close'
				THEN 'S'
			WHEN T_Side.Side = 'Sell short' or T_Side.Side ='Sell to Open'
				THEN 'SS'			
		END AS [B/S],
	                                                                                                 
	VT.Symbol AS Ticker, 	              
	T_CounterParty.ShortName AS CounterParty, 	 
	Format(CONVERT(Decimal(38,0),Sum(VT.TaxLotQty)),'#,###') AS Quantity,
	convert(VARCHAR(10),Cast(VT.AUECLocalDate As Date), 1) AS [Trade Date],                        
	SM.CompanyName AS [Description]                                        
	
INTO #Temp                   
From V_TaxLots VT With (NoLock)         
Inner Join T_Currency Currency With (NoLock) on Currency.CurrencyID = VT.CurrencyID                            
Inner Join T_Side With (NoLock) ON T_Side.SideTagValue = VT.OrderSideTagValue
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol=VT.Symbol 
Inner Join T_CompanyFunds With (NoLock) on T_CompanyFunds.CompanyFundID=VT.FundID                                                                                                     
Inner Join T_CounterParty With (NoLock) on T_CounterParty.CounterPartyID=VT.CounterPartyID                            
 
Where DateDiff(d,VT.AUECLocalDate,@inputDate) = 0
AND T_CounterParty.ShortName =@CounterPartyName 
And VT.TransactionType <> 'Assignment' And VT.TransactionType <> 'Exercise' and VT.TransactionType <> 'Expire' 
Group by                                                                               
 Cast(VT.AUECLocalDate As Date),             
 T_CompanyFunds.FundName,                                                           
 T_Side.Side,                                                                      
 VT.Symbol,
 T_CounterParty.ShortName,                                                                                    
 SM.CompanyName    

Declare @SymbolCount int
Select @SymbolCount=(Select Count(*) from #Temp)


IF(@SymbolCount<>0)
BEGIN

DECLARE @xml NVARCHAR(MAX)
DECLARE @body NVARCHAR(MAX)
DECLARE @subject NVARCHAR(MAX)

DECLARE @s VARCHAR(max)

Set @xml = CAST(( SELECT [Trade Date]  AS 'td','',[B/S] AS 'td','', Ticker AS 'td','', [Description] AS 'td','', Quantity AS 'td','', AccountName AS 'td'
From #Temp 
Order By Ticker
FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))


SET @body ='<html><body><p>'+'Please see the trade allocation details below and ensure formal trade confirmations are sent to ops@maplerockpartners.com; mrconfirms@merakiglobaladvisors.com. Thank you.</p>
<table border = 1> 
<tr>
<th> Trade Date </th><th> B/S </th><th> Ticker </th><th> Description </th><th> Quantity </th><th> Allocations </th></tr>'  


SET @body = @body +
           REPLACE(CAST(@xml AS NVARCHAR(MAX)), '<td>', '<td style="padding: 10px;">')+'</table></body></html>'


SET @s = CONVERT(VARCHAR(12),@inputdate,1)+': Maple Rock Trade Allocations'

EXEC msdb.dbo.sp_send_dbmail
@profile_name = 'Nirvana Alerts',
@body = @body,
@body_format ='HTML',
@recipients = @TOemail,
@copy_recipients =@CCEmail,
@subject = @s;

END;
 
Drop table #Temp