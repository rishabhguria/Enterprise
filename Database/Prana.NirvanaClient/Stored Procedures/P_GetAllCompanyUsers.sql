CREATE PROCEDURE [dbo].[P_GetAllCompanyUsers]  
AS  
 SELECT   UserID, LastName, FirstName  
 FROM         T_CompanyUser  