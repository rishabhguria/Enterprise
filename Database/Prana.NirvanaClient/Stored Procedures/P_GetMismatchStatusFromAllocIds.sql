CREATE PROCEDURE P_GetMismatchStatusFromAllocIds @entityIds VARCHAR(MAX)
	,@dateParam DATE
	,@isL2Data BIT
AS
BEGIN
	-- Declare a table variable to store the allocIds and entityIds  
	DECLARE @IndividualAllocIds TABLE (individualAllocId NVARCHAR(50))
	DECLARE @EntityId TABLE (entityId NVARCHAR(50))

	-- Split the comma-separated entityIds and insert into the table variable  
	INSERT INTO @EntityId (entityId)
	SELECT ITEMS
	FROM dbo.Split(@entityIds, ',');

	IF (@isL2Data = 1)
	BEGIN
		INSERT INTO @IndividualAllocIds (individualAllocId)
		SELECT Level1AllocationID
		FROM T_Level2Allocation
		JOIN @EntityId E ON T_Level2Allocation.TaxLotID = E.entityId;
	END
	ELSE
	BEGIN
		INSERT INTO @IndividualAllocIds (individualAllocId)
		SELECT Level1AllocationID
		FROM T_Level2Allocation
		JOIN @EntityId E ON T_Level2Allocation.Level1AllocationID = E.entityId;
	END

	DECLARE @TaxlotsToCheck TABLE (
		IndividualAllocId NVARCHAR(100)
		,MatchStatus VARCHAR(3)
		);

	INSERT INTO @TaxlotsToCheck (
		IndividualAllocId
		,MatchStatus
		)
	SELECT DISTINCT AM.IndividualAllocID
		,AM.MatchStatus
	FROM T_ThirdPartyAllocationMessages AM
	JOIN @IndividualAllocIds AID ON AM.IndividualAllocID = AID.individualAllocId;

	DECLARE @IndividualAllocIdsCount INT;
	DECLARE @TaxlotsToCheckCount INT;

	SELECT @IndividualAllocIdsCount = COUNT(1)
	FROM @IndividualAllocIds;

	SELECT @TaxlotsToCheckCount = COUNT(1)
	FROM @TaxlotsToCheck;

	DECLARE @MismatchCount INT;

	SELECT @MismatchCount = COUNT(1)
	FROM @TaxlotsToCheck
	WHERE MatchStatus NOT IN (
			'6'
			,'20'
			,'21'
			)

	IF (
			@MismatchCount = 0
			AND @IndividualAllocIdsCount = @TaxlotsToCheckCount
			)
	BEGIN
		SELECT 0 AS Result;
	END
	ELSE
	BEGIN
		SELECT 1 AS Result;
	END
END