


/****** Object:  Stored Procedure dbo.P_GetAllVendors    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllVendors
AS
	SELECT VendorID, VendorName, ContactLastName, ContactFirstName, ShortName, 
		Title, [Product], Comment, MailingAddress, EMail, TelphoneWork, 
		TelphoneHome, TelphoneMobile, Pager, Fax, Address1, Address2
	FROM         T_Vendor



