 
CREATE procedure [dbo].[P_GetCompanyCAPrefs]                    
(  
@companyID int              
)                    
as                    
             
Select CAPreference from T_CAPreferences where CompanyID = @companyID   


