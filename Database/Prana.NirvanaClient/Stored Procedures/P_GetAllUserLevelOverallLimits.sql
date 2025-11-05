CREATE PROCEDURE dbo.P_GetAllUserLevelOverallLimits
	(
		@companyID int 
	)
AS
	
	SELECT        RMCompanyUserID, CompanyUserID, UserExposureLimit, MaximumPNLLoss, MaximumSizePerOrder, MaximumSizePerBasket, 
	                         CompanyID
	FROM            T_RMCompanyUsersOverall
	WHERE        (CompanyID = @companyID)

