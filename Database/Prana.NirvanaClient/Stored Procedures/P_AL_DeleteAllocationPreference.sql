
CREATE Procedure [P_AL_DeleteAllocationPreference]
(
	@PreferenceId int
)
AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

--declare @PreferenceId int
--set @PreferenceId = 8


-- Deleting from asset table
DELETE FROM T_AL_Asset 
where CheckListId IN
(
	SELECT CheckListid 
	FROM T_AL_CheckList
	WHERE PresetDefId = @PreferenceId
)


-- Deleting from exchange table
DELETE FROM T_AL_Exchange 
where CheckListId IN
(
	SELECT CheckListid 
	FROM T_AL_CheckList
	WHERE PresetDefId = @PreferenceId
)

-- Deleting from PR table
DELETE FROM T_AL_PR
where CheckListId IN
(
	SELECT CheckListid 
	FROM T_AL_CheckList
	WHERE PresetDefId = @PreferenceId
)

-- Deleting from order side table
DELETE FROM T_AL_OrderSide 
where CheckListId IN
(
	SELECT CheckListid 
	FROM T_AL_CheckList
	WHERE PresetDefId = @PreferenceId
)


DELETE FROM T_AL_StrategyChecklistValues
where AccountCheckListId IN
(
	SELECT Id 
	FROM T_AL_AccountCheckListValue
	where CheckListId IN
	(
		SELECT CheckListId 
		FROM T_AL_CheckList
		WHERE PresetDefId = @PreferenceId
	)
)

-- Deleting from account checklist value table
DELETE FROM T_AL_AccountCheckListValue 
where CheckListId IN
(
	SELECT CheckListid 
	FROM T_AL_CheckList
	WHERE PresetDefId = @PreferenceId
)

-- Deleting from CheckList table
DELETE FROM T_AL_CheckList
WHERE PresetDefId = @PreferenceId

-- Deleting from Strategy table
DELETE FROM T_AL_StrategyValue
where AllocationPrefDataId IN
(
	SELECT Id 
	FROM T_AL_AllocationPreferenceData
	WHERE PresetDefId = @PreferenceId
)

-- Deleting from AllocationPreferenceTable table
DELETE FROM T_AL_AllocationPreferenceData
WHERE PresetDefId = @PreferenceId

-- Deleting from PreferenceDef table
DELETE from T_AL_AllocationPreferenceDef
Where Id = @PreferenceId

COMMIT

END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	

    -- Use RAISERROR inside the CATCH block to return 
    -- error information about the original error that 
    -- caused execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH


