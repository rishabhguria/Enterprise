


/****** Object:  Stored Procedure dbo.P_GetAllUnits    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_GetAllUnits
AS
	Select UnitID, UnitName
	From T_Units Order By UnitName
	


