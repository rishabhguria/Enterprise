Create Proc [dbo].[PMGetDataSourceByCompanyFundID]  
(  
 @CompanyFundID int  
)  
As  
Select   
 TTP.ThirdPartyID,  
 TTP.ThirdPartyName,  
 TTP.ShortName  
From  
 T_CompanyFunds TCF,  
 T_ThirdParty TTP  
Where  
 TCF.CompanyThirdPartyID = TTP.ThirdPartyID AND  
 TCF.CompanyFundID=@CompanyFundID  