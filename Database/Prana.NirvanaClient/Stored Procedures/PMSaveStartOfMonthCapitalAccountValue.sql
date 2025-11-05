/****************************************************************************
Name		:	PMSaveStartOfMonthCapitalAccountValue
Purpose		:	Save Start of Month Capital Account Values datewise.
Module		:	PM
Author		:	Bharat Kumar Jangir       
****************************************************************************/         
CREATE Procedure [dbo].[PMSaveStartOfMonthCapitalAccountValue]                          
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
                          
CREATE TABLE #TempStartOfMonthCapitalAccountValue
(
Date datetime,
StartOfMonthCapitalAccount float,
FundID int
)

INSERT INTO #TempStartOfMonthCapitalAccountValue
(
Date,
StartOfMonthCapitalAccount,  
FundID                
)                                                                                
SELECT
Date,
StartOfMonthCapitalAccount,  
FundID FROM OPENXML(@handle, '//Table1', 2)
WITH
(
Date datetime,
StartOfMonthCapitalAccount float,  
FundID int                
)

DELETE PM_StartOfMonthCapitalAccount
WHERE
DATEADD(day, DATEDIFF(day, 0, PM_StartOfMonthCapitalAccount.Date), 0) in
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempStartOfMonthCapitalAccountValue.Date, 113)), 0) from  #TempStartOfMonthCapitalAccountValue)    

INSERT INTO
PM_StartOfMonthCapitalAccount (StartOfMonthCapitalAccount,Date,FundID)
SELECT StartOfMonthCapitalAccount,Date,FundID FROM #TempStartOfMonthCapitalAccountValue

DROP TABLE #TempStartOfMonthCapitalAccountValue

EXEC sp_xml_removedocument @handle            
            
COMMIT TRANSACTION TRAN1            
            
END TRY            
BEGIN CATCH             
 SET @ErrorMessage = ERROR_MESSAGE();            
print @errormessage            
 SET @ErrorNumber = Error_number();             
 ROLLBACK TRANSACTION TRAN1             
END CATCH;

