


/****** Object:  Stored Procedure dbo.P_DeleteOrderType    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteOrderType
	(
		@orderTypeID int
	)
AS
--Delete Corresponding Side from the tables referring it.
	Declare @total int

		Select @total = Count(1) 
		FROM T_AUECOrderTypes	Where OrderTypeID = @orderTypeID
	
			if ( @total = 0)
			begin
			
					Select @total = Count(1) 
					FROM T_CVAUECOrderTypes	Where OrderTypesID = @orderTypeID
	
					if ( @total = 0)
					begin 		
						-- If SideID is not referenced anywhere.
						--Delete SideID.
				
						Delete T_OrderType
						Where OrderTypesID = @orderTypeID
					end
				end


