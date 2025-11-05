CREATE PROCEDURE [dbo].[P_GetCompanyFundsForThirdParty]    
(    
 @companyID int,  
 @userID int         
)    
AS      
 SELECT         
 0,     
 0,     
 CFund.CompanyFundID,       
 CFund.FundShortName,     
 CFund.FundTypeID      
 FROM   
T_CompanyUserFunds UFund            
Inner Join T_CompanyFunds CFund on UFund.CompanyFundID = CFund.CompanyFundID  
  
 Where CFund.CompanyID = @companyID and UFund.CompanyUserID = @userID AND CFund.IsActive = 1