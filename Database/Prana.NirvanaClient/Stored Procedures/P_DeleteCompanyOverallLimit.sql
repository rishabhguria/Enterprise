CREATE PROCEDURE dbo.P_DeleteCompanyOverallLimit
	
	(
	 @companyID int 
	)
	
AS

Begin Tran

DELETE FROM T_RMCompanyClientOverall
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMCompanyFundAccntOverall
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMAUECs
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMCompanyUserUI
WHERE        (CompanyID = @companyID) 

DELETE FROM T_RMCompanyUsersOverall
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMUserTradingAccount
WHERE        (CompanyID = @companyID) 

DELETE FROM T_RMCompanyTradAccntOverall
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMDefaultAlerts
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMCompanyAlerts
WHERE        (CompanyID = @companyID)

DELETE FROM T_RMCompanyOverallLimit
WHERE        (CompanyID = @companyID) 

if @@error <> 0
begin
	
	rollback tran
end
else
begin 
	
	commit tran
end



