CREATE PROCEDURE P_CheckActivUsernameDuplication
(
	@userLoginName VARCHAR (50),
	@accessID VARCHAR (200)
)
AS

Select Count(UserID) As Occurance From T_CompanyUser
WHERE [ActivUsername] = @accessID
AND	[Login] <> @userLoginName