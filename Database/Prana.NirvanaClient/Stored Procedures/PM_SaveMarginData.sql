CREATE Proc [dbo].[PM_SaveMarginData]                                                    
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
 [Date] [varchar](100) NULL,          
 [Account] [varchar](100) NULL,          
 [CUSIP] [varchar](100) NULL,          
 [RIC] [varchar](100) NULL,          
 [Currency] [varchar](100) NULL,          
 [VMargin] [varchar](100) NULL,          
 [Quantity] [varchar](100) NULL,          
 [SettlementPrice] [varchar](100) NULL,          
 [Description] [varchar](150) NULL,          
 [FXRate] [varchar](100) NULL,          
 [Bloomberg] [varchar](100) NULL,          
 [ConvertedVMargin] [varchar](100) NULL,          
 [FundID] [varchar](100) NULL,          
 [FundShortName] [varchar](100) NULL,          
 [AccountShortName] [varchar](100) NULL          
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
COL11,            
COL12,            
COL13,            
COL14,            
COL15            
            
FROM OPENXML(@handle, '//PositionMaster', 2)              
 WITH                                                                                                         
  (                                                                                   
     COL1 [varchar](100),            
 COL2 varchar(100),            
 COL3 varchar(100),            
     COL4 varchar(100),            
 COL5 varchar(100),            
 COL6 [varchar](100),            
     COL7 [varchar](100),            
 COL8 [varchar](100),            
 COL9 varchar(100),            
     COL10 [varchar](100),            
 COL11 varchar(100),            
 COL12 [varchar](100),            
     COL13 varchar(100),            
 COL14 varchar(100),            
 COL15 varchar(100)            
                                                  
  )  Where COL1 not in ('COB DATE')        
        
        
           
Update #temp          
Set Account = '05ABAAVW7'          
Where Currency != 'USD'          
          
;WITH SecMasterCTE(Date, CUSIP, Account, Ranking)                    
AS                    
(                    
SELECT                    
 Date, CUSIP, Account,                   
Ranking = DENSE_RANK() OVER(PARTITION BY Date, CUSIP, Account ORDER BY NEWID() ASC)                    
FROM #temp                    
)                    
Delete FROM SecMasterCTE                    
WHERE Ranking > 1             
          
          
  Select #temp.* into #tempJoin          
  from T_MarginData MD          
  INNER JOIN #temp          
  ON DateDiff(D, #temp.Date,MD.Date) = 0 and MD.Cusip = #temp.CUSIP and MD.Account = #temp.Account          
          
  Delete from T_MarginData          
  Where Cusip in (Select Cusip from #tempJoin)          
  and Date in (Select Date from #tempJoin)          
            
           
  insert into T_MarginData          
  Select * from #temp          
          
drop table #temp, #tempJoin          
            
              
 EXEC sp_xml_removedocument @handle                 
              
COMMIT TRANSACTION TRAN1                                                    
                                               
END TRY                                                    
 BEGIN CATCH                                                     
  SET @ErrorMessage = ERROR_MESSAGE();                                                    
  print @errormessage                                                    
  SET @ErrorNumber = Error_number();               
  ROLLBACK TRANSACTION TRAN1                                                     
END CATCH; 