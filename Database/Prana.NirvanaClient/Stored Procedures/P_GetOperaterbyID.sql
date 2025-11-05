

/****** Object:  Stored Procedure dbo.P_GetOperaterbyID  Script Date: 2/21/2006 2:35:21 PM ******/
CREATE PROCEDURE dbo.P_GetOperaterbyID
(
	@operaterID  int
)
AS
	Select OperatorID, Name
	From T_Operator
	Where  OperatorID = @operaterID 
	


