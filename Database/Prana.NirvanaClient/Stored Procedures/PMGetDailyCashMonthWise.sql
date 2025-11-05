 
      
/****************************************************************************                            
Name :   [PMGetDailyCashMonthWise]        
Purpose:  Returns all the DailyCash Values Month wise.        
Module: DailyCash/PM        
Author: Ishant Kathuria
Parameters:                             
  @ErrorMessage varchar(500)                             
  , @ErrorNumber int 
@fromDate DateTime,                        
                    
@Type int, -- 0 for Same Date, 2 for Month                        
   
                           
Execution StateMent:                             
   EXEC [PMGetDailyCashMonthWise] '02-18-2008' , '02-18-2008', 2, ' ', 0        
                            
Date Modified:                             
Description:                               
Modified By:                               
****************************************************************************/            
          
Create Procedure [dbo].[PMGetDailyCashMonthWise]    
(                          
@fromDate DateTime,                          
                      
@Type int, -- 0 for Same Date, 2 for Month                          
@ErrorMessage varchar(500) output,                                    
@ErrorNumber int output                             
)                          
As                          
DECLARE @Dates varchar(2000)                          
DECLARE @FirstDateofMonth varchar(50)                          
DECLARE @LastDateofMonth varchar(50)                          
                       
If(@Type=0)                          
Begin                          
Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                           
Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                           
end                          
                         
Else If(@Type=2)                          
Begin                          
Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)              
Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)                         
END                       
                          
SET @ErrorMessage = 'Success'                                    
SET @ErrorNumber = 0                             
                          
BEGIN TRY                      
                          
                   
 print @FirstDateofMonth  
print @LastDateofMonth  
  
If(@Type=0)   
BEGIN  
select Date,CashCurrencyID,FundID,BaseCurrencyID,CashValueBase,LocalCurrencyID,CashValueLocal from PM_CompanyFundCashCurrencyValue where Datediff(dd,@fromDate,date)=0 
END  
    
Else If(@Type=2)  
BEGIN  
select CashCurrencyID,Date,FundID,BaseCurrencyID,CashValueBase,LocalCurrencyID,CashValueLocal  from PM_CompanyFundCashCurrencyValue where Datediff(dd,@FirstDateofMonth,date)>=0 AND  Datediff(dd,@LastDateofMonth,date)<=0
END  
                          
END TRY                                    
BEGIN CATCH                            
                                      
 SET @ErrorMessage = ERROR_MESSAGE();                                    
 SET @ErrorNumber = Error_number();        
                                     
END CATCH;        
   
