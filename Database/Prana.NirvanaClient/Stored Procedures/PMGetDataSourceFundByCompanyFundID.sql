Create Proc PMGetDataSourceFundByCompanyFundID  
(  
 @CompanyFundID int  
)  
As  
Select   
 CompanyFundID,  
 FundshortName  
From  
 T_CompanyFunds  
Where  
 CompanyFundID=@CompanyFundID