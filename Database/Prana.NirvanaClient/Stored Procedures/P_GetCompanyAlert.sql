

CREATE PROCEDURE dbo.P_GetCompanyAlert
	(
		@companyID int
		
	)
AS


select RMCompanyAlertID,CompanyExposureLower,CompanyExposureUpper,T_RMCompanyAlerts.AlertTypeID,RefreshRateCalculation,
 T_RMCompanyAlerts.RMCompanyOverallLimitID,AlertMessage,EmailAddress,BlockTrading,T_RMCompanyAlerts.CompanyID ,Rank
 from T_RMCompanyAlerts

inner join T_RMAlertType on T_RMAlertType.AlertTypeID = T_RMCompanyAlerts.AlertTypeID
inner join T_Company ON T_Company.CompanyID = T_RMCompanyAlerts.CompanyID 
inner join T_RMCompanyOverallLimit on T_RMCompanyOverallLimit.RMCompanyOverallLimitID = T_RMCompanyAlerts.RMCompanyOverallLimitID

where T_RMCompanyAlerts.CompanyID = @companyID 

