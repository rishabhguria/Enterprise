-- Author	: Gaurav
-- Date		: 07 dec 12
-- Description: Picks up the max id from T_journal. This id is further used to generate the new distinct ids greater than this.
CREATE PROCEDURE [dbo].[P_GetMaxGeneratedNumber]
AS    
SELECT MAX(MaxNumber)
FROM (
	SELECT Max(cast(isnull(round(transactionEntryID, 0), 0) AS BIGINT)) AS MaxNumber
	FROM T_Journal
	
	UNION
	
	SELECT Max(cast(isnull(round(ActivityID, 0), 0) AS BIGINT)) AS MaxNumber
	FROM T_AllActivity

	UNION
	
	SELECT Max(cast(isnull(round(transactionEntryID, 0), 0) AS BIGINT)) AS MaxNumber
	FROM T_SymbolLevelAccrualsJournal
) AS subQuery
