create procedure [dbo].[P_GetAllActivityTables]                       
As                    
Begin                      
SELECT [ActivityTypeId],[ActivityType],[Description],[BalanceType],[ActivitySource],[Acronym] FROM T_ActivityType order BY ActivityType       
      
SELECT [AmountTypeId],[AmountType] FROM T_ActivityAmountType order BY AmountType        
     
SELECT [ActivityTypeId_FK],[AmountTypeId_FK],[DebitAccount],[CreditAccount],[CashValueType],[ActivityDateType],[Id],[Description],[IsEnabled] FROM T_ActivityJournalMapping              
    
SELECT [SubAccountID],[Name],[Acronym],[SubCategoryID],[TransactiontypeID],[IsFixedAccount],[SubAccountTypeId] FROM T_SubAccounts order BY Acronym  
  
SELECT [ActivityDateTypeId],[ActivityDateType] FROM T_ActivityDateType    
  
SELECT FundID, LastCalcDate, UpdatedRevaluation FROM T_LastCalcDateRevaluation          
                        
select SAT.SubAccountTypeId,SAT.SubAccountType from T_SubAccountType SAT

select TransactionTypeID, TransactionType from T_TransactionType

End   


