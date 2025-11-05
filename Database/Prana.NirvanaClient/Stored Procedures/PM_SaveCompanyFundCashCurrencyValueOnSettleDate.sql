
                      
CREATE PROCEDURE [dbo].[PM_SaveCompanyFundCashCurrencyValueOnSettleDate]          
(                                       
@Xml NText,                                                                    
@ErrorMessage varchar(500) output,   
@ErrorNumber int output  
)                
AS   
                                                                         
SET @ErrorMessage = 'Success'                 
SET @ErrorNumber = 0          
                   
BEGIN TRAN TRAN1                
                  
BEGIN TRY                    
DECLARE @handle int                                                                               
                                  
exec sp_xml_preparedocument @handle OUTPUT,@Xml                  
                
CREATE TABLE #XmlCashCurrency                
(                                 
  SettlementDateBaseCurrencyID int,              
  SettlementDateLocalCurrencyID int,              
  SettlementDateCashValueBase float,             
  SettlementDateCashValueLocal float,            
  SettlementDate Datetime,           
  FundID int        
)                                                                          
                                  
INSERT INTO #XmlCashCurrency          
(                                
  SettlementDateBaseCurrencyID ,                 
  SettlementDateLocalCurrencyID ,                 
  SettlementDateCashValueBase ,                   
  SettlementDateCashValueLocal ,                 
  SettlementDate ,                            
  FundID             
)                                                                          
                                  
SELECT              
  SettlementDateBaseCurrencyID ,                  
  SettlementDateLocalCurrencyID ,                 
  Sum(SettlementDateCashValueBase) ,                  
  Sum(SettlementDateCashValueLocal) ,                  
  SettlementDate ,                     
  FundID                              
FROM                                                                          
                                  
OPENXML(@handle, '//SettlementDateCashCurrencyValue', 2)                                                                               
WITH                            
(                             
  SettlementDateBaseCurrencyID Int ,              
  SettlementDateLocalCurrencyID Int ,             
  SettlementDateCashValueBase Float,             
  SettlementDateCashValueLocal Float ,           
  SettlementDate Datetime ,           
  FundID Int                                                                                     
)           
Group By SettlementDateBaseCurrencyID,SettlementDateLocalCurrencyID,SettlementDate,FundID             
       
    Delete PM_CompanyFundCashCurrencyValueOnSettleDate  
 From PM_CompanyFundCashCurrencyValueOnSettleDate SettleDateFund  
 Inner Join #XmlCashCurrency Temp On DateDiff(d,Temp.SettlementDate,SettleDateFund.SettlementDate)=0 And Temp.FundID = SettleDateFund.FundID   
 And Temp.SettlementDateBaseCurrencyID = SettleDateFund.BaseCurrencyID   
 And Temp.SettlementDateLocalCurrencyID =SettleDateFund.LocalCurrencyID            
             
    Insert Into PM_CompanyFundCashCurrencyValueOnSettleDate          
    (              
     SettlementDate,              
     FundID,              
     BaseCurrencyID,              
     CashValueBase,           
     LocalCurrencyID,            
     CashValueLocal             
    )      
SELECT   
	 SettlementDate,              
     FundID,              
     SettlementDateBaseCurrencyID,              
     SettlementDateCashValueBase,              
     SettlementDateLocalCurrencyID,              
     SettlementDateCashValueLocal     
From #XmlCashCurrency   
  
Drop Table #XmlCashCurrency                                                                      
 --drop table #XmlTemp                                
EXEC sp_xml_removedocument @handle                                    
                                   
COMMIT TRANSACTION TRAN1                                                  
                                   
END TRY                             
                                  
 BEGIN CATCH                                           
                                   
  SET @ErrorMessage = ERROR_MESSAGE();                                              
  SET @ErrorNumber = Error_number();   
  EXEC sp_xml_removedocument @handle                                    
  ROLLBACK TRANSACTION TRAN1                                                                               
                                   
 END CATCH;


