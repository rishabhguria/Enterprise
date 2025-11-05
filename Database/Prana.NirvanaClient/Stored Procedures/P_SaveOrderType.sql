
/****** Object:  Stored Procedure dbo.P_SaveOrderType    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveOrderType
(
		@orderTypesID int,	
		@orderTypes varchar(50),
		@orderTypesTagValue varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_OrderType
Where OrderTypesID = @orderTypesID

if(@total > 0)
begin	
	select @count = count(*)
		from T_OrderType
		Where (OrderTypes = @orderTypes OR OrderTypeTagValue = @orderTypesTagValue) AND OrderTypesID <> @orderTypesID
		if(@count = 0)
		begin

			Update T_OrderType
			Set OrderTypes = @orderTypes,
				OrderTypeTagValue = @orderTypesTagValue
			Where OrderTypesID = @orderTypesID
			Set @result = @orderTypesID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_OrderType 
	Where (OrderTypes = @orderTypes OR OrderTypeTagValue = @orderTypesTagValue)
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_OrderType(OrderTypes, OrderTypeTagValue)
		Values(@orderTypes, @orderTypesTagValue)
		Set @result = scope_identity()
	end
end
select @result
