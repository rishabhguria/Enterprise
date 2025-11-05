


/****** Object:  Stored Procedure dbo.P_DeleteCompanyUsers    Script Date: 01/24/2006 12:30:20 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyUsers
	(
		@companyID int
	)
AS

		Delete T_CompanyUserCounterPartyVenues
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
		
		Delete T_CompanyUserModule
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
								 
		Delete T_CompanyUserAUEC
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
		
		Delete T_CompanyUserTradingAccounts
			Where (CompanyUserID in (SELECT UserID 
				FROM T_CompanyUser WHERE 
					CompanyID = @companyID) )
		
					
		Delete T_CompanyUser
		Where CompanyID = @companyID	
	
		
	

