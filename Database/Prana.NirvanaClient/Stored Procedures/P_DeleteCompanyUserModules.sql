  
  
  
  
  
/****** Object:  Stored Procedure dbo.P_DeleteCompanyUserModules    Script Date: 11/17/2005 9:50:24 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteCompanyUserModules]  
 (  
  @userID int,  
  @companyUserModuleID varchar(max) = ''  
 )  
AS  
 Declare @userLicCount int  
 set @userLicCount = 0  
 if(@companyUserModuleID = '')   
 begin  
  Delete T_CompanyUserModule  
   Where CompanyUserID = @userID   
 end  
 else  
 begin  
  Create table   
   #TempUserModuleIDListToBeDeleted (companyUserModuleID int)  
  
   insert into   
    #TempUserModuleIDListToBeDeleted  
     (  
      companyUserModuleID  
     )    
     (  
      Select   
       CAST(ITEMS as INT)  
      FROM   
       SPLIT(@companyUserModuleID, ',')  
     )  
   
   
 /* exec ('Delete T_CompanyUserModule  
  Where convert(varchar, CompanyUserModuleID) NOT IN(' + @companyUserModuleID + ') AND CompanyUserID = ' + @userID)  
 */  
 DELETE   
  T_CompanyUserModule   
 WHERE  
  CompanyUserModuleID NOT IN (SELECT companyUserModuleID from #TempUserModuleIDListToBeDeleted)  
  AND   
  CompanyUserID = @userID  
    
    
  declare @companyID int  
  Select @companyID = CU.CompanyID FROM T_CompanyUser CU inner join T_CompanyUserModule CUM on  
       CU.UserID = CUM.CompanyUserID Where  
       CUM.CompanyUserID = @userID  
    
  Declare @containsPMModule int  
  
   set @containsPMModule =  
    ( select   
    count(*)  
   from   
    T_CompanyUserModule CUM    
    INNER JOIN #TempUserModuleIDListToBeDeleted Temp ON CUM.CompanyUserModuleID = temp.companyUserModuleID  
    INNER JOIN T_CompanyModule CM on CUM.CompanyModuleID = CM.CompanyModuleID inner join T_Module M  
    on CM.ModuleID = M.ModuleID  
   Where  
    CM.ModuleID in   
     (  
      Select   
       ModuleID   
      from   
       T_Module   
      WHERE   
       UPPER(ModuleName) = UPPER('Position Management'))  
     )  
  IF( @containsPMModule = 0)  
  begin  
   Select @userLicCount = NofUserLicenses From PM_Company Where NOMSCompanyID = @companyID  
   Set @userLicCount = @userLicCount - 1  
   UPDATE   
    PM_Company  
   SET   
    NofUserLicenses = @userLicCount  
   Where  
    NOMSCompanyID = @companyID   
  end  
    
 end  
   
   
    
   
 --Delete T_CompanyUserModule  
 --Where CompanyUserID = @userID  
  
  
  
  
  