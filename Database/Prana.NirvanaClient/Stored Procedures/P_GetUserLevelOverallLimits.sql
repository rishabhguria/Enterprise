CREATE PROCEDURE dbo.P_GetUserLevelOverallLimits
	(
		--@companyUserID int,
		@companyID int
		
	)
AS
	SELECT     T_RMCompanyUsersOverall.RMCompanyUserID, T_RMCompanyUsersOverall.CompanyUserID, T_RMCompanyUsersOverall.UserExposureLimit, 
	                      T_RMCompanyUsersOverall.MaximumPNLLoss, T_RMCompanyUsersOverall.MaximumSizePerOrder, 
	                      T_RMCompanyUsersOverall.MaximumSizePerBasket, T_RMCompanyUsersOverall.CompanyID
	FROM         T_RMCompanyUsersOverall INNER JOIN
	                      T_CompanyUser ON T_CompanyUser.UserID = T_RMCompanyUsersOverall.CompanyUserID AND 
	                      T_RMCompanyUsersOverall.CompanyID = T_CompanyUser.CompanyID
	WHERE     (T_RMCompanyUsersOverall.CompanyID = @companyID) 
