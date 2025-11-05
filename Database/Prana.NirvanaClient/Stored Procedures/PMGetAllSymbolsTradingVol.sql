 /*
exec [PMGetAllSymbolsTradingVol] '08-08-2012', '08-08-2012', 0,'',0 
Edited By: Ankit Gupta
On Oct 16, 2012   
In order to retrieve data for open positions from DB

purpose :- https://jira.nirvanasolutions.com:8443/browse/PRANA-27204
Modified by: sachin Mishra june 7 2018

 */ 
                                      
CREATE PROCEDURE [dbo].[PMGetAllSymbolsTradingVol] (                                          
  @FromDate DateTime,                                    
  @ToDate DateTime,                                    
  @Type int, -- 0 for Same Date,1 for Week , 2 for Month                                          
  @ErrorMessage varchar(500) output,                                          
  @ErrorNumber int output                                          
 )                                          
AS      
--declare @FromDate datetime      
--declare @ToDate datetime      
--declare @Type int      
--declare @ErrorMessage varchar(500)      
--declare @ErrorNumber int      
--Set @FromDate = '08-08-2012'      
--Set @ToDate = '10-10-2012'      
--Set @Type = 0                                          
                                      
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

Select 
BloombergSymbol,
TickerSymbol 
InTo #Temp_V_SecMasterData 
From V_SecMasterData	

				                     
Select Distinct 
AUECID,
Symbol 
InTo #TempGroup from T_Group

--declare @Dates varchar(2000)
--Set @Dates='[05/14/2018]'
--                                           
exec ('select *                                    
from (Select T_Group.Symbol,Date,TradingVolume,T_Group.AUECID,AUEC.ExchangeIdentifier as AUECIdentifier,Sm.BloombergSymbol  
from #TempGroup T_Group left outer join PM_DailyTradingVol on PM_DailyTradingVol.Symbol=T_Group.Symbol  
Left Outer Join T_AUEC AUEC On AUEC.AUECID=T_Group.AUECID  
Left Outer Join #Temp_V_SecMasterData Sm on Sm.TickerSymbol = T_Group.Symbol)  
AS DTV PIVOT (MAX(TradingVolume) FOR Date IN (' + @Dates + ')) AS pvt ; ')             
     
Drop table #TempGroup, #Temp_V_SecMasterData
              
END TRY                                            
BEGIN CATCH    
 SET @ErrorMessage = ERROR_MESSAGE();                       
 SET @ErrorNumber = Error_number();                                             
END CATCH;  
  