CREATE PROCEDURE [dbo].[P_GetBlotterClearanceForCompany]
(
@CompanyID INT
)
AS
SELECT A.AssetName + '\' + U.UnderLyingName + '\' + AUEC.DisplayName + '\' + C.CurrencySymbol AS [AUEC]
	,AUEC.AUECID AS [AUECID]
	,AUEC.RegularTradingStartTime AS [Start Time]
	,AUEC.RegularTradingEndTime AS [End Time]
	,ISNULL(CUACT.ClearanceTime, '') AS [Clearance Time]
	,ISNULL(CUACT.[CompanyAUECClearanceTimeID], 0) AS [Clearance Time ID]
	,CA.CompanyAUECID
	,AUEC.ExchangeIdentifier
	,ISNULL(CUACT.PermitRollover,0) AS PermitRollover
	,ISNULL(CUACT.IsSendManualOrderViaFIX,0) AS [Is Send Manual Order Via FIX]
	,ISNULL(CUACT.SendManualOrderTriggerTime, '') AS [Send Manual Order Trigger Time]
	,ISNULL(CUACT.LastManualOrderRunTriggerTime, '') AS [Last Manual Order Run Trigger Time]
FROM T_CompanyAUEC CA
LEFT OUTER JOIN [T_CompanyAUECClearanceTimeBlotter] CUACT ON CA.CompanyAUECID = CUACT.[CompanyAUECID]
INNER JOIN T_AUEC AUEC ON AUEC.AUECID = CA.AUECID
INNER JOIN T_ASSET A ON AUEC.AssetID = A.AssetID
INNER JOIN T_UNDERLYING U ON AUEC.UnderLyingID = U.UnderLyingID
INNER JOIN T_CURRENCY C ON AUEC.BaseCurrencyID = C.CurrencyID
WHERE CA.CompanyID = @CompanyID