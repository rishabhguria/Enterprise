/*
Created by : Kuldeep Agrawal
Created On: 01 June 2016
Purpose: Sometimes we need to fetch some data from SM DB and for that we are dependent on V_SecMasterData which takes much time as it 
fully executes each time and then return the result.
So here to query SM DB, we just need to provide this SP a prepared query statement with SM DB name as "SMDBToUpdate"
Usage:- To get multiplier of AAPL and MSFT symbols, use this

CREATE table #Symbols
(
Symbol varchar(100),
Multiplier float
)

Declare @query varchar(500)
set @query = 'Select TickerSymbol,Multiplier from [DatabaseToUpdate].dbo.T_Smsymbollookuptable SM INNER JOIN
[SMDBToUpdate].dbo.T_SMEquityNonHistoryData SME ON SM.SYmbol_PK = SME.Symbol_PK  Where TickerSymbol in(''AAPL'', ''MSFT'')'
INSERT into #Symbols
EXEC P_ExecuteQueryOnSMDB @query

SELECT * from #Symbols
Drop TABLE #Symbols
*/
CREATE Proc P_ExecuteQueryOnSMDB  
@Query nvarchar(max)  
As  
  
Declare @SMDBName varchar(50)  
SET @SMDBName = '$(SecurityMaster)'  
  
Set @Query = REPLACE(@Query,'SMDBToUpdate',@SMDBName)  
  
execute(@Query)  

