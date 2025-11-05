

CREATE PROCEDURE dbo.P_GetCompanyAlerts
(
		--@rMCompanyOverallLimitID int,
		@companyID int
		
	)
AS
	SELECT     RMCompanyAlertID , CompanyExposureLower , CompanyExposureUpper ,  
	           T_RMCompanyAlerts.AlertTypeID ,  RefreshRateCalculation , T_RMCompanyAlerts.RMCompanyOverallLimitID , 
	           AlertMessage , EmailAddress , BlockTrading , T_RMCompanyAlerts.CompanyID,Rank 
	          
	FROM  T_RMCompanyAlerts
	
	inner join T_Company ON T_Company.CompanyID = T_RMCompanyAlerts.CompanyID
	inner join T_RMCompanyOverallLimit ON T_RMCompanyOverallLimit.RMCompanyOverallLimitID = T_RMCompanyAlerts.RMCompanyOverallLimitID
	inner join T_RMAlertType ON T_RMAlertType.AlertTypeID = T_RMCompanyAlerts.AlertTypeID
	

