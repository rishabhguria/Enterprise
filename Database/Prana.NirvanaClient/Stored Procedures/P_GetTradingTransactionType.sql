
/*
Author: Sandeep Singh
Date: 30 October 2014
Desc: Added Transaction type in the application
*/
CREATE PROC [dbo].[P_GetTradingTransactionType]       
as    
begin       
SELECT TransactionTypeAcronym,TransactionTypeName FROM T_TradingTransactionType    
end

