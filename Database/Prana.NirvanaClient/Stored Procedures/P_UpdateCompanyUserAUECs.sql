


/****** Object:  Stored Procedure dbo.P_UpdateCompanyUserAUECs    Script Date: 01/05/2006 3:24:24 PM ******/
CREATE PROCEDURE dbo.P_UpdateCompanyUserAUECs
(
	@companyID int
)
AS
	Delete T_CompanyUserAUEC
	Where CompanyAUECID Not IN (Select CompanyAUECID
								From T_CompanyAUEC
								Where CompanyID = @companyID
							  )
		AND CompanyUserID IN (Select UserID
								From T_CompanyUser
								Where CompanyID = @companyID)

	

