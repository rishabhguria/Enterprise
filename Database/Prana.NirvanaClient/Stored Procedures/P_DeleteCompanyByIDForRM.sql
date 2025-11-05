CREATE PROCEDURE dbo.P_DeleteCompanyByIDForRM
(
	@companyID int
	--@deleteForceFully int
)
AS	
	--Delete Corresponding Company Details	
	--if ( @deleteForceFully = 1)
		begin 
		Delete T_RMCompanyOverallLimit
			Where CompanyID = @companyID
			
			Delete T_RMCompanyAlerts
			Where CompanyID = @companyID
			
			Delete T_RMCompanyUsersOverall
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
					
					Delete T_RMCompanyUserUI
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
					
					Delete T_RMCompanyClientOverall
			Where (ClientID in (SELECT CompanyClientID 
				FROM T_CompanyClient WHERE 
					CompanyID = @companyID) )
end
--return 1
