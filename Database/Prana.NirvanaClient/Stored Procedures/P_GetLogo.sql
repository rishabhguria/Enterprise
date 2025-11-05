


/****** Object:  Stored Procedure dbo.P_GetLogo    Script Date: 02/10/2006 4:25:21 PM ******/
CREATE PROCEDURE dbo.P_GetLogo
	(
		@logoID int
	)
AS
	SELECT     LogoID, LogoName, LogoImage
	FROM         T_Logo
	Where LogoID = @logoID



