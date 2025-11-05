Create PROCEDURE [dbo].[PMGetCompanyDataSourceNames]       
 (      
  @companyID int       
 )    
AS     
Begin        
  SELECT distinct TCF.CompanyThirdPartyID,      
   TTP.ThirdPartyName,      
   TTP.ShortName       
  From T_CompanyFunds as TCF      
  INNER JOIN  T_ThirdParty as TTP on TCF.CompanyThirdPartyID = TTP.ThirdPartyID      
  Where TCF.CompanyID = @companyID and TCF.IsActive=1    
End       
      
/*      
PMGetCompanyDataSourceNames '5'      
select * from T_ThirdParty      
select * from T_CompanyFunds         
*/    