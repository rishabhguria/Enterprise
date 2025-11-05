
CREATE PROC [dbo].[P_IfCashSubAccountInUse]
(
@subAccountID int
)

as 

--select top(1) (
-- exists(SELECT SubAccountID FROM T_Journal WHERE SubAccountID = @subAccountID) AND
-- exists (SELECT SubAccountID FROM T_SubAccounts WHERE SubAccountID = @subAccountID and IsFixedAccount='True'))
-- 

IF     EXISTS(SELECT 1 FROM T_Journal WHERE SubAccountID = @subAccountID)
    OR EXISTS(SELECT 1 FROM T_SubAccounts WHERE SubAccountID = @subAccountID and IsFixedAccount='True')
	OR EXISTS(SELECT 1 FROM T_ActivityJournalMapping WHERE DebitAccount = @subAccountID)
	OR EXISTS(SELECT 1 FROM T_ActivityJournalMapping WHERE CreditAccount = @subAccountID)
BEGIN
select 1
END

