
/****** Object:  Stored Procedure dbo.P_DeleteSetUpContracts    Script Date: 04/18/2007 8:45:23 PM ******/
CREATE PROCEDURE [dbo].[P_DeleteSetUpContracts]
	(		
		@companyID int,
		@companyContractSetUpIDs varchar(200) = ''
	)
AS
	
	if(@companyContractSetUpIDs = '') 
	begin
		Delete T_SetUpContracts
			Where CompanyID = @companyID	
	end
	else
	begin
	
		exec ('Delete T_SetUpContracts
		Where convert(varchar, CompanySetUpContractID) NOT IN(' + @companyContractSetUpIDs + ') AND CompanyID = ' + @companyID)
			
	end
