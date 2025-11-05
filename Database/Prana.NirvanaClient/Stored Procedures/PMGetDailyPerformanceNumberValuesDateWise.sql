CREATE PROCEDURE [dbo].[PMGetDailyPerformanceNumberValuesDateWise] (
	@date DATETIME
	,@errorMessage VARCHAR(500) OUTPUT
	,@errorNumber INT OUTPUT
	)
AS
SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
	SELECT DATE
		,FundID
		,MTDValue
		,QTDValue
		,YTDValue
		,MTDReturn
		,QTDReturn
		,YTDReturn
	FROM PM_DailyPerformanceNumbers
	WHERE Datediff(d, DATE, @date) = 0
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;
