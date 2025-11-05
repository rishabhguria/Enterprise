
/*        
Modified By: Ankit Gupta on 10 Oct, 2014
Description: Get only those funds, users, strategies, third parties from DB, that are currently in Active State.
*/        
CREATE PROCEDURE [dbo].[P_GetFrequentlyUsedData]  
AS    
BEGIN      
  
 SELECT UserID, ShortName from T_CompanyUser where  IsActive =1   
    
 SELECT CurrencyID, CurrencySymbol from T_Currency     
    
 SELECT     CompanyFundID,FundShortName, FundName, CompanyID, LocalCurrency, IsSwapAccount       
 FROM         T_CompanyFunds   where  IsActive =1  
    
 SELECT     CompanyStrategyID, StrategyShortName, StrategyName, CompanyID        
 FROM         T_CompanyStrategy  where  IsActive =1  
    
 select ThirdPartyID, ThirdPartyName ,ShortName    
 from T_ThirdParty  where  IsActive =1  
    
select ThirdPartyID, CompanyFundID     
from T_ThirdParty left JOIN T_CompanyFunds    
on CompanyThirdPartyID=ThirdPartyID where  T_ThirdParty.IsActive =1

  SELECT CounterPartyID,ShortName FROM T_CounterParty   

END     

