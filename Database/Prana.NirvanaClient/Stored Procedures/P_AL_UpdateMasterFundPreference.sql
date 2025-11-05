CREATE PROCEDURE [dbo].[P_AL_UpdateMasterFundPreference]
(
	@MasterFundPrefXML nText,
	@CalculatedPrefsXML xml
)
AS

BEGIN TRY

DECLARE @intErrorCode INT
DECLARE @NewMasterFundPrefId INT
DECLARE @NewMFId INT
DECLARE @handle INT
DECLARE @CalPrefsXML NVARCHAR(MAX)
DECLARE @CalPrefId INT

exec sp_xml_preparedocument @handle OUTPUT,@MasterFundPrefXML 

BEGIN TRAN

-------------------------------------------------------------------------------
----------------- Creating #TempMasterFundPreferenceDef table -----------------
-------------------------------------------------------------------------------
CREATE TABLE #TempMasterFundPreferenceDef
(
	[MasterFundPrefId]					INT				NOT NULL, 
	[MasterFundPrefName]				NVARCHAR(50)	NOT NULL,
	[CompanyId]							INT				NOT NULL,	
	[UpdateDateTime]					DATETIME		NOT NULL,
	[AllocationBase]					INT				NOT NULL,
	[MatchingRule]						INT				NOT NULL,
	[MatchPortfolioPosition]			INT				NOT NULL,
	[PreferencedFundId]					INT				NULL,
	[ProrataDaysBack]					INT				NULL,
)

-------------------------------------------------------------------------------
----------------- Creating #Temp_MasterFund_Target_Percentage table -------------------
-------------------------------------------------------------------------------
CREATE TABLE #Temp_MasterFund_Target_Percentage
(	
	[MasterFundId] INT NOT NULL,	
	[Value] DECIMAL(32,19) NOT NULL
)

-------------------------------------------------------------------------------
----------------- Creating #Temp_MasterFund_Preferences table -------------------
-------------------------------------------------------------------------------
CREATE TABLE #Temp_MasterFund_Preferences
(	
	[MasterFundId] INT NOT NULL,	
	[CalculatedPrefId] INT NOT NULL
)

-------------------------------------------------------------------------------
----------------- Creating ##TempMasterFundCalculatedPreferences table -----------------
-------------------------------------------------------------------------------
CREATE TABLE #TempMasterFundCalculatedPreferences
(
	CalculatedPrefsXML XML,
	CalculatedPrefId INT
)


-------------------------------------------------------------------------------
----------------- Creating #CalculatedPrefIDs table ---------------------------
-------------------------------------------------------------------------------
CREATE TABLE #CalculatedPrefIDs 
(
	CalculatedPrefId INT
)  

DECLARE @calculatedPrefId INT
-------------------------------------------------------------------------------
----------------- Inserting data in #TempMasterFundPreferenceDef table -----------
-------------------------------------------------------------------------------
insert into #TempMasterFundPreferenceDef
	(
		[MasterFundPrefId], [MasterFundPrefName], [CompanyId], [UpdateDateTime], [AllocationBase], [MatchingRule], [MatchPortfolioPosition], [PreferencedFundId], [ProrataDaysBack]  
	)																			 				
	SELECT 																		 
		[MasterFundPrefId], [MasterFundPrefName], [CompanyId], [UpdateDateTime], 
		(
			SELECT	id 
			FROM	T_AL_AllocationBase AS B 
			WHERE	B.AllocationBase =xm.AllocationBase
		) AS AllocationBase,		
		(
			SELECT	id 
			FROM	T_AL_MatchingRule AS B 
			WHERE	B.MatchingRule =xm.MatchingRule
		) AS MatchingRule,
		(
			SELECT id 
			FROM T_AL_MatchPortfolioPosition AS B
			WHERE B.MatchPortfolioPosition=xm.MatchPortfolioPosition
		) AS MatchPortfolioPosition, 
		[PreferencedFundId], [ProrataDaysBack]   		
