CREATE PROCEDURE [dbo].[P_SavePTTAllocationMapping]
	@originalAllocationPrefId int,
	@allocationPrefId varchar(MAX)
AS
	BEGIN
		CREATE TABLE #TempPTTAccounts 
		(  
            [AllocationPrefId] int NOT NULL
        )
		INSERT INTO #TempPTTAccounts
			SELECT * FROM Split(@allocationPrefId, ',')
		INSERT INTO T_PTTAllocationMapping
			SELECT @originalAllocationPrefId, [AllocationPrefId] FROM #TempPTTAccounts
	END