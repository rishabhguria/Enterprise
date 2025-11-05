


/****** Object:  Stored Procedure dbo.P_InsertAUECOrderType    Script Date: 12/20/2005 9:00:24 PM ******/
CREATE PROCEDURE dbo.P_InsertAUECOrderType
	(
		@auecID int,
		@OrderTypeID int
		
	)
AS
		--Insert Data
		INSERT INTO T_AUECOrderTypes(AUECID, OrderTypeID)
			
		Values(@auecID, @OrderTypeID)



