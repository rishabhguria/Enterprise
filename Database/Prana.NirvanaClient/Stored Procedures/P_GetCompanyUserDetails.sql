
CREATE PROCEDURE [dbo].[P_GetCompanyUserDetails] 
(
@userID int
)
AS
	select UserID,LastName,FirstName,EMail,Login,Password,CompanyID,Region,RoleID,IsAllGroupsAccess,ShortName 
    from T_CompanyUser where UserID = @userID

