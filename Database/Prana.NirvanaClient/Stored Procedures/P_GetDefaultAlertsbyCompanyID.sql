

CREATE PROCEDURE dbo.P_GetDefaultAlertsbyCompanyID
(
	@companyID int	
)
AS
	SELECT RMDefaultID,T_RMDefaultAlerts.AlertTypeID, RefreshRateCalculation,T_RMDefaultAlerts.CompanyID 
	      FROM  T_RMDefaultAlerts
	
	inner join T_Company ON T_Company.CompanyID = T_RMDefaultAlerts.CompanyID
	inner join T_RMAlertType ON T_RMAlertType.AlertTypeID = T_RMDefaultAlerts.AlertTypeID
	
	where T_RMDefaultAlerts.CompanyID=@companyID 


RETURN


