
/****** Object:  Stored Procedure dbo.P_SaveModule    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveModule
(
		@moduleID int,
		@moduleName varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Module
Where ModuleID = @moduleID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Module
		Where ModuleName = @moduleName AND ModuleID <> @moduleID
		if(@count = 0)
		begin
			Update T_Module 
			Set ModuleName = @moduleName	
			Where ModuleID = @moduleID
			Set @result = @moduleID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_Module 
	Where ModuleName = @moduleName
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_Module(ModuleName)
		Values(@moduleName)
		Set @result = scope_identity()
	end
end
select @result
