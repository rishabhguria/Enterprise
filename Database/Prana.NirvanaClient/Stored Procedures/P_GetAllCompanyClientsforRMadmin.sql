CREATE PROCEDURE dbo.P_GetAllCompanyClientsforRMadmin
(
	@companyID int	
)
AS
	SELECT   T_CompanyClient.CompanyClientID, ClientName, MailingAddress1, MailingAddress2,CompanyType, Telephone, Fax, 
			PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle, 
			PrimaryContactEMail, PrimaryContactTelephone, PrimaryContactCell, 
			SecondaryContactFirstName, SecondaryContactLastName, SecondaryContactTitle, 
			SecondryContactEmail, SecondaryContactTelephone, SecondaryContactCell,T_CompanyClient.CompanyID, ShortName, 
			CountryID, StateID, Zip
	FROM  T_CompanyClient
	inner join T_RMCompanyClientOverall on T_RMCompanyClientOverall.ClientID = T_CompanyClient.CompanyClientID
	Where T_CompanyClient.CompanyID = @companyID
	and T_RMCompanyClientOverall.ClientID = T_CompanyClient.CompanyClientID

/*
select ClientID, ClientName
from T_RMCompanyClientOverall,T_CompanyClient
where T_RMCompanyClientOverall.ClientID = T_CompanyClient.CompanyClientID
 and T_RMCompanyClientOverall.CompanyID = @companyID */