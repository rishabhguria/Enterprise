CREATE PROCEDURE P_GetThirdPartyFileStatus @runDate DATETIME
AS
BEGIN
    SELECT        
        FTD.ThirdPartyBatchId
    FROM 
        T_ThirdPartyFileStatus FTD
    WHERE 
        CAST(FTD.BatchRunDate AS DATE) = CAST(@runDate AS DATE) 
END