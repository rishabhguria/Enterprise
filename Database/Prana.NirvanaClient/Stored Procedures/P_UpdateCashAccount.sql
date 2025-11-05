CREATE PROCEDURE P_UpdateCashAccount 
(
    @AccountID int,
    @Name nvarchar(50),
    @Acronym nvarchar(50)
)
AS

UPDATE
  T_Accounts 
SET
  Name = @Name,
  Acronym = @Acronym
WHERE
  AccountID = @AccountID
 
IF @@ROWCOUNT > 0
  -- This statement is used to update the DataSet if changes are done on the updated record (identities, timestamps or triggers )
  SELECT 
	AccountID, Name, Acronym
  FROM 
	T_Accounts
  WHERE 
	AccountID = @AccountID
