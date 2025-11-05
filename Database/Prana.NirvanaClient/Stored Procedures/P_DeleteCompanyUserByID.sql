
/****** Object:  Stored Procedure dbo.P_DeleteCompanyUserByID    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyUserByID (
	@companyUserID INT
	,@deleteForceFully INT
	)
AS
--T_RMUserTradingAccount  
DECLARE @total INT,@Login VARCHAR(50)

SELECT @Login = login
FROM T_CompanyUser
WHERE UserID = @companyUserID

SELECT @total = Count(1)
FROM T_RMCompanyUsersOverall
WHERE CompanyUserID = @companyUserID

IF (@deleteForceFully = 1)
BEGIN
	IF (@total = 0)
	BEGIN
		SELECT @total = Count(1)
		FROM T_RMCompanyUserUI
		WHERE CompanyUserID = @companyUserID

		IF (@total = 0)
		BEGIN
			SELECT @total = Count(1)
			FROM T_RMUserTradingAccount
			WHERE CompanyUserID = @companyUserID

			IF (@total = 0)
			BEGIN
				--Delete Corresponding Company Client Details   
				-- If Company User is referenced anywhere and still we want to delete it.  
				--Delete Company User and related information.  
				-------------------------- Start : Delete Company User Permissions --------------------------  
				-- Delete Company User CounterpartyVenues.  
				DELETE T_CompanyUserCounterPartyVenues
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User Application Component \ Module.  
				DELETE T_CompanyUserModule
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User AUEC's.  
				DELETE T_CompanyUserAUEC
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User Trading Account.  
				DELETE T_CompanyUserTradingAccounts
				WHERE CompanyUserID = @companyUserID

				--Delete from T_AuditTrailOtherPermissions
				DELETE
				FROM T_AuditTrailOtherPermissions
				WHERE UserId = @companyUserID

				--Delete Fund permissions
				Delete from T_CompanyUserfunds where CompanyUserID = @companyUserID

				--Delete Strategies permissions
				Delete from T_CompanyUserStrategies where CompanyUserID = @companyUserID

				--Delete Live Feed Data type permissions
				Delete from T_CompanyUserMarketDataTypes where companyuserid = @companyUserID 
				
				DELETE T_UserTTAssetPreferences where UserID = @companyUserID 

				-------------------------- End : Delete Company User Permissions --------------------------  
				DELETE T_CompanyUser
				WHERE UserID = @companyUserID
				
				SELECT @Login
			END
		END
	END
END
ELSE
BEGIN
	IF (@total = 0)
	BEGIN
		SELECT @total = Count(1)
		FROM T_RMCompanyUserUI
		WHERE CompanyUserID = @companyUserID

		IF (@total = 0)
		BEGIN
			SELECT @total = Count(1)
			FROM T_RMUserTradingAccount
			WHERE CompanyUserID = @companyUserID

			IF (@total = 0)
			BEGIN
				--Delete Corresponding Company Client Details   
				-- If Company User is referenced anywhere and still we want to delete it.  
				--Delete Company User and related information.  
				-------------------------- Start : Delete Company User Permissions --------------------------  
				-- Delete Company User CounterpartyVenues.  
				DELETE T_CompanyUserCounterPartyVenues
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User Application Component \ Module.  
				DELETE T_CompanyUserModule
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User AUEC's.  
				DELETE T_CompanyUserAUEC
				WHERE CompanyUserID = @companyUserID

				-- Delete Company User Trading Account.  
				DELETE T_CompanyUserTradingAccounts
				WHERE CompanyUserID = @companyUserID

				--Delete from T_AuditTrailOtherPermissions
				DELETE
				FROM T_AuditTrailOtherPermissions
				WHERE UserId = @companyUserID

				--Delete from T_CompanyUserFundGroupMapping
				DELETE
				FROM T_CompanyUserFundGroupMapping
				WHERE CompanyUserID = @companyUserID


				--Delete Fund permissions
				Delete from T_CompanyUserfunds where CompanyUserID = @companyUserID

				--Delete Strategies permissions
				Delete from T_CompanyUserStrategies where CompanyUserID = @companyUserID

				--Delete Live Feed Data type permissions
				Delete from T_CompanyUserMarketDataTypes where companyuserid = @companyUserID 

				DELETE T_UserTTAssetPreferences where UserID = @companyUserID 

				-------------------------- End : Delete Company User Permissions --------------------------  
				UPDATE T_CompanyUser
				SET IsActive = 0
				WHERE UserID = @companyUserID

				SELECT @Login
			END
		END
	END
END
