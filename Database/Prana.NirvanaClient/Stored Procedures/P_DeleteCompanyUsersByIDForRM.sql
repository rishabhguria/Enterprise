

CREATE PROCEDURE dbo.P_DeleteCompanyUsersByIDForRM
	(
		@companyID int,
		@companyUserID int,
		@rMCompanyUserUIID int
	)
AS
	--Delete Corresponding Company Client Details	
	--if ( @deleteForceFully = 1)
		--begin 
		 
		
		DELETE T_RMUserAUECs
		WHERE     RMCompanyUserUIID IN (SELECT     RMCompanyUserUIID
		FROM         T_RMCompanyUserUI
		WHERE     (CompanyID = @companyID) AND (CompanyUserID = @companyUserID))
		   
		Delete T_RMCompanyUsersOverall
			Where CompanyID = @companyID and CompanyUserID = @companyUserID  
			
		Delete T_RMCompanyUserUI
			Where  CompanyID = @companyID and CompanyUserID = @companyUserID 	
			
			
		--end
return 1

