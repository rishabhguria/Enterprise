

CREATE PROCEDURE dbo.P_GetUserLevelUIbyUserID
	(
		
		@companyID int,
		@companyUserID int
		
	)
AS
	SELECT     T_RMCompanyUserUI.RMCompanyUserUIID, T_RMCompanyUserUI.CompanyID, T_RMCompanyUserUI.CompanyUserID, 
	                      T_RMCompanyUserUI.TicketSize, T_RMCompanyUserUI.PriceDeviation, T_RMCompanyUserUI.AllowUsertoOverwrite, 
	                      T_RMCompanyUserUI.NotifyUserWhenLiveFeedsAreDown, T_CompanyUser.UserID, T_Company.CompanyID AS Expr2, 
	                      T_CompanyUser.ShortName
	FROM         T_RMCompanyUserUI INNER JOIN
	                      T_CompanyUser ON T_CompanyUser.UserID = T_RMCompanyUserUI.CompanyUserID INNER JOIN
	                      T_Company ON T_Company.CompanyID = T_RMCompanyUserUI.CompanyID
	WHERE     (T_RMCompanyUserUI.CompanyID = @companyID) AND (T_RMCompanyUserUI.CompanyUserID = @companyUserID)  
	 


