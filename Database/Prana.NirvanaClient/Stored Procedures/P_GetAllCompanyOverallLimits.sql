CREATE PROCEDURE dbo.P_GetAllCompanyOverallLimits
	/*
	(
	@parameter1 int = 5,
	@parameter2 datatype OUTPUT
	)
	*/
AS
	SELECT     RMCompanyOverallLimitID, CompanyID, RMBaseCurrencyID, CalculateRiskLimit, ExposureLimit, PositivePNL, NegativePNL
	FROM         T_RMCompanyOverallLimit
