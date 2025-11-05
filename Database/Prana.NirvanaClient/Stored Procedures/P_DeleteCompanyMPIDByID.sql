
/****** Object:  Stored Procedure dbo.P_DeleteCompanyMPIDByID    Script Date: 05/12/2006 5:20:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyMPIDByID
(
		@companyMPID int	
)
AS
			Delete T_CompanyCounterPartyVenueDetails
			Where CompanyMPID = @companyMPID
			
			Delete T_CompanyMPID
			Where CompanyMPID = @companyMPID