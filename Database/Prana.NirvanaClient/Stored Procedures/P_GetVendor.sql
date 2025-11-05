


/****** Object:  Stored Procedure dbo.P_GetVendor    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetVendor
	(
		@vendorID int
	)
AS
	SELECT   VendorID, VendorName, ContactLastName, ContactFirstName, ShortName, Title, [product], Comment,
			 MailingAddress, eMail,
			 TelphoneWork, TelphoneHome, TelphoneMobile, Pager, Fax, Address1, Address2
	FROM T_Vendor
	Where VendorID = @vendorID