FROM  OPENXML(@handle, '/AllocationMasterFundPreference',2)                                                                                                                                
WITH
(	
	[MasterFundPrefId]			INT				'MasterFundPreferenceId',
	[MasterFundPrefName]		NVARCHAR(50)	'MasterFundPreferenceName',
	[CompanyId]					INT				'CompanyId',
	[UpdateDateTime]			DATETIME		'UpdateDateTime',
	[AllocationBase]			NVARCHAR(50)	'/AllocationMasterFundPreference/DefaultRule/BaseType',
	[MatchingRule]				NVARCHAR(50)	'/AllocationMasterFundPreference/DefaultRule/RuleType',
	[MatchPortfolioPosition]	NVARCHAR(50)	'/AllocationMasterFundPreference/DefaultRule/MatchClosingTransaction',
	[PreferencedFundId]			NVARCHAR(50)	'/AllocationMasterFundPreference/DefaultRule/PreferenceAccountId',
	[ProrataDaysBack]			INT				'/AllocationMasterFundPreference/DefaultRule/ProrataDaysBack'
) AS xm

-------------------------------------------------------------------------------
----------------- Inserting data in #Temp_MasterFund_Target_Percentage table -----------
-------------------------------------------------------------------------------
INSERT INTO #Temp_MasterFund_Target_Percentage
	(
		[MasterFundId], [Value]
	)
SELECT 
		[MasterFundId], [Value]
FROM  OPENXML(@handle, '/AllocationMasterFundPreference/MasterFundTargetPercentage/KeyValuePair',2)                                                                                                                                  
WITH
(	
	[MasterFundId] INT 'Key',
	[Value] DECIMAL(32,19)'Value'
) AS xm1 WHERE [Value]>0

-------------------------------------------------------------------------------
----------------- Inserting data in #Temp_MasterFund_Preferences table -----------
-------------------------------------------------------------------------------
INSERT INTO #Temp_MasterFund_Preferences
	(
		[MasterFundId], [CalculatedPrefId]
	)
SELECT 
		[MasterFundId], [CalculatedPrefId]
FROM  OPENXML(@handle, '/AllocationMasterFundPreference/MasterFundPreference/KeyValuePair',2)                                                                                                                                  
WITH
(	
	[MasterFundId] INT 'Key',
	[CalculatedPrefId] INT 'Value'
) AS xm2

-------------------------------------------------------------------------------
----------------- Inserting data in ##TempMasterFundCalculatedPreferences table -----------
-------------------------------------------------------------------------------
insert into #TempMasterFundCalculatedPreferences
	(
		[CalculatedPrefsXML],
		[CalculatedPrefId]
	)
SELECT
      T.C.query('.') AS calclatedPref,
	  T.C.value('(./OperationPreferenceId)[1]','int') AS CalculatedPrefId
FROM 
@CalculatedPrefsXML.nodes('/List/System.Collections.IEnumerable/AllocationOperationPreference') AS T(C)

------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------

SELECT TOP 1 @NewMasterFundPrefId = MasterFundPrefId FROM #TempMasterFundPreferenceDef

