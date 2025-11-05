CREATE PROCEDURE P_AddCashAccount
(
    @Name nvarchar(50),
    @Acronym nvarchar(50)
)
AS

INSERT INTO  
  T_Accounts (Name, Acronym)
VALUES 
  (@Name, @Acronym)

SELECT 
    AccountID, Name, Acronym
FROM 
    T_Accounts
WHERE 
    AccountID = SCOPE_IDENTITY()


--  select * from T_Accounts