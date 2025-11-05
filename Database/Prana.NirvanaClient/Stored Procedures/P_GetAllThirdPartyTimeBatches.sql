CREATE PROCEDURE [dbo].[P_GetAllThirdPartyTimeBatches]
AS
BEGIN
    SELECT 
        Id,
        ThirdPartyBatchId,
        BatchRunTime,
        IsPaused
    FROM T_ThirdPartyTimeBatches
    INNER JOIN T_ThirdPartyBatch
        ON T_ThirdPartyTimeBatches.FileFormatId = T_ThirdPartyBatch.ThirdPartyFormatId
    WHERE IsPaused = 0
END
