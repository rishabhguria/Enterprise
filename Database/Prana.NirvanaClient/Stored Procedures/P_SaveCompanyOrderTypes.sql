CREATE PROCEDURE dbo.P_SaveCompanyOrderTypes (
	@companyID INT
	,@orderTypeID INT
	)
AS
--Insert Data
INSERT INTO T_CompanyOrderTypes (
	CompanyID
	,OrderTypeID
	)
VALUES (
	@companyID
	,@orderTypeID
	)
