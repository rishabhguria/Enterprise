/*  
  
Author: Ankit Yaman Gupta  
Date: April 26, 2013  
Desc: Save Data for Stock Load report for Lazard  
  
*/  
  
  
CREATE Proc [dbo].[PM_SaveStockLoanData]                                                        
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
 [SecurityType] [varchar](100) NULL,            
 [Security] [varchar](100) NULL,            
 [Date] [Varchar](100) NULL,            
 [ContractValue] [varchar](50) NULL,            
 [Quantity] [varchar](50) NULL,            
 [ContractPrice] [varchar](50) NULL,            
 [FedRate] [varchar](50) NULL,            
 [BorrowRate] [varchar](50) NULL,            
 [BorrowInterest] [varchar](50) NULL            
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
COL8,              
COL9,              
COL10,              
COL11             
              
FROM OPENXML(@handle, '//PositionMaster', 2)                
 WITH                                                                                                           
  (                                                                                     
 COL1 varchar(100),              
 COL2 varchar(50),              
 COL3 varchar(100),              
 COL4 varchar(100),              
 COL5 Varchar(100),              
 COL6 varchar(50),              
 COL7 varchar(50),              
 COL8 varchar(50),              
 COL9 varchar(50),              
 COL10 varchar(50),              
 COL11 varchar(50)             
                                                    
  )  Where COL1 not in ('ReportName')          
                    
    

Delete T_StockloanReport      
From T_StockloanReport      
Inner Join #Temp On DateDiff(Day,#Temp.Date,T_StockloanReport.Date)=0 and T_StockloanReport.Account=#Temp.Account      
   

 Insert InTo T_StockloanReport            
Select   
 ReportName,  
 Account,  
 SecurityType,  
 Security,  
 Date,  
 Cast(ContractValue As Float) As ContractValue,  
 Cast(Quantity As Float) As Quantity,  
 Cast(ContractPrice As Float) As ContractPrice,  
 Cast(FedRate As Float) As FedRate,  
 Cast(BorrowRate As Float) As BorrowRate,  
 Cast(BorrowInterest As Float) As BorrowInterest  
From #temp                 
              
Drop table #temp                     
                  
EXEC sp_xml_removedocument @handle                     
              
COMMIT TRANSACTION TRAN1                                                        
                                                   
END TRY                                                        
 BEGIN CATCH                                                         
  SET @ErrorMessage = ERROR_MESSAGE();                                                        
  print @errormessage                                                        
  SET @ErrorNumber = Error_number();                   
  ROLLBACK TRANSACTION TRAN1                                                         
END CATCH; 

