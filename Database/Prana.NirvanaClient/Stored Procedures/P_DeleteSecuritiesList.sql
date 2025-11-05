CREATE PROCEDURE [dbo].[P_DeleteSecuritiesList]
	@companyID int,
	@RestrictedOrAllowed int
AS
	DELETE from T_RestrictedAllowedSecuritiesList where CompanyID = @companyID AND RestrictedOrAllowed = @RestrictedOrAllowed
