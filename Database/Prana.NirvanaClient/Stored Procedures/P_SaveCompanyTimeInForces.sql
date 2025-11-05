CREATE PROCEDURE dbo.P_SaveCompanyTimeInForces (
	@companyID INT
	,@timeInForceID INT
	)
AS
--Insert Data
INSERT INTO T_CompanyTimeInForce (
	CompanyID
	,TimeInForceID
	)
VALUES (
	@companyID
	,@timeInForceID
	)
