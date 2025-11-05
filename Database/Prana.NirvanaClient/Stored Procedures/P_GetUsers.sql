
CREATE PROCEDURE [dbo].[P_GetUsers]
AS
	SELECT UserID, LastName, FirstName, ShortName, Title, EMail, TelphoneWork, 
		TelphoneHome, TelphoneMobile, Fax, Login, Password, TelphonePager, Address1, Address2, CountryID,
		StateID, Zip, CompanyID, TradingPermission, City,Region,RoleID,IsAllGroupsAccess
	FROM T_CompanyUser where IsActive=1

