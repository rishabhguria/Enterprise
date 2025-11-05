  
/****** Object:  Stored Procedure dbo.P_DeleteCompanyModules      
[P_DeleteCompanyModules] 1, '1,2,3,4,12'  
  
Script Date: 11/17/2005 9:50:22 AM ******/  
CREATE PROCEDURE [dbo].[P_DeleteCompanyModules]  
 (  
  @companyID int,  
  @companyModulesID varchar(max) = ''  
 )  
AS  
  
  
  
if(@companyModulesID = '')   
begin  
 Delete T_CompanyModule  
  Where CompanyID = @companyID   
end  
else  
begin   
  
Create table   
   #TempModuleIDListToBeDeleted ( companyModulesID int)  
  
insert into   
 #tempModuleIDListToBeDeleted  
  (  
   companyModulesID  
  )    
  (  
   Select   
    CAST(ITEMS as INT)  
   FROM   
    SPLIT(@companyModulesID, ',')  
  )  
  
 DELETE   
  T_CompanyUserModule   
 WHERE  
  CompanyModuleID NOT IN (SELECT companyModulesID from #tempModuleIDListToBeDeleted)  
  AND   
  CompanyUserID IN (Select UserID From T_CompanyUser Where CompanyID = @CompanyID)  
   
 Delete   
  T_CompanyModule  
 Where   
  CompanyModuleID NOT IN (SELECT companyModulesID from #tempModuleIDListToBeDeleted)  
  AND   
  CompanyID =  @companyID  
/*  
exec ( 'Delete T_CompanyUserModule  
     Where CompanyModuleID NOT IN(  cast('+ @companyModulesID + ' as int)) AND CompanyUserID IN (Select UserID From T_CompanyUser Where CompanyID = ' + @companyID + ')' )  
   
 exec ('Delete T_CompanyModule  
 Where CompanyModuleID NOT IN(  cast('+ @companyModulesID + ' as int)) AND CompanyID = ' + @companyID)  
*/   
  
  
Declare @containsPMModule int  
  
 set @containsPMModule =  
  ( select   
  count(*)  
 from   
  T_CompanyModule TCM    
  INNER JOIN #tempModuleIDListToBeDeleted Temp ON TCM.CompanyModuleID = temp.companyModulesID  
 Where  
  TCM.ModuleID in   
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
 UPDATE   
  PM_Company  
 SET   
  IsActive = 0  
 Where  
  NOMSCompanyID = @companyID   
end  
end  
  
  
  
  