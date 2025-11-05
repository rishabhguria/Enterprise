Create Procedure [dbo].[P_SaveShortLocateGridPreferences]                          
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
                          
CREATE TABLE #TempShortLocate
(
BrokerID int,
AccountID int  
)

INSERT INTO #TempShortLocate
(
BrokerID,
AccountID             
)                                                                                
SELECT
BorrowerBroker,
Account     FROM OPENXML(@handle, '//Table1', 2)
WITH
(
BorrowerBroker int,
Account int                
)


INSERT INTO
T_ShortLocateBrokerAccountMapping (BrokerID,AccountID)
SELECT BrokerID,AccountID FROM #TempShortLocate


EXEC sp_xml_removedocument @handle            
            
COMMIT TRANSACTION TRAN1            
            
END TRY            
BEGIN CATCH             
 SET @ErrorMessage = ERROR_MESSAGE();            
print @errormessage            
 SET @ErrorNumber = Error_number();             
 ROLLBACK TRANSACTION TRAN1             
END CATCH;