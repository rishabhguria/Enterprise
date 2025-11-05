
/****** Object:  Stored Procedure dbo.P_GetCompanyClient    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyClient
(
		@companyID int
)
AS
	SELECT     CompanyClientID, ClientName, MailingAddress1, MailingAddress2, Telephone, Fax, 
			PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle, 
			PrimaryContactEMail, PrimaryContactTelephone, PrimaryContactCell, 
			SecondaryContactFirstName, SecondaryContactLastName, SecondaryContactTitle, 
			SecondryContactEmail, SecondaryContactTelephone, SecondaryContactCell, CompanyID, ShortName, 
			CountryID, StateID, Zip
	FROM  T_CompanyClient
Where CompanyID = @companyID

