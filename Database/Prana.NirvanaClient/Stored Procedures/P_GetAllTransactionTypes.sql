CREATE PROC [dbo].[P_GetAllTransactionTypes]    
  
as    
  
SELECT     
  
TransactionTypeId,     
TransactionType as TypeName     
  
FROM T_TransactionType  