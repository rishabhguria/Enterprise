
/****** Object:  Stored Procedure dbo.P_GetAllCompanyClients    Script Date: 11/17/2005 9:50:23 AM ******/
create PROCEDURE dbo.P_GetCompanyClientsByCompanyID
(@CompanyID int )
AS
	SELECT     CompanyClientID, ClientName, MailingAddress1, MailingAddress2, CompanyType,Telephone, Fax, 
			PrimaryContactFirstName, PrimaryContactLastName, PrimaryContactTitle, 
			PrimaryContactEMail, PrimaryContactTelephone, PrimaryContactCell, 
			SecondaryContactFirstName, SecondaryContactLastName, SecondaryContactTitle, 
			SecondryContactEmail, SecondaryContactTelephone, SecondaryContactCell, CompanyID
FROM  T_CompanyClient
where CompanyID=@CompanyID



