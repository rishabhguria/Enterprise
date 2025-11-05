
/****** Object:  Stored Procedure dbo.P_GetCompanyUser    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyUser] (
		@userID int
		
	)
AS	
	SELECT   T_CompanyUser.UserID, T_CompanyUser.LastName, T_CompanyUser.FirstName, T_CompanyUser.ShortName, 
			 T_CompanyUser.Title, T_CompanyUser.EMail, T_CompanyUser.TelphoneWork, T_CompanyUser.TelphoneHome, 
			 T_CompanyUser.TelphoneMobile, T_CompanyUser.Fax, T_CompanyUser.[Login], T_CompanyUser.Password, 
			 T_CompanyUser.TelphonePager, T_CompanyUser.Address1, T_CompanyUser.Address2, T_CompanyUser.CountryID,
			 T_CompanyUser.StateID, T_CompanyUser.Zip, T_CompanyUser.CompanyID, T_CompanyUser.TradingPermission, 
			 T_CompanyUser.City,T_Company.[Name], T_CompanyUser.[FactSetUsernameAndSerialNumber], T_CompanyUser.[IsFactSetSupportUser],
			 T_CompanyUser.[MarketDataAccessIPAddresses], T_CompanyUser.ActivUsername, T_CompanyUser.ActivPassword, T_CompanyUser.SamsaraAzureId, T_CompanyUser.HasPowerBIAccess, T_CompanyUser.SapiUsername
FROM         T_CompanyUser, T_Company 
	WHERE T_Company.CompanyID = T_CompanyUser.CompanyID and T_CompanyUser.UserID = @userID
	
