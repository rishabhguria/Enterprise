CREATE PROCEDURE dbo.P_DeleteRMTradingAccnt
	
	(
	@companyID int,
	@tradAccntID int
	)
	
AS
Begin Tran

DELETE FROM T_RMUserTradingAccount
WHERE        (CompanyID = @companyID) AND (UserTradingAccntID = @tradAccntID)

	DELETE FROM T_RMCompanyTradAccntOverall
	WHERE        (CompanyID = @companyID) AND (CompanyTradAccntID = @tradAccntID)

if @@error <> 0
begin
	
	rollback tran
end
else
begin 
	
	commit tran
end

select @@rowcount
