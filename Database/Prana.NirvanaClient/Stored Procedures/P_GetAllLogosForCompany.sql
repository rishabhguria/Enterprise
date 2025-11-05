

/****** Object:  Stored Procedure dbo.P_GetAllLogosForCompany    Script Date: 01/23/2008 5:26:22 PM ******/  
CREATE PROCEDURE [dbo].[P_GetAllLogosForCompany] AS  
 Select LogoID, LogoName, Logo  
 From T_CompanyLogo
