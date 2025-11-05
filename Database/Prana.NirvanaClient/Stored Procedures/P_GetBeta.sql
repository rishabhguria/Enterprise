CREATE PROCEDURE [dbo].[P_GetBeta] (        
@DateForRiskPositions    DATETIME) 
AS
BEGIN
DECLARE @Local_DateForRiskPositions DATETIME

SET @Local_DateForRiskPositions = @DateForRiskPositions


Select Symbol,Beta From PM_DailyBeta where Date = @Local_DateForRiskPositions

END
