CREATE PROCEDURE dbo.P_GetAllClientOverallLimits
	(
		@companyID int	
	)
AS
	SELECT     T_RMCompanyClientOverall.CompanyClientRMID, T_RMCompanyClientOverall.ClientID, T_RMCompanyClientOverall.ClientExposureLimit, 
	                      T_RMCompanyClientOverall.CompanyID, T_CompanyClient.ClientName
	FROM         T_RMCompanyClientOverall INNER JOIN
	                      T_CompanyClient ON T_RMCompanyClientOverall.ClientID = T_CompanyClient.CompanyClientID AND 
	                      T_RMCompanyClientOverall.CompanyID = T_CompanyClient.CompanyID
	WHERE     (T_RMCompanyClientOverall.CompanyID = @companyID) 

