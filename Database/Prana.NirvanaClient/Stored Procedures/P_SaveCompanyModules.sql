

/*
[P_SaveCompanyModules]  1, 8, '-2147483648',1

select isactive , nomscompanyid from pm_company
*/

CREATE PROCEDURE [dbo].[P_SaveCompanyModules]
	(
		@companyID int,
		@moduleID int,
		@companyModuleID int,
		@read_writeID int
	)
AS
Declare @result int
Declare @total int 
declare @toBeUpdated int
Set @total = 0

Select @total = Count(*)
From T_CompanyModule
Where CompanyID = @companyID And ModuleID = @moduleID

if(@total > 0)
begin	
	--Update CompanyModule
	Update T_CompanyModule
	Set CompanyID = @companyID, 
		ModuleID = @moduleID,
		Read_writeID = @read_writeID
				
	Where CompanyID = @companyID And ModuleID = @moduleID
	
	Select @result = CompanyModuleID From T_CompanyModule Where CompanyID = @companyID And ModuleID = @moduleID
	
If(@moduleID = (Select ModuleID from T_Module WHERE UPPER(ModuleName) = UPPER('Position Management')))
	begin 
		
		SET @toBeUpdated = (select count(*) from pm_company where NOMSCompanyID = @CompanyID)
		
		if(@toBeUpdated = 0)
		begin
			INSERT INTO 
				PM_Company
				(
					NOMSCompanyID
					, IsActive
				)
				Values
				(
					@companyID
					, 1
				)				
		end
	else
		begin 
		Update 
			Pm_company 
		SET 
			IsActive = 1
		WHERE
			NOMSCompanyID = 1
		end
	End
end
else

begin
	
	Insert T_CompanyModule(companyID, moduleID, Read_writeID)
	Values(@companyID, @moduleID, @read_writeID)	
	Set @result = scope_identity()


	If(@moduleID = (Select ModuleID from T_Module WHERE UPPER(ModuleName) = UPPER('Position Management')))
	begin 
		
		SET @toBeUpdated = (select count(*) from pm_company where NOMSCompanyID = @companyID)
		
		if(@toBeUpdated = 0)
		begin
			INSERT INTO 
				PM_Company
				(
					NOMSCompanyID
					, IsActive
				)
				Values
				(
					@companyID
					, 1
				)				
		end
	else
		begin 
		Update 
			Pm_company 
		SET 
			IsActive = 1
		WHERE
			NOMSCompanyID = @companyID
		end
	End
	
end
select @result






