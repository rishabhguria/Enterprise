CREATE PROCEDURE [dbo].[P_CheckSapiUsernameDuplication]
(
	@userLoginName VARCHAR (50),
	@accessID VARCHAR (200)
)
AS

Select Count(UserID) As Occurance From T_CompanyUser
WHERE [SapiUsername] = @accessID
AND	[Login] <> @userLoginName