/****** Object:  Stored Procedure dbo.P_GetCompanyUsersBoth    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyUsersBoth]
	(
		@companyID int,
		@userID int
	)
AS
	SELECT UserID, LastName, FirstName, ShortName, Title, EMail, TelphoneWork, 
		TelphoneHome, TelphoneMobile, Fax, Login, Password, TelphonePager, Address1, Address2, CountryID,
		StateID, Zip, CompanyID, TradingPermission, City, [FactSetUsernameAndSerialNumber], [IsFactSetSupportUser],
		[MarketDataAccessIPAddresses], [ActivUsername], [ActivPassword], [SamsaraAzureId], [SapiUsername]
	FROM T_CompanyUser
	Where CompanyID = @companyID and UserID = @userID	




