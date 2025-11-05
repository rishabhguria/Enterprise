


/****** Object:  Stored Procedure dbo.P_AddAdminUserDetail    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_AddAdminUserDetail
	(
		@F_name varchar(50),
		@L_name varchar(50),
		@S_name varchar(50),
		@Title varchar(50),
		@E_mail varchar(50),
		@Tele_work varchar(50),
		@Tele_home varchar(50),
		@Tele_mobile varchar(50),
		@Tele_pager varchar(50),
		@login varchar(20),
		@password varchar(20),
		@address1 varchar(50),
		@address2 varchar(50),
		@fax varchar(50)
	)
AS 
INSERT T_User (FirstName, LastName, ShortName, Title, Email, TelphoneWork, TelphoneHome,
               TelphoneMobile, TelphonePager, Login, Password, Address1, Address2, Fax)
  Values(@F_name, 
		@L_name, 
		@S_name, 
		@Title, 
		@E_mail,
		@Tele_work,
		@Tele_home,
		@Tele_mobile,
		@Tele_pager,
		@login,
		@password,
		@address1,
		@address2,
		@fax)



