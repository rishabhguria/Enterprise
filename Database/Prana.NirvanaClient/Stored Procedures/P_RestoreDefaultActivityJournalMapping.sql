          
/*                                                
Author: Narendra Kumar Jangir                                                
Date: July 23,2013                                                
Desc: This SP restores default activity journal mapping                                           
EXEC:                                                
P_RestoreDefaultActivityJournalMapping                                              
*/                                                
                                                
CREATE Procedure [dbo].[P_RestoreDefaultActivityJournalMapping] 
As
IF Exists(select name FROM sys.tables where Name = N'T_ActivityJournalMapping')
Begin
delete T_ActivityJournalMapping
insert into T_ActivityJournalMapping
(
	ActivityTypeId_FK,
	AmountTypeId_FK,
	DebitAccount,
	CreditAccount
)
SELECT
activity.ActivityTypeId,
amt.AmountTypeId,
sub.SubAccountID,
sub1.SubAccountID
FROM T_DefaultActivityJournalMapping map
left JOIN  T_ActivityType activity
on activity.ActivityType = map.ActivityType
left JOIN  T_ActivityAmountType amt
on amt.AmountType = map.AmountType
left JOIN  T_SubAccounts sub
on sub.Acronym = map.DebitAccount
left JOIN  T_SubAccounts sub1
on sub1.Acronym = map.CreditAccount
End
