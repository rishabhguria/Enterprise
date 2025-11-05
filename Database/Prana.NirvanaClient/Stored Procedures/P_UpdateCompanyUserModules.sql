


/****** Object:  Stored Procedure dbo.P_UpdateCompanyUserModules    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_UpdateCompanyUserModules
(
	@companyID int
)
AS
	Delete T_CompanyUserModule
	Where CompanyModuleID Not IN (Select ModuleID
								From T_CompanyModule
								Where CompanyID = @companyID
							  )
		AND CompanyUserID IN (Select UserID
								From T_CompanyUser
								Where CompanyID = @companyID)



