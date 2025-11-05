/****************************************************************************
Name		:	PMGetStartOfMonthCapitalAccountValuesDateWise
Purpose		:	Returns all the Start of Month Capital Account Values date range wise.
Module		:	PM
Author		:	Bharat Kumar Jangir
****************************************************************************/
CREATE Procedure [dbo].[PMGetStartOfMonthCapitalAccountValuesDateWise]      
(
@fromDate DateTime,
@Type int, -- 0 for Month, 1 for Year
@ErrorMessage varchar(500) output,
@ErrorNumber int output
)
As
DECLARE @Dates varchar(2000)
DECLARE @FirstDateofMonth varchar(50)
DECLARE @LastDateofMonth varchar(50)

--If(@Type=0)
	Begin
	Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)
	Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)
	End
--Else If(@Type=1)                          
--	Begin                          
--	Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)              
--	Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(mm,-(Month(DATEADD(yy,1,@fromDate))),DATEADD(yy,1,@fromDate)),101)
--	END                       
                          
SET @ErrorMessage = 'Success'                                    
SET @ErrorNumber = 0                             
                          
BEGIN TRY                                                
SET @Dates = ''                          
SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                          
FROM (select Top 35 AllDates.Items                               
from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items Desc) ForexDate                          
SET @Dates = LEFT(@Dates, LEN(@Dates) - 1)                          
        
exec ('select *                          
from (Select Date,StartOfMonthCapitalAccount,FundID       
from PM_StartOfMonthCapitalAccount)        
AS DMP PIVOT (MAX(StartOfMonthCapitalAccount) FOR Date IN (' + @Dates + ')) AS pvt ; ')                        
                          
END TRY                                    
BEGIN CATCH                            
                                      
 SET @ErrorMessage = ERROR_MESSAGE();                                    
 SET @ErrorNumber = Error_number();        
                                     
END CATCH;        

