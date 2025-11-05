/****************************************************************************
Name		:	PMSaveUserDefinedMTDPnLValue
Purpose		:	Save User Defined MTD PnL Values datewise.
Module		:	PM
Author		:	Bharat Kumar Jangir       
****************************************************************************/         
CREATE Procedure [dbo].[PMSaveUserDefinedMTDPnLValue]                          
(
@Xml nText,
@ErrorMessage varchar(500) output,
@ErrorNumber int output                          
)                          
AS                           
                          
SET @ErrorMessage = 'Success'                          
SET @ErrorNumber = 0                          
BEGIN TRAN TRAN1                           
                          
BEGIN TRY                          
DECLARE @handle int                             
exec sp_xml_preparedocument @handle OUTPUT,@Xml                             
                          
CREATE TABLE #TempUserDefinedMTDPnLValue
(
Date datetime,
UserDefinedMTDPnL float,
FundID int
)

INSERT INTO #TempUserDefinedMTDPnLValue
(
Date,
UserDefinedMTDPnL,  
FundID                
)                                                                                
SELECT
Date,
UserDefinedMTDPnL,  
FundID FROM OPENXML(@handle, '//Table1', 2)
WITH
(
Date datetime,
UserDefinedMTDPnL float,  
FundID int                
)

DELETE PM_UserDefinedMTDPnL
WHERE
DATEADD(day, DATEDIFF(day, 0, PM_UserDefinedMTDPnL.Date), 0) in
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempUserDefinedMTDPnLValue.Date, 113)), 0) from  #TempUserDefinedMTDPnLValue)    

INSERT INTO
PM_UserDefinedMTDPnL (UserDefinedMTDPnL,Date,FundID)
SELECT UserDefinedMTDPnL,Date,FundID FROM #TempUserDefinedMTDPnLValue

DROP TABLE #TempUserDefinedMTDPnLValue

EXEC sp_xml_removedocument @handle            
            
COMMIT TRANSACTION TRAN1            
            
END TRY            
BEGIN CATCH             
 SET @ErrorMessage = ERROR_MESSAGE();            
print @errormessage            
 SET @ErrorNumber = Error_number();             
 ROLLBACK TRANSACTION TRAN1             
END CATCH;

