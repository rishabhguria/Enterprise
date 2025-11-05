


/****** Object:  Stored Procedure dbo.P_AddVendorDetail    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_AddVendorDetail
	(
		@V_name varchar(50),
		@F_name varchar(50),
		@L_name varchar(50),
		@S_name varchar(50),
		@Product varchar(50),
		@E_mail varchar(50),
		@Tele_work varchar(50),
		@Tele_home varchar(50),
		@Tele_mobile varchar(50),
		@Tele_pager varchar(50),
		@address1 varchar(50),
		@address2 varchar(50),
		@fax varchar(50),
		@title varchar(50),
		@comment varchar(50),
		@mailingaddress varchar(100)
	)
AS 
INSERT T_Vendor (VendorName, ContactFirstName, ContactLastName, ShortName, [Product], Email, TelphoneWork, TelphoneHome,
               TelphoneMobile, Pager, Address1, Address2, Fax, Title, Comment, MailingAddress)
  Values(@V_name,
		@F_name, 
		@L_name, 
		@S_name, 
		@Product, 
		@E_mail,
		@Tele_work,
		@Tele_home,
		@Tele_mobile,
		@Tele_pager,
		@address1,
		@address2,
		@fax,
		@title,
		@comment,
		@mailingaddress)



