CREATE Procedure [dbo].[PMGetCollateralInterest]    
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
                                              
                          
SET @ErrorMessage = 'Success'                                    
SET @ErrorNumber = 0                             
                          
BEGIN TRY                      
                          
                   
 print @FirstDateofMonth  
print @LastDateofMonth  
  
If(@Type=0)   
BEGIN  
select Date,FundID,BenchmarkName,BenchmarkRate,Spread from PM_CollateralInterest where Datediff(dd,@fromDate,date)=0 
END  
    
                          
END TRY                                    
BEGIN CATCH                            
                                      
 SET @ErrorMessage = ERROR_MESSAGE();                                    
 SET @ErrorNumber = Error_number();        
                                     
END CATCH;
