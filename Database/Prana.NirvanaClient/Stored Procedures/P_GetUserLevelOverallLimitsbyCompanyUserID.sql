CREATE PROCEDURE dbo.P_GetUserLevelOverallLimitsbyCompanyUserID
	(
		
		@companyID int,
		@companyUserID int
		
	)
AS
	SELECT     T_RMCompanyUsersOverall.RMCompanyUserID, T_RMCompanyUsersOverall.CompanyUserID, T_RMCompanyUsersOverall.UserExposureLimit, 
	                      T_RMCompanyUsersOverall.MaximumPNLLoss, T_RMCompanyUsersOverall.MaximumSizePerOrder, 
	                      T_RMCompanyUsersOverall.MaximumSizePerBasket, T_RMCompanyUsersOverall.CompanyID, T_CompanyUser.ShortName
	FROM         T_RMCompanyUsersOverall INNER JOIN
	                      T_CompanyUser ON T_RMCompanyUsersOverall.CompanyUserID = T_CompanyUser.UserID AND 
	                      T_RMCompanyUsersOverall.CompanyID = T_CompanyUser.CompanyID
	WHERE     (T_RMCompanyUsersOverall.CompanyID = @companyID) AND (T_RMCompanyUsersOverall.CompanyUserID = @companyUserID) 
