

/****** Object:  Stored Procedure dbo.P_GetAllFlags    Script Date: 12/16/2005 5:40:22 PM ******/
CREATE PROCEDURE dbo.P_GetAllFlags
AS
	Select CountryFlagID, CountryFlagName, CountryFlagImage
	From T_CountryFlag
	Order By CountryFlagName Asc


