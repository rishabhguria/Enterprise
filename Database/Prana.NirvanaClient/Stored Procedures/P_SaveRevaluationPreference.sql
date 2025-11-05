CREATE PROCEDURE [dbo].[P_SaveRevaluationPreference]
	@xmlRevaluationPreferences NTEXT,
	@fromUI INT
	-- @fromUI = 0 for Revaluation Preference
	-- @fromUI = 1 for Account Setup
	-- @fromUI = 2 for Cash Preference
	
AS
DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@xmlRevaluationPreferences

BEGIN TRY
	
	BEGIN TRANSACTION

	CREATE TABLE #TempRevaluationPreference
	(
		[FundID] INT NOT NULL,
		[OperationMode] INT NOT NULL
	)

	CREATE TABLE #TempRevaluationPreferenceFundUI
	(
		[FundID] INT NOT NULL,
		[ModifiedType] INT NOT NULL
	)

	CREATE TABLE #TempRevaluationCashPref
	(
		[AccountID] INT NOT NULL
	)

	IF @fromUI = 1
	BEGIN
		
		INSERT INTO #TempRevaluationPreferenceFundUI (
			FundID,
			ModifiedType
		)
		SELECT FundID, ModifiedType	
		FROM openXML(@handle, 'dsFund/dtFund', 2) WITH (
			[FundID] INT,
			[ModifiedType] INT
		)

		DELETE FROM T_RevaluationPreference
		WHERE FundID IN (SELECT FundID FROM #TempRevaluationPreferenceFundUI WHERE ModifiedType = 2)

		
	END

	ELSE IF @fromUI = 2
	BEGIN
		
		INSERT INTO #TempRevaluationCashPref (
			AccountID
		)
		SELECT AccountID	
		FROM openXML(@handle, 'DsPref/DtPref', 2) WITH (
			[AccountID] INT
		)

		INSERT INTO T_RevaluationPreference (
			FundID,
			OperationMode
		)
		SELECT AccountID,1
			
		FROM #TempRevaluationCashPref
		WHERE AccountID NOT IN (SELECT FundID FROM T_RevaluationPreference)

		
	END

	ELSE
	BEGIN

		INSERT INTO #TempRevaluationPreference (
			FundID,
			OperationMode
		)
		SELECT FundID,
			OperationMode
		FROM openXML(@handle, 'dsRevaluationPref/dtRevaluationPref', 2) WITH (
			[FundID] INT ,
			[OperationMode] INT
		)

		UPDATE T_RevaluationPreference
		SET OperationMode = TRP.OperationMode
		FROM T_RevaluationPreference RP
		INNER JOIN #TempRevaluationPreference TRP ON RP.FundID = TRP.FundID

	END

	DROP TABLE #TempRevaluationPreference
	DROP TABLE #TempRevaluationPreferenceFundUI

	COMMIT
END TRY

BEGIN CATCH
	ROLLBACK
END CATCH

