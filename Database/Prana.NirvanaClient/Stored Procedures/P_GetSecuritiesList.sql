CREATE PROCEDURE [dbo].[P_GetSecuritiesList]
	@companyID int,
	@RestrictedOrAllowed int
AS
	SELECT Symbol,IsTickerSymbology from T_RestrictedAllowedSecuritiesList where CompanyID = @companyID AND RestrictedOrAllowed = @RestrictedOrAllowed
