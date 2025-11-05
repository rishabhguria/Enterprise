
/****** Object:  Stored Procedure dbo.P_DeleteSetUpContractByID    Script Date: 04/18/2007 9:00:22 PM ******/
CREATE PROCEDURE [dbo].[P_DeleteSetUpContractByID]
	(
		@companySetUpContractID int	
	)
AS
	
			Delete T_SetUpContracts
			Where CompanySetUpContractID = @companySetUpContractID

