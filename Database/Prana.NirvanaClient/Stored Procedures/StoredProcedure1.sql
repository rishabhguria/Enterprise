CREATE PROCEDURE dbo.StoredProcedure1 
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	
declare @result int

 if exists(
	select * from T_Fills 
	where
	T_Fills.OrderID not in (select OrderID from T_EntityOrder)
)
begin
set @result = 1
end
else
begin 
set @result =0
end
select @result
print @result