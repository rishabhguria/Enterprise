
CREATE PROCEDURE [dbo].[P_GetUsersForTradingAccount]
	
	(
	
	@tradingAccntID int
	)
	
AS
	SELECT         T_CompanyUserTradingAccounts.CompanyUserID,T_CompanyUser.LastName, T_CompanyUser.FirstName, T_CompanyUser.ShortName,T_CompanyUser.Title,
				  T_CompanyUser.EMail, T_CompanyUser.TelphoneWork,  T_CompanyUser.TelphoneHome, T_CompanyUser.TelphoneMobile, T_CompanyUser.Fax, 
				   T_CompanyUser.Login, T_CompanyUser.Password,T_CompanyUser.TelphonePager, T_CompanyUser.Address1, T_CompanyUser.Address2,
				   T_CompanyUser.CountryID,T_CompanyUser.StateID, T_CompanyUser.Zip,T_CompanyUser.CompanyID, T_CompanyUser.TradingPermission, T_CompanyUser.City,
				   T_CompanyUser.[FactSetUsernameAndSerialNumber], T_CompanyUser.[IsFactSetSupportUser], T_CompanyUser.[MarketDataAccessIPAddresses], 
				   T_CompanyUser.ActivUsername, T_CompanyUser.ActivPassword, T_CompanyUser.SamsaraAzureId, T_CompanyUser.SapiUsername
	FROM            T_CompanyUser INNER JOIN
	                         T_CompanyUserTradingAccounts ON T_CompanyUser.UserID = T_CompanyUserTradingAccounts.CompanyUserID
	WHERE        (T_CompanyUserTradingAccounts.TradingAccountID = @tradingAccntID) 

