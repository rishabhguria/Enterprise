
CREATE Procedure [dbo].[P_GetFKIDForDivUndo]  
(  
 @CorpActionId varchar(200)  
)  
As  
  
Select T_CashDivTransactions.TransactionID from T_CashDivTransactions 
inner join PM_CorpActionTaxlots CATaxlots on CATaxlots.FKId=T_CashDivTransactions.FKId
--inner join T_Journal on CATaxlots.TaxlotId = T_Journal.TaxlotId 
--inner join T_TaxlotCashDividends A on A.DivPKId = CATaxlots.FKId 
where CATaxlots.CorpActionId = @CorpActionId 
  

