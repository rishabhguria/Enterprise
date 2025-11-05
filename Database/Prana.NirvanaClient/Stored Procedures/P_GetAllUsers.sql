

/****** Object:  Stored Procedure dbo.P_GetAllUsers    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE [dbo].[P_GetAllUsers]
AS
	SELECT   UserID, LastName, FirstName, ShortName, Title, EMail, 
                      TelphoneWork, TelphoneHome, TelphoneMobile, Fax, Login, Password, TelphonePager, 
                      Address1, Address2, CountryID, StateID, Zip, City, SuperUser
FROM         T_User
--Where IsActive  = 1
cr
