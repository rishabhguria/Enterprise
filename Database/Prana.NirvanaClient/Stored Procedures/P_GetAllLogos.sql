

/****** Object:  Stored Procedure dbo.P_GetAllLogos    Script Date: 02/10/2006 4:40:22 PM ******/
CREATE PROCEDURE dbo.P_GetAllLogos
AS
	Select LogoID, LogoName, LogoImage
	From T_Logo
	Order By LogoName Asc


