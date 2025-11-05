/****************************************************************************
Name		:	SavePMCalculationPrefValues
Purpose		:	Save PM Calculation Preference Values.
Module		:	PM
Author		:	Bharat Kumar Jangir       
****************************************************************************/         
CREATE Procedure [dbo].[SavePMCalculationPrefValues]                          
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
                          
CREATE TABLE #TempPMCalculationPrefs
(
CompanyID int,
FundID int,
HighWaterMark float,
Stopout float,
TraderPayoutPercent float
)

INSERT INTO #TempPMCalculationPrefs
(
CompanyID,
FundID,
HighWaterMark,
Stopout,  
TraderPayoutPercent                
)                                                                                
SELECT
CompanyID,
FundID,
HighWaterMark,
Stopout,  
TraderPayoutPercent FROM OPENXML(@handle, '//Table1', 2)
WITH
(
CompanyID int,
FundID int,
HighWaterMark float,
Stopout float,  
TraderPayoutPercent float                
)

DELETE T_PMCalculationPrefs

INSERT INTO
T_PMCalculationPrefs (CompanyID,FundID,HighWaterMark,Stopout,TraderPayoutPercent)
SELECT CompanyID,FundID,HighWaterMark,Stopout,TraderPayoutPercent FROM #TempPMCalculationPrefs

DROP TABLE #TempPMCalculationPrefs

EXEC sp_xml_removedocument @handle            
            
COMMIT TRANSACTION TRAN1            
            
END TRY            
BEGIN CATCH             
 SET @ErrorMessage = ERROR_MESSAGE();            
print @errormessage            
 SET @ErrorNumber = Error_number();             
 ROLLBACK TRANSACTION TRAN1             
END CATCH;

