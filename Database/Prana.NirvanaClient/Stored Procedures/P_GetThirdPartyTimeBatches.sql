CREATE PROCEDURE [dbo].[P_GetThirdPartyTimeBatches] (@thirdPartyId INT = 0)
AS
BEGIN
    SELECT
        t.Id,
        t.FileFormatId,
        t.BatchRunTime,
        t.IsPaused
    FROM T_ThirdPartyTimeBatches AS t
    INNER JOIN T_ThirdPartyFileFormat AS f ON t.FileFormatId = f.FileFormatId
    WHERE f.ThirdPartyId = @thirdPartyId;
END;