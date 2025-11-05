CREATE PROCEDURE [dbo].[P_GetRevaluationPreference]
AS
BEGIN
	SELECT T_RevaluationPreference.FundID, T_RevaluationPreference.OperationMode 
	FROM T_RevaluationPreference INNER JOIN T_CashPreferences
	ON T_RevaluationPreference.FundID = T_CashPreferences.FundID
END
