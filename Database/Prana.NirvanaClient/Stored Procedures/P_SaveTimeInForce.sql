
/****** Object:  Stored Procedure dbo.P_SaveTimeInForce    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveTimeInForce
(
		@timeInForceID int,	
		@timeInForce varchar(50),
		@timeInForceTagValue varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_TimeInForce
Where TimeInForceID = @timeInForceID

if(@total > 0)
begin	
	select @count = count(*)
		from T_TimeInForce
		Where (TimeInForce = @timeInForce OR TimeInForceTagValue = @timeInForceTagValue) AND TimeInForceID <> @timeInForceID
		if(@count = 0)
		begin	
			Update T_TimeInForce
			Set TimeInForce = @timeInForce,
			TimeInForceTagValue = @timeInForceTagValue
			Where TimeInForceID = @timeInForceID
			Set @result = @timeInForceID
		end
		else
		begin
			Set @result = -1
		end			
end
else
begin
	select @count = count(*)
	from T_TimeInForce 
	Where (TimeInForce = @timeInForce OR TimeInForceTagValue = @timeInForceTagValue)
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_TimeInForce(TimeInForce, TimeInForceTagValue)
		Values(@timeInForce, @timeInForceTagValue)
		Set @result = scope_identity()
	end
end
select @result
