

/****** Object:  Stored Procedure dbo.PMGetCompanyUser 1     
 Script Date: 11/17/2005 9:50:23 AM ******/    
CREATE PROCEDURE [dbo].[PMGetCompanyUser] (    
    @companyID int    
  , @ErrorMessage varchar(500) output    
  , @ErrorNumber int output    
      
 )    
AS    
    
SET @ErrorMessage = 'Success'    
SET @ErrorNumber = 0    
--BEGIN TRAN TRAN1     
    
BEGIN TRY    
    
    
 SELECT       
    UserID as [UserID]    
  , Login AS [Login]    
  , FirstName + ', ' + LastName   AS [Name]    
  , Password AS [Password]    
 FROM     
  T_CompanyUser as CU    
 INNER JOIN PM_Company AS PMC ON CU.CompanyID = PMC.NOMSCompanyID    
 Where     
  PMC.PMCompanyID = @companyID    
  
    
END TRY    
BEGIN CATCH     
      
 SET @ErrorMessage = ERROR_MESSAGE();    
 SET @ErrorNumber = Error_number();     
     
     
END CATCH;
