

Create proc [dbo].[P_SaveCompanyPMUIPrefs]  
(  
@companyID int,  
@NumberOfCustomViewsAllowed int,  
@NumberOfVisibleColumnsAllowed int,  
@FetchData bit
  
)  
As  
if((select count(*) from T_PMUIPrefs where CompanyID = @companyID)= 0)  
begin  
  
insert into T_PMUIPrefs  
(  
T_PMUIPrefs.CompanyID,  
T_PMUIPrefs.NumberOfCustomViewsAllowed,  
T_PMUIPrefs.NumberOfVisibleColumnsAllowed,  
T_PMUIPrefs.FetchData
)  
values  
(  
@companyID,  
@NumberOfCustomViewsAllowed ,  
@NumberOfVisibleColumnsAllowed ,  
@FetchData
)  
  
end  
else   
begin  
update T_PMUIPrefs  
set  
T_PMUIPrefs.NumberOfCustomViewsAllowed = @NumberOfCustomViewsAllowed,  
T_PMUIPrefs.NumberOfVisibleColumnsAllowed = @NumberOfVisibleColumnsAllowed,  
T_PMUIPrefs.FetchData = @FetchData
  
end  
  
