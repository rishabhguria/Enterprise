      
      
/****************************************************************************                            
Name :   [PMGetNavValuesDateWise]        
Purpose:  Returns all the NAV Values date range wise.        
Module: NAV/PM        
Author: Sandeep Singh        
Parameters:                             
  @ErrorMessage varchar(500)                             
  , @ErrorNumber int                              
Execution StateMent:                             
   EXEC [PMGetNavValuesDateWise] '02-18-2008' , '02-18-2008', 2, ' ', 0        
                            
Date Modified:                             
Description:                               
Modified By:                               
****************************************************************************/            
          
CREATE Procedure [dbo].[PMGetNavValuesDateWise]    
(                        
@fromDate DateTime,                        
@ToDate DateTime,                        
@Type int, -- 0 for Same Date,1 for Week , 2 for Month                        
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
Else If(@Type=1)                        
Begin                        
Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                         
Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)                         
End                        
Else If(@Type=2)                        
Begin                        
Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)            
Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)                       
END                     
                        
SET @ErrorMessage = 'Success'                                  
SET @ErrorNumber = 0                           
                        
BEGIN TRY                    
                        
SET @Dates = ''                        
SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                        
FROM (select Top 35 AllDates.Items                             
from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items Desc) ForexDate                        
SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)                        
      
exec ('select *                        
from (Select Date,NAVValue,FundID     
from PM_NAVValue)      
AS DMP PIVOT (MAX(NAVValue) FOR Date IN (' + @Dates + ')) AS pvt ; ')                      
                        
END TRY                                  
BEGIN CATCH                          
                                    
 SET @ErrorMessage = ERROR_MESSAGE();                                  
 SET @ErrorNumber = Error_number();      
                                   
END CATCH;      
    