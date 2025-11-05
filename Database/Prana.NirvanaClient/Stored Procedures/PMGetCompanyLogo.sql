

CREATE PROCEDURE [dbo].[PMGetCompanyLogo] (                            
  @companyID int,                            
  @userID int,                            
  @ErrorMessage varchar(500) output,                            
  @ErrorNumber int output                            
                              
 )                            
AS                            

select Logo from T_CompanyLogo
