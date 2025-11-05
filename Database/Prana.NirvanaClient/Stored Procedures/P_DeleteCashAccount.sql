CREATE PROCEDURE P_DeleteCashAccount
(
	@AccountID int
)
AS

DELETE 
	T_Accounts 
WHERE 
	AccountID = @AccountID
