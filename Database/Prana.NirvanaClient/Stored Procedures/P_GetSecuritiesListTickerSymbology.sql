CREATE PROCEDURE [dbo].[P_GetSecuritiesListTickerSymbology]
	@companyID int,
	@RestrictedOrAllowed int
AS
	SELECT IsTickerSymbology from T_RestrictedAllowedSecuritiesList where CompanyID = @companyID AND RestrictedOrAllowed = @RestrictedOrAllowed
