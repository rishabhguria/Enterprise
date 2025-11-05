CREATE Procedure [dbo].[PMSaveDailyCreditLimitValues]                        
(
	@Xml nText,
	@IsSavingFromImport bit
)
AS

BEGIN TRAN TRAN1
BEGIN TRY                          
DECLARE @handle INT                             
EXEC sp_xml_preparedocument @handle OUTPUT,@Xml                             
                          
CREATE TABLE #TempDailyCreditLimit
(
	FundID int,
	LongDebitLimit float,
	ShortCreditLimit float,
	LongDebitBalance float,
	ShortCreditBalance float
)

INSERT INTO #TempDailyCreditLimit
(
	FundID,
	LongDebitLimit,
	ShortCreditLimit,
	LongDebitBalance,
	ShortCreditBalance
)
SELECT
FundID,
LongDebitLimit,
ShortCreditLimit,
LongDebitBalance,
ShortCreditBalance
FROM OPENXML(@handle, '//Table1', 2)
WITH
(
	FundID int,
	LongDebitLimit float,
	ShortCreditLimit float,
	LongDebitBalance float,
	ShortCreditBalance float
)

IF @IsSavingFromImport = 0
BEGIN
	DELETE FROM PM_DailyCreditLimit
END

--Update Existing Values
UPDATE DCL
SET DCL.LongDebitLimit = 
	CASE
	WHEN @IsSavingFromImport = 0 THEN #DCL.LongDebitLimit
	ELSE DCL.LongDebitLimit
	END,
DCL.ShortCreditLimit = 
	CASE
	WHEN @IsSavingFromImport = 0 THEN #DCL.ShortCreditLimit
	ELSE DCL.ShortCreditLimit
	END,
DCL.LongDebitBalance = #DCL.LongDebitBalance, DCL.ShortCreditBalance = #DCL.ShortCreditBalance
FROM PM_DailyCreditLimit AS DCL
INNER JOIN #TempDailyCreditLimit AS #DCL
ON DCL.FundID = #DCL.FundID

--Deleting those value which are Updated
DELETE TDCL FROM #TempDailyCreditLimit AS TDCL
INNER JOIN PM_DailyCreditLimit AS DCL
ON DCL.FundID = TDCL.FundID

--Insert remaining values
INSERT INTO PM_DailyCreditLimit (FundID,LongDebitLimit,ShortCreditLimit,LongDebitBalance,ShortCreditBalance)
SELECT #DCL.FundID,#DCL.LongDebitLimit,#DCL.ShortCreditLimit,#DCL.LongDebitBalance,#DCL.ShortCreditBalance FROM #TempDailyCreditLimit AS #DCL

DROP TABLE #TempDailyCreditLimit

EXEC sp_xml_removedocument @handle
COMMIT TRANSACTION TRAN1
END TRY

BEGIN CATCH
ROLLBACK TRANSACTION TRAN1             
END CATCH;

