


/****** Object:  Stored Procedure dbo.P_GetCompanyClientTraders    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyClientTraders
(
	@companyClientID int
)
AS
	SELECT     TraderID, FirstName, LastName, ShortName, Title, EMail, TelephoneWork, TelephoneCell, 
				Pager, TelephoneHome, Fax, CompanyClientID
	FROM         T_CompanyClientTrader
	WHERE CompanyClientID = @companyClientID



