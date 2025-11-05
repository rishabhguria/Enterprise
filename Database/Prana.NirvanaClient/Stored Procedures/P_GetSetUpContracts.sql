/****** Object:  Stored Procedure dbo.P_GetSetUpContracts    Script Date: 04/18/2007 1:08:22 PM ******/
CREATE PROCEDURE [dbo].[P_GetSetUpContracts]
	(
		@companyID	int	
	)
AS
	
	Select Symbol, AuecID, ContractSize, ContractMonthID, CompanyID, CompanySetUpContractID, Description 
	From T_SetUpContracts
	Where CompanyID = @companyID

