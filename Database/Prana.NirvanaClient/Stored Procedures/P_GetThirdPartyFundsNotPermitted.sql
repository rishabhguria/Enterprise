      
      
      
CREATE PROCEDURE [dbo].[P_GetThirdPartyFundsNotPermitted]      
       
 (      
 @thirdPartyID int       
 )      
       
AS      
 SELECT         
 T_ThirdPartyPermittedFunds.ThirdPartyFundID_PK,     
 T_ThirdPartyPermittedFunds.ThirdPartyID,     
 T_ThirdPartyPermittedFunds.CopanyFundID,       
 T_CompanyFunds.FundShortName,     
 T_CompanyFunds.FundTypeID      
    FROM             
  T_ThirdPartyPermittedFunds INNER JOIN      
  T_CompanyFunds     
  ON T_ThirdPartyPermittedFunds.CopanyFundID = T_CompanyFunds.CompanyFundID     
        Inner Join T_CompanyThirdParty CTP     
  on CTP.CompanyThirdPartyID= T_ThirdPartyPermittedFunds.ThirdPartyID    
  WHERE  T_ThirdPartyPermittedFunds.ThirdPartyID <> @thirdPartyID    
    