IF EXISTS(SELECT 1 FROM T_AL_MFWisePrefValues WHERE [MFPreferenceId] = @NewMasterFundPrefId)
BEGIN 
	-- Getting the calculated pref Id's associated with Master fund Id which are not referenced in updated master fund pref
	INSERT INTO #CalculatedPrefIDs   
	SELECT	MFCP.CalculatedPrefId 
	FROM	T_AL_MFWisePrefValues MFCP
	LEFT JOIN #TempMasterFundCalculatedPreferences TMFCP ON MFCP.CalculatedPrefId = TMFCP.CalculatedPrefId
	WHERE [MFPreferenceId] = @NewMasterFundPrefId AND TMFCP.CalculatedPrefId IS NULL

	-- Calling the SP 'P_AL_DeleteAllocationPreference' in Loop for all the calculated pref Id's
	WHILE EXISTS (SELECT 1 FROM #CalculatedPrefIDs)
	BEGIN
		SELECT TOP 1 @calculatedPrefId = [CalculatedPrefId] FROM #CalculatedPrefIDs
		EXEC P_AL_DeleteAllocationPreference @calculatedPrefId
		DELETE FROM #CalculatedPrefIDs WHERE CalculatedPrefId = @calculatedPrefId
	END

	DELETE FROM T_AL_MFWisePrefValues WHERE [MFPreferenceId] = @NewMasterFundPrefId
END

------------------------------------------------------------------------------------------------------------
----------------- Inserting data in T_AL_MFWisePrefValues and T_AL_MFAllocationPreference tables -----------
------------------------------------------------------------------------------------------------------------
WHILE (SELECT COUNT(*) FROM #TempMasterFundPreferenceDef) > 0
Begin

	UPDATE T_AL_MFAllocationPreference SET [MFPreferenceName] = TMP.[MasterFundPrefName], [CompanyId] = TMP.[CompanyId], [UpdateDateTime]= TMP.[UpdateDateTime], [AllocationBase] = TMP.[AllocationBase], [MatchingRule] = TMP.[MatchingRule], [MatchPortfolioPosition] = TMP.[MatchPortfolioPosition], [PreferencedFundId] = TMP.[PreferencedFundId], [ProrataDaysBack] = TMP.[ProrataDaysBack]
	FROM  #TempMasterFundPreferenceDef TMP WHERE MFPreferenceId = @NewMasterFundPrefId
		
		DECLARE @MatchingRule INT
		SET @MatchingRule = (SELECT MatchingRule FROM #TempMasterFundPreferenceDef WHERE MasterFundPrefId = @NewMasterFundPrefId)
		IF(@MatchingRule != 4 AND @MatchingRule != 5 AND @MatchingRule != 6)
		BEGIN
			WHILE (SELECT COUNT(*) FROM #Temp_MasterFund_Target_Percentage) > 0
			BEGIN
				SELECT TOP 1 @NewMFId = MasterFundId FROM #Temp_MasterFund_Target_Percentage

				INSERT INTO T_AL_MFWisePrefValues
				(
						[MFPreferenceId], [MFId], [Value], [CalculatedPrefId]
				)
				SELECT 
					@NewMasterFundPrefId, @NewMFId, TMPValues.[Value], TMPCAL.[CalculatedPrefId]
				FROM #Temp_MasterFund_Target_Percentage TMPValues JOIN #Temp_MasterFund_Preferences TMPCAL ON TMPCAL.MasterFundId = TMPValues.MasterFundId
				WHERE TMPValues.[MasterFundId] = @NewMFId

				DELETE FROM #Temp_MasterFund_Target_Percentage WHERE [MasterFundId] = @NewMFId
			END
		END
		ELSE
		BEGIN
				INSERT INTO T_AL_MFWisePrefValues
				(
						[MFPreferenceId], [MFId], [Value], [CalculatedPrefId]
				)
				SELECT 
					@NewMasterFundPrefId, [MasterFundId], 0.0, [CalculatedPrefId]
				FROM #Temp_MasterFund_Preferences 
		END

		DELETE FROM #TempMasterFundPreferenceDef WHERE MasterFundPrefId = @NewMasterFundPrefId
END

-------------------------------------------------------------------------------
---------- Inserting into temp #T_AL_MasterFundProrataList table from XML -----------------
-------------------------------------------------------------------------------

delete from T_AL_MasterFundProrataList where [MFPreferenceId] = @NewMasterFundPrefId

insert into T_AL_MasterFundProrataList	( [MFPreferenceId],[MasterFundId] )
SELECT @NewMasterFundPrefId,[MasterFundId]
FROM  OPENXML(@handle, '/AllocationMasterFundPreference/DefaultRule/ProrataAccountList/Int32',2)
WITH
(		
	[MasterFundId] int '.'	
) as xm

------------------------------------------------------------------------------------------------------------
----------------- Inserting data in Calculated Prefs tables -----------
------------------------------------------------------------------------------------------------------------
WHILE (SELECT COUNT(*) FROM #TempMasterFundCalculatedPreferences) > 0
BEGIN
		SELECT TOP 1 @CalPrefsXML = Convert(NVARCHAR(MAX),TMP_CAL_PREF.CalculatedPrefsXML), @CalPrefId = TMP_CAL_PREF.CalculatedPrefId FROM #TempMasterFundCalculatedPreferences TMP_CAL_PREF
			EXEC P_AL_UpdateAllocationPreference @CalPrefsXML
		DELETE FROM #TempMasterFundCalculatedPreferences WHERE CalculatedPrefId = @CalPrefId
END

DROP TABLE #TempMasterFundPreferenceDef
DROP TABLE #Temp_MasterFund_Target_Percentage
DROP TABLE #Temp_MasterFund_Preferences
DROP TABLE #TempMasterFundCalculatedPreferences
EXEC P_AL_GetMasterFundPrefById @NewMasterFundPrefId

COMMIT 
EXEC sp_xml_removedocument @handle
END TRY

BEGIN CATCH
	ROLLBACK
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	
   RAISERROR (
			  @ErrorMessage, -- Message text.
              @ErrorSeverity, -- Severity.
              @ErrorState -- State.
              );
END CATCH