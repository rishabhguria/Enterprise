-- =============================================
-- Author:		<Ashish, Poddar>
-- Create date: <15th Jan, 2007>
-- Description:	<This Procedure refers to the view V_GetAUECandUserBlotterClearanceData and can be used for clearly understanding it. >
-- =============================================
CREATE PROCEDURE [dbo].[P_GetAUECandBlotterClearanceData]
	(@date DATETIME)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT dbo.T_AUEC.AUECID
		,dbo.T_CompanyAUEC.CompanyID
		,dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime
		,CASE 
			WHEN datediff(ms, dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date)), @date) > 0
				THEN dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date))
			ELSE
				--IF we have NOT crossed the clearance time then set it as the upper limit. 
				-- Then LOWER limit = UPPER limit - 1(day)
				dateadd(dd, - 1, dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date)))
			END AS lowerClearanceTime
		,CASE 
			WHEN datediff(ms, dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date)), @date) > 0
				THEN dateadd(dd, 1, dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date)))
			ELSE
				--IF we have NOT crossed the clearance time then set it as the upper limit. 
				-- Then LOWER limit = UPPER limit - 1(day)
				dateadd(dd, datediff(dd, ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date), @date), ISNULL(dbo.T_CompanyAUECClearanceTimeBlotter.ClearanceTime, @date))
			END AS UpperClearanceTime
	FROM dbo.T_CompanyAUECClearanceTimeBlotter
	INNER JOIN dbo.T_CompanyAUEC ON dbo.T_CompanyAUECClearanceTimeBlotter.CompanyAUECID = dbo.T_CompanyAUEC.CompanyAUECID
	INNER JOIN dbo.T_AUEC ON dbo.T_CompanyAUEC.AUECID = dbo.T_AUEC.AUECID
END
