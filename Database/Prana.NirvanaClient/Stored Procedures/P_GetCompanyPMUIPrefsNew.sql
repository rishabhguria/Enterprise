
Create proc [dbo].[P_GetCompanyPMUIPrefsNew]          
(        
@companyID int        
        
)        
as          
      
select T_PMUIPrefs.NumberOfCustomViewsAllowed,T_PMUIPrefs.NumberOfVisibleColumnsAllowed,T_PMUIPrefs.FetchData
from T_PMUIPrefs where T_PMUIPrefs.CompanyID = @companyID 
