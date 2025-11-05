


/****** Object:  Stored Procedure dbo.P_GetCountryFlag    Script Date: 12/16/2005 5:45:21 PM ******/
CREATE PROCEDURE dbo.P_GetCountryFlag
	(
		@countryFlagID int
	)
AS
	SELECT     CountryFlagID, CountryFlagName, CountryFlagImage
	FROM         T_CountryFlag
	Where CountryFlagID = @countryFlagID



