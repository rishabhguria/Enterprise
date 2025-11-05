

/****************************************************************************                    
Name :   PMGetConversionRateDateWise
Purpose:  Returns all the conversion forex rate for the date range passed.
Module: MarkPriceAndForexConversion/PM
Author: Sandeep Singh
Parameters:                     
  @ErrorMessage varchar(500)                     
  , @ErrorNumber int                      
Execution StateMent:                     
   EXEC [PMGetConversionRateDateWise] '02-02-2008' , '02-02-2008', 0, ' ', 0
                    
Date Modified:                     
Description:                       
Modified By:                       
****************************************************************************/    
  
CREATE Procedure [dbo].[PMGetConversionRateDateWise] (                
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
-- if DATEDIFF(m,CONVERT(VARCHAR(25),GetUTCDate(),101),CONVERT(VARCHAR(25),@fromDate,101)) = 0             
--  begin            
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),GetUTcDate(),101)            
--  end            
-- else            
--  begin            
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                
-- end                
--End   
  
    
                
SET @ErrorMessage = 'Success'                          
SET @ErrorNumber = 0                          
                          
                
BEGIN TRY                  
                
SET @Dates = ''                
SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                
FROM (select Top 35 AllDates.Items                     
from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items Desc) ForexDate                
SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)                
                
--exec ('select *                
--from (Select  Date,TCC.FromCurrencyID,TCC.ToCurrencyID,TCFROM.CurrencySymbol + ''/'' + TCTO.CurrencySymbol  as Symbol, ConversionFactor FROM T_CurrencyConversion TCC                
--Left Outer Join T_Currency TCFROM ON TCFROM.CurrencyID=TCC.FromCurrencyID                
--Left Outer Join T_Currency TCTO ON TCTO.CurrencyID=TCC.ToCurrencyID ) AS DMP                
--PIVOT (MAX(ConversionFactor) FOR Date IN (' + @Dates + ')) AS pvt;')         
      
exec ('select *                
from (Select Date,TCC.FromCurrencyID,TCC.ToCurrencyID,TCC.Symbol,'' '' as Summary,ConversionFactor      
FROM T_CurrencyConversion TCC Where TCC.Symbol is not null ) AS DMP                
PIVOT (MAX(ConversionFactor) FOR Date IN (' + @Dates + ')) AS pvt ; ')              
                
END TRY                          
BEGIN CATCH                           
                            
 SET @ErrorMessage = ERROR_MESSAGE();                          
 SET @ErrorNumber = Error_number();                           
                           
                           
END CATCH;
