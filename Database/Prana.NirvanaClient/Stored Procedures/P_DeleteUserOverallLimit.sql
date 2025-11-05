CREATE PROCEDURE dbo.P_DeleteUserOverallLimit 
	
	(
		@companyID int,
		@userID int
	)
	
AS

Begin Tran

DELETE FROM T_RMCompanyUserUI
WHERE        (CompanyID = @companyID) AND (CompanyUserID = @userID)

	DELETE FROM T_RMCompanyUsersOverall
	WHERE        (CompanyID = @companyID) AND (CompanyUserID = @userID)
	
	if @@error <> 0
begin
	
	rollback tran
end
else
begin 
	
	commit tran
end

select @@rowcount