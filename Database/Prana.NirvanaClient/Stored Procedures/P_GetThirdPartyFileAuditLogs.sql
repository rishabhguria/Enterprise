CREATE PROCEDURE [dbo].[P_GetThirdPartyFileAuditLogs]
	@runDate DATETIME  
AS
BEGIN
     SELECT 
     FORMAT(DateTime, 'M/d/yyyy') AS Date,
     FORMAT(DateTime, 'hh:mm:ss tt') AS Time,
	 Type,
	 Action,
	 Details
From T_ThirdPartyFileAuditLogs
    WHERE CAST(DateTime AS DATE) = CAST(@runDate AS DATE)
END