/*  
  
Author: Ankit Yaman Gupta  
Date: April 26, 2013  
Desc: Save Data for Stock Load report for Lazard  
  
*/  
  
  
CREATE Proc [dbo].[PM_SaveInterestData]                                                        
(                   
 @Xml varchar(Max)                  
 , @ErrorMessage varchar(500) output                                                        
 , @ErrorNumber int output                     
)                  
AS                  
SET @ErrorMessage = 'Success'                                                        
SET @ErrorNumber = 0                    
                  
BEGIN TRAN TRAN1                                                         
                                                 
BEGIN TRY               
                
 DECLARE @handle int                                                         
 exec sp_xml_preparedocument @handle OUTPUT,@Xml                 
              
CREATE TABLE [dbo].[#temp]              
(              
 [ReportName] [varchar](100) NULL,              
 [Account] [varchar](50) NULL,              
 [InterestMode] [varchar](100) NULL,                     
 [Date] [varchar](50) NULL,              
 [LongShort] [varchar](20) NULL,              
 [Balance] [varchar](50) NULL,              
 [InterestRate] [varchar](50) NULL,              
 [DailyInterest] [varchar](50) NULL             
)              
                 
INSERT INTO #temp                
Select                 
COL1,                
COL2,                
COL3,                
COL4,    
COL5,      
COL6,                
COL7,                
COL8             
                
FROM OPENXML(@handle, '//PositionMaster', 2)                  
 WITH                                                                                                             
  (                                                                                       
 COL1 varchar(100),                
 COL2 varchar(50),                
 COL3 varchar(100),                
 COL4 varchar(50),                
 COL5 varchar(20),                
 COL6 varchar(50),                
 COL7 varchar(50),                
 COL8 varchar(50)              
                                                      
  )  Where COL1 not in ('ReportName')            
                      
   
Delete T_InterestReport        
From T_InterestReport        
Inner Join #Temp On DateDiff(Day,#Temp.Date,T_InterestReport.Date)=0 and T_InterestReport.Account=#Temp.Account        
   
      
insert into T_InterestReport             
Select   
 ReportName,  
 Account,  
 InterestMode,  
 Date,  
 LongShort,  
 cast(Balance as Float) as Balance,  
 cast(InterestRate as Float) as InterestRate,  
 cast(DailyInterest as Float) as DailyInterest  
from #temp              
              
drop table #temp              
                
                  
 EXEC sp_xml_removedocument @handle                     
                  
COMMIT TRANSACTION TRAN1                                                        
                                                   
END TRY                                                        
 BEGIN CATCH                                                         
  SET @ErrorMessage = ERROR_MESSAGE();                                                        
  print @errormessage                                                        
  SET @ErrorNumber = Error_number();                   
  ROLLBACK TRANSACTION TRAN1                                                         
END CATCH; 

