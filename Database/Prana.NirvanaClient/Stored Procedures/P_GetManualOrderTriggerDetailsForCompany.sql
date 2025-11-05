CREATE PROCEDURE [dbo].[P_GetManualOrderTriggerDetailsForCompany]
(
	@CompanyID INT
)
AS
SELECT AUEC.AUECID AS AUECID
	,CUACT.SendManualOrderTriggerTime AS SendManualOrderTriggerTime
	,CUACT.LastManualOrderRunTriggerTime AS LastManualOrderRunTriggerTime
	,ISNULL(CUACT.IsSendManualOrderViaFIX,0) AS IsSendManualOrderViaFIX
FROM T_CompanyAUEC CA
LEFT OUTER JOIN [T_CompanyAUECClearanceTimeBlotter] CUACT ON CA.CompanyAUECID = CUACT.[CompanyAUECID]
INNER JOIN T_AUEC AUEC ON AUEC.AUECID = CA.AUECID
WHERE CA.CompanyID = @CompanyID