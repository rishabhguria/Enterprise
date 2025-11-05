CREATE PROCEDURE [dbo].[P_SetLastManualOrderTriggerTime]
	@AUECID INT,
	@LastManualOrderRunTriggerTime DATETIME,
	@CompanyId INT
AS
	UPDATE [T_CompanyAUECClearanceTimeBlotter]
		SET LastManualOrderRunTriggerTime = @LastManualOrderRunTriggerTime
		WHERE CompanyAUECClearanceTimeID IN	(SELECT CUACT.CompanyAUECClearanceTimeID FROM T_CompanyAUEC CA
	LEFT OUTER JOIN [T_CompanyAUECClearanceTimeBlotter] CUACT ON CA.CompanyAUECID = CUACT.[CompanyAUECID]
	INNER JOIN T_AUEC AUEC ON AUEC.AUECID = CA.AUECID
	WHERE AUEC.AUECID = @AUECID)

RETURN 0
