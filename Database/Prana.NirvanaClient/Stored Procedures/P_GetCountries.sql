


/****** Object:  Stored Procedure dbo.P_GetCountries    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetCountries
AS
	Select CountryID, CountryName
	From T_Country
	Order By CountryName Asc



