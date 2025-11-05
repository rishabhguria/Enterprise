


/****** Object:  Stored Procedure dbo.P_DeleteCompanyMPIDs    Script Date: 02/01/2006 12:25:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyMPIDs
	(
		@companyID int,
		@companyMPID varchar(200) = ''
	)
AS
	if(@companyMPID = '') 
	begin
		Delete T_CompanyMPID
			Where CompanyID = @companyID	
	end
	else
	begin
	
		exec ('Delete T_CompanyMPID
		Where convert(varchar, CompanyMPID) NOT IN(' + @companyMPID + ') AND CompanyID = ' + @companyID)
			
					
		--exec ( 'Delete T_CompanyUserModule
		--	    Where convert(varchar, CompanyModuleID) NOT IN( ' + @companyModulesID + ')' )
			
	end



