Create Procedure [dbo].[P_SaveDefaultAUECData]                          
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
                          
CREATE TABLE #TempDefaultAUEC
(
CountryID int,
CurrencyID int,
AUECID int
)

INSERT INTO #TempDefaultAUEC
(
CountryID,
CurrencyID,
AUECID           
)                                                                                
SELECT
Country,
Currency,
AUEC
     FROM OPENXML(@handle, '//Table1', 2)
WITH
(
Country int,
Currency int,
AUEC     int           
)

truncate table T_DefaultAUECMapping
INSERT INTO
T_DefaultAUECMapping (CountryID,CurrencyID,AUECID)
SELECT CountryID,CurrencyID,AUECID FROM #TempDefaultAUEC


EXEC sp_xml_removedocument @handle            
            
COMMIT TRANSACTION TRAN1            
            
END TRY            
BEGIN CATCH             
 SET @ErrorMessage = ERROR_MESSAGE();            
print @errormessage            
 SET @ErrorNumber = Error_number();             
 ROLLBACK TRANSACTION TRAN1             
END CATCH;