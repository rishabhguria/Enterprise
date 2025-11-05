

CREATE PROCEDURE dbo.P_GetClientOverallLimits
	(
		--@companyClientID int,
		@companyID int
		
	)
AS
	SELECT     CompanyClientRMID, T_RMCompanyClientOverall.ClientID, ClientExposureLimit,
				 T_RMCompanyClientOverall.CompanyID
	FROM         T_RMCompanyClientOverall 
	
	INNER JOIN T_CompanyClient ON T_CompanyClient.CompanyClientID = T_RMCompanyClientOverall.ClientID
	INNER JOIN T_Company ON T_Company.CompanyID = T_RMCompanyClientOverall.CompanyID 



