CREATE PROCEDURE dbo.P_SaveCompanyHandlingInstructions (
	@companyID INT
	,@handlingInstructionsID INT
	)
AS
--Insert Data
INSERT INTO T_CompanyHandlingInstructions (
	CompanyID
	,HandlingInstructionsID
	)
VALUES (
	@companyID
	,@handlingInstructionsID
	)
