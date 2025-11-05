

/****** Object:  Stored Procedure dbo.P_SaveSymbolConvention    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE [dbo].[P_SaveSymbolConvention]
(
		@symbolConventionID int,
		@symbolConventionName varchar(100),
		@symbolConventionShortName varchar(20),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_SymbolConvention
Where SymbolConventionID = @symbolConventionID

if(@total > 0)
begin	
	select @count = count(*)
		from T_SymbolConvention
		Where (SymbolConventionName = @symbolConventionName OR SymbolConventionShortName = @symbolConventionShortName) AND SymbolConventionID <> @symbolConventionID
		if(@count = 0)
		begin
			Update T_SymbolConvention 
			Set SymbolConventionName = @symbolConventionName,
			SymbolConventionShortName = @symbolConventionShortName 	
			Where SymbolConventionID = @symbolConventionID
			Set @result = @symbolConventionID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_SymbolConvention 
	Where SymbolConventionName = @symbolConventionName OR SymbolConventionShortName = @symbolConventionShortName
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_SymbolConvention(SymbolConventionName, SymbolConventionShortName)
		Values(@symbolConventionName, @symbolConventionShortName)
			Set @result = scope_identity()
	end
end
select @result

