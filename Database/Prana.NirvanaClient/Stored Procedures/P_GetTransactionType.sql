CREATE PROC [dbo].[P_GetTransactionType]   
as
begin   
SELECT TransactionSourceAcronym,TransactionSourceName FROM T_TransactionSource
end 
