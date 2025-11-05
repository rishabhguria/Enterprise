DECLARE @revaluationPreferenceRowCount INT = (SELECT COUNT(*) FROM T_RevaluationPreference)

IF @revaluationPreferenceRowCount = 0
BEGIN
	INSERT INTO T_RevaluationPreference(FundID, OperationMode)
	SELECT T_CompanyFunds.CompanyFundID,1 
	FROM T_CompanyFunds INNER JOIN T_CashPreferences 
	ON T_CompanyFunds.CompanyFundID = T_CashPreferences.FundID
END

DELETE FROM T_RevaluationPreference
WHERE FundID NOT IN (SELECT FundID FROM T_CashPreferences)