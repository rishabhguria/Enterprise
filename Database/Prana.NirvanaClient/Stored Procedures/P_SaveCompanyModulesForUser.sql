




/****** Object:  Stored Procedure dbo.P_SaveCompanyModulesForUser    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE [dbo].[P_SaveCompanyModulesForUser]
	(
		@companyUserID int,
		@companyModuleID int,
		@readWriteID int,
		@isShowExport bit
	)
AS
	Declare @userLicCount int
	Declare @result int
	Declare @total int 
	Set @total = 0
	
	Declare @companyID int 
	Set @companyID = 0
	Declare @PM int 
	Set @PM = 0
	Select @PM = M.ModuleID from T_Module M inner join T_CompanyModule CM on M.ModuleID = CM.ModuleID   		
						Where CM.CompanyModuleID = @companyModuleID AND 
						UPPER(ModuleName) = UPPER('Position Management')
						
	if(@PM > 0)
	begin
		Select @companyID = CU.CompanyID FROM T_CompanyUser CU inner join T_CompanyUserModule CUM on
							CU.UserID = CUM.CompanyUserID Where
							CUM.CompanyUserID = @companyUserID
	end

	Select @total = Count(*)
	From T_CompanyUserModule
	Where CompanyUserID = @companyUserID AND CompanyModuleID = @companyModuleID

	if(@total > 0)
	begin	
		--Update T_CompanyUserModule
		Update T_CompanyUserModule 
		Set CompanyUserID = @companyUserID, 
			CompanyModuleID = @companyModuleID,
			Read_WriteID = @readWriteID,
			IsShowExport = @isShowExport
			
		Where CompanyUserID = @companyUserID AND CompanyModuleID = @companyModuleID
		
		Select @result = CompanyUserModuleID From T_CompanyUserModule Where CompanyUserID = @companyUserID AND CompanyModuleID = @companyModuleID
	end
	else
		--Insert T_CompanyUserModule
		begin
			Insert T_CompanyUserModule(CompanyModuleID, CompanyUserID, Read_WriteID,IsShowExport)
			Values(@companyModuleID, @companyUserID, @readWriteID,@isShowExport)
			Set @result = scope_identity()
			
			if(@PM > 0)
			begin
				Select @userLicCount = NofUserLicenses From PM_Company Where NOMSCompanyID = @companyID
				Set @userLicCount = @userLicCount + 1
				Update PM_Company Set NofUserLicenses = @userLicCount Where NOMSCompanyID = @companyID
			end
	end
	select @result
	




