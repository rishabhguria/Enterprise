                                 
CREATE PROCEDURE [dbo].[PMGetAllSymbolsBeta_Old] 
(                                    
  @FromDate DateTime,                              
  @ToDate DateTime,                              
  @Type int, -- 0 for Same Date,1 for Week , 2 for Month                                    
  @ErrorMessage varchar(500) output,                                    
  @ErrorNumber int output                                    
 )                                    
AS                                    
                              
DECLARE @FirstDateofMonth varchar(50)                              
DECLARE @LastDateofMonth varchar(50)                              
                              
If(@Type=0) -- Daily view                    
Begin                              
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                               
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                               
end                              
Else If(@Type=1) -- Weekly view                    
Begin                              
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                               
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)                               
End                              
Else If(@Type=2) -- Monthly view                    
Begin                             
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)                    
 Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                         
End                              
                                    
SET @ErrorMessage = 'Success'                                    
SET @ErrorNumber = 0                                    
                                    
BEGIN TRY                                    
                            
                               
 DECLARE @Dates varchar(2000)                                
 SET @Dates = ''                                
 SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                                
     FROM (select Top 35 AllDates.Items                                   
  from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items desc) MarkDate                                
   SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)                                
                              
 --SELECT @Dates                              
                                   
exec ('select *                            
from (Select PM_Taxlots.Symbol,Date,Beta         
from PM_Taxlots left outer join  PM_DailyBeta on PM_Taxlots.Symbol=PM_DailyBeta.Symbol Union Select T_Group.Symbol,Date,Beta         
from T_Group left outer join  PM_DailyBeta on T_Group.Symbol=PM_DailyBeta.Symbol)          
AS DB PIVOT (MAX(Beta) FOR Date IN (' + @Dates + ')) AS pvt ; ')         
      
                                  
END TRY                                    
BEGIN CATCH                                     
 SET @ErrorMessage = ERROR_MESSAGE();               
 SET @ErrorNumber = Error_number();                                     
END CATCH;       