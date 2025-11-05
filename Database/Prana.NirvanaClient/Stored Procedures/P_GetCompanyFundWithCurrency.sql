/*********************************************              
P_GetCompanyFundWithCurrency  1182        
          
Create Date: 2015-04-17          
Created By: Pooja Porwal          
Decsription:  Fund With Base Currency in ascending order in VSR , TSR ,PSR and in other reports          
            
 Added order by fundname.          
*********************************************/              
              
CREATE PROCEDURE [dbo].[P_GetCompanyFundWithCurrency]                  
(                  
  @FundID varchar(max)                      
)              
              
AS 
BEGIN            
            
  Declare @CompanyFundID Table                                                                                      
  (                                                                                      
   FundID int                                                                                  
  )                                     
                                    
  Insert into @CompanyFundID                                    
  Select Cast(Items as int) from dbo.Split(@FundID,',')               
                
  Select             
  CF.CompanyFundID as FundID,               
  CF.FundName as FundName,        
  C.CurrencySymbol As BaseCurrency,        
  CF.FundName+' (' + C.CurrencySymbol + ')' as CompanyFundWithCurrency            
  From T_CompanyFunds CF         
  Inner JOIN T_Currency C on CF.LocalCurrency=C.CurrencyID             
  Where CF.CompanyFundID in (select FundID from @CompanyFundID)            
  Order by CF.FundName       
END   
