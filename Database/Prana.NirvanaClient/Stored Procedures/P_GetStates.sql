


/****** Object:  Stored Procedure dbo.P_GetStates    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetStates
AS
	Select StateID, State, CountryID
	From T_State
	Order By State Asc



