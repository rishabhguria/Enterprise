CREATE PROCEDURE P_CheckFactSetUsernameAndSerialNumberDuplication
(
	@userLoginName VARCHAR (50),
	@accessID VARCHAR (200)
)
AS

Select Count(UserID) As Occurance From T_CompanyUser
WHERE [FactSetUsernameAndSerialNumber] = @accessID
AND	[Login] <> @userLoginName