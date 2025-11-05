  --PMGetAllDataSourceForComany  
Create Proc [dbo].[PMGetAllDataSourceForComany]    
As    
Select     
 distinct TCF.CompanyFundID,    
 TTP.ThirdPartyID,    
 TTP.thirdPartyName,    
 TTP.ShortName    
From    
 T_thirdParty TTP,    
 T_companyFunds TCF    
Where    
 TTP.thirdPartyID = TCF.CompanythirdPartyID 