


/****** Object:  Stored Procedure dbo.P_SaveAdvancedOrder    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveAdvancedOrder
(
		@advancedOrdersID int,	
		@advancedOrders varchar(50),
		@result	int
)
AS 

if(@advancedOrdersID > 0)
begin	
	Update T_AdvancedOrders
	Set AdvancedOrders = @advancedOrders
		

	Where AdvancedOrdersID = @advancedOrdersID
			
	
	Set @result = @advancedOrdersID
end
else
begin
	declare @total int
	set @total = 0
	select @total = count(*)
	from T_AdvancedOrders 
	Where AdvancedOrdersID = @advancedOrdersID
	
	if(@total > 0)
	begin
		Set @result = -1
	end
	else
	begin
	INSERT T_AdvancedOrders(AdvancedOrders)
	Values(@advancedOrders)
        
        Set @result = scope_identity()
	end
end
select @result


