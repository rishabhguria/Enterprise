CREATE PROCEDURE dbo.P_GetUserLevelUI
	(
		--@companyUserID int,
		@companyID int
		
	)
AS
	SELECT     T_RMCompanyUserUI.RMCompanyUserUIID, T_RMCompanyUserUI.CompanyID, T_RMCompanyUserUI.CompanyUserID, 
	                      T_RMCompanyUserUI.TicketSize, T_RMCompanyUserUI.PriceDeviation, T_RMCompanyUserUI.AllowUsertoOverwrite, 
	                      T_RMCompanyUserUI.NotifyUserWhenLiveFeedsAreDown, T_RMCompanyUserUI.CompanyUserAUECID, T_CompanyUser.ShortName
	FROM         T_RMCompanyUserUI INNER JOIN
	                      T_CompanyUser ON T_CompanyUser.UserID = T_RMCompanyUserUI.CompanyUserID AND 
	                      T_RMCompanyUserUI.CompanyID = T_CompanyUser.CompanyID
	WHERE     (T_RMCompanyUserUI.CompanyID = @companyID)
	