/*    
Author   : Rajat tandon    
Date   : Feb 01, 2012    
Description  : Returns the transaction entries for a subaccount between daterange. Eventually would work as a ledger source.    
P_GetSubAccTransactionEntriesForDateRange null, null, null
*/    
CREATE PROCEDURE [dbo].[P_GetSubAccTransactionEntriesForDateRange]    
(    
 @subAccountID int,    
 @startDate datetime,    
 @endDate datetime    
)    
As    
    
Select Journal.TransactionEntryID, Journal.TransactionID, Journal.TransactionDate, Journal.FundID, Funds.FundShortName As FundName, Journal.CurrencyID, Curr.CurrencySymbol,     
SubAcc.TransactionTypeId, AccType.TransactionType, Journal.DR, Journal.CR    
from T_Journal Journal    
INNER JOIN T_SubAccounts SubAcc on Journal.SubAccountID = SubAcc.SubAccountID    
INNER JOIN T_CompanyFunds Funds on Journal.FundID = Funds.CompanyFundID    
INNER JOIN T_Currency Curr on Journal.CurrencyID = Curr.CurrencyID    
INNER JOIN T_TransactionType AccType on SubAcc.TransactionTypeId = AccType.TransactionTypeId    
inner JOIN T_CashPreferences tcpref on tcpref.FundID=Journal.FundID
where Journal.SubAccountID = @subAccountID 
and DateDiff(d,TransactionDate,@startDate) <= 0 
and DateDiff(d,TransactionDate,@endDate) >= 0
and DATEDIFF(d,Journal.TransactionDate,tcpref.CashMgmtStartDate)<=0   
	AND (
		(
			Journal.TransactionSource = 11
			AND tcpref.IsCashSettlementEntriesVisible = 1
			)
		OR Journal.TransactionSource <> 11
		)   
