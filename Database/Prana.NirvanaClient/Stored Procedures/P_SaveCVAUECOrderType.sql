


/****** Object:  Stored Procedure dbo.P_SaveCVAUECSide    Script Date: 12/28/2005 2:15:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECOrderType
	(
		@cvAUECID int,
		@orderTypeID int
	)
AS
		--Insert Data
		INSERT INTO T_CVAUECOrderTypes(CVAUECID, OrderTypesID)
			
		Values(@cvAUECID, @orderTypeID)


