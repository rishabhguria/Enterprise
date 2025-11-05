

/****** Object:  Stored Procedure dbo.P_GetUser    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE [dbo].[P_GetUser]
	(
		@userID int
	)
AS
	SELECT     UserID, LastName, FirstName, ShortName, Title, EMail, TelphoneWork, 
			TelphoneHome, TelphoneMobile, Fax, Login, Password, TelphonePager, Address1, Address2, CountryID, 
			StateID, Zip, City, SuperUser
	FROM         T_User
	Where UserID = @userID


