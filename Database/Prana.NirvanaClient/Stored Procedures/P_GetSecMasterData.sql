/*
Created By: Kuldeep Agrawal
Creation Date: 02 Feb 2016
Purpose:- There was only views that were giving SM information of a symbol, so in case of repetitive need, calling view every time was 
heavy operation.
Thus we created an SP which will take comma separated list of symbols to fetch SM information.
Usage:- exec P_GetSecMasterData 'O:SPX 12M1040.00,MSFT,DX H9,ES G1C1090,HKD-JPY,BOM BI 28NOV2015,TestEquity,USD-KRW 01Dec2011'
*/
CREATE Procedure [dbo].[P_GetSecMasterData]
(
@Symbols VARCHAR(max)
)

AS

Create table #Symbols (Symbol nvarchar(max))        

Insert into #Symbols
Select Items as Symbol
from dbo.Split(@Symbols,',')

SELECT Distinct       
SM.TickerSymbol,     
SM.AuecID,
SM.CurrencyID,
COALESCE(NonHistoryData.Multiplier, OptionData.Multiplier, FutureData.Multiplier, FxData.Multiplier, FxForwardData.Multiplier, FixedIncomeData.Multiplier,0) AS Multiplier,
COALESCE(FxData.LeadCurrencyID, FxForwardData.LeadCurrencyID, 0) AS LeadCurrencyID,
COALESCE(FxData.VsCurrencyID, FxForwardData.VsCurrencyID, 0) AS VsCurrencyID
FROM 
[$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM 
INNER JOIN #Symbols S ON SM.TickerSymbol = S.Symbol
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMReuters AS Reuters ON Reuters.Symbol_PK = SM.Symbol_PK AND Reuters.ISPrimaryExchange = 'true'  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData AS NonHistoryData ON NonHistoryData.Symbol_PK = SM.Symbol_PK  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMOptionData AS OptionData ON OptionData.Symbol_PK = SM.Symbol_PK  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFutureData AS FutureData ON FutureData.Symbol_PK = SM.Symbol_PK     
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxData AS FxData ON FxData.Symbol_PK = SM.Symbol_PK     
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData AS FxForwardData ON FxForwarddata.Symbol_PK = SM.Symbol_PK  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData AS FixedIncomeData ON FixedIncomeData.Symbol_PK = SM.Symbol_PK  


Drop table #Symbols





