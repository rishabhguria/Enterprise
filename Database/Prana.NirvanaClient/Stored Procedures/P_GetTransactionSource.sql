
CREATE PROC [dbo].[P_GetTransactionSource]     
as  
begin     
SELECT TransactionSourceAcronym,TransactionSourceName FROM T_TransactionSource  
end   

