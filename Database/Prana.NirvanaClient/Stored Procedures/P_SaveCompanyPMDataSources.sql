  
CREATE PROCEDURE [dbo].[P_SaveCompanyPMDataSources]  
 (  
  @companyID int,  
  @thirdPartyID int  
 )  
AS  
Declare @result int  
Declare @total int   
Set @total = 0  
  
Declare @PMCompanyID int   
Set @PMCompanyID = 0  
  
Select @PMCompanyID = PMCompanyID  
From PM_Company  
Where NOMSCompanyID = @companyID  
  
if(@PMCompanyID > 0)  
begin  
 Insert PM_CompanyDataSources(PMCompanyID, ThirdpartyID)  
 Values(@PMCompanyID, @thirdPartyID)  
end  
  
/* if(@total > 0)  
begin   
 --Update PM_CompanyDataSources  
 Update PM_CompanyDataSources  
 Set PMCompanyID = @PMCompanyID,   
  ThirdPartyID = @ThirdPartyID  
      
 Where PMCompanyID = @PMCompanyID And ThirdPartyID = @ThirdPartyID  
   
 --Select @result = CompanyModuleID From PM_CompanyDataSources Where CompanyID = @companyID And ThirdPartyID = @ThirdPartyID  
end  
else  
--Insert PM_CompanyDataSources  
begin  
  
 Insert PM_CompanyDataSources(PMCompanyID, ThirdPartyID)  
 Values(@PMCompanyID, @ThirdPartyID)  
   
 --Set @result = scope_identity()  
end  
--select @result  
*/  
  
  