



/*       
EXEC [P_AutomailerBatch_EMail_Brokers_MapleRock]  '08-22-2023','UBS','kuldeep.kumar@nirvanasolutions.com','sandeep.singh@nirvanasolutions.com',1
 
*/
CREATE PROCEDURE [dbo].[P_AutomailerBatch_EMail_Brokers_MapleRock]
(                                                                                                      
 @inputDate datetime, 
 @CounterPartyName nvarchar(100),
 @ToEmail nvarchar(500),
 @CCEmail nvarchar(500),
 @IsSwapRequired Int                                                                                                  
)                                                                                                         
AS          
     
Set NoCount On

-- Declare @inputDate datetime 
-- Set @inputDate = '08-22-2023'

-- Declare @CounterPartyName nvarchar(max) 
-- Set @CounterPartyName = 'UBS'

-- Declare @ToEmail nvarchar(500)
-- Set @ToEmail='kuldeep.kumar@nirvanasolutions.com'

-- Declare @CCEmail nvarchar(500)
-- Set  @CCEmail='sandeep.singh@nirvanasolutions.com'
--Declare @IsSwapRequired Int

--Set @IsSwapRequired = 1


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
AND VT.IsSwapped = @IsSwapRequired
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

DECLARE @MapleRockInfo NVARCHAR(MAX)
set @MapleRockInfo='Maple Rock''s Info:'

Set @xml = CAST(( SELECT [Trade Date]  AS 'td','',[B/S] AS 'td','', Ticker AS 'td','', [Description] AS 'td','', Quantity AS 'td','', AccountName AS 'td'
From #Temp 
Order By Ticker
FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

SET @body ='<html><body><p>'+'Please see the trade allocation details below and ensure formal trade confirmations are sent to ops@maplerockpartners.com; mrconfirms@merakiglobaladvisors.com. Thank you.</p>
<table border = 1> 
<tr>
<th> Trade Date </th><th> B/S </th><th> Ticker </th><th> Description </th><th> Quantity </th><th> Allocations </th></tr>'

If(@IsSwapRequired = 0)
	Begin

		SET @body = @body +
				   REPLACE(CAST(@xml AS NVARCHAR(MAX)), '<td>', '<td style="padding: 10px;">')+'</table>
				   <p><u><b>NOTE:</u></b> Maple Rock is now live on CTM and ALERT - info below. Kindly submit messages for matching on CTM.
				   <br>
				   </br>
				   Should there be any issues, please reach out to maplerock.mo@apexgroup.com.
				   <br>
				   </br>
				   <br>
				   </br>'+@MapleRockInfo+'
				   <br></br>CTM BIC: MRCTCAT2
				   <br></br>ALERT Acronym: MRCTCAT2
				   </p>
				   </body></html>'
	End

Else

	Begin 

		SET @body = @body +
				   REPLACE(CAST(@xml AS NVARCHAR(MAX)), '<td>', '<td style="padding: 10px;">')+'</table></body></html>'

	End


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


