CREATE PROCEDURE dbo.P_SaveCompanyExecutionInstructions (
	@companyID INT
	,@executionInstructionsID INT
	)
AS
--Insert Data
INSERT INTO T_CompanyExecutionsInstructions (
	CompanyID
	,ExecutionInstructionsID
	)
VALUES (
	@companyID
	,@executionInstructionsID
	)
