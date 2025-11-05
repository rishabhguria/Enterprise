


/****** Object:  Stored Procedure dbo.P_GetCountryStates    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCountryStates
	(
		@countryID int
	)
AS
	SELECT     StateID, State, CountryID
	FROM         T_State
	Where CountryID = @countryID